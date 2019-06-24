using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Terrapar.DAL;
using Terrapar.DTO;

namespace Terrapar
{
    public partial class viagens : Page
    {
        //linha de totais das tabelas de viagens
        private int soma_qtd_cargas, soma_qtd_viagens, primeiro_km_final, dif_km_final, primeiro_km_abastecimento, dif_km_abastecimento;
        private decimal soma_qtd_diesel, soma_valor_frete, soma_valor_abastecimento, soma_peso, soma_valor_produtos, soma_valor_nfs;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            try
            {
                var id = Request["id"];
                //busca
                if (id == null)
                {
                    vwEstados.ActiveViewIndex = 0;
                    PopulaUsuarios();

                    var usu = Request["usu"];
                    var per = Request["per"];
                    if(usu != null && per != null)
                    {
                        ddlUsuarios.SelectedValue = usu;
                        ddlUsuarios_SelectedIndexChanged(null, null);
                        ddlPeriodo.SelectedValue = per;
                        Buscar();
                    }
                    else
                    {
                        if (ddlUsuarios.Items.Count == 2)
                        {
                            ddlUsuarios.SelectedIndex = 1;
                            ddlUsuarios_SelectedIndexChanged(null, null);
                        }
                    }
                }
                else
                {
                    int idViagem;
                    int.TryParse(id, out idViagem);
                    var tipo = Request["t"];

                    //edição
                    if (tipo == "0")
                    {
                        //pão de açúcar

                        ZerarLojasViagem();

                        var viagem = new viagens_pao_acucarDAL().RecuperarPorID(idViagem);
                        if (viagem == null)
                        {
                            ClientScript.RegisterStartupScript(GetType(), "viagemNaoEncontrada", "alert('Viagem não encontrada');window.location.href='viagens.aspx';", true);
                        }
                        else
                        {
                            txtDataPaoAcucar.Text = viagem.data.ToString("dd/MM/yyyy");
                            txtKmFinal.Text = viagem.km_final.ToString();
                            txtQtdDieselPaoAcucar.Text = viagem.qtd_diesel.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            txtKmAbastecimentoPaoAcucar.Text = viagem.km_abastecimento.ToString();
                            txtValorAbastecimentoPaoAcucar.Text = viagem.valor_abastecimento.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            chkDomingoPaoAcucar.Checked = viagem.domingo;
                            vwEstados.ActiveViewIndex = 1;

                            var lojas_viagem = new lojas_viagem_pao_acucarDAL().Listar(new loja_viagem_pao_acucar()
                                                                               {
                                                                                   id_viagem_pao_acucar = idViagem
                                                                               });
                            var dt = (DataTable)Session["lojas_viagem"];

                            foreach (var r in lojas_viagem)
                            {
                                var dr = dt.NewRow();
                                dr["id_loja_viagem_pao_acucar"] = dt.Rows.Count > 0
                                                    ? Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["id_loja_viagem_pao_acucar"]) + 1
                                                    : 1;
                                dr["id_loja"] = r.id_loja;
                                dr["loja"] = new lojasDAL().RecuperarPorID(r.id_loja).identificacaoEnome;
                                dr["valor_frete"] = r.valor_frete;
                                dr["numero_ordem_coleta"] = r.numero_ordem_coleta;

                                dt.Rows.Add(dr);
                            }

                            grdLojasViagem.DataSource = dt;
                            grdLojasViagem.DataBind();
                        }
                    }
                    else
                    {
                        //pedreira
                        var viagem = new viagens_pedreiraDAL().RecuperarPorID(idViagem);
                        if (viagem == null)
                        {
                            ClientScript.RegisterStartupScript(GetType(), "viagemNaoEncontrada", "alert('Viagem não encontrada');window.location.href='viagens.aspx';", true);
                        }
                        else
                        {
                            txtDataPedreira.Text = viagem.data.ToString("dd/MM/yyyy");
                            txtOrigem.Text = viagem.origem;
                            txtDestino.Text = viagem.destino;
                            txtNumeroNotaFiscal.Text = viagem.numero_nota_fiscal;
                            txtPeso.Text = viagem.peso.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            txtValorFretePedreira.Text = viagem.valor_frete.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            txtBaseCalculoICMS.Text = viagem.base_calculo_icms.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            txtValorICMS.Text = viagem.valor_icms.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            txtValorProdutos.Text = viagem.valor_produtos.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            txtValorNotaFiscal.Text = viagem.valor_nota_fiscal.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            txtQtdDieselPedreira.Text = viagem.qtd_diesel.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            txtKmAbastecimentoPedreira.Text = viagem.km_abastecimento.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            txtValorAbastecimentoPedreira.Text = viagem.valor_abastecimento.ToString(CultureInfo.InvariantCulture.NumberFormat);
                            chkNoitePedreira.Checked = viagem.noite;
                            chkDomingoFeriadoPedreira.Checked = viagem.domingo_feriado;
                            vwEstados.ActiveViewIndex = 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                vwEstados.ActiveViewIndex = 0;
            }
        }

        protected void PopulaUsuarios()
        {
            var usuario = (usuario)Session["usuario"];
            var dt = new usuariosDAL().ListarComViagem();
            if (!usuario.admin) dt = dt.Where(u => u.id_usuario == usuario.id_usuario);
            ddlUsuarios.DataSource = dt.OrderBy(u => u.placa);
            ddlUsuarios.DataBind();
            ddlUsuarios.Items.Insert(0, new ListItem("Selecione", "-1"));
        }

        protected void ddlUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            var tipoPlaca = new usuariosDAL().RecuperarPorID(idUsuario);

            if (tipoPlaca == null)
            {
                ddlPeriodo.Items.Clear();
                grdViagensPedreira.Visible = false;
                grdViagensPaoAcucar.Visible = false;
                return;
            }

            ddlPeriodo.DataSource = tipoPlaca.tipo == 0
                                        ? new viagens_pao_acucarDAL().Listar(new viagem_pao_acucar() { id_usuario = idUsuario }).Select(p => p.data).OrderBy(v => v).Select(v => v.ToString("MM/yyyy")).Distinct()
                                        : new viagens_pedreiraDAL().Listar(new viagem_pedreira() { id_usuario = idUsuario }).Select(p => p.data).OrderBy(v => v).Select(v => v.ToString("MM/yyyy")).Distinct();
            ddlPeriodo.DataBind();
            ddlPeriodo.SelectedIndex = ddlPeriodo.Items.Count - 1;
        }

