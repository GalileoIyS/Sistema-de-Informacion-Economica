<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="registrado_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <!-- SHORTCUT AREA : With large tiles (activated via clicking user name tag)
		Note: These tiles are completely responsive,
		you can add as many as you like
		-->
    <div style="display: none;" id="shortcut">
        <ul>
            <asp:ListView ID="lstDashboards" runat="server" EnableViewState="true" DataKeyNames="TITULO">
                <ItemTemplate>
                    <li>
                        <asp:HyperLink ID="btnViewDashboard" CssClass="jarvismetro-tile big-cubes" NavigateUrl='<%# "~/registrado/dashboard.aspx?iddashboard=" + Eval("IDDASHBOARD") %>' runat="server">
                            <span class="iconbox"><i class="fa fa-bar-chart-o fa-3x"></i><span><%# Eval("TITULO") %><span class="label pull-right bg-color-darken"><%# Eval("CONTADOR") %></span></span> </span>
                        </asp:HyperLink></li>
                </ItemTemplate>
            </asp:ListView>
        </ul>
    </div>

    <!-- MAIN PANEL -->
    <div role="main">

        <!-- RIBBON -->
        <div id="ribbon" class="margin-top-md">

            <span class="ribbon-button-alignment">
                <a href="javascript:void(0);" id="show-shortcut" data-action="toggleShortcut" class="collapseMenu">
                    <i class="fa fa-reorder"></i>
                </a>
            </span>

            <!-- breadcrumb -->
            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="glyphicon glyphicon-home">
                </span>&nbsp;Home</asp:HyperLink></li>
                <li>
                    <asp:HyperLink ID="HlnkDesktop" runat="server" NavigateUrl="~/registrado/dashboard.aspx"><i class="fa fa-laptop">
                </i>&nbsp;Dashboard</asp:HyperLink></li>
            </ol>
            <!-- end breadcrumb -->

        </div>
        <!-- END RIBBON -->

        <!-- MAIN CONTENT -->
        <div id="content">
            <div class="row">
                <div class="col text-align-center">
                    <h1 class="page-title txt-color-blueDark"><i class="fa-fw fa fa-home"></i>
                        <asp:Label runat="server" ID="lbDashboardName"></asp:Label></h1>
                </div>
            </div>
            <div class="row">
                <div id="dashboard-message-warning">
                </div>
            </div>
            <!-- widget grid -->
            <div id="widget-grid" class="">
                <div class="row">
                    <article class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
                        <asp:ListView ID="lstWidgetsColLeft" runat="server">
                            <ItemTemplate>
                                <div class="jarviswidget" id='<%# "widget-" + Eval("IDWIDGET")%>' data-widget-editbutton="false" data-widget-fullscreenbutton="false" data-idwidget='<%#Eval("IDWIDGET")%>'>
                                    <header>
                                        <span class="widget-icon"><i class="glyphicon glyphicon-stats"></i></span>
                                        <h2><%#Eval("TITULO")%></h2>
                                    </header>
                                    <div>
                                        <div class="jarviswidget-editbox">
                                            <!-- This area used as dropdown edit box -->
                                            <input class="form-control" type="text">
                                            <span class="note"><i class="fa fa-check text-success"></i>Change title to update and save instantly!</span>
                                        </div>
                                        <!-- widget content -->
                                        <div class="widget-body no-padding">
                                            <div class="widget-container position-relative">
                                                <!-- widget toolbar -->
                                                <div class="widget-body-toolbar">
                                                    <div class="btn-group" data-toggle="buttons">
                                                        <label class="btn btn-default btn-sm btn-type-graph active" rel="tooltip" data-original-title="Pie" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnPieChart" value="Q" checked="checked" />
                                                            P
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Bar" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnBarChart" value="B" />
                                                            B
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Lines" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnLineChart" value="L" />
                                                            L
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Area" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnAreaChart" value="A" />
                                                            A
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Histogram" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnHistogram" value="H" />
                                                            H
                                                        </label>
                                                    </div>
                                                    <div class="btn-group" data-toggle="buttons">
                                                        <label class="btn btn-default btn-sm btn-type-time active" rel="tooltip" data-original-title="Day" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnDia" value="D" checked="checked" />
                                                            D
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Week" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnSemana" value="s" />
                                                            W
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Fortnight" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnQuincena" value="Q" />
                                                            F
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Month" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnMes" value="M" />
                                                            M
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Quarter" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnTrimestre" value="T" />
                                                            Q
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Half-Year" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnSemestre" value="S" />
                                                            S
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Year" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnAnual" value="A" />
                                                            Y
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="graphic-leyend">
                                                </div>
                                                <div class="graphic-chart" id='<%# "widget-graph-" + Eval("IDWIDGET")%>'>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end widget content -->
                                        <div class="widget-loading hidden">
                                            <div class="widget-loading-content">
                                                <img style="border-width: 0px;" src="../images/indicators/loading.gif" alt="...loading..." />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </article>
                    <article class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
                        <asp:ListView ID="lstWidgetsColMedium" runat="server">
                            <ItemTemplate>
                                <div class="jarviswidget" id='<%# "widget-" + Eval("IDWIDGET")%>' data-widget-editbutton="false" data-widget-fullscreenbutton="false" data-idwidget='<%#Eval("IDWIDGET")%>'>
                                    <header>
                                        <span class="widget-icon"><i class="glyphicon glyphicon-stats"></i></span>
                                        <h2><%#Eval("TITULO")%></h2>
                                    </header>
                                    <div>
                                        <!-- widget content -->
                                        <div class="widget-body no-padding">
                                            <div class="widget-container position-relative">
                                                <!-- widget toolbar -->
                                                <div class="widget-body-toolbar">
                                                    <div class="btn-group" data-toggle="buttons">
                                                        <label class="btn btn-default btn-sm btn-type-graph active" rel="tooltip" data-original-title="Pie" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnPieChart" value="Q" checked="checked" />
                                                            P
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Bar" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnBarChart" value="B" />
                                                            B
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Lines" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnLineChart" value="L" />
                                                            L
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Area" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnAreaChart" value="A" />
                                                            A
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Histogram" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnHistogram" value="H" />
                                                            H
                                                        </label>
                                                    </div>
                                                    <div class="btn-group" data-toggle="buttons">
                                                        <label class="btn btn-default btn-sm btn-type-time active" rel="tooltip" data-original-title="Day" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnDia" value="D" checked="checked" />
                                                            D
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Week" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnSemana" value="s" />
                                                            W
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Fortnight" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnQuincena" value="Q" />
                                                            F
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Month" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnMes" value="M" />
                                                            M
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Quarter" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnTrimestre" value="T" />
                                                            Q
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Half-Year" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnSemestre" value="S" />
                                                            S
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Year" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnAnual" value="A" />
                                                            Y
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="graphic-leyend">
                                                </div>
                                                <div class="graphic-chart" id='<%# "widget-graph-" + Eval("IDWIDGET")%>'>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end widget content -->
                                    </div>
                                    <div class="widget-loading hidden">
                                        <div class="widget-loading-content">
                                            <img style="border-width: 0px;" src="../images/indicators/loading.gif" alt="...loading..." />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </article>
                    <article class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
                        <asp:ListView ID="lstWidgetsColRight" runat="server">
                            <ItemTemplate>
                                <div class="jarviswidget" id='<%# "widget-" + Eval("IDWIDGET")%>' data-widget-editbutton="false" data-widget-fullscreenbutton="false" data-idwidget='<%#Eval("IDWIDGET")%>'>
                                    <header>
                                        <span class="widget-icon"><i class="glyphicon glyphicon-stats"></i></span>
                                        <h2><%#Eval("TITULO")%></h2>
                                    </header>
                                    <div>
                                        <!-- widget content -->
                                        <div class="widget-body no-padding">
                                            <div class="widget-container position-relative">
                                                <!-- widget toolbar -->
                                                <div class="widget-body-toolbar">
                                                    <div class="btn-group" data-toggle="buttons">
                                                        <label class="btn btn-default btn-sm btn-type-graph active" rel="tooltip" data-original-title="Pie" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnPieChart" value="Q" checked="checked" />
                                                            P
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Bar" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnBarChart" value="B" />
                                                            B
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Lines" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnLineChart" value="L" />
                                                            L
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Area" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnAreaChart" value="A" />
                                                            A
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-graph" rel="tooltip" data-original-title="Histogram" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnHistogram" value="H" />
                                                            H
                                                        </label>
                                                    </div>
                                                    <div class="btn-group" data-toggle="buttons">
                                                        <label class="btn btn-default btn-sm btn-type-time active" rel="tooltip" data-original-title="Day" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnDia" value="D" checked="checked" />
                                                            D
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Week" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnSemana" value="s" />
                                                            W
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Fortnight" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnQuincena" value="Q" />
                                                            F
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Month" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnMes" value="M" />
                                                            M
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Quarter" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnTrimestre" value="T" />
                                                            Q
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Half-Year" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnSemestre" value="S" />
                                                            S
                                                        </label>
                                                        <label class="btn btn-default btn-sm btn-type-time" rel="tooltip" data-original-title="Year" data-placement="bottom">
                                                            <input type="radio" name="options" class="btnAnual" value="A" />
                                                            Y
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="graphic-leyend">
                                                </div>
                                                <div class="graphic-chart" id='<%# "widget-graph-" + Eval("IDWIDGET")%>'>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end widget content -->
                                    </div>
                                    <div class="widget-loading hidden">
                                        <div class="widget-loading-content">
                                            <img style="border-width: 0px;" src="../images/indicators/loading.gif" alt="...loading..." />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </article>
                </div>
            </div>
            <!-- end widget grid -->
        </div>
        <!-- END MAIN CONTENT -->
    </div>
    <!-- END MAIN PANEL -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="Server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load('visualization', '1.0', { 'packages': ['corechart', 'table', 'gauge'] });
    </script>
    <!-- Para mostrar un selector de color para las fórmulas -->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/colorpicker/bootstrap-colorpicker.min.js") %>" type="text/javascript"></script>
    <!-- Para crear el widget que va a contener el gráfico-->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/smartwidgets/jarvis.widget.js") %>" type="text/javascript"></script>
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/dashboard.js") %>" type="text/javascript"></script>
</asp:Content>

