<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="avisos.aspx.cs" Inherits="administrador_avisos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <h2 class="color-terciario-foreground">
        Avisos, incidencias y errores</h2>
    <ul class="breadcrumb">
        <li class="first">
            <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="pictogram home-icon">
                </span>&nbsp;Inicio</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HlnkBuscador" runat="server" NavigateUrl="~/search.aspx"><span class="pictogram administrator-icon">
                </span>&nbsp;Administrador</asp:HyperLink></li>
        <li><span class="current-page"><span class="pictogram current-page-icon"></span>&nbsp;<strong>Avisos</strong></span></li>
    </ul>
    <p>
        A través de este panel, usted podrá visualizar las diferentes incidencias que se
        han ido registrando en la base de datos.
    </p>
    <asp:UpdatePanel ID="UPAvisos" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="DataPager">
                <div class="floatleft">
                    <asp:Label ID="lbBusqueda" runat="server" Text="Búsqueda..." CssClass="no-mostrar"></asp:Label>
                    <asp:TextBox ID="txtBusqueda" placeholder="Búsqueda..." runat="server" CssClass="txtbusqueda"></asp:TextBox>
                    <asp:Button ID="btnBusqueda" runat="server" Text="SearchButton" CssClass="no-mostrar"
                        OnClick="btnFiltrar_Click" ClientIDMode="Static" />
                </div>
                <asp:DataPager ID="DataPagerAvisos" runat="server" PagedControlID="lstAvisos" PageSize="15"
                    OnPreRender="DataPagerAvisos_PreRender">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Image" ShowFirstPageButton="false" ShowPreviousPageButton="true"
                            PreviousPageImageUrl="~/images/buttons/leftarrow.png" PreviousPageText="Anterior"
                            ShowLastPageButton="false" ShowNextPageButton="false" />
                        <asp:NumericPagerField CurrentPageLabelCssClass="current" ButtonCount="10" NextPageText=">"
                            PreviousPageText="<" RenderNonBreakingSpacesBetweenControls="false" />
                        <asp:NextPreviousPagerField ButtonType="Image" ShowNextPageButton="true" NextPageImageUrl="~/images/buttons/rightarrow.png"
                            NextPageText="Siguiente" ShowPreviousPageButton="false" />
                    </Fields>
                </asp:DataPager>
                <div class="clear">
                </div>
            </div>
            <div class="row">
                Acciones &nbsp;&nbsp;
                <asp:DropDownList ID="dplstAcciones" runat="server" CssClass="textbox">
                    <asp:ListItem Text="Eliminar todos los avisos&nbsp;" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Eliminar todos los incidencias&nbsp;" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Eliminar todos los errores&nbsp;" Value="3"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnAccion" runat="server" Text="Ejecutar" CssClass="button" OnClick="btnAccion_Click" />
            </div>
            <ul class="KPIListView">
                <asp:ListView ID="lstAvisos" runat="server" EnableViewState="True">
                    <ItemTemplate>
                        <li class="search">
                            <div class="categoria color-terciario-background">
                                <div class="categoria-valor">
                                    <asp:Label ID="lbPrimeraLetra" runat="server"><%# Eval("ERROR") %></asp:Label>
                                </div>
                            </div>
                            <div>
                                <h4>
                                    <asp:Label ID="lbNombreValue" runat="server" Text='<%#Eval("PAGINA")%>' /></h4>
                                <div>
                                    <span class="pictogram calendar"></span>
                                    <%# CalculaFechaDesdeCuando(Eval("FECHA").ToString())%>
                                </div>
                                <div>
                                    <asp:Label ID="lbMensaje" runat="server" Text='<%#Eval("MENSAJE")%>' Width="800" />
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <li class="margin">
                            <asp:Label ID="Label3" runat="server" CssClass="TituloMediano naranja_fuerte center"
                                Text="No se ha encontrado ningún aviso, incidencia o error en la base de datos" />
                        </li>
                    </EmptyDataTemplate>
                </asp:ListView>
            </ul>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
