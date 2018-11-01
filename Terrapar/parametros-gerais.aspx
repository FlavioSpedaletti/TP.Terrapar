<%@ Page Title="Cadastro de parâmetros gerais - Terrapar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="parametros-gerais.aspx.cs" Inherits="Terrapar.parametros_gerais" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style>
        .menu li:nth-child(8) a{
            background-color: #bfcbd6 !important;
            color: #465EC9 !important;
            text-decoration: underline;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $('[id$=txtValorRefeicaoEdit]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtValorValeEdit]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtSalarioPaoAcucarEdit]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtValorHoraExtraPaoAcucarEdit]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtValorDiariaPaoAcucarEdit]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtInssPaoAcucarEdit]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtSalarioPedreiraEdit]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtInssPedreiraEdit]').limitkeypress({ rexp: regDecimal });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:multiView runat="server" ID="vwEstados">
        <asp:View runat="server" ID="vBuscar">
            <asp:GridView runat="server" ID="grdParametrosGerais" AutoGenerateColumns="false" FooterStyle-CssClass="total" CssClass="borda">
                <Columns>
                    <asp:BoundField HeaderText="Valor refeição" DataField="valor_refeicao">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Valor vale" DataField="valor_vale">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Salário Pão de Açúcar" DataField="salario_pao_acucar">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Valor hora extra Pão de Açúcar" DataField="valor_hora_extra_pao_acucar">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Valor diária Pão de Açúcar" DataField="valor_diaria_pao_acucar">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="INSS Pão de Açúcar" DataField="inss_pao_acucar">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Salário Pedreira" DataField="salario_pedreira">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="INSS Pedreira" DataField="inss_pedreira">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="parametros-gerais.aspx?id=<%#Eval("id_parametro")%>">Editar</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>

        <asp:View runat="server" ID="vEditarCadastrar">

            <table>
                <tr>
                    <td align="right"><label for="txtValorRefeicaoEdit">Valor refeição: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorRefeicaoEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorValeEdit">Valor vale: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorValeEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtSalarioPaoAcucarEdit">Salário Pão de Açúcar: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtSalarioPaoAcucarEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorHoraExtraPaoAcucarEdit">Valor hora extra Pão de Açúcar: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorHoraExtraPaoAcucarEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorDiariaPaoAcucarEdit">Valor diária Pão de Açúcar: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorDiariaPaoAcucarEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtInssPaoAcucarEdit">INSS Pão de Açúcar:</label></td>
                    <td><asp:TextBox runat="server" ID="txtInssPaoAcucarEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtSalarioPedreiraEdit">Salário Pedreira: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtSalarioPedreiraEdit"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtInssPedreiraEdit">INSS Pedreira:</label></td>
                    <td><asp:TextBox runat="server" ID="txtInssPedreiraEdit"></asp:TextBox></td>
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
