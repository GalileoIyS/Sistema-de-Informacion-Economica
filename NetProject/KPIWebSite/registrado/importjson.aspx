<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="importjson.aspx.cs" Inherits="registrado_importjson" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <asp:HiddenField ID="hdnIndicatorID" runat="server" ClientIDMode="Static" />
    <div class="container margin-top-lg margin-bottom-lg">
        <h2>
            <asp:Label ID="lbTitulo" runat="server" CssClass="color-primario-foreground"></asp:Label></h2>
        <ul class="breadcrumb">
            <li class="first">
                <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="glyphicon glyphicon-home">
                </span>&nbsp;Home</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="HlnkBuscador" runat="server" NavigateUrl="~/search.aspx"><i class="fa fa-search">
                </i>&nbsp;Search</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="HlnkIndicador" runat="server" NavigateUrl="~/indicator.aspx" ClientIDMode="Static">
                    <i class="fa fa-tachometer"></i>&nbsp;<asp:Label ID="lbIndicatorName" runat="server"></asp:Label>
                </asp:HyperLink></li>
            <li><span class="current-page"><span class="glyphicon glyphicon-save"></span>&nbsp;<strong>Import JSON data</strong></span></li>
        </ul>
        <div class="well">
            <a href="#" class="btn btn-primary btn-circle btn-xl pull-left margin-right-10"><i class="fa fa-2x fa-magic"></i></a>
            <h5>Welcome to the import data assistance. A wizard will guide you through a series of simple steps to help you importing the data through an JSON file</h5>
        </div>
        <div class="widget-body fuelux">
            <div class="wizard" id="ImportWizard">
                <ul class="steps">
                    <li class="active" data-target="#step1"><span class="badge">1</span>Data source<span class="chevron"></span></li>
                    <li data-target="#step2"><span class="badge">2</span>Data Fields<span class="chevron"></span></li>
                    <li data-target="#step3"><span class="badge">3</span>Other Attributes<span class="chevron"></span></li>
                    <li data-target="#step4"><span class="badge">4</span>Import mode<span class="chevron"></span></li>
                    <li data-target="#step5"><span class="badge">5</span>Name and description<span class="chevron"></span></li>
                </ul>
                <div class="actions">
                    <button class="btn btn-sm btn-primary btn-prev" type="button"><span class="glyphicon glyphicon-arrow-left"></span>&nbsp;Prev</button>
                    <button class="btn btn-sm btn-success btn-next" data-last="Complete&nbsp;" type="button">Next&nbsp;<span class="glyphicon glyphicon-arrow-right"></span></button>
                </div>
            </div>
            <div class="step-content well">
                <div class="form-horizontal">
                    <div id="step1" class="step-pane active">
                        <div class="row">
                            <div class="col-lg-4">
                                <asp:Image ID="imgLoadData" runat="server" ImageUrl="~/images/indicators/que-son-datasets.png" CssClass="img-responsive margin-top-10" />
                            </div>
                            <div class="col-lg-8">
                                <h4 class="StepTitle"><strong>Step 1. </strong>Select JSON Data Source</h4>
                                <div>
                                    Please click on the button labeled <strong>Browse ... </strong>and select the file with extension *.json from your hard drive. Then, wait a few seconds until the file is uploaded to our server
                                </div>
                                <fieldset>
                                    <legend><i class="fa fa-file-excel-o"></i>&nbsp;JSON Data Source File</legend>
                                    <div class="well">
                                        <div class="row">
                                            <div class="col-lg-8 text-align-right">
                                                <input id="fileToUpload" type="file" name="fileToUpload" class="btn-upload"/>
                                            </div>
                                            <div class="col-lg-4">
                                                <span id="uploadOkMessage" class="hidden">
                                                    <img src="../images/buttons/check.png" alt="exito" />File uploaded</span>
                                                <span id="uploadingMessage" class="hidden">
                                                    <i class="fa fa-refresh fa-spin"></i>&nbsp;Uploading file...</span>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                                <fieldset>
                                    <legend><i class="fa fa-calendar"></i>&nbsp;Datetime format</legend>
                                    <div class="form-group">
                                        <label class="col-md-4 control-label">Datetime format</label>
                                        <div class="col-md-8">
                                            <asp:DropDownList ID="cmbFormatInfo" runat="server" ClientIDMode="Static" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </fieldset>
                                <div id="messageFileExists" class="alert alert-info fade in margin-top-10">
                                    <i class="fa-fw fa fa-info"></i>Remember that <strong class="color-primario-foreground">you must upload the JSON file first</strong> in order to continue the proccess.
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="step2" class="step-pane">
                        <h4 class="StepTitle"><strong>Step 2. </strong>Source Fields</h4>
                        <p>
                            On the table above, we show the first 12 rows detected on the JSON file. Please, specify which fields correspond to each attribute.
                        </p>
                        <div id="messageAttributeExists" class="alert alert-info fade in margin-top-10">
                            <i class="fa-fw fa fa-info"></i>Note that the <strong>Date field </strong>and the <strong>Value Field </strong>are strictly <strong>required</strong>
                        </div>
                        <div id="TablaDatos">
                        </div>
                    </div>
                    <div id="step3" class="step-pane">
                        <div class="row">
                            <div class="col-sm-12 col-md-4 col-lg-4">
                                <h4 class="StepTitle"><strong>Step 3. </strong>Fill Other Attribute Values</h4>
                                <p>If you have the value of other attributes that are not present in your data file source, you can enter now their values using the textboxs on the right side of the panel</p>
                                <div class="text-center">
                                    <img src="../images/indicators/caracteristicas.png" alt="dropkeys attributes" />
                                </div>
                            </div>
                            <div class="col-sm-12 col-md-8 col-lg-8">
                                <div class="well">
                                    <div class="widget-body no-padding">
                                        <div class="panel-group smart-accordion-default" id="accordion-2">
                                            <asp:ListView ID="lstDimensiones" runat="server">
                                                <ItemTemplate>
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading">
                                                            <h4 class="panel-title">
                                                                <a class="dimension-title collapsed" data-toggle="collapse" href='<%# "#dimValue_" + Eval("DIMENSIONID")%>'><i class="fa fa-fw fa-plus-square txt-color-green"></i><i class="fa fa-fw fa-minus-square txt-color-red"></i><%#Eval("NOMBRE")%></a>
                                                            </h4>
                                                        </div>
                                                        <div id='<%# "dimValue_" + Eval("DIMENSIONID")%>' class="panel-collapse collapse">
                                                            <div class="panel-body">
                                                                <asp:TextBox ID="txtAttributeValues" runat="server" CssClass="tag-prefix attribute" data-dimensionid='<%#Eval("DIMENSIONID")%>'></asp:TextBox>
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
                    </div>
                    <div id="step4" class="step-pane">
                        <h4 class="StepTitle"><strong>Step 4. </strong>What would you like to do if data existed for a certain date and attributes?</h4>
                        <p>
                            See the following update modes available and select that one that best suits your requirements. Note that this mode is only used in the event that it intends to insert the data already in our database data.
                        </p>
                        <div class="row">
                            <div class="col-lg-4">
                                <asp:Image ID="imgImportMode" runat="server" ImageUrl="~/images/indicators/que-son-datasets.png" CssClass="img-responsive margin-top-10" />
                            </div>
                            <div class="col-lg-8">
                                <fieldset>
                                    <legend>Specify the update mode</legend>
                                    <div class="row margin-top-10">
                                        <input id="rbModo0" type="radio" name="rbmodo" value="0" checked />&nbsp;<strong>Mode 1:</strong>&nbsp;Replace the old value with the new one
                                    </div>
                                    <div class="row margin-top-10">
                                        <input id="rbModo1" type="radio" name="rbmodo" value="1" />&nbsp;<strong>Mode 2:</strong>&nbsp;Use the sum of both values
                                    </div>
                                    <div class="row margin-top-10">
                                        <input id="rbModo2" type="radio" name="rbmodo" value="2" />&nbsp;<strong>Mode 3:</strong>&nbsp;Use the maximum value of both
                                    </div>
                                    <div class="row margin-top-10">
                                        <input id="rbModo3" type="radio" name="rbmodo" value="3" />&nbsp;<strong>Mode 4:</strong>&nbsp;Use the minimum value of both
                                    </div>
                                    <div class="row margin-top-10">
                                        <input id="rbModo4" type="radio" name="rbmodo" value="4" />&nbsp;<strong>Mode 5:</strong>&nbsp;Use the average value of both
                                    </div>
                                    <div class="row margin-top-10">
                                        <input id="rbModo5" type="radio" name="rbmodo" value="5" />&nbsp;<strong>Mode 6:</strong>&nbsp;Dont do anything
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                    <div id="step5" class="step-pane">
                        <h4 class="StepTitle"><strong>Step 5. </strong>Enter a name and description</h4>
                        <p>
                            Finally, if you wish you can enter a <strong>name</strong> that will allow later identify the associated datasets such importation or leave the system do it automatically for you.
                        </p>
                        <div class="row">
                            <div class="col-lg-4">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/indicators/que-son-datasets.png" CssClass="img-responsive margin-top-10" />
                            </div>
                            <div class="col-lg-8">
                                <fieldset>
                                    <legend>Name and description of the import process</legend>
                                    <div class="form-group">
                                        <label class="col-md-2 control-label">Name</label>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtNombreImport" runat="server" placeholder="Enter the title..." ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-2 control-label">Description</label>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtDescripcionImport" runat="server" placeholder="Enter a brief description..." ClientIDMode="Static" TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="frmImportEnd" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title"><i class="fa fa-refresh fa-spin"></i>&nbsp;Running Import...</h4>
                </div>
                <div class="modal-body">
                    <div class="text-center margin-top-10">
                        The import process is sent to the server for processing.
                        <br />
                        This process can take a long time depending on how many rows you have filled in the table to import.
                    </div>
                    <div class="text-center margin-top-10">
                        Please, check periodically your import folder to view the status of the import process
                    </div>
                    <div class="text-center margin-top-10">
                        <asp:LinkButton ID="btnVolver" runat="server" ClientIDMode="Static" CssClass="btn btn-gray"><i class="fa fa-check"></i>&nbsp;&nbsp;&nbsp;Return</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentScript" runat="server" ContentPlaceHolderID="ScriptsContentPlaceHolder">
    <!-- Para el funcionamiento completo de los botones del asistente-->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/fuelux/wizard/wizard.min.js") %>" type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/fileupload/ajaxfileupload.js") %>" type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/fileupload/jquery.nicefileinput.min.js") %>" type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("~/scripts/jquery/jquery.handsontable.full.js") %>" type="text/javascript"></script>
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/importjson.js") %>" type="text/javascript"></script>
</asp:Content>
