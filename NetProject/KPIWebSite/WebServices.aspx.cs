using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Security;
using Newtonsoft.Json;
using Google.DataTable.Net.Wrapper;
using System.Text;

public partial class publico_WebServices : System.Web.UI.Page
{
    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    #region Funciones Privadas
    protected static void InsertaAviso(string sPage, string sMessage)
    {
        using (Clases.cASPNET_AVISOS objAviso = new Clases.cASPNET_AVISOS())
        {
            objAviso.pagina = sPage;
            objAviso.codigo_error = 4;
            objAviso.mensaje = sMessage;
            objAviso.bInsertar();
        }
    }
    protected static List<string> StringToFechaFormat(List<string> fmts_out, string Fecha)
    {
        string[] cultureNames = { "en-US", "en-GB", "fr-FR", "fi-FI", "es-ES" };
        foreach (var cultureName in cultureNames)
        {
            System.Globalization.CultureInfo cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(cultureName);
            List<string> fmts_in = cultureInfo.DateTimeFormat.GetAllDateTimePatterns().ToList();
            fmts_in.Add("yyyy");

            DateTime dResultado;
            foreach (string Formato in fmts_in)
            {
                if (DateTime.TryParseExact(Fecha, Formato, System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None,
                                    out dResultado))
                {
                    if (!fmts_out.Contains(Formato))
                        fmts_out.Add(Formato);
                }
            }
        }
        return fmts_out;
    }
    protected static Decimal? ConvertirStringToDecimal(string Valor)
    {
        Decimal dResultado;
        if (Decimal.TryParse(Valor, out dResultado))
            return dResultado;
        return null;
    }
    protected string CalculaFechaDesdeCuando(string sFechaOriginal)
    {
        if (sFechaOriginal == "nunca")
            return "nunca";

        string sFechaDesde = string.Empty;
        DateTime dtFechaOriginal, dtFechaActual;

        try
        {
            dtFechaOriginal = Convert.ToDateTime(sFechaOriginal);
            dtFechaActual = DateTime.Now;

            //Diferencia en Dias
            TimeSpan ts = dtFechaActual - dtFechaOriginal;

            // Diferencia en días.
            int diferenciaEnDias = ts.Days;

            if (diferenciaEnDias > 0)
            {
                if (diferenciaEnDias == 1)
                    sFechaDesde = "Hace 1 día";
                else
                    sFechaDesde = "Hace " + diferenciaEnDias.ToString() + " días";
            }
            else
            {
                int diferenciaEnHoras = ts.Hours;
                if (diferenciaEnHoras > 0)
                {
                    if (diferenciaEnHoras == 1)
                        sFechaDesde = "Hace 1 hora";
                    else
                        sFechaDesde = "Hace " + diferenciaEnHoras.ToString() + " horas";
                }
                else
                {
                    int diferenciaEnMinutos = ts.Minutes;
                    if (diferenciaEnMinutos > 0)
                    {
                        if (diferenciaEnMinutos == 1)
                            sFechaDesde = "Hace 1 minuto";
                        else
                            sFechaDesde = "Hace " + diferenciaEnMinutos.ToString() + " minutos";
                    }
                    else
                        sFechaDesde = "Menos de 1 minuto";
                }
            }
        }
        catch
        {
            sFechaDesde = string.Empty;
        }

        return sFechaDesde;
    }
    protected string CalculaFechaDesdeCuando(DateTime dtFechaOriginal)
    {
        string sFechaDesde = string.Empty;
        DateTime dtFechaActual;

        try
        {
            dtFechaActual = DateTime.Now;

            //Diferencia en Dias
            TimeSpan ts = dtFechaActual - dtFechaOriginal;

            // Diferencia en días.
            int diferenciaEnDias = ts.Days;

            if (diferenciaEnDias > 0)
            {
                if (diferenciaEnDias == 1)
                    sFechaDesde = "Hace 1 día";
                else
                    sFechaDesde = "Hace " + diferenciaEnDias.ToString() + " días";
            }
            else
            {
                int diferenciaEnHoras = ts.Hours;
                if (diferenciaEnHoras > 0)
                {
                    if (diferenciaEnHoras == 1)
                        sFechaDesde = "Hace 1 hora";
                    else
                        sFechaDesde = "Hace " + diferenciaEnHoras.ToString() + " horas";
                }
                else
                {
                    int diferenciaEnMinutos = ts.Minutes;
                    if (diferenciaEnMinutos > 0)
                    {
                        if (diferenciaEnMinutos == 1)
                            sFechaDesde = "Hace 1 minuto";
                        else
                            sFechaDesde = "Hace " + diferenciaEnMinutos.ToString() + " minutos";
                    }
                    else
                        sFechaDesde = "Menos de 1 minuto";
                }
            }
        }
        catch
        {
            sFechaDesde = string.Empty;
        }

        return sFechaDesde;
    }
    #endregion

