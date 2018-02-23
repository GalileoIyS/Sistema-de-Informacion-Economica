<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="indicator.aspx.cs" Inherits="indicator" %>

<%@ Register Src="~/controls/KPILibrary.ascx" TagName="KPILibrary" TagPrefix="kpi" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="ContentCenter" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <asp:HiddenField ID="hdnIndicatorID" runat="server" ClientIDMode="Static" />
    <div class="container margin-top-lg margin-bottom-lg">
        <h1>
            <asp:Label ID="lbTitulo" runat="server" CssClass="color-primario-foreground"></asp:Label></h1>
        <ol class="breadcrumb">
            <li class="first">
                <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="glyphicon glyphicon-home"></span>&nbsp;Home</asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="HlnkBuscador" runat="server" NavigateUrl="~/search.aspx"><span class="glyphicon glyphicon-search"></span>&nbsp;Search</asp:HyperLink>
            </li>
            <li><span class="current-page"><strong><span class="glyphicon glyphicon-list-alt"></span>&nbsp;Dropkey's details</strong></span>
            </li>
        </ol>
        <div class="container">
            <div class="row">
                <div class="col-md-8 no-padding-left">
                    <asp:Panel runat="server" ID="PanelPendingRevision" ClientIDMode="Static" Visible="false">
                        <div class="alert alert-info alert-block">
                            <a class="close" data-dismiss="alert" href="#">×</a>
                            <h4 class="alert-heading">YOUR REVISION IS WAITING FOR THE USER WHO CREATED THIS DROPKEY TO BE APPROVAL</h4>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="PanelAccetpOrCancelRevision" ClientIDMode="Static" Visible="false">
                        <div class="alert alert-info alert-block">
                            <a class="close" data-dismiss="alert" href="#">×</a>
                            <h4 class="alert-heading">SOMEBODY HAS SEND YOU A NEW REVISION WAITING FOR YOU APPROVAL</h4>
                            Please, check the <strong>Revisions Tab</strong> to know about these requests. They are pendding for your approval
                        </div>
                    </asp:Panel>
                    <ul class="nav nav-tabs bordered" role="tablist">
                        <li id="homeItem" runat="server" class="homeItem-class active"><a href="#ArtIndicador" role="tab" data-toggle="tab">
                            <span class="glyphicon glyphicon-home"></span>&nbsp;GENERAL</a></li>
                        <li id="revisionItem" runat="server" class="revisionsItem-class" visible="false"><a href="#ArtRevisions" role="tab" data-toggle="tab">
                            <i class="fa fa-file-text"></i>&nbsp;<span>REVISIONS<span id="countRevisions" class="badge bg-color-red txt-color-white">0</span></span></a></li>
                        <li id="othersourcesItem" runat="server" class="othersourcesItem-class" visible="false"><a href="#ArtOtherSources" role="tab" data-toggle="tab">
                            <i class="fa fa-group"></i>&nbsp;<span>USERS<span id="countOtherSources" class="badge bg-color-green txt-color-white">0</span></span></a></li>
                        <li id="dimensionsItem" runat="server" class="dimensionsItem-class" visible="false"><a href="#ArtDimensions" role="tab" data-toggle="tab">
                            <i class="fa fa-bullseye"></i>&nbsp;<span>ATTRIBUTES<span id="countAttributes" class="badge bg-color-pinkDark txt-color-white">0</span></span></a></li>
                        <li id="datasetsItem" runat="server" class="datasetsItem-class" visible="false"><a href="#ArtDataSets" role="tab" data-toggle="tab">
                            <i class="fa fa-dot-circle-o"></i>&nbsp;<span>DATASETS<span id="countDatasets" class="badge bg-color-blue txt-color-white">0</span></span></a></li>
                        <li id="importsItem" runat="server" class="importsItem-class" visible="false"><a href="#ArtImports" role="tab" data-toggle="tab">
                            <span class="glyphicon glyphicon-save"></span>&nbsp;<span>IMPORTS<span id="countImports" class="badge bg-color-yellow txt-color-white">0</span></span></a></li>
                    </ul>
                    <div class="tab-content padding-15">
                        <div id="ArtIndicador" class="tab-pane fade in active">
                            <div id="preview-mode">
                                <div class="row">
                                    <div class="col-lg-4 vcenter">
                                        <br />
                                        <br />
                                        <br />
                                        <div>
                                            <asp:Image ID="imgIndicador" runat="server" ImageUrl="~/images/indicators/no-image.jpg"
                                                ClientIDMode="Static" CssClass="img-rounded center-block" Style="max-height: 170px; max-width: 170px;" />
                                        </div>
                                        <div class="text-center">
                                            <asp:Panel runat="server" ID="PanelSocialMedia" ClientIDMode="Static" CssClass="margin-top-10">
                                                <span class='st_facebook_large' displaytext='Facebook'></span><span class='st_twitter_large'
                                                    displaytext='Tweet'></span><span class='st_linkedin_large' displaytext='LinkedIn'></span><span class='st_email_large' displaytext='Email'></span>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="PanelChangeImage" ClientIDMode="Static" CssClass="margin-top-10" Visible="false">
                                                <asp:LinkButton ID="btnChangePhoto" runat="server" ClientIDMode="Static" CssClass="btn btn-gray"><span class="glyphicon glyphicon-camera"></span>&nbsp;&nbsp;&nbsp;Change</asp:LinkButton>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="form-horizontal">
                                            <fieldset id="PanelAnonimo" runat="server" visible="false">
                                                <legend>How do you want to be seen in this dropkey?</legend>
                                                <div class="form-group">
                                                    <label class="control-label col-md-4 no-padding" for="prepend">Anonymous</label>
                                                    <div class="col-md-8">
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <span class="onoffswitch">
                                                                    <asp:CheckBox ID="cbIsAnonymnous" runat="server" ClientIDMode="Static" />
                                                                    <label class="onoffswitch-label" for="cbIsAnonymnous">
                                                                        <span class="onoffswitch-inner" data-swchon-text="ON" data-swchoff-text="OFF"></span>
                                                                        <span class="onoffswitch-switch"></span>
                                                                    </label>
                                                                </span>
                                                                <span class="savingIsAnonymous hidden">&nbsp;<i class="fa fa-refresh fa-spin"></i>&nbsp;Saving...</span>
                                                                <p class="note"><strong>Note:</strong>&nbsp;<asp:Label ID="lbIsAnonymousHelp" runat="server" ClientIDMode="Static"></asp:Label></p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>
                                            <fieldset>
                                                <legend>General Information</legend>
                                                <asp:LoginView ID="LoginViewCreatorUser" runat="server">
                                                    <LoggedInTemplate>
                                                        <div class="form-group">
                                                            <label class="control-label col-md-4" for="prepend">Created by</label>
                                                            <div class="col-md-8">
                                                                <div class="row">
                                                                    <div class="col-sm-12">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                                                            <asp:HyperLink ID="lbCreatorUserName" runat="server" CssClass="form-control show-modal-info-user" NavigateUrl="#"></asp:HyperLink>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </LoggedInTemplate>
                                                </asp:LoginView>
                                                <div class="form-group">
                                                    <label class="control-label col-md-4" for="prepend">Rating value</label>
                                                    <div class="col-md-8">
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="fa fa-star"></i></span>
                                                                    <span id="targetout" runat="server" class="target-out form-control"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-md-4" for="prepend">Creation date</label>
                                                    <div class="col-md-8">
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <asp:Label ID="lbFechaAlta" runat="server" CssClass="form-control"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-md-4" for="prepend">Aggregate function</label>
                                                    <div class="col-md-8">
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="fa fa-gears"></i></span>
                                                                    <asp:Label ID="lbAgregacion" runat="server" CssClass="form-control"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-md-4" for="prepend">Measure&nbsp;(symbol)</label>
                                                    <div class="col-md-8">
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="fa fa-money"></i></span>
                                                                    <asp:Label ID="lbUnidad" runat="server" CssClass="form-control"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-md-4" for="prepend">Subcategory</label>
                                                    <div class="col-md-8">
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="fa fa-sitemap"></i></span>
                                                                    <asp:Label ID="lbSubcategoria" runat="server" CssClass="form-control"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                                <fieldset>
                                    <legend><span>Summary</span></legend>
                                    <div>
                                        <asp:Literal ID="txtResumen" runat="server"></asp:Literal>
                                    </div>
                                </fieldset>
                                <fieldset>
                                    <legend><span>Description</span></legend>
                                    <div>
                                        <asp:Literal ID="txtDescripcion" runat="server"></asp:Literal>
                                    </div>
                                </fieldset>
                                <h4 class="text-center">
                                    <span class="glyphicon glyphicon-signal"></span>&nbsp;Dropkey's evolution through the last years</h4>
                                <asp:LoginView ID="LoginViewTimeChart" runat="server">
                                    <AnonymousTemplate>
                                        <div class="well text-center margin-top-10">
                                            <h3>You must be logged in to view the data evolution
                                            </h3>
                                            <a href="#" class="btn btn-dropkeys login-trigger" data-toggle="modal" data-target="#myModal"><span class="glyphicon glyphicon-user"></span>&nbsp;Sign up!</a>
                                        </div>
                                    </AnonymousTemplate>
                                    <LoggedInTemplate>
                                        <div class="center-block position-relative margin-top-10">
                                            <div id="chart_div"></div>
                                            <asp:HyperLink ID="lnkRefreshGraph" runat="server" ClientIDMode="Static" CssClass="show-button" NavigateUrl="#"><span class="glyphicon glyphicon-refresh"></span>&nbsp;Reload</asp:HyperLink>
                                        </div>
                                    </LoggedInTemplate>
                                </asp:LoginView>
                            </div>
                            <div id="edit-mode" class="hidden">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-horizontal">
                                            <fieldset>
                                                <legend>General Information</legend>
                                                <div class="form-group">
                                                    <label class="col-md-4 control-label">Name</label>
                                                    <div class="col-md-8">
                                                        <asp:TextBox ID="txtTituloValue" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-4 control-label">Aggregate function</label>
                                                    <div class="col-md-8">
                                                        <asp:DropDownList ID="cmbFuncionAgregadaValue" runat="server" ClientIDMode="Static" CssClass="form-control">
                                                            <asp:ListItem Text="Sum" Value="SUM"></asp:ListItem>
                                                            <asp:ListItem Text="Average" Value="AVG"></asp:ListItem>
                                                            <asp:ListItem Text="Maximum" Value="MAX"></asp:ListItem>
                                                            <asp:ListItem Text="Minimum" Value="MIN"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-4 control-label">Measure unit</label>
                                                    <div class="col-md-8">
                                                        <asp:TextBox ID="txtUnidadValue" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-4 control-label">Symbol</label>
                                                    <div class="col-md-8">
                                                        <asp:TextBox ID="txtSimboloValue" runat="server" MaxLength="3" Width="60" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                                <fieldset>
                                    <legend>Summary</legend>
                                    <div class="form-group">
                                        <div class="help-block">
                                            Please, introduce a brief summary in this textbox:
                                        </div>
                                        <asp:TextBox ID="txtResumenValue" runat="server" MaxLength="1000" ClientIDMode="Static" Height="100" TextMode="MultiLine" CssClass="form-control">
                                        </asp:TextBox>
                                    </div>
                                </fieldset>
                                <fieldset>
                                    <legend>Description</legend>
                                    <div class="form-group">
                                        <div class="help-block">
                                            Please, introduce the full description in this textbox:
                                        </div>
                                        <CKEditor:CKEditorControl ID="txtDescripcionValue" runat="server" Toolbar="Basic" ClientIDMode="Static" Height="160">
                                        </CKEditor:CKEditorControl>
                                    </div>
                                </fieldset>
                                <div class="form-group">
                                    <div class="pull-right">
                                        <asp:LinkButton ID="btnViewIndicador" runat="server" CssClass="btn btn-labeled btn-primary" ClientIDMode="Static"><span class="btn-label"><i class="fa fa-times"></i></span>&nbsp;Cancel</asp:LinkButton>
                                        <asp:LinkButton ID="btnGuardarIndicador" runat="server" CssClass="btn btn-labeled btn-success" OnClick="btnGuardarIndicador_Click" OnClientClick="return validarIndicador();"><span class="btn-label"><i class="fa fa-check"></i></span>&nbsp;Save</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="ArtRevisions" class="tab-pane fade" ClientIDMode="Static" Visible="false">
                            <div class="btn-group">
                                <asp:Button ID="btnDelRevision" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Text="Delete All" />
                                <asp:Button ID="btnDelSelRevision" runat="server" ClientIDMode="Static" CssClass="btn btn-gray hidden" Text="Delete Selected" />
                            </div>
                            <div class="row margin-top-md">
                                <div class="col-md-3 text-center">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/indicators/revision.jpg" />
                                </div>
                                <div class="col-md-9">
                                    Here are shown the different import of data you have made on this dropkey. Be very careful if you want to eliminate any of these imports, since all the associate data will be also deleted
                                </div>
                            </div>
                            <ul class="nav nav-pills-dropkeys">
                                <li>
                                    <input type="checkbox" id="select_all_revisions" /></li>
                                <li id="dataPagerRevisions" class="paginacion pull-right">
                                    <a href="#" class="first" data-action="first">&laquo;</a> <a href="#" class="previous"
                                        data-action="previous">&lsaquo;</a>
                                    <input type="text" readonly="readonly" data-max-page="40" />
                                    <a href="#" class="next" data-action="next">&rsaquo;</a> <a href="#" class="last"
                                        data-action="last">&raquo;</a>
                                </li>
                            </ul>
                            <div id="listaRevisions">
                                <asp:ListView ID="lstRevisions" runat="server">
                                    <ItemTemplate>
                                        <div class="search lirevision" data-revisionid='<%#Eval("REVISIONID")%>'>
                                            <div class="col-md-1">
                                                <input type="checkbox" data-revisionid='<%#Eval("REVISIONID")%>' onchange="revisionOnChange();" />
                                            </div>
                                            <div class="col-md-1 no-padding-left no-margin-left no-padding-right no-margin-right">
                                                <div class="friends-list">
                                                    <asp:Image ID="imgRevision" runat="server" ImageUrl='<%#Eval("IMAGEURL")%>'></asp:Image>
                                                </div>
                                            </div>
                                            <div class="col-md-10">
                                                <h6 class="no-margin">
                                                    <asp:HyperLink ID="lnkViewUserInfo" runat="server" CssClass="show-modal-info-user" data-userid='<%#Eval("USERID")%>' NavigateUrl="#"><%#Eval("APELLIDOS")%>,&nbsp;<%#Eval("NOMBRE")%></asp:HyperLink></h6>
                                                <div class="iconos hidden-xs">
                                                    <asp:Label runat="server" title="Add date"><span class="glyphicon glyphicon-calendar"></span>&nbsp;<%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA")))%></asp:Label>
                                                </div>
                                                <asp:HyperLink ID="lnkViewRevision" runat="server" CssClass="add-indicator lnkViewRevision" NavigateUrl='<%# "~/registrado/revision.aspx?idrevision=" + Eval("REVISIONID") %>'><i class="fa fa-eye"></i>&nbsp;View</asp:HyperLink>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="ArtOtherSources" class="tab-pane fade" ClientIDMode="Static">
                            <div class="row margin-top-md">
                                <div class="col-md-3 text-center">
                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/images/indicators/sources.jpg" />
                                </div>
                                <div class="col-md-9">
                                    Here you can find other people that have been used this dropkey. Some of them, may also have included data values so you can compare with them if you add into your friendly list
                                </div>
                            </div>
                            <ul class="nav nav-pills-dropkeys">
                                <li>
                                    Sort by&nbsp;</li>
                                <li><a id="lnkOrderSourcesByFecha" href="#" title="Sort by creation date" data-orderby="B.NOMBRE ASC" class="sources-order-by pointer">Name</a></li>
                                <li><a id="lnkOrderSourcesByTitle" href="#" title="Sort by name or title" data-orderby="NUM_DATOS DESC" class="sources-order-by pointer">Number of data values</a></li>
                                <li>&nbsp;&nbsp;<asp:TextBox ID="txtBuscarUsers" runat="server" placeholder="Search by..." CssClass="txtbusqueda" ClientIDMode="Static"></asp:TextBox></li>
                            </ul>
                            <div id="listaUsuarios">
                                <asp:ListView ID="lstOtherSources" runat="server">
                                    <ItemTemplate>
                                        <div class="search liuser" data-userid='<%#Eval("USERID")%>'>
                                            <div class="col-md-1 no-padding-left no-margin-left no-padding-right no-margin-right">
                                                <div class="friends-list">
                                                    <asp:Image ID="imgRevision" runat="server" ImageUrl='<%#Eval("IMAGEURL")%>'></asp:Image>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <h6 class="no-margin">
                                                    <asp:HyperLink ID="lnkViewUserInfo" runat="server" CssClass="show-modal-info-user" data-userid='<%#Eval("USERID")%>' NavigateUrl="#"><%#Eval("APELLIDOS")%>,&nbsp;<%#Eval("NOMBRE")%></asp:HyperLink></h6>
                                                <div class="iconos hidden-xs">
                                                    <asp:Label runat="server" title="Number of datasets"><i class="fa fa-dot-circle-o"></i>&nbsp;<%# Eval("NUM_DATASETS")%>&nbsp;datasets</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label runat="server" title="Number of data values"><i class="fa fa-cube"></i>&nbsp;<%# Eval("NUM_DATOS") %>&nbsp;data values</asp:Label>
                                                </div>
                                                
                                            </div>
                                            <div class="col-md-3">
                                                <div class="add-indicator">
                                                    <asp:HyperLink ID="lnkEnsureUserr" runat="server" CssClass="add-indicator" NavigateUrl="#" data-userid='<%# Eval("USERID") %>'><span class="glyphicon glyphicon-plus"></span>&nbsp;Ensure</asp:HyperLink>
                                                </div>
                                                </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="ArtDimensions" class="tab-pane fade" ClientIDMode="Static" Visible="false">
                            <div class="btn-group">
                                <asp:Button ID="btnNewDimension" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Text="New" />
                                <asp:Button ID="btnDelDimension" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Text="Delete All" />
                                <asp:Button ID="btnDelSelDimension" runat="server" ClientIDMode="Static" CssClass="btn btn-gray hidden" Text="Delete Selected" />
                            </div>
                            <div id="PanelNewDimension" class="form-horizontal hidden" role="form">
                                <div class="new-item-panel">
                                    <div class="form-group-sm">
                                        <label class="col-sm-3 control-label">Name</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtNombreDimension" placeholder="Enter the name" runat="server" ClientIDMode="Static" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                            <span class="help-block">Please, enter a name in this textbox</span>
                                        </div>
                                    </div>
                                    <div class="form-group-sm">
                                        <label class="col-sm-3 control-label">Description</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtDescripcionDimension" placeholder="Enter a brief description" TextMode="MultiLine" Height="80" runat="server" ClientIDMode="Static" MaxLength="500" CssClass="form-control"></asp:TextBox>
                                            <span class="help-block">Please, introduce a brief description about the new attribute</span>
                                        </div>
                                    </div>
                                    <div class="form-group-sm">
                                        <div class="col-sm-9 col-md-offset-3">
                                            <asp:Button ID="btnCreateDimension" runat="server" Text="Save" OnClientClick="return InsertaAtributo();"
                                                CssClass="btn btn-gray" />
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                            <div class="row margin-top-md">
                                <div class="col-md-3 text-center">
                                    <asp:Image ID="imgCaracteristicas" runat="server" ImageUrl="~/images/indicators/caracteristicas.png" />
                                </div>
                                <div class="col-md-9">
                                    Here are the different features that users have been attaching to this dropkey. If you detect a fault, please insert including a brief summary of the meaning of it so that others users can easily understand it and so it can be used also. Remember that you can create as many as you need.
                                </div>
                            </div>
                            <ul class="nav nav-pills-dropkeys">
                                <li>
                                    <input type="checkbox" id="select_all_dimensions" />&nbsp;&nbsp;|&nbsp;&nbsp;Sort by&nbsp;</li>
                                <li><a id="lnkOrderAtributoByFecha" href="#" title="Sort by creation date" data-orderby="A.FECHA DESC" class="attribute-order-by pointer">Creation date</a></li>
                                <li><a id="lnkOrderAtributoByTitle" href="#" title="Sort by name or title" data-orderby="A.NOMBRE DESC" class="attribute-order-by pointer">Name</a></li>
                                <li>&nbsp;&nbsp;<asp:TextBox ID="txtBuscarDimension" runat="server" placeholder="Search by..." CssClass="txtbusqueda" ClientIDMode="Static"></asp:TextBox></li>
                                <li id="dataPagerAttributes" class="paginacion pull-right">
                                    <a href="#" class="first" data-action="first">&laquo;</a> <a href="#" class="previous"
                                        data-action="previous">&lsaquo;</a>
                                    <input type="text" readonly="readonly" data-max-page="40" />
                                    <a href="#" class="next" data-action="next">&rsaquo;</a> <a href="#" class="last"
                                        data-action="last">&raquo;</a>
                                </li>
                            </ul>
                            <div id="listaDimensions">
                                <asp:ListView ID="lstDimensions" runat="server" EnableViewState="True" DataKeyNames="DIMENSIONID">
                                    <ItemTemplate>
                                        <div class="search lidimensions" data-dimensionid='<%#Eval("DIMENSIONID")%>'>
                                            <div class="col-md-1">
                                                <input type="checkbox" data-dimensionid='<%#Eval("DIMENSIONID")%>' onchange="dimensionOnChange();" />
                                            </div>
                                            <div class="col-md-1 no-padding-left no-margin-left no-padding-right no-margin-right">
                                                <div class="categoria color-secundario-background">
                                                    <div class="categoria-valor">
                                                        <asp:Label ID="lbCountDimensions" runat="server"><%#Eval("NUMDATOS")%></asp:Label>
                                                    </div>
                                                    <div class="categoria-desc">
                                                        values
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-10">
                                                <h4>
                                                    <asp:HyperLink ID="lbNombreValue" runat="server" Text='<%#Eval("NOMBRE")%>' /></h4>
                                                <div class="iconos">
                                                    <span title="Creation date"><span class="glyphicon glyphicon-calendar"></span>
                                                        <%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA")))%></span>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:HyperLink runat="server" CssClass="show-modal-info-user" data-userid='<%#Eval("USERID")%>' NavigateUrl="#"><span class="glyphicon glyphicon-user"></span>&nbsp;<%# Eval("USERFIRSTNAME") + " " + Eval("USERLASTNAME") %></asp:HyperLink>
                                                </div>
                                                <div>
                                                    <%# Eval("DESCRIPCION")%>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <h4>No attribute available?</h4>
                                        <p>
                                            Be the first to do so. Just simply enter a descriptive name for the new feature or attribute and a brief summary about it and click the "Save" button.
                                        </p>
                                        <div class="alert alert-info fade in">
                                            <button class="close" data-dismiss="alert">
                                                ×
                                            </button>
                                            <i class="fa-fw fa fa-info"></i>
                                            <strong>Remember!</strong>&nbsp;&nbsp;&nbsp;Attributes allow us to specify a set of values ​​that define the behavior of a given set of data for a particular dropkey.
                                        </div>
                                        <div class="text-center">
                                            <asp:Image ID="imgDimension" runat="server" EnableViewState="false" ImageUrl="~/images/frontpage/fill-data.jpg" CssClass="img-responsive center-image" />
                                            <br />
                                            <a href="http://www.freepik.com/free-photos-vectors/infographic" class="infographic-link">Infographic vector designed by Freepik</a>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="ArtDataSets" class="tab-pane fade" ClientIDMode="Static" Visible="false">
                            <div class="btn-group">
                                <asp:Button ID="btnNewDataSet" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Text="New" />
                                <asp:Button ID="btnDelAllDataSet" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Text="Delete All" />
                                <asp:Button ID="btnDelSelDataSet" runat="server" ClientIDMode="Static" CssClass="btn btn-gray hidden" Text="Delete Selected" />
                            </div>
                            <div id="PanelNewDataSet" class="form-horizontal hidden" role="form">
                                <div class="new-item-panel">
                                    <div class="form-group-sm">
                                        <label class="col-sm-3 control-label">Name</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtNombreDataSet" placeholder="Enter the name" runat="server" ClientIDMode="Static" MaxLength="200" CssClass="form-control"></asp:TextBox>
                                            <span class="help-block">Please, enter a name in this textbox</span>
                                        </div>
                                    </div>
                                    <div class="form-group-sm">
                                        <label class="col-sm-3 control-label">Frequency</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cmbTemporal" runat="server" ClientIDMode="Static" CssClass="form-control">
                                            </asp:DropDownList>
                                            <span class="help-block">Please, choose a sampling frecuency from the list</span>
                                        </div>
                                    </div>
                                    <div class="form-group-sm">
                                        <div class="col-sm-9 col-md-offset-3">
                                            <asp:Button ID="btnCreateDataset" runat="server" Text="Save" OnClientClick="return InsertaDataset();"
                                                CssClass="btn btn-gray" />
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                            <div class="row margin-top-md">
                                <div class="col-md-3 text-center">
                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/indicators/datasets.png" />
                                </div>
                                <div class="col-md-9">
                                    Here is a list of different data sets created by you, each of which is represented by the values ​​of their associated attributes. The graph below represents the last ten values of each. Remember that you can create as many as you need, in the time frecuency you want.
                                </div>
                            </div>
                            <ul class="nav nav-pills-dropkeys">
                                <li>
                                    <input type="checkbox" id="select_all_datasets" />&nbsp;&nbsp;|&nbsp;&nbsp;Sort by&nbsp;</li>
                                <li><a id="lnkOrderDatasetByFecha" href="#" title="Sort by creation date" data-orderby="A.FECHA DESC" class="dataset-order-by pointer">Creation date</a></li>
                                <li><a id="lnkOrderDatasetByTitle" href="#" title="Sort by title or name" data-orderby="A.NOMBRE DESC" class="dataset-order-by pointer">Name</a></li>
                                <li>&nbsp;&nbsp;<asp:TextBox ID="txtBuscarDataset" runat="server" placeholder="Search by..." class="txtbusqueda" ClientIDMode="Static"></asp:TextBox></li>
                                <li id="dataPagerDatasets" class="paginacion pull-right">
                                    <a href="#" class="first" data-action="first">&laquo;</a> <a href="#" class="previous"
                                        data-action="previous">&lsaquo;</a>
                                    <input type="text" readonly="readonly" data-max-page="40" />
                                    <a href="#" class="next" data-action="next">&rsaquo;</a> <a href="#" class="last"
                                        data-action="last">&raquo;</a>
                                </li>
                            </ul>
                            <div id="listaDatasets">
                                <asp:ListView ID="lstDataSets" runat="server">
                                    <ItemTemplate>
                                        <div class="search lidatasets" data-datasetid='<%#Eval("DATASETID")%>'>
                                            <div class="col-md-1">
                                                <input type="checkbox" data-datasetid='<%#Eval("DATASETID")%>' onchange="datasetOnChange();" />
                                            </div>
                                            <div class="col-md-1 no-padding-left no-margin-left no-padding-right no-margin-right">
                                                <div class="categoria color-secundario-background">
                                                    <div class="categoria-valor">
                                                        <asp:Label ID="lbCountDatasets" runat="server"><%#Eval("NUMDATOS")%></asp:Label>
                                                    </div>
                                                    <div class="categoria-desc">
                                                        values
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <h4>
                                                    <%#Eval("NOMBRE")%></h4>
                                                <div class="iconos">
                                                    <span title="Data frequency sampling"><span class="glyphicon glyphicon-time"></span>&nbsp;<%# Eval("DESCRIPCION")%></span>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <span title="Number of used attributes"><i class="fa fa-bullseye"></i>&nbsp;<%# Eval("NUMDIMENSIONS")%> attributes</span>
                                                </div>
                                                <div class="iconos">
                                                    <span>
                                                        <asp:HyperLink ID="lnkUpdateDataset" runat="server" NavigateUrl='<%#"~/registrado/datosbydataset.aspx?dataset=" + Eval("DATASETID")%>' title="Edit the dataset and its values"><span class=" glyphicon glyphicon-pencil"></span>&nbsp;data</asp:HyperLink></span>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <span>
                                                                <asp:HyperLink ID="lnkDeleteDataset" runat="server" NavigateUrl="#" title="Delete the dataset and all its associated data" data-datasetid='<%#Eval("DATASETID")%>' class="lnkDeleteCurrentDataset"><span class="glyphicon glyphicon-remove"></span>&nbsp;delete</asp:HyperLink></span>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <span>
                                                                <asp:HyperLink ID="lnkExportDataset" runat="server" NavigateUrl="#" title="Export all the values" data-datasetid='<%#Eval("DATASETID")%>' class="lnkExportCurrentDataset"><span class="glyphicon glyphicon-open"></span>&nbsp;export</asp:HyperLink></span>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <h4>No data available?</h4>
                                        <p>
                                            It's time to start adding data and enjoy DropKeys. Simply enter a name for the new data set, click on the button that says "New Dataset" and ... <strong>¡volia!</strong>
                                        </p>
                                        <div class="alert alert-info fade in">
                                            <button class="close" data-dismiss="alert">
                                                ×
                                            </button>
                                            <i class="fa-fw fa fa-info"></i>
                                            <strong>Remember!</strong>&nbsp;&nbsp;&nbsp;Remember, all the datasets uses Attributes to give greater sense. Use as many as you need to provide more meaningfully to your data
                                        </div>
                                        <div class="text-center">
                                            <asp:Image ID="imgDataset" runat="server" EnableViewState="false" ImageUrl="~/images/frontpage/datasets.jpg" CssClass="img-responsive center-image" />
                                            <br />
                                            <a href="http://www.freepik.com/free-photos-vectors/infographic" class="infographic-link">Infographic vector designed by Freepik</a>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="ArtImports" class="tab-pane fade" ClientIDMode="Static" Visible="false">
                            <div class="btn-group">
                                <a class="btn btn-gray" data-toggle="modal" data-target="#frmImportData"><span class="glyphicon glyphicon-save"></span>&nbsp;&nbsp;New</a>
                                <asp:Button ID="btnDelAllImports" runat="server" ClientIDMode="Static" CssClass="btn btn-gray" Text="Delete All" />
                                <asp:Button ID="btnDelSelImport" runat="server" ClientIDMode="Static" CssClass="btn btn-gray hidden" Text="Delete Selected" />
                            </div>
                            <div class="row margin-top-md">
                                <div class="col-md-3 text-center">
                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/images/indicators/imports.png" />
                                </div>
                                <div class="col-md-9">
                                    Here are shown the different import of data you have made on this dropkey. Be very careful if you want to eliminate any of these imports, since all the associate data will be also deleted
                                </div>
                            </div>
                            <ul class="nav nav-pills-dropkeys">
                                <li>
                                    <input type="checkbox" id="select_all_imports" />&nbsp;&nbsp;|&nbsp;&nbsp;Sort by&nbsp;</li>
                                <li><a id="lnkOrderImportByFecha" href="#" title="Sort by creation date" data-orderby="A.FECHA DESC" class="import-order-by pointer">Creation date</a></li>
                                <li><a id="lnkOrderImportByTitle" href="#" title="Sort by title or name" data-orderby="A.NOMBRE ASC" class="import-order-by pointer">Name</a></li>
                                <li>&nbsp;&nbsp;<asp:TextBox ID="txtBuscarImportacion" runat="server" placeholder="Search by..." class="txtbusqueda" ClientIDMode="Static"></asp:TextBox></li>
                                <li id="dataPagerImports" class="paginacion pull-right">
                                    <a href="#" class="first" data-action="first">&laquo;</a> <a href="#" class="previous"
                                        data-action="previous">&lsaquo;</a>
                                    <input type="text" readonly="readonly" data-max-page="40" />
                                    <a href="#" class="next" data-action="next">&rsaquo;</a> <a href="#" class="last"
                                        data-action="last">&raquo;</a>
                                </li>
                            </ul>
                            <div id="listaImports">
                                <asp:ListView ID="lstImports" runat="server">
                                    <ItemTemplate>
                                        <div class="search liimports" data-importid='<%#Eval("IMPORTID")%>'>
                                            <div class="col-md-1">
                                                <input type="checkbox" data-importid='<%#Eval("IMPORTID")%>' onchange="importOnChange();" />
                                            </div>
                                            <div class="col-md-1 no-padding-left no-margin-left no-padding-right no-margin-right">
                                                <div class="categoria color-secundario-background">
                                                    <div class="categoria-valor">
                                                        <asp:Label ID="lbCountDatasets" runat="server"><%#Eval("NUM_DATASETS")%></asp:Label>
                                                    </div>
                                                    <div class="categoria-desc">
                                                        datasets
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-10">
                                                <h4>
                                                    <%#Eval("NOMBRE")%></h4>
                                                <div class="iconos">
                                                    <span title="Import date"><span class="glyphicon glyphicon-calendar"></span>&nbsp;<%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA")))%></span>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <span title="Number of accepted values"><span class="glyphicon glyphicon-ok"></span>&nbsp;<%# Eval("NUM_DATA_OK")%>&nbsp;imported values</span>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <span title="Number of rejected values"><span class="glyphicon glyphicon-remove"></span>&nbsp;<%# Eval("NUM_DATA_ERROR")%>&nbsp;rejected values</span>
                                                </div>
                                                <div>
                                                    <span>
                                                        <%#Eval("DESCRIPCION")%></span>
                                                </div>
                                                <asp:Panel runat="server" ID="PanelEnProceso" class="import-en-proceso" Visible='<%# Convert.ToBoolean(Eval("FINALIZADO").ToString() == "N") %>'>
                                                    <i class="fa fa-refresh fa-spin"></i>&nbsp;processing...
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="PanelProcesado" class="import-finalizado" Visible='<%# Convert.ToBoolean(Eval("FINALIZADO").ToString() == "S") %>'>
                                                    <i class="fa fa-check"></i>&nbsp;Processed
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </asp:Panel>
                    </div>
                    <asp:Panel ID="PanelEtiquetasEditable" runat="server" CssClass="panel margin-top-md" Visible="false">
                        <div class="panel-heading">
                            <span class="glyphicon glyphicon-tags"></span>&nbsp;TAGS&nbsp;(<asp:Label ID="lbNumEtiquetasEditable" runat="server" Text="0" ClientIDMode="Static"></asp:Label>)
                        </div>
                        <div class="panel-body">
                            <p>
                                Enter all the labels you want to enable for help other users to locate this dropkey
                            </p>
                            <asp:TextBox ID="txtEtiquetasEditable" runat="server" CssClass="tag-prefix"></asp:TextBox>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PanelEtiquetasNoEditable" runat="server" CssClass="panel margin-top-md">
                        <div class="panel-heading">
                            <span class="glyphicon glyphicon-tags"></span>&nbsp;TAGS&nbsp;(<asp:Label ID="lbNumEtiquetasNoEditable" runat="server" Text="0"></asp:Label>)
                        </div>
                        <div class="panel-body">
                            <asp:PlaceHolder ID="PlaceHolderEtiquetas" runat="server"></asp:PlaceHolder>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PanelComentarios" runat="server" CssClass="panel margin-top-md">
                        <div class="panel-heading">
                            <span class="glyphicon glyphicon-comment"></span>&nbsp;<asp:Label ID="lbNumComentarios" runat="server" Text="COMMENTS (0)"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <asp:LoginView ID="LoginViewComments" runat="server">
                                <AnonymousTemplate>
                                    <div class="well">
                                        <i class="fa fa-lock fa-2x"></i>&nbsp;<strong>Remember:</strong>&nbsp;You must be logged in to post a comment 
                                    </div>
                                </AnonymousTemplate>
                                <LoggedInTemplate>
                                    <div class="well">
                                        <textarea id="txtEditComment" rows="2" class="form-control" placeholder="What are you thinking?"></textarea>
                                        <div class="margin-top-10">
                                            <a id="lnkPostComment" href="#" class="btn btn-sm btn-primary pull-right">Post</a>
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>
                                </LoggedInTemplate>
                            </asp:LoginView>
                            <div id="listaDeComentarios">
                                <asp:ListView ID="lstComentarios" runat="server" OnItemDataBound="lstComentarios_ItemDataBound" DataKeyNames="COMMENTID">
                                    <ItemTemplate>
                                        <div class="timeline-seperator text-center">
                                            <span><%# MostrarFechaLarga(Convert.ToString(Eval("FECHA"))) %></span>
                                        </div>
                                        <div class="chat-body no-padding profile-message" data-commentid='<%#Eval("COMMENTID")%>'>
                                            <ul>
                                                <li class="message">
                                                    <asp:Image runat="server" ID="imgCommentUser" ImageUrl='<%#Eval("IMAGEURL")%>' class="online" alt='<%# Eval("NOMBRE")%>' Style="width: 50px;" />
                                                    <span class="message-text">
                                                        <a href="#" data-userid='<%#Eval("USERID")%>' class="username show-modal-info-user"><%# Eval("NOMBRE")%>&nbsp;<%# Eval("APELLIDOS")%> <small class="text-muted pull-right ultra-light"><%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA")))%></small></a>
                                                        <div>
                                                            <%# Eval("COMENTARIO")%>
                                                        </div>
                                                    </span>
                                                    <ul class="list-inline font-xs">
                                                        <li>
                                                            <a href="javascript:void(0);" class="text-danger"><i class="fa fa-thumbs-up"></i>Like</a>
                                                        </li>
                                                        <li>
                                                            <asp:HyperLink ID="HlnkViewMoreReplies" runat="server" class="text-muted" NavigateUrl="#"></asp:HyperLink>
                                                        </li>
                                                        <li>
                                                            <a href="javascript:void(0);" class="text-primary">Edit</a>
                                                        </li>
                                                        <li>
                                                            <a href="javascript:void(0);" class="text-danger">Delete</a>
                                                        </li>
                                                    </ul>
                                                </li>
                                                <asp:ListView ID="lstReplicas" runat="server">
                                                    <ItemTemplate>
                                                        <li class="message message-reply">
                                                            <asp:Image runat="server" ID="imgReplyUser" ImageUrl='<%#Eval("IMAGEURL")%>' alt='<%# Eval("NOMBRE")%>' Style="width: 50px;" />
                                                            <span class="message-text">
                                                                <a href="#" data-userid='<%#Eval("USERID")%>' class="username show-modal-info-user"><%# Eval("NOMBRE")%>&nbsp;<%# Eval("APELLIDOS")%></a>
                                                                <div><%# Eval("COMENTARIO")%></div>
                                                            </span>
                                                            <ul class="list-inline font-xs">
                                                                <li><%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA")))%></li>
                                                                <li><a href="javascript:void(0);" class="text-danger"><i class="fa fa-thumbs-up"></i>Like</a></li>
                                                            </ul>
                                                        </li>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>

                                            </ul>
                                            <asp:LoginView ID="LoginPostComment" runat="server">
                                                <LoggedInTemplate>
                                                    <ul>
                                                        <li class="message message-reply">
                                                            <input class="form-control input-xs txtNewReply" placeholder="Type and enter" type="text">
                                                        </li>
                                                    </ul>
                                                </LoggedInTemplate>
                                            </asp:LoginView>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="col-md-4">
                    <div class="bloque-estadisticas marginbottom">
                        <h2><strong>Statistics</strong></h2>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="title-text">
                                    USERS
                                </div>
                                <div class="title_desc">
                                    Sharing
                                </div>
                                <div class="title-number">
                                    <strong><span class="numUsers">0</span></strong>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="title-text">
                                    DATASETS
                                </div>
                                <div class="title_desc">
                                    On database
                                </div>
                                <div class="title-number">
                                    <strong><span class="numDatasets">0</span></strong>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="title-text">
                                    VALUES
                                </div>
                                <div class="title_desc">
                                    inside datasets
                                </div>
                                <div class="title-number">
                                    <strong><span class="numDatas">0</span></strong>
                                </div>
                            </div>
                        </div>
                        <asp:HyperLink ID="lnkRefreshSatistics" runat="server" ClientIDMode="Static" CssClass="show-button" NavigateUrl="#"><span class="glyphicon glyphicon-refresh"></span>&nbsp;Reload</asp:HyperLink>
                    </div>
                    <asp:LoginView ID="LoginViewIndicador" runat="server">
                        <AnonymousTemplate>
                            <div class="panel panel-dropkeys">
                                <div class="panel-body">
                                    <h3>DO YOU WANT THIS DROPKEY?</h3>
                                    <asp:HyperLink runat="server" ID="btnCreate" class="btn btn-dropkeys login-trigger" data-toggle="modal" data-target="#myModal"><span class="glyphicon glyphicon-user"></span>&nbsp;Sign up!</asp:HyperLink>
                                </div>
                            </div>
                        </AnonymousTemplate>
                        <LoggedInTemplate>
                            <asp:Panel ID="PanelIndicadorAusente" runat="server">
                                <div class="panel panel-dropkeys">
                                    <div class="panel-body">
                                        <h3>STILL NOT HAVE THIS DROPKEY?</h3>
                                        <asp:LinkButton ID="btnAddIndicador" runat="server" CssClass="btn btn-dropkeys" OnClick="btnAddIndicador_Click"><span class="glyphicon glyphicon-plus"></span>&nbsp;Add it!</asp:LinkButton>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PanelIndicadorCompartido" runat="server">
                                <div class="panel panel-primary margin-top-md">
                                    <div class="panel-heading">
                                        <span class="glyphicon glyphicon-list"></span>&nbsp;MY OPTIONS
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4 hidden-xs text-center">
                                                <asp:Image ID="imgIndicador" runat="server" ImageUrl="~/images/indicators/indicador.png" />
                                            </div>
                                            <div class="col-md-8 col-xs-12">
                                                <div class="margin-top-md">
                                                    <asp:LinkButton ID="btnCancelSharedIndicador" runat="server" CssClass="btn btn-gray btn-cancel-indicator hidden"><i class="fa fa-reply"></i>&nbsp;&nbsp;&nbsp;Cancel</asp:LinkButton>
                                                    <asp:LinkButton ID="btnEditSharedIndicador" runat="server" CssClass="btn btn-gray btn-edit-indicator"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;&nbsp;Edit</asp:LinkButton>
                                                </div>
                                                <div class="margin-top-md">
                                                    <asp:LinkButton ID="btnDeleteSharedIndicator" runat="server" OnClick="btnEliminarIndicador_Click" CssClass="btn btn-gray"><span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Delete</asp:LinkButton>
                                                </div>
                                                <div class="margin-top-md">
                                                    <a id="btnImportSharedIndicator" class="btn btn-gray" data-toggle="modal" data-target="#frmImportData"><span class="glyphicon glyphicon-save"></span>&nbsp;&nbsp;Import</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default margin-top-md">
                                    <div class="panel-heading">
                                        <i class="fa fa-flask"></i>&nbsp;MOST SUGGESTED FORMULAS        
                                    </div>
                                    <div id="listadoFormulas">
                                        <div class="dropkeys-shortlist-container">
                                            <ul class="dropkeys-shortlist">
                                                <asp:ListView ID="lstFormulas" runat="server" EnableViewState="True">
                                                    <ItemTemplate>
                                                        <li>
                                                            <asp:HyperLink ID="HlnkFormula" CssClass="show-formula" runat="server" NavigateUrl="#" data-id='<%# Eval("IDFORMULA")%>'><%# Eval("NOMBRE")%><span class="badge bg-color-blueMedium pull-right" title="Number of users"><%# Eval("CONTADOR")%></span></asp:HyperLink>
                                                        </li>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <li class="empty">
                                                            <span class="list-group-item">There are no yet formulas</span>
                                                        </li>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </ul>
                                        </div>
                                        <div class="dropkeys-shortlist-search">
                                            <div class="control-group">
                                                <div class="smart-form">
                                                    <label class="input">
                                                        <input type="text" id="txtSearchFormulas" placeholder="Search...">
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default margin-top-md">
                                    <div class="panel-heading">
                                        <i class="fa fa-group "></i>&nbsp;FRIENDS USING THIS DROPKEY        
                                    </div>
                                    <div id="listadoFriends">
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
                                                        <input type="text" id="txtSearchFriends" placeholder="Search...">
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PanelIndicadorPrivado" runat="server">
                                <div class="panel panel-primary margin-top-md">
                                    <div class="panel-heading">
                                        <span class="glyphicon glyphicon-list"></span>&nbsp;MY OPTIONS
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4 hidden-xs text-center">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/indicators/indicador.png" />
                                            </div>
                                            <div class="col-md-8 col-xs-12">
                                                <div class="margin-top-md">
                                                    <asp:LinkButton ID="btnCancelPrivateIndicador" runat="server" CssClass="btn btn-gray btn-cancel-indicator hidden"><i class="fa fa-reply"></i>&nbsp;&nbsp;&nbsp;Cancel</asp:LinkButton>
                                                    <asp:LinkButton ID="btnEditPrivateIndicador" runat="server" CssClass="btn btn-gray btn-edit-indicator" ClientIDMode="Static"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;&nbsp;Edit</asp:LinkButton>
                                                </div>
                                                <div class="margin-top-md">
                                                    <asp:LinkButton ID="btnDeletePrivateIndicator" runat="server" OnClick="btnEliminarIndicador_Click" OnClientClick="return confirm('Are you sure you want to delete this dropkey from your library?');" CssClass="btn btn-gray"><span class=" glyphicon glyphicon-remove"></span>&nbsp;&nbsp;&nbsp;&nbsp;Delete</asp:LinkButton>
                                                </div>
                                                <div class="margin-top-md">
                                                    <a id="btnImportPrivateIndicator" class="btn btn-gray" data-toggle="modal" data-target="#frmImportData"><span class="glyphicon glyphicon-save"></span>&nbsp;&nbsp;&nbsp;Import</a>
                                                </div>
                                                <div class="margin-top-md">
                                                    <asp:LinkButton ID="btnSharePrivateIndicador" runat="server" CssClass="btn btn-gray"><span class="glyphicon glyphicon-share"></span>&nbsp;&nbsp;&nbsp;Share</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-dropkeys">
                                    <div class="panel-body">
                                        <h3>PUBLISH AND COMPARE IT WITH OTHERS!</h3>
                                        <asp:LinkButton ID="btnCompartir" runat="server" CssClass="btn btn-dropkeys"><span class="glyphicon glyphicon-share"></span>&nbsp;Share</asp:LinkButton>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div>
                                <kpi:KPILibrary ID="KPILibreria" runat="server" />
                            </div>
                        </LoggedInTemplate>
                    </asp:LoginView>
                    <div>
                        <h3><span class="glyphicon glyphicon-align-left"></span>&nbsp;AVAILABLE DATA</h3>
                        <div id="data_div">
                        </div>
                    </div>
                    <div class="panel panel-primary margin-top-md">
                        <div class="panel-heading">
                            <i class="fa fa-tachometer"></i>&nbsp;RELATED DROPKEYS
                        </div>
                        <div class="panel-body">
                            <asp:ListView ID="lstIndicadoresRelacionados" runat="server" EnableViewState="True"
                                DataKeyNames="INDICATORID">
                                <ItemTemplate>
                                    <div class="list-group no-margin-bottom">
                                        <img src="images/buttons/check.png" alt="exito" />
                                        <asp:HyperLink ID="HlnkNombreIndicador" runat="server" Text='<%# Eval("TITULO") %>' NavigateUrl='<%# "~/indicator.aspx?indicatorid=" + Eval("INDICATORID") %>' idformula='<%# Eval("INDICATORID") %>'></asp:HyperLink>
                                    </div>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="list-group no-margin-bottom">
                                        There are no yet dropkeys related with this in our database
                                    </div>
                                </EmptyDataTemplate>
                            </asp:ListView>
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
                    <div id="ImageDisplaySelection" class="text-center">
                        <input id="fileImageUpload" type="file" name="file" class="btn-upload" />
                        <asp:HiddenField ID="X" runat="server" EnableViewState="true" />
                        <asp:HiddenField ID="Y" runat="server" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="W" runat="server" Value="100" />
                        <asp:HiddenField ID="H" runat="server" />
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
                    <div id="ImageDisplayButtons" class="margin-top-md hidden">
                        <asp:LinkButton ID="btnUploadImage" runat="server" ClientIDMode="Static" CssClass="btn btn-gray"><span class="glyphicon glyphicon-open"></span>&nbsp;&nbsp;&nbsp;Upload</asp:LinkButton>
                        <asp:LinkButton ID="btnCancelImage" runat="server" ClientIDMode="Static" CssClass="btn btn-gray"><span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;&nbsp;Cancel</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="frmShowFormula" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">
                        <i class="fa fa-flask"></i>&nbsp;<asp:Label ID="lbNombreDeFormula" runat="server" ClientIDMode="Static" /></h4>
                </div>
                <div class="modal-body">
                    <div>
                        <span class="glyphicon glyphicon-user"></span>&nbsp;<strong>Last used:</strong>
                        <asp:Label ID="lbFechaDeFormula" runat="server" ClientIDMode="Static" />
                    </div>
                    <div class="jumbotron margin-top-md">
                        <span id="formulagraphic"></span>
                    </div>
                    <div role="form">
                        <div class="checkbox">
                            <label>
                                <input id="cbIncluirEnGrafico" type="checkbox" value="ok" />
                                Do you want to include it in a graph?
                            </label>
                        </div>
                        <div id="panelIncluirEnGrafico" class="hidden">
                            <div class="form-group">
                                <label for="txtNombre"><i class="fa fa-edit"></i>&nbsp;Name</label>
                                <asp:TextBox runat="server" ID="txtNombre" Enabled="false" placeholder="Dropkey's name"
                                    ClientIDMode="Static" MaxLength="200" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="cmbDashboard"><i class="fa fa-bar-chart-o"></i>&nbsp;Dashboard</label>
                                <asp:DropDownList ID="cmbDashboard" runat="server" Enabled="false" ClientIDMode="Static"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="cmbWidget"><i class="fa fa-signal"></i>&nbsp;Chart</label>
                                <asp:DropDownList ID="cmbWidget" runat="server" Enabled="false" ClientIDMode="Static"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a id="btnCopiarFormula" class="btn btn-dropkeys pull-right hidden" href="#"><span class="glyphicon glyphicon-play"></span>&nbsp;Finish</a>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="frmImportData" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title"><span class="glyphicon glyphicon-save"></span>&nbsp;Available formats</h4>
                </div>
                <div class="modal-body">
                    <p>Choose a data format from the list</p>
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
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentScript" runat="server" ContentPlaceHolderID="ScriptsContentPlaceHolder">
    <!-- Galería javascript de google para poder dibujar gráficas -->
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load('visualization', '1.0', { 'packages': ['corechart', 'table', 'gauge'] });
    </script>
    <!-- scripts para incluir los botones de social media -->
    <script type="text/javascript">        var switchTo5x = true;</script>
    <script type="text/javascript" src="http://w.sharethis.com/button/buttons.js"></script>
    <script type="text/javascript">        stLight.options({ publisher: "0f96cea4-a74a-48a2-8fa0-b6a5f93e9d8c", doNotHash: false, doNotCopy: false, hashAddressBar: false });</script>
    <!-- Para mostrar las formulas en formato gráfico -->
    <script src="<%=Page.ResolveUrl("~/scripts/varios/jqmath-etc-0.4.0.min.js") %>" type="text/javascript"></script>
    <!-- Para mostrar las estrella de las valoraciones y poder realizar nuevas votaciones -->
    <script src="<%=Page.ResolveUrl("~/scripts/jquery/jquery.raty.js") %>" type="text/javascript"></script>
    <!-- Para insertar las etiquetas asociadas al indicador-->
    <script src="<%=Page.ResolveUrl("~/scripts/jquery/jquery.tagsinput.min.js") %>" type="text/javascript"></script>
    <!-- Para crear mini-gráficos representativos de los últimos 10 datos de un dataset-->
    <script src="<%=Page.ResolveUrl("~/scripts/jquery/jquery.peity.min.js") %>" type="text/javascript"></script>
    <!-- Para recortar las imagenes -->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/jcrop/jquery.Jcrop.min.js") %>" type="text/javascript"></script>
    <!-- Para mejorar la visualización de la subida de imagenes/ficheros -->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/fileupload/jquery.nicefileinput.min.js") %>" type="text/javascript"></script>
    <!-- Para crear los datapager de las listas de atributos, datasets e importaciones-->
    <script src="<%=Page.ResolveUrl("~/scripts/jquery/jquery.jqpagination.min.js") %>"
        type="text/javascript"></script>
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/indicator.js") %>" type="text/javascript"></script>
</asp:Content>
