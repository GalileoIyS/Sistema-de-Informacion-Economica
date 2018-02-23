<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="revision.aspx.cs" Inherits="registrado_revision" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="ContentCenter" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
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
                <asp:HyperLink ID="HlnkIndicador" runat="server" NavigateUrl="~/search.aspx" ClientIDMode="Static">
                    <i class="fa fa-tachometer"></i>&nbsp;<asp:Label ID="lbIndicatorName" runat="server"></asp:Label>
                </asp:HyperLink></li>
            <li><span class="current-page"><i class="fa fa-file-text"></i>&nbsp;<strong>View revision</strong></span></li>
        </ul>
        <div class="well">
            <div class="row">
                <div class="panel panel-dropkeys">
                    <div class="panel-body">
                        <h4>Once you have readed the proposed revision, what do you want to do about it?</h4>
                        <asp:LinkButton ID="HlnkRejectAndForget" runat="server" CssClass="btn btn-dropkeys login-trigger" OnClick="HlnkRejectAndForget_Click"><i class="fa fa-times"></i>&nbsp;Reject & Forget</asp:LinkButton>
                        <asp:LinkButton ID="HlnkAcceptAndChange" runat="server" CssClass="btn btn-dropkeys login-trigger" OnClientClick="return validaDatos();" OnClick="HlnkAcceptAndChange_Click"><i class="fa fa-check"></i>&nbsp;Accept & Change</asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <div class="form-horizontal">
                        <fieldset>
                            <legend>Current Information</legend>
                            <div class="form-group">
                                <label class="control-label col-md-4" for="prepend">Title</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-dashboard"></i></span>
                                        <asp:Label ID="lbTitle" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-4" for="prepend">Creation date</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:Label ID="lbFechaAlta" runat="server" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-4" for="prepend">Aggregate function</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-gears"></i></span>
                                        <asp:Label ID="lbAgregacion" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-4" for="prepend">Measure unit</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-money"></i></span>
                                        <asp:Label ID="lbUnidad" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-4" for="prepend">Symbol</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-superscript"></i></span>
                                        <asp:Label ID="lbSimbolo" runat="server" Width="60" CssClass="form-control" ClientIDMode="Static"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <fieldset>
                        <legend><span>Current Summary</span></legend>
                        <div>
                            <asp:TextBox ID="txtResumen" runat="server" ClientIDMode="Static" CssClass="form-control" Height="100" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend><span>Current Description</span></legend>
                        <div>
                            <asp:Literal ID="txtDescripcion" runat="server"></asp:Literal>
                        </div>
                    </fieldset>
                </div>
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <div class="form-horizontal">
                        <fieldset>
                            <legend>Proposed Information</legend>
                            <div class="form-group">
                                <label class="col-md-4 control-label" for="prepend">Title</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-dashboard"></i></span>
                                        <asp:TextBox ID="txtTituloValue" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label" for="prepend">Creation date</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:Label ID="lbFechaRevision" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Aggregate function</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-gears"></i></span>
                                        <asp:DropDownList ID="cmbFuncionAgregadaValue" runat="server" ClientIDMode="Static" CssClass="form-control">
                                            <asp:ListItem Text="Sum" Value="SUM"></asp:ListItem>
                                            <asp:ListItem Text="Average" Value="AVG"></asp:ListItem>
                                            <asp:ListItem Text="Maximum" Value="MAX"></asp:ListItem>
                                            <asp:ListItem Text="Minimum" Value="MIN"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Measure unit</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-money"></i></span>
                                        <asp:TextBox ID="txtUnidadValue" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Symbol</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-superscript"></i></span>
                                        <asp:TextBox ID="txtSimboloValue" runat="server" MaxLength="3" Width="60" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <fieldset>
                        <legend>Proposed Summary</legend>
                        <div class="form-group">
                            <asp:TextBox ID="txtResumenValue" runat="server" MaxLength="1000" ClientIDMode="Static" Height="100" TextMode="MultiLine" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>Proposed Description</legend>
                        <div class="form-group">
                            <CKEditor:CKEditorControl ID="txtDescripcionValue" runat="server" Toolbar="Basic"
                                ClientIDMode="Static" Height="260">
                            </CKEditor:CKEditorControl>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentScript" runat="server" ContentPlaceHolderID="ScriptsContentPlaceHolder">
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/revision.js") %>" type="text/javascript"></script>
</asp:Content>
