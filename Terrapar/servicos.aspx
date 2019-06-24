<%@ Page Title="Servicos - Terrapar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="servicos.aspx.cs" Inherits="Terrapar.servicos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style>
        .menu li:nth-child(3) a{
            background-color: #bfcbd6 !important;
            color: #465EC9 !important;
            text-decoration: underline;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $('[id$=txtValorEdit]').limitkeypress({ rexp: regDecimal });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:multiView runat="server" ID="vwEstados">
        <asp:View runat="server" ID="vBuscar">

            <table>
                <tr>
                    <td align="right"><label for="ddlUsuarios">Placa:</label></td>
                    <td><asp:DropDownList runat="server" ID="ddlUsuarios" DataTextField="placa" DataValueField="id_usuario"
                AutoPostBack="true" OnSelectedIndexChanged="ddlUsuarios_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="right"><label for="ddlPeriodo">Período:</label></td>
                    <td><asp:DropDownList runat="server" ID="ddlPeriodo"></asp:DropDownList></td>
                </tr>
            </table>
            <br />
            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" />
            <input type="button" value="Novo" onclick="window.location.href='servicos.aspx?id=0';" /><br />
            <br />
            <asp:GridView runat="server" ID="grdServicos" AutoGenerateColumns="false" FooterStyle-CssClass="total" CssClass="borda">
                <Columns>
                    <asp:TemplateField HeaderText="Data">
                        <ItemTemplate>
                            <%#Convert.ToDateTime(Eval("data")).ToString("dd/MM/yyyy")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Descrição" DataField="descricao">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Valor" DataField="valor">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Km" DataField="km">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Tipo">
                        <ItemTemplate>
                            <%#Convert.ToInt32(Eval("tipo")) == 0 ? "Manutenção" : "Despesa"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="servicos.aspx?id=<%#Eval("id_servico")%>">Editar</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDeletar" runat="server" CommandArgument='<%#Eval("id_servico")%>' OnClientClick = "return confirm('Deseja realmente excluir o serviço?')" Text = "Excluir" OnClick = "lnkDeletar_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>

        <asp:View runat="server" ID="vEditarCadastrar">
            <table>
                <tr>
                    <td align="right"><label for="txtDataEdit">Data:</label></td>
                    <td><asp:TextBox alt="date" runat="server" ID="txtDataEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="ddlUsuariosEdit">Placa:</label></td>
                    <td><asp:DropDownList runat="server" ID="ddlUsuariosEdit" DataTextField="placa" DataValueField="id_usuario"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtDescricaoEdit">Descrição:</label></td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDescricaoEdit"></asp:TextBox>
                        <asp:CheckBox runat="server" ID="chkOleo" Text="Troca de óleo" AutoPostBack="true" OnCheckedChanged="chkOleo_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorEdit">Valor: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtKmEdit">Km:</label></td>
                    <td><asp:TextBox alt="999999999" runat="server" ID="txtKmEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="ddlTiposEdit">Tipo:</label></td>
                    <td><asp:DropDownList runat="server" ID="ddlTiposEdit">
                            <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                            <asp:ListItem Text="Manutenção" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Despesa" Value="1"></asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
            </table>
            <br />
            <asp:Button runat="server" ID="btnEditar" Text="Salvar" OnClick="btnEditar_Click" />
            <input type="button" value="Voltar" onclick="window.history.back();" /><br />
        </asp:View>

    </asp:multiView>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyEndContent" runat="server">
</asp:Content>
