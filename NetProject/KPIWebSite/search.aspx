<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="search" %>

<%@ Register Src="~/controls/KPILibrary.ascx" TagName="KPILibrary" TagPrefix="kpi" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="ContentCenter" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <div class="container">
        <div class="row margin-top-lg">
            <div class="col-md-4 hidden-xs">
                <img alt="Busqueda de indicadores" class="img-responsive pull-right" src="images/search-banner.png" />
            </div>
            <div class="col-md-8 col-xs-12">
                <div class="search-form">
                    <h2>find</h2>
                    <h1>your dropkeys</h1>
                    <div class="search-text">
                        <asp:TextBox runat="server" ID="txtBuscarIndicador" placeholder="Write the dropkey you are looking for"
                            autofocus="autofocus" class="texto-buscar" ClientIDMode="Static"></asp:TextBox>
                        <asp:HyperLink runat="server" ID="btnBuscar" ClientIDMode="Static" CssClass="hidden-xs"><span class="glyphicon glyphicon-search"></span>&nbsp;Find</asp:HyperLink>
                    </div>
                    <div>
                        <asp:Label ID="lbNumIndicadores" runat="server" class="search-count"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lbFiltroCategoria" runat="server" class="search-category"> Selected category : --All--</asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row margin-top-lg margin-bottom-lg">
            <div class="col-md-8">
                <ul class="nav nav-pills-dropkeys">
                    <li>Sort by&nbsp;&nbsp;&nbsp;</li>
                    <li><a id="lnkOrderAtributoByFecha" href="#" title="Sort by creation date" data-orderby=" ORDER BY FECHA_ALTA DESC" class="attribute-order-by">Creation date</a></li>
                    <li><a id="lnkOrderByNumberOfData" href="#" title="Sort by the number of data" data-orderby=" ORDER BY NUM_VALORES DESC" class="attribute-order-by">Number of data</a></li>
                    <li><a id="lnkOrderByNumberOfUsers" href="#" title="Sort by the number of users sharing this dropkey" data-orderby=" ORDER BY NUM_USUARIOS DESC" class="attribute-order-by">Number of users</a></li>
                    <li><a id="lnkOrderByLastVisited" href="#" title="Sort by the most recently viewed" data-orderby=" ORDER BY ULTIMA_FECHA DESC" class="attribute-order-by">Last visited</a></li>
                </ul>

                <div id="PanelResultadosBusqueda" class="panel panel-dropkeys hide">
                    <div class="panel-body">
                        Sorry, we have not been able to find any dropkey that meets the specified requirements.
                    </div>
                    <asp:LoginView ID="LoginViewSearch" runat="server">
                        <AnonymousTemplate>
                            <a href="#" class="btn btn-dropkeys login-trigger" data-toggle="modal" data-target="#myModal"><span class="glyphicon glyphicon-user"></span>&nbsp;Sign up!</a>
                        </AnonymousTemplate>
                        <LoggedInTemplate>
                            <asp:LinkButton runat="server" ID="btnCreate2" class="btn btn-dropkeys" PostBackUrl="~/registrado/createkpi.aspx"><span class="glyphicon glyphicon-plus"></span>&nbsp;New dropkey</asp:LinkButton>
                        </LoggedInTemplate>
                    </asp:LoginView>
                </div>
                <div id="listaIndicadores">
                    <asp:ListView ID="lstIndicadores" runat="server" DataKeyNames="INDICATORID">
                        <ItemTemplate>
                            <div class="search">
                                <div class="col-md-2">
                                    <div class="circular width80" style='border: 5px solid <%# Eval("ESTILO") %>;'>
                                        <asp:Image ID="imgIndicator" runat="server" ImageUrl='<%# Eval("IMAGEURL") %>' AlternateText='<%# Eval("TITULO") %>' Width="100" />
                                    </div>
                                </div>
                                <div class="col-md-10">
                                    <h4>
                                        <asp:HyperLink ID="lbTitulo" Text='<%# Eval("TITULO") %>' runat="server" NavigateUrl='<%# "~/indicator.aspx?indicatorid=" + Eval("INDICATORID") %>'></asp:HyperLink></h4>
                                    <p>
                                        <asp:Label ID="lbResumen" runat="server" Text='<%# Eval("RESUMEN") %>' CssClass="line-fade"></asp:Label>
                                    </p>
                                    <div class="iconos hidden-xs">
                                        <asp:Label runat="server" title="Creation date"><span class="glyphicon glyphicon-calendar"></span>
                                        <%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA_ALTA")))%></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server"
                                            title="Latest data modified"><span class="glyphicon glyphicon-eye-open"></span>
                                            <%# CalculaFechaDesdeCuando(Convert.ToString(Eval("ULTIMA_FECHA")))%></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server"
                                                title="Number of users sharing this dropkey"><i class="fa fa-group"></i><span class="num-users" data-indicadorid='<%# Eval("INDICATORID") %>'>
                                                    <%# Eval("NUM_USUARIOS")%></span></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" title="Number of comments"><span
                                                        class="glyphicon glyphicon-comment"></span>
                                                        <%# Eval("NUM_COMENTARIOS")%></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" title="Current rating"><span
                                                            class="glyphicon glyphicon-star"></span>
                                                            <%# Eval("RATING")%></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" title="Measure unit"><span
                                                                class="glyphicon glyphicon-signal"></span>
                                                                <%# Eval("UNIDAD")%></asp:Label>
                                    </div>
                                    <asp:LoginView ID="LoginViewKPIIndicators" runat="server">
                                        <LoggedInTemplate>
                                            <asp:HyperLink ID="lnkAddIndicator" runat="server" Visible='<%# Convert.ToBoolean(Eval("ASIGNADO").ToString() == "0") %>' CssClass="add-indicator" NavigateUrl="#" data-indicadorid='<%# Eval("INDICATORID") %>' data-nombre='<%# Eval("TITULO") %>'><span class="glyphicon glyphicon-plus"></span>&nbsp;Add</asp:HyperLink>
                                            <asp:HyperLink ID="lnkShareIndicador" runat="server" Visible='<%# Convert.ToBoolean(Eval("COMPARTIDO").ToString() == "N") %>' CssClass="share-indicator" NavigateUrl='<%# "~/registrado/sharekpi.aspx?indicatorid=" + Eval("INDICATORID") %>'><span class="glyphicon glyphicon-share"></span>&nbsp;Share</asp:HyperLink>
                                        </LoggedInTemplate>
                                    </asp:LoginView>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <div id="divloadingMessage" class="text-center well margin-top-md hidden">
                    <i class="fa fa-refresh fa-spin"></i>&nbsp;...loading data...
                </div>
            </div>
            <div class="col-md-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title"><span class="glyphicon glyphicon-th-list"></span>&nbsp;CATEGORIES</h3>
                    </div>
                    <div class="list-group">
                        <a href="#" class="list-group-item licategory active" data-categoryid="-1" title="Show all the dropkeys"><span class="nombre">--All--</span><asp:Label ID="lbNumDatos" runat="server" CssClass="badge"></asp:Label>
                        </a>
                    </div>
                    <asp:ListView ID="lstCategorias" runat="server">
                        <ItemTemplate>
                            <div class="list-group" data-categoryid='<%# Eval("CATEGORYID") %>'>
                                <a href="#" class='list-group-item <%# Eval("CLASE") %>' data-categoryid='<%# Eval("CATEGORYID") %>' data-subcategoryid='<%# Eval("SUBCATEGORYID") %>' title='<%# Eval("DESCRIPCION") %>'>
                                    <span class="nombre"><%# Eval("NOMBRE") %></span><span class="badge" style='background: <%# Eval("ESTILO") %>;'><%# Eval("RECUENTO") %></span>
                                </a>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <asp:LoginView ID="LoginViewKPIBoard" runat="server">
                    <AnonymousTemplate>
                        <div class="panel panel-dropkeys">
                            <div class="panel-body">
                                <h3>CAN'T FIND WHAT YOU ARE LOOKING FOR?</h3>
                                <asp:HyperLink runat="server" ID="btnUserRegistry2" class="btn btn-dropkeys login-trigger" data-toggle="modal" data-target="#myModal"><span class="glyphicon glyphicon-user"></span>&nbsp;Sign up!</asp:HyperLink>
                            </div>
                        </div>
                    </AnonymousTemplate>
                    <LoggedInTemplate>
                        <div class="panel panel-dropkeys">
                            <div class="panel-body">
                                <h3>CAN'T FIND WHAT YOU ARE LOOKING FOR?</h3>
                                <asp:LinkButton runat="server" ID="btnCreate" class="btn btn-dropkeys" PostBackUrl="~/registrado/createkpi.aspx"><span class="glyphicon glyphicon-plus"></span>&nbsp;New Dropkey</asp:LinkButton>
                            </div>
                        </div>
                    </LoggedInTemplate>
                </asp:LoginView>
                <div class="bloque-estadisticas">
                    <h2><strong>Statistics</strong></h2>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="title-text">
                                DROPKEYS
                            </div>
                            <div class="title_desc">
                                On database
                            </div>
                            <div class="title-number">
                                <strong>
                                    <asp:Label ID="lbTotalDropkeys" runat="server" Text="0"></asp:Label></strong>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="title-text">
                                DATASETS
                            </div>
                            <div class="title_desc">
                                On database
                            </div>
                            <div class="title-number">
                                <strong><asp:Label ID="lbTotalDatasets" runat="server" Text="0"></asp:Label></strong>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="margintop">
                    <kpi:KPILibrary ID="KPILibreria" runat="server" />
                </div>
                <div id="whatever">
                    <asp:ListView ID="lstEtiquetas" runat="server">
                        <ItemTemplate>
                            <asp:HyperLink ID="LnkEtiqueta" runat="server" NavigateUrl='<%# "~/search.aspx?tagstring=" + Eval("NOMBRE") %>'
                                Text='<%#  Eval("NOMBRE") %>' rel='<%#  Eval("TAMANO") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="frmAddIndicador" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">
                        <img src="images/buttons/check.png" alt="exito" />&nbsp;Dropkey correctly added to your library</h4>
                </div>
                <div class="modal-body">
                    <div role="form">
                        <div class="alert alert-success fade in">
                            <i class="fa-fw fa fa-check"></i>
                            <strong>Congratulations!</strong> The dropkey has been inserted into your library. Now, you can introduce your data and view the result. 
                        </div>
                        <div class="checkbox">
                            <label>
                                <input id="cbIncluirEnGrafico" type="checkbox" value="ok" />
                                Do you want to include it in a graph?
                            </label>
                        </div>
                        <div id="panelIncluirEnGrafico" class="hide">
                            <div class="form-group">
                                <label for="txtNombre">Name</label>
                                <asp:TextBox runat="server" ID="txtNombre" Enabled="false" placeholder="The Dropkey's name"
                                    ClientIDMode="Static" MaxLength="200" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="cmbAggFuncion">Aggregate function</label>
                                <asp:DropDownList ID="cmbAggFuncion" runat="server" Enabled="false" ClientIDMode="Static"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="cmbDashboard">Dashboard</label>
                                <asp:DropDownList ID="cmbDashboard" runat="server" Enabled="false" ClientIDMode="Static"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="cmbWidget">Chart</label>
                                <asp:DropDownList ID="cmbWidget" runat="server" Enabled="false" ClientIDMode="Static"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a id="btnTerminar" class="btn btn-dropkeys pull-right" href="#">Finish</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentScript" runat="server" ContentPlaceHolderID="ScriptsContentPlaceHolder">
    <!-- Librerías externas -->
    <script src="<%=Page.ResolveUrl("~/scripts/jquery/jquery.tagcloud.js") %>" type="text/javascript"></script>
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/search.js") %>" type="text/javascript"></script>
</asp:Content>
