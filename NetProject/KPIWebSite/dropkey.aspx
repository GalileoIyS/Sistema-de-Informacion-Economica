<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dropkey.aspx.cs" Inherits="dropkey" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="Big Data cataloge using shared indicators" />
    <meta name="keywords" content="Big Data, Business Intelligence, KPI, Dashboards, Realtime analysis" />
    <meta name="author" content="Nicolas Agustin Hernandez Guerra de Aguilar" />
    <title>DropKeys</title>

    <!-- Bootstrap Core CSS -->
    <link href="/content/css/bootstrap.css" rel="stylesheet" />

    <!-- SmartAdmin Styles : Please note (smartadmin-production.css) was created using LESS variables -->
    <link rel="stylesheet" type="text/css" media="screen" href="/content/css/smartadmin-production.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="/content/css/smartadmin-skins.css" />

    <!-- Custom CSS -->
    <link rel="stylesheet" type="text/css" href="/content/css/dropkeys.css" />
    <link rel="stylesheet" type="text/css" href="/content/css/desktop.css" />

    <!-- Custom Fonts -->
    <link href="/content/css/font-awesome.css" rel="stylesheet" />

    <!-- Google Fonts -->
    <link href="http://fonts.googleapis.com/css?family=Roboto:300" rel="stylesheet" type="text/css" />
    <link href="http://fonts.googleapis.com/css?family=Montserrat:400,700" rel="stylesheet" type="text/css" />
    <link href="http://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet" type="text/css" />

    <!-- iOS web-app metas : hides Safari UI Components and Changes Status Bar Appearance -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />

    <!-- Link to Google Charts -->
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load('visualization', '1.0', { 'packages': ['corechart', 'table', 'gauge'] });
    </script>
    <!-- Link to Google CDN's jQuery + jQueryUI; fall back to local -->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.0.2/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        if (!window.jQuery) {
            document.write('<script src="scripts/libs/jquery-2.0.2.min.js"><\/script>');
        }
    </script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        if (!window.jQuery.ui) {
            document.write('<script src="scripts/libs/jquery-ui-1.10.3.min.js"><\/script>');
        }
    </script>
    <!-- Libreria javascript de bootstrap-->
    <script src="<%=Page.ResolveUrl("~/scripts/bootstrap.min.js") %>" type="text/javascript"></script>
    <!-- Libreria javascript de bootstrap-->
    <script src="<%=Page.ResolveUrl("~/scripts/bootstrap3-typeahead.min.js") %>" type="text/javascript"></script>
    <!-- Librerías necesarias para el tema smartadmin-->
    <script src="<%=Page.ResolveUrl("~/scripts/app.config.js") %>" type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("~/scripts/app.js") %>" type="text/javascript"></script>
    <!-- Mis librerías generales -->
    <%= System.Web.Optimization.Scripts.Render("~/js/kpigeneralV100")%>
    <!-- Mis librerías de clases -->
    <%= System.Web.Optimization.Scripts.Render("~/js/kpiclassesV100")%>
    <!-- Mis librerías de objetos -->
    <%= System.Web.Optimization.Scripts.Render("~/js/kpiobjectsV100")%>
    <!-- Mis controles propios -->
    <%= System.Web.Optimization.Scripts.Render("~/js/kpicontrolsV100")%>
    <!-- Mis librerías customizadas -->
    <%= System.Web.Optimization.Scripts.Render("~/js/customSmartAdminV100")%>
    <!--[if IE 8]>
		<h1>Your browser is out of date, please update your browser by going to www.microsoft.com/download</h1>
    <![endif]-->
    <script type="text/javascript" src="scripts/custom/pages/kpiboard.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnWidgetID" runat="server" ClientIDMode="Static" />
        <div id="content">
            <div id="desktop-message-warning">
            </div>
            <div class="widget-loading hidden">
                <div class="widget-loading-content">
                    <img style="border-width: 0px;" src="../images/indicators/loading.gif" alt="...loading..." />
                </div>
            </div>
            <!-- widget grid -->
            <div id="widget-grid">
                <div class="row">
                    <article class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="jarviswidget" id="wid-id-0" data-widget-deletebutton="false" data-widget-togglebutton="false" data-widget-editbutton="false" data-widget-sortable="false" data-widget-colorbutton="false">
                            <header>
                                <span class="widget-icon"><i class="glyphicon glyphicon-stats txt-color-darken"></i></span>
                                <div class="widget-toolbar">
                                    <div class="btn-group">
                                        <a class="txt-color-blueDark" title="" data-toggle="dropdown"><i class="fa fa-gears"></i></a>
                                        <ul class="dropdown-menu pull-right">
                                            <li>
                                                <a id="btnShowHideLegend" href="#"><i class="fa fa-eye"></i>&nbsp;Show formulas</a>
                                            </li>
                                            <li>
                                                <a id="btnDownloadCSV" href="#"><i class="fa fa-download"></i>&nbsp;Download CSV</a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <h2 class="widget-title">Widget Title</h2>
                            </header>
                            <!-- widget div-->
                            <div>
                                <!-- widget content -->
                                <div class="widget-body no-padding">
                                    <!-- WIDGET CONTAINER -->
                                    <div class="widget-container">
                                        <!-- widget toolbar -->
                                        <div class="widget-body-toolbar">
                                            <div class="btn-group" data-toggle="buttons">
                                                <label class="btn btn-default btn-sm btn-type-graph active">
                                                    <input type="radio" name="options" id="btnPieChart" value="Q" checked="checked" />
                                                    Pie
                                                </label>
                                                <label class="btn btn-default btn-sm btn-type-graph">
                                                    <input type="radio" name="options" id="btnBarChart" value="B" />
                                                    Bar
                                                </label>
                                                <label class="btn btn-default btn-sm btn-type-graph">
                                                    <input type="radio" name="options" id="btnLineChart" value="L" />
                                                    Line
                                                </label>
                                                <label class="btn btn-default btn-sm btn-type-graph">
                                                    <input type="radio" name="options" id="btnAreaChart" value="A" />
                                                    Area
                                                </label>
                                                <label class="btn btn-default btn-sm btn-type-graph">
                                                    <input type="radio" name="options" id="btnHistogram" value="H" />
                                                    Histogram
                                                </label>
                                            </div>
                                            <div class="btn-group" data-toggle="buttons">
                                                <label class="btn btn-default btn-sm btn-type-time active">
                                                    <input type="radio" name="options" id="btnDia" value="D" checked="checked" />
                                                    Day
                                                </label>
                                                <label class="btn btn-default btn-sm btn-type-time">
                                                    <input type="radio" name="options" id="btnSemana" value="s" />
                                                    Week
                                                </label>
                                                <label class="btn btn-default btn-sm btn-type-time">
                                                    <input type="radio" name="options" id="btnQuincena" value="Q" />
                                                    Fortnight
                                                </label>
                                                <label class="btn btn-default btn-sm btn-type-time">
                                                    <input type="radio" name="options" id="btnMes" value="M" />
                                                    Month
                                                </label>
                                                <label class="btn btn-default btn-sm btn-type-time">
                                                    <input type="radio" name="options" id="btnTrimestre" value="T" />
                                                    Quarter
                                                </label>
                                                <label class="btn btn-default btn-sm btn-type-time">
                                                    <input type="radio" name="options" id="btnSemestre" value="S" />
                                                    Semester
                                                </label>
                                                <label class="btn btn-default btn-sm btn-type-time">
                                                    <input type="radio" name="options" id="btnAnual" value="A" />
                                                    Year
                                                </label>
                                            </div>
                                        </div>
                                        <div id="graphic-leyend" class="graphic-leyend">
                                        </div>
                                        <div id="graphic-chart">
                                        </div>
                                    </div>
                                </div>
                                <!-- end widget content -->
                            </div>
                            <!-- end widget div -->
                        </div>
                    </article>
                </div>
            </div>
            <!-- end widget grid -->
        </div>
    </form>
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/dropkey.js") %>" type="text/javascript"></script>
</body>
</html>