    #region Metodos Web
    //Nombre: ObtenerDashboards
    //Llamador: /custom/search.js
    //Descripción: Obtiene todos los cuadros de mando de un usuario
    [WebMethod]
    public static string ObtenerDashboards()
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        using (Clases.cKPI_DASHBOARDS objDashboards = new Clases.cKPI_DASHBOARDS())
        {
            System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
            if (usr == null)
            {
                return JsonConvert.SerializeObject(string.Empty);
            }

            objDashboards.userid = Convert.ToInt32(usr.ProviderUserKey);
            System.Data.DataTableReader dtrDatos = objDashboards.ObtenerDatos().CreateDataReader();
            while (dtrDatos.Read())
            {
                JSonClases.KPI_DASHBOARD objJSonDashboards = new JSonClases.KPI_DASHBOARD();

                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IDDASHBOARD")))
                    objJSonDashboards.id = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("IDDASHBOARD")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("TITULO")))
                    objJSonDashboards.value = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("TITULO")));

                sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonDashboards);
                sComa = ", ";
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return JsonConvert.SerializeObject(string.Empty);
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerWidgets
    //Llamador: /custom/search.js
    //Descripción: Obtiene todos los widgets de un determinado cuadro de mando
    [WebMethod]
    public static string ObtenerWidgets(int iddashboard)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        using (Clases.cKPI_WIDGETS objWidget = new Clases.cKPI_WIDGETS())
        {
            System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
            if (usr == null)
            {
                return string.Empty;
            }

            objWidget.userid = Convert.ToInt32(usr.ProviderUserKey);
            objWidget.iddashboard = iddashboard;
            System.Data.DataTableReader dtrDatos = objWidget.ObtenerDatos().CreateDataReader();
            while (dtrDatos.Read())
            {
                JSonClases.KPI_WIDGET objJSonWidgets = new JSonClases.KPI_WIDGET();

                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IDWIDGET")))
                    objJSonWidgets.id = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("IDWIDGET")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("TITULO")))
                    objJSonWidgets.value = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("TITULO")));

                sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonWidgets);
                sComa = ", ";
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return string.Empty;
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerFunctions
    //Llamador: /custom/search.js
    //Descripción: Obtiene las diferentes funciones aplicables a un indicador
    [WebMethod]
    public static string ObtenerFunciones()
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        using (Clases.cKPI_INDICATOR_TIPOS objFunctions = new Clases.cKPI_INDICATOR_TIPOS())
        {
            System.Data.DataTableReader dtrDatos = objFunctions.ObtenerDatos().CreateDataReader();
            while (dtrDatos.Read())
            {
                JSonClases.KPI_FUNCTIONS objJSonFunction = new JSonClases.KPI_FUNCTIONS();

                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IDTIPO")))
                    objJSonFunction.id = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("IDTIPO")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("NOMBRE")))
                    objJSonFunction.value = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("NOMBRE")));

                sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonFunction);
                sComa = ", ";
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return JsonConvert.SerializeObject(string.Empty);
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerDimensionValores
    //Llamador: /custom/datos.js
    //Descripción: Obtiene valores de una determinada dimension para el autocomplete
    [WebMethod]
    public static string ObtenerDimensionValores(int dimensionid, string texto)
    {
        string sJSON = string.Empty;

        using (Clases.cKPI_DIMENSION_VALUES objDimensionValues = new Clases.cKPI_DIMENSION_VALUES())
        {
            objDimensionValues.dimensionid = dimensionid;
            objDimensionValues.codigo = texto;
            System.Data.DataTable dtValores = objDimensionValues.ObtenerValores();

            if (dtValores != null)
                sJSON = JsonConvert.SerializeObject(dtValores);
        }
        return sJSON;
    }

    //Nombre: ObtenerDimensionValoresAsignados
    //Llamador: /custom/datos.js
    //Descripción: Obtiene los valores de una determinada dimension asignados al filtro actual
    [WebMethod]
    public static string ObtenerDimensionValoresAsignados(int dimensionid, int idfilter)
    {
        string sJSON = string.Empty;

        using (Clases.cKPI_DIMENSION_VALUES objDimensionValues = new Clases.cKPI_DIMENSION_VALUES())
        {
            objDimensionValues.dimensionid = dimensionid;
            System.Data.DataTable dtValores = objDimensionValues.ObtenerValoresAsignados(idfilter);

            if (dtValores != null)
                sJSON = JsonConvert.SerializeObject(dtValores);
        }
        return sJSON;
    }

    //Nombre: ObtenerDimensionValoresNoAsignados
    //Llamador: /custom/datos.js
    //Descripción: Obtiene los valores de una determinada dimension que aún no han sido asignados al filtro actual
    [WebMethod]
    public static string ObtenerDimensionValoresNoAsignados(int dimensionid, int idfilter)
    {
        string sJSON = string.Empty;

        using (Clases.cKPI_DIMENSION_VALUES objDimensionValues = new Clases.cKPI_DIMENSION_VALUES())
        {
            objDimensionValues.dimensionid = dimensionid;
            System.Data.DataTable dtValores = objDimensionValues.ObtenerValoresNoAsignados(idfilter);

            if (dtValores != null)
                sJSON = JsonConvert.SerializeObject(dtValores);
        }
        return sJSON;
    }

    //Nombre: ObtenerIndicadoresDelWidget
    //Llamador: /custom/graphics.js
    //Descripción: Obtiene los indicadores asignados a un widget
    [WebMethod]
    public static string ObtenerFormulasDelWidget(int idwidget)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        using (Clases.cKPI_WIDGET_FORMULAS objFormulas = new Clases.cKPI_WIDGET_FORMULAS())
        {
            objFormulas.idwidget = idwidget;
            System.Data.DataTableReader dtrDatos = objFormulas.ObtenerDatos().CreateDataReader();
            while (dtrDatos.Read())
            {
                JSonClases.KPI_WIDGET_FORMULAS objJSonFormula = new JSonClases.KPI_WIDGET_FORMULAS();

                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IDFORMULA")))
                    objJSonFormula.formulaid = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("IDFORMULA")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IDWIDGET")))
                    objJSonFormula.widgetid = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("IDWIDGET")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("FECHA")))
                    objJSonFormula.fecha = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("FECHA")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("FORMULA")))
                    objJSonFormula.formula = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("FORMULA")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("NOMBRE")))
                    objJSonFormula.nombre = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("NOMBRE")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("COLOR")))
                    objJSonFormula.color = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("COLOR")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("VALIDATED")))
                    objJSonFormula.validated = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("VALIDATED")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("DISPLAY")))
                    objJSonFormula.display = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("DISPLAY")));

                sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonFormula);
                sComa = ", ";
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return string.Empty;
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerDimensionesDeIndicador
    //Llamador: /custom/graphics.js
    //Descripción: Obtiene las dimensiones de un indicador
    [WebMethod]
    public static string ObtenerDimensionesDeIndicador(int indicatorid)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        using (Clases.cKPI_DIMENSIONS objDimensiones = new Clases.cKPI_DIMENSIONS())
        {
            objDimensiones.indicatorid = indicatorid;
            System.Data.DataTableReader dtrDatos = objDimensiones.ObtenerDatos(10, 1).CreateDataReader();
            while (dtrDatos.Read())
            {
                JSonClases.KPI_COMBOBOX objJSonDimensiones = new JSonClases.KPI_COMBOBOX();

                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("DIMENSIONID")))
                    objJSonDimensiones.id = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("DIMENSIONID")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("INDICATORID")))
                    objJSonDimensiones.idextra = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("INDICATORID")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("NOMBRE")))
                    objJSonDimensiones.value = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("NOMBRE")));

                sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonDimensiones);
                sComa = ", ";
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return string.Empty;
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerDimensionesDeIndicadorParaImportar
    //Llamador: /proxy.js
    //Descripción: Obtiene todos los atributos definidos para un indicador mas el campo (vacío), fecha y Indicador
    [WebMethod]
    public static string ObtenerDimensionesDeIndicadorParaImportar(int idindicator)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        //Añadimos (Vacio)
        JSonClases.KPI_DIMENSIONS objJSonNinguna = new JSonClases.KPI_DIMENSIONS();
        objJSonNinguna.id = -1;
        objJSonNinguna.indicatorid = idindicator;
        objJSonNinguna.value = "-- none --";
        sJSON = JsonConvert.SerializeObject(objJSonNinguna);

        //Añadimos Fecha
        JSonClases.KPI_DIMENSIONS objJSonFecha = new JSonClases.KPI_DIMENSIONS();
        objJSonFecha.id = -2;
        objJSonFecha.indicatorid = idindicator;
        objJSonFecha.value = "Date Field";
        sJSON = sJSON + ", " + JsonConvert.SerializeObject(objJSonFecha);

        //Añadimos (Vacio)
        JSonClases.KPI_DIMENSIONS objJSonIndicador = new JSonClases.KPI_DIMENSIONS();
        objJSonIndicador.id = -3;
        objJSonIndicador.indicatorid = idindicator;
        objJSonIndicador.value = "Value Field";
        sJSON = sJSON + ", " + JsonConvert.SerializeObject(objJSonIndicador);

        using (Clases.cKPI_DIMENSIONS objDimensiones = new Clases.cKPI_DIMENSIONS())
        {
            objDimensiones.indicatorid = idindicator;
            System.Data.DataTableReader dtrDatos = objDimensiones.ObtenerDatos(10, 1).CreateDataReader();
            while (dtrDatos.Read())
            {
                JSonClases.KPI_DIMENSIONS objJSonDimensiones = new JSonClases.KPI_DIMENSIONS();

                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("DIMENSIONID")))
                    objJSonDimensiones.id = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("DIMENSIONID")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("INDICATORID")))
                    objJSonDimensiones.indicatorid = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("INDICATORID")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("NOMBRE")))
                    objJSonDimensiones.value = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("NOMBRE")));

                sJSON = sJSON + ", " + JsonConvert.SerializeObject(objJSonDimensiones);
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return string.Empty;
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerExpresionesDeFormula
    //Llamador: /custom/graphics.js
    //Descripción: Obtiene las expresiones de filtro de una fórmula
    [WebMethod]
    public static string ObtenerExpresionesDeFormula(int idformula)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        using (Clases.cKPI_WIDGET_EXPRESIONS objExpresiones = new Clases.cKPI_WIDGET_EXPRESIONS())
        {
            objExpresiones.idformula = idformula;
            System.Data.DataTableReader dtrDatos = objExpresiones.ObtenerDatos().CreateDataReader();
            while (dtrDatos.Read())
            {
                JSonClases.KPI_COMBOBOX objJSonExpresions = new JSonClases.KPI_COMBOBOX();

                //if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IDFORMULA")))
                //    objJSonExpresions.formulaid = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("IDFORMULA")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IDEXPRESION")))
                    objJSonExpresions.id = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("IDEXPRESION")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("INDICATORID")))
                    objJSonExpresions.idextra = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("INDICATORID")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("TITULO")))
                    objJSonExpresions.value = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("TITULO")));
                //if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("FUNCION")))
                //    objJSonExpresions.funcion = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("FUNCION")));

                sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonExpresions);
                sComa = ", ";
            }
        }
        return "[" + sJSON + "]";
    }

    //Nombre: ObtenerFiltrosDeExpresion
    //Llamador: /custom/graphics.js
    //Descripción: Obtiene los filtros de una expresión
    [WebMethod]
    public static string ObtenerFiltrosDeExpresion(int idexpresion, int pageSize, int currentPage, string orderby)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        using (Clases.cKPI_WIDGET_FILTERS objFiltros = new Clases.cKPI_WIDGET_FILTERS())
        {
            objFiltros.idexpresion = idexpresion;
            System.Data.DataTableReader dtrDatos = objFiltros.ObtenerDatos(pageSize, currentPage, string.Empty, orderby).CreateDataReader();
            while (dtrDatos.Read())
            {
                JSonClases.KPI_WIDGET_FILTERS objJSonFilters = new JSonClases.KPI_WIDGET_FILTERS();

                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IDFILTER")))
                    objJSonFilters.filterid = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("IDFILTER")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IDEXPRESION")))
                    objJSonFilters.expresionid = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("IDEXPRESION")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("DIMENSIONID")))
                    objJSonFilters.dimensionid = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("DIMENSIONID")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("FILTRO")))
                    objJSonFilters.filtro = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("FILTRO")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("VALOR")))
                    objJSonFilters.valor = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("VALOR")));

                sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonFilters);
                sComa = ", ";
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return string.Empty;
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerTiposFiltros
    //Llamador: /custom/graphics.js
    //Descripción: Obtiene las dimensiones de un indicador
    [WebMethod]
    public static string ObtenerTiposFiltros()
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        using (Clases.cKPI_TIPOS_FILTERS objFiltros = new Clases.cKPI_TIPOS_FILTERS())
        {
            System.Data.DataTableReader dtrDatos = objFiltros.ObtenerDatos().CreateDataReader();
            while (dtrDatos.Read())
            {
                JSonClases.KPI_TIPOS_FILTERS objJSonFiltros = new JSonClases.KPI_TIPOS_FILTERS();

                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("FILTRO")))
                    objJSonFiltros.id = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("FILTRO")));
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("NOMBRE")))
                    objJSonFiltros.value = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("NOMBRE")));

                sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonFiltros);
                sComa = ", ";
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return string.Empty;
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerCualquierIndicador
    //Llamador: /custom/graphics.js 
    //Descripción: Busca indicadores que cumplan con los criterios de búsqueda especificados
    [WebMethod]
    public static string ObtenerCualquierIndicador(string nombre)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_INDICATORS objIndicadores = new Clases.cKPI_INDICATORS())
            {
                objIndicadores.userid = Convert.ToInt32(User.ProviderUserKey);
                objIndicadores.titulo = nombre;
                System.Data.DataTableReader dtrDatos = objIndicadores.ObtenerDatosBuscador(string.Empty).CreateDataReader();

                while (dtrDatos.Read())
                {
                    JSonClases.KPI_INDICATORS objJSonFilters = new JSonClases.KPI_INDICATORS();

                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("INDICATORID")))
                        objJSonFilters.indicatorid = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("INDICATORID")));
                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("TITULO")))
                        objJSonFilters.titulo = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("TITULO")));

                    sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonFilters);
                    sComa = ", ";
                }
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return string.Empty;
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerResumenDeWidget
    //Llamador: /custom/graphics.js
    //Descripción: Obtiene el resumen de datos de un widget
    [WebMethod]
    public static string ObtenerResumenDeWidget(int widgetid)
    {
        JSonClases.KPI_RESUMEN objJSonResumen = new JSonClases.KPI_RESUMEN();

        using (Clases.cKPI_WIDGETS objWidget = new Clases.cKPI_WIDGETS())
        {
            objWidget.idwidget = widgetid;
            System.Data.DataTableReader dtrDatos = objWidget.ObtenerResumen(true).CreateDataReader();
            while (dtrDatos.Read())
            {
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("CAMPO")))
                {
                    switch (Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("CAMPO"))))
                    {
                        case "USERID":
                            if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("RESULTADO")))
                                objJSonResumen.usuariosAll = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("RESULTADO")));
                            break;
                        case "DATASETID":
                            if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("RESULTADO")))
                                objJSonResumen.datasetsAll = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("RESULTADO")));
                            break;
                        case "VALORID":
                            if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("RESULTADO")))
                                objJSonResumen.datosAll = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("RESULTADO")));
                            break;
                    }
                }
            }
            dtrDatos.Close();

            System.Data.DataTableReader dtrDatosf = objWidget.ObtenerResumen(false).CreateDataReader();
            while (dtrDatosf.Read())
            {
                if (!dtrDatosf.IsDBNull(dtrDatosf.GetOrdinal("CAMPO")))
                {
                    switch (Convert.ToString(dtrDatosf.GetValue(dtrDatosf.GetOrdinal("CAMPO"))))
                    {
                        case "USERID":
                            if ((!dtrDatosf.IsDBNull(dtrDatosf.GetOrdinal("RESULTADO"))) && (objJSonResumen.usuariosAll > 0))
                            {
                                objJSonResumen.usuariosV = Convert.ToInt32(dtrDatosf.GetValue(dtrDatosf.GetOrdinal("RESULTADO")));
                                objJSonResumen.usuariosP = (int)(100 * Convert.ToInt32(dtrDatosf.GetValue(dtrDatosf.GetOrdinal("RESULTADO"))) / objJSonResumen.usuariosAll);
                            }
                            break;
                        case "DATASETID":
                            if ((!dtrDatosf.IsDBNull(dtrDatosf.GetOrdinal("RESULTADO"))) && (objJSonResumen.datasetsAll > 0))
                            {
                                objJSonResumen.datasetsV = Convert.ToInt32(dtrDatosf.GetValue(dtrDatosf.GetOrdinal("RESULTADO")));
                                objJSonResumen.datasetsP = (int)(100 * Convert.ToInt32(dtrDatosf.GetValue(dtrDatosf.GetOrdinal("RESULTADO"))) / objJSonResumen.datasetsAll);
                            }
                            break;
                        case "VALORID":
                            if ((!dtrDatosf.IsDBNull(dtrDatosf.GetOrdinal("RESULTADO"))) && (objJSonResumen.datosAll > 0))
                            {
                                objJSonResumen.datosV = Convert.ToInt32(dtrDatosf.GetValue(dtrDatosf.GetOrdinal("RESULTADO")));
                                objJSonResumen.datosP = (int)(100 * Convert.ToInt32(dtrDatosf.GetValue(dtrDatosf.GetOrdinal("RESULTADO"))) / objJSonResumen.datosAll);
                            }
                            break;
                    }
                }
            }
            dtrDatosf.Close();
        }
        return JsonConvert.SerializeObject(objJSonResumen);
    }

    //Nombre: IndicatorLineChart
    //Llamador: /custom/indicator.js
    //Descripción: Obtiene los datos para representar en la gráfica de la página de un indicador
    [WebMethod]
    public static string ObtenerIndicatorLineChart(int indicatorid)
    {
        System.Data.DataTable dtDatos = null;
        DataTable dtResult = new DataTable();

        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_DATASET_VALUES objValores = new Clases.cKPI_DATASET_VALUES())
            {
                objValores.indicatorid = indicatorid;
                objValores.userid = Convert.ToInt32(User.ProviderUserKey);
                dtDatos = objValores.ObtenerGraficoIndicador();

                dtResult = SystemDataTableConverter.Convert(dtDatos);
                return dtResult.GetJson();
            }
        }
        return JsonConvert.SerializeObject(string.Empty);
    }

    //Nombre: IndicatorBarChart
    //Llamador: /custom/indicator.js
    //Descripción: Obtiene los datos para representar en la gráfica de la página de un indicador
    [WebMethod]
    public static string ObtenerIndicatorBarChart(int indicatorid)
    {
        System.Data.DataTable dtDatos = null;
        DataTable dtResult = new DataTable();

        using (Clases.cKPI_DATASET_VALUES objValores = new Clases.cKPI_DATASET_VALUES())
        {
            objValores.indicatorid = indicatorid;
            dtDatos = objValores.ObtenerDatosPorEjercicio();

            dtResult = SystemDataTableConverter.Convert(dtDatos);
            return dtResult.GetJson();
        }
    }

    //Nombre: ObtenerDatasetLastChart
    //Llamador: /custom/indicator.js
    //Descripción: Obtiene los últimos datos de un dataset para representar en la gráfica de la página de un indicador
    [WebMethod]
    public static string ObtenerDatasetLastChart(int datasetid)
    {
        System.Data.DataTable dtDatos = null;
        StringBuilder sJSON = new StringBuilder();

        using (Clases.cKPI_DATASET_VALUES objValores = new Clases.cKPI_DATASET_VALUES())
        {
            objValores.datasetid = datasetid;
            dtDatos = objValores.ObtenerUltimosDatos(10);

            if (dtDatos != null)
            {
                Boolean bEsPrimero = true;
                sJSON.AppendLine("[");
                System.Data.DataTableReader dtrNodos = dtDatos.CreateDataReader();
                while (dtrNodos.Read())
                {
                    if (!bEsPrimero) sJSON.AppendLine(",");
                    bEsPrimero = false;

                    sJSON.AppendLine("{ \"valor\": " + dtrNodos.GetValue(dtrNodos.GetOrdinal("VALOR")).ToString() + " }");
                }
                sJSON.AppendLine("]");
                return sJSON.ToString();
            }
        }
        return string.Empty;
    }

    //Nombre: ObtenerIndicadores
    //Llamador: /objects/kpiboard.indicator.js
    //Descripción: Obtiene los indicadores buscados
    [WebMethod]
    public static string ObtenerIndicadores(string titulo, int categoryid, int subcategoryid, int pageSize, int currentPage, string orderby)
    {
        string sJSON = string.Empty;

        using (Clases.cKPI_INDICATORS objIndicadores = new Clases.cKPI_INDICATORS())
        {
            System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
            if (usr != null)
            {
                objIndicadores.userid = Convert.ToInt32(usr.ProviderUserKey);
            }
            objIndicadores.titulo = titulo;
            objIndicadores.publicado = true;
            if (categoryid >= 0)
                objIndicadores.categoryid = categoryid;
            if (subcategoryid >= 0)
                objIndicadores.subcategoryid = subcategoryid;
            System.Data.DataTable TablaDeResultados = objIndicadores.BuscarIndicadores(pageSize, currentPage, orderby);

            if (TablaDeResultados != null)
                sJSON = JsonConvert.SerializeObject(TablaDeResultados);
            else
                sJSON = JsonConvert.SerializeObject(string.Empty);
        }
        return sJSON;
    }

    //Nombre: ObtenerUsuarios
    //Llamador: /objects/kpiboard.user.js
    //Descripción: Obtiene los usuarios de cada indicador
    [WebMethod]
    public static string ObtenerUsuarios(int indicatorid, string nombre, int pageSize, int currentPage, string orderby)
    {
        string sJSON = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return JsonConvert.SerializeObject(string.Empty);
        }

        using (Clases.cKPI_INDICATOR_USERS objUsuarios = new Clases.cKPI_INDICATOR_USERS())
        {
            objUsuarios.userid = Convert.ToInt32(usr.ProviderUserKey);
            objUsuarios.indicatorid = indicatorid;
            objUsuarios.nombre = nombre;
            System.Data.DataTable TablaDeResultados = objUsuarios.OtherUsers(pageSize, currentPage, "", orderby);

            if (TablaDeResultados != null)
                sJSON = JsonConvert.SerializeObject(TablaDeResultados);
            else
                sJSON = JsonConvert.SerializeObject(string.Empty);
        }
        return sJSON;
    }

    //Nombre: ObtenerAtributos
    //Llamador: /objects/kpiboard.dimension.js
    //Descripción: Obtiene los atributos de cada indicador y usuario
    [WebMethod]
    public static string ObtenerAtributos(int indicatorid, string nombre, int pageSize, int currentPage, string orderby)
    {
        string sJSON = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return JsonConvert.SerializeObject(string.Empty);
        }

        using (Clases.cKPI_DIMENSIONS objDimension = new Clases.cKPI_DIMENSIONS())
        {
            objDimension.indicatorid = indicatorid;
            objDimension.nombre = nombre;
            System.Data.DataTable TablaDeResultados = objDimension.ObtenerDatos(pageSize, currentPage, "", orderby);

            if (TablaDeResultados != null)
                sJSON = JsonConvert.SerializeObject(TablaDeResultados);
            else
                sJSON = JsonConvert.SerializeObject(string.Empty);
        }
        return sJSON;
    }

    //Nombre: ObtenerDatasets
    //Llamador: /custom/datos.js
    //Descripción: Obtiene los datos de los dataset de cada indicador y usuario
    [WebMethod]
    public static string ObtenerDatasets(int indicatorid, string nombre, int pageSize, int currentPage, string orderby)
    {
        string sJSON = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return JsonConvert.SerializeObject(string.Empty);
        }

        using (Clases.cKPI_DATASETS objDataSet = new Clases.cKPI_DATASETS())
        {
            objDataSet.userid = Convert.ToInt32(usr.ProviderUserKey);
            objDataSet.indicatorid = indicatorid;
            objDataSet.nombre = nombre;
            System.Data.DataTable TablaDeResultados = objDataSet.ObtenerDatos(pageSize, currentPage, "", orderby);

            if (TablaDeResultados != null)
                sJSON = JsonConvert.SerializeObject(TablaDeResultados);
            else
                sJSON = JsonConvert.SerializeObject(string.Empty);
        }
        return sJSON;
    }

    //Nombre: ObtenerImports
    //Llamador: /custom/datos.js
    //Descripción: Obtiene los datos de las importaciones de cada indicador y usuario
    [WebMethod]
    public static string ObtenerImports(int indicatorid, string nombre, int pageSize, int currentPage, string orderby)
    {
        string sJSON = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return JsonConvert.SerializeObject(string.Empty);
        }

        using (Clases.cKPI_IMPORTS objImport = new Clases.cKPI_IMPORTS())
        {
            objImport.userid = Convert.ToInt32(usr.ProviderUserKey);
            objImport.indicatorid = indicatorid;
            objImport.nombre = nombre;
            System.Data.DataTable TablaDeResultados = objImport.ObtenerDatos(pageSize, currentPage, "", orderby);

            if (TablaDeResultados != null)
                sJSON = JsonConvert.SerializeObject(TablaDeResultados);
            else
                sJSON = JsonConvert.SerializeObject(string.Empty);
        }
        return sJSON;
    }

    //Nombre: ObtenerDatasetValues
    //Llamador: /custom/datos.js
    //Descripción: Obtiene los datos de un dataset
    [WebMethod]
    public static string ObtenerDatasetValues(int datasetid, int pini, int pfin)
    {
        string sJSON = string.Empty;
        Clases.ITemporal objDatos = null;

        using (Clases.cKPI_DATASETS objDataSet = new Clases.cKPI_DATASETS())
        {
            objDataSet.datasetid = datasetid;
            if (objDataSet.bConsultar())
            {
                switch (objDataSet.dimension)
                {
                    case "D":
                        objDatos = new Clases.cKPI_DIAS();
                        break;
                    case "s":
                        objDatos = new Clases.cKPI_SEMANAS();
                        break;
                    case "Q":
                        objDatos = new Clases.cKPI_QUINCENAS();
                        break;
                    case "M":
                        objDatos = new Clases.cKPI_MESES();
                        break;
                    case "T":
                        objDatos = new Clases.cKPI_TRIMESTRES();
                        break;
                    case "S":
                        objDatos = new Clases.cKPI_SEMESTRES();
                        break;
                    case "A":
                        objDatos = new Clases.cKPI_EJERCICIOS();
                        break;
                    case "L":
                        objDatos = new Clases.cKPI_LIBRE();
                        break;
                }

                if (objDatos != null)
                {
                    try
                    {
                        objDatos.datasetid = objDataSet.datasetid;
                        System.Data.DataTable TablaDeDatos = objDatos.ObtenerValores(pini, pfin);

                        if (TablaDeDatos != null)
                            sJSON = JsonConvert.SerializeObject(TablaDeDatos);
                        else
                            sJSON = JsonConvert.SerializeObject(string.Empty);
                    }
                    catch
                    {
                        objDatos.userid = -1;
                    }
                }
            }
        }
        return sJSON;
    }

    //Nombre: ObtenerSubcategorias
    //Llamador: /custom/proxy.js
    //Descripción: Obtiene las diferentes subcategorías encontradas dentro de una categoría determinada
    [WebMethod]
    public static string ObtenerSubcategorias(int categoryid, string filterBy, int pageSize, int currentPage, string orderby)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            using (Clases.cKPI_SUBCATEGORIES objSubcategorias = new Clases.cKPI_SUBCATEGORIES())
            {
                objSubcategorias.categoryid = categoryid;
                System.Data.DataTableReader dtrDatos = objSubcategorias.ObtenerDatos(10, 1, filterBy, string.Empty).CreateDataReader();

                while (dtrDatos.Read())
                {
                    JSonClases.KPI_SHORTLIST objJSonFriends = new JSonClases.KPI_SHORTLIST();

                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("SUBCATEGORYID")))
                        objJSonFriends.id = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("SUBCATEGORYID")));
                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("NOMBRE")))
                        objJSonFriends.name = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("NOMBRE")));
                    else
                        objJSonFriends.name = "--Undefined--";
                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("RECUENTO")))
                        objJSonFriends.value = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("RECUENTO")));

                    sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonFriends);
                    sComa = ", ";
                }
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return JsonConvert.SerializeObject(string.Empty);
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerAmistades
    //Llamador: /objects/kpiboard.friendship.js
    //Descripción: Obtiene los atributos de cada indicador y usuario
    [WebMethod]
    public static string ObtenerAmistades(string aceptado, int pageSize, int currentPage, string orderby)
    {
        string sJSON = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return JsonConvert.SerializeObject(string.Empty);
        }

        using (Clases.cASPNET_FRIENDSHIP objAmigos = new Clases.cASPNET_FRIENDSHIP())
        {
            if (!string.IsNullOrEmpty(aceptado))
            {
                if (aceptado == "N")
                    objAmigos.aceptado = false;
                else
                    objAmigos.aceptado = true;
            }
            objAmigos.fromuserid = Convert.ToInt32(usr.ProviderUserKey);
            System.Data.DataTable TablaDeResultados = objAmigos.ObtenerDatos(string.Empty, pageSize, currentPage, orderby);

            if (TablaDeResultados != null)
                sJSON = JsonConvert.SerializeObject(TablaDeResultados);
            else
                sJSON = JsonConvert.SerializeObject(string.Empty);
        }
        return sJSON;
    }

    //Nombre: ObtenerAmigosXIndicador
    //Llamador: /objects/kpiboard.friendship.js
    //Descripción: Obtiene las amistades comunes para un determinado indicador
    [WebMethod]
    public static string ObtenerListadoAmigos(int indicatorid, string aceptado, string filterBy, int pageSize, int currentPage, string orderby)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            using (Clases.cASPNET_FRIENDSHIP objAmigos = new Clases.cASPNET_FRIENDSHIP())
            {
                objAmigos.fromuserid = Convert.ToInt32(usr.ProviderUserKey);
                if (aceptado == "S")
                    objAmigos.aceptado = true;
                else
                    objAmigos.aceptado = false;
                System.Data.DataTableReader dtrDatos = objAmigos.CommonUsers(10, 1, indicatorid, filterBy, string.Empty).CreateDataReader();

                while (dtrDatos.Read())
                {
                    JSonClases.KPI_SHORTLIST objJSonFriends = new JSonClases.KPI_SHORTLIST();

                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("USERID")))
                        objJSonFriends.id = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("USERID")));
                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("NOMBRE")) && !dtrDatos.IsDBNull(dtrDatos.GetOrdinal("APELLIDOS")))
                        objJSonFriends.name = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("NOMBRE"))) + ' ' + Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("APELLIDOS")));
                    else if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("NOMBRE")))
                        objJSonFriends.name = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("NOMBRE")));
                    else
                        objJSonFriends.name = "--Undefined--";
                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IMAGEURL")))
                        objJSonFriends.imageurl = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("IMAGEURL")));

                    sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonFriends);
                    sComa = ", ";
                }
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return JsonConvert.SerializeObject(string.Empty);
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ObtenerFormulasXIndicador
    //Llamador: /objects/kpiboard.formula.js
    //Descripción: Obtiene las formulas para un determinado indicador
    [WebMethod]
    public static string ObtenerFormulasXIndicador(int indicatorid, string filterBy, int pageSize, int currentPage, string orderby)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            using (Clases.cKPI_WIDGET_EXPRESIONS objExpresiones = new Clases.cKPI_WIDGET_EXPRESIONS())
            {
                objExpresiones.indicatorid = indicatorid;
                System.Data.DataTableReader dtrDatos = objExpresiones.ObtenerOtrasFormulas(10, 1, filterBy, string.Empty).CreateDataReader();

                while (dtrDatos.Read())
                {
                    JSonClases.KPI_SHORTLIST objJSonFriends = new JSonClases.KPI_SHORTLIST();

                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IDFORMULA")))
                        objJSonFriends.id = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("IDFORMULA")));
                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("NOMBRE")))
                        objJSonFriends.name = Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("NOMBRE")));
                    else
                        objJSonFriends.name = "--Undefined--";
                    if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("CONTADOR")))
                        objJSonFriends.value = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("CONTADOR")));

                    sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonFriends);
                    sComa = ", ";
                }
            }
        }
        if (string.IsNullOrEmpty(sJSON))
            return JsonConvert.SerializeObject(string.Empty);
        else
            return "[" + sJSON + "]";
    }

    //Nombre: ResumeIndicator
    //Llamador: /custom/indicator.js
    //Descripción: Añade una nueva puntación al indicador actual
    [WebMethod]
    public static string ObtenerResumenDeIndicador(int indicatorid)
    {
        JSonClases.KPI_RESUMEN objJSonResumen = new JSonClases.KPI_RESUMEN();

        using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
        {
            objIndicador.indicatorid = indicatorid;
            System.Data.DataTableReader dtrDatos = objIndicador.ObtenerResumen().CreateDataReader();
            while (dtrDatos.Read())
            {
                if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("CAMPO")))
                {
                    switch (Convert.ToString(dtrDatos.GetValue(dtrDatos.GetOrdinal("CAMPO"))))
                    {
                        case "USERID":
                            if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("RESULTADO")))
                                objJSonResumen.usuariosAll = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("RESULTADO")));
                            break;
                        case "DATASETID":
                            if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("RESULTADO")))
                                objJSonResumen.datasetsAll = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("RESULTADO")));
                            break;
                        case "VALORID":
                            if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("RESULTADO")))
                                objJSonResumen.datosAll = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("RESULTADO")));
                            break;
                    }
                }
            }
            dtrDatos.Close();
        }

        return JsonConvert.SerializeObject(objJSonResumen);
    }

    //Nombre: GuardarDatasetValue
    //Llamador: /custom/datos.js
    //Descripción: Guarda los datos asociados a un dataset
    [WebMethod]
    public static int GuardarDatasetValue(int datasetid, string data)
    {
        data = data.Replace("\"\"", "null");
        System.Data.DataTable tester = (System.Data.DataTable)JsonConvert.DeserializeObject(data, (typeof(System.Data.DataTable)), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return -1;
        }

        using (Clases.cKPI_DATASETS objDataSet = new Clases.cKPI_DATASETS())
        {
            objDataSet.datasetid = datasetid;
            if (objDataSet.bConsultar())
            {
                using (Clases.cKPI_DATASET_VALUES objValor = new Clases.cKPI_DATASET_VALUES())
                {
                    foreach (System.Data.DataRow Fila in tester.Rows)
                    {
                        switch (objDataSet.dimension)
                        {
                            case "D":
                                objValor.datasetid = objDataSet.datasetid;
                                DateTime FechaActual;
                                try
                                {
                                    FechaActual = new DateTime(Convert.ToInt32(Fila["EJERCICIO"]), Convert.ToInt32(Fila["MES"]), Convert.ToInt32(Fila["DIA"]));
                                    objValor.fecha = FechaActual;
                                    try
                                    {
                                        objValor.valor = Convert.ToDecimal(Fila["VALOR"]);
                                        objValor.bGuardarLibre(0);
                                    }
                                    catch
                                    {
                                        objValor.bEliminarLibre();
                                    }
                                }
                                catch
                                {
                                    continue;
                                }
                                break;
                            case "s":
                                objValor.datasetid = objDataSet.datasetid;
                                objValor.ejercicio = Convert.ToInt32(Fila["EJERCICIO"]);
                                objValor.mes = Convert.ToInt32(Fila["MES"]);
                                objValor.semana = Convert.ToInt32(Fila["SEMANA"]);
                                try
                                {
                                    objValor.valor = Convert.ToDecimal(Fila["VALOR"]);
                                    objValor.bGuardarSemana(0);
                                }
                                catch
                                {
                                    objValor.bEliminarSemana();
                                }
                                break;
                            case "Q":
                                objValor.datasetid = objDataSet.datasetid;
                                objValor.ejercicio = Convert.ToInt32(Fila["EJERCICIO"]);
                                objValor.mes = Convert.ToInt32(Fila["MES"]);
                                objValor.quincena = Convert.ToInt32(Fila["QUINCENA"]);
                                try
                                {
                                    objValor.valor = Convert.ToDecimal(Fila["VALOR"]);
                                    objValor.bGuardarQuincena(0);
                                }
                                catch
                                {
                                    objValor.bEliminarQuincena();
                                }
                                break;
                            case "M":
                                objValor.datasetid = objDataSet.datasetid;
                                objValor.ejercicio = Convert.ToInt32(Fila["EJERCICIO"]);
                                objValor.mes = Convert.ToInt32(Fila["MES"]);
                                try
                                {
                                    objValor.valor = Convert.ToDecimal(Fila["VALOR"]);
                                    objValor.bGuardarMes(0);
                                }
                                catch
                                {
                                    objValor.bEliminarMes();
                                }
                                break;
                            case "T":
                                objValor.datasetid = objDataSet.datasetid;
                                objValor.ejercicio = Convert.ToInt32(Fila["EJERCICIO"]);
                                objValor.trimestre = Convert.ToInt32(Fila["TRIMESTRE"]);
                                try
                                {
                                    objValor.valor = Convert.ToDecimal(Fila["VALOR"]);
                                    objValor.bGuardarTrimestre(0);
                                }
                                catch
                                {
                                    objValor.bEliminarTrimestre();
                                }
                                break;
                            case "S":
                                objValor.datasetid = objDataSet.datasetid;
                                objValor.ejercicio = Convert.ToInt32(Fila["EJERCICIO"]);
                                objValor.semestre = Convert.ToInt32(Fila["SEMESTRE"]);
                                try
                                {
                                    objValor.valor = Convert.ToDecimal(Fila["VALOR"]);
                                    objValor.bGuardarSemestre(0);
                                }
                                catch
                                {
                                    objValor.bEliminarSemestre();
                                }
                                break;
                            case "A":
                                objValor.datasetid = objDataSet.datasetid;
                                objValor.ejercicio = Convert.ToInt32(Fila["EJERCICIO"]);
                                try
                                {
                                    objValor.valor = Convert.ToDecimal(Fila["VALOR"]);
                                    objValor.bGuardarEjercicio(0);
                                }
                                catch
                                {
                                    objValor.bEliminarEjercicio();
                                }
                                break;
                            case "L":
                                if (Fila["FECHA"] != null)
                                {
                                    objValor.datasetid = objDataSet.datasetid;
                                    objValor.fecha = null;
                                    objValor.valor = null;
                                    try
                                    {
                                        objValor.fecha = Convert.ToDateTime(Fila["FECHA"]);
                                        try
                                        {
                                            objValor.valor = Convert.ToDecimal(Fila["VALOR"]);
                                            objValor.bGuardarLibre(0);
                                        }
                                        catch
                                        {
                                            objValor.bEliminarLibre();
                                        }
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
        return 1;
    }

    //Nombre: PercentChart
    //Llamador: /custom/graphics.js
    //Descripción: Obtiene los datos para representar en la gráfica de un widget
    [WebMethod]
    public static string PercentChart(int widgetid, string psTipo, string pdDesde, string pdHasta)
    {
        System.Data.DataTable dtDatos = null;
        DataTable dtResult = new DataTable();

        using (Clases.cKPI_WIDGETS objWidget = new Clases.cKPI_WIDGETS())
        {
            objWidget.idwidget = widgetid;
            objWidget.grafico = psTipo;
            try
            {
                objWidget.fecha_inicio = Convert.ToDateTime(pdDesde);
            }
            catch
            {
                return string.Empty;
            }
            try
            {
                objWidget.fecha_fin = Convert.ToDateTime(pdHasta);
            }
            catch
            {
                return string.Empty;
            }
            dtDatos = objWidget.ObtenerPie();

            dtResult = SystemDataTableConverter.Convert(dtDatos);
            return dtResult.GetJson();
        }
    }

    //Nombre: TimeChart
    //Llamador: /custom/graphics.js
    //Descripción: Obtiene los datos para representar en la gráfica de un widget
    [WebMethod]
    public static string TimeChart(int widgetid, string pDimension, string psTipo, string pdDesde, string pdHasta)
    {
        System.Data.DataTable dtDatos = null;
        DataTable dtResult = new DataTable();

        using (Clases.cKPI_WIDGETS objWidget = new Clases.cKPI_WIDGETS())
        {
            objWidget.idwidget = widgetid;
            objWidget.dimension = pDimension;
            objWidget.grafico = psTipo;
            try
            {
                objWidget.fecha_inicio = Convert.ToDateTime(pdDesde);
            }
            catch
            {
                return string.Empty;
            }
            try
            {
                objWidget.fecha_fin = Convert.ToDateTime(pdHasta);
            }
            catch
            {
                return string.Empty;
            }
            dtDatos = objWidget.ObtenerLineaTemporal();

            dtResult = SystemDataTableConverter.Convert(dtDatos);

            return dtResult.GetJson();
        }
    }

    //Nombre: ExportDataset
    //Llamador: /objects/kpiboard.dataset.js
    //Descripción: Obtiene los datos de los dataset para su posterior exportación
    [WebMethod]
    public static string ExportDataset(int datasetid)
    {
        string sJSON = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return JsonConvert.SerializeObject(string.Empty);
        }

        using (Clases.cKPI_DATASETS objDataSet = new Clases.cKPI_DATASETS())
        {
            objDataSet.userid = Convert.ToInt32(usr.ProviderUserKey);
            objDataSet.datasetid = datasetid;
            System.Data.DataTable TablaDeResultados = objDataSet.ExportarDatos();

            if (TablaDeResultados != null)
                sJSON = JsonConvert.SerializeObject(TablaDeResultados);
            else
                sJSON = JsonConvert.SerializeObject(string.Empty);
        }
        return sJSON;
    }

    //Nombre: LoginFacebookUser
    //Llamador: /objects/kpiboard.user.js
    //Descripción: Intenta registrar el usuario con Facebook
    [WebMethod]
    public static string LoginFacebookUser(string userid, string email, string nombre, string apellidos)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            string currentUser = Membership.GetUserNameByEmail(email);
            if (!string.IsNullOrEmpty(currentUser))
            {
                FormsAuthentication.SetAuthCookie(currentUser, true);
                return currentUser;
            }
            else
            {
                string newPass = Membership.GeneratePassword(12, 1);
                MembershipUser newUser = Membership.CreateUser(email, newPass, email);
                if (newUser != null)
                {
                    using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
                    {
                        objUsuario.userid = Convert.ToInt32(newUser.ProviderUserKey);
                        objUsuario.nombre = nombre;
                        objUsuario.apellidos = apellidos;
                        objUsuario.imageurl = "https://graph.facebook.com/" + userid + "/picture?type=large";
                        if (objUsuario.bModificar())
                            return newUser.UserName;
                        else
                            return string.Empty;
                    }
                }
                else
                    return string.Empty;
            }
        }
        return usr.UserName;
    }

    /************************************************************************************************************************************/

    //Nombre: SelectWidget
    //Llamador: /custom/graphics.js
    //Descripción: Obtiene los datos de un determinado widget
    [WebMethod]
    public static string SelectWidget(int idwidget)
    {
        string sJSON = string.Empty;

        using (Clases.cKPI_WIDGETS objWidget = new Clases.cKPI_WIDGETS())
        {
            objWidget.idwidget = idwidget;
            if (objWidget.bConsultar())
            {
                JSonClases.KPI_WIDGET objJSonWidget = new JSonClases.KPI_WIDGET();

                objJSonWidget.iddashboard = objWidget.iddashboard.Value;
                objJSonWidget.idwidget = objWidget.idwidget.Value;
                objJSonWidget.titulo = objWidget.titulo;
                if (objWidget.fecha_inicio.HasValue)
                    objJSonWidget.fecha_inicio = objWidget.fecha_inicio.Value.ToShortDateString();
                if (objWidget.fecha_fin.HasValue)
                    objJSonWidget.fecha_fin = objWidget.fecha_fin.Value.ToShortDateString();
                objJSonWidget.grafico = objWidget.grafico;
                objJSonWidget.dimension = objWidget.dimension;

                sJSON = JsonConvert.SerializeObject(objJSonWidget);
            }
            else
                sJSON = JsonConvert.SerializeObject(string.Empty);
        }
        return sJSON;
    }

    //Nombre: SelectFormula
    //Llamador: /custom/graphics.js
    //Descripción: Consulta la fórmula especificada por parámetro
    [WebMethod]
    public static string SelectFormula(int idformula)
    {
        using (Clases.cKPI_WIDGET_FORMULAS objFormula = new Clases.cKPI_WIDGET_FORMULAS())
        {
            objFormula.idformula = idformula;
            if (objFormula.bConsultar())
            {
                JSonClases.KPI_WIDGET_FORMULAS objJSonFormula = new JSonClases.KPI_WIDGET_FORMULAS();
                objJSonFormula.formulaid = objFormula.idformula.Value;
                objJSonFormula.widgetid = objFormula.idwidget.Value;
                objJSonFormula.nombre = objFormula.nombre;
                objJSonFormula.color = objFormula.color;
                objJSonFormula.formula = objFormula.formula;
                objJSonFormula.original = objFormula.original;
                if (objFormula.fecha.HasValue)
                    objJSonFormula.fecha = objFormula.fecha.Value.ToString("dd MMMM, yyyy", new System.Globalization.CultureInfo("en-US"));
                if (objFormula.validado)
                    objJSonFormula.validated = 0;
                else
                    objJSonFormula.validated = -1;
                objJSonFormula.display = objFormula.display;
                return JsonConvert.SerializeObject(objJSonFormula);
            }
        }
        return string.Empty;
    }

    //Nombre: SelectFiltro
    //Llamador: /custom/graphics.js
    //Descripción: Consulta el filtro especificado por parámetro
    [WebMethod]
    public static string SelectFiltro(int filtroid)
    {
        using (Clases.cKPI_WIDGET_FILTERS objFiltro = new Clases.cKPI_WIDGET_FILTERS())
        {
            objFiltro.idfilter = filtroid;
            if (objFiltro.bConsultar())
            {
                JSonClases.KPI_WIDGET_FILTERS objJSonFiltro = new JSonClases.KPI_WIDGET_FILTERS();
                objJSonFiltro.filterid = objFiltro.idfilter.Value;
                objJSonFiltro.dimensionid = objFiltro.dimensionid.Value;
                objJSonFiltro.filtro = objFiltro.filtro;
                objJSonFiltro.valor = objFiltro.valor;
                return JsonConvert.SerializeObject(objJSonFiltro);
            }
        }
        return string.Empty;
    }

    //Nombre: SelectIndicator
    //Llamador: /custom/graphics.js
    //Descripción: Consulta el filtro especificado por parámetro
    [WebMethod]
    public static string SelectIndicator(int indicatorid)
    {
        using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
        {
            objIndicador.indicatorid = indicatorid;
            if (objIndicador.bConsultar())
            {
                JSonClases.KPI_INDICATORS objJSonIndicador = new JSonClases.KPI_INDICATORS();
                objJSonIndicador.indicatorid = objIndicador.indicatorid.Value;
                objJSonIndicador.titulo = objIndicador.titulo;
                if (objIndicador.categoryid.HasValue)
                    objJSonIndicador.categoryid = objIndicador.categoryid.Value;
                else
                    objJSonIndicador.categoryid = -1;
                if (objIndicador.subcategoryid.HasValue)
                    objJSonIndicador.subcategoryid = objIndicador.subcategoryid.Value;
                else
                    objJSonIndicador.subcategoryid = -1;
                objJSonIndicador.imageurl = objIndicador.imageurl;
                return JsonConvert.SerializeObject(objJSonIndicador);
            }
        }
        return string.Empty;
    }

    //Nombre: SelectAtributte
    //Llamador: /custom/pages/loaddata.js
    //Descripción: Consulta el atributo especificado por parámetro
    [WebMethod]
    public static string SelectAtributte(int dimensionid)
    {
        using (Clases.cKPI_DIMENSIONS objAtributo = new Clases.cKPI_DIMENSIONS())
        {
            objAtributo.dimensionid = dimensionid;
            if (objAtributo.bConsultar())
            {
                JSonClases.KPI_DIMENSIONS objJSonAtributo = new JSonClases.KPI_DIMENSIONS();
                objJSonAtributo.indicatorid = objAtributo.indicatorid.Value;
                objJSonAtributo.id = objAtributo.dimensionid.Value;
                objJSonAtributo.nombre = objAtributo.nombre;
                return JsonConvert.SerializeObject(objJSonAtributo);
            }
        }
        return string.Empty;
    }

    //Nombre: SelectUser
    //Llamador: /generales/menusecundario.js
    //Descripción: Consula el usuario especificado por parámetros
    [WebMethod]
    public static string SelectUser(int userid)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
            {
                objUsuario.userid = userid;
                if (objUsuario.bConsultar())
                {
                    JSonClases.ASPNET_INFO_USUARIO objJSonUsuario = new JSonClases.ASPNET_INFO_USUARIO();
                    objJSonUsuario.userid = objUsuario.userid.Value;
                    objJSonUsuario.nombre = objUsuario.nombre;
                    objJSonUsuario.apellidos = objUsuario.apellidos;
                    objJSonUsuario.resumen = objUsuario.resumen;
                    objJSonUsuario.imageurl = objUsuario.imageurl;
                    using (Clases.cKPI_INDICATOR_USERS objIndicadorUsuario = new Clases.cKPI_INDICATOR_USERS())
                    {
                        objIndicadorUsuario.userid = userid;
                        objJSonUsuario.formulas = objIndicadorUsuario.nRecuentoFormulas();
                        System.Data.DataTableReader dtrDatos = objIndicadorUsuario.CommonIndicators(6, 1, " AND A.USERID = " + User.ProviderUserKey.ToString(), string.Empty).CreateDataReader();
                        while (dtrDatos.Read())
                        {
                            JSonClases.KPI_INDICATORS objJSonIndicador = new JSonClases.KPI_INDICATORS();
                            if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("INDICATORID")))
                            {
                                objJSonIndicador.indicatorid = Convert.ToInt32(dtrDatos.GetValue(dtrDatos.GetOrdinal("INDICATORID")));
                            }
                            if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("TITULO")))
                            {
                                objJSonIndicador.titulo = dtrDatos.GetValue(dtrDatos.GetOrdinal("TITULO")).ToString();
                            }
                            if (!dtrDatos.IsDBNull(dtrDatos.GetOrdinal("IMAGEURL")))
                            {
                                objJSonIndicador.imageurl = dtrDatos.GetValue(dtrDatos.GetOrdinal("IMAGEURL")).ToString();
                            }
                            objJSonUsuario.indicadores.Add(objJSonIndicador);
                        }
                    }
                    using (Clases.cKPI_INDICATORS objIndicadores = new Clases.cKPI_INDICATORS())
                    {
                        objIndicadores.userid = userid;
                        objIndicadores.compartido = true;
                        objJSonUsuario.shared = objIndicadores.NumeroIndicadores();
                    }
                    using (Clases.cASPNET_FRIENDSHIP objFriends = new Clases.cASPNET_FRIENDSHIP())
                    {
                        //Averiguamos cuantos amigos tiene
                        objFriends.fromuserid = Convert.ToInt32(User.ProviderUserKey);
                        objFriends.aceptado = true;
                        objJSonUsuario.friends = objFriends.nRecuento();
                        //Comprobamos la relación actual entre ellos
                        objFriends.Inicializar();
                        objFriends.touserid = userid;
                        objFriends.fromuserid = Convert.ToInt32(User.ProviderUserKey);
                        if (objFriends.bConsultar())
                        {
                            if (objFriends.aceptado == true)
                                objJSonUsuario.situacion = 2;
                            else
                            {
                                if (objFriends.relacion == 1)
                                    objJSonUsuario.situacion = 2;
                                else
                                    objJSonUsuario.situacion = 1;
                            }
                        }
                        else
                            objJSonUsuario.situacion = 0;
                    }
                    return JsonConvert.SerializeObject(objJSonUsuario);
                }
            }
        }
        return JsonConvert.SerializeObject(string.Empty);
    }

    //Nombre: AsignIndicator
    //Llamador: /search.aspx
    //Descripción: Asigna el indicador al usuario actual
    [WebMethod]
    public static int AsignIndicator(int indicatorid)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return -1;
        }

        using (Clases.cKPI_INDICATOR_USERS objIndicadorUser = new Clases.cKPI_INDICATOR_USERS())
        {
            objIndicadorUser.indicatorid = indicatorid;
            objIndicadorUser.userid = Convert.ToInt32(usr.ProviderUserKey);
            if (objIndicadorUser.bInsertar())
                return 1;
        }

        return -1;
    }

    //Nombre: ShareIndicator
    //Llamador: kpiboard.indicator.js
    //Descripción: Comparte el indicador con el resto de usuarios
    [WebMethod]
    public static int ShareIndicator(int indicatorid, string imageurl, int categoryid, int subcategoryid, string nombre)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return -1;
        }

        using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
        {
            objIndicador.indicatorid = indicatorid;
            objIndicador.usuario = usr.UserName;
            objIndicador.imageurl = imageurl;
            objIndicador.categoryid = categoryid;
            objIndicador.subcategoryid = subcategoryid;
            if (objIndicador.bCompartir(nombre))
                return 1;
        }
        return -1;
    }

    //Nombre: VoteIndicator
    //Llamador: /custom/indicator.js
    //Descripción: Añade una nueva puntación al indicador actual
    [WebMethod]
    public static int VoteIndicator(int indicatorid, int score)
    {
        int nResultado = 0;

        using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
        {
            objIndicador.indicatorid = indicatorid;
            nResultado = objIndicador.bModificarRanking(score);
        }

        return nResultado;
    }

    /************************************************************************************************************************************/

    //Nombre: InsertFilter
    //Llamador: /custom/graphics.js
    //Descripción: Añade un nuevo filtro a la dimensión especificada
    [WebMethod]
    public static string InsertFilter(int idexpresion, int dimensionid, string filtro, string valor)
    {
        using (Clases.cKPI_WIDGET_FILTERS objFiltro = new Clases.cKPI_WIDGET_FILTERS())
        {
            objFiltro.idexpresion = idexpresion;
            objFiltro.dimensionid = dimensionid;
            objFiltro.filtro = filtro;
            objFiltro.valor = valor;
            if (objFiltro.bInsertar())
            {
                JSonClases.KPI_WIDGET_FILTERS objJSonFilters = new JSonClases.KPI_WIDGET_FILTERS();

                objJSonFilters.filterid = objFiltro.idfilter.Value;
                objJSonFilters.expresionid = objFiltro.idexpresion.Value;
                objJSonFilters.dimensionid = objFiltro.dimensionid.Value;
                objJSonFilters.filtro = objFiltro.filtro;
                objJSonFilters.valor = objFiltro.valor;

                return JsonConvert.SerializeObject(objJSonFilters);
            }
        }

        return JsonConvert.SerializeObject(string.Empty);
    }

    //Nombre: InsertFormula
    //Llamador: /custom/graphics.js
    //Descripción: Inserta una nueva fórmula desde cero
    [WebMethod]
    public static string InsertFormula(int idwidget, string nombre, string formula, string color)
    {
        using (Clases.cKPI_WIDGET_FORMULAS objNewFormula = new Clases.cKPI_WIDGET_FORMULAS())
        {
            objNewFormula.idwidget = idwidget;
            objNewFormula.nombre = nombre;
            objNewFormula.formula = formula;
            objNewFormula.color = color;
            if (objNewFormula.bInsertar())
            {
                JSonClases.KPI_WIDGET_FORMULAS objJSonFormula = new JSonClases.KPI_WIDGET_FORMULAS();

                objJSonFormula.formulaid = objNewFormula.idformula.Value;
                objJSonFormula.widgetid = objNewFormula.idwidget.Value;
                objJSonFormula.nombre = objNewFormula.nombre;
                objJSonFormula.formula = objNewFormula.formula;
                objJSonFormula.color = objNewFormula.color;

                return JsonConvert.SerializeObject(objJSonFormula);
            }
        }
        return string.Empty;
    }

    //Nombre: InsertIndicator
    //Llamador: /objects/kpiboard.indicator.js
    //Descripción: Inserta un nuevo indicador desde el asistente
    [WebMethod]
    public static int InsertIndicator(string titulo, string resumen, string unidad, string simbolo, string funcion, string atributos)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
            {
                objIndicador.usuario = User.UserName;
                objIndicador.userid = Convert.ToInt32(User.ProviderUserKey);
                objIndicador.titulo = titulo;
                if (string.IsNullOrEmpty(resumen))
                    objIndicador.resumen = " - sin descripción - ";
                else
                    objIndicador.resumen = resumen;
                objIndicador.descripcion = string.Empty;
                objIndicador.unidad = unidad;
                objIndicador.simbolo = simbolo;
                objIndicador.funcion_agregada = funcion;
                objIndicador.imageurl = "/images/noimage.png";
                objIndicador.compartido = false;
                objIndicador.es_de_sistema = false;
                objIndicador.publicado = true;
                objIndicador.categoryid = 1;

                if (objIndicador.bInsertar())
                {
                    List<JSonClases.KPI_DIMENSIONS> jsonDimensions = (List<JSonClases.KPI_DIMENSIONS>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<JSonClases.KPI_DIMENSIONS>>(atributos);
                    using (Clases.cKPI_DIMENSIONS objAtributo = new Clases.cKPI_DIMENSIONS())
                    {
                        foreach (JSonClases.KPI_DIMENSIONS iAtributo in jsonDimensions)
                        {
                            objAtributo.nombre = iAtributo.nombre;
                            objAtributo.descripcion = iAtributo.value;
                            objAtributo.userid = Convert.ToInt32(User.ProviderUserKey);
                            objAtributo.indicatorid = objIndicador.indicatorid.Value;
                            objAtributo.bInsertar();
                        }
                    }
                    return objIndicador.indicatorid.Value;
                }
            }
        }
        return -1;
    }

    //Nombre: InsertDashboard
    //Llamador: /custom/graphics.js
    //Descripción: Crea un nuevo dashboard y lo asocia al usuario al final de la lista
    [WebMethod]
    public static string InsertDashboard(string titulo)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_DASHBOARDS objNewDashboard = new Clases.cKPI_DASHBOARDS())
            {
                objNewDashboard.userid = Convert.ToInt32(User.ProviderUserKey);
                objNewDashboard.titulo = titulo;
                if (objNewDashboard.bInsertar())
                {
                    JSonClases.KPI_DASHBOARD objJSonDashboard = new JSonClases.KPI_DASHBOARD();

                    objJSonDashboard.dashboardid = objNewDashboard.iddashboard.Value;
                    objJSonDashboard.titulo = objNewDashboard.titulo;

                    return JsonConvert.SerializeObject(objJSonDashboard);
                }
            }
        }
        return JsonConvert.SerializeObject(string.Empty);
    }

    //Nombre: InsertWidget
    //Llamador: /custom/graphics.js
    //Descripción: Inserta un nuevo widget al dashboard especificado
    [WebMethod]
    public static string InsertWidget(int iddashboard, string titulo)
    {
        using (Clases.cKPI_WIDGETS objNewWidget = new Clases.cKPI_WIDGETS())
        {
            objNewWidget.iddashboard = iddashboard;
            objNewWidget.titulo = titulo;
            if (objNewWidget.bInsertar())
            {
                JSonClases.KPI_WIDGET objJSonWidget = new JSonClases.KPI_WIDGET();

                objJSonWidget.iddashboard = objNewWidget.iddashboard.Value;
                objJSonWidget.idwidget = objNewWidget.idwidget.Value;
                objJSonWidget.titulo = objNewWidget.titulo;

                return JsonConvert.SerializeObject(objJSonWidget);
            }
        }
        return string.Empty;
    }

    //Nombre: InsertDataset
    //Llamador: /custom/objects/kpiboard.dataset.js
    //Descripción: Añade un nuevo dataset vacío al indicador actual
    [WebMethod]
    public static int InsertDataset(int indicatorid, string nombre, string dimension)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            if (!string.IsNullOrEmpty(nombre))
            {
                using (Clases.cKPI_DATASETS objDataSet = new Clases.cKPI_DATASETS())
                {
                    objDataSet.indicatorid = indicatorid;
                    objDataSet.userid = Convert.ToInt32(User.ProviderUserKey);
                    objDataSet.nombre = nombre;
                    objDataSet.dimension = dimension;
                    if (objDataSet.bInsertar())
                    {
                        return objDataSet.datasetid.Value;
                    }
                }
            }
        }
        return -1;
    }

    //Nombre: InsertEtiqueta
    //Llamador: /custom/indicator.js
    //Descripción: Añade una nueva etiqueta al indicador actual para facilitar su búsqueda
    [WebMethod]
    public static int InsertEtiqueta(int indicatorid, string etiqueta)
    {
        Boolean bResultado = false;

        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_INDICATOR_ETIQUETAS objEtiqueta = new Clases.cKPI_INDICATOR_ETIQUETAS())
            {
                objEtiqueta.indicatorid = indicatorid;
                objEtiqueta.userid = Convert.ToInt32(User.ProviderUserKey);
                bResultado = objEtiqueta.bInsertar(etiqueta);
            }
        }

        if (bResultado)
            return 1;
        else
            return -1;
    }

    //Nombre: InsertFollower
    //Llamador: /custom/generales/menusecundario.js
    //Descripción: Inserta una nueva relación de seguimiento hacia otro usuario
    [WebMethod]
    public static int InsertFollower(int touserid)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cASPNET_FRIENDSHIP objNewFollower = new Clases.cASPNET_FRIENDSHIP())
            {
                objNewFollower.fromuserid = Convert.ToInt32(User.ProviderUserKey);
                objNewFollower.touserid = touserid;
                if (objNewFollower.bInsertar())
                    return 0;
                else
                    return -1;
            }
        }
        return -1;
    }

    //Nombre: AcceptFollower
    //Llamador: /custom/objects/kpiboard.friendship.js
    //Descripción: Acepta una solicitud de amistad de otro usuario
    [WebMethod]
    public static int AcceptFollower(int fromuserid)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cASPNET_FRIENDSHIP objNewFollower = new Clases.cASPNET_FRIENDSHIP())
            {
                objNewFollower.fromuserid = fromuserid;
                objNewFollower.touserid = Convert.ToInt32(User.ProviderUserKey);
                objNewFollower.aceptado = true;
                if (objNewFollower.bModificar())
                    return 0;
                else
                    return -1;
            }
        }
        return -1;
    }

    //Nombre: InsertDimension
    //Llamador: /custom/datos.js
    //Descripción: Crea una nueva dimensión para el conjunto de datos especificado en el parámetro
    [WebMethod]
    public static int InsertAtributo(int indicatorid, string nombre, string descripcion)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_DIMENSIONS objAtributo = new Clases.cKPI_DIMENSIONS())
            {
                objAtributo.indicatorid = indicatorid;
                objAtributo.userid = Convert.ToInt32(User.ProviderUserKey);
                objAtributo.nombre = nombre;
                objAtributo.descripcion = descripcion;
                if (objAtributo.bInsertar())
                    return objAtributo.dimensionid.Value;
                else
                    return -1;
            }
        }
        return -1;
    }

    //Nombre: InsertDimensionValue
    //Llamador: /custom/datos.js
    //Descripción: Añade una nueva etiqueta a la dimensión del dataset indicado
    [WebMethod]
    public static int InsertDimensionValue(int dimensionid, int datasetid, string texto)
    {
        Boolean bResultado = false;

        using (Clases.cKPI_DIMENSION_VALUES objDimensionValores = new Clases.cKPI_DIMENSION_VALUES())
        {
            objDimensionValores.datasetid = datasetid;
            objDimensionValores.dimensionid = dimensionid;
            objDimensionValores.codigo = texto;
            bResultado = objDimensionValores.bInsertar();
        }

        if (bResultado)
            return 1;
        else
            return -1;
    }

    //Nombre: InsertComment
    //Llamador: /custom/datos.js
    //Descripción: Crea una nueva dimensión para el conjunto de datos especificado en el parámetro
    [WebMethod]
    public static string InsertComment(int indicatorid, string comment, int padreid)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_COMMENTS objComentario = new Clases.cKPI_COMMENTS())
            {
                objComentario.indicatorid = indicatorid;
                objComentario.userid = Convert.ToInt32(User.ProviderUserKey);
                objComentario.comentario = comment;
                if (padreid >= 0)
                    objComentario.padreid = padreid;
                if (objComentario.bInsertar())
                {
                    JSonClases.KPI_COMMENTS objJSonComentario = new JSonClases.KPI_COMMENTS();

                    objJSonComentario.commentid = objComentario.commentid.Value;
                    objJSonComentario.indicatorid = objComentario.indicatorid.Value;
                    objJSonComentario.userid = objComentario.userid.Value;
                    objJSonComentario.fecha = DateTime.Now;
                    objJSonComentario.comentario = objComentario.comentario;
                    if (objComentario.padreid.HasValue)
                        objJSonComentario.padreid = objComentario.padreid.Value;
                    else
                        objJSonComentario.padreid = -1;
                    using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
                    {
                        objUsuario.userid = objComentario.userid;
                        if (objUsuario.bConsultar())
                        {
                            objJSonComentario.nombre = objUsuario.nombre;
                            objJSonComentario.apellidos = objUsuario.apellidos;
                            objJSonComentario.imageurl = objUsuario.imageurl;
                        }
                        else
                        {
                            objJSonComentario.nombre = string.Empty;
                            objJSonComentario.apellidos = string.Empty;
                            objJSonComentario.imageurl = string.Empty;
                        }
                    }
                    return JsonConvert.SerializeObject(objJSonComentario);
                }
            }
        }
        return JsonConvert.SerializeObject(string.Empty);
    }

    //Nombre: InsertIncidencia
    //Llamador: /custom/classes/kpiboard.avisos.js
    //Descripción: Inserta un registro de incidencia en la base de datos
    [WebMethod]
    public static void InsertIncidencia(string pagina, string mensaje, int error)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cASPNET_AVISOS objAviso = new Clases.cASPNET_AVISOS())
            {
                objAviso.pagina = pagina;
                objAviso.mensaje = mensaje;
                objAviso.codigo_error = error;
                objAviso.bInsertar();
            }
        }
    }

    //Nombre: CopyFormula
    //Llamador: /custom/datos.js
    //Descripción: Copia una formula en otro gráfico/dashboard
    [WebMethod]
    public static int CopyFormula(int idwidget, int idformula, string nombre)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_WIDGET_FORMULAS objNewFormula = new Clases.cKPI_WIDGET_FORMULAS())
            {
                objNewFormula.idwidget = idwidget;
                objNewFormula.idformula = idformula;
                objNewFormula.nombre = nombre;
                if (objNewFormula.bCopiar())
                    return objNewFormula.idformula.Value;
            }
        }
        return -1;
    }

    //Nombre: ChangeIndicatorImage
    //Llamador: /custom/objects/kpiboard.indicator.js
    //Descripción: Actualiza la imagen asociada a un indicador
    [WebMethod]
    public static string ChangeIndicatorImage(int indicatorid, string imageData)
    {
        Boolean bResultado = false;

        string newFilename = string.Format(@"{0}.png", Guid.NewGuid());
        string phisicalDirectoryPath = HttpContext.Current.Server.MapPath("\\uploads\\kpis\\" + indicatorid.ToString() + "\\images\\");
        if (!System.IO.Directory.Exists(phisicalDirectoryPath))
            System.IO.Directory.CreateDirectory(phisicalDirectoryPath);

        string phisicalImagePath = HttpContext.Current.Server.MapPath("\\uploads\\kpis\\" + indicatorid.ToString() + "\\images\\" + newFilename);
        string relativeImagePath = "/uploads/kpis/" + indicatorid.ToString() + "/images/" + newFilename;
        using (System.IO.FileStream fs = new System.IO.FileStream(phisicalImagePath, System.IO.FileMode.Create))
        {
            try
            {
                using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
        {
            objIndicador.indicatorid = indicatorid;
            if (objIndicador.bConsultar())
            {
                objIndicador.imageurl = relativeImagePath;
                bResultado = objIndicador.bModificar();
            }
        }

        if (bResultado)
            return relativeImagePath;
        else
            return string.Empty;
    }

    //Nombre: ChangeUserImage
    //Llamador: /custom/objects/kpiboard.user.js
    //Descripción: Actualiza la imagen asociada a un usuario
    [WebMethod]
    public static int ChangeProfileImage(string imageData)
    {
        Boolean bResultado = false;

        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            int userid = Convert.ToInt32(User.ProviderUserKey);
            string newFilename = string.Format(@"{0}.png", Guid.NewGuid());
            string phisicalDirectoryPath = HttpContext.Current.Server.MapPath("\\uploads\\users\\" + userid.ToString() + "\\images\\");
            if (!System.IO.Directory.Exists(phisicalDirectoryPath))
                System.IO.Directory.CreateDirectory(phisicalDirectoryPath);

            string phisicalImagePath = HttpContext.Current.Server.MapPath("\\uploads\\users\\" + userid.ToString() + "\\images\\" + newFilename);
            string relativeImagePath = "/uploads/users/" + userid.ToString() + "/images/" + newFilename;
            using (System.IO.FileStream fs = new System.IO.FileStream(phisicalImagePath, System.IO.FileMode.Create))
            {
                try
                {
                    using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))
                    {
                        byte[] data = Convert.FromBase64String(imageData);
                        bw.Write(data);
                        bw.Close();
                    }
                }
                catch
                {
                    return -1;
                }
            }
            using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
            {
                objUsuario.userid = userid;
                if (objUsuario.bConsultar())
                {
                    objUsuario.imageurl = relativeImagePath;
                    bResultado = objUsuario.bModificar();
                }
            }
        }

        if (bResultado)
            return 1;
        else
            return -1;
    }

    //Nombre: ChangeIndicatorUser
    //Llamador: /custom/objects/kpiboard.indicator.js
    //Descripción: Cambia la visibilidad del usuario con respecto a este indicador
    [WebMethod]
    public static int ChangeIndicatorUser(int indicatorid, char isanonymous)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_INDICATOR_USERS objIndicadorUsuario = new Clases.cKPI_INDICATOR_USERS())
            {
                objIndicadorUsuario.userid = Convert.ToInt32(User.ProviderUserKey);
                objIndicadorUsuario.indicatorid = indicatorid;
                if (isanonymous == 'S')
                    objIndicadorUsuario.anonimo = true;
                else
                    objIndicadorUsuario.anonimo = false;
                if (objIndicadorUsuario.bModificarAnonimo())
                    return 1;
            }
        }

        return -1;
    }

    //Nombre: ChangePasswordUser
    //Llamador: /custom/objects/kpiboard.user.js
    //Descripción: Cambia la contraseña del usuario actual
    [System.Web.Services.WebMethod]
    public static int ChangePasswordUser(string oldpwd, string newpdw)
    {
        if (string.IsNullOrEmpty(oldpwd) || string.IsNullOrEmpty(newpdw))
            return -1;
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            if (User.ChangePassword(oldpwd, newpdw))
                return 0;
        }
        return -2;
    }

    [WebMethod]
    public static int RecuperarPassword(string email)
    {
        int nResultado = 0;

        try
        {
            MembershipUser user = Membership.GetUser(Membership.GetUserNameByEmail(email));
            if (user != null)
            {
                //1.-Destino del mensaje
                System.Net.Mail.MailAddressCollection MisDestinos = new System.Net.Mail.MailAddressCollection();
                MisDestinos.Add(new System.Net.Mail.MailAddress(email));

                //2.-Cuerpo del mensaje
                string sMensaje = "Su nueva contraseña es : " + user.ResetPassword();

                //3.-Envio del mensaje
                if (EmailUtils.SendMessageEmail(MisDestinos, "Dropkeys : Nueva contraseña", sMensaje))
                    return 0;
                else
                    nResultado = -1;
            }
        }
        catch
        {
            nResultado = -1;
        }
        return nResultado;
    }
    /********************************************************************************************************************/

    //Nombre: DeleteIndicator
    //Llamador: /custom/generales/menusecundario.js
    //Descripción: Elimina un indicador de la librería del usuario
    [WebMethod]
    public static int DeleteIndicator(int indicatorid)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_INDICATOR_USERS objIndicadorUsuario = new Clases.cKPI_INDICATOR_USERS())
            {
                objIndicadorUsuario.indicatorid = indicatorid;
                objIndicadorUsuario.userid = Convert.ToInt32(User.ProviderUserKey);
                if (objIndicadorUsuario.bEliminar())
                    return 0;
                else
                    return -1;
            }
        }
        return -1;
    }

    //Nombre: DeleteFormula
    //Llamador: /custom/graphics.js
    //Descripción: Elimina una fórmula del widget especificado
    [WebMethod]
    public static int DeleteFormula(int formulaid)
    {
        using (Clases.cKPI_WIDGET_FORMULAS objFormula = new Clases.cKPI_WIDGET_FORMULAS())
        {
            objFormula.idformula = formulaid;
            if (objFormula.bEliminar())
                return formulaid;
        }
        return -1;
    }

    //Nombre: DeleteFiltro
    //Llamador: /custom/graphics.js
    //Descripción: Elimina el filtro de una expresión determinada
    [WebMethod]
    public static int DeleteFiltro(int filtroid)
    {
        using (Clases.cKPI_WIDGET_FILTERS objFiltro = new Clases.cKPI_WIDGET_FILTERS())
        {
            objFiltro.idfilter = filtroid;
            if (objFiltro.bEliminar())
                return filtroid;
        }
        return -1;
    }

    //Nombre: DeleteDashboard
    //Llamador: /custom/graphics.js
    //Descripción: Elimina el dashboard y todos sus widgets asociados
    [WebMethod]
    public static int DeleteDashboard(int dashboardid)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_DASHBOARDS objDashboard = new Clases.cKPI_DASHBOARDS())
            {
                objDashboard.userid = Convert.ToInt32(User.ProviderUserKey);
                objDashboard.iddashboard = dashboardid;
                if (objDashboard.bEliminar())
                {
                    return dashboardid;
                }
            }
        }
        return -1;
    }

    //Nombre: DeleteRevision
    //Llamador: /kpiboard.revision.js
    //Descripción: Elimina la revisión asociada a un determinado indicador
    [WebMethod]
    public static int DeleteRevision(int revisionid)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return -1;
        }

        using (Clases.cKPI_INDICATOR_REVISIONS objRevision = new Clases.cKPI_INDICATOR_REVISIONS())
        {
            objRevision.revisionid = revisionid;
            if (objRevision.bEliminar())
            {
                return 1;
            }
        }
        return -1;
    }

    //Nombre: DeleteDimension
    //Llamador: /custom/datos.js y /custom/indicator.js
    //Descripción: Elimina (si puede) la dimensión para el conjunto de datos especificado en el parámetro
    [WebMethod]
    public static int DeleteDimension(int dimensionid)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return -1;
        }

        using (Clases.cKPI_DIMENSIONS objDimension = new Clases.cKPI_DIMENSIONS())
        {
            objDimension.userid = Convert.ToInt32(usr.ProviderUserKey);
            objDimension.dimensionid = dimensionid;
            if (objDimension.bEliminar())
            {
                return 1;
            }
        }

        return -1;
    }

    //Nombre: DeleteDimension
    //Llamador: /custom/datos.js
    //Descripción: Elimina el valor asociado a una dimensión X del dataset Y
    [WebMethod]
    public static int DeleteDimensionValue(int dimensionid, int datasetid, string texto)
    {
        using (Clases.cKPI_DIMENSION_VALUES objDimensionValores = new Clases.cKPI_DIMENSION_VALUES())
        {
            objDimensionValores.datasetid = datasetid;
            objDimensionValores.dimensionid = dimensionid;
            objDimensionValores.codigo = texto;
            if (objDimensionValores.bEliminar())
                return 1;
            else
                return -1;
        }
    }

    //Nombre: DeleteDataset
    //Llamador: /custom/indicator.js
    //Descripción: Elimina el conjunto de datos (dataset) especificado en el parámetro y todos sus datos
    [WebMethod]
    public static int DeleteDataset(int datasetid)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return -1;
        }

        using (Clases.cKPI_DATASETS objDataset = new Clases.cKPI_DATASETS())
        {
            objDataset.userid = Convert.ToInt32(usr.ProviderUserKey);
            objDataset.datasetid = datasetid;
            if (objDataset.bEliminar())
            {
                return 1;
            }
        }

        return -1;
    }

    //Nombre: DeleteImport
    //Llamador: /custom/objects/kpiboard.importdata.js
    //Descripción: Elimina la importación y todos sus datos asociados
    [WebMethod]
    public static int DeleteImport(int importid)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return -1;
        }

        using (Clases.cKPI_IMPORTS objImport = new Clases.cKPI_IMPORTS())
        {
            objImport.userid = Convert.ToInt32(usr.ProviderUserKey);
            objImport.importid = importid;
            if (objImport.bEliminar())
            {
                return 1;
            }
        }

        return -1;
    }

    //Nombre: DeleteEtiqueta
    //Llamador: /custom/indicator.js
    //Descripción: Elimina una etiqueta del indicador
    [WebMethod]
    public static int DeleteEtiqueta(int indicatorid, string etiqueta)
    {
        Boolean bResultado = false;

        using (Clases.cKPI_INDICATOR_ETIQUETAS objEtiqueta = new Clases.cKPI_INDICATOR_ETIQUETAS())
        {
            objEtiqueta.indicatorid = indicatorid;
            bResultado = objEtiqueta.bEliminar(etiqueta);
        }

        if (bResultado)
            return 1;
        else
            return -1;
    }

    //Nombre: DelIndicador
    //Llamador: /custom/profile.js
    //Descripción: Elimina el indicador y todo sus conjuntos de datos (dataset) asociados especificado en el parámetro
    [WebMethod]
    public static int DelIndicador(int indicatorid)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return -1;
        }

        using (Clases.cKPI_INDICATOR_USERS objIndicatorUser = new Clases.cKPI_INDICATOR_USERS())
        {
            objIndicatorUser.userid = Convert.ToInt32(usr.ProviderUserKey);
            objIndicatorUser.indicatorid = indicatorid;
            if (objIndicatorUser.bEliminar())
            {
                return 1;
            }
        }
        return -1;
    }

    //Nombre: DelAllIndicators
    //Llamador: /custom/profile.js
    //Descripción: Elimina todos los conjunto de datos (dataset) y todos sus datos
    [WebMethod]
    public static int DelAllIndicators()
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return -1;
        }

        using (Clases.cKPI_INDICATOR_USERS objIndicatorUser = new Clases.cKPI_INDICATOR_USERS())
        {
            objIndicatorUser.userid = Convert.ToInt32(usr.ProviderUserKey);
            if (objIndicatorUser.bEliminarTodos())
            {
                return 1;
            }
        }
        return -1;
    }

    //Nombre: DeleteWidget
    //Llamador: /custom/indicator.js
    //Descripción: Elimina el gráfico y todos sus elementos asociados
    [WebMethod]
    public static int DeleteWidget(int idwidget)
    {
        using (Clases.cKPI_WIDGETS objWidget = new Clases.cKPI_WIDGETS())
        {
            objWidget.idwidget = idwidget;
            if (objWidget.bEliminar())
                return idwidget;
        }
        return -1;
    }

    //Nombre: DeleteFollower
    //Llamador: /custom/generales/menusecundario.js
    //Descripción: Elimina una relación de seguimiento hacia otro usuario
    [WebMethod]
    public static int DeleteFollower(int touserid)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cASPNET_FRIENDSHIP objNewFollower = new Clases.cASPNET_FRIENDSHIP())
            {
                objNewFollower.fromuserid = Convert.ToInt32(User.ProviderUserKey);
                objNewFollower.touserid = touserid;
                if (objNewFollower.bEliminar())
                    return 0;
                else
                    return -1;
            }
        }
        return -1;
    }

    //Nombre: DeleteComment
    //Descripción: Elimina un comentario realizado en algún indicador
    [WebMethod]
    public static int DeleteComment(int commentid)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_COMMENTS objComentario = new Clases.cKPI_COMMENTS())
            {
                objComentario.commentid = commentid;
                if (objComentario.bEliminar())
                    return 0;
                else
                    return -1;
            }
        }
        return -1;
    }

    /********************************************************************************************************************/

    //Nombre: UpdateFilter
    //Llamador: /custom/graphics.js
    //Descripción: Actualiza los datos de un filtro
    [WebMethod]
    public static string UpdateFilter(int filtroid, int dimensionid, string filtro, string valor)
    {
        using (Clases.cKPI_WIDGET_FILTERS objFiltro = new Clases.cKPI_WIDGET_FILTERS())
        {
            objFiltro.idfilter = filtroid;
            objFiltro.dimensionid = dimensionid;
            objFiltro.filtro = filtro;
            objFiltro.valor = valor;
            if (objFiltro.bModificar())
            {
                JSonClases.KPI_WIDGET_FILTERS objJSonFilters = new JSonClases.KPI_WIDGET_FILTERS();

                objJSonFilters.filterid = objFiltro.idfilter.Value;
                //objJSonFilters.expresionid = objFiltro.idexpresion.Value;
                objJSonFilters.dimensionid = objFiltro.dimensionid.Value;
                objJSonFilters.filtro = objFiltro.filtro;
                objJSonFilters.valor = objFiltro.valor;

                return JsonConvert.SerializeObject(objJSonFilters);
            }
        }
        return string.Empty;
    }

    //Nombre: CrearFormula
    //Llamador: /custom/graphics.js
    //Descripción: Inserta una nueva fórmula desde cero
    [WebMethod]
    public static string UpdateFormula(int idformula, string nombre, string formula, string color)
    {
        using (Clases.cKPI_WIDGET_FORMULAS objNewFormula = new Clases.cKPI_WIDGET_FORMULAS())
        {
            objNewFormula.idformula = idformula;
            objNewFormula.nombre = nombre;
            objNewFormula.formula = formula;
            objNewFormula.color = color;
            if (objNewFormula.bModificar())
            {
                if (objNewFormula.bConsultar())
                {
                    JSonClases.KPI_WIDGET_FORMULAS objJSonFormula = new JSonClases.KPI_WIDGET_FORMULAS();

                    objJSonFormula.formulaid = objNewFormula.idformula.Value;
                    objJSonFormula.nombre = objNewFormula.nombre;
                    objJSonFormula.formula = objNewFormula.formula;
                    objJSonFormula.color = objNewFormula.color;
                    if (objNewFormula.validado)
                        objJSonFormula.validated = 0;
                    else
                        objJSonFormula.validated = -1;

                    return JsonConvert.SerializeObject(objJSonFormula);
                }
            }
        }
        return JsonConvert.SerializeObject(string.Empty);
    }

    //Nombre: UpdateDashboard
    //Llamador: /custom/graphics.js 
    //Descripción: Modifica el texto del dashboard
    [WebMethod]
    public static string UpdateDashboard(int dashboardid, string titulo)
    {
        System.Web.Security.MembershipUser User = System.Web.Security.Membership.GetUser();
        if (User != null)
        {
            using (Clases.cKPI_DASHBOARDS objDashboard = new Clases.cKPI_DASHBOARDS())
            {
                objDashboard.userid = Convert.ToInt32(User.ProviderUserKey);
                objDashboard.iddashboard = dashboardid;
                objDashboard.titulo = titulo.Trim();
                if (objDashboard.bModificar())
                {
                    JSonClases.KPI_DASHBOARD objJSonDashboard = new JSonClases.KPI_DASHBOARD();

                    objJSonDashboard.dashboardid = objDashboard.iddashboard.Value;
                    objJSonDashboard.titulo = objDashboard.titulo;

                    return JsonConvert.SerializeObject(objJSonDashboard);
                }
            }
        }
        return string.Empty;
    }

    //Nombre: UpdateDataset
    //Llamador: /custom/objects/kpiboard.dataset.js
    //Descripción: Actualiza los datos de un filtro
    [WebMethod]
    public static int UpdateDataset(int datasetid, string nombre)
    {
        using (Clases.cKPI_DATASETS objDataset = new Clases.cKPI_DATASETS())
        {
            objDataset.datasetid = datasetid;
            objDataset.nombre = nombre;
            if (objDataset.bModificar())
                return 1;
        }
        return -1;
    }

    [WebMethod]
    public static int UpdateWidget(int idwidget, string titulo, string estilo)
    {
        using (Clases.cKPI_WIDGETS objWidget = new Clases.cKPI_WIDGETS())
        {
            objWidget.idwidget = idwidget;
            objWidget.titulo = titulo;
            objWidget.estilo = estilo;
            if (objWidget.bModificar())
                return objWidget.idwidget.Value;
        }
        return -1;
    }

    /************************************************************************************************************************************/

    //Nombre: CountIndicators
    //Llamador: /custom/objects/kpiboard.indicator.js
    //Descripción: Obtiene el recuento de indicadores
    [WebMethod]
    public static int CountIndicators(string titulo, int categoryid, int subcategoryid)
    {
        using (Clases.cKPI_INDICATORS objIndicadores = new Clases.cKPI_INDICATORS())
        {
            System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
            if (usr != null)
            {
                objIndicadores.userid = Convert.ToInt32(usr.ProviderUserKey);
            }
            objIndicadores.titulo = titulo;
            objIndicadores.publicado = true;
            if (categoryid >= 0)
                objIndicadores.categoryid = categoryid;
            if (subcategoryid >= 0)
                objIndicadores.subcategoryid = subcategoryid;
            return objIndicadores.RecuentoBuscador();
        }
    }

    //Nombre: CountUsers
    //Llamador: /custom/objects/kpiboard.user.js
    //Descripción: Obtiene el recuento de usuarios para un indicador
    [WebMethod]
    public static int CountUsers(int indicatorid, string nombre)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return 0;
        }

        using (Clases.cKPI_INDICATOR_USERS objUsers = new Clases.cKPI_INDICATOR_USERS())
        {
            objUsers.userid = Convert.ToInt32(usr.ProviderUserKey);
            objUsers.indicatorid = indicatorid;
            objUsers.nombre = nombre;
            return objUsers.nRecuentoUsers();
        }
    }

    //Nombre: CountAttribute
    //Llamador: /custom/objects/kpiboard.dimension.js
    //Descripción: Obtiene el recuento de datasets para un indicador
    [WebMethod]
    public static int CountAttribute(int indicatorid, string nombre)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return 0;
        }

        using (Clases.cKPI_DIMENSIONS objDimension = new Clases.cKPI_DIMENSIONS())
        {
            //objDimension.userid = Convert.ToInt32(usr.ProviderUserKey);
            objDimension.indicatorid = indicatorid;
            objDimension.nombre = nombre;
            return objDimension.Recuento();
        }
    }

    //Nombre: CountDataset
    //Llamador: /custom/objects/kpiboard.dataset.js
    //Descripción: Obtiene el recuento de datasets para un indicador
    [WebMethod]
    public static int CountDataset(int indicatorid, string nombre)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return 0;
        }

        using (Clases.cKPI_DATASETS objDataSet = new Clases.cKPI_DATASETS())
        {
            objDataSet.userid = Convert.ToInt32(usr.ProviderUserKey);
            objDataSet.indicatorid = indicatorid;
            objDataSet.nombre = nombre;
            return objDataSet.Recuento();
        }
    }

    //Nombre: CountImport
    //Llamador: /custom/objects/kpiboard.importdata.js
    //Descripción: Obtiene el recuento de importaciones para un indicador
    [WebMethod]
    public static int CountImport(int indicatorid, string nombre)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return 0;
        }

        using (Clases.cKPI_IMPORTS objImport = new Clases.cKPI_IMPORTS())
        {
            objImport.userid = Convert.ToInt32(usr.ProviderUserKey);
            objImport.indicatorid = indicatorid;
            objImport.nombre = nombre;
            return objImport.nRecuento();
        }
    }

    //Nombre: CountFriend
    //Llamador: /custom/classes/kpiboard.friendship.js
    //Descripción: Obtiene el recuento de importaciones para un indicador
    [WebMethod]
    public static int CountFriend(string aceptado)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return 0;
        }

        using (Clases.cASPNET_FRIENDSHIP objAmigos = new Clases.cASPNET_FRIENDSHIP())
        {
            objAmigos.fromuserid = Convert.ToInt32(usr.ProviderUserKey);
            if (!string.IsNullOrEmpty(aceptado))
            {
                if (aceptado == "N")
                    objAmigos.aceptado = false;
                else
                    objAmigos.aceptado = true;
            }
            return objAmigos.nRecuento();
        }
    }

    //Nombre: CountRevision
    //Llamador: /custom/objects/kpiboard.revision.js
    //Descripción: Obtiene el recuento de revisiones pendientes para un indicador
    [WebMethod]
    public static int CountRevision(int indicatorid)
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
        {
            return 0;
        }

        using (Clases.cKPI_INDICATOR_REVISIONS objRevision = new Clases.cKPI_INDICATOR_REVISIONS())
        {
            objRevision.indicatorid = indicatorid;
            return objRevision.nRecuento();
        }
    }
    /******************************************* I M P O R T A R   *********************************************/

    //Nombre: LeerDesdeCSV
    //Llamador: kpiboard.importdata.js 
    //Descripción: Lee los datos del archivo origen tipo csv
    [WebMethod]
    public static string LeerDesdeCSV(string filename)
    {
        string sJSON = string.Empty;
        char separator;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString() + "/" + filename);

            char[] separators = new char[] { ',', '^', ';', '|' };
            using (System.IO.StreamReader Reader = new System.IO.StreamReader(path))
            {
                separator = csv.csvSeparatorDetector.Detect(Reader, 10, separators);
            }
            //create object for CSVReader and pass the stream
            using (System.IO.FileStream fs = System.IO.File.OpenRead(path))
            {
                csv.csvReader reader = new csv.csvReader(fs, separator);

                //get the header
                string[] headers = reader.GetCSVLine();
                System.Data.DataTable TablaDeDatos = new System.Data.DataTable();

                //add headers
                foreach (string strHeader in headers)
                    TablaDeDatos.Columns.Add(strHeader);

                try
                {
                    int MaxLines = 1;
                    string[] data;
                    while ((MaxLines < 13) && (data = reader.GetCSVLine()) != null)
                    {
                        TablaDeDatos.Rows.Add(data);
                        MaxLines++;
                    }
                }
                catch (Exception excp)
                {
                    InsertaAviso("LeerDesdeCSV", excp.Message);
                    return JsonConvert.SerializeObject(string.Empty);
                }

                sJSON = JsonConvert.SerializeObject(TablaDeDatos);
            }
        }
        else
            sJSON = JsonConvert.SerializeObject(string.Empty);

        return sJSON.Replace('.', ',');
    }

    //Nombre: GetCSVSeparator
    //Llamador: kpiboard.importdata.js 
    //Descripción: Obtiene el caracter separador del fichero de datos csv
    [WebMethod]
    public static string GetCSVSeparator(string filename)
    {
        string sCaracter = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString() + "/" + filename);

            char[] separators = new char[] { ',', '^', ';', '|' };
            using (System.IO.StreamReader Reader = new System.IO.StreamReader(path))
            {
                sCaracter = csv.csvSeparatorDetector.Detect(Reader, 10, separators).ToString();
            }
        }

        return sCaracter;
    }

    //Nombre: FechaDesdeCSV
    //Llamador: kpiboard.importdata.js 
    //Descripción: comprueba el formato de la columna Fecha del archivo origen tipo csv
    [WebMethod]
    public static string FechaDesdeCSV(string filename, int dateColumn)
    {
        string sJSON = string.Empty;
        char separator;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString() + "/" + filename);

            char[] separators = new char[] { ',', '^', ';', '|' };
            using (System.IO.StreamReader Reader = new System.IO.StreamReader(path))
            {
                separator = csv.csvSeparatorDetector.Detect(Reader, 10, separators);
            }
            //create object for CSVReader and pass the stream
            using (System.IO.FileStream fs = System.IO.File.OpenRead(path))
            {
                csv.csvReader reader = new csv.csvReader(fs, separator);

                try
                {
                    List<string> fmts = new List<string>();
                    string[] headers = reader.GetCSVLine();
                    string[] data = reader.GetCSVLine();
                    if (data.Length >= dateColumn)
                    {
                        int MaxLines = 1;
                        while ((MaxLines < 10) && (data = reader.GetCSVLine()) != null)
                        {
                            fmts = StringToFechaFormat(fmts, data[dateColumn]);
                            MaxLines++;
                        }
                    }
                    sJSON = JsonConvert.SerializeObject(fmts.ToArray());
                }
                catch (Exception excp)
                {
                    InsertaAviso("LeerDesdeCSV", excp.Message);
                    return JsonConvert.SerializeObject(string.Empty);
                }
            }
        }
        else
            sJSON = JsonConvert.SerializeObject(string.Empty);

        return sJSON;
    }

    //Nombre: ImportarDesdeCSV
    //Llamador: kpiboard.importdata.js 
    //Descripción: Lee los datos del archivo origen tipo csv
    [WebMethod]
    public static Boolean ImportarDesdeCSV(string filename, string nombre, string descripcion, string info, string attributes, int indicatorid, char separator, int modo, string datetimeFormat)
    {
        //Comenzamos con algunas comprobaciones previas
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
            return false;

        using (Clases.cKPI_IMPORTS objImportacion = new Clases.cKPI_IMPORTS())
        {
            objImportacion.indicatorid = indicatorid;
            objImportacion.userid = Convert.ToInt32(usr.ProviderUserKey);
            objImportacion.nombre = nombre;
            objImportacion.descripcion = descripcion;
            objImportacion.num_data_error = 0;
            objImportacion.num_data_ok = 0;
            objImportacion.num_datasets = 0;
            if (objImportacion.bInsertar())
            {
                // create a new AsyncProcessorDetail
                AsyncProcessorDetail detail = new AsyncProcessorDetail(objImportacion.importid.Value, System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString()), filename, 0, "ProcessorImportCsv", 1, info, attributes, indicatorid, separator, modo, datetimeFormat);

                // call the AsyncProcessManager and pass it the AsyncProcessorDetail to be processed
                AsyncProcessManager.StartAsyncProcess(detail);

                // start the asyncProcessingMonitor control
                //asyncProcessingMonitor1.Start(unique_proc_id, true, "Default.aspx");
            }
        }
        return true;
    }

    //Nombre: LeerHojasDesdeXLS
    //Llamador: kpiboard.importdata.js 
    //Descripción: Lee las hojas del archivo origen tipo xls
    [WebMethod]
    public static string LeerHojasDesdeXLS(string filename)
    {
        string sJSON = string.Empty;
        string sComa = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString() + "/" + filename);

            System.IO.FileStream stream = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Excel.IExcelDataReader excelReader;

            if (System.IO.Path.GetExtension(path).ToLower() == ".xls")
            {
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = Excel.ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (System.IO.Path.GetExtension(path).ToLower() == ".xlsx")
            {
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = Excel.ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            else
            {
                sJSON = JsonConvert.SerializeObject(string.Empty);
                return "[" + sJSON + "]";
            }
            //3. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            using (System.Data.DataSet dsResult = excelReader.AsDataSet())
            {
                for (int i = 0; i < dsResult.Tables.Count; i++)
                {
                    JSonClases.KPI_DIMENSIONS objJSonHojas = new JSonClases.KPI_DIMENSIONS();

                    objJSonHojas.value = dsResult.Tables[i].TableName;
                    objJSonHojas.id = i;

                    sJSON = sJSON + sComa + JsonConvert.SerializeObject(objJSonHojas);
                    sComa = ", ";
                }
            }

            //5. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();
        }
        else
            sJSON = JsonConvert.SerializeObject(string.Empty);

        return "[" + sJSON + "]";
    }

    //Nombre: LeerDatosDesdeXLS
    //Llamador: kpiboard.importdata.js 
    //Descripción: Lee los datos del archivo origen tipo xls
    [WebMethod]
    public static string LeerDatosDesdeXLS(string filename, int hoja)
    {
        string sJSON = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString() + "/" + filename);

            System.IO.FileStream stream = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Excel.IExcelDataReader excelReader;

            if (System.IO.Path.GetExtension(path).ToLower() == ".xls")
            {
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = Excel.ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (System.IO.Path.GetExtension(path).ToLower() == ".xlsx")
            {
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = Excel.ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            else
            {
                sJSON = JsonConvert.SerializeObject(string.Empty);
                return "[" + sJSON + "]";
            }

            //3. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            using (System.Data.DataSet dsResult = excelReader.AsDataSet())
            {
                System.Data.DataTable TablaDeDatos = new System.Data.DataTable();

                foreach (System.Data.DataColumn Columna in dsResult.Tables[hoja].Columns)
                {
                    TablaDeDatos.Columns.Add(new System.Data.DataColumn(Columna.ColumnName, Columna.DataType));
                }

                int MaxLines = 1;
                while ((MaxLines < 11) && (MaxLines < dsResult.Tables[hoja].Rows.Count))
                {
                    TablaDeDatos.ImportRow(dsResult.Tables[hoja].Rows[MaxLines]);
                    MaxLines++;
                }
                sJSON = JsonConvert.SerializeObject(TablaDeDatos);
            }
        }
        else
            sJSON = JsonConvert.SerializeObject(string.Empty);

        return sJSON;
    }

    //Nombre: FechaDesdeXLS
    //Llamador: kpiboard.importdata.js 
    //Descripción: comprueba el formato de la columna Fecha del archivo origen tipo xls
    [WebMethod]
    public static string FechaDesdeXLS(string filename, int hoja, int dateColumn)
    {
        string sJSON = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString() + "/" + filename);

            System.IO.FileStream stream = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Excel.IExcelDataReader excelReader;

            if (System.IO.Path.GetExtension(path).ToLower() == ".xls")
            {
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = Excel.ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (System.IO.Path.GetExtension(path).ToLower() == ".xlsx")
            {
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = Excel.ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            else
            {
                sJSON = JsonConvert.SerializeObject(string.Empty);
                return "[" + sJSON + "]";
            }
            //3. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            using (System.Data.DataSet dsResult = excelReader.AsDataSet())
            {
                List<string> fmts = new List<string>();
                if (dsResult.Tables[hoja].Columns.Count >= dateColumn)
                {
                    int MaxLines = 1;
                    while ((MaxLines < 10) && (MaxLines < dsResult.Tables[hoja].Rows.Count))
                    {
                        fmts = StringToFechaFormat(fmts, dsResult.Tables[hoja].Rows[MaxLines][dateColumn].ToString());
                        MaxLines++;
                    }
                }
                sJSON = JsonConvert.SerializeObject(fmts.ToArray());
            }
        }
        else
            sJSON = JsonConvert.SerializeObject(string.Empty);

        return sJSON;
    }

    //Nombre: ImportarDesdeXLS
    //Llamador: kpiboard.importdata.js 
    //Descripción: Lee los datos del archivo origen tipo csv
    [WebMethod]
    public static Boolean ImportarDesdeXLS(string filename, string nombre, string descripcion, string info, string attributes, int indicatorid, int hoja, int modo, string datetimeFormat)
    {
        //Comenzamos con algunas comprobaciones previas
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
            return false;

        using (Clases.cKPI_IMPORTS objImportacion = new Clases.cKPI_IMPORTS())
        {
            objImportacion.indicatorid = indicatorid;
            objImportacion.userid = Convert.ToInt32(usr.ProviderUserKey);
            objImportacion.nombre = nombre;
            objImportacion.descripcion = descripcion;
            objImportacion.num_data_error = 0;
            objImportacion.num_data_ok = 0;
            objImportacion.num_datasets = 0;
            if (objImportacion.bInsertar())
            {
                // create a new AsyncProcessorDetail
                AsyncProcessorDetail detail = new AsyncProcessorDetail(objImportacion.importid.Value, System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString()), filename, 0, "ProcessorImportXls", 1, info, attributes, indicatorid, hoja, modo, datetimeFormat);

                // call the AsyncProcessManager and pass it the AsyncProcessorDetail to be processed
                AsyncProcessManager.StartAsyncProcess(detail);

                // start the asyncProcessingMonitor control
                //asyncProcessingMonitor1.Start(unique_proc_id, true, "Default.aspx");
            }
        }
        return true;
    }

    //Nombre: LeerDesdeJson
    //Llamador: kpiboard.importdata.js 
    //Descripción: Lee los datos del archivo origen tipo json
    [WebMethod]
    public static string LeerDesdeJSON(string filename)
    {
        string sJSON = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString() + "/" + filename);
            string data = string.Empty;
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(path, Encoding.UTF8))
            {
                data = streamReader.ReadToEnd();
            }

            System.Data.DataTable tester = (System.Data.DataTable)JsonConvert.DeserializeObject(data, (typeof(System.Data.DataTable)));
            System.Data.DataTable TablaDeDatos = tester.Clone();

            int MaxLines = 1;
            while ((MaxLines < 13) && (MaxLines < tester.Rows.Count))
            {
                TablaDeDatos.ImportRow(tester.Rows[MaxLines]);
                MaxLines++;
            }

            sJSON = JsonConvert.SerializeObject(TablaDeDatos);
        }
        else
            sJSON = JsonConvert.SerializeObject(string.Empty);

        return sJSON;
    }

    //Nombre: ImportarDesdeJSON
    //Llamador: kpiboard.importdata.js 
    //Descripción: Lee los datos del archivo origen tipo json
    [WebMethod]
    public static Boolean ImportarDesdeJSON(string filename, string nombre, string descripcion, string info, string attributes, int indicatorid, int modo, string datetimeFormat)
    {
        //Comenzamos con algunas comprobaciones previas
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
            return false;

        using (Clases.cKPI_IMPORTS objImportacion = new Clases.cKPI_IMPORTS())
        {
            objImportacion.indicatorid = indicatorid;
            objImportacion.userid = Convert.ToInt32(usr.ProviderUserKey);
            objImportacion.nombre = nombre;
            objImportacion.descripcion = descripcion;
            objImportacion.num_data_error = 0;
            objImportacion.num_data_ok = 0;
            objImportacion.num_datasets = 0;
            if (objImportacion.bInsertar())
            {
                // create a new AsyncProcessorDetail
                AsyncProcessorDetail detail = new AsyncProcessorDetail(objImportacion.importid.Value, System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString()), filename, 0, "ProcessorImportJson", 1, info, attributes, indicatorid, modo, datetimeFormat);

                // call the AsyncProcessManager and pass it the AsyncProcessorDetail to be processed
                AsyncProcessManager.StartAsyncProcess(detail);

                // start the asyncProcessingMonitor control
                //asyncProcessingMonitor1.Start(unique_proc_id, true, "Default.aspx");
            }
        }
        return true;
    }

    //Nombre: LeerDesdeXML
    //Llamador: kpiboard.importdata.js 
    //Descripción: Lee los datos del archivo origen tipo xml
    [WebMethod]
    public static string LeerDesdeXML(string filename)
    {
        string sJSON = string.Empty;

        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString() + "/" + filename);

            System.Data.DataSet TablaOriginal = new System.Data.DataSet();
            TablaOriginal.ReadXml(path);
            System.Data.DataTable TablaDeDatos = TablaOriginal.Tables[0].Clone();

            int MaxLines = 1;
            while ((MaxLines < 13) && (MaxLines < TablaOriginal.Tables[0].Rows.Count))
            {
                TablaDeDatos.ImportRow(TablaOriginal.Tables[0].Rows[MaxLines]);
                MaxLines++;
            }

            sJSON = JsonConvert.SerializeObject(TablaDeDatos);
        }
        else
            sJSON = JsonConvert.SerializeObject(string.Empty);

        return sJSON;
    }

    //Nombre: ImportarDesdeJson
    //Llamador: kpiboard.importdata.js 
    //Descripción: Lee los datos del archivo origen tipo xml
    [WebMethod]
    public static Boolean ImportarDesdeXML(string filename, string nombre, string descripcion, string info, string attributes, int indicatorid, int modo, string datetimeFormat)
    {
        //Comenzamos con algunas comprobaciones previas
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
            return false;

        using (Clases.cKPI_IMPORTS objImportacion = new Clases.cKPI_IMPORTS())
        {
            objImportacion.indicatorid = indicatorid;
            objImportacion.userid = Convert.ToInt32(usr.ProviderUserKey);
            objImportacion.nombre = nombre;
            objImportacion.descripcion = descripcion;
            objImportacion.num_data_error = 0;
            objImportacion.num_data_ok = 0;
            objImportacion.num_datasets = 0;
            if (objImportacion.bInsertar())
            {
                // create a new AsyncProcessorDetail
                AsyncProcessorDetail detail = new AsyncProcessorDetail(objImportacion.importid.Value, System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString()), filename, 0, "ProcessorImportXML", 1, info, attributes, indicatorid, modo, datetimeFormat);

                // call the AsyncProcessManager and pass it the AsyncProcessorDetail to be processed
                AsyncProcessManager.StartAsyncProcess(detail);

                // start the asyncProcessingMonitor control
                //asyncProcessingMonitor1.Start(unique_proc_id, true, "Default.aspx");
            }
        }
        return true;
    }

    //Nombre: ImportarDesdeTabla
    //Llamador: kpiboard.importdata.js 
    //Descripción: Lee los datos de una carga manual desde tabla
    [WebMethod]
    public static Boolean ImportarDesdeTabla(string nombre, string descripcion, string columns, string rows, int indicatorid, int modo, string dimension)
    {
        //Comenzamos con algunas comprobaciones previas
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr == null)
            return false;

        using (Clases.cKPI_IMPORTS objImportacion = new Clases.cKPI_IMPORTS())
        {
            objImportacion.indicatorid = indicatorid;
            objImportacion.userid = Convert.ToInt32(usr.ProviderUserKey);
            objImportacion.nombre = nombre;
            objImportacion.descripcion = descripcion;
            objImportacion.num_data_error = 0;
            objImportacion.num_data_ok = 0;
            objImportacion.num_datasets = 0;
            if (objImportacion.bInsertar())
            {
                // create a new AsyncProcessorDetail
                AsyncProcessorDetail detail = new AsyncProcessorDetail(objImportacion.importid.Value, 0, "ProcessorImportTable", 1, columns, rows, dimension, indicatorid, modo);

                // call the AsyncProcessManager and pass it the AsyncProcessorDetail to be processed
                AsyncProcessManager.StartAsyncProcess(detail);

                // start the asyncProcessingMonitor control
                //asyncProcessingMonitor1.Start(unique_proc_id, true, "Default.aspx");
            }
        }
        return true;
    }
    #endregion
}