using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Globalization;
using System.Web.Configuration;
using System.Text;
using System.Security.Cryptography;

namespace PostgresCustomProvider
{
    public sealed class PostgresMembershipProvider : MembershipProvider
    {
        #region "Variables privadas de la Clase"
        // Global generated password length, generic exception message, event log info.
        private int newPasswordLength = 8;
        private string eventSource = "PostgresCustomProvider";
        private string eventLog = "Kpiboard";
        private string exceptionMessage = "Ha ocurrido un error importanto. Por favor, revise el registro de sucesos.";

        private string connectionString;
        // System.Web.Security.MembershipProvider properties.
        private string pApplicationName;
        private int pApplicationId;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;

        private MembershipPasswordFormat pPasswordFormat;
        // Used when determining encryption key values.

        private MachineKeySection machineKey;
        private int pMinRequiredNonAlphanumericCharacters;
        private int pMinRequiredPasswordLength;

        private string pPasswordStrengthRegularExpression;
        // If False, exceptions are thrown to the caller. If True,
        // exceptions are written to the event log.

        private bool pWriteExceptionsToEventLog;
        #endregion

        #region "Propiedades Publicas"
        public int ApplicationId
        {
            get { return pApplicationId; }
            set { pApplicationId = value; }
        }
        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }
        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }
        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }
        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }
        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }
        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }
        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }
        #endregion

        #region "Funciones Publicas de la Clase"
        // Initializes the provider. 
        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.
            if (config == null)
                throw new ArgumentNullException("config");
            if (name == null || name.Length == 0)
                name = "CustomPostgresConnection";
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Sample Postgres Membership provider");
            }
            // Initialize the abstract base class.
            base.Initialize(name, config);

            pApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredAlphaNumericCharacters"], "1"));
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "True"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "True"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "False"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "True"));
            pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "True"));

            string temp_format = config["passwordFormat"];
            if (temp_format == null)
            {
                temp_format = "Hashed";
            }

            switch (temp_format)
            {
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            // Initialize Oracle connection.
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            ConnectionStringSettings ConexionUnifica = new ConnectionStringSettings("KPIBOARD", connections["CustomPostgresConnection"].ConnectionString);

            if (ConexionUnifica == null || string.IsNullOrEmpty(ConexionUnifica.ConnectionString.Trim()))
            {
                throw new ProviderException("La cadena de conexión no puede estar vacía.");
            }
            connectionString = ConexionUnifica.ConnectionString;

            // Get encryption and decryption key information from the configuration.            
            System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");


            //Obtener el Id de la Aplicacion
            this.ApplicationId = GetApplicationIDByName(this.ApplicationName);
        }
        /// <summary>
        /// Processes a request to update the password for a membership user. 
        /// </summary>
        /// <param name="username">The user to update the password for. </param>
        /// <param name="oldPwd">The current password for the specified user. </param>
        /// <param name="newPwd">The new password for the specified user.</param>
        /// <returns>true if the password was updated successfully; otherwise, false. </returns>
        /// <remarks></remarks>
        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {
            Int64 nResultado = 0;

            if (!ValidateUser(username, oldPwd))
                return false;
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPwd, true);
            OnValidatingPassword(args);
            if (args.Cancel)
            {
                if ((args.FailureInformation != null))
                {
                    throw args.FailureInformation;
                }
                else
                {
                    throw new ProviderException("El cambio de contraseña no se ha efectuado debido a problemas en la validación de la anterior.");
                }
            }
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("aspnet.update_aspnet_password", conn);

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("return_value", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("p_password", NpgsqlDbType.Varchar, 30).Value = EncodePassword(newPwd);
                cmd.Parameters.Add("p_lastpassword", NpgsqlDbType.Timestamp).Value = DateTime.Now;
                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;

                cmd.ExecuteNonQuery();
                nResultado = (Int64)cmd.Parameters["return_value"].Value;
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePassword");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            if (nResultado < 0)
                return false;

            return true;
        }
        /// <summary>
        /// Processes a request to update the password question and answer for a membership user. 
        /// </summary>
        /// <param name="username">The user to change the password question and answer for. </param>
        /// <param name="password">The password for the specified user.</param>
        /// <param name="newPwdQuestion">The new password question for the specified user.</param>
        /// <param name="newPwdAnswer">The new password answer for the specified user.</param>
        /// <returns>true if the password question and answer are updated successfully; otherwise, false. </returns>
        /// <remarks></remarks>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPwdQuestion, string newPwdAnswer)
        {
            Int64 nResultado = 0;

            if (!ValidateUser(username, password))
                return false;

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("aspnet.update_aspnet_passwordquestion", conn);

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("return_value", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("p_passwordquestion", NpgsqlDbType.Varchar, 255).Value = newPwdQuestion;
                cmd.Parameters.Add("p_passwordAnswer", NpgsqlDbType.Varchar, 255).Value = EncodePassword(newPwdAnswer);
                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;

                cmd.ExecuteNonQuery();
                nResultado = (Int64)cmd.Parameters["return_value"].Value;
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePasswordQuestionAndAnswer");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            if (nResultado < 0)
                return false;

            return true;
        }
        /// <summary>
        /// MembershipProvider.CreateUser
        /// Uses columns 
        /// </summary>
        /// <param name="username">The user name for the new user.</param>
        /// <param name="password">The password for the new user. </param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password question for the new user.</param>
        /// <param name="passwordAnswer">The password answer for the new user</param>
        /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
        /// <param name="status">A MembershipCreateStatus enumeration value indicating whether the user was created successfully.</param>
        /// <returns>A MembershipUser object populated with the information for the newly created user. </returns>
        /// <remarks></remarks>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            Int64 nResultado = -1;
            ValidatePasswordEventArgs Args = new ValidatePasswordEventArgs(email, password, true);

            //Funcion de la clase base que comprueba si la contraseña es válida
            this.OnValidatingPassword(Args);

            if (Args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail & !string.IsNullOrEmpty(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser ExistUser = GetUser(email.Trim(), false);

            if (ExistUser == null)
            {
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                NpgsqlCommand cmd = new NpgsqlCommand("aspnet.add_aspnet_membershipuser", conn);

                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("return_value", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add("p_applicationname", NpgsqlDbType.Varchar, 40).Value = this.ApplicationName;
                    cmd.Parameters.Add("P_username", NpgsqlDbType.Varchar, 40).Value = email.Trim();
                    cmd.Parameters.Add("P_password", NpgsqlDbType.Varchar, 30).Value = EncodePassword(password);
                    cmd.Parameters.Add("p_passwordsalt", NpgsqlDbType.Varchar, 60).Value = string.Empty;
                    cmd.Parameters.Add("P_email", NpgsqlDbType.Varchar, 255).Value = email;
                    cmd.Parameters.Add("P_passwordquestion", NpgsqlDbType.Varchar, 255).Value = passwordQuestion;
                    cmd.Parameters.Add("P_passwordanswer", NpgsqlDbType.Varchar, 255).Value = EncodePassword("noseutiliza");
                    cmd.Parameters.Add("P_isapproved", NpgsqlDbType.Bigint).Value = isApproved;
                    cmd.Parameters.Add("P_currenttimeutc", NpgsqlDbType.Timestamp).Value = System.DateTime.Now;
                    cmd.Parameters.Add("P_createdate", NpgsqlDbType.Timestamp).Value = System.DateTime.Now;
                    cmd.Parameters.Add("P_uniqueemail", NpgsqlDbType.Bigint).Value = pRequiresUniqueEmail;
                    cmd.Parameters.Add("P_passwordformat", NpgsqlDbType.Bigint).Value = PasswordFormat.GetHashCode();

                    cmd.ExecuteNonQuery();
                    nResultado = (Int64)cmd.Parameters["return_value"].Value;
                }
                catch (NpgsqlException e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "CreateUser");
                    }
                    nResultado = -1;
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                        cmd.Dispose();
                    }
                }
            }
            else
            {
                nResultado = 1;
            }

            switch ((int)nResultado)
            {
                case -2:
                    status = MembershipCreateStatus.ProviderError;
                    return null;
                case -4:
                    status = MembershipCreateStatus.DuplicateUserName;
                    return null;
                case -5:
                    status = MembershipCreateStatus.DuplicateUserName;
                    return null;
                case -7:
                    status = MembershipCreateStatus.DuplicateEmail;
                    return null;
                default:
                    status = MembershipCreateStatus.Success;
                    return GetUser(email, false);
            }
        }
        /// <summary>
        /// Removes a user from the membership data source.
        /// </summary>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the databases
        ///</param>
        /// <returns>true if the user was successfully deleted; otherwise, false. </returns>
        /// <remarks></remarks>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            Int64 nResultado = -1;
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("aspnet.del_aspnet_user", conn);
            NpgsqlTransaction trans = default(NpgsqlTransaction);

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("return_value", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;

                trans = conn.BeginTransaction();
                cmd.ExecuteNonQuery();
                nResultado = (Int64)cmd.Parameters["return_value"].Value;
                if ((nResultado == 0))
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
                if (deleteAllRelatedData)
                {
                    // Process commands to delete all data for the user in the database.
                    // Como utilizamos PL/SQL, las hemos puesto todas dentro.
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteUser");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            if (nResultado < 0)
                return false;

            return true;
        }
        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data. 
        /// </summary>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>A MembershipUserCollection collection that contains a page of pageSizeMembershipUser objects beginning at the page specified by pageIndex. </returns>
        /// <remarks></remarks>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(USERID) FROM ASPNET_MEMBERSHIP WHERE APPLICATIONID = :p_app", conn);
            MembershipUserCollection users = new MembershipUserCollection();
            NpgsqlDataReader reader = null;

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;

                totalRecords = 0;
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar());
                if (totalRecords <= 0)
                {
                    return users;
                }

                cmd.CommandText = "SELECT USERID, USERNAME, EMAIL, PASSWORDQUESTION, COMMENTS, ISAPPROVED, ISLOCKEDOUT, CREATEDATE, LASTLOGINDATE, LASTACTIVITYDATE, LASTPWDCHANGEDDATE, LASTLOCKOUTDATE FROM ASPNET_MEMBERSHIP WHERE APPLICATIONID = :p_app ORDER BY USERNAME ASC";

                reader = cmd.ExecuteReader();
                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }
                    if (counter >= endIndex)
                        cmd.Cancel();
                    counter += 1;
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllUsers");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if ((reader != null))
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return users;
        }
        /// <summary>
        /// Gets the number of users currently accessing the application. 
        /// </summary>
        /// <returns>The number of users currently accessing the application. </returns>
        /// <remarks></remarks>
        public override int GetNumberOfUsersOnline()
        {

            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(USERID) FROM ASPNET_MEMBERSHIP WHERE LASTACTIVITYDATE > :p_CompareDate AND APPLICATIONID = :p_app", conn);

            int numOnline = 0;
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_CompareDate", NpgsqlDbType.Timestamp).Value = compareTime;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                numOnline = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetNumberOfUsersOnline");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return numOnline;
        }
        /// <summary>
        /// Gets the password for the specified user name from the data source. 
        /// </summary>
        /// <param name="username">The user to retrieve the password for.</param>
        /// <param name="answer">The password answer for the user.</param>
        /// <returns>The password for the specified user name.</returns>
        /// <remarks></remarks>
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("La recuperación de Claves no está habilitada.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("No es posible recuperar claves codificadas en Hash.");
            }

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT PASSWORD, PASSWORDANSWER, ISLOCKEDOUT FROM ASPNET_MEMBERSHIP WHERE USERNAME = :p_username AND APPLICATIONID = :p_app ", conn);

            string password = string.Empty;
            string passwordAnswer = string.Empty;
            NpgsqlDataReader reader = null;

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Read();
                    bool isLockedOut = true;
                    if ((!object.ReferenceEquals(reader.GetValue(2), DBNull.Value)))
                    {
                        if (Convert.ToInt16(reader.GetValue(2)) == 0)
                        {
                            isLockedOut = false;
                        }
                        else
                        {
                            isLockedOut = true;
                        }
                    }
                    if (isLockedOut)
                        throw new MembershipPasswordException("El usuario especificado está desbloqueado.");
                    if (!reader.IsDBNull(0))
                    {
                        password = reader.GetString(0);
                    }
                    if (!reader.IsDBNull(1))
                    {
                        passwordAnswer = reader.GetString(1);
                    }
                }
                else
                {
                    throw new MembershipPasswordException("No se ha encontrado ningún usuario con ese nombre.");
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetPassword");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if ((reader != null))
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");
                throw new MembershipPasswordException("Incorrect password answer.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(password);
            }

            return password;
        }
        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="username">The name of the user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>A MembershipUser object populated with the specified user's information from the data source.</returns>
        /// <remarks></remarks>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT USERID, USERNAME, EMAIL, PASSWORDQUESTION, COMMENTS, ISAPPROVED, ISLOCKEDOUT, CREATEDATE, LASTLOGINDATE, LASTACTIVITYDATE, LASTPWDCHANGEDDATE, LASTLOCKOUTDATE FROM ASPNET_MEMBERSHIP WHERE (USERNAME = :p_user OR EMAIL = :p_email) AND APPLICATIONID = :p_app", conn);

            MembershipUser u = null;
            NpgsqlDataReader reader = null;

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_user", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_email", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    u = GetUserFromReader(reader);

                    if (userIsOnline)
                    {
                        NpgsqlConnection connInside = new NpgsqlConnection(connectionString);
                        NpgsqlCommand updateCmd = new NpgsqlCommand("UPDATE ASPNET_MEMBERSHIP SET LASTACTIVITYDATE = :p_date WHERE USERNAME = :p_user AND APPLICATIONID = :p_app", connInside);

                        try
                        {
                            connInside.Open();
                            updateCmd.CommandType = CommandType.Text;

                            updateCmd.Parameters.Add("p_date", NpgsqlDbType.Timestamp).Value = DateTime.Now;
                            updateCmd.Parameters.Add("p_user", NpgsqlDbType.Varchar, 40).Value = username;
                            updateCmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                            updateCmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (connInside != null)
                            {
                                connInside.Close();
                                connInside.Dispose();
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(String, Boolean)");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if ((reader != null))
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return u;
        }
        /// <summary>
        /// Gets information from the data source for a user based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>A MembershipUser object populated with the specified user's information from the data source.</returns>
        /// <remarks></remarks>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT USERID, USERNAME, EMAIL, PASSWORDQUESTION, COMMENTS, ISAPPROVED, ISLOCKEDOUT, CREATEDATE, LASTLOGINDATE, LASTACTIVITYDATE, LASTPWDCHANGEDDATE, LASTLOCKOUTDATE FROM ASPNET_MEMBERSHIP WHERE USERID = :p_userid", conn);

            MembershipUser u = null;
            NpgsqlDataReader reader = null;

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_userid", NpgsqlDbType.Bigint).Value = providerUserKey;

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    u = GetUserFromReader(reader);

                    if (userIsOnline)
                    {
                        NpgsqlConnection connInside = new NpgsqlConnection(connectionString);
                        NpgsqlCommand updateCmd = new NpgsqlCommand("UPDATE ASPNET_MEMBERSHIP SET LASTACTIVITYDATE = :p_LastActivityDate WHERE USERID = :p_userid", connInside);
                        try
                        {
                            connInside.Open();
                            updateCmd.CommandType = CommandType.Text;

                            updateCmd.Parameters.Add("p_LastActivityDate", NpgsqlDbType.Timestamp).Value = DateTime.Now;
                            updateCmd.Parameters.Add("p_userid", NpgsqlDbType.Bigint).Value = providerUserKey;
                            updateCmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (connInside != null)
                            {
                                connInside.Close();
                                connInside.Dispose();
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(Object, Boolean)");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if ((reader != null))
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return u;
        }
        public override bool UnlockUser(string username)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("aspnet.update_aspnet_unlockuser", conn);

            Int64 rowsAffected = 0;
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("return_value", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("p_lastlockoutdate", NpgsqlDbType.Timestamp).Value = DateTime.Now;
                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                cmd.ExecuteNonQuery();

                rowsAffected = (Int64)cmd.Parameters["return_value"].Value;
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UnlockUser");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }
        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email">The e-mail address to search for. </param>
        /// <returns>The user name associated with the specified e-mail address. If no match is found, return a null reference (Nothing in Visual Basic).</returns>
        /// <remarks></remarks>
        public override string GetUserNameByEmail(string email)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT USERNAME FROM ASPNET_MEMBERSHIP WHERE LOWEREDEMAIL = :p_email AND APPLICATIONID = :p_app", conn);

            string username = string.Empty;
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_email", NpgsqlDbType.Varchar, 128).Value = email.ToLower();
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;

                object result = null;
                result = cmd.ExecuteScalar();
                if (result != null)
                {
                    username = result.ToString();
                }
                else
                {
                    username = string.Empty;
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUserNameByEmail");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return username;
        }
        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">The user to reset the password for.</param>
        /// <param name="answer">The password answer for the specified user.</param>
        /// <returns>The new password for the specified user.</returns>
        /// <remarks></remarks>
        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException("El reestablecimiento de contraseñas no está habilitado.");
            }

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, "passwordAnswer");
                throw new ProviderException("Password answer required for password Reset.");
            }

            //Utilizar esta función propia sólo si sigue dando problemas
            //string newPassword = GetRandomPasswordUsingGUID(newPasswordLength);

            string newPassword = System.Web.Security.Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);
            ValidatePasswordEventArgs Args = new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(Args);
            if (Args.Cancel)
            {
                if ((Args.FailureInformation != null))
                {
                    throw Args.FailureInformation;
                }
                else
                {
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");
                }
            }
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT PASSWORDANSWER, ISLOCKEDOUT FROM ASPNET_MEMBERSHIP WHERE USERNAME = :p_username AND APPLICATIONID = :p_app ", conn);

            int rowsAffected = 0;
            string passwordAnswer = string.Empty;
            NpgsqlDataReader reader = null;

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Read();
                    bool isLockedOut = true;
                    if ((!object.ReferenceEquals(reader.GetValue(1), DBNull.Value)))
                    {
                        if (Convert.ToInt16(reader.GetValue(1)) == 0)
                        {
                            isLockedOut = false;
                        }
                        else
                        {
                            isLockedOut = true;
                        }
                    }
                    if (isLockedOut)
                        throw new MembershipPasswordException("The supplied user is locked out.");
                    if (!reader.IsDBNull(0))
                    {
                        passwordAnswer = reader.GetString(0);
                    }
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }

                if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
                {
                    UpdateFailureCount(username, "passwordAnswer");
                    throw new MembershipPasswordException("Incorrect password answer.");
                }

                NpgsqlConnection connInside = new NpgsqlConnection(connectionString);
                NpgsqlCommand updateCmd = new NpgsqlCommand("UPDATE ASPNET_MEMBERSHIP SET PASSWORD = :p_password, LASTPWDCHANGEDDATE = :p_lastpassword WHERE USERNAME = :p_username AND APPLICATIONID = :p_app AND ISLOCKEDOUT = 0 ", connInside);

                try
                {
                    connInside.Open();
                    updateCmd.CommandType = CommandType.Text;

                    updateCmd.Parameters.Add("p_password", NpgsqlDbType.Varchar, 30).Value = EncodePassword(newPassword);
                    updateCmd.Parameters.Add("p_lastpassword", NpgsqlDbType.Timestamp).Value = DateTime.Now;
                    updateCmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40).Value = username;
                    updateCmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                    rowsAffected = updateCmd.ExecuteNonQuery();
                }
                finally
                {
                    if (connInside != null)
                    {
                        connInside.Close();
                        connInside.Dispose();
                    }
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ResetPassword");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if ((reader != null))
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            if (rowsAffected > 0)
            {
                return newPassword;
            }
            else
            {
                throw new MembershipPasswordException("User not found, or user is locked out. Password not Reset.");
            }
        }
        /// <summary>
        /// Updates information about a user in the data source. 
        /// </summary>
        /// <param name="user">A MembershipUser object that represents the user to update and the updated information for the user.</param>
        /// <remarks></remarks>
        public override void UpdateUser(MembershipUser user)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("aspnet.update_aspnet_membership", conn);

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("return_value", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("p_email", NpgsqlDbType.Varchar, 255).Value = user.Email;
                cmd.Parameters.Add("p_comments", NpgsqlDbType.Varchar, 255).Value = user.Comment;
                cmd.Parameters.Add("p_approved", NpgsqlDbType.Bigint).Value = user.IsApproved;
                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40).Value = user.UserName;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateUser");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>true if the specified username and password are valid; otherwise, false.</returns>
        /// <remarks></remarks>
        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT PASSWORD, ISAPPROVED FROM ASPNET_MEMBERSHIP WHERE (USERNAME = :p_user OR EMAIL = :p_email) AND APPLICATIONID = :p_app AND ISLOCKEDOUT = 0 ", conn);

            NpgsqlDataReader reader = null;
            bool isApproved = false;
            string pwd = string.Empty;
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_user", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_email", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Read();
                    pwd = reader.GetString(0);
                    if ((!object.ReferenceEquals(reader.GetValue(1), DBNull.Value)))
                    {
                        if (Convert.ToInt16(reader.GetValue(1)) == 0)
                        {
                            isApproved = false;
                        }
                        else
                        {
                            isApproved = true;
                        }
                    }
                }
                else
                {
                    return false;
                }

                reader.Close();
                if (CheckPassword(password, pwd))
                {
                    //Tiene permisos para logearse en el sistema
                    if (isApproved)
                    {
                        isValid = true;

                        NpgsqlConnection connInside = new NpgsqlConnection(connectionString);
                        NpgsqlCommand updateCmd = new NpgsqlCommand("UPDATE ASPNET_MEMBERSHIP SET LASTLOGINDATE = :p_lastlogin " + "WHERE (USERNAME = :p_user OR EMAIL = :p_email) AND APPLICATIONID = :p_app ", connInside);

                        try
                        {
                            connInside.Open();
                            updateCmd.CommandType = CommandType.Text;

                            updateCmd.Parameters.Add("p_lastlogin", NpgsqlDbType.Timestamp).Value = DateTime.Now;
                            updateCmd.Parameters.Add("p_user", NpgsqlDbType.Varchar, 40).Value = username;
                            updateCmd.Parameters.Add("p_email", NpgsqlDbType.Varchar, 40).Value = username;
                            updateCmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                            updateCmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (connInside != null)
                            {
                                connInside.Close();
                                connInside.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    UpdateFailureCount(username, "password");
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ValidateUser");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if ((reader != null))
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return isValid;
        }
        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>The total number of matched users.</returns>
        /// <remarks></remarks>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(USERID) FROM ASPNET_MEMBERSHIP WHERE " + usernameToMatch + " AND APPLICATIONID = :p_app", conn);

            MembershipUserCollection users = new MembershipUserCollection();
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar());

                if (totalRecords <= 0)
                    return users;
                cmd.CommandText = "SELECT USERID, USERNAME, EMAIL, PASSWORDQUESTION, COMMENTS, ISAPPROVED, ISLOCKEDOUT, CREATEDATE, LASTLOGINDATE, LASTACTIVITYDATE, LASTPWDCHANGEDDATE, LASTLOCKOUTDATE FROM ASPNET_MEMBERSHIP WHERE " + usernameToMatch + " AND APPLICATIONID = :p_app ORDER BY USERNAME ASC";

                reader = cmd.ExecuteReader();
                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if ((counter >= startIndex) & (counter <= endIndex))
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }
                    counter += 1;
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByName");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if ((reader != null))
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return users;
        }
        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>A MembershipUserCollection collection that contains a page of pageSizeMembershipUser objects beginning at the page specified by pageIndex.</returns>
        /// <remarks></remarks>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(USERID) FROM ASPNET_MEMBERSHIP WHERE EMAIL LIKE :p_email AND APPLICATIONID = :p_app ", conn);

            MembershipUserCollection users = new MembershipUserCollection();
            NpgsqlDataReader reader = null;
            totalRecords = 0;

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_email", NpgsqlDbType.Varchar, 255).Value = emailToMatch;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar());

                if (totalRecords <= 0)
                    return users;
                cmd.CommandText = "SELECT USERID, USERNAME, EMAIL, PASSWORDQUESTION, COMMENTS, ISAPPROVED, ISLOCKEDOUT, CREATEDATE, LASTLOGINDATE, LASTACTIVITYDATE, LASTPWDCHANGEDDATE, LASTLOCKOUTDATE FROM ASPNET_MEMBERSHIP WHERE EMAIL LIKE :p_email AND APPLICATIONID = :p_app ORDER BY USERNAME ASC";

                reader = cmd.ExecuteReader();
                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if ((counter >= startIndex) & (counter <= endIndex))
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }
                    counter += 1;
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByEmail");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if ((reader != null))
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return users;
        }
        #endregion

        #region "Funciones Privadas de la Clase"
        //
        // GetUserFromReader
        //    A helper function that takes the current row from the NpgsqlDataReader
        // and hydrates a MembershiUser from the values. Called by the 
        // MembershipUser.GetUser implementation.
        //
        private MembershipUser GetUserFromReader(NpgsqlDataReader reader)
        {
            int providerUserKey = 0;
            if (!reader.IsDBNull(0))
            {
                providerUserKey = Convert.ToInt32(reader.GetValue(0));
            }
            string username = string.Empty;
            if (!reader.IsDBNull(1))
            {
                username = reader.GetValue(1).ToString();
            }
            string email = string.Empty;
            if (!reader.IsDBNull(2))
            {
                email = reader.GetValue(2).ToString();
            }
            string passwordQuestion = string.Empty;
            if (!reader.IsDBNull(3))
            {
                passwordQuestion = reader.GetValue(3).ToString();
            }
            string comment = string.Empty;
            if (!reader.IsDBNull(4))
            {
                comment = reader.GetValue(4).ToString();
            }
            bool isApproved = false;
            if ((!object.ReferenceEquals(reader.GetValue(5), DBNull.Value)))
            {
                if (Convert.ToInt16(reader.GetValue(5)) == 0)
                {
                    isApproved = false;
                }
                else
                {
                    isApproved = true;
                }
            }
            bool isLockedOut = false;
            if ((!object.ReferenceEquals(reader.GetValue(6), DBNull.Value)))
            {
                if (Convert.ToInt16(reader.GetValue(6)) == 0)
                {
                    isLockedOut = false;
                }
                else
                {
                    isLockedOut = true;
                }
            }
            DateTime creationDate = default(DateTime);
            if (!reader.IsDBNull(7))
            {
                creationDate = Convert.ToDateTime(reader.GetValue(7));
            }
            DateTime lastLoginDate = default(DateTime);
            if (!reader.IsDBNull(8))
            {
                lastLoginDate = Convert.ToDateTime(reader.GetValue(8));
            }
            DateTime lastActivityDate = default(DateTime);
            if (!reader.IsDBNull(9))
            {
                lastActivityDate = Convert.ToDateTime(reader.GetValue(9));
            }
            DateTime lastPasswordChangedDate = default(DateTime);
            if (!reader.IsDBNull(10))
            {
                lastPasswordChangedDate = Convert.ToDateTime(reader.GetValue(10));
            }
            DateTime lastLockedOutDate = default(DateTime);
            if (!reader.IsDBNull(11))
            {
                lastLockedOutDate = Convert.ToDateTime(reader.GetValue(11));
            }

            MembershipUser u = new MembershipUser(this.Name, username, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate,
            lastActivityDate, lastPasswordChangedDate, lastLockedOutDate);

            return u;
        }
        //
        // UpdateFailureCount
        //   A helper method that performs the checks and updates associated with
        // password failure tracking.
        //
        private void UpdateFailureCount(string username, string failureType)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT FAILEDPWDATTEMPTCOUNT, FAILEDPWDATTEMPTWINST, FAILEDPWDANSWERATTEMPTCOUNT, FAILEDPWDANSWERATTEMPTWINST FROM ASPNET_MEMBERSHIP WHERE USERNAME = :p_user AND APPLICATIONID = :p_app", conn);

            NpgsqlDataReader reader = null;
            DateTime windowStart = new DateTime();
            int failureCount = 0;
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_user", NpgsqlDbType.Varchar, 40).Value = username;
                cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();
                    if (failureType == "password")
                    {
                        if (!reader.IsDBNull(0))
                        {
                            failureCount = Convert.ToInt32(reader.GetValue(0));
                        }
                        if (!reader.IsDBNull(1))
                        {
                            windowStart = Convert.ToDateTime(reader.GetValue(1));
                        }
                    }
                    if (failureType == "passwordAnswer")
                    {
                        if (!reader.IsDBNull(2))
                        {
                            failureCount = Convert.ToInt32(reader.GetValue(2));
                        }
                        if (!reader.IsDBNull(3))
                        {
                            windowStart = Convert.ToDateTime(reader.GetValue(3));
                        }
                    }
                }

                reader.Close();
                DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);
                if (failureCount == 0 || DateTime.Now > windowEnd)
                {
                    // First password failure or outside of PasswordAttemptWindow. 
                    // Start a New password failure count from 1 and a New window starting now.

                    if (failureType == "password")
                        cmd.CommandText = "UPDATE ASPNET_MEMBERSHIP   SET FAILEDPWDATTEMPTCOUNT = :p_Count,      FAILEDPWDATTEMPTWINST = :p_WindowStart   WHERE USERNAME = :p_user AND APPLICATIONID = :p_app";
                    if (failureType == "passwordAnswer")
                        cmd.CommandText = "UPDATE ASPNET_MEMBERSHIP   SET FAILEDPWDANSWERATTEMPTCOUNT = :p_Count,       FAILEDPWDANSWERATTEMPTWINST = :p_WindowStart   WHERE USERNAME = :p_user AND APPLICATIONID = :p_app";
                    
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("p_Count", NpgsqlDbType.Bigint).Value = 1;
                    cmd.Parameters.Add("p_WindowStart", NpgsqlDbType.Timestamp).Value = DateTime.Now;
                    cmd.Parameters.Add("p_user", NpgsqlDbType.Varchar, 40).Value = username;
                    cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                    if (cmd.ExecuteNonQuery() < 0)
                        throw new ProviderException("Unable to update failure count and window start.");
                }
                else
                {
                    failureCount += 1;

                    if (failureCount >= MaxInvalidPasswordAttempts)
                    {
                        // Password attempts have exceeded the failure threshold. Lock out
                        // the user.

                        cmd.CommandText = "UPDATE ASPNET_MEMBERSHIP  SET ISLOCKEDOUT = :p_IsLockedOut, LASTLOCKOUTDATE = :p_LastLockedOutDate WHERE USERNAME = :p_user AND APPLICATIONID = :p_app";

                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_IsLockedOut", NpgsqlDbType.Bit).Value = true;
                        cmd.Parameters.Add("p_LastLockedOutDate", NpgsqlDbType.Timestamp).Value = DateTime.Now;
                        cmd.Parameters.Add("p_user", NpgsqlDbType.Varchar, 40).Value = username;
                        cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                        if (cmd.ExecuteNonQuery() < 0)
                            throw new ProviderException("Unable to lock out user.");
                    }
                    else
                    {
                        // Password attempts have not exceeded the failure threshold. Update
                        // the failure counts. Leave the window the same.

                        if (failureType == "password")
                            cmd.CommandText = "UPDATE ASPNET_MEMBERSHIP  SET FAILEDPWDATTEMPTCOUNT = :p_Count  WHERE USERNAME = :p_user AND APPLICATIONID = :p_app";

                        if (failureType == "passwordAnswer")
                            cmd.CommandText = "UPDATE ASPNET_MEMBERSHIP  SET FAILEDPWDANSWERATTEMPTCOUNT = :p_Count  WHERE USERNAME = :p_user AND APPLICATIONID = :p_app";

                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("p_Count", NpgsqlDbType.Bigint).Value = failureCount;
                        cmd.Parameters.Add("p_user", NpgsqlDbType.Varchar, 40).Value = username;
                        cmd.Parameters.Add("p_app", NpgsqlDbType.Bigint).Value = this.ApplicationId;
                        if (cmd.ExecuteNonQuery() < 0)
                            throw new ProviderException("Unable to update failure count.");
                    }
                }
            }
            catch (NpgsqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateFailureCount");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if ((reader != null))
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        //
        // CheckPassword
        //   Compares password values based on the MembershipPasswordFormat.
        //
        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
            {
                return true;
            }
            return false;
        }
        //
        // EncodePassword
        //   Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        //
        private string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword = Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(machineKey.ValidationKey);
                    encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Formato de contraseña no soportado.");
            }
            return encodedPassword;
        }
        //
        // UnEncodePassword
        //   Decrypts or leaves the password clear based on the PasswordFormat.
        //
        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }
            return password;
        }
        //
        // HexToByte
        //   Converts a hexadecimal string to a byte array. Used to convert encryption
        // key values from the configuration.
        //
        private byte[] HexToByte(string hexString)
        {
            byte[] ReturnBytes = new byte[hexString.Length / 2 + 1];
            for (int i = 0; i <= hexString.Length / 2 - 1; i++)
            {
                string s = null;
                s = hexString.Substring(i * 2, 2);
                ReturnBytes[i] = Convert.ToByte(s, 16);
            }
            return ReturnBytes;
        }
        //
        // WriteToEventLog
        //   A helper function that writes exception detail to the event log. Exceptions
        // are written to the event log as a security measure to aSub Private database
        // details from being Returned to the browser. If a method does not Return a status
        // or boolean indicating the action succeeded or failed, a generic exception is also 
        // Thrown by the caller.
        private void WriteToEventLog(Exception e, string action)
        {
            EventLog log = new EventLog();
            log.Source = eventSource;
            log.Log = eventLog;

            string message = "An exception occurred communicating with the data source.\r\n\r\n";
            message += "Action: " + action + "\r\n\r\n";
            message += "Exception: " + e.ToString();

            log.WriteEntry(message);
        }

        private int GetApplicationIDByName(string application)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT APPLICATIONID FROM ASPNET_APPLICATIONS WHERE LOWER(APPLICATIONNAME) = LOWER(:p_app)", conn);

            int applicationID = -1;

            try
            {             
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_app", NpgsqlDbType.Varchar, 40).Value = application;
                applicationID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return applicationID;
        }
        //
        // A helper function to retrieve config values from the configuration file.
        //
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;
            return configValue;
        }
        //
        // Funcion para generar una contraseña aleatoria sin caracteres extraños
        //
        public string GetRandomPasswordUsingGUID(int length)
        {
            // Get the GUID
            string guidResult = System.Guid.NewGuid().ToString();

            // Remove the hyphens
            guidResult = guidResult.Replace("-", string.Empty);

            // Make sure length is valid
            if (length <= 0 || length > guidResult.Length)
                return GetRandomPasswordUsingGUID(length);

            // Return the first length bytes
            return guidResult.Substring(0, length);
        }
        #endregion
    }
}

