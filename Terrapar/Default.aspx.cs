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
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            ZerarLojasViagem();

            txtDataPedreira.Text = txtDataPaoAcucar.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");

            var idUsuario = Convert.ToInt32(((usuario)Session["usuario"]).id_usuario);
            var tipoPlaca = new usuariosDAL().RecuperarPorID(idUsuario).tipo;
            rdlViagens.SelectedIndex = vwViagens.ActiveViewIndex = tipoPlaca;
        }

        protected void btnInserir_Click(object sender, EventArgs e)
        {
            string validacao = Validar();
            if (!string.IsNullOrEmpty(validacao))
            {
                ClientScript.RegisterStartupScript(GetType(), "validaViagem", "alert('" + validacao + "');", true);
                return;
            }

            long viagem = 0, lojas_viagem = 0;
            bool ok = false;
            switch (vwViagens.ActiveViewIndex)
            {
                case 0: //pão de açúcar
                    viagem = new viagens_pao_acucarDAL().Adicionar(new viagem_pao_acucar()
                                                             {
                                                                 id_usuario = ((usuario)Session["usuario"]).id_usuario,
                                                                 data = DateTime.Parse(txtDataPaoAcucar.Text),
                                                                 km_final = int.Parse(txtKmFinal.Text),
                                                                 qtd_diesel = !string.IsNullOrEmpty(txtQtdDieselPaoAcucar.Text) ? decimal.Parse(txtQtdDieselPaoAcucar.Text, CultureInfo.InvariantCulture.NumberFormat) : 0,
                                                                 km_abastecimento = !string.IsNullOrEmpty(txtKmAbastecimentoPaoAcucar.Text) ? int.Parse(txtKmAbastecimentoPaoAcucar.Text) : 0,
                                                                 valor_abastecimento = !string.IsNullOrEmpty(txtValorAbastecimentoPaoAcucar.Text) ? decimal.Parse(txtValorAbastecimentoPaoAcucar.Text, CultureInfo.InvariantCulture.NumberFormat) : 0,
                                                                 domingo = chkDomingoPaoAcucar.Checked
                                                             });


                    var dt = (DataTable)Session["lojas_viagem"];
                    foreach (DataRow r in dt.Rows)
                    {
                        lojas_viagem = new lojas_viagem_pao_acucarDAL().Adicionar(new loja_viagem_pao_acucar()
                                                                      {
                                                                          id_viagem_pao_acucar = (int)viagem,
                                                                          id_loja = Convert.ToInt32(r["id_loja"]),
                                                                          valor_frete = !string.IsNullOrEmpty(r["valor_frete"].ToString()) ? Convert.ToDecimal(r["valor_frete"], CultureInfo.InvariantCulture.NumberFormat) : 0,
                                                                          numero_ordem_coleta = r["numero_ordem_coleta"].ToString()
                                                                      });
                    }
                    ok = viagem > 0 && lojas_viagem > 0;
                    break;
                case 1: //pedreira
                    viagem = new viagens_pedreiraDAL().Adicionar(new viagem_pedreira()
                                                            {
                                                                id_usuario = ((usuario)Session["usuario"]).id_usuario,
                                                                data = DateTime.Parse(txtDataPedreira.Text),
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

            ClientScript.RegisterStartupScript(GetType(), "cadastrou", ok ? "alert('Viagem cadastrada com sucesso.');" : "alert('Ocorreu algum erro ao tentar cadastrar. Tente novamente.');", true);
            btnLimpar_Click(null, null);
        }

        protected string Validar()
        {
            var ret = "";
            DateTime data;

            switch (vwViagens.ActiveViewIndex)
            {
                case 0: //pão de açúcar
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
                case 1: //pedreira
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

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            switch (vwViagens.ActiveViewIndex)
            {
                case 0: //pão de açúcar
                    txtDataPaoAcucar.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    txtKmFinal.Text =
                        txtQtdDieselPaoAcucar.Text =
                        txtKmAbastecimentoPaoAcucar.Text =
                        txtValorAbastecimentoPaoAcucar.Text = "";
                    chkDomingoPaoAcucar.Checked = false;
                    ZerarLojasViagem();
                    break;
                case 1: //pedreira
                    txtDataPedreira.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    txtOrigem.Text =
                        txtDestino.Text =
                        txtNumeroNotaFiscal.Text =
                        txtPeso.Text =
                        txtValorFretePedreira.Text =
                        txtBaseCalculoICMS.Text =
                        txtValorICMS.Text =
                        txtValorProdutos.Text =
                        txtValorNotaFiscal.Text =
                        txtQtdDieselPedreira.Text =
                        txtKmAbastecimentoPedreira.Text = txtValorAbastecimentoPedreira.Text = "";
                    chkNoitePedreira.Checked = chkDomingoFeriadoPedreira.Checked = false;
                    break;
            }
            lblTrocaOleo.Visible = false;
            lblRendimentoPaoAcucar.Text =
                lblRendimentoPedreira.Text = "Rendimento: ainda não é possível calcular";
        }

        protected void rdlViagens_TextChanged(object sender, EventArgs e)
        {
            vwViagens.ActiveViewIndex = rdlViagens.SelectedIndex;
        }

        [WebMethod]
        public static object CalculaRendimentoeTrocaOleoPaoAcucar(string KmAbastecimento, string QtdDiesel)
        {
            var usu = (usuario)HttpContext.Current.Session["usuario"];
            decimal txtKmAbastecimento, txtQtdDiesel;
            Decimal.TryParse(KmAbastecimento, out txtKmAbastecimento);
            Decimal.TryParse(QtdDiesel, out txtQtdDiesel);

            var trocaIdeal = usu.troca_oleo;
            var trocas = new servicosDAL().Listar(new servico() { id_usuario = usu.id_usuario, descricao = "Troca de óleo" });
            var ultimaTroca = trocas.Count() > 0 ? trocas.OrderByDescending(u => u.data).First().km : 0;

            var viagens = new viagens_pao_acucarDAL().Listar(new viagem_pao_acucar() { id_usuario = usu.id_usuario });
            var ultimaViagem = viagens.Count() > 0 ? viagens.Where(v => v.km_abastecimento > 0).OrderByDescending(u => u.data).FirstOrDefault() : null;

            var trocaOleo = txtKmAbastecimento >= (ultimaTroca + trocaIdeal);
            var rendimentoDesc = "KM abastecimento da última viagem: " + (ultimaViagem != null
                                                                              ? ultimaViagem.
                                                                                    km_abastecimento.
                                                                                    ToString()
                                                                              : "não há última viagem");
            var rendimento = string.Format("Rendimento: {0}",
                                                        ultimaViagem != null && txtQtdDiesel > 0 &&
                                                        txtKmAbastecimento > 0
                                                            ? ((txtKmAbastecimento - ultimaViagem.km_abastecimento) /
                                                               txtQtdDiesel).ToString("N2") + " KMs/litro"
                                                            : "ainda não é possível calcular");

            return new
            {
                trocaOleo,
                rendimentoDesc,
                rendimento
            };
        }

        [WebMethod]
        public static object CalculaRendimentoeTrocaOleoPedreira(string KmAbastecimento, string QtdDiesel)
        {
            var usu = (usuario)HttpContext.Current.Session["usuario"];
            decimal txtKmAbastecimento, txtQtdDiesel;
            Decimal.TryParse(KmAbastecimento, out txtKmAbastecimento);
            Decimal.TryParse(QtdDiesel, out txtQtdDiesel);

            var trocaIdeal = usu.troca_oleo;
            var trocas = new servicosDAL().Listar(new servico() { id_usuario = usu.id_usuario, descricao = "Troca de óleo" });
            var ultimaTroca = trocas.Count() > 0 ? trocas.OrderByDescending(u => u.data).First().km : 0;

            var viagens = new viagens_pedreiraDAL().Listar(new viagem_pedreira() { id_usuario = usu.id_usuario });
            var ultimaViagem = viagens.Count() > 0 ? viagens.Where(v => v.km_abastecimento > 0).OrderByDescending(u => u.data).FirstOrDefault() : null;

            var trocaOleo = txtKmAbastecimento >= (ultimaTroca + trocaIdeal);
            var rendimentoDesc = "KM abastecimento da última viagem: " + (ultimaViagem != null
                                                                              ? ultimaViagem.
                                                                                    km_abastecimento.
                                                                                    ToString()
                                                                              : "não há última viagem");
            var rendimento = string.Format("Rendimento: {0}",
                                                       ultimaViagem != null && txtQtdDiesel > 0 &&
                                                       txtKmAbastecimento > 0
                                                           ? ((txtKmAbastecimento - ultimaViagem.km_abastecimento)/
                                                              txtQtdDiesel).ToString("N2") + " KMs/litro"
                                                           : "ainda não é possível calcular");

            return new
            {
                trocaOleo,
                rendimentoDesc,
                rendimento
            };
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

        protected void grdLojasViagem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //var id = Convert.ToInt32(grdLojasViagem.DataKeys[e.RowIndex].Value);
            //var dt = (DataTable)Session["lojas_viagem"];
            //var drr = dt.Select("id_loja_viagem_pao_acucar=" + id);

            //if (drr.Length != 1)
            //{
            //    ClientScript.RegisterStartupScript(GetType(), "deletaLojaViagem", "alert('Ocorreu um problema ao tentar excluir a loja. Tente novamente.');", true);
            //    return;
            //}

            //drr[0].Delete();
            //dt.AcceptChanges();

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
            var txtValorFrete = ((TextBox) grdLojasViagem.Rows[e.RowIndex].Cells[2].Controls[1]).Text;
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
    }
}
