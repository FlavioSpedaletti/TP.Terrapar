<%@ Page Title="Folha de pagamento - Terrapar" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="folha-pagamento.aspx.cs" Inherits="Terrapar.folha_pagamento" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HeadContent">

    <style>
        .menu li:nth-child(4) a{
            background-color: #bfcbd6 !important;
            color: #465EC9 !important;
            text-decoration: underline;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            //fechamento pão de açúcar
            $('[id$=txtQtdHorasExtras]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtQtdDiarias]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtValorHoraExtra]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtValorDiaria]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtSalarioPaoAcucar]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtValorValePaoAcucar]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtINSSPaoAcucar]').limitkeypress({ rexp: regDecimal });

            //fechamento pedreira
            $('[id$=txtValorRefeicao]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtSalarioPedreira]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtValorValePedreira]').limitkeypress({ rexp: regDecimal });
            $('[id$=txtINSSPedreira]').limitkeypress({ rexp: regDecimal });

            //Débitos e créditos
            $('[id*=txtValor]').limitkeypress({ rexp: regDecimal });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">

    <table>
        <tr>
            <td align="right" style="width:65px;"><label for="ddlUsuarios">Placa:</label></td>
            <td><asp:DropDownList runat="server" ID="ddlUsuarios" DataTextField="placa" DataValueField="id_usuario"
                AutoPostBack="true" OnSelectedIndexChanged="ddlUsuarios_SelectedIndexChanged"></asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right"><label for="ddlPeriodo">Período:</label></td>
            <td><asp:DropDownList runat="server" ID="ddlPeriodo" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelecteIndexChanged"></asp:DropDownList></td>
        </tr>
    </table>
    <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" />
    <br /><br />
    <asp:Panel runat="server" ID="pnlFolha" Visible="false">
        <hr />
        <strong>Parâmetros para o fechamento:</strong><br /><br />
        <asp:Panel runat="server" ID="pnlFechamentoPaoAcucar" Visible="false">
            <table>
                <tr>
                    <td align="right" style="width:65px;"><label for="txtQtdHorasExtras">Qtd. horas extras:</label></td>
                    <td><asp:TextBox runat="server" ID="txtQtdHorasExtras"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtQtdDiarias">Qtd. diarias:</label></td>
                    <td><asp:TextBox runat="server" ID="txtQtdDiarias"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorHoraExtra">Valor hora extra:</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorHoraExtra"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorDiaria">Valor diária:</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorDiaria"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtSalarioPaoAcucar">Salário:</label></td>
                    <td><asp:TextBox runat="server" ID="txtSalarioPaoAcucar"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorValePaoAcucar">Valor vale:</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorValePaoAcucar"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtINSSPaoAcucar">INSS:</label></td>
                    <td><asp:TextBox runat="server" ID="txtINSSPaoAcucar"></asp:TextBox></td>
                </tr>
            </table>
            <br />
            <asp:Button runat="server" ID="btnAtualizar" Text="Salvar" OnClick="btnAtualizar_Click" /><br /><br />
            <hr />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlFechamentoPedreira" Visible="false">
            <table>
                <tr>
                    <td align="right" style="width:65px;"><label for="txtValorRefeicao">Valor refeição:</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorRefeicao"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtSalarioPedreira">Salário:</label></td>
                    <td><asp:TextBox runat="server" ID="txtSalarioPedreira"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorValePedreira">Valor vale:</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorValePedreira"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtINSSPedreira">INSS:</label></td>
                    <td><asp:TextBox runat="server" ID="txtINSSPedreira"></asp:TextBox></td>
                </tr>
            </table>
            <br />
            <asp:Button runat="server" ID="btnAtualizar2" Text="Atualizar" OnClick="btnAtualizar_Click" /><br /><br />
            <hr />
        </asp:Panel>
        <strong>Débitos e créditos:</strong><br /><br />
        <asp:Button runat="server" ID="btnAddDebitoCredito" Text="Adicionar" OnClick="btnAddDebitoCredito_Click" /><br /><br />
        <asp:GridView runat="server" ID="grdDebitosCreditos" DataKeyNames="id_debito_credito" AutoGenerateColumns="false"
        OnRowCancelingEdit="grdDebitosCreditos_RowCancelingEdit" OnRowDeleting="grdDebitosCreditos_RowDeleting" OnRowEditing="grdDebitosCreditos_RowEditing"
        OnRowUpdating="grdDebitosCreditos_RowUpdating" OnRowDataBound="grdDebitosCreditos_RowDataBound" FooterStyle-CssClass="total" CssClass="borda">
            <Columns>
                <asp:CommandField HeaderText="Editar" ShowEditButton="True" InsertText="Ok" CancelText="<br/>Cancelar" />
                <asp:TemplateField HeaderText="Tipo">
                    <EditItemTemplate>
                        <asp:DropDownList Width="80" runat="server" ID="ddlTipos">
                            <asp:ListItem Text="Débito" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Crédito" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblTipo" runat="server" Text='<%#Eval("tipo")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Descrição">
                    <EditItemTemplate>
                        <asp:TextBox Width="125" ID="txtDescricao" runat="server" Text='<%#Eval("descricao")%>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDescricao" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor">
                    <EditItemTemplate>
                        <asp:TextBox Width="50" ID="txtValor" runat="server" Text='<%#Eval("valor")%>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblValor" runat="server" Text='<%#Eval("valor")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField HeaderText="Deletar" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        <br />
        <hr />
        <br />

        <asp:Label ID="lblMotorista" runat="server" Font-Bold="true"></asp:Label>
        <br /><br />
        <asp:Panel ID="pnlFolhaPaoAcucar" runat="server" Visible="false">
            <table class="folha borda">
                <thead>
                    <tr>
                        <th>Descrição</th>
                        <th>Valor</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="credito-titulo">
                        <td align="center" colspan="2">Créditos</td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Salário:</td>
                        <td>R$ <asp:Label runat="server" ID="lblSalarioPaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Horas extras:</td>
                        <td>R$ <asp:Label runat="server" ID="lblHorasExtrasPaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Diárias:</td>
                        <td>R$ <asp:Label runat="server" ID="lblDiariasPaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Domingos:</td>
                        <td>R$ <asp:Label runat="server" ID="lblDomingosPaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Comissão 10%:</td>
                        <td>R$ <asp:Label runat="server" ID="lblComissao10PaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Outros:</td>
                        <td>R$ <asp:Label runat="server" ID="lblOutrosCreditosPaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="total">
                        <td align="right">Total 1:</td>
                        <td>R$ <asp:Label runat="server" ID="lblTotal1PaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="debito-titulo">
                        <td align="center" colspan="2">Débitos</td>
                    </tr>
                    <tr class="debito">
                        <td align="right">Vale:</td>
                        <td>R$ <asp:Label runat="server" ID="lblValePaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="debito">
                        <td align="right">INSS:</td>
                        <td>R$ <asp:Label runat="server" ID="lblINSSPaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="debito">
                        <td align="right">Outros:</td>
                        <td>R$ <asp:Label runat="server" ID="lblOutrosDebitosPaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="total">
                        <td align="right">Total 2:</td>
                        <td>R$ <asp:Label runat="server" ID="lblTotal2PaoAcucar"></asp:Label></td>
                    </tr>
                    <tr class="total total-geral">
                        <td align="right">Total geral:</td>
                        <td>R$ <asp:Label runat="server" ID="lblTotalGeralPaoAcucar"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlFolhaPedreira" runat="server" Visible="false">
            <table class="folha borda">
                <thead>
                    <tr>
                        <th>Descrição</th>
                        <th>Valor</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="credito-titulo">
                        <td align="center" colspan="2">Créditos</td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Salário:</td>
                        <td>R$ <asp:Label runat="server" ID="lblSalarioPedreira"></asp:Label></td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Comissão <span class="info" title="7% para viagens durante o dia e 15% para viagens durante a noite, domingos ou feriados">i</span>:</td>
                        <td>R$ <asp:Label runat="server" ID="lblComissaoPedreira"></asp:Label></td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Refeição:</td>
                        <td>R$ <asp:Label runat="server" ID="lblRefeicaoPedreira"></asp:Label></td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Comissão 5%:</td>
                        <td>R$ <asp:Label runat="server" ID="lblComissao5Pedreira"></asp:Label></td>
                    </tr>
                    <tr class="credito">
                        <td align="right">Outros:</td>
                        <td>R$ <asp:Label runat="server" ID="lblOutrosCreditosPedreira"></asp:Label></td>
                    </tr>
                    <tr class="total">
                        <td align="right">Total 1:</td>
                        <td>R$ <asp:Label runat="server" ID="lblTotal1Pedreira"></asp:Label></td>
                    </tr>
                    <tr class="debito-titulo">
                        <td align="center" colspan="2">Débitos</td>
                    </tr>
                    <tr class="debito">
                        <td align="right">Vale:</td>
                        <td>R$ <asp:Label runat="server" ID="lblValePedreira"></asp:Label></td>
                    </tr>
                    <tr class="debito">
                        <td align="right">INSS:</td>
                        <td>R$ <asp:Label runat="server" ID="lblINSSPedreira"></asp:Label></td>
                    </tr>
                    <tr class="debito">
                        <td align="right">Outros:</td>
                        <td>R$ <asp:Label runat="server" ID="lblOutrosDebitosPedreira"></asp:Label></td>
                    </tr>
                    <tr class="total">
                        <td align="right">Total 2:</td>
                        <td>R$ <asp:Label runat="server" ID="lblTotal2Pedreira"></asp:Label></td>
                    </tr>
                    <tr class="total total-geral">
                        <td align="right">Total geral:</td>
                        <td>R$ <asp:Label runat="server" ID="lblTotalGeralPedreira"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>

        <asp:Literal runat="server" ID="lblResultadoFolha"></asp:Literal>
        <br /><br />
        <asp:HyperLink ID="hplViagensUsuPer" runat="server" Text="Viagens do usuário no período"></asp:HyperLink>
    </asp:Panel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyEndContent" runat="server">
</asp:Content>