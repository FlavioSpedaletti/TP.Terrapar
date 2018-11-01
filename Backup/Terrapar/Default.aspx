<%@ Page Title="Registrar viagem - Terrapar" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Terrapar._Default" MaintainScrollPositionOnPostback="true" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HeadContent">

    <style>
    
        .menu li:nth-child(1) a{
            background-color: #bfcbd6 !important;
            color: #465EC9 !important;
            text-decoration: underline;
        }
    
    </style>

    <script type="text/javascript">

        function CalculaRendimentoeTrocaOleoPaoAcucar() {
            var lblTrocaOleo = $('[id$=lblTrocaOleo]');
            var lblRendimentoPaoAcucar = $('[id$=lblRendimentoPaoAcucar]');
            $.ajax({
                type: 'POST',
                url: '/Default.aspx/CalculaRendimentoeTrocaOleoPaoAcucar',
                data: '{ KmAbastecimento: "' + $('[id$=txtKmAbastecimentoPaoAcucar]').val() + '", QtdDiesel: "' + $('[id$=txtQtdDieselPaoAcucar]').val() + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    lblTrocaOleo.hide();
                    lblRendimentoPaoAcucar.attr('title', 'Calculando...');
                    lblRendimentoPaoAcucar.text('Calculando...');
                },
                error: function () {
                    lblTrocaOleo.hide();
                    lblRendimentoPaoAcucar.attr('title', 'Erro ao calcular');
                    lblRendimentoPaoAcucar.text('Erro ao calcular');
                },
                success: function (result) {
                    var r = result.d;
                    lblTrocaOleo.toggle(r.trocaOleo);
                    lblRendimentoPaoAcucar.attr('title', r.rendimentoDesc);
                    lblRendimentoPaoAcucar.text(r.rendimento);
                }
            });
        }

        function CalculaRendimentoeTrocaOleoPedreira() {
            var lblTrocaOleo = $('[id$=lblTrocaOleo]');
            var lblRendimentoPedreira = $('[id$=lblRendimentoPedreira]');
            $.ajax({
                type: 'POST',
                url: '/Default.aspx/CalculaRendimentoeTrocaOleoPedreira',
                data: '{ KmAbastecimento: "' + $('[id$=txtKmAbastecimentoPedreira]').val() + '", QtdDiesel: "' + $('[id$=txtQtdDieselPedreira]').val() + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    lblTrocaOleo.toggle(false);
                    lblRendimentoPedreira.attr('title', 'Calculando...');
                    lblRendimentoPedreira.text('Calculando...');
                },
                error: function () {
                    lblTrocaOleo.toggle(false);
                    lblRendimentoPedreira.attr('title', 'Erro ao calcular');
                    lblRendimentoPedreira.text('Erro ao calcular');
                },
                success: function (result) {
                    var r = result.d;
                    lblTrocaOleo.toggle(r.trocaOleo);
                    lblRendimentoPedreira.attr('title', r.rendimentoDesc);
                    lblRendimentoPedreira.text(r.rendimento);
                }
            });
        }

        $(document).ready(function () {
            //lojas viagem
            $('[id*=ddlLojas]').on('change', function () {
                var transf = $('[id*=ddlLojas] :selected').text().toLowerCase().indexOf("transferência") >= 0;
                var centralPaes = $('[id*=ddlLojas] :selected').text().toLowerCase().indexOf("central de pães") >= 0;
                var txtValorFrete = $('[id*=txtValorFrete]');
                var txtNumeroOrdemColeta = $('[id*=txtNumeroOrdemColeta]');
                txtValorFrete.attr('readonly', transf);
                txtNumeroOrdemColeta.attr('readonly', !transf && !centralPaes);
                txtValorFrete.css('background-color', transf ? '#ddd' : '#fff');
                txtNumeroOrdemColeta.css('background-color', !transf && !centralPaes ? '#ddd' : '#fff');
                txtValorFrete.val(transf ? '' : txtValorFrete.val());
                txtNumeroOrdemColeta.val(!transf && !centralPaes ? '' : txtNumeroOrdemColeta.val());
            });

            //cálculo de rendimento de combustível e troca de óleo no load
            if ($('[id$=txtQtdDieselPaoAcucar]').is(':visible'))
                CalculaRendimentoeTrocaOleoPaoAcucar();
            else
                CalculaRendimentoeTrocaOleoPedreira();

            //cálculo de rendimento de combustível e troca de óleo no lostfocus
            $('[id$=txtKmAbastecimentoPaoAcucar]').on('blur', function () {
                CalculaRendimentoeTrocaOleoPaoAcucar()
            });
            $('[id$=txtQtdDieselPaoAcucar]').on('blur', function () {
                CalculaRendimentoeTrocaOleoPaoAcucar()
            });
            $('[id$=txtKmAbastecimentoPedreira]').on('blur', function () {
                CalculaRendimentoeTrocaOleoPedreira()
            });
            $('[id$=txtQtdDieselPedreira]').on('blur', function () {
                CalculaRendimentoeTrocaOleoPedreira()
            });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">

    <asp:RadioButtonList runat="server" ID="rdlViagens" Width="270" AutoPostBack="true" OnTextChanged="rdlViagens_TextChanged" RepeatDirection="Horizontal" Enabled="false" Visible="false">
        <asp:ListItem Selected="True" Text="Pão de açúcar" Value="0"></asp:ListItem>
        <asp:ListItem Text="Pedreira" Value="1"></asp:ListItem>
    </asp:RadioButtonList>
    <br />

    <asp:multiView runat="server" ID="vwViagens">
        <asp:View runat="server" ID="vPaoAcucar">
            <table>
                <tr>
                    <td align="right"><label for="txtDataPaoAcucar">* Data:</label></td>
                    <td><asp:TextBox alt="date" runat="server" ID="txtDataPaoAcucar" required></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtKmFinal">* KM final:</label></td>
                    <td><asp:TextBox alt="999999999" runat="server" ID="txtKmFinal" type="number" required></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtQtdDieselPaoAcucar">Quantidade diesel:</label></td>
                    <td><asp:TextBox runat="server" ID="txtQtdDieselPaoAcucar" type="number" step="any"></asp:TextBox> litros</td>
                </tr>
                <tr>
                    <td align="right"><label for="txtKmAbastecimentoPaoAcucar">KM abastecimento:</label></td>
                    <td><asp:TextBox alt="999999999" runat="server" ID="txtKmAbastecimentoPaoAcucar" type="number"></asp:TextBox><br />
                        <asp:Label runat="server" ID="lblRendimentoPaoAcucar" CssClass="aviso-critico" Text="Rendimento: ainda não é possível calcular"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorAbastecimentoPaoAcucar">Valor abastecimento: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorAbastecimentoPaoAcucar" type="number" step="any"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="chkDomingoPaoAcucar">Domingo:</label></td>
                    <td><asp:CheckBox runat="server" ID="chkDomingoPaoAcucar"></asp:CheckBox></td>
                </tr>
            </table>

            <br />
            <hr />

            <strong>Lojas:</strong><br /><br />
            <asp:Button runat="server" ID="btnAddLojasViagem" Text="Adicionar" OnClick="btnAddLojasViagem_Click" />
            <asp:Button runat="server" ID="btnZerarLojasViagem" Text="Apagar todas" OnClick="btnZerarLojasViagem_Click" /><br /><br />
            <asp:GridView runat="server" ID="grdLojasViagem" DataKeyNames="id_loja_viagem_pao_acucar" AutoGenerateColumns="false"
            OnRowCancelingEdit="grdLojasViagem_RowCancelingEdit" OnRowDeleting="grdLojasViagem_RowDeleting" OnRowEditing="grdLojasViagem_RowEditing"
            OnRowUpdating="grdLojasViagem_RowUpdating" OnRowDataBound="grdLojasViagem_RowDataBound" FooterStyle-CssClass="total" CssClass="borda">
                <Columns>
                    <asp:CommandField HeaderText="Editar" ShowEditButton="True" EditText="Editar" InsertText="Ok" UpdateText="Ok" CancelText="Cancelar" ControlStyle-CssClass="interno" />
                    <asp:TemplateField HeaderText="Loja">
                        <EditItemTemplate>
                            <asp:DropDownList Width="130" runat="server" ID="ddlLojas" DataTextField="identificacaoEnome" DataValueField="id_loja"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblLoja" runat="server" Text='<%#Eval("loja")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor frete">
                        <EditItemTemplate>
                            <asp:TextBox Width="50" ID="txtValorFrete" runat="server" Text='<%#String.IsNullOrEmpty(Eval("valor_frete").ToString()) ? "" : Decimal.Parse(Eval("valor_frete").ToString()).ToString(CultureInfo.InvariantCulture.NumberFormat)%>' type="number" step="any" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblValorFrete" runat="server" Text='<%#Eval("valor_frete")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nº ordem coleta">
                        <EditItemTemplate>
                            <asp:TextBox Width="50" ID="txtNumeroOrdemColeta" runat="server" Text='<%#Eval("numero_ordem_coleta")%>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblNumeroOrdemColeta" runat="server" Text='<%#Eval("numero_ordem_coleta")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField HeaderText="Deletar" ShowDeleteButton="True" ControlStyle-CssClass="interno" />
                </Columns>
            </asp:GridView>
            <br />

        </asp:View>

        <asp:View runat="server" ID="vPedreira">
            <table>
                <tr>
                    <td align="right"><label for="txtDataPedreira">* Data:</label></td>
                    <td><asp:TextBox alt="date" runat="server" ID="txtDataPedreira" required></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtOrigem">* Origem:</label></td>
                    <td><asp:TextBox runat="server" ID="txtOrigem" required></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtDestino">* Destino:</label></td>
                    <td><asp:TextBox runat="server" ID="txtDestino" required></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtNumeroNotaFiscal">* Numero nota fiscal:</label></td>
                    <td><asp:TextBox runat="server" ID="txtNumeroNotaFiscal" required></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtPeso">* Peso:</label></td>
                    <td><asp:TextBox runat="server" ID="txtPeso" type="number" step="any" required></asp:TextBox> Kilos</td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorFretePedreira">* Valor frete: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorFretePedreira" type="number" step="any" required></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtBaseCalculoICMS">Base cálculo ICMS: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtBaseCalculoICMS" type="number" step="any"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorICMS">Valor ICMS: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorICMS" type="number" step="any"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorProdutos">Valor produtos: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorProdutos" type="number" step="any"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorNotaFiscal">Valor nota fiscal: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorNotaFiscal" type="number" step="any"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtQtdDieselPedreira">Quantidade diesel:</label></td>
                    <td><asp:TextBox runat="server" ID="txtQtdDieselPedreira" type="number" step="any"></asp:TextBox> litros</td>
                </tr>
                <tr>
                    <td align="right"><label for="txtKmAbastecimentoPedreira">KM abastecimento:</label></td>
                    <td><asp:TextBox alt="999999999" runat="server" ID="txtKmAbastecimentoPedreira" type="number"></asp:TextBox><br />
                        <asp:Label runat="server" ID="lblRendimentoPedreira" CssClass="aviso-critico" Text="Rendimento: ainda não é possível calcular"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right"><label for="txtValorAbastecimentoPedreira">Valor abastecimento: R$</label></td>
                    <td><asp:TextBox runat="server" ID="txtValorAbastecimentoPedreira" type="number" step="any"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="chkNoitePedreira">Noite:</label></td>
                    <td><asp:CheckBox runat="server" ID="chkNoitePedreira"></asp:CheckBox></td>
                </tr>
                <tr>
                    <td align="right"><label for="chkDomingoFeriadoPedreira">Domingo / feriado:</label></td>
                    <td><asp:CheckBox runat="server" ID="chkDomingoFeriadoPedreira"></asp:CheckBox></td>
                </tr>
            </table>
        </asp:View>
    </asp:multiView>
    <br />
    <asp:Label runat="server" style="display:none;" ID="lblTrocaOleo" CssClass="aviso-critico" Text="Fazer a troca de óleo"></asp:Label><br />
    <br />
    <asp:Button runat="server" ID="btnInserir" Text="Salvar" OnClick="btnInserir_Click" />
    <asp:Button runat="server" ID="btnLimpar" Text="Limpar" OnClick="btnLimpar_Click" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyEndContent" runat="server">
</asp:Content>