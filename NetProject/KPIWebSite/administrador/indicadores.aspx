<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="indicadores.aspx.cs" Inherits="administrador_indicadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <h2 class="color-terciario-foreground">
        Catálogo de indicadores</h2>
    <ul class="breadcrumb">
        <li class="first">
            <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="pictogram home-icon">
                </span>&nbsp;Inicio</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HlnkBuscador" runat="server" NavigateUrl="~/search.aspx"><span class="pictogram administrator-icon">
                </span>&nbsp;Administrador</asp:HyperLink></li>
        <li><span class="current-page"><span class="pictogram current-page-icon"></span>&nbsp;<strong>Etiquetas</strong></span></li>
    </ul>
    <p>
        A través de este panel, usted podrá gestionar todos los indicadores introducidos
        en el sistema. para ello, dispondrá de un buscador con características avanzadas
        que le permitirá realizar criterios de filtrado específicos.</p>
    <asp:UpdatePanel ID="UPKPIs" runat="server">
        <ContentTemplate>
            <div class="DataPager">
                <div class="floatleft">
                    <asp:Label ID="lbBusqueda" runat="server" Text="Búsqueda..." CssClass="no-mostrar"></asp:Label>
                    <asp:TextBox ID="txtBusqueda" placeholder="Búsqueda..." runat="server" CssClass="txtbusqueda"></asp:TextBox>
                    <asp:Button ID="btnBusqueda" runat="server" Text="SearchButton" CssClass="no-mostrar"
                        OnClick="btnFiltrar_Click" ClientIDMode="Static" />
                </div>
                <asp:DataPager ID="DataPagerKpis" runat="server" PagedControlID="lstIndicadores"
                    PageSize="15" OnPreRender="DataPagerKpis_PreRender">
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
            <ul class="KPIListView">
                <asp:ListView ID="lstIndicadores" runat="server">
                    <ItemTemplate>
                        <li class="search lidimensions ">
                            <div class="floatleft mediaaltura">
                                <asp:CheckBox ID="cbIndicador" runat="server" CssClass="art_cb" />
                                <asp:HiddenField ID="HdnIndicatorId" runat="server" Value='<%# Eval("INDICATORID") %>' />
                            </div>
                            <div class="categoria color-terciario-background">
                                <div class="categoria-valor">
                                    <%# Eval("NUM_USUARIOS")%>
                                </div>
                                <div class="categoria-desc">
                                    usuarios
                                </div>
                            </div>
                            <div class="marginleft">
                                <h3>
                                    <asp:HyperLink ID="lbTitulo" runat="server" Text='<%# Eval("TITULO") %>' NavigateUrl='<%# "~/indicator.aspx?indicatorid=" + Eval("INDICATORID") %>'></asp:HyperLink></h3>
                                <div>
                                    <span class="pictogram calendar-icon"></span>
                                    <asp:Label ID="lbFecha" runat="server" Text='<%# Eval("ULTIMA_FECHA") %>' />&nbsp;&nbsp;&nbsp;
                                </div>
                                <div>
                                    <asp:Label ID="lbResumen" runat="server" Text='<%# Eval("RESUMEN") %>' />
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:ListView>
            </ul>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
