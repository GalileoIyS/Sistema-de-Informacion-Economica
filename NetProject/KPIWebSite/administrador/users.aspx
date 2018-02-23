<%@ Page Language="C#" MasterPageFile="~/masterpages/NormalPage.master" AutoEventWireup="true"
    CodeFile="users.aspx.cs" Inherits="administrador_users" Title="Página sin título" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("input").bind("keydown", function (event) {
                // track enter key
                var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
                if (keycode == 13) { // keycode for enter key
                    // force the 'Enter Key' to implicitly click the Update button
                    document.getElementById('btnBusqueda').click();
                    return false;
                } else {
                    return true;
                }
            })
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <h2 class="color-terciario-foreground">
        Listado de usuarios</h2>
    <ul class="breadcrumb">
        <li class="first">
            <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="pictogram home-icon">
                </span>&nbsp;Inicio</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HlnkBuscador" runat="server" NavigateUrl="~/administrador/admin.aspx"><span class="pictogram administrator-icon">
                </span>&nbsp;Administración</asp:HyperLink></li>
        <li><span class="current-page"><span class="pictogram current-page-icon"></span>&nbsp;<strong>Listado de usuarios</strong></span></li>
    </ul>
    <p>
        A través de este panel, usted podrá administrar la cuenta web de los usuarios registrados
        en el sistema.</p>
    <p>
        A continuación se muestra un listado con los usuarios registrados en el portal.
        Para añadir uno nuevo, deberá salir de la sesión y registrarlo manualmente desde
        la opción del menú secundario <span class="naranja negrita">registro</span>. A continuación,
        introduzca su nombre en la casilla de texto siguiente y pulse sobre el botón de
        crear.</p>
    <asp:UpdatePanel ID="UPUsuarios" runat="server">
        <ContentTemplate>
            <div class="DataPager">
                <div class="floatleft">
                    <asp:Label ID="lbBusqueda" runat="server" Text="Búsqueda..." CssClass="no-mostrar"></asp:Label>
                    <asp:TextBox ID="txtBusqueda" placeholder="Búsqueda..." runat="server" CssClass="txtbusqueda"></asp:TextBox>
                    <asp:Button ID="btnBusqueda" runat="server" Text="SearchButton" CssClass="no-mostrar" OnClick="btnFiltrar_Click" ClientIDMode="Static" />
                </div>
                <asp:DataPager ID="DataPagerUsuarios" runat="server" PagedControlID="lstUsuarios"
                    PageSize="10" OnPreRender="DataPagerUsuarios_PreRender">
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
                <asp:ListView ID="lstUsuarios" runat="server" EnableViewState="True" DataKeyNames="USERNAME" OnItemDeleting="lstUsuarios_DeleteList">
                    <ItemTemplate>
                        <li class="search">
                            <div class="floatleft marginright">
                                <asp:Image ID="imgUsuario" runat="server" Width="70" ImageUrl='<%# CalculaImagenPerfil(Convert.ToInt32(Eval("ProviderUserKey")))%>'
                                    CssClass="border-rounded" />
                            </div>
                            <h4>
                                <asp:Label ID="lbNombreValue" runat="server" Text='<%#Eval("USERNAME")%>' /></h4>
                            <div>
                                <span title="Fecha de alta : <%#Eval("CreationDate")%>"><span class="pictogram calendar-icon">
                                </span>&nbsp;<%# CalculaFechaDesdeCuando(Convert.ToString(Eval("CreationDate")))%></span>&nbsp;&nbsp;&nbsp;&nbsp;
                                <span title="Último acceso : <%#Eval("LastLoginDate")%>"><span class="pictogram calendar-icon">
                                </span>&nbsp;<%# CalculaFechaDesdeCuando(Convert.ToString(Eval("LastLoginDate")))%></span>&nbsp;&nbsp;&nbsp;&nbsp;
                                <span title="¿Cuenta activa?"><span class="pictogram confirm-icon"></span>&nbsp;<asp:Label
                                    ID="lbIsApproved" runat="server" Text='<%#IsYesNo(Eval("IsApproved"))%>' /></span>&nbsp;&nbsp;&nbsp;&nbsp;
                                <span title="¿Está bloqueado?"><span class="pictogram locked-icon"></span>&nbsp;<asp:Label
                                    ID="lbIsLockedOut" runat="server" Text='<%#IsYesNo(Eval("IsLockedOut"))%>' /></span>&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                            <div>
                                <span class="pictogram edit-icon"></span><span>
                                    <asp:LinkButton runat="server" ID="lnkEditLista" PostBackUrl='<%# "~/registrado/profile.aspx?userid=" + Eval("PROVIDERUSERKEY") %>'>Editar</asp:LinkButton></span>
                                &nbsp;&nbsp;&nbsp;&nbsp;<span class="pictogram delete-icon"> </span><span>
                                    <asp:LinkButton runat="server" ID="lnkDeleteLista" CommandName="Delete" OnClientClick="return confirm('¿Seguro que desea eliminar este Usuario?');">Eliminar</asp:LinkButton></span>
                            </div>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <li class="margin">
                            <asp:Label ID="Label3" runat="server" CssClass="TituloMediano naranja_fuerte center"
                                Text="No se ha encontrado ningún usuario en la base de datos" />
                        </li>
                    </EmptyDataTemplate>
                </asp:ListView>
            </ul>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
