<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master" AutoEventWireup="true" CodeFile="desktop.aspx.cs" Inherits="registrado_desktop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
    <link rel="stylesheet" type="text/css" href="/content/css/jquery.datepick.css" />
    <link rel="stylesheet" type="text/css" href="/content/css/bootstrap-colorpicker.css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <!-- Barra lateral izquierda -->
    <aside id="left-panel">
        <!-- User info -->
        <div class="login-info">
            <span>
                <!-- User image size is adjusted inside CSS, it should stay as it -->
                <a href="javascript:void(0);">
                    <asp:Image runat="server" ID="imgProfileUser" alt="User avatar profile" class="online" />
                    <span>
                        <asp:Label ID="lbNombreUsuario" runat="server"></asp:Label>
                    </span>
                </a>
            </span>
        </div>
        <!-- end user info -->
        <nav class="dashboards">
            <ul>
                <li>
                    <a href="#" class="add-dashboard" title="Add a new dashboard"><i class="fa fa-lg fa-fw fa-plus"></i><span class="menu-item-parent">New dashboard</span></a>
                </li>
                <asp:ListView ID="lstDashboards" runat="server" OnItemDataBound="lstDashboards_ItemDataBound"
                    DataKeyNames="IDDASHBOARD">
                    <ItemTemplate>
                        <li class="dashboards-items" data-iddashboard='<%# Eval("IDDASHBOARD") %>'>
                            <a href="#" class="dashboard-menu-item" title="Dropkeys dashboard"><i class="fa fa-lg fa-fw fa-bar-chart-o"></i><span class="menu-item-parent max-width-dashboard"><%# Eval("TITULO") %></span></a>
                            <ul class="widget-list" data-iddashboard='<%# Eval("IDDASHBOARD") %>'>
                                <li>
                                    <a href="#" class="txt-color-blue"><i class="fa fa-sitemap"></i>&nbsp;Dashboard options</a>
                                    <ul>
                                        <li>
                                            <a href="#" class="edit-dashboard" title="Change dashboard name" data-iddashboard='<%# Eval("IDDASHBOARD") %>'><i class="fa fa-lg fa-fw fa-pencil txt-color-blue"></i>Rename dashboard</a>
                                        </li>
                                        <li>
                                            <a href="#" class="delete-dashboard" title="Delete current dashboard" data-iddashboard='<%# Eval("IDDASHBOARD") %>'><i class="fa fa-lg fa-fw fa-times txt-color-redLight"></i>Delete dashboard</a>
                                        </li>
                                        <li>
                                            <a href="#" class="insert-widget" title="Insert new graph" data-iddashboard='<%# Eval("IDDASHBOARD") %>'><i class="fa fa-lg fa-fw fa-plus txt-color-greenLight"></i>Insert graph</span></a>
                                        </li>
                                    </ul>
                                </li>
                                <asp:ListView ID="lstWidgets" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <a href="#" class="widget-menu-item" title="Dropkeys widget" data-idwidget='<%# Eval("IDWIDGET") %>'>
                                                <span class="menu-item-parent max-width-dashboard"><%# Eval("TITULO") %></span>
                                            </a>
                                        </li>
                                    </ItemTemplate>
                                </asp:ListView>
                            </ul>
                        </li>
                    </ItemTemplate>
                </asp:ListView>
            </ul>
        </nav>
    </aside>

    <!-- MAIN PANEL -->
    <div id="main" role="main">

        <!-- RIBBON -->
        <div id="ribbon">
            <span class="ribbon-button-alignment">
                <span>
                    <a href="javascript:void(0);" data-action="toggleMenu" title="Collapse Menu" class="collapseMenu"><i class="fa fa-reorder"></i></a>
                </span>
            </span>
            <!-- breadcrumb -->
            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="glyphicon glyphicon-home">
                </span>&nbsp;Home</asp:HyperLink></li>
                <li>
                    <asp:HyperLink ID="HlnkDesktop" runat="server" NavigateUrl="~/registrado/desktop.aspx"><i class="fa fa-laptop">
                </i>&nbsp;Desktop</asp:HyperLink></li>
            </ol>
            <!-- end breadcrumb -->
        </div>
        <!-- END RIBBON -->

        <!-- MAIN CONTENT -->
        <section>
            <div id="content" class="hidden">
                <div class="row">
                    <!-- col -->
                    <div class="col">
                        <h3 class="page-title txt-color-blueDark">
                            <!-- PAGE HEADER -->
                            <i class="fa-fw fa fa-bar-chart-o"></i>
                            <asp:Label ID="lbCurrentDashboard" ClientIDMode="Static" runat="server" Text="None"></asp:Label>
                            <asp:Label ID="lbCurrentWidget" ClientIDMode="Static" runat="server" Text="> none"></asp:Label>
                        </h3>
                    </div>
                </div>
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
                                    <div role="menu" class="widget-toolbar">
                                        <div class="btn-group">
                                            <a class="txt-color-blueDark" title="" data-toggle="dropdown"><i class="fa fa-gears"></i></a>
                                            <ul class="dropdown-menu pull-right">
                                                <li>
                                                    <a id="btnNewFormulaDialog" href="#"><i class="fa fa-plus"></i>&nbsp;New formula</a>
                                                </li>
                                                <li>
                                                    <a id="btnRenameWidget" href="#"><i class="fa fa-pencil"></i>&nbsp;Rename</a>
                                                </li>
                                                <li class="divider"></li>
                                                <li>
                                                    <a id="btnShowHideLegend" href="#"><i class="fa fa-eye"></i>&nbsp;Show formulas</a>
                                                </li>
                                                <li>
                                                    <a id="btnDownloadCSV" href="#"><i class="fa fa-download"></i>&nbsp;Download CSV</a>
                                                </li>
                                                <li>
                                                    <a id="btnShareWidget" href="#"><i class="fa fa-share-alt"></i>&nbsp;Share</a>
                                                </li>
                                                <li class="divider"></li>
                                                <li>
                                                    <a id="btnDeleteWidget" href="#"><i class="fa fa-times"></i>&nbsp;Delete</a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                    <h2 class="widget-title">Widget Title</h2>
                                </header>
                                <!-- widget div-->
                                <div>
                                    <!-- widget edit box -->
                                    <div class="jarviswidget-editbox">
                                        <!-- This area used as dropdown edit box -->
                                        <div class="row">
                                            <div class="col-lg-9 col-md-8">
                                                <input class="form-control" type="text" id="txtRenameWidget" />
                                                <span class="note"><i class="fa fa-check text-success"></i>Change title to update and save instantly!</span>
                                            </div>
                                            <div class="col-lg-3 col-md-4">
                                                <a id="btnSaveRenameWidget" class="btn btn-success" href="#">
                                                    <i class="glyphicon glyphicon-ok"></i>&nbsp;Save
                                                </a>
                                                <a id="btnCancelRenameWidget" class="btn btn-default" href="#">
                                                    <i class="fa fa-reply"></i>&nbsp;Cancel
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- end widget edit box -->
                                    <!-- widget content -->
                                    <div class="widget-body no-padding hidden">
                                        <!-- WIDGET CONTAINER -->
                                        <div class="widget-container position-relative">
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
                                                <div class="btn-group pull-right smart-form" data-toggle="buttons">
                                                    <label class="input font-xs">
                                                        <i class="icon-prepend fa fa-calendar"></i>
                                                        <asp:TextBox ID="txtDateRange" runat="server" ClientIDMode="Static" CssClass="daterange" ReadOnly="true" placeholder="Especify a time range..."></asp:TextBox>
                                                    </label>

                                                </div>
                                            </div>
                                            <!-- widget tabpanel -->
                                            <div class="tabbable tabs-below hidden">
                                                <div class="tab-content padding-10">
                                                    <div class="tab-pane min-heigh fade active in" id="AA">
                                                        <!-- CHAT CONTAINER -->
                                                        <div id="chat-container">
                                                            <span class="chat-list-open-close"><i class="fa fa-user"></i><b>!</b></span>
                                                            <div class="dropkeys-shortlist-container">
                                                                <ul class="dropkeys-shortlist">
                                                                    <asp:ListView ID="lstFriends" runat="server" EnableViewState="True">
                                                                        <ItemTemplate>
                                                                            <li>
                                                                                <asp:HyperLink ID="HlnkFriend" CssClass="show-modal-info-user" data-userid='<%# Eval("USERID")%>' runat="server" NavigateUrl="#"><asp:Image runat="server" ImageUrl='<%# Eval("IMAGEURL")%>' /><%# Eval("NOMBRE")%>&nbsp;<%# Eval("APELLIDOS")%></asp:HyperLink>
                                                                            </li>
                                                                        </ItemTemplate>
                                                                        <EmptyDataTemplate>
                                                                            <li class="empty">
                                                                                <span class="list-group-item">There are no yet friends</span>
                                                                            </li>
                                                                        </EmptyDataTemplate>
                                                                    </asp:ListView>
                                                                </ul>
                                                            </div>
                                                            <div class="dropkeys-shortlist-search">
                                                                <div class="control-group">
                                                                    <div class="smart-form">
                                                                        <label class="input">
                                                                            <input type="text" id="txtSearchFriends" placeholder="Search..." />
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div id="graphic-leyend" class="graphic-leyend">
                                                        </div>
                                                        <div id="graphic-chart">
                                                        </div>
                                                        <div id="graphic-timeline" class="margin-left-10 margin-right-10">
                                                            <input type="text" id="time-slider" name="time-slider" value="" />
                                                        </div>
                                                        <div class="show-stat-microcharts">
                                                            <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">
                                                                <div style="width: 50px; height: 50px; line-height: 50px;" class="easy-pie-chart txt-color-orangeDark easyPieChart userPercent" data-percent="60" data-pie-size="50">
                                                                    <span class="userPercent percent percent-sign">32</span>
                                                                    <canvas width="50" height="50"></canvas>
                                                                </div>
                                                                <span class="easy-pie-title">Users&nbsp;<i class="fa fa-caret-up icon-color-bad"></i></span>
                                                                <ul class="smaller-stat hidden-sm pull-right">
                                                                    <li><span class="info">Included users</span>&nbsp;<span class="label bg-color-greenLight userIncluded"><i class="fa fa-caret-up"></i>97%</span>
                                                                    </li>
                                                                    <li><span class="info">Available users</span>&nbsp;<span class="label bg-color-blueLight userAvailable"><i class="fa fa-caret-down"></i>44%</span>
                                                                    </li>
                                                                </ul>
                                                                <div class="sparkline txt-color-greenLight hidden-sm hidden-md pull-right" data-sparkline-type="line" data-sparkline-height="33px" data-sparkline-width="70px" data-fill-color="transparent">
                                                                    <canvas height="33" width="70" style="display: inline-block; width: 70px; height: 33px; vertical-align: top;"></canvas>
                                                                </div>
                                                            </div>
                                                            <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">
                                                                <div style="width: 50px; height: 50px; line-height: 50px;" class="easy-pie-chart txt-color-greenLight easyPieChart datasetPercent" data-percent="78.9" data-pie-size="50">
                                                                    <span class="datasetPercent percent percent-sign">77</span>
                                                                    <canvas width="50" height="50"></canvas>
                                                                </div>
                                                                <span class="easy-pie-title">Datasets&nbsp;<i class="fa fa-caret-down icon-color-good"></i></span>
                                                                <ul class="smaller-stat hidden-sm pull-right">
                                                                    <li>
                                                                        <span class="info">Included datasets</span>&nbsp;<span class="label bg-color-blueDark datasetIncluded"><i class="fa fa-caret-up"></i>76%</span>
                                                                    </li>
                                                                    <li>
                                                                        <span class="info">Available datasets</span>&nbsp;<span class="label bg-color-blue datasetAvailable"><i class="fa fa-caret-down"></i>3%</span>
                                                                    </li>
                                                                </ul>
                                                                <div class="sparkline txt-color-blue hidden-sm hidden-md pull-right" data-sparkline-type="line" data-sparkline-height="33px" data-sparkline-width="70px" data-fill-color="transparent">
                                                                    <canvas height="33" width="70" style="display: inline-block; width: 70px; height: 33px; vertical-align: top;"></canvas>
                                                                </div>
                                                            </div>
                                                            <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">
                                                                <div style="width: 50px; height: 50px; line-height: 50px;" class="easy-pie-chart txt-color-blue easyPieChart valuePercent" data-percent="23" data-pie-size="50">
                                                                    <span class="valuePercent percent percent-sign">22</span>
                                                                    <canvas width="50" height="50"></canvas>
                                                                </div>
                                                                <span class="easy-pie-title">Values&nbsp;<i class="fa fa-caret-up icon-color-good"></i></span>
                                                                <ul class="smaller-stat hidden-sm pull-right">
                                                                    <li>
                                                                        <span class="info">Included values</span>&nbsp;<span class="label bg-color-darken valueIncluded">10GB</span>
                                                                    </li>
                                                                    <li>
                                                                        <span class="info">Available values</span>&nbsp;<span class="label bg-color-blueDark valueAvailable"><i class="fa fa-caret-up"></i>10%</span>
                                                                    </li>
                                                                </ul>
                                                                <div class="sparkline txt-color-darken hidden-sm hidden-md pull-right" data-sparkline-type="line" data-sparkline-height="33px" data-sparkline-width="70px" data-fill-color="transparent">
                                                                    <canvas height="33" width="70" style="display: inline-block; width: 70px; height: 33px; vertical-align: top;"></canvas>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="tab-pane min-heigh fade" id="BB">
                                                        <div id="graphic-table"></div>
                                                    </div>
                                                    <div class="tab-pane min-heigh fade" id="CC">
                                                        <div id="vector-map" class="vector-map"></div>
                                                    </div>
                                                </div>
                                                <ul class="nav nav-tabs" id="myTab">
                                                    <li class="active">
                                                        <a data-toggle="tab" href="#AA"><i class="fa fa-bar-chart-o"></i>&nbsp;Chart</a>
                                                    </li>
                                                    <li class="">
                                                        <a data-toggle="tab" href="#BB"><i class="fa fa-table"></i>&nbsp;Table</a>
                                                    </li>
                                                    <li class="">
                                                        <a data-toggle="tab" href="#CC"><i class="fa fa-globe "></i>&nbsp;Map</a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- end widget content -->
                                </div>
                                <!-- end widget div -->
                            </div>
                            <!-- end widget -->
                        </article>
                    </div>
                </div>
                <!-- end widget grid -->
            </div>
            <div id="content-none">
                <div class="container">
                    <section class="content-section-b">
                        <div class="row">
                            <div class="col-lg-12 text-center">
                                <h2 class="section-heading">START WORKING WITH DROPKEYS</h2>
                                <hr class="star-primary" />
                                <h3 class="section-subheading text-muted">A good decision is based on knowledge and not on numbers. - Plato -</h3>
                            </div>
                            <div class="col-lg-12 text-center">
                                <asp:Image runat="server" ID="imgDesktop" ClientIDMode="Static" class="img-responsive display-inline animate-title" ImageUrl="~/images/desktop.jpg" alt="" />
                            </div>
                        </div>
                        <div class="row text-center margin-top-lg">
                            <div id="imgColLeft" class="col-md-4">
                                <span class="fa-stack fa-4x">
                                    <i class="fa fa-circle fa-stack-2x txt-color-blueMedium"></i>
                                    <i class="fa fa-bar-chart-o fa-stack-1x fa-inverse"></i>
                                </span>
                                <h4 class="service-heading">CREATE A NEW DASHBOARD</h4>
                                <p>
                                    Press the button called <i>"New dashboard"</i> and specify a name no longer than 100 characters on the modal dialog in order to add a new dashboard into our database.
                                </p>
                            </div>
                            <div id="imgColMed" class="col-md-4">
                                <span class="fa-stack fa-4x">
                                    <i class="fa fa-circle fa-stack-2x txt-color-blueMedium"></i>
                                    <i class="fa fa-signal fa-stack-1x fa-inverse"></i>
                                </span>
                                <h4 class="service-heading">ADD SOME WIDGETS</h4>
                                <p>
                                    Insert as many graphs as you want into the recently created dashboard.
                                </p>
                            </div>
                            <div id="imgColRight" class="col-md-4">
                                <span class="fa-stack fa-4x">
                                    <i class="fa fa-circle fa-stack-2x txt-color-blueMedium"></i>
                                    <i class="fa fa-flask fa-stack-1x fa-inverse"></i>
                                </span>
                                <h4 class="service-heading">AND DEFINE FORMULAS</h4>
                                <p>
                                    Combine as many Dropkeys as you want and build complex formulas with them. Apply filters on the Dropkey's attributes and limit the resultsets to the target of your studies.
                                </p>
                            </div>
                        </div>
                    </section>
                    <section class="content-section-b">
                        <div class="row">
                            <div class="col-lg-12 text-center">
                                <h2 class="section-heading">SHARE YOUR WIDGETS ON ANY WEBSITE</h2>
                                <hr class="star-primary" />
                                <h3 class="section-subheading text-muted">A good decision is based on knowledge and not on numbers. - Plato -</h3>
                            </div>
                            <div class="col-lg-12 text-center">
                                <asp:Image runat="server" ID="imgiFrame" ClientIDMode="Static" class="img-responsive display-inline animate-title" ImageUrl="~/images/dropkey-iframe.jpg" alt="" />
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </section>
        <!-- END MAIN CONTENT -->

    </div>
    <!-- END MAIN PANEL -->

    <!-- Modal -->
    <div id="frmAddDashboard" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title"><i class="fa fa-lg fa-fw fa-bar-chart-o"></i>&nbsp;Add or rename a dashboard...</h4>
                </div>
                <div class="modal-body">
                    <div role="form">
                        <div class="form-group">
                            <label for="txtNombre">Name of the dashboard</label>
                            <asp:TextBox runat="server" ID="txtNewDashboardName" placeholder="Name of the dashboard" ClientIDMode="Static" MaxLength="100" CssClass="form-control"></asp:TextBox>
                            <p class="note"><strong>Note:</strong> maximun 100 characters</p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a id="btnSaveNewDashboard" class="btn btn-dropkeys pull-right" href="#">Finish</a>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="frmAddFormula" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">
                        <i class="fa fa-lg fa-fw fa-flask"></i>&nbsp;<asp:Label ID="lbNombreDeFormula" runat="server" ClientIDMode="Static" /></h4>
                </div>
                <div class="modal-body">
                    <div class="form-inline">
                        <fieldset>
                            <legend>Especify a Name and its representative color</legend>
                            <div class="form-group">
                                <label class="sr-only"></label>
                                <asp:TextBox runat="server" ID="txtNombreFormula" placeholder="Formula's name" ClientIDMode="Static" MaxLength="200" Width="510" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group pickercolor">
                                <input type="text" id="txtFormulaColor" value="#596D82" class="form-control hidden" />
                                <span class="input-group-addon no-border"><i></i></span>
                            </div>
                        </fieldset>
                    </div>
                    <div class="form-inline">
                        <fieldset>
                            <legend>Introduce the Formula Expression</legend>
                            <div class="well" role="search">
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtSearchIndicators" placeholder="Search indicators..." ClientIDMode="Static" Width="270" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <select class="form-control" id="lstAggreggateFunction">
                                        <option value="NONE">My data</option>
                                        <option value="AVG">World Average</option>
                                        <option value="MAX">World Maximum</option>
                                        <option value="MIN">World Minimum</option>
                                    </select>
                                </div>
                                <a id="btnAddIndicator" href="#" class="btn btn-default"><i class="fa fa-check"></i></a>
                                <a id="btnClearIndicator" href="#" class="btn btn-default"><i class="fa fa-times"></i></a>
                            </div>
                        </fieldset>
                    </div>
                    <div class="form-inline">
                        <fieldset>
                            <asp:TextBox ID="txtValorFormula" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                                Height="50" Width="100%"></asp:TextBox>
                        </fieldset>
                    </div>
                </div>
                <div class="modal-footer">
                    <a id="btnSaveFormula" class="btn btn-dropkeys pull-right" href="#">Finish</a>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="frmAddFilter" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title"><i class="fa fa-lg fa-fw fa-filter"></i>&nbsp;Filter dialog</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <span id="formulagraphic"></span>
                    </div>
                    <div class="form-horizontal">
                        <fieldset>
                            <legend>Especify the dropkey you want to filter on</legend>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="select-1">Available dropkeys</label>
                                <div class="col-lg-8">
                                    <asp:DropDownList ID="cmbFilterIndicators" runat="server" ClientIDMode="Static" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div>
                        <div class="well text-center">
                            <div class="kpi-new-filter">
                            </div>
                        </div>
                        <div class="kpi-no-filter">
                            Lo sentimos, pero no existen características asociadas a este indicador sobre las
                                    que aplicar ningún filtro
                        </div>
                        <div id="filter-list">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a id="btnExitFilter" class="btn btn-dropkeys pull-right" href="#">Finish</a>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="frmShareWidget" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title"><i class="fa fa-lg fa-fw fa-share-alt"></i>&nbsp;Share graph...</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <fieldset>
                            <legend>Configuration</legend>
                            <div class="form-group">
                                <label class="col-md-2 control-label">Width</label>
                                <div class="col-md-4">
                                    <input id="txtiFrameWidth" class="form-control" placeholder="Please, enter the width..." type="text" value="600">
                                </div>
                                <label class="col-md-2 control-label">Height</label>
                                <div class="col-md-4">
                                    <input id="txtiFrameHeight" class="form-control" placeholder="Please, enter the height..." type="text" value="400">
                                </div>
                            </div>
                            <div>
                                <a id="btnGenerateiFrame" class="btn btn-dropkeys pull-right" href="#">Generate</a>
                            </div>
                        </fieldset>
                    </div>
                    <div class="form">
                        <fieldset>
                            <legend>Copy and paste this code into your website</legend>
                            <div class="form-group">
                                <asp:TextBox runat="server" ID="txtShareWidget" ClientIDMode="Static" ReadOnly="true" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="Server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load('visualization', '1.0', { 'packages': ['corechart', 'table', 'gauge'] });
    </script>
    <!-- Vector Maps Plugin: Vectormap engine, Vectormap language -->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js") %>" type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js") %>" type="text/javascript"></script>
    <!-- Para mostrar un selector de color para las fórmulas -->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/colorpicker/bootstrap-colorpicker.min.js") %>" type="text/javascript"></script>
    <!-- Para crear mini-gráficos representativos de los últimos 10 datos de un dataset-->
    <!--<script src="<%=Page.ResolveUrl("~/scripts/plugin/sparkline/jquery.sparkline.min.js") %>" type="text/javascript"></script>-->
    <!-- Para mostrar las gráficas circulares del número de usuarios/datasets/datos -->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js") %>" type="text/javascript"></script>
    <!-- Para crear el widget que va a contener el gráfico-->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/smartwidgets/jarvis.widget.js") %>" type="text/javascript"></script>
    <!-- Para mostrar el time-line para las gráficas lineales -->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/rangeslider/ion.rangeSlider.min.js") %>" type="text/javascript"></script>
    <!-- Para mostrar las formulas en formato gráfico -->
    <script src="<%=Page.ResolveUrl("~/scripts/varios/jqmath-etc-0.4.0.min.js") %>" type="text/javascript"></script>
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/desktop.js") %>" type="text/javascript"></script>
</asp:Content>

