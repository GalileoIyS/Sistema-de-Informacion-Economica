<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="profile.aspx.cs" Inherits="registrado_profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <div class="container margin-top-lg margin-bottom-lg">
        <div class="row">
            <div class="col-sm-12 col-md-12 col-lg-12">
                <div class="well well-light well-sm no-margin no-padding">
                    <div class="row">
                        <div class="col-sm-12">
                            <div id="myCarousel" class="position-relative">
                                <div class="air air-bottom-right padding-10">
                                    <div class="btn-group">
                                        <a class="btn btn-default" href="javascript:void(0);"><i class="fa fa-gear"></i></a>
                                        <a class="btn btn-default dropdown-toggle" data-toggle="dropdown" href="javascript:void(0);"><span class="caret"></span></a>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <a id="btnChangePhoto" href="#"><i class="fa fa-camera"></i>&nbsp;Change photo</a>
                                            </li>
                                            <li>
                                                <a href="#" data-toggle="modal" data-target="#frmChangePassword"><i class="fa fa-unlock-alt"></i>&nbsp;Change password</a>
                                            </li>
                                            <li>
                                                <a id="btnChangeProfile" href="#"><i class="fa fa-user"></i>&nbsp;Change profile data</a>
                                            </li>
                                            <li class="divider"></li>
                                            <li>
                                                <a href="/registrado/createkpi.aspx"><i class="fa fa-dashboard"></i>&nbsp;Add Indicator</a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="background-avatar">
                                    <img src="/images/pages/profile-background.jpg" alt="demo user" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-lg-3 col-sm-3 hidden-xs profile-pic">
                                    <asp:Image runat="server" ID="imagePerfilUsuario" ImageUrl="~/images/noavatar.png" AlternateText="user photo" ClientIDMode="Static" Style="border: 5px solid #dcdcdc;" />
                                    <div class="padding-10">
                                        <h4 class="font-md"><strong>
                                            <asp:Label ID="lbNumberOfFormulas" runat="server" Text="Label" CssClass="txt-color-blue font-lg"></asp:Label></strong>
                                            <br />
                                            FORMULAS</h4>
                                        <h4 class="font-md"><strong>
                                            <asp:Label ID="lbNumberOfWidgets" runat="server" Text="Label" CssClass="txt-color-blue font-lg"></asp:Label></strong>
                                            <br />
                                            GRAPHS</h4>
                                        <h4 class="font-md"><strong>
                                            <asp:Label ID="lbNumberOfDashboards" runat="server" Text="Label" CssClass="txt-color-blue font-lg"></asp:Label></strong>
                                            <br />
                                            DASHBOARDS</h4>
                                    </div>
                                </div>
                                <div class="col-lg-9 col-sm-9 col-xs-12">
                                    <h3>
                                        <asp:Label ID="lbNombreUsuario" runat="server"></asp:Label>
                                        <br />
                                        <small>
                                            <asp:Label ID="lbPuestoUsuario" runat="server"></asp:Label></small>
                                    </h3>
                                    <div class="col-lg-7 col-sm-7 col-xs-12">
                                        <div id="view-profile-data">
                                            <!-- front content -->
                                            <h4>
                                                <i class="fa fa-user"></i>&nbsp;&nbsp;Profile information
                                            </h4>
                                            <ul class="list-unstyled">
                                                <li>
                                                    <p class="text-muted">
                                                        <i class="fa fa-envelope"></i>&nbsp;&nbsp;<span class="txt-color-darken">Em@il</span>&nbsp;<asp:Label ID="lbEmailUsuario" runat="server"></asp:Label>
                                                    </p>
                                                </li>
                                                <li>
                                                    <p class="text-muted">
                                                        <i class="fa fa-calendar"></i>&nbsp;&nbsp;<span class="txt-color-darken">Creation date</span>&nbsp;<asp:Label ID="lbFechaDeAltaUsuario" runat="server"></asp:Label>
                                                    </p>
                                                </li>
                                                <li>
                                                    <p class="text-muted">
                                                        <i class="fa fa-key"></i>&nbsp;&nbsp;<span class="txt-color-darken">Last access</span>&nbsp;<asp:Label ID="lbUltimoAccesoUsuario" runat="server"></asp:Label>
                                                    </p>
                                                </li>
                                                <li>
                                                    <p class="text-muted">
                                                        <i class="fa fa-share-alt-square"></i>&nbsp;&nbsp;<span class="txt-color-darken">
                                                            <asp:Label ID="lbIndicadoresCompartidos" runat="server">23</asp:Label>&nbsp;indicators shared</span>
                                                    </p>
                                                </li>
                                                <li>
                                                    <h4>
                                                        <i class="fa fa-info-circle"></i>&nbsp;&nbsp;A little about me...
                                                    </h4>
                                                    <p class="margin-top-10">
                                                        <asp:Label ID="lbResumenUsuario" runat="server"></asp:Label>
                                                    </p>
                                                </li>
                                            </ul>
                                        </div>
                                        <div id="edit-profile-data" class="hidden">
                                            <div class="form-group">
                                                <label>First name</label>
                                                <asp:TextBox ID="txtFirstName" runat="server" ClientIDMode="Static" CssClass="form-control" placeholder="Input your name"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label>Last name</label>
                                                <asp:TextBox ID="txtLastName" runat="server" ClientIDMode="Static" CssClass="form-control" placeholder="Input your last name"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label>A little about me</label>
                                                <asp:TextBox ID="txtResume" runat="server" TextMode="MultiLine" Rows="4" ClientIDMode="Static" CssClass="form-control" placeholder="A little about me"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <button class="btn btn-labeled btn-primary" type="submit">
                                                            <span class="btn-label"><i class="fa fa-times"></i></span>Cancel
                                                        </button>
                                                        <asp:LinkButton runat="server" ID="btnSaveDataProfile" CssClass="btn btn-labeled btn-success" OnClick="btnSaveDataProfile_Click">
                                                                    <span class="btn-label"><i class="fa fa-check"></i></span>Save
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5 col-sm-5 hidden-xs">
                                        <h4>
                                            <i class="fa fa-tachometer"></i>&nbsp;&nbsp;Most used dropkeys                                                     
                                        </h4>
                                        <ul class="list-inline friends-list">
                                            <asp:ListView ID="lstTopSixPublicIndicators" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <asp:HyperLink runat="server" ID="lnkPublicIndicatorPage" NavigateUrl='<%# "~/indicator.aspx?indicatorid=" + Eval("INDICATORID") %>' title='<%# Eval("TITULO") %>'><img src='<%# Eval("IMAGEURL") %>'/></asp:HyperLink>
                                                    </li>
                                                </ItemTemplate>
                                                <EmptyDataTemplate>
                                                    <li>
                                                        <img src="../images/noimage.png" alt="no indicators" />
                                                    </li>
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                            <li runat="server" id="PanelMasIndicators">
                                                <asp:Label ID="lbMasIndicators" runat="server"></asp:Label>
                                            </li>
                                        </ul>
                                        <h4>
                                            <i class="fa fa-group"></i>&nbsp;&nbsp;Lastest connections                                                       
                                        </h4>
                                        <ul class="list-inline friends-list">
                                            <asp:ListView ID="lstLastSixFriends" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <asp:HyperLink runat="server" ID="lnkPrivateIndicatorPage" CssClass="show-modal-info-user" NavigateUrl='#' data-userid='<%# Eval("USERID") %>'><img src='<%# Eval("IMAGEURL") %>'/></asp:HyperLink>
                                                    </li>
                                                </ItemTemplate>
                                                <EmptyDataTemplate>
                                                    <li>
                                                        <img runat="server" src="~/images/noavatar.png" alt="no friends" />
                                                    </li>
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                            <li runat="server" id="PanelMasFriends">
                                                <asp:Label ID="lbMasFriends" runat="server"></asp:Label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <hr />
                        <div class="padding-10">
                            <ul class="nav nav-tabs bordered">
                                <li class="active">
                                    <a href="#TabActivity" data-toggle="tab"><i class="fa fa fa-fw fa-lg fa-rss"></i>&nbsp;Recent Activity<asp:Label runat="server" ClientIDMode="Static" ID="numTabActivity" class="badge bg-color-blueMedium txt-color-white">12</asp:Label></a>
                                </li>
                                <li>
                                    <a href="#TabFriends" data-toggle="tab"><i class="fa fa fa-fw fa-lg fa-group"></i>&nbsp;Friends<asp:Label runat="server" ClientIDMode="Static" ID="numTabFriends" class="badge bg-color-blueMedium txt-color-white">12</asp:Label></a>
                                </li>
                                <li>
                                    <a href="#TabComments" data-toggle="tab"><i class="fa fa-fw fa-lg fa-comments"></i>&nbsp;Comments<asp:Label runat="server" ClientIDMode="Static" ID="numTabComments" class="badge bg-color-blueMedium txt-color-white">12</asp:Label></a>
                                </li>
                                <li>
                                    <a href="#TabIndicators" data-toggle="tab"><i class="fa fa-fw fa-lg fa-tachometer"></i>&nbsp;Dropkeys<asp:Label runat="server" ClientIDMode="Static" ID="numTabIndicators" class="badge bg-color-blueMedium txt-color-white">12</asp:Label></a>
                                </li>
                                <li>
                                    <a href="#TabDashboards" data-toggle="tab"><i class="fa fa-fw fa-lg fa-bar-chart-o"></i>&nbsp;Dashboards<asp:Label runat="server" ClientIDMode="Static" ID="numTabDashboards" class="badge bg-color-blueMedium txt-color-white">12</asp:Label></a>
                                </li>
                            </ul>
                            <div class="tab-content padding-top-10">
                                <div class="tab-pane fade in active min-height-md" id="TabActivity">
                                    <!-- Timeline Content -->
                                    <div class="smart-timeline">
                                        <ul class="smart-timeline-list">

                                            <asp:ListView ID="lstActivity" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <div class="smart-timeline-icon">
                                                            <%# Eval("ICONO") %>
                                                        </div>
                                                        <div class="smart-timeline-time">
                                                            <small><%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA")))%></small>
                                                        </div>
                                                        <div class="smart-timeline-content">
                                                            <p>
                                                                <strong>
                                                                    <asp:HyperLink ID="LnkIndicador" runat="server" ClientIDMode="Static" NavigateUrl='<%# "~/indicator.aspx?indicatorid=" + Eval("REFID") %>'><span class="pictogram indicator-icon"></span>&nbsp;<%#Eval("TITULO")%></asp:HyperLink></strong>
                                                            </p>
                                                            <p>
                                                                The user&nbsp;<asp:HyperLink ID="LnkUsuario" runat="server" ClientIDMode="Static" CssClass="show-modal-info-user" data-userid='<%# Eval("USERID") %>' NavigateUrl="#"><%#Eval("NOMBRE") + " " + Eval("APELLIDOS") %></asp:HyperLink>&nbsp;<%#Eval("MESSAGE")%>
                                                            </p>
                                                            <%# Eval("CONTENIDO") %>
                                                        </div>
                                                    </li>
                                                </ItemTemplate>
                                                <EmptyDataTemplate>
                                                    <div class="search">
                                                        <div class="col-lg-1 col-xs-1 col-sm-1">
                                                            <i class="fa fa-gear fa-4x fa-rss"></i>
                                                        </div>
                                                        <div class="col-lg-11 col-xs-11 col-sm-11">
                                                            No activity
                                                        </div>
                                                    </div>
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                            <li class="text-center">
                                                <a href="javascript:void(0)" class="btn btn-sm btn-default"><i class="fa fa-arrow-down text-muted"></i>LOAD MORE</a>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- END Timeline Content -->
                                </div>
                                <div class="tab-pane fade min-height-md" id="TabFriends">
                                    <asp:ListView ID="lstFriends" runat="server" EnableViewState="True">
                                        <ItemTemplate>
                                            <div class="search elemFriendship" data-userid='<%#Eval("USERID")%>'>
                                                <div class="col-lg-1 col-xs-1 col-sm-1">
                                                    <div class="friends-list">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("IMAGEURL") %>' AlternateText='<%# Eval("NOMBRE") %>' />
                                                    </div>
                                                </div>
                                                <div class="col-lg-11 col-xs-11 col-sm-11">
                                                    <h6 class="no-margin">
                                                        <asp:HyperLink ID="lnkViewUserInfo" runat="server" CssClass="show-modal-info-user" data-userid='<%#Eval("USERID")%>' NavigateUrl="#"><%#Eval("APELLIDOS")%>,&nbsp;<%#Eval("NOMBRE")%></asp:HyperLink></h6>
                                                    <div class="iconos hidden-xs">
                                                        <asp:Label runat="server" title="Add date"><span class="glyphicon glyphicon-calendar"></span>&nbsp;<%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA")))%></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </div>
                                                    <asp:HyperLink ID="lnkRemoveFriend" runat="server" CssClass="add-indicator lnkRemoveFriend" NavigateUrl="#" data-fromuserid='<%# Eval("USERID") %>'><i class="fa fa-times"></i>&nbsp;Remove</asp:HyperLink>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="search">
                                                <div class="col-lg-1 col-xs-1 col-sm-1">
                                                    <i class="fa fa-gear fa-4x fa-group"></i>
                                                </div>
                                                <div class="col-lg-11 col-xs-11 col-sm-11">
                                                    No friends
                                                </div>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                                <div class="tab-pane fade min-height-md" id="TabComments">
                                    <asp:ListView ID="lstComments" runat="server" EnableViewState="True">
                                        <ItemTemplate>
                                            <div class="search elemComment" data-commentid='<%# Eval("COMMENTID") %>'>
                                                <div class="col-xs-2 col-sm-1">
                                                    <i class="fa fa-4x fa-comment-o"></i>
                                                </div>
                                                <div class="col-xs-10 col-sm-11">
                                                    <h6 class="no-margin">
                                                        <asp:HyperLink ID="lnkEditIndicador" runat="server" NavigateUrl='<%#"~/indicator.aspx?indicatorid=" + Eval("INDICATORID") + "#lnkPostComment"%>'><%#Eval("TITULO")%></asp:HyperLink></h6>
                                                    <p>
                                                        <%# Eval("COMENTARIO") %>
                                                    </p>
                                                    <div class="iconos hidden-xs">
                                                        <asp:Label runat="server" title="Creation date"><span class="glyphicon glyphicon-calendar"></span><%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA")))%></asp:Label>
                                                    </div>
                                                    <asp:HyperLink ID="lnkRemoveComment" runat="server" ClientIDMode="Static" CssClass="add-indicator lnkRemoveComment" NavigateUrl="#" data-commentid='<%# Eval("COMMENTID") %>'><i class="fa fa-times"></i>&nbsp;Remove</asp:HyperLink>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="search">
                                                <div class="col-lg-1 col-xs-1 col-sm-1">
                                                    <i class="fa fa-gear fa-4x fa-comments"></i>
                                                </div>
                                                <div class="col-lg-11 col-xs-11 col-sm-11">
                                                    No comments
                                                </div>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                                <div class="tab-pane fade min-height-md" id="TabIndicators">
                                    <asp:ListView ID="lstIndicators" runat="server" EnableViewState="True">
                                        <ItemTemplate>
                                            <div class="search elemIndicator" data-indicatorid='<%# Eval("INDICATORID") %>'>
                                                <div class="col-xs-2 col-sm-1">
                                                    <div class="circular width50" style='border: 5px solid <%# Eval("ESTILO") %>;'>
                                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("IMAGEURL") %>' AlternateText='<%# Eval("TITULO") %>' Width="100" />
                                                    </div>
                                                </div>
                                                <div class="col-xs-10 col-sm-11">
                                                    <h6 class="no-margin">
                                                        <asp:HyperLink ID="lnkEditIndicador" runat="server" NavigateUrl='<%#"~/indicator.aspx?indicatorid=" + Eval("INDICATORID")%>'><%#Eval("TITULO")%></asp:HyperLink></h6>
                                                    <div><%# Eval("RESUMEN") %></div>
                                                    <div class="iconos hidden-xs">
                                                        <asp:Label runat="server" title="Add date"><span class="glyphicon glyphicon-calendar"></span>&nbsp;<%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA")))%></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label runat="server" title="Number of datasets"><i class="fa fa-dot-circle-o"></i>&nbsp;<%# Eval("NUM_DATASETS")%></asp:Label>&nbsp;datasets
                                                    </div>
                                                    <asp:HyperLink ID="lnkRemoveIndicator" runat="server" ClientIDMode="Static" CssClass="add-indicator lnkRemoveIndicator" NavigateUrl="#" data-indicatorid='<%# Eval("INDICATORID") %>'><i class="fa fa-times"></i>&nbsp;Remove</asp:HyperLink>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="search">
                                                <div class="col-lg-1 col-xs-1 col-sm-1">
                                                    <div class="friends-list">
                                                        <i class="fa fa-gear fa-4x fa-tachometer"></i>
                                                    </div>
                                                </div>
                                                <div class="col-lg-11 col-xs-11 col-sm-11">
                                                    No dropkeys
                                                </div>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                                <div class="tab-pane fade min-height-md" id="TabDashboards">
                                    <asp:ListView ID="lstDashboards" runat="server">
                                        <ItemTemplate>
                                            <div class="search elemDashboard" data-dashboardid='<%# Eval("IDDASHBOARD") %>'>
                                                <div class="col-xs-2 col-sm-1">
                                                    <div class="friends-list">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/indicators/dashboard.png" />
                                                    </div>
                                                </div>
                                                <div class="col-xs-10 col-sm-11">
                                                    <h6 class="no-margin">
                                                        <asp:HyperLink ID="lnkEditIndicador" runat="server" NavigateUrl='<%#"~/registrado/dashboard.aspx?iddashboard=" + Eval("IDDASHBOARD")%>'><%# Eval("TITULO") %></asp:HyperLink>
                                                    </h6>
                                                    <div class="iconos">
                                                        <span title="Creation date"><span class="glyphicon glyphicon-calendar"></span>&nbsp;<%# CalculaFechaDesdeCuando(Convert.ToString(Eval("FECHA_ALTA"))) %></span>
                                                    </div>
                                                    <asp:HyperLink ID="lnkRemoveDashboard" runat="server" ClientIDMode="Static" CssClass="add-indicator lnkRemoveDashboard" NavigateUrl="#" data-dashboardid='<%# Eval("IDDASHBOARD") %>'><i class="fa fa-times"></i>&nbsp;Remove</asp:HyperLink>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="search">
                                                <div class="col-lg-1 col-xs-1 col-sm-1">
                                                    <i class="fa fa-gear fa-4x fa-bar-chart-o"></i>
                                                </div>
                                                <div class="col-lg-11 col-xs-11 col-sm-11">
                                                    No dashboards
                                                </div>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <!-- end tab -->
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- end row -->
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
    <!-- Modal -->
    <div id="frmChangePassword" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">CHANGE PASSWORD</h4>
                </div>
                <div class="modal-body">
                    <div role="form">
                        <div id='PanelErrorPassword' class='form-group hidden'>
                            <div class='alert alert-danger fade in'>
                                <button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button>
                                <span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The password is invalid
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="txtNombre">Old password</label>
                            <asp:TextBox runat="server" ID="txtOldUserPassword" placeholder="Enter the old password"
                                ClientIDMode="Static" MaxLength="200" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtNombre">New password</label>
                            <asp:TextBox runat="server" ID="txtNewUserPassword" placeholder="Enter the new password"
                                ClientIDMode="Static" MaxLength="200" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtNombre">Confirm password</label>
                            <asp:TextBox runat="server" ID="txtNewUserPasswordRepeated" placeholder="Repeat the new password"
                                ClientIDMode="Static" MaxLength="200" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a id="btnChangePwd" class="btn btn-dropkeys pull-right" href="#">Confirm</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentScript" runat="server" ContentPlaceHolderID="ScriptsContentPlaceHolder">
    <!-- Para mostrar las formulas en formato gráfico -->
    <script src="<%=Page.ResolveUrl("~/scripts/varios/jqmath-etc-0.4.0.min.js") %>" type="text/javascript"></script>
    <!-- Para crear mini-gráficos representativos de los últimos 10 datos de un dataset-->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/sparkline/jquery.sparkline.min.js") %>" type="text/javascript"></script>
    <!-- Para mejorar la visualización de la subida de imagenes/ficheros -->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/fileupload/jquery.nicefileinput.min.js") %>" type="text/javascript"></script>
    <!-- Para recortar las imagenes -->
    <script src="<%=Page.ResolveUrl("~/scripts/plugin/jcrop/jquery.Jcrop.min.js") %>" type="text/javascript"></script>
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/profile.js") %>" type="text/javascript"></script>
</asp:Content>
