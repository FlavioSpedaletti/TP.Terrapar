<%@ Page Title="Cadastro de lojas - Terrapar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="lojas.aspx.cs" Inherits="Terrapar.lojas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style>
        .menu li:nth-child(7) a{
            background-color: #bfcbd6 !important;
            color: #465EC9 !important;
            text-decoration: underline;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:multiView runat="server" ID="vwEstados">
        <asp:View runat="server" ID="vBuscar">
            <table>
                <tr>
                    <td align="right"><label for="txtIdentificacao">Identificação:</label></td>
                    <td><asp:TextBox runat="server" ID="txtIdentificacao"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtNome">Nome:</label></td>
                    <td><asp:TextBox runat="server" ID="txtNome"></asp:TextBox></td>
                </tr>
            </table>
            <br />
            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" />
            <asp:Button runat="server" ID="btnLimpar" Text="Limpar" OnClick="btnLimpar_Click" />
            <input type="button" value="Novo" onclick="window.location.href='lojas.aspx?id=0';" /><br />
            <br />

            <asp:GridView runat="server" ID="grdLojas" AutoGenerateColumns="false" FooterStyle-CssClass="total" CssClass="borda">
                <Columns>
                    <asp:BoundField HeaderText="Identificação" DataField="identificacao">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Nome" DataField="nome">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="lojas.aspx?id=<%#Eval("id_loja")%>">Editar</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDeletar" runat="server" CommandArgument='<%#Eval("id_loja")%>' OnClientClick = "return confirm('Deseja realmente excluir a loja?')" Text = "Excluir" OnClick = "lnkDeletar_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>

        <asp:View runat="server" ID="vEditarCadastrar">

            <table>
                <tr>
                    <td align="right"><label for="txtIdentificacaoEdit">Identificação:</label></td>
                    <td><asp:TextBox runat="server" ID="txtIdentificacaoEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtNome">Nome:</label></td>
                    <td><asp:TextBox runat="server" ID="txtNomeEdit"></asp:TextBox></td>
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
