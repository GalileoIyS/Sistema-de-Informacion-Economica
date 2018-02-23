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
using System.Collections.Generic;
using System.Linq;

namespace PostgresCustomProvider
{
    public class PostgresRoleProvider : RoleProvider
    {
        #region "Variables privadas de la Clase"
        private string connectionString;
        private int pApplicationId;
        private string pApplicationName;
        #endregion

        #region "Propiedades Publicas"
        public int ApplicationId
        {
            get { return pApplicationId; }
            set { pApplicationId = value; }
        }
        public override string ApplicationName
        {
            get { return this.pApplicationName; }
            set { this.pApplicationName = value; }
        }
        #endregion

        #region "Funciones Publicas de la Clase"
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            // Initialize Oracle connection.
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            ConnectionStringSettings MiConexion = new ConnectionStringSettings("KPIBOARD", connections["CustomPostgresConnection"].ConnectionString);

            if (MiConexion == null || string.IsNullOrEmpty(MiConexion.ConnectionString.Trim()))
            {
                throw new ProviderException("La cadena de conexión no puede estar vacía.");
            }
            connectionString = MiConexion.ConnectionString;

            //Obtener el Id de la Aplicacion
            pApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            this.ApplicationId = GetApplicationIDByName(this.ApplicationName);

        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("aspnet.add_aspnet_userstoroles", conn);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("return_value", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40);
                cmd.Parameters.Add("p_rolename", NpgsqlDbType.Varchar, 40);

