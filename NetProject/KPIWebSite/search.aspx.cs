using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

public partial class search : BasePage
{
    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["tagstring"]))
            {
                txtBuscarIndicador.Text = Convert.ToString(Request.QueryString["tagstring"]);
            }

            RellenaEtiquetas();
            RellenaCategorias();
            RellenaIndicadores();
        }
    }
    private void Page_Error(object sender, EventArgs e)
    {
        // Obtenemos el último error del servidor.
        Exception exc = Server.GetLastError();

        // Controlamos esta específica excepción
        if (exc is InvalidOperationException)
        {
            // Enviamos la información del error a la página de errores
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%search.aspx",
                true);
        }
    }
    #endregion

    #region Funciones Privadas de Relleno
    private void RellenaIndicadores()
    {
        int nRecuentoTotal = 0;
        int? nUsuarioID = CalculaUserId();

        try
        {
            using (Clases.cKPI_INDICATORS objIndicadores = new Clases.cKPI_INDICATORS())
            {
                objIndicadores.titulo = txtBuscarIndicador.Text;
                objIndicadores.userid = nUsuarioID;
                objIndicadores.publicado = true;
                nRecuentoTotal = objIndicadores.RecuentoBuscador();
            }
            using (Clases.cKPI_DATASETS objDatasets = new Clases.cKPI_DATASETS())
            {
                lbTotalDatasets.Text = objDatasets.Recuento().ToString();
            }
        }
        catch (Exception excp)
        {
            ExceptionUtility.LogException(excp, "Error en la función << RellenaIndicadores() >>");
        }
        finally
        {
            if (nRecuentoTotal == 0)
            {
                lbNumIndicadores.Text = "There are no available data";
                lbNumIndicadores.CssClass = "search-no-count";
            }
            else
            {
                if (nRecuentoTotal < 15)
                {
                        lbNumIndicadores.Text = "Showing 1-" + nRecuentoTotal.ToString() + " of " + nRecuentoTotal.ToString() + " results";
                }
                else
                {
                        lbNumIndicadores.Text = "Showing 1-15 of " + nRecuentoTotal.ToString() + " results";
                }
                lbNumIndicadores.CssClass = "search-count";
                lbTotalDropkeys.Text = nRecuentoTotal.ToString();
            }

            using (Clases.cKPI_INDICATORS objIndicadores = new Clases.cKPI_INDICATORS())
            {
                objIndicadores.titulo = txtBuscarIndicador.Text;
                objIndicadores.userid = nUsuarioID;
                objIndicadores.publicado = true;
                lstIndicadores.DataSource = objIndicadores.BuscarIndicadores(15, 1, " ORDER BY FECHA_ALTA DESC");
                lstIndicadores.DataBind();
            }
        }
    }
    private void RellenaEtiquetas()
    {
        try
        {
            using (Clases.cKPI_ETIQUETAS objEtiquetas = new Clases.cKPI_ETIQUETAS())
            {
                lstEtiquetas.DataSource = objEtiquetas.TopXUtilizadas(25);
                lstEtiquetas.DataBind();
            }
        }
        catch (Exception excp)
        {
            ExceptionUtility.LogException(excp, "Error en la función << RellenaEtiquetas() >>");
        }
    }
    private void RellenaCategorias()
    {
        try
        {
            using (Clases.cKPI_CATEGORIES objCategorias = new Clases.cKPI_CATEGORIES())
            {
                System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
                if (usr != null)
                {
                    lstCategorias.DataSource = objCategorias.ObtenerTodos(Convert.ToInt32(usr.ProviderUserKey));
                }
                else
                    lstCategorias.DataSource = objCategorias.ObtenerTodos(null);
                lstCategorias.DataBind();
            }
        }
        catch (Exception excp)
        {
            ExceptionUtility.LogException(excp, "Error en la función << RellenaCategorias() >>");
        }
        finally
        {
            using (Clases.cKPI_INDICATORS objIndicadores = new Clases.cKPI_INDICATORS())
            {
                lbNumDatos.Text = objIndicadores.RecuentoBuscador().ToString();
            }
        }
    }
    #endregion
}