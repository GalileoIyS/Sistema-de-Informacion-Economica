<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="createkpi.aspx.cs" Inherits="registrado_createkpi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <asp:HiddenField ID="hdnIndicatorID" runat="server" ClientIDMode="Static" />
        <div class="container margin-top-lg margin-bottom-lg">
            <h2>
            <asp:Label ID="lbTitulo" runat="server" CssClass="color-primario-foreground" Text="Create a new Dropkey"></asp:Label></h2>
            <ul class="breadcrumb">
                <li class="first">
                    <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="glyphicon glyphicon-home">
                </span>&nbsp;Home</asp:HyperLink></li>
                <li>
                    <asp:HyperLink ID="HlnkBuscador" runat="server" NavigateUrl="~/search.aspx"><span class="glyphicon glyphicon-search">
                </span>&nbsp;Search</asp:HyperLink></li>
                <li><span class="current-page"><span class="glyphicon glyphicon-plus"></span>&nbsp;<strong>New dropkey</strong></span></li>
            </ul>
            <div class="well">
                <img src="../images/indicators/indicador.png" alt="Dropkeys big" class="pull-left margin-right-10" />
                <h5>Welcome to the new dropkey creation assistance. A wizard will guide you through a series of simple steps to help you introduce a new dropkey in the database. Please, fill all the text boxes that are being asked.</h5>
            </div>
            <div class="widget-body fuelux">
                <div class="wizard" id="CreateWizard">
                    <ul class="steps">
                        <li class="active" data-target="#step1"><span class="badge">1</span>Title & Description<span class="chevron"></span></li>
                        <li data-target="#step2"><span class="badge">2</span>Aggregation function<span class="chevron"></span></li>
                        <li data-target="#step3"><span class="badge">3</span>Attributes<span class="chevron"></span></li>
                    </ul>
                    <div class="actions">
                        <button class="btn btn-sm btn-primary btn-prev" type="button"><span class="glyphicon glyphicon-arrow-left"></span>&nbsp;Prev</button>
                        <button class="btn btn-sm btn-success btn-next" data-last="Complete&nbsp;" type="button">Next&nbsp;<span class="glyphicon glyphicon-arrow-right"></span></button>
                    </div>
                </div>
                <div class="step-content well">
                    <div class="form-horizontal">
                        <div id="step1" class="step-pane active">
                            <h4 class="StepTitle"><strong>Step 1 </strong>- Fill all the fields bellow</h4>
                            <p>
                                Please enter the title and a brief description about the dropkey's meaning. This description will help other users to find it more easily and faster, in case you decide to share it with the rest of the community.
                            </p>
                            <fieldset>
                                <div class="form-group">
                                    <label class="col-md-2 control-label">Title</label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtTitulo" runat="server" MaxLength="250" placeholder="Enter the title..." ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2 control-label">Description</label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtResumen" runat="server" placeholder="Enter a brief description..." ClientIDMode="Static" TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2 control-label">Measure unit</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtUnidad" runat="server" MaxLength="100" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                        <span class="help-block font-xs">Enter the measure unit that is used to represent each basic element of this dropkey.</span>
                                    </div>
                                    <label class="col-md-2 control-label">Symbol</label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtSimbolo" runat="server" MaxLength="5" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                        <span class="help-block font-xs">For example: %, m2, Kg,...</span>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div id="step2" class="step-pane">
                            <h4 class="StepTitle"><strong>Step 2 </strong>- Aggregation function</h4>
                            <p>
                                Specify the aggregation function that must be automatically applied by the system when you need to represent the values ​​of this dropkey in a higher level of the time dimension in which they are introduced. 
                            </p>
                            <fieldset>
                                <div class="form-group">
                                    <label class="col-md-5 control-label text-align-right">Aggregation function</label>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="cmbFuncionAgregada" runat="server" ClientIDMode="Static" CssClass="form-control">
                                            <asp:ListItem Text="Sum" Value="SUM"></asp:ListItem>
                                            <asp:ListItem Text="Average" Value="AVG"></asp:ListItem>
                                            <asp:ListItem Text="Maximum" Value="MAX"></asp:ListItem>
                                            <asp:ListItem Text="Minimum" Value="MIN"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="form-group">
                                    <ul>
                                        <li><strong>Sum:</strong> The added result is the sum of individual values of the inferior temporal dimension. For example, the production of cars, births, sales,...</li>
                                        <li><strong>Average:</strong> The added result is the average of individual values of the inferior temporal dimension. For example, the production of oranges per hectare, the percentage of cesarean deliveries,...</li>
                                        <li><strong>Maximum:</strong> the aggregate result is the maximum of the individual values ​​of the inferior temporal dimension. For example, stop sales, maximum capacity of an enclosure...</li>
                                        <li><strong>Minimum:</strong> the aggregate result is the minimum of the individual values ​​of the inferior temporal dimension. For example, minimum sales,...</li>
                                    </ul>
                                </div>
                            </fieldset>
                        </div>
                        <div id="step3" class="step-pane">
                            <h4 class="StepTitle"><strong>Step 2 </strong>- Attributes</h4>
                            <p>
                                <strong>What are attributes?</strong>. These are properties whose values ​​represent the characteristics of an dropkey (give meaning). For example, for the dropkey <span class="color-terciario-foreground">orange production</span>, we could include the variety of orange cultivated, type of irrigation employed, etc ...
                            </p>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 text-center">
                                    <div class="navbar-form " role="search">
                                        <div class="form-group">
                                            <asp:HyperLink ID="btnAddAttribute" runat="server" CssClass="btn btn-gray" ClientIDMode="Static"><span class="glyphicon glyphicon-plus"></span>&nbsp;Add</asp:HyperLink>
                                            <asp:HyperLink ID="btnDelAttribute" runat="server" CssClass="btn btn-gray" ClientIDMode="Static"><span class="glyphicon glyphicon-remove"></span>&nbsp;Delete</asp:HyperLink>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12">
                                    <div class="text-center">
                                        <asp:ListBox ID="lstAtributos" runat="server" Height="150" Width="300" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <p class="margintop">
                                You can add as many attributes as necessary but as you'll see later, you can also add them through the screen of detailed dropkey.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    <!-- Modal -->
    <div id="frmEditAttribute" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">New Attribute</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="txtNombre">Attribute name</label>
                        <asp:TextBox ID="txtNombreAttribute" placeholder="Enter the name" runat="server" ClientIDMode="Static" MaxLength="100" title="Enter the name of the new attribute" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="cmbDashboard">Description</label>
                        <asp:TextBox ID="txtDescripcionAttribute" placeholder="Enter a brief description" TextMode="MultiLine" Height="80" runat="server" ClientIDMode="Static" MaxLength="500" title="Enter a brief description that will help users to easily understand its meaning" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <a id="btnGuardarAttribute" class="btn btn-dropkeys pull-right" href="#"><span class="glyphicon glyphicon-play"></span>&nbsp;Finish</a>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="frmImportarExcel" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title text-center text-success"><i class="fa fa-check fa-lg"></i>Complete</h4>
                </div>
                <div class="modal-body">
                    <p>Congratulations!. The dropkey has been correctly added to your library.</p>
                    <p>
                        In order to facilitate data entry, if you are so kind, answer one last question to finish. <strong>Have you got any dataset to associate this dropkey?</strong>
                    </p>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 text-center">
                            <div class="navbar-form " role="search">
                                <div class="form-group">
                                    <asp:LinkButton ID="btnCargarSi" runat="server" ClientIDMode="Static" CssClass="btn btn-gray"><span class="glyphicon glyphicon-save"></span>&nbsp;&nbsp;&nbsp;Yes</asp:LinkButton>
                                    <asp:LinkButton ID="btnCargarNo" runat="server" ClientIDMode="Static" CssClass="btn btn-gray"><span class="glyphicon glyphicon-save"></span>&nbsp;&nbsp;&nbsp;No</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="cargaDeDatos" class="hidden">
                        <fieldset>
                            <legend>Choose a data format from the list</legend>
                            <div class="margin-top-md text-center">
                                <asp:LinkButton ID="btnImportarTabla" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Width="110"><span class="glyphicon glyphicon-save"></span>&nbsp;&nbsp;&nbsp;Manual load</asp:LinkButton>
                            </div>
                            <div class="margin-top-md text-center">
                                <asp:LinkButton ID="btnImportarExcel" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Width="110"><span class="glyphicon glyphicon-save"></span>&nbsp;&nbsp;&nbsp;Excel File</asp:LinkButton>
                            </div>
                            <div class="margin-top-md text-center">
                                <asp:LinkButton ID="btnImportarCsv" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Width="110"><span class="glyphicon glyphicon-save"></span>&nbsp;&nbsp;&nbsp;CSV File</asp:LinkButton>
                            </div>
                            <div class="margin-top-md text-center">
                                <asp:LinkButton ID="btnImportarJson" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Width="110"><span class="glyphicon glyphicon-save"></span>&nbsp;&nbsp;&nbsp;JSON File</asp:LinkButton>
                            </div>
                            <div class="margin-top-md text-center">
                                <asp:LinkButton ID="btnImportarXML" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Width="110"><span class="glyphicon glyphicon-save"></span>&nbsp;&nbsp;&nbsp;XML File</asp:LinkButton>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentScript" runat="server" ContentPlaceHolderID="ScriptsContentPlaceHolder">
    <!-- Para el funcionamiento completo de los botones del asistente-->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/fuelux/wizard/wizard.min.js") %>" type="text/javascript"></script>
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/createkpi.js") %>" type="text/javascript"></script>
</asp:Content>
