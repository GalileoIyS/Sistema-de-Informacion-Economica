<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/Blank.master" AutoEventWireup="true" CodeFile="novalidated.aspx.cs" Inherits="novalidated" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PagePlaceHolder" runat="Server">
    <div class="logo">
        <h1 class="semi-bold">
            <a href="http://www.dropkeys.com"><img runat="server" src="~/images/dropkeys.png" alt="DropKeys"/></a></h1>
    </div>
    <div>
        <img runat="server" src="~/images/noavatar.png" alt="" height="120" width="120">
        <div>
            <h3><i class="fa fa-user fa-3x text-muted air air-top-right hidden-mobile"></i>Hi,&nbsp;<asp:Literal ID="lbFirstAndLastName" runat="server"></asp:Literal>!</h3>
            <p>We have send you a validation code to the e-mail account provided to us.</p>
            <p>In the email, click on the activation link to validate your email address, and complete the registration. The link will launch a web browser and open to the DropKeys website.</p>
            <p class="txt-color-blueMarine">
                <strong>Have some problem?</strong> <a href="mailto:contact@dropkeys.com">contact@dropkeys.com</a>
            </p>
            <div class="alert alert-info fade in">
                <i class="fa-fw fa fa-info"></i><strong>Please check your email for the validation key</strong>
            </div>
            <div class="text-center">
                <asp:Button ID="btnSendCode" runat="server" Text="Send me again" CssClass="btn btn-danger" OnClick="btnSendCode_Click"/>
            </div>
        </div>
    </div>
    <p class="font-xs margin-top-5">
        Copyright Dropkeys 2014-2020.			
    </p>
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

