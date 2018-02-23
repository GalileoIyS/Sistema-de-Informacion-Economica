<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="etiquetas.aspx.cs" Inherits="administrador_etiquetas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <h2 class="color-terciario-foreground">
        Etiquetas de búsqueda</h2>
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
        A través de este panel, usted podrá realizar todas las operaciones de mantenimiento
        (bajas y modificaciones) de las diferentes etiquetas que se han ido introduciendo
        en los <span class="naranja negrita">indicadores</span> para facilitar su organización
        y posterior búsqueda.
    </p>
    <asp:UpdatePanel ID="UPCategorias" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="DataPager">
                <div class="floatleft">
                    <asp:Label ID="lbBusqueda" runat="server" Text="Búsqueda..." CssClass="no-mostrar"></asp:Label>
                    <asp:TextBox ID="txtBusqueda" placeholder="Búsqueda..." runat="server" CssClass="txtbusqueda"></asp:TextBox>
                    <asp:Button ID="btnBusqueda" runat="server" Text="SearchButton" CssClass="no-mostrar"
                        OnClick="btnFiltrar_Click" ClientIDMode="Static" />
                </div>
                <asp:DataPager ID="DataPagerEtiquetas" runat="server" PagedControlID="lstEtiquetas"
                    PageSize="15" OnPreRender="DataPagerEtiquetas_PreRender">
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
                <asp:ListView ID="lstEtiquetas" runat="server" EnableViewState="True" DataKeyNames="ETIQUETAID"
                    OnItemEditing="lstEtiquetas_ItemEditing" OnItemCommand="lstEtiquetas_CommandList"
                    OnItemUpdating="lstEtiquetas_UpdateList" OnItemDeleting="lstEtiquetas_DeleteList"
                    OnItemCanceling="lstEtiquetas_CancelList">
                    <ItemTemplate>
                        <li class="search">
                            <div class="categoria color-terciario-background">
                                <div class="categoria-valor">
                                    <asp:Label ID="lbPrimeraLetra" runat="server"><%# Eval("NOMBRE").ToString().Substring(0, 1).ToUpper() %></asp:Label>
                                </div>
                            </div>
                            <div class="floatleft">
                                <h4>
                                    <asp:Label ID="lbNombreValue" runat="server" Text='<%#Eval("NOMBRE")%>' Width="800" /></h4>
                                <div>
                                    <asp:Label ID="lbTamanoValue" runat="server" Text='<%#Eval("UPPERNOMBRE")%>' Width="800" />
                                </div>
                                <div>
                                    <span class="pictogram edit-icon"></span><span>
                                        <asp:LinkButton runat="server" ID="lnkEditEtiqueta" CommandName="Edit">editar</asp:LinkButton></span>
                                    &nbsp;&nbsp;&nbsp;&nbsp;<span class="pictogram delete-icon"> </span><span>
                                        <asp:LinkButton runat="server" ID="lnkDeleteEtiqueta" CommandName="Delete" OnClientClick="return confirm('¿Seguro que desea eliminar esta ETIQUETA?');">eliminar</asp:LinkButton></span>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                        </li>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <li class="edit-mode">
                            <div class="marginleft marginright marginbottom padding">
                                <div class="row">
                                    <asp:Label ID="Label3" runat="server" CssClass="negrita" Text="Nombre:" />
                                    <asp:TextBox runat="server" ID="txtNombreEdit" CssClass="textbox" Text='<%#DataBinder.Eval(Container.DataItem, "NOMBRE") %>' Width="400" />
                                </div>
                                <div class="row">
                                    <span>&nbsp;</span>
                                    <asp:ImageButton ID="lnkGuardar" CommandName="Save" runat="server" Text="Grabar"
                                        ImageUrl="~/images/buttons/notification_done.png"></asp:ImageButton>
                                    &nbsp;
                                    <asp:ImageButton ID="lnkCancelar" CommandName="Cancel" runat="server" Text="Cancelar"
                                        ImageUrl="~/images/buttons/notification_error.png"></asp:ImageButton>
                                </div>
                            </div>
                        </li>
                    </EditItemTemplate>
                    <EmptyDataTemplate>
                        <li class="margin">
                            <asp:Label ID="Label3" runat="server" CssClass="TituloMediano naranja_fuerte center"
                                Text="No se ha encontrado ninguna etiqueta en la base de datos" Width="700" />
                        </li>
                    </EmptyDataTemplate>
                </asp:ListView>
            </ul>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
