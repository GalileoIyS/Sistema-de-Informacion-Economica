<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="configuracion.aspx.cs" Inherits="administrador_configuracion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <h2>
        <asp:Label ID="lbTitulo" runat="server" CssClass="color-terciario-foreground"></asp:Label></h2>
    <ul class="breadcrumb">
        <li class="first">
            <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="pictogram home-icon">
                </span>&nbsp;Inicio</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HlnkBuscador" runat="server" NavigateUrl="~/search.aspx"><span class="pictogram administrator-icon">
                </span>&nbsp;Administrador</asp:HyperLink></li>
        <li><span class="current-page"><span class="pictogram current-page-icon"></span>&nbsp;<strong>Parametros de configuración</strong></span></li>
    </ul>
    <div class="contenido-izquierda">
        <div class="margintop">
            <ul class="idTabs tab-indicator-menu">
                <li><a href="#General"><span class="pictogram home"></span>&nbsp;GENERAL</a></li>
                <li><a href="#Correo"><span class="pictogram dimension-icon"></span>&nbsp;<span>CORREO</span></a></li>
                <li><a href="#Otros"><span class="pictogram dataset"></span>&nbsp;<span>OTROS</span></a></li>
            </ul>
        </div>
        <div id="General" class="article">
            <div class="article-content article-min-height">
                &nbsp;
            </div>
        </div>
        <div id="Correo" class="article no-mostrar">
            <div class="article-content article-min-height">
                <h4>
                    SERVIDOR DE CORREO</h4>
                <div class="row">
                    <asp:Label ID="lbSmtpHost" runat="server" Text="Servidor" EnableViewState="false"></asp:Label>
                    <asp:TextBox ID="txtSmtpHost" runat="server" CssClass="textbox" MaxLength="250" Width="300"></asp:TextBox>
                    <div class="explicacion">
                        <asp:Literal ID="lbSmtpHostDesc" runat="server" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
                <div class="row">
                    <asp:Label ID="lbSmtpPort" runat="server" Text="Puerto" EnableViewState="false"></asp:Label>
                    <asp:TextBox ID="txtSmtpPort" runat="server" CssClass="textbox" MaxLength="250" Width="50" ValidationGroup="GrupoDeParametros"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="PortRegularExpressionValidator" runat="server"
                        ErrorMessage="Número inválido" ValidationExpression="^\d+$" ControlToValidate="txtSmtpPort"
                        ValidationGroup="GrupoDeParametros" ForeColor="#ff0000" Display="Dynamic">&nbsp;Puerto inválido</asp:RegularExpressionValidator>
                    <div class="explicacion">
                        <asp:Literal ID="lbSmtpPortDesc" runat="server" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
                <div class="row">
                    <asp:Label ID="lbSmtpUser" runat="server" Text="Usuario" EnableViewState="false"></asp:Label>
                    <asp:TextBox ID="txtSmtpUser" runat="server" CssClass="textbox" MaxLength="250" Width="300"></asp:TextBox>
                    <div class="explicacion">
                        <asp:Literal ID="lbSmtpUserDesc" runat="server" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
                <div class="row">
                    <asp:Label ID="lbSmtpPassword" runat="server" Text="Password" EnableViewState="false"></asp:Label>
                    <asp:TextBox ID="txtSmtpPassword" runat="server" CssClass="textbox" MaxLength="250"
                        Width="300"></asp:TextBox>
                    <div class="explicacion">
                        <asp:Literal ID="lbSmtpPasswordDesc" runat="server" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
                <div class="row">
                    <asp:Label ID="lbSmtpTimeOut" runat="server" Text="Tiempo de espera" EnableViewState="false"></asp:Label>
                    <asp:TextBox ID="txtSmtpTimeOut" runat="server" CssClass="textbox" MaxLength="250"
                        Width="50" ValidationGroup="GrupoDeParametros"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="TimeoutRegularExpressionValidator" runat="server"
                        ErrorMessage="Número inválido" ValidationExpression="^\d+$" Display="Dynamic"
                        ControlToValidate="txtSmtpTimeOut" ValidationGroup="GrupoDeParametros" ForeColor="#ff0000">Timeout inválido</asp:RegularExpressionValidator>
                    <div class="explicacion">
                        <asp:Literal ID="lbSmtpTimeOutDesc" runat="server" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
                <div class="row">
                    <asp:Label ID="lbSmtpSslMode" runat="server" Text="Modo Seguro" EnableViewState="false"></asp:Label>
                    <asp:TextBox ID="txtSmtpSslMode" runat="server" CssClass="textbox" MaxLength="250"
                        Width="50" ValidationGroup="GrupoDeParametros"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="SslModeRegularExpressionValidator" runat="server"
                        ErrorMessage="Número inválido" ValidationExpression="[0-1]" Display="Dynamic"
                        ControlToValidate="txtSmtpSslMode" ValidationGroup="GrupoDeParametros" ForeColor="#ff0000">Modo inválido (1-0)</asp:RegularExpressionValidator>
                    <div class="explicacion">
                        <asp:Literal ID="lbSmtpSslModeDesc" runat="server" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        <div id="Otros" class="article no-mostrar">
            <div class="article-content article-min-height">
                &nbsp;
            </div>
        </div>
    </div>
    <div class="contenido-derecha">
        <div class="bloque">
            <div class="bloque-header">
                <h3>
                    OPCIONES</h3>
            </div>
            <div class="bloque-content">
                <div class="floatleft margin">
                    <asp:Image ID="imgIndicador" runat="server" ImageUrl="~/images/frontpage/defina.png" />
                </div>
                <div class="floatleft margin leftborder">
                    <ul class="KPIListView">
                        <li>
                            <asp:LinkButton ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" CssClass="button-gray"><span class="pictogram confirm"></span>&nbsp;&nbsp;Guardar</asp:LinkButton></li>
                    </ul>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
