<%@ Page Title="Cadastro de usuários - Terrapar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="usuarios.aspx.cs" Inherits="Terrapar.usuarios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style>
        .menu li:nth-child(6) a{
            background-color: #bfcbd6 !important;
            color: #465EC9 !important;
            text-decoration: underline;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $('[id$=txtMetaComissaoEdit]').limitkeypress({ rexp: regDecimal });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:multiView runat="server" ID="vwEstados">
        <asp:View runat="server" ID="vBuscar">
            <table>
                <tr>
                    <td align="right"><label for="txtPlaca">Placa:</label></td>
                    <td><asp:TextBox alt="placa" runat="server" ID="txtPlaca"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtNomeMotorista">Nome motorista:</label></td>
                    <td><asp:TextBox runat="server" ID="txtNomeMotorista"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtNomeCaminhao">Nome caminhão:</label></td>
                    <td><asp:TextBox runat="server" ID="txtNomeCaminhao"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="ddlTipo">Tipo:</label></td>
                    <td><asp:DropDownList runat="server" ID="ddlTipos">
                            <asp:ListItem Text="Todos" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Pão de açúcar" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Pedreira" Value="1"></asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
            </table>
            <br />
            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" />
            <asp:Button runat="server" ID="btnLimpar" Text="Limpar" OnClick="btnLimpar_Click" />
            <input type="button" value="Novo" onclick="window.location.href='usuarios.aspx?id=0';" /><br />
            <br />

            <asp:GridView runat="server" ID="grdUsuarios" AutoGenerateColumns="false" FooterStyle-CssClass="total" CssClass="borda">
                <Columns>
                    <asp:BoundField HeaderText="Motorista" DataField="nome_motorista">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Placa" DataField="placa">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Tipo">
                        <ItemTemplate>
                            <%#Convert.ToInt32(Eval("tipo")) == 0 ? "Pão de açúcar" : "Pedreira"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Meta comissão" DataField="meta_comissao">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Troca óleo" DataField="troca_oleo">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="usuarios.aspx?id=<%#Eval("id_usuario")%>">Editar</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDeletar" runat="server" CommandArgument='<%#Eval("id_usuario")%>' OnClientClick = "return confirm('Deseja realmente excluir o usuário?')" Text = "Excluir" OnClick = "lnkDeletar_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>

        <asp:View runat="server" ID="vEditarCadastrar">
            <table>
                <tr>
                    <td align="right"><label for="txtPlacaEdit">Placa:</label></td>
                    <td><asp:TextBox alt="placa" runat="server" ID="txtPlacaEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtNomeMotoristaEdit">Nome motorista:</label></td>
                    <td><asp:TextBox runat="server" ID="txtNomeMotoristaEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtNomeCaminhaoEdit">Nome caminhão:</label></td>
                    <td><asp:TextBox runat="server" ID="txtNomeCaminhaoEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtSenhaEdit">Senha:</label></td>
                    <td><asp:TextBox runat="server" ID="txtSenhaEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="ddlTiposEdit">Tipo:</label></td>
                    <td><asp:DropDownList runat="server" ID="ddlTiposEdit">
                            <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                            <asp:ListItem Text="Pão de açúcar" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Perdreira" Value="1"></asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtMetaComissaoEdit">Meta comissão: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtMetaComissaoEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtTrocaOleoEdit">Troca de óleo:</label></td>
                    <td><asp:TextBox alt="999999999" runat="server" ID="txtTrocaOleoEdit"></asp:TextBox> KM</td>
                </tr>
                <tr>
                    <td align="right"><label for="chkAdminEdit">Administrador:</label></td>
                    <td><asp:CheckBox runat="server" ID="chkAdminEdit"></asp:CheckBox></td>
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