        protected void Buscar()
        {
            string validacao = ValidarBuscar();
            if (!string.IsNullOrEmpty(validacao))
            {
                ClientScript.RegisterStartupScript(GetType(), "validaBuscaFolhaPgto", "alert('" + validacao + "');", true);
                return;
            }

            var idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            var tipoPlaca = new usuariosDAL().RecuperarPorID(idUsuario).tipo;
            var textoTitulo = "Viagens {0} - " + ddlUsuarios.SelectedItem.Text + " em " + ddlPeriodo.SelectedItem.Text;

            switch (tipoPlaca)
            {
                case 0:
                    grdViagensPaoAcucar.DataSource = new viagens_pao_acucarDAL().Listar(new viagem_pao_acucar() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) });
                    grdViagensPaoAcucar.DataBind();
                    grdViagensPaoAcucar.Visible = true;
                    grdViagensPedreira.Visible = false;
                    lblTitulo.Text = string.Format(textoTitulo, "Pão de açúcar");
                    break;

                case 1:
                    grdViagensPedreira.DataSource = new viagens_pedreiraDAL().Listar(new viagem_pedreira() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) });
                    grdViagensPedreira.DataBind();
                    grdViagensPedreira.Visible = true;
                    grdViagensPaoAcucar.Visible = false;
                    lblTitulo.Text = string.Format(textoTitulo, "Pedreira");
                    break;
            }
        }

        protected string ValidarBuscar()
        {
            var ret = "";

            if (ddlUsuarios.SelectedValue == "-1")
                ret += "Selecione uma placa.\\n";
            if (ddlPeriodo.SelectedValue == "-1" || ddlPeriodo.SelectedValue == "")
                ret += "Selecione um período.\\n";

            return ret;
        }

        protected string ValidarEditar()
        {
            var ret = "";
            DateTime data;

            switch (vwEstados.ActiveViewIndex)
            {
                case 1: //pão de açúcar
                    if (!DateTime.TryParse(txtDataPaoAcucar.Text, out data) || txtDataPaoAcucar.Text.Length != 10)
                        ret += "Preencha uma data válida.\\n";
                    if (string.IsNullOrEmpty(txtKmFinal.Text) || (Convert.ToInt32(txtKmFinal.Text) <= 0))
                        ret += "Preencha o KM final.\\n";
                    //if (string.IsNullOrEmpty(txtQtdDieselPaoAcucar.Text) || (Convert.ToDecimal(txtQtdDieselPaoAcucar.Text) <= 0))
                    //    ret += "Preencha a quantidade de diesel.\\n";
                    //if (string.IsNullOrEmpty(txtKmAbastecimentoPaoAcucar.Text) || (Convert.ToInt32(txtKmAbastecimentoPaoAcucar.Text) <= 0))
                    //    ret += "Preencha o KM de abastecimento.\\n";
                    //if (string.IsNullOrEmpty(txtValorAbastecimentoPaoAcucar.Text) || (Convert.ToDecimal(txtValorAbastecimentoPaoAcucar.Text) <= 0))
                    //    ret += "Preencha o valor do abastecimento.\\n";

                    if (string.IsNullOrEmpty(ret))
                    {
                        var dt = (DataTable)Session["lojas_viagem"];
                        if (dt == null || dt.Rows.Count == 0 || grdLojasViagem.Rows.Count == 0)
                            ret += "Preencha ao menos uma loja para essa viagem.\\n";
                        else
                        {
                            foreach (DataRow r in dt.Rows)
                            {
                                var idLoja = string.IsNullOrEmpty(r["id_loja"].ToString()) ? "-1" : r["id_loja"].ToString();
                                var loja = new lojasDAL().RecuperarPorID(Convert.ToInt32(idLoja));
                                if (loja == null || idLoja == "-1")
                                    return "Lojas: Selecione uma loja.\\n";

                                var transf = loja.nome.ToLower().Contains("transferência");

                                if (!transf && (string.IsNullOrEmpty(r["valor_frete"].ToString()) || (Convert.ToDecimal(r["valor_frete"].ToString()) <= 0)))
                                    ret += "Lojas: Preencha o valor do frete.\\n";
                                if (transf && string.IsNullOrEmpty(r["numero_ordem_coleta"].ToString()))
                                    ret += "Lojas: Preencha o número da ordem de coleta.\\n";

                                if (!string.IsNullOrEmpty(ret))
                                    return ret;
                            }
                        }
                    }
                    break;
                case 2: //pedreira
                    if (!DateTime.TryParse(txtDataPedreira.Text, out data) || txtDataPedreira.Text.Length != 10)
                        ret += "Preencha uma data válida.\\n";
                    if (string.IsNullOrEmpty(txtOrigem.Text))
                        ret += "Preencha a origem.\\n";
                    if (string.IsNullOrEmpty(txtDestino.Text))
                        ret += "Preencha o destino.\\n";
                    if (string.IsNullOrEmpty(txtNumeroNotaFiscal.Text))
                        ret += "Preencha o número da nota fiscal.\\n";
                    if (string.IsNullOrEmpty(txtPeso.Text) || (Convert.ToDecimal(txtPeso.Text) <= 0))
                        ret += "Preencha o peso.\\n";
                    if (string.IsNullOrEmpty(txtValorFretePedreira.Text) || (Convert.ToDecimal(txtValorFretePedreira.Text) <= 0))
                        ret += "Preencha o valor do frete.\\n";
                    //if (string.IsNullOrEmpty(txtBaseCalculoICMS.Text) || (Convert.ToDecimal(txtBaseCalculoICMS.Text) <= 0))
                    //    ret += "Preencha a base de cálculo ICMS.\\n";
                    //if (string.IsNullOrEmpty(txtValorICMS.Text) || (Convert.ToDecimal(txtValorICMS.Text) <= 0))
                    //    ret += "Preencha o valor do ICMS.\\n";
                    //if (string.IsNullOrEmpty(txtValorProdutos.Text) || (Convert.ToDecimal(txtValorProdutos.Text) <= 0))
                    //    ret += "Preencha o valor dos produtos.\\n";
                    //if (string.IsNullOrEmpty(txtValorNotaFiscal.Text) || (Convert.ToDecimal(txtValorNotaFiscal.Text) <= 0))
                    //    ret += "Preencha o valor da nota fiscal.\\n";
                    //if (string.IsNullOrEmpty(txtQtdDieselPedreira.Text) || (Convert.ToDecimal(txtQtdDieselPedreira.Text) <= 0))
                    //    ret += "Preencha a quantidade de diesel.\\n";
                    //if (string.IsNullOrEmpty(txtKmAbastecimentoPedreira.Text) || (Convert.ToInt32(txtKmAbastecimentoPedreira.Text) <= 0))
                    //    ret += "Preencha o KM de abastecimento.\\n";
                    //if (string.IsNullOrEmpty(txtValorAbastecimentoPedreira.Text) || (Convert.ToDecimal(txtValorAbastecimentoPedreira.Text) <= 0))
                    //    ret += "Preencha o valor do abastecimento.\\n";
                    break;
            }


            return ret;
        }

        protected void grdViagensPaoAcucar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[9].Visible = ((usuario)Session["usuario"]).admin || Convert.ToDateTime("01/" + ddlPeriodo.Text).Month == DateTime.Now.Month;
            e.Row.Cells[10].Visible = ((usuario)Session["usuario"]).admin;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    primeiro_km_final = Convert.ToInt32(e.Row.Cells[3].Text);
                    primeiro_km_abastecimento = Convert.ToInt32(e.Row.Cells[5].Text);
                }
                dif_km_final = Convert.ToInt32(e.Row.Cells[3].Text) - primeiro_km_final;
                dif_km_abastecimento = Convert.ToInt32(e.Row.Cells[5].Text) - primeiro_km_abastecimento;
                soma_qtd_diesel += Convert.ToDecimal(e.Row.Cells[4].Text);
                soma_valor_abastecimento += Convert.ToDecimal(e.Row.Cells[6].Text);

                var lojas_viagem =
                    new lojas_viagem_pao_acucarDAL().Listar(new loja_viagem_pao_acucar()
                                                          {
                                                              id_viagem_pao_acucar =
                                                                  Convert.ToInt32(DataBinder.Eval(e.Row.DataItem,
                                                                                                  "id_viagem_pao_acucar"))
                                                          });

                e.Row.Cells[1].Text = lojas_viagem.Count().ToString();
                e.Row.Cells[7].Text = lojas_viagem.Sum(c => c.valor_frete).ToString();

                soma_qtd_cargas += Convert.ToInt32(e.Row.Cells[0].Text);
                soma_qtd_viagens += Convert.ToInt32(e.Row.Cells[1].Text);
                soma_valor_frete += Convert.ToDecimal(e.Row.Cells[7].Text);

                //linhas destacadas (persistidas no BD)
                e.Row.Attributes.Add("data-id-viagem", (DataBinder.Eval(e.Row.DataItem, "id_viagem_pao_acucar").ToString()));
                if(new viagens_pao_acucar_destacadasDAL().Listar(new viagem_pao_acucar_destacada()
                                                                {id_usuario = ((usuario)Session["usuario"]).id_usuario,
                                                                 id_viagem_pao_acucar = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id_viagem_pao_acucar"))}).Count() > 0)
                {
                    e.Row.CssClass = "clicada";
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "(+) " + soma_qtd_cargas; 
                e.Row.Cells[1].Text = "(+) " + soma_qtd_viagens;
                e.Row.Cells[3].Text = "(-) " + dif_km_final;
                e.Row.Cells[4].Text = "(+) " + soma_qtd_diesel;
                e.Row.Cells[5].Text = "(-) " + dif_km_abastecimento;
                e.Row.Cells[6].Text = "(+) " + soma_valor_abastecimento;
                e.Row.Cells[7].Text = "(+) " + soma_valor_frete;
            }
        }

        protected void grdViagensPedreira_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[15].Visible = ((usuario)Session["usuario"]).admin || Convert.ToDateTime("01/" + ddlPeriodo.Text).Month == DateTime.Now.Month;
            e.Row.Cells[16].Visible = ((usuario)Session["usuario"]).admin;
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                soma_qtd_cargas += Convert.ToInt32(e.Row.Cells[0].Text);
                soma_peso += Convert.ToDecimal(e.Row.Cells[5].Text);
                soma_valor_frete += Convert.ToDecimal(e.Row.Cells[6].Text);
                soma_valor_produtos += Convert.ToDecimal(e.Row.Cells[9].Text);
                soma_valor_nfs += Convert.ToDecimal(e.Row.Cells[10].Text);
                soma_qtd_diesel += Convert.ToDecimal(e.Row.Cells[11].Text);
                if (e.Row.RowIndex == 0)
                {
                    primeiro_km_abastecimento = Convert.ToInt32(e.Row.Cells[12].Text);
                }
                dif_km_abastecimento = Convert.ToInt32(e.Row.Cells[12].Text) - primeiro_km_abastecimento;

                //linhas destacadas (persistidas no BD)
                e.Row.Attributes.Add("data-id-viagem", (DataBinder.Eval(e.Row.DataItem, "id_viagem_pedreira").ToString()));
                if (new viagens_pedreira_destacadasDAL().Listar(new viagem_pedreira_destacada()
                {
                    id_usuario = ((usuario)Session["usuario"]).id_usuario,
                    id_viagem_pedreira = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id_viagem_pedreira"))
                }).Count() > 0)
                {
                    e.Row.CssClass = "clicada";
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "(+) " + soma_qtd_cargas;
                e.Row.Cells[5].Text = "(+) " + soma_peso;
                e.Row.Cells[6].Text = "(+) " + soma_valor_frete;
                e.Row.Cells[9].Text = "(+) " + soma_valor_produtos;
                e.Row.Cells[10].Text = "(+) " + soma_valor_nfs;
                e.Row.Cells[11].Text = "(+) " + soma_qtd_diesel;
                e.Row.Cells[12].Text = "(-) " + dif_km_abastecimento;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        protected void lnkDeletar_Click(object sender, EventArgs e)
        {
            var lnkRemove = (LinkButton)sender;
            int ok;

            if(lnkRemove.CommandName=="pao_acucar")
            {
                ok =
                    new viagens_pao_acucarDAL().Deletar(new viagem_pao_acucar()
                                                            {
                                                                id_viagem_pao_acucar = int.Parse(lnkRemove.CommandArgument)
                                                            });
            }
            else
            {
                ok =
                    new viagens_pedreiraDAL().Deletar(new viagem_pedreira()
                    {
                        id_viagem_pedreira = int.Parse(lnkRemove.CommandArgument)
                    });
            }

            ClientScript.RegisterStartupScript(GetType(), "deletou", ok > 0 ? "alert('Viagem excluída com sucesso.');" : "alert('Ocorreu algum erro ao tentar excluir. Tente novamente.');", true);
            Buscar();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            string validacao = ValidarEditar();
            if (!string.IsNullOrEmpty(validacao))
            {
                ClientScript.RegisterStartupScript(GetType(), "validaViagem", "alert('" + validacao + "');", true);
                return;
            }

            bool ok = false;
            long viagem = 0, deletar_lojas_viagem = 0, lojas_viagem = 0;
            switch (vwEstados.ActiveViewIndex)
            {
                case 1: //pão de açúcar
                    viagem = new viagens_pao_acucarDAL().Atualizar(new viagem_pao_acucar()
                    {
                        id_viagem_pao_acucar = Convert.ToInt32(Request["id"]),
                        id_usuario = new viagens_pao_acucarDAL().RecuperarPorID(Convert.ToInt32(Request["id"])).id_usuario,
                        data = Convert.ToDateTime(txtDataPaoAcucar.Text),
                        km_final = int.Parse(txtKmFinal.Text),
                        qtd_diesel = !string.IsNullOrEmpty(txtQtdDieselPaoAcucar.Text) ? decimal.Parse(txtQtdDieselPaoAcucar.Text, CultureInfo.InvariantCulture.NumberFormat) : 0,
                        km_abastecimento = !string.IsNullOrEmpty(txtKmAbastecimentoPaoAcucar.Text) ? int.Parse(txtKmAbastecimentoPaoAcucar.Text) : 0,
                        valor_abastecimento = !string.IsNullOrEmpty(txtValorAbastecimentoPaoAcucar.Text) ? decimal.Parse(txtValorAbastecimentoPaoAcucar.Text, CultureInfo.InvariantCulture.NumberFormat) : 0,
                        domingo = chkDomingoPaoAcucar.Checked
                    });

                    //lojas pão de açúcar
                    var dt = (DataTable)Session["lojas_viagem"];
                    deletar_lojas_viagem = new viagens_pao_acucarDAL().DeletarLojas(new viagem_pao_acucar()
                                                                 {id_viagem_pao_acucar = Convert.ToInt32(Request["id"])});
                    foreach (DataRow r in dt.Rows)
                    {
                        lojas_viagem = new lojas_viagem_pao_acucarDAL().Adicionar(new loja_viagem_pao_acucar()
                                                                      {
                                                                          id_viagem_pao_acucar = Convert.ToInt32(Request["id"]),
                                                                          id_loja = Convert.ToInt32(r["id_loja"]),
                                                                          valor_frete = !string.IsNullOrEmpty(r["valor_frete"].ToString()) ? Convert.ToDecimal(r["valor_frete"], CultureInfo.InvariantCulture.NumberFormat) : 0,
                                                                          numero_ordem_coleta = r["numero_ordem_coleta"].ToString()
                                                                      });
                    }
                    
                    ok = viagem > 0 && lojas_viagem > 0;
                    break;
                case 2: //pedreira
                    viagem = new viagens_pedreiraDAL().Atualizar(new viagem_pedreira()
                    {
                        id_viagem_pedreira = Convert.ToInt32(Request["id"]),
                        id_usuario = new viagens_pedreiraDAL().RecuperarPorID(Convert.ToInt32(Request["id"])).id_usuario,
                        data = Convert.ToDateTime(txtDataPedreira.Text),
                        origem = txtOrigem.Text,
                        destino = txtDestino.Text,
                        numero_nota_fiscal = txtNumeroNotaFiscal.Text,
                        peso = decimal.Parse(txtPeso.Text, CultureInfo.InvariantCulture.NumberFormat),
                        valor_frete = decimal.Parse(txtValorFretePedreira.Text, CultureInfo.InvariantCulture.NumberFormat),
                        base_calculo_icms = !string.IsNullOrEmpty(txtBaseCalculoICMS.Text) ? decimal.Parse(txtBaseCalculoICMS.Text, CultureInfo.InvariantCulture.NumberFormat) : 0,
                        valor_icms = !string.IsNullOrEmpty(txtValorICMS.Text) ? decimal.Parse(txtValorICMS.Text, CultureInfo.InvariantCulture.NumberFormat) : 0,
                        valor_produtos = !string.IsNullOrEmpty(txtValorProdutos.Text) ? decimal.Parse(txtValorProdutos.Text, CultureInfo.InvariantCulture.NumberFormat) : 0,
                        valor_nota_fiscal = !string.IsNullOrEmpty(txtValorNotaFiscal.Text) ? decimal.Parse(txtValorNotaFiscal.Text, CultureInfo.InvariantCulture.NumberFormat) : 0,
                        qtd_diesel = !string.IsNullOrEmpty(txtQtdDieselPedreira.Text) ? decimal.Parse(txtQtdDieselPedreira.Text, CultureInfo.InvariantCulture.NumberFormat) : 0,
                        km_abastecimento = !string.IsNullOrEmpty(txtKmAbastecimentoPedreira.Text) ? int.Parse(txtKmAbastecimentoPedreira.Text) : 0,
                        valor_abastecimento = !string.IsNullOrEmpty(txtValorAbastecimentoPedreira.Text) ? decimal.Parse(txtValorAbastecimentoPedreira.Text, CultureInfo.InvariantCulture.NumberFormat) : 0,
                        noite = chkNoitePedreira.Checked,
                        domingo_feriado = chkDomingoFeriadoPedreira.Checked
                    });
                    ok = viagem > 0;
                    break;
            }

            ClientScript.RegisterStartupScript(GetType(), "alterou", ok ? "alert('Viagem alterada com sucesso.');window.location.href='viagens.aspx';" : "alert('Ocorreu algum erro ao tentar alterar. Tente novamente.');", true);
        }

        protected void grdViagensPaoAcucar_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void grdViagensPedreira_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void btnAddLojasViagem_Click(object sender, EventArgs e)
        {
            var dt = (DataTable)Session["lojas_viagem"];

            var dr = dt.NewRow();
            dr["id_loja_viagem_pao_acucar"] = dt.Rows.Count > 0
                                            ? Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["id_loja_viagem_pao_acucar"]) + 1
                                            : 1;
            dt.Rows.Add(dr);

            grdLojasViagem.EditIndex = dt.Rows.Count - 1;
            grdLojasViagem.DataSource = dt;
            grdLojasViagem.DataBind();

            ((LinkButton)grdLojasViagem.Rows[dt.Rows.Count - 1].Cells[0].Controls[0]).Text = "Inserir";
        }

        protected void BindLojasViagem()
        {
            var dt = (DataTable)Session["lojas_viagem"];
            dt.DefaultView.Sort = "id_loja_viagem_pao_acucar ASC";
            grdLojasViagem.DataSource = dt;
            grdLojasViagem.DataBind();
        }

        protected void ZerarLojasViagem()
        {
            Session.Remove("lojas_viagem");
            var lojas_viagem = new DataTable();
            lojas_viagem.Columns.Add("id_loja_viagem_pao_acucar", typeof(int));
            lojas_viagem.Columns.Add("id_loja", typeof(int));
            lojas_viagem.Columns.Add("loja", typeof(string));
            lojas_viagem.Columns.Add("valor_frete", typeof(decimal));
            lojas_viagem.Columns.Add("numero_ordem_coleta", typeof(string));
            Session["lojas_viagem"] = lojas_viagem;
            BindLojasViagem();
        }

        protected void btnZerarLojasViagem_Click(object sender, EventArgs e)
        {
            ZerarLojasViagem();
        }

        protected void grdLojasViagem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdLojasViagem.EditIndex = -1;
            BindLojasViagem();
        }

        protected void grdLojasViagem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var id = Convert.ToInt32(grdLojasViagem.DataKeys[e.RowIndex].Value);
            var dt = (DataTable)Session["lojas_viagem"];
            var drr = dt.Select("id_loja_viagem_pao_acucar=" + id);

            if (drr.Length != 1)
            {
                ClientScript.RegisterStartupScript(GetType(), "deletaLojas", "alert('Ocorreu um problema ao tentar excluir a loja. Tente novamente.');", true);
                return;
            }

            drr[0].Delete();
            dt.AcceptChanges();

            BindLojasViagem();
        }

        protected void grdLojasViagem_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdLojasViagem.EditIndex = e.NewEditIndex;
            BindLojasViagem();
        }

        protected string validarLojasViagem(string id_loja, decimal valor_frete, string numero_ordem_coleta)
        {
            var ret = "";

            var loja = new lojasDAL().RecuperarPorID(Convert.ToInt32(id_loja));
            if (loja == null || id_loja == "-1")
                return "Selecione uma loja.\\n";

            var transf = loja.nome.ToLower().Contains("transferência");

            if (!transf && valor_frete <= 0)
                ret += "Preencha o valor do frete.\\n";
            if (transf && string.IsNullOrEmpty(numero_ordem_coleta))
                ret += "Preencha o número da ordem de coleta.\\n";

            return ret;
        }

        protected void grdLojasViagem_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var dt = (DataTable)Session["lojas_viagem"];
            //var inserir = ((LinkButton) grdLojasViagem.Rows[0].Cells[0].Controls[0]).Text == "Inserir";

            int id_loja_viagem = Convert.ToInt32(grdLojasViagem.DataKeys[e.RowIndex].Value);
            var id_loja = ((DropDownList)grdLojasViagem.Rows[e.RowIndex].Cells[1].Controls[1]).SelectedValue;
            var loja = ((DropDownList)grdLojasViagem.Rows[e.RowIndex].Cells[1].Controls[1]).SelectedItem.Text;
            var txtValorFrete = ((TextBox)grdLojasViagem.Rows[e.RowIndex].Cells[2].Controls[1]).Text;
            var valor_frete = Convert.ToDecimal(!String.IsNullOrEmpty(txtValorFrete) ? txtValorFrete : "0", CultureInfo.InvariantCulture.NumberFormat);
            var numero_ordem_coleta = ((TextBox)grdLojasViagem.Rows[e.RowIndex].Cells[3].Controls[1]).Text;

            //validação
            var validacao = validarLojasViagem(id_loja, valor_frete, numero_ordem_coleta);
            if (!string.IsNullOrEmpty(validacao))
            {
                ClientScript.RegisterStartupScript(GetType(), "validaLojasViagem", "alert('" + validacao + "');", true);
                return;
            }

            var drr = dt.Select("id_loja_viagem_pao_acucar=" + id_loja_viagem);

            if (drr.Length != 1)
            {
                ClientScript.RegisterStartupScript(GetType(), "atualizarLojaViagem", "alert('Ocorreu um problema ao tentar editar a loja. Tente novamente.');", true);
                return;
            }

            var dc = drr[0];
            dc["id_loja"] = id_loja;
            dc["loja"] = loja;
            dc["valor_frete"] = valor_frete;
            dc["numero_ordem_coleta"] = numero_ordem_coleta;

            grdLojasViagem.EditIndex = -1;
            BindLojasViagem();
        }

        protected void grdLojasViagem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    var ddlLojas = (DropDownList)e.Row.FindControl("ddlLojas");
                    ddlLojas.DataSource = new lojasDAL().Listar(null);
                    ddlLojas.DataBind();
                    ddlLojas.Items.Insert(0, new ListItem("Selecione", "-1"));
                    ddlLojas.SelectedValue = DataBinder.Eval(e.Row.DataItem, "id_loja").ToString();
                }
                else
                {
                    var lblLoja = (Label)e.Row.FindControl("lblLoja");
                    lblLoja.Text = lblLoja.Text;
                }
            }
        }

        [WebMethod]
        public static object SalvaViagemDestacada(int tipo_viagem, bool adicionar, int id_viagem)
        {
            if (tipo_viagem == 1)
            {
                return adicionar
                           ? new viagens_pao_acucar_destacadasDAL().Adicionar(new viagem_pao_acucar_destacada()
                                                                                  {
                                                                                      id_usuario =
                                                                                          ((usuario)
                                                                                           HttpContext.Current.Session[
                                                                                               "usuario"]).id_usuario,
                                                                                      id_viagem_pao_acucar = id_viagem
                                                                                  }) > 0
                           : new viagens_pao_acucar_destacadasDAL().Deletar(new viagem_pao_acucar_destacada()
                                                                                {
                                                                                    id_usuario =
                                                                                        ((usuario)
                                                                                         HttpContext.Current.Session[
                                                                                             "usuario"]).id_usuario,
                                                                                    id_viagem_pao_acucar = id_viagem
                                                                                }) > 0;
            }

            return adicionar
                       ? new viagens_pedreira_destacadasDAL().Adicionar(new viagem_pedreira_destacada()
                                                                            {
                                                                                id_usuario =
                                                                                    ((usuario)
                                                                                     HttpContext.Current.Session[
                                                                                         "usuario"]).id_usuario,
                                                                                id_viagem_pedreira = id_viagem
                                                                            }) > 0
                       : new viagens_pedreira_destacadasDAL().Deletar(new viagem_pedreira_destacada()
                                                                          {
                                                                              id_usuario =
                                                                                  ((usuario)
                                                                                   HttpContext.Current.Session[
                                                                                       "usuario"]).id_usuario,
                                                                              id_viagem_pedreira = id_viagem
                                                                          }) > 0;
        }
    }
}