<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master" AutoEventWireup="true"
    CodeFile="roles.aspx.cs" Inherits="administrador_roles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="Server">
    <script type="text/javascript">
        function ValidaNuevoRol() {
            var inputNombre = $('#txtRoleName');
            if (inputNombre.val()) {
                inputNombre.removeClass('with-error');
            }
            else {
                inputNombre.addClass('with-error');
                inputNombre.next('.mensaje-error').fadeIn().delay(2000).fadeOut();
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="CenterContent" ContentPlaceHolderID="OneColumnPlaceHolder" runat="Server">
    <h2 class="color-terciario-foreground">
        Listado de Roles</h2>
    <ul class="breadcrumb">
        <li class="first">
            <asp:HyperLink ID="HlnkInicio" runat="server" NavigateUrl="~/Default.aspx"><span class="pictogram home-icon">
                </span>&nbsp;Inicio</asp:HyperLink></li>
        <li>
            <asp:HyperLink ID="HlnkBuscador" runat="server" NavigateUrl="~/administrador/admin.aspx"><span class="pictogram administrator-icon">
                </span>&nbsp;Administración</asp:HyperLink></li>
        <li><span class="current-page"><span class="pictogram current-page-icon"></span>&nbsp;<strong>Roles</strong></span></li>
    </ul>
    <p>
        A través de este panel, usted podrá realizar todas las operaciones de mantenimiento
        (altas, bajas y modificaciones) de los diversos roles definidos en el Portal. Se
        ruega encarecidamente no se intente eliminar el rol de <span class="naranja negrita">
            administrador</span> ya que se trata de un rol de sistema, necesario para el
        buen comportamiento de la web.</p>
    <div class="row">
        <span>&nbsp;</span>
        <asp:TextBox runat="server" ID="txtRoleName" placeholder=" -Introduzca el nombre del nuevo rol- " MaxLength="200" Width="400px" CssClass="txtTextbox" ClientIDMode="Static"></asp:TextBox>&nbsp;&nbsp;
            <div class="mensaje-error">
            Por favor, debe introducir el nombre del nuevo rol
        </div>
        <asp:LinkButton ID="btnAddRole" runat="server" Text="Nuevo Rol" OnClick="btnAddRole_Click" OnClientClick="return ValidaNuevoRol();" CssClass="button color-terciario-background" />
    </div>
    <asp:UpdatePanel ID="UPRoles" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="DataPager">
                <div class="floatleft">
                    <asp:Label ID="lbBusqueda" runat="server" Text="Búsqueda..." CssClass="no-mostrar"></asp:Label>
                    <asp:TextBox ID="txtBusqueda" placeholder="Búsqueda..." runat="server" CssClass="txtbusqueda" ClientIDMode="Static" ></asp:TextBox>
                    <asp:Button ID="btnBusqueda" runat="server" Text="SearchButton" CssClass="no-mostrar" OnClick="btnFiltrar_Click" ClientIDMode="Static" />
                </div>
                <asp:DataPager ID="DataPagerRoles" runat="server" PagedControlID="lstRoles" PageSize="15"
                    OnPreRender="DataPagerRoles_PreRender">
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
                <asp:ListView ID="lstRoles" runat="server" EnableViewState="True" DataKeyNames="ROLEID"
                    OnItemEditing="lstRoles_ItemEditing" OnItemCommand="lstRoles_CommandList" OnItemUpdating="lstRoles_UpdateList"
                    OnItemDeleting="lstRoles_DeleteList" OnItemCanceling="lstRoles_CancelList">
                    <ItemTemplate>
                        <li class="search">
                            <div class="floatleft">
                                <div class="categoria color-terciario-background">
                                    <div class="categoria-valor">
                                        R
                                    </div>
                                </div>
                            </div>
                            <h4>
                                <asp:Label ID="lbNombreValue" runat="server" Text='<%#Eval("ROLENAME")%>' /></h4>
                            <div>
                                <asp:Label ID="lbDescripcionValue" runat="server" Text='<%#Eval("DESCRIPTION")%>' />
                            </div>
                            <div>
                                <span class="pictogram edit-icon"></span><span>
                                    <asp:LinkButton runat="server" ID="lnkEditRol" CommandName="Edit">Editar</asp:LinkButton></span>
                                &nbsp;&nbsp;&nbsp;&nbsp;<span class="pictogram delete-icon"> </span><span>
                                    <asp:LinkButton runat="server" ID="lnkDeleteRol" CommandName="Delete" OnClientClick="return confirm('¿Seguro que desea eliminar este ROL?');">Eliminar</asp:LinkButton></span>
                            </div>
                        </li>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <li class="edit-mode">
                            <div class="marginleft marginright marginbottom padding">
                                <div class="row">
                                    <asp:Label ID="Label3" runat="server" CssClass="negrita" Text="Nombre:" />
                                    <asp:TextBox runat="server" ID="txtNombreEdit" CssClass="textbox" Text='<%#DataBinder.Eval(Container.DataItem, "ROLENAME") %>' Width="400" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="Label4" runat="server" CssClass="negrita" Text="Descripción:" />
                                    <asp:TextBox runat="server" ID="txtDescripcionEdit" CssClass="textbox" Height="80" Width="400"
                                        TextMode="MultiLine" MaxLength="250" Text='<%#DataBinder.Eval(Container.DataItem, "DESCRIPTION") %>' />
                                </div>
                                <div class="row">
                                    <span>&nbsp;</span>
                                    <asp:ImageButton ID="lnkGuardar" CommandName="Save" runat="server" Text="Grabar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ROLEID") %>'
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
                            ningún rol en la base de datos</span> </li>
                    </EmptyDataTemplate>
                </asp:ListView>
            </ul>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAddRole" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
