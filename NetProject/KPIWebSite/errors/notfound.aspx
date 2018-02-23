<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/Blank.master" AutoEventWireup="true" CodeFile="notfound.aspx.cs" Inherits="errors_notfound" %>

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
                            <h1 class="error-text-2 bounceInDown animated">Error 404 <span class="particle particle--c"></span><span class="particle particle--a"></span><span class="particle particle--b"></span></h1>
                            <h2 class="font-xl"><strong><i class="fa fa-fw fa-warning fa-lg text-warning"></i>Page <u>Not</u> Found</strong></h2>
                            <br />
                            <p class="lead">
                                The page you requested could not be found, either contact your webmaster or try again. Use your browsers <b>Back</b> button to navigate to the page you have prevously come from
                            </p>
                            <p class="font-md">
                                <b>... That didn't work on you? Dang. May we suggest a search, then?</b>
                            </p>
                            <br/>
                            <div class="error-search well well-lg padding-10">
                                <div class="input-group">
                                    <asp:TextBox id="txtSearchError" runat="server" CssClass="form-control input-lg" placeholder="let's try this again" OnTextChanged="txtSearchError_TextChanged" />
                                    <span class="input-group-addon"><i class="fa fa-fw fa-lg fa-search"></i></span>
                                </div>
                            </div>
                            <div class="row">

                                <div class="col-sm-12">
                                    <asp:LoginView ID="LoginViewIndicador" runat="server">
                                        <AnonymousTemplate>
                                            <ul class="list-inline">
                                                <li>&nbsp;<asp:HyperLink ID="lnkAnonymousHome" runat="server" NavigateUrl="~/Default.aspx">Dropkeys</asp:HyperLink>&nbsp;
                                                </li>
                                                <li>.
                                                </li>
                                                <li>&nbsp;<asp:HyperLink ID="lnkAnonymousHowItWorks" runat="server" NavigateUrl="~/howitworks.aspx">How it works</asp:HyperLink>&nbsp;
                                                </li>
                                                <li>.
                                                </li>
                                                <li>&nbsp;<asp:HyperLink ID="lnkAnonymousSearch" runat="server" NavigateUrl="~/search.aspx">Search</asp:HyperLink>&nbsp;
                                                </li>
                                            </ul>
                                        </AnonymousTemplate>
                                        <LoggedInTemplate>
                                            <ul class="list-inline">
                                                <li>&nbsp;<asp:HyperLink ID="lnkLoggedInHome" runat="server" NavigateUrl="~/Default.aspx">Dropkeys</asp:HyperLink>&nbsp;
                                                </li>
                                                <li>.
                                                </li>
                                                <li>&nbsp;<asp:HyperLink ID="lnkLoggedInHowItWorks" runat="server" NavigateUrl="~/howitworks.aspx">How it works</asp:HyperLink>&nbsp;
                                                </li>
                                                <li>.
                                                </li>
                                                <li>&nbsp;<asp:HyperLink ID="lnkLoggedInSearch" runat="server" NavigateUrl="~/search.aspx">Search</asp:HyperLink>&nbsp;
                                                </li>
                                                <li>.
                                                </li>
                                                <li>&nbsp;<asp:HyperLink ID="lnkLoggedInDesktop" runat="server" NavigateUrl="~/registrado/desktop.aspx">Desktop</asp:HyperLink>&nbsp;
                                                </li>
                                                <li>.
                                                </li>
                                                <li>&nbsp;<asp:HyperLink ID="lnkLoggedInDashboard" runat="server" NavigateUrl="~/registrado/dashboard.aspx">Dashboard</asp:HyperLink>&nbsp;
                                                </li>
                                                <li>.
                                                </li>
                                                <li>&nbsp;<asp:HyperLink ID="lnkLoggedInMyProfile" runat="server" NavigateUrl="~/registrado/profile.aspx">My Profile</asp:HyperLink>&nbsp;
                                                </li>
                                            </ul>
                                        </LoggedInTemplate>
                                    </asp:LoginView>
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