                foreach (string username in usernames)
                {
                    foreach (string rolename in roleNames)
                    {
                        conn.Open();
                        cmd.Parameters["p_username"].Value = username;
                        cmd.Parameters["p_rolename"].Value = rolename;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public override void CreateRole(string roleName)
        {
            Int64 nResultado = -1;
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("aspnet.add_aspnet_roles", conn);
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("return_value", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("p_applicationname", NpgsqlDbType.Varchar, 40).Value = this.ApplicationName;
                cmd.Parameters.Add("p_rolename", NpgsqlDbType.Varchar, 40).Value = roleName;

                cmd.ExecuteNonQuery();
                nResultado = (Int64)cmd.Parameters["return_value"].Value;
                if (nResultado < 0)
                {
                    throw new ApplicationException("Ha sido imposible añadir el Rol a la Base de Datos.");
                }
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            Int64 nResultado = -1;
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("aspnet.del_aspnet_role", conn);
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("return_value", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("p_applicationname", NpgsqlDbType.Varchar, 40).Value = this.ApplicationName;
                cmd.Parameters.Add("p_rolename", NpgsqlDbType.Varchar, 40).Value = roleName;
                if ((throwOnPopulatedRole))
                {
                    cmd.Parameters.Add("p_deleteonlyifroleisempty", NpgsqlDbType.Smallint).Value = 1;
                }
                else
                {
                    cmd.Parameters.Add("p_deleteonlyifroleisempty", NpgsqlDbType.Smallint).Value = 0;
                }

                cmd.ExecuteNonQuery();
                nResultado = (Int64)cmd.Parameters["return_value"].Value;
                if (nResultado < 0)
                {
                    throw new System.Configuration.Provider.ProviderException("Role is not empty.");
                }
            }
            catch (NpgsqlException excp)
            {
                nResultado = -1;
                throw new System.Configuration.Provider.ProviderException(excp.Message);
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return true;
        }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT USERID FROM ASPNET_USERSINROLES A, ASPNET_ROLES B " + "WHERE A.ROLEID = B.ROLEID AND B.LOWEREDROLENAME = :p_rolename ", conn);
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("p_rolename", NpgsqlDbType.Varchar, 40).Value = roleName.ToLower();

                System.Data.DataTable dtRoles = new System.Data.DataTable();
                NpgsqlDataAdapter daRoles = new NpgsqlDataAdapter(cmd);
                daRoles.Fill(dtRoles);
                System.String[] userCount = new System.String[dtRoles.Rows.Count];

                for (int c = 0; c <= dtRoles.Rows.Count - 1; c++)
                {
                    userCount[c] = dtRoles.Rows[c]["USERID"].ToString();
                }
                return userCount;
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public override string[] GetAllRoles()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT ROLEID, ROLENAME FROM ASPNET_ROLES", conn);
            System.Data.DataTable dtRoles = new System.Data.DataTable();
            NpgsqlDataAdapter daRoles = new NpgsqlDataAdapter(cmd);

            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;
                daRoles.Fill(dtRoles);

                System.String[] roles = new System.String[dtRoles.Rows.Count];
                for (int c = 0; c <= dtRoles.Rows.Count - 1; c++)
                {
                    roles[c] = dtRoles.Rows[c]["ROLENAME"].ToString();
                }
                return roles;
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public override string[] GetRolesForUser(string username)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT C.ROLENAME FROM ASPNET_USERSINROLES A, ASPNET_MEMBERSHIP B, ASPNET_ROLES C WHERE C.ROLEID = A.ROLEID AND A.USERID = B.USERID AND (B.LOWEREDUSERNAME = :p_username OR B.EMAIL = :p_email) ", conn);
            System.Data.DataTable dtRoles = new System.Data.DataTable();
            NpgsqlDataAdapter daRoles = new NpgsqlDataAdapter(cmd);
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40).Value = username.ToLower();
                cmd.Parameters.Add("p_email", NpgsqlDbType.Varchar, 40).Value = username.ToLower();
                daRoles.Fill(dtRoles);

                System.String[] roles = new System.String[dtRoles.Rows.Count];
                for (int c = 0; c <= dtRoles.Rows.Count - 1; c++)
                {
                    roles[c] = dtRoles.Rows[c]["ROLENAME"].ToString();
                }
                return roles;
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public override string[] GetUsersInRole(string roleName)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT USERID FROM ASPNET_USERSINROLES A, ASPNET_ROLES B WHERE A.ROLEID = B.ROLEID AND B.LOWEREDROLENAME = :p_rolename ", conn);
            System.Data.DataTable dtRoles = new System.Data.DataTable();
            NpgsqlDataAdapter daRoles = new NpgsqlDataAdapter(cmd);
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("p_rolename", NpgsqlDbType.Varchar, 40).Value = roleName.ToLower();
                daRoles.Fill(dtRoles);

                System.String[] roles = new System.String[dtRoles.Rows.Count];
                for (int c = 0; c <= dtRoles.Rows.Count - 1; c++)
                {
                    roles[c] = dtRoles.Rows[c]["USERID"].ToString();
                }
                return roles;
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            Decimal nResultado = -1;
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(A.USERID) " + "FROM ASPNET_USERSINROLES A, ASPNET_MEMBERSHIP B, ASPNET_ROLES C WHERE A.USERID = B.USERID AND A.ROLEID = C.ROLEID AND (B.LOWEREDUSERNAME = :p_username OR B.EMAIL = :p_email) AND LOWER(C.ROLENAME) = :p_rolename ", conn);
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40).Value = username.ToLower();
                cmd.Parameters.Add("p_email", NpgsqlDbType.Varchar, 40).Value = username.ToLower();
                cmd.Parameters.Add("p_rolename", NpgsqlDbType.Varchar, 40).Value = roleName.ToLower();

                nResultado = (Decimal)cmd.ExecuteScalar();
                if (nResultado == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("aspnet.del_aspnet_usersfromroles", conn);

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("return_value", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("p_username", NpgsqlDbType.Varchar, 40);
                cmd.Parameters.Add("p_rolename", NpgsqlDbType.Varchar, 40);

                foreach (string username in usernames)
                {
                    foreach (string rolename in roleNames)
                    {
                        cmd.Parameters["p_username"].Value = username;
                        cmd.Parameters["p_rolename"].Value = rolename;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public override bool RoleExists(string roleName)
        {
            Int64 nResultado = -1;
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(ROLEID) FROM ASPNET_ROLES where ROLENAME = :p_rolename ", conn);

            try
            {
                conn.Open();
                cmd.Parameters.Add("p_rolename", NpgsqlDbType.Varchar, 40).Value = roleName;
                cmd.CommandType = CommandType.Text;
                nResultado = (Int64)cmd.ExecuteScalar();
                if (nResultado > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public int ObtenerDatos()
        {
            return 0;
        }
        #endregion

        #region "Funciones Privadas de la Clase"
        private int GetApplicationIDByName(string application)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            NpgsqlCommand cmd = new NpgsqlCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT APPLICATIONID FROM ASPNET_APPLICATIONS WHERE LOWER(APPLICATIONNAME) = LOWER(:p_app)";
            cmd.Parameters.Add("p_app", NpgsqlDbType.Varchar, 40).Value = application;

            try
            {
                int? applicationID = -1;
                conn.Open();
                cmd.Connection = conn;

                applicationID = Convert.ToInt16(cmd.ExecuteScalar());

                if (applicationID.HasValue)
                    return applicationID.Value;
                else
                    return -1;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if ((conn != null))
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }
        #endregion
    }
}

