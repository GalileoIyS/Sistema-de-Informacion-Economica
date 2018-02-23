<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/Blank.master" AutoEventWireup="true" CodeFile="customerror.aspx.cs" Inherits="errors_notfound" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PagePlaceHolder" runat="Server">

    <!-- MAIN CONTENT -->
    <div id="content">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="text-center error-box">
                            <h1 class="error-text-2 bounceInDown animated">Unexpected Error <span class="particle particle--c"></span><span class="particle particle--a"></span><span class="particle particle--b"></span></h1>
                            <h2 class="font-xl"><strong><i class="fa fa-fw fa-warning fa-lg text-warning"></i>Oooops!</strong></h2>
                            <br />
                            <p class="lead">
                                <asp:Label ID="FriendlyErrorMsg" runat="server" Text="Label"></asp:Label>
                            </p>
                            <p class="font-md">
                                <b>... That didn't work on you? Dang. May we suggest a search, then?</b>
                            </p>
                            <br />
                            <div class="error-search well well-lg padding-10">
                                    <a runat="server" href="~/Default.aspx" class="btn btn-primary"><i class="fa fa-home"></i>&nbsp;GO HOME</a>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Panel ID="DetailedErrorPanel" runat="server" Visible="false">
                                        <p>&nbsp;</p>
                                        <h4>Detalle del Error:</h4>
                                        <p>
                                            <asp:Label ID="ErrorDetailedMsg" runat="server" Font-Size="Small" /><br />
                                        </p>

                                        <h4>Error Handler:</h4>
                                        <p>
                                            <asp:Label ID="ErrorHandler" runat="server" Font-Size="Small" /><br />
                                        </p>

                                        <h4>Motivo:</h4>
                                        <p>
                                            <asp:Label ID="InnerMessage" runat="server" Font-Size="Small" /><br />
                                        </p>
                                        <p>
                                            <asp:Label ID="InnerTrace" runat="server" />
                                        </p>
                                        <hr />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END MAIN CONTENT -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPlaceHolder" runat="Server">
    <!-- Link to Google CDN's jQuery + jQueryUI; fall back to local -->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.0.2/jquery.min.js"></script>
    <script>
        if (!window.jQuery) {
            document.write('<script src="scripts/libs/jquery-2.0.2.min.js"><\/script>');
        }
    </script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/jquery-ui.min.js"></script>
    <script>
        if (!window.jQuery.ui) {
            document.write('<script src="scripts/libs/jquery-ui-1.10.3.min.js"><\/script>');
        }
    </script>

    <!-- Libreria javascript de bootstrap-->
    <script src="<%=Page.ResolveUrl("~/scripts/bootstrap.min.js") %>" type="text/javascript"></script>
</asp:Content>

