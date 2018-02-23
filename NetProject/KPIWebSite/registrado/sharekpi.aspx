<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="sharekpi.aspx.cs" Inherits="registrado_sharekpi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <asp:HiddenField ID="hdnIndicatorID" runat="server" ClientIDMode="Static" />
    <div class="container margin-top-lg margin-bottom-lg">
        <h1>
            <asp:Label ID="lbTitulo" runat="server" CssClass="color-primario-foreground"></asp:Label></h1>
        <ul class="breadcrumb">
            <li class="first">
                <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="glyphicon glyphicon-home">
                </span>&nbsp;Home</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="HlnkBuscador" runat="server" NavigateUrl="~/search.aspx"><i class="fa fa-search">
                </i>&nbsp;Search</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="HlnkIndicador" runat="server" NavigateUrl="~/indicator.aspx" ClientIDMode="Static">
                    <i class="fa fa-tachometer"></i>&nbsp;<asp:Label ID="lbIndicatorName"
                        runat="server"></asp:Label>
                </asp:HyperLink></li>
            <li><span class="current-page"><span class="glyphicon glyphicon-save"></span>&nbsp;<strong>Share indicator</strong></span></li>
        </ul>
        <div class="well">
            <img src="../images/indicators/indicador.png" alt="Dropkeys indicator big" class="pull-left margin-right-10" />
            <h5>Welcome to the share indicator assistance. A wizard will guide you through a series of simple steps to help you sharing the indicator through the community</h5>
        </div>
        <div class="widget-body fuelux">
            <div class="wizard" id="ShareWizard">
                <ul class="steps">
                    <li class="active" data-target="#step1"><span class="badge">1</span>Related indicators<span class="chevron"></span></li>
                    <li data-target="#step2"><span class="badge">2</span>Representative image<span class="chevron"></span></li>
                    <li data-target="#step3"><span class="badge">3</span>Select the subcategory<span class="chevron"></span></li>
                </ul>
                <div class="actions">
                    <button class="btn btn-sm btn-primary btn-prev" type="button"><span class="glyphicon glyphicon-arrow-left"></span>&nbsp;Prev</button>
                    <button class="btn btn-sm btn-success btn-next" data-last="Complete&nbsp;" type="button">Next&nbsp;<span class="glyphicon glyphicon-arrow-right"></span></button>
                </div>
            </div>
            <div class="step-content well min-height-lg">
                <div class="form-horizontal">
                    <div id="step1" class="step-pane active">
                        <h3 class="StepTitle margin-top-0 margin-bottom-0"><strong>Step 1. </strong>Similar indicators found in the database</h3>
                        <p>
                            Please make sure that the indicator you are trying to share with the rest of users do not match any existing in our database. <strong>Only </strong>if so, <strong>continue </strong>with the wizard.
                        </p>
                        <asp:UpdatePanel ID="UPIndicadores" runat="server">
                            <ContentTemplate>
                                <div class="DataPager">
                                    <span class="orden">
                                        <asp:Literal ID="lbNumIndicadores" runat="server" Text="N possible indicators has been founded"></asp:Literal></span>
                                    <asp:DataPager ID="DataPagerDespues" runat="server" PagedControlID="lstIndicadores"
                                        PageSize="3" OnPreRender="DataPagerIndicadores_PreRender">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonType="Image" ShowFirstPageButton="false" ShowPreviousPageButton="true"
                                                PreviousPageImageUrl="~/images/buttons/leftarrow.png" PreviousPageText="Anterior"
                                                ShowLastPageButton="false" ShowNextPageButton="false" />
                                            <asp:NumericPagerField CurrentPageLabelCssClass="current" ButtonCount="10" NextPageText=">"
                                                PreviousPageText="<" RenderNonBreakingSpacesBetweenControls="false" />
                                            <asp:NextPreviousPagerField ButtonType="Image" ShowNextPageButton="true" NextPageImageUrl="~/images/buttons/rightarrow.png"
                                                NextPageText="Siguiente" ShowPreviousPageButton="false" />
                                        </Fields>
                                    </asp:DataPager>
                                    <div class="clear">
                                    </div>
                                </div>
                                <asp:Panel ID="PanelResultadosBusqueda" runat="server" CssClass="margin-top-10">
                                    <div class="alert alert-success alert-block">
                                        <a class="close" data-dismiss="alert" href="#">×</a>
                                        <h4 class="alert-heading">
                                            <asp:Label ID="lbTextoBusqueda" runat="server"></asp:Label></h4>
                                        <asp:Label ID="lbResultadosBusqueda" runat="server"></asp:Label>
                                    </div>
                                </asp:Panel>
                                <asp:ListView ID="lstIndicadores" runat="server" DataKeyNames="INDICATORID">
                                    <ItemTemplate>
                                        <div class="search">
                                            <div class="col-md-2 hidden-xs">
                                                <div class="circular width80" style='border: 5px solid <%# Eval("ESTILO") %>;'>
                                                    <asp:Image ID="IndicatorImage" runat="server" ImageUrl='<%# Eval("IMAGEURL") %>' AlternateText='<%# Eval("TITULO") %>' Width="100" />
                                                </div>
                                            </div>
                                            <div class="col-md-10 col-xs-12">
                                                <h4>
                                                    <asp:HyperLink ID="lbTitulo" Text='<%# Eval("TITULO") %>' runat="server" NavigateUrl='<%# "~/indicator.aspx?indicatorid=" + Eval("INDICATORID") %>'></asp:HyperLink></h4>
                                                <div>
                                                    <asp:Label ID="lbResumen" runat="server" Text='<%# Eval("RESUMEN") %>' CssClass="line-fade"></asp:Label>
                                                </div>
                                                <div class="iconos hidden-xs">
                                                    <span title="Creation date"><span class="glyphicon glyphicon-calendar"></span>&nbsp;<%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA_ALTA")))%></span>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <span title="Measure unit"><span class="glyphicon glyphicon-signal"></span>&nbsp;<%# Eval("UNIDAD")%></span>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <span title="Aggregation function"><i class="fa fa-gear"></i>&nbsp;<%# Eval("FUNCION_AGREGADA")%></span>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="step2" class="step-pane">
                        <h3 class="StepTitle margin-top-0 margin-bottom-0"><strong>Step 2. </strong>Indicator image</h3>
                        <p>
                            Specify the image that is representative about the indicator's real meaning. To further facilitate this choice, you can do this in two ways:
                        </p>
                        <div class="row">
                            <div class="col5 col-lg-5 text-center">
                                <h4>Option 1</h4>
                                <p>
                                    Specifying an image from our disk:
                                </p>
                                <div class="margin-top-10">
                                    <asp:Image ID="imgIndicador" runat="server" Width="170" ImageUrl="~/images/indicators/no-image.jpg" ClientIDMode="Static" CssClass="border-rounded" />
                                </div>
                                <div class="margin-top-10">
                                    <asp:HyperLink ID="btnChangePhoto" runat="server" ClientIDMode="Static" CssClass="btn btn-dropkeys" NavigateUrl="#"><i class="fa fa-camera"></i>&nbsp;Change</asp:HyperLink>
                                </div>
                            </div>
                            <div class="col7 col-lg-7 text-center">
                                <h4>Option 2</h4>
                                <p>
                                    Selecting an image from the list below:&nbsp;&nbsp;&nbsp;&nbsp;
                                </p>
                                <div class="margin-top-10">
                                    <asp:TextBox ID="txtBuscarImagen" runat="server" placeholder="Search by ..." class="txtbusqueda" ClientIDMode="Static"></asp:TextBox><asp:Button runat="server" ID="btnBuscarImagen" Text="Search" CssClass="hidden" ClientIDMode="Static"></asp:Button>
                                </div>
                                <div class="margin-top-10">
                                    <ul id="searchedimages" class="list-inline friends-list">
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="step3" class="step-pane">
                        <h3 class="StepTitle margin-top-0 margin-bottom-0"><strong>Step 3. </strong>Especify the subcategory</h3>
                        <p>
                            Finally, indicate which category of the list should be associated to this indicator. This will keep organized our catalog and will allow other users to find the indicator as soon as possible.
                        </p>
                        <div class="form-horizontal">
                            <fieldset>
                                <legend>Category</legend>
                                <div class="form-group">
                                    <label class="control-label col-md-2">Available categories</label>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="cmbCategorias" runat="server" CssClass="form-control input-sm" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </fieldset>
                            <fieldset>
                                <legend>Subcategory</legend>
                            </fieldset>
                        </div>
                        <div class="row">
                            <div class="col6 col-lg-6 text-center">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-8">
                                            <label class="radio radio-inline">
                                                <asp:RadioButton ID="rbNuevaSubcategoria" ClientIDMode="Static" runat="server" GroupName="GrpSubcategoria" />
                                                <span>I want to create a new subcategory</span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4 control-label">New subcategory</label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtNuevaSubcategoria" ClientIDMode="Static" CssClass="form-control" placeholder="Enter a name..." Enabled="false" MaxLength="255" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col6 col-lg-6 text-center">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-8">
                                            <label class="radio radio-inline">
                                                <asp:RadioButton ID="rbViejaSubcategoria" ValidationGroup="mia" ClientIDMode="Static" runat="server" GroupName="GrpSubcategoria" Checked="true" />
                                                <span>I will use one of the existing subcategories</span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <div id="listadoSubcategorias">
                                                <div class="dropkeys-shortlist-container">
                                                    <ul class="dropkeys-shortlist">
                                                        <asp:ListView ID="lstSubcategorias" runat="server" EnableViewState="True">
                                                            <ItemTemplate>
                                                                <li class="show-subcategory" data-id='<%# Eval("SUBCATEGORYID")%>'>
                                                                    <asp:HyperLink ID="HlnkSubcategoria" runat="server" NavigateUrl="#"><%# Eval("NOMBRE")%><span class="badge bg-color-blueMedium pull-right"><%# Eval("RECUENTO")%></span></asp:HyperLink>
                                                                </li>
                                                            </ItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <li class="empty">
                                                                    <span class="list-group-item">There are no yet formulas associated to this indicator</span>
                                                                </li>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                    </ul>
                                                </div>
                                                <div class="dropkeys-shortlist-search">
                                                    <div class="control-group">
                                                        <div class="smart-form">
                                                            <label class="input">
                                                                <input type="text" id="txtSearchSubcategorias" placeholder="Search...">
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="frmShowImage" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title"><i class="fa fa-camera"></i>&nbsp;Choose an image</h4>
                </div>
                <div class="modal-body">
                    <div id="ImageDisplaySelection">
                        Select an image:<input id="fileImageUpload" type="file" name="file" />
                    </div>
                    <div id="ImageDisplayArea" class="text-center">
                        <div id="preview-pane">
                            <fieldset>
                                <legend>Preview image</legend>
                            </fieldset>
                        </div>
                        <div class="text-center">
                            <asp:Image runat="server" ID="uploadImage" ClientIDMode="Static" AlternateText="Upload an image" ImageUrl="~/images/noavatar.png" />
                            <canvas id="finishImage" class="preview-image"></canvas>
                        </div>
                    </div>
                    <div id="ImageDisplayButtons" class="margin-top-md hide">
                        <asp:LinkButton ID="btnUploadImage" runat="server" ClientIDMode="Static" CssClass="btn btn-gray"><span class="glyphicon glyphicon-open"></span>&nbsp;&nbsp;&nbsp;Upload</asp:LinkButton>
                        <asp:LinkButton ID="btnCancelImage" runat="server" ClientIDMode="Static" CssClass="btn btn-gray"><span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;&nbsp;Cancel</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentScript" runat="server" ContentPlaceHolderID="ScriptsContentPlaceHolder">
    <!-- Para el funcionamiento completo de los botones del asistente-->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/fuelux/wizard/wizard.min.js") %>" type="text/javascript"></script>
    <!-- Para recortar las imagenes -->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/jcrop/jquery.Jcrop.min.js") %>" type="text/javascript"></script>
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/sharekpi.js") %>" type="text/javascript"></script>
</asp:Content>

