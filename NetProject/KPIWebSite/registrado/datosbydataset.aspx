<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="datosbydataset.aspx.cs" Inherits="registrado_datosbydataset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="ContentCenter" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <asp:HiddenField ID="HdnDatasetId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HdnTemporal" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HdnUnidad" runat="server" ClientIDMode="Static" />
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
            <li><span class="current-page"><span class="glyphicon glyphicon-save"></span>&nbsp;<strong><asp:Label ID="lbDatasetName" runat="server"></asp:Label></strong></span></li>
        </ul>
        <asp:Panel runat="server" ID="PanelDeAtributos" CssClass="well">
            <div class="row">
                <div class="col-sm-12 col-md-4 col-lg-4">
                    <h4>Fill Dropkeys Attribute Values</h4>
                    <p>Complete the meaning of your dataset adding attribute values</p>
                    <img src="../images/indicators/caracteristicas.png" alt="dropkeys attributes" />
                    <br />
                    <p>
                        On the right side, you will find all the attribute elements added by the community. Please, feel free to populate with <strong>data tags</strong> all those who best describe the data associated below.
                    </p>
                </div>
                <div class="col-sm-12 col-md-8 col-lg-8">
                    <div class="well">
                        <div class="widget-body no-padding">
                            <div class="panel-group smart-accordion-default" id="accordion-2">
                                <asp:ListView ID="lstDimensiones" runat="server" OnItemDataBound="lstDimensiones_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a class="dimension-title collapsed" data-toggle="collapse" data-dimensionid='<%#Eval("DIMENSIONID")%>' href='<%# "#dimValue_" + Eval("DIMENSIONID")%>'><i class="fa fa-fw fa-plus-square txt-color-green"></i><i class="fa fa-fw fa-minus-square txt-color-red"></i><%#Eval("NOMBRE")%>&nbsp;(<asp:Label ID="lbNumDimensionX" runat="server"></asp:Label>)
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id='<%# "dimValue_" + Eval("DIMENSIONID")%>' class="panel-collapse collapse">
                                                <div class="panel-body" data-dimensionid='<%#Eval("DIMENSIONID")%>'>
                                                    <asp:TextBox ID="txtDimensionValues" runat="server" CssClass="tag-prefix" data-dimensionid='<%#Eval("DIMENSIONID")%>'></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="well">
            <h4>Fill Dataset Values</h4>
            <p>
                Use the grid below to introduce the data associated with this dataset. You can use the arrows on the rigth top side of the table to navigate between dates.
            </p>
            <div class="alert alert-info fade in">
                <button class="close" data-dismiss="alert">x</button>
                <i class="fa-fw fa fa-info"></i>
                <strong>Advice!</strong>&nbsp;The sampling frecuency defined on this dataset will be used to populate the datetime column.
            </div>
            <div class="collapse navbar-collapse">
                <ul class="nav navbar-nav navbar-right">
                    <li><a id="btnPreviousTop" onclick="Siguiente();"><span class="glyphicon glyphicon-chevron-right"></span></a></li>
                </ul>
                <div class="navbar-form navbar-right">
                    <div class="form-group">
                        <input id="txtPreviousTop" type="text" size="4" value="15" class="form-control" />
                    </div>
                </div>
                <ul class="nav navbar-nav navbar-right">
                    <li class="disabled"><a href="#"><span class="glyphicon glyphicon-minus"></span></a></li>
                </ul>
                <div class="navbar-form navbar-right">
                    <div class="form-group">
                        <input id="txtNextTop" type="text" size="4" value="1" class="form-control" />
                    </div>
                </div>
                <ul class="nav navbar-nav navbar-right no-margin-right">
                    <li><a id="btnNextTop" onclick="Anterior();"><span class="glyphicon glyphicon-chevron-left"></span></a></li>
                </ul>
            </div>
            <div class="well">

                <div id="TablaDatos" class="table">
                </div>
                <p class="text-center">
                    DropKeys property ensures that all data entered will remain anonymous and will only be used to estimate aggregate functions compared in no time the authors of them appears.
                </p>
            </div>
            <div class="pull-right">
                <a id="btnGuardar" onclick="Guardar();" class="btn btn-dropkeys">Save</a>
            </div>
            <div class="clearfix"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentScript" runat="server" ContentPlaceHolderID="ScriptsContentPlaceHolder">
    <%--<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>--%>
    <script src="<%=Page.ResolveUrl("~/scripts/jquery/jquery.handsontable.full.js") %>"
        type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("~/scripts/jquery/jquery.tagsinput.min.js") %>" type="text/javascript"></script>
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/datos.js") %>" type="text/javascript"></script>
</asp:Content>
