<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="fechamento.aspx.cs" Inherits="Terrapar.fechamento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="divFechamentos">Carregando...</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyEndContent" runat="server">

    <style>
        .menu li:nth-child(5) a{
            background-color: #bfcbd6 !important;
            color: #465EC9 !important;
            text-decoration: underline;
        }
    </style>

    <script src="http://cdnjs.cloudflare.com/ajax/libs/floatthead/1.2.8/jquery.floatThead.min.js"></script>
    <script src="http://cdnjs.cloudflare.com/ajax/libs/numeral.js/1.4.5/numeral.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            numeral.language('pt-br', {
                delimiters: {
                    thousands: '.',
                    decimal: ','
                },
//                abbreviations: {
//                    thousand: 'k',
//                    million: 'm',
//                    billion: 'b',
//                    trillion: 't'
//                },
//                ordinal: function (number) {
//                    return number === 1 ? 'er' : 'ème';
//                },
                currency: {
                    symbol: 'R$'
                }
            });
            numeral.language('pt-br');
        });

        $.ajax({
            type: "POST",
            url: "fechamento.aspx/RecuperaFechamentos",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var fechamento = data.d;
                var items = [];
                var ultimoPeriodo = "", periodo, placa;
                var liquido_periodo = 0, bruto_periodo = 0, liquido_ano = 0, bruto_ano = 0;
                var placas = [];
                var i;

                //Cabeçalho
                items.push("<thead>");
                items.push("<tr>");
                items.push("<th>Período</th>");
                items.push("<th>Placa</th>");
                items.push("<th>Total Líquido</th>");
                items.push("<th>Total Bruto</th>");
                items.push("<th class='limite-esquerdo'>Líquido Anual</th>");
                items.push("<th>Bruto Anual</th>");
                items.push("</tr>");
                items.push("</thead>");

                $.each(fechamento, function (key, val) {
                    periodo = val.periodo;
                    numFormat = '0.0,';

                    //Acumula valores líquido e bruto das placas, por ano
                    var acumuladoPlaca = placas.filter(function (pl) { return pl.placa == val.placa && pl.ano == val.ano });
                    if (typeof (acumuladoPlaca) == "undefined" || acumuladoPlaca.length == 0) {
                        i = placas.push({ placa: val.placa, ano: val.ano, valor_liquido: val.valor_liquido, valor_bruto: val.valor_bruto });
                        acumuladoPlaca = placas[i - 1];
                    }
                    else {
                        acumuladoPlaca[0].valor_liquido += val.valor_liquido;
                        acumuladoPlaca[0].valor_bruto += val.valor_bruto;
                        acumuladoPlaca = acumuladoPlaca[0];
                    }

                    //Linha de soma
                    if (ultimoPeriodo !== "" && periodo !== ultimoPeriodo) {
                        items.push("<tr class='total'>");
                        items.push("<td>TOTAL " + ultimoPeriodo + "</td>");
                        items.push("<td>" + (bruto_periodo == 0 ? 0 : (numeral(liquido_periodo / bruto_periodo).format(numFormat))) + "%</td>");
                        items.push("<td>R$ " + numeral(liquido_periodo).format(numFormat) + "</td>");
                        items.push("<td>R$ " + numeral(bruto_periodo).format(numFormat) + "</td>");
                        items.push("<td class='limite-esquerdo'>R$ " + numeral(liquido_ano).format(numFormat) + "</td>");
                        items.push("<td>R$ " + numeral(bruto_ano).format(numFormat) + "</td>");
                        items.push("</tr>");
                        liquido_periodo = bruto_periodo = liquido_ano = bruto_ano = 0;
                    }

                    //Linha de detalhe
                    items.push("<tr>");
                    items.push("<td>" + periodo + "&nbsp;<span class='info' title='" + val.composicao_valor_liquido + "'>i</span></td>");
                    items.push("<td>" + val.placa + "</td>");
                    items.push("<td>R$ " + numeral(val.valor_liquido).format(numFormat) + "</td>");
                    items.push("<td>R$ " + numeral(val.valor_bruto).format(numFormat) + "</td>");
                    items.push("<td class='limite-esquerdo'>R$ " + numeral(acumuladoPlaca.valor_liquido).format(numFormat) + "</td>");
                    items.push("<td>R$ " + numeral(acumuladoPlaca.valor_bruto).format(numFormat) + "</td>");
                    items.push("</tr>");

                    //Acumula totais para a linha de soma
                    ultimoPeriodo = val.periodo;
                    liquido_periodo += val.valor_liquido;
                    bruto_periodo += val.valor_bruto;
                    liquido_ano += acumuladoPlaca.valor_liquido;
                    bruto_ano += acumuladoPlaca.valor_bruto;
                });

                //Última linha de soma
                items.push("<tr class='total'>");
                items.push("<td>TOTAL " + ultimoPeriodo + "</td>");
                items.push("<td>" + (bruto_periodo == 0 ? 0 : (numeral(liquido_periodo / bruto_periodo).format(numFormat))) + "%</td>");
                items.push("<td>R$ " + numeral(liquido_periodo).format(numFormat) + "</td>");
                items.push("<td>R$ " + numeral(bruto_periodo).format(numFormat) + "</td>");
                items.push("<td class='limite-esquerdo'>R$ " + numeral(liquido_ano).format(numFormat) + "</td>");
                items.push("<td>R$ " + numeral(bruto_ano).format(numFormat) + "</td>");
                items.push("</tr>");

                //Append
                $("#divFechamentos").html('').append($("<table/>", {
                    "class": "borda fechamento",
                    html: items.join("")
                }));

                //Valores negativos em vermelho
                $('table td').each(function () {
                    $(this).toggleClass('valor-negativo', $(this).text().indexOf('-') >= 0);
                });

                $('table.borda').floatThead();
            },
            error: function () {
                alert('Erro ao requisitar o fechamento.');
            }
        });

    </script>
</asp:Content>
