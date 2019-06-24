<%@ Page Title="Consultar viagens - Terrapar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="viagens.aspx.cs" Inherits="Terrapar.viagens" %>
<%@ Import Namespace="Terrapar.DAL" %>
<%@ Import Namespace="System.Globalization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="http://cdnjs.cloudflare.com/ajax/libs/floatthead/1.2.8/jquery.floatThead.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('table.borda').not('[id$=grdLojasViagem]').floatThead();

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

            //tabela clicável
            $('table.borda:not(.floatThead-table) tr:not(.total):not(:first) td:not(.not-print)').on('click', function () {
                var idViagem = $(this).parents('tr').data("id-viagem");
                $.ajax({
                    type: 'POST',
                    url: '/Viagens.aspx/SalvaViagemDestacada',
                    data: '{ tipo_viagem: "' + ($(this).parents("table").attr("id").indexOf("Pao") > -1 ? 1 : 2) + '", adicionar: "' + !$(this).parents('tr').hasClass("clicada") + '", id_viagem: "' + idViagem + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: function () {
                        //$(this).parents('tr').removeClass('clicada');
                    },
                    success: function (result) {
                        var r = result.d;
                        if (r) {
                            $("tr[data-id-viagem='" + idViagem + "']").toggleClass('clicada');
                            //viagens de "noite", "domingo" ou "feriado" pintar de outra cor
                            $('table.borda:not(.floatThead-table) tr:not(.total):not(:first) td:contains("Sim")').parent('tr:not(.clicada)').css('background-color', '#E5E7EA');
                        }
                    }
                });
            }).hover(function () {
                $(this).children().css('backgroundColor', '#fbf4e2');
            }, function () {
                $(this).children().css('backgroundColor', '');
            });

            //viagens de "noite", "domingo" ou "feriado" pintar de outra cor
            $('table.borda:not(.floatThead-table) tr:not(.total):not(:first) td:contains("Sim")').parent('tr:not(.clicada)').css('background-color', '#E5E7EA')

            //cria dropdowns de filtro nos headers de "Origem" e "Destino" => falta criar o de destino, e os eventos onclick para destacar as linhas filtradas
            $('<select id="ddlOrigem" class="ddlFiltro" data-index-col="3"></select>').appendTo('th:contains("Origem")');
            $('table.borda:not(.floatThead-table) tr:not(.total):not(:first) td:nth-child(3)').each(function () { $('#ddlOrigem').append($('<option value="' + $.trim($(this).text()) + '">' + $.trim($(this).text()) + '</option>')); });
            $('select#ddlOrigem option').each(function () {
                $(this).prevAll('option[value="' + this.value + '"]').remove();
            });
            $('#ddlOrigem').append($("#ddlOrigem option").remove().sort(function (a, b) {
                var at = $(a).text().toLowerCase(), bt = $(b).text().toLowerCase();
                return (at > bt) ? 1 : ((at < bt) ? -1 : 0);
            }));
            $('#ddlOrigem').prepend($('<option value="0">Selecione</option>'));

            $('<select id="ddlDestino" class="ddlFiltro" data-index-col="4"></select>').appendTo('th:contains("Destino")');
            $('table.borda:not(.floatThead-table) tr:not(.total):not(:first) td:nth-child(4)').each(function () { $('#ddlDestino').append($('<option value="' + $.trim($(this).text()) + '">' + $.trim($(this).text()) + '</option>')); });
            $('select#ddlDestino option').each(function () {
                $(this).prevAll('option[value="' + this.value + '"]').remove();
            });
            $('#ddlDestino').append($("#ddlDestino option").remove().sort(function (a, b) {
                var at = $(a).text().toLowerCase(), bt = $(b).text().toLowerCase();
                return (at > bt) ? 1 : ((at < bt) ? -1 : 0);
            }));
            $('#ddlDestino').prepend($('<option value="0">Selecione</option>'));

            function Filtro() {
                var currentOrigem = $.trim($('#ddlOrigem').val());
                var currentDestino = $.trim($('#ddlDestino').val());

                $('table.borda:not(.floatThead-table) tr:not(.total):not(:first)').each(function () {

                    var origem = $.trim($(this).find('td:nth-child(3)').text());
                    var destino = $.trim($(this).find('td:nth-child(4)').text());

                    if (origem === currentOrigem || destino === currentDestino || (currentOrigem === '0' && currentDestino === '0')) {
                        $(this).css('color', '#696969');
                        $(this).css('text-shadow', '');
                    }
                    else {
                        $(this).css('color', 'transparent');
                        $(this).css('text-shadow', '0 0 3px #aaa');
                    }

                });
            }

            $('#ddlOrigem').change(function () {
                Filtro();
            });
            $('#ddlDestino').change(function () {
                Filtro();
            });
        });

    </script>

    <style>
        
        .menu li:nth-child(2) a{
            background-color: #bfcbd6 !important;
            color: #465EC9 !important;
            text-decoration: underline;
        }
        
        .ddlFiltro{
            width: 150px;
            display: block;
            margin-top: 6px;
        }
        
        @media print 
        {
            div.header,
            div.footer,
            div.filtro,
            table.borda .not-print
            {
                display:none;
            }
            body, div.page
            {
                background-color: transparent;
                border: 0;
            }
            table.borda
            {
                background-color: white;
            }
            span.titulo
            {
                display:block !important;
                margin: 10px 0 20px 0;
                font-weight: bold;
                font-size: 14px;
            }
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:multiView runat="server" ID="vwEstados">
        <asp:View runat="server" ID="vBuscar">

            <div class="filtro">
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
                <input type="button" id="btnImprimir" value="Imprimir" onclick="javascript:window.print();" /><br /><br />
            </div>

            <asp:Label runat="server" ID="lblTitulo" CssClass="titulo" style="display:none;"></asp:Label>

            <asp:GridView runat="server" ID="grdViagensPaoAcucar" AutoGenerateColumns="false" Visible="false" ShowFooter="true"
                OnRowDataBound="grdViagensPaoAcucar_RowDataBound" OnRowCreated="grdViagensPaoAcucar_RowCreated" FooterStyle-CssClass="total" CssClass="borda">
                <Columns>
                    <asp:TemplateField HeaderText="Qtd. cargas">
                        <ItemTemplate>1</ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Qtd. lojas">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Data" DataField="data" DataFormatString="{0:d}">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Km final" DataField="km_final">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Qtde. diesel" DataField="qtd_diesel">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Km abast." DataField="km_abastecimento">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Valor abast." DataField="valor_abastecimento">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Valor frete">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Domingo">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%#Convert.ToBoolean(Eval("domingo")) ? "Sim" : "Não"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="not-print" HeaderStyle-CssClass="not-print" FooterStyle-CssClass="not-print">
                        <ItemTemplate>
                            <a href="viagens.aspx?t=0&id=<%#Eval("id_viagem_pao_acucar")%>">Editar</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="not-print" HeaderStyle-CssClass="not-print" FooterStyle-CssClass="not-print">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDeletar" runat="server" CommandArgument='<%#Eval("id_viagem_pao_acucar")%>' CommandName="pao_acucar" OnClientClick = "return confirm('Deseja realmente excluir a viagem?')" Text="Excluir" OnClick="lnkDeletar_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:GridView runat="server" ID="grdViagensPedreira" AutoGenerateColumns="false" Visible="false" ShowFooter="true"
                OnRowDataBound="grdViagensPedreira_RowDataBound" OnRowCreated="grdViagensPedreira_RowCreated" FooterStyle-CssClass="total" CssClass="borda">
                <Columns>
                    <asp:TemplateField HeaderText="Qtd. cargas">
                        <ItemTemplate>1</ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Data" DataField="data" DataFormatString="{0:d}">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Origem" DataField="origem">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Destino" DataField="destino">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Nº nota fiscal" DataField="numero_nota_fiscal">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Peso" DataField="peso">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Valor frete" DataField="valor_frete">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Base cálculo ICMS" DataField="base_calculo_icms">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Valor icms" DataField="valor_icms">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Valor produtos" DataField="valor_produtos">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Valor nota fiscal" DataField="valor_nota_fiscal">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Qtd. diesel" DataField="qtd_diesel">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Km abast." DataField="km_abastecimento">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Noite">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%#Convert.ToBoolean(Eval("noite")) ? "Sim" : "Não"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Domingo / feriado">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%#Convert.ToBoolean(Eval("domingo_feriado")) ? "Sim" : "Não"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="not-print" HeaderStyle-CssClass="not-print" FooterStyle-CssClass="not-print">
                        <ItemTemplate>
                            <a href="viagens.aspx?t=1&id=<%#Eval("id_viagem_pedreira")%>">Editar</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="not-print" HeaderStyle-CssClass="not-print" FooterStyle-CssClass="not-print">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDeletar" runat="server" CommandArgument='<%#Eval("id_viagem_pedreira")%>' CommandName="pedreira" OnClientClick = "return confirm('Deseja realmente excluir a viagem?')" Text="Excluir" OnClick="lnkDeletar_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>

        <asp:View runat="server" ID="vEditarPaoAcucar">
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
                    <td><asp:TextBox alt="999999999" runat="server" ID="txtKmAbastecimentoPaoAcucar" type="number"></asp:TextBox></td>
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
                            <asp:Label ID="lblLoja" runat="server" Text='<%#Eval("loja")%>'></asp:Label><br />
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

            <br />
            <asp:Button runat="server" ID="btnEditarPaoAcucar" Text="Salvar" CommandName="pao_acucar" OnClick="btnEditar_Click" />
            <input type="button" value="Voltar" onclick="window.history.back();" /><br />
        </asp:View>

        <asp:View runat="server" ID="vEditarPedreira">

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
                    <td><asp:TextBox alt="999999999" runat="server" ID="txtKmAbastecimentoPedreira" type="number"></asp:TextBox></td>
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
            <br />
            <asp:Button runat="server" ID="btnEditarPedreira" Text="Salvar" CommandName="pedreira" OnClick="btnEditar_Click" />
            <input type="button" value="Voltar" onclick="window.history.back();" /><br />
        </asp:View>

    </asp:multiView>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyEndContent" runat="server">
</asp:Content>
