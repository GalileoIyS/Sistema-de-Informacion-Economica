<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="categories.aspx.cs" Inherits="administrador_categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/categorias.js") %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <h2>
        <span class="color-terciario-foreground">CATEGORÍAS</span></h2>
    <ul class="breadcrumb">
        <li class="first">
            <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="pictogram home-icon">
                </span>&nbsp;Inicio</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HlnkBuscador" runat="server" NavigateUrl="~/search.aspx"><span class="pictogram administrator-icon">
                </span>&nbsp;Administrador</asp:HyperLink></li>
        <li><span class="current-page"><span class="pictogram current-page-icon"></span>&nbsp;<strong>Categorías</strong></span></li>
    </ul>
    <p>
        A través de este panel, usted podrá realizar todas las operaciones de mantenimiento
        (altas, bajas y modificaciones) de las diversas categorías en las que se pueden
        organizar los diferentes indicadores. Tenga en cuenta que para que una categoría
        pueda ser eliminada de la base de datos, no puede existir ningún indicador asociado
        a la misma o alguna de las subcategorías dependientes.
    </p>
    <div class="row">
        <span>&nbsp;</span>
        <asp:TextBox runat="server" ID="txtCategoria" placeholder=" -Introduzca el nombre de la nueva categoría- "
            MaxLength="200" Width="400px" CssClass="txtTextbox" ClientIDMode="Static"></asp:TextBox>&nbsp;&nbsp;
        <div class="mensaje-error">
            Por favor, debe introducir el nombre de la nueva categoría
        </div>
        <asp:LinkButton ID="btnCrearCategoria" runat="server" Text="Nueva categoría" OnClick="btnCrearCategoria_Click" OnClientClick="return InsertaCategoria();" CssClass="button color-terciario-background" />
    </div>
    <asp:UpdatePanel ID="UPCategorias" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="DataPager">
                <div class="floatleft">
                    <asp:Label ID="lbBusqueda" runat="server" Text="Búsqueda..." CssClass="no-mostrar"></asp:Label>
                    <asp:TextBox ID="txtBusqueda" placeholder="Búsqueda..." runat="server" CssClass="txtbusqueda" ClientIDMode="Static" ></asp:TextBox>
                    <asp:Button ID="btnBusqueda" runat="server" Text="SearchButton" CssClass="no-mostrar" OnClick="btnFiltrar_Click" ClientIDMode="Static" />
                </div>
                <asp:DataPager ID="DataPagerCategorias" runat="server" PagedControlID="lstCategorias"
                    PageSize="15" OnPreRender="DataPagerCategorias_PreRender">
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
                <asp:ListView ID="lstCategorias" runat="server" EnableViewState="True" DataKeyNames="CATEGORYID"
                    OnItemEditing="lstCategorias_ItemEditing" OnItemCommand="lstCategorias_CommandList"
                    OnItemUpdating="lstCategorias_UpdateList" OnItemDeleting="lstCategorias_DeleteList"
                    OnItemCanceling="lstCategorias_CancelList">
                    <ItemTemplate>
                        <li class="search">
                            <div class="floatleft">
                                <div class="categoria" style='background-color: <%# Eval("ESTILO") %>;'>
                                    <div class="categoria-valor">
                                        <%# Eval("NOMBRE").ToString().Substring(0, 1).ToUpper() %>
                                    </div>
                                </div>
                            </div>
                            <h4>
                                <asp:Label ID="lbNombreValue" runat="server" Text='<%#Eval("NOMBRE")%>' /></h4>
                            <div>
                                <asp:Label ID="lbDescripcionValue" runat="server" Text='<%#Eval("DESCRIPCION")%>' />
                            </div>
                            <div>
                                <span class="pictogram edit-icon"></span><span>
                                    <asp:LinkButton runat="server" ID="lnkEditRol" CommandName="Edit">Editar</asp:LinkButton></span>
                                &nbsp;&nbsp;&nbsp;&nbsp;<span class="pictogram delete-icon"> </span><span>
                                    <asp:LinkButton runat="server" ID="lnkDeleteRol" CommandName="Delete" OnClientClick="return confirm('¿Seguro que desea eliminar esta Categoría?');">Eliminar</asp:LinkButton></span>
                            </div>
                        </li>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <li class="edit-mode">
                            <div class="marginleft marginright marginbottom padding">
                                <div class="row">
                                    <asp:Label ID="Label3" runat="server" CssClass="negrita" Text="Nombre:" />
                                    <asp:TextBox runat="server" ID="txtNombreEdit" CssClass="textbox" Text='<%#DataBinder.Eval(Container.DataItem, "NOMBRE") %>'
                                        Width="400" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="Label1" runat="server" CssClass="negrita" Text="Descripción:" />
                                    <asp:TextBox runat="server" ID="txtDescripcionEdit" CssClass="textbox" Text='<%#DataBinder.Eval(Container.DataItem, "DESCRIPCION") %>'
                                        Width="400" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="Label2" runat="server" CssClass="negrita" Text="Estilo:" />
                                    <asp:TextBox runat="server" ID="txtEstiloEdit" CssClass="textbox" Text='<%#DataBinder.Eval(Container.DataItem, "ESTILO") %>'
                                        Width="200" />
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
                        <li class="margin"><span class="TituloMediano naranja_fuerte center">No se ha encontrado
                            ninguna categoría en la base de datos</span> </li>
                    </EmptyDataTemplate>
                </asp:ListView>
            </ul>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCrearCategoria" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
