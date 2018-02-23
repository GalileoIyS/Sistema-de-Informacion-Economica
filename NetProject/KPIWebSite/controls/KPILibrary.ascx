<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KPILibrary.ascx.cs" Inherits="controls_KPILibrary" %>
<div id="PanelMiLibreria" runat="server" class="panel panel-default">
    <div class="panel-heading">
        <h3 class="panel-title"><span class="glyphicon glyphicon-th-list"></span>&nbsp;MY LIBRARY</h3>
    </div>
    <div class="dropkeys-shortlist-container">
        <ul class="dropkeys-shortlist">
            <asp:ListView ID="lstIndicadores" runat="server" DataKeyNames="INDICATORID">
                <ItemTemplate>
                    <li class="pull">
                        <asp:HyperLink ID="HlnkMiIndicador" runat="server" NavigateUrl='<%# "~/indicator.aspx?indicatorid=" + Eval("INDICATORID") %>'>
                    <asp:Image runat="server" ImageUrl='<%# Eval("IMAGEURL")%>' /><%# Eval("TITULO") %><span class="badge pull-right" title="Number of data values" style='background: <%# Eval("ESTILO") %>;'><%# Eval("RECUENTO") %></span>
                        </asp:HyperLink>
                    </li>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div class="panel-body">
                        <h3>ADD ALL THE INDICATORS YOU WANT INTO YOUR LIBRARY!</h3>
                        <p>Your library is empty</p>
                    </div>
                </EmptyDataTemplate>
            </asp:ListView>
        </ul>
    </div>
    <div id="PanelMas" runat="server" class="panel-footer text-center">
        <a href="../registrado/profile.aspx" title="My indicators" class="btn btn-dropkeys">View more</a>
    </div>
</div>
