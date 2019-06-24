using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Terrapar.DAL;
using Terrapar.DTO;

namespace Terrapar
{
    public partial class folha_pagamento : Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //verifica se usuário logado é administrador
            if (!((usuario)Session["usuario"]).admin)
            {
                ClientScript.RegisterStartupScript(GetType(), "usuSemPermissao", "alert('Usuário não tem permissão para acessar a página requisitada.');window.location.href='default.aspx';", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            ddlUsuarios.DataSource = new usuariosDAL().ListarComViagem().OrderBy(u => u.placa);
            ddlUsuarios.DataBind();
            ddlUsuarios.Items.Insert(0, new ListItem("Selecione", "-1"));
        }

        protected void ddlUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlFolha.Visible = false;
            var idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            var tipoPlaca = new usuariosDAL().RecuperarPorID(idUsuario);

            if (tipoPlaca == null)
            {
                ddlPeriodo.Items.Clear();
                pnlFechamentoPaoAcucar.Visible = false;
                pnlFechamentoPedreira.Visible = false;
                pnlFolha.Visible = false;
                pnlFolhaPaoAcucar.Visible = false;
                pnlFolhaPedreira.Visible = false;
                return;
            }

            ddlPeriodo.DataSource = tipoPlaca.tipo == 0
                                        ? new viagens_pao_acucarDAL().Listar(new viagem_pao_acucar() { id_usuario = idUsuario }).Select(p => p.data).OrderBy(v => v).Select(v => v.ToString("MM/yyyy")).Distinct()
                                        : new viagens_pedreiraDAL().Listar(new viagem_pedreira() { id_usuario = idUsuario }).Select(p => p.data).OrderBy(v => v).Select(v => v.ToString("MM/yyyy")).Distinct();
            ddlPeriodo.DataBind();
            ddlPeriodo.SelectedIndex = ddlPeriodo.Items.Count - 1;
        }

        protected void ddlPeriodo_SelecteIndexChanged(object sender, EventArgs e)
        {
            pnlFolha.Visible = false;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            pnlFechamentoPaoAcucar.Visible = pnlFechamentoPedreira.Visible = pnlFolha.Visible = false;

            string validacao = Validar();
            if (!string.IsNullOrEmpty(validacao))
            {
                ClientScript.RegisterStartupScript(GetType(), "validaBuscaFolhaPgto", "alert('" + validacao + "');", true);
                return;
            }

            var idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            var usuario = new usuariosDAL().RecuperarPorID(idUsuario);
            var parametrosGerais = new parametros_geraisDAL().RecuperarPorID(1);
            if (usuario.tipo == 0)
            {
                //pão de açúcar
                var fechamentoPaoAcucar =
                    new fechamentos_pao_acucarDAL().Listar(new fechamento_pao_acucar()
                                                               {
                                                                   id_usuario = idUsuario,
                                                                   data = Convert.ToDateTime("01/" + ddlPeriodo.Text)
                                                               }).
                        SingleOrDefault();

                txtQtdHorasExtras.Text = fechamentoPaoAcucar != null ? fechamentoPaoAcucar.qtd_horas_extras.ToString() : Convert.ToDecimal("52,00").ToString();
                txtQtdDiarias.Text = fechamentoPaoAcucar != null ? fechamentoPaoAcucar.qtd_diarias.ToString() : Convert.ToDecimal("26,00").ToString();
                txtValorHoraExtra.Text = fechamentoPaoAcucar != null
                                             ? fechamentoPaoAcucar.valor_hora_extra_pao_acucar.ToString()
                                             : parametrosGerais.valor_hora_extra_pao_acucar.ToString();
                txtValorDiaria.Text = fechamentoPaoAcucar != null
                                             ? fechamentoPaoAcucar.valor_diaria_pao_acucar.ToString()
                                             : parametrosGerais.valor_diaria_pao_acucar.ToString();
                txtSalarioPaoAcucar.Text = fechamentoPaoAcucar != null
                                             ? fechamentoPaoAcucar.salario_pao_acucar.ToString()
                                             : parametrosGerais.salario_pao_acucar.ToString();
                txtValorValePaoAcucar.Text = fechamentoPaoAcucar != null
                                             ? fechamentoPaoAcucar.valor_vale.ToString()
                                             : parametrosGerais.valor_vale.ToString();
                txtINSSPaoAcucar.Text = fechamentoPaoAcucar != null
                                             ? fechamentoPaoAcucar.inss_pao_acucar.ToString()
                                             : parametrosGerais.inss_pao_acucar.ToString();
                pnlFechamentoPaoAcucar.Visible = true;
                pnlFechamentoPedreira.Visible = false;
            }
            else
            {
                //pedreira
                var fechamentoPedreira = new fechamentos_pedreiraDAL().Listar(new fechamento_pedreira() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) }).SingleOrDefault();

                txtValorRefeicao.Text = fechamentoPedreira != null
                                             ? fechamentoPedreira.valor_refeicao.ToString()
                                             : parametrosGerais.valor_refeicao.ToString();
                txtSalarioPedreira.Text = fechamentoPedreira != null
                                             ? fechamentoPedreira.salario_pedreira.ToString()
                                             : parametrosGerais.salario_pedreira.ToString();
                txtValorValePedreira.Text = fechamentoPedreira != null
                                             ? fechamentoPedreira.valor_vale.ToString()
                                             : parametrosGerais.valor_vale.ToString();
                txtINSSPedreira.Text = fechamentoPedreira != null
                                             ? fechamentoPedreira.inss_pedreira.ToString()
                                             : parametrosGerais.inss_pedreira.ToString();
                pnlFechamentoPedreira.Visible = true;
                pnlFechamentoPaoAcucar.Visible = false;
            }

            grdDebitosCreditos.EditIndex = -1;
            BindDebitosCreditos();

            CalcularFolha();
            pnlFolha.Visible = true;
        }

        protected string Validar()
        {
            var ret = "";

            if (ddlUsuarios.SelectedValue == "-1")
                ret += "Selecione uma placa.\\n";
            if (ddlPeriodo.SelectedValue == "-1" || ddlPeriodo.SelectedValue == "")
                ret += "Selecione um período.\\n";
            
            return ret;
        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            CalcularFolha();
            ClientScript.RegisterStartupScript(GetType(), "atualizaFechamento", "alert('Atualizado com sucesso.');", true);
        }

        protected void CalcularFolha()
        {
            var idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            var usuario = new usuariosDAL().RecuperarPorID(idUsuario);
            if (usuario.tipo == 0)
            {
                #region pão de açúcar
                //salva ou atualiza os dados do fechamento
                var fechamento = new fechamentos_pao_acucarDAL();
                var fechamentoNovoEdit = new fechamento_pao_acucar()
                                             {
                                                 qtd_horas_extras =
                                                     !string.IsNullOrEmpty(txtQtdHorasExtras.Text)
                                                         ? Convert.ToDecimal(txtQtdHorasExtras.Text)
                                                         : 0,
                                                 qtd_diarias =
                                                     !string.IsNullOrEmpty(txtQtdDiarias.Text)
                                                         ? Convert.ToDecimal(txtQtdDiarias.Text)
                                                         : 0,
                                                 valor_hora_extra_pao_acucar =
                                                     !string.IsNullOrEmpty(txtValorHoraExtra.Text)
                                                         ? Convert.ToDecimal(txtValorHoraExtra.Text)
                                                         : 0,
                                                 valor_diaria_pao_acucar =
                                                     !string.IsNullOrEmpty(txtValorDiaria.Text)
                                                         ? Convert.ToDecimal(txtValorDiaria.Text)
                                                         : 0,
                                                 salario_pao_acucar =
                                                     !string.IsNullOrEmpty(txtSalarioPaoAcucar.Text)
                                                         ? Convert.ToDecimal(txtSalarioPaoAcucar.Text)
                                                         : 0,
                                                 valor_vale =
                                                     !string.IsNullOrEmpty(txtValorValePaoAcucar.Text)
                                                         ? Convert.ToDecimal(txtValorValePaoAcucar.Text)
                                                         : 0,
                                                 inss_pao_acucar =
                                                     !string.IsNullOrEmpty(txtINSSPaoAcucar.Text)
                                                         ? Convert.ToDecimal(txtINSSPaoAcucar.Text)
                                                         : 0,
                                                 id_usuario = idUsuario,
                                                 data = Convert.ToDateTime("01/" + ddlPeriodo.Text)
                                             };

                if(fechamento.Listar(new fechamento_pao_acucar() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) }).Count()>0)
                {
                    fechamento.Atualizar(fechamentoNovoEdit);
                }
                else
                {
                    fechamento.Adicionar(fechamentoNovoEdit);
                }

                //viagens
                var viagensPaoAcucar = new viagens_pao_acucarDAL().Listar(new viagem_pao_acucar() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) });
                var qtdViagens = viagensPaoAcucar.Count();
                var qtdHorasExtras = string.IsNullOrEmpty(txtQtdHorasExtras.Text) ? 0 : Convert.ToDecimal(txtQtdHorasExtras.Text);
                var qtdDiarias = string.IsNullOrEmpty(txtQtdDiarias.Text) ? 0 : Convert.ToDecimal(txtQtdDiarias.Text);
                var valorHoraExtra = string.IsNullOrEmpty(txtValorHoraExtra.Text) ? 0 : Convert.ToDecimal(txtValorHoraExtra.Text);
                var valorDiaria = string.IsNullOrEmpty(txtValorDiaria.Text) ? 0 : Convert.ToDecimal(txtValorDiaria.Text);
                var salarioPaoAcucar = string.IsNullOrEmpty(txtSalarioPaoAcucar.Text) ? 0 : Convert.ToDecimal(txtSalarioPaoAcucar.Text);
                var valorVale = string.IsNullOrEmpty(txtValorValePaoAcucar.Text) ? 0 : Convert.ToDecimal(txtValorValePaoAcucar.Text);
                var INSSPaoAcucar = string.IsNullOrEmpty(txtINSSPaoAcucar.Text) ? 0 : Convert.ToDecimal(txtINSSPaoAcucar.Text);
                var totalFrete = new lojas_viagem_pao_acucarDAL().Listar(null).Join(viagensPaoAcucar, c => c.id_viagem_pao_acucar, v => v.id_viagem_pao_acucar, (c, v) => new { c.valor_frete }).Sum(c => c.valor_frete);
                var domingo70reais = viagensPaoAcucar.Where(v => v.domingo).Count()*70;
                var excedenteMetaFrete = totalFrete - usuario.meta_comissao;

                //débitos e créditos
                var dc = new debitos_creditosDAL().Listar(new debito_credito() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) });

                //resultados calculados
                var valorHorasExtras = qtdHorasExtras * valorHoraExtra;
                var valorDiarias = qtdDiarias * valorDiaria;
                var comissao = excedenteMetaFrete > 0 ? (excedenteMetaFrete * (decimal)0.1) : 0;
                var creditos = dc.Where(c => c.tipo == 1).Sum(c => c.valor);
                var total1 = salarioPaoAcucar + valorHorasExtras + valorDiarias + domingo70reais + comissao +
                             creditos;
                var debitos = dc.Where(d => d.tipo == 0).Sum(d => d.valor);
                var total2 = valorVale + INSSPaoAcucar + debitos;

                lblMotorista.Text = "Folha de pagamento Pão de açúcar - " + usuario.placa;
                lblSalarioPaoAcucar.Text = salarioPaoAcucar.ToString("N2");
                lblHorasExtrasPaoAcucar.Text = valorHorasExtras.ToString("N2");
                lblDiariasPaoAcucar.Text = valorDiarias.ToString("N2");
                lblDomingosPaoAcucar.Text = domingo70reais.ToString("N2");
                lblComissao10PaoAcucar.Text = comissao.ToString("N2");
                lblOutrosCreditosPaoAcucar.Text = creditos.ToString("N2");
                lblTotal1PaoAcucar.Text = total1.ToString("N2");
                lblValePaoAcucar.Text = valorVale.ToString("N2");
                lblINSSPaoAcucar.Text = INSSPaoAcucar.ToString("N2");
                lblOutrosDebitosPaoAcucar.Text = debitos.ToString("N2");
                lblTotal2PaoAcucar.Text = total2.ToString("N2");
                lblTotalGeralPaoAcucar.Text = (total1 - total2).ToString("N2");

                pnlFolhaPaoAcucar.Visible = true;
                pnlFolhaPedreira.Visible = false;
                #endregion
            }
            else
            {
                #region pedreira
                //salva ou atualiza os dados do fechamento
                var fechamento = new fechamentos_pedreiraDAL();
                var fechamentoNovoEdit = new fechamento_pedreira()
                {
                    valor_refeicao =
                        !string.IsNullOrEmpty(txtValorRefeicao.Text)
                            ? Convert.ToDecimal(txtValorRefeicao.Text)
                            : 0,
                    salario_pedreira = 
                        !string.IsNullOrEmpty(txtSalarioPedreira.Text)
                            ? Convert.ToDecimal(txtSalarioPedreira.Text)
                            : 0,
                    valor_vale =
                        !string.IsNullOrEmpty(txtValorValePedreira.Text)
                            ? Convert.ToDecimal(txtValorValePedreira.Text)
                            : 0,
                    inss_pedreira = 
                        !string.IsNullOrEmpty(txtINSSPedreira.Text)
                            ? Convert.ToDecimal(txtINSSPedreira.Text)
                            : 0,
                    id_usuario = idUsuario,
                    data = Convert.ToDateTime("01/" + ddlPeriodo.Text)
                };

                if (fechamento.Listar(new fechamento_pedreira() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) }).Count() > 0)
                {
                    fechamento.Atualizar(fechamentoNovoEdit);
                }
                else
                {
                    fechamento.Adicionar(fechamentoNovoEdit);
                }

                //viagens
                var viagensPedreira = new viagens_pedreiraDAL().ListarParaFolhaPgto(new viagem_pedreira() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) });
                var qtdViagens = viagensPedreira.Sum(v => v.qtd_viagens);
                var qtdRefeicoes = viagensPedreira.Where(vp => !vp.domingo_feriado).Count();
                var valorRefeicao = string.IsNullOrEmpty(txtValorRefeicao.Text) ? 0 : Convert.ToDecimal(txtValorRefeicao.Text);
                var salarioPedreira = string.IsNullOrEmpty(txtSalarioPedreira.Text) ? 0 : Convert.ToDecimal(txtSalarioPedreira.Text);
                var valorVale = string.IsNullOrEmpty(txtValorValePedreira.Text) ? 0 : Convert.ToDecimal(txtValorValePedreira.Text);
                var INSSPedreira = string.IsNullOrEmpty(txtINSSPedreira.Text) ? 0 : Convert.ToDecimal(txtINSSPedreira.Text);
                var totalFrete = viagensPedreira.Sum(v => v.total_frete);
                var excedenteMetaFrete = totalFrete - usuario.meta_comissao;

                //débitos e créditos
                var dc = new debitos_creditosDAL().Listar(new debito_credito() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) });

                //resultados calculados
                //var comissao7ou15 = totalFrete * (decimal)0.07;
                var comissao7ou15 = viagensPedreira.Sum(v => v.total_frete * (v.noite || v.domingo_feriado ? (decimal)0.15 : (decimal)0.07));
                var refeicao = qtdRefeicoes * valorRefeicao;
                var comissao5 = excedenteMetaFrete > 0 ? (excedenteMetaFrete * (decimal)0.05) : 0;
                var creditos = dc.Where(c => c.tipo == 1).Sum(c => c.valor);
                var total1 = salarioPedreira + comissao7ou15 + refeicao + comissao5 + creditos;
                var debitos = dc.Where(d => d.tipo == 0).Sum(d => d.valor);
                var total2 = valorVale + INSSPedreira + debitos;

                lblMotorista.Text = "Folha de pagamento Pedreira - " + usuario.placa;
                lblSalarioPedreira.Text = salarioPedreira.ToString("N2");
                lblComissaoPedreira.Text = comissao7ou15.ToString("N2");
                lblRefeicaoPedreira.Text = refeicao.ToString("N2");
                lblComissao5Pedreira.Text = comissao5.ToString("N2");
                lblOutrosCreditosPedreira.Text = creditos.ToString("N2");
                lblTotal1Pedreira.Text = total1.ToString("N2");
                lblValePedreira.Text = valorVale.ToString("N2");
                lblINSSPedreira.Text = INSSPedreira.ToString("N2");
                lblOutrosDebitosPedreira.Text = debitos.ToString("N2");
                lblTotal2Pedreira.Text = total2.ToString("N2");
                lblTotalGeralPedreira.Text = (total1 - total2).ToString("N2");

                pnlFolhaPedreira.Visible = true;
                pnlFolhaPaoAcucar.Visible = false;
                #endregion
            }

            hplViagensUsuPer.NavigateUrl = "viagens.aspx?usu=" + ddlUsuarios.SelectedValue + "&per=" +
                                           ddlPeriodo.SelectedValue;
        }

        protected void BindDebitosCreditos()
        {
            var idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            var dc = new debitos_creditosDAL().Listar(new debito_credito() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) });
            grdDebitosCreditos.DataSource = dc;
            grdDebitosCreditos.DataBind();
        }

        protected void grdDebitosCreditos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdDebitosCreditos.EditIndex = e.NewEditIndex;
            BindDebitosCreditos();
        }

        protected void grdDebitosCreditos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdDebitosCreditos.EditIndex = -1;
            BindDebitosCreditos();
        }

        protected string validarDebitoCredito(string descricao, string valor)
        {
            var ret = "";

            if (string.IsNullOrEmpty(descricao))
                ret += "Preencha a descrição.\\n";
            if (string.IsNullOrEmpty(valor) || (Convert.ToDecimal(valor) <= 0))
                ret += "Preencha o valor.\\n";

            return ret;
        }

        protected void grdDebitosCreditos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            var dc = new debitos_creditosDAL();
            if (((LinkButton)grdDebitosCreditos.Rows[0].Cells[0].Controls[0]).Text == "Inserir")
            {
                var tipo = ((DropDownList)grdDebitosCreditos.Rows[0].Cells[1].Controls[1]).SelectedValue;
                var descricao = ((TextBox)grdDebitosCreditos.Rows[0].Cells[2].Controls[1]).Text;
                var valor = ((TextBox)grdDebitosCreditos.Rows[0].Cells[3].Controls[1]).Text;

                //validação
                var validacao = validarDebitoCredito(descricao, valor);
                if (!string.IsNullOrEmpty(validacao))
                {
                    ClientScript.RegisterStartupScript(GetType(), "validaDebitoCreditoFolhaPgto", "alert('" + validacao + "');", true);
                    return;
                }

                dc.Adicionar(new debito_credito()
                {
                    tipo = Convert.ToInt32(tipo),
                    descricao = descricao,
                    valor = Convert.ToDecimal(valor),
                    id_usuario = idUsuario,
                    data = Convert.ToDateTime("01/" + ddlPeriodo.Text)
                });
            }

            else
            {
                int id = (int)grdDebitosCreditos.DataKeys[e.RowIndex].Value;
                var tipo = ((DropDownList)grdDebitosCreditos.Rows[e.RowIndex].Cells[1].Controls[1]).SelectedValue;
                var descricao = ((TextBox)grdDebitosCreditos.Rows[e.RowIndex].Cells[2].Controls[1]).Text;
                var valor = ((TextBox)grdDebitosCreditos.Rows[e.RowIndex].Cells[3].Controls[1]).Text;

                //validação
                var validacao = validarDebitoCredito(descricao, valor);
                if (!string.IsNullOrEmpty(validacao))
                {
                    ClientScript.RegisterStartupScript(GetType(), "validaDebitoCreditoFolhaPgto", "alert('" + validacao + "');", true);
                    return;
                }

                dc.Atualizar(new debito_credito()
                {
                    id_debito_credito = id,
                    tipo = Convert.ToInt32(tipo),
                    descricao = descricao,
                    valor = Convert.ToDecimal(valor),
                    id_usuario = idUsuario,
                    data = Convert.ToDateTime("01/" + ddlPeriodo.Text)
                });
            }

            grdDebitosCreditos.EditIndex = -1;
            BindDebitosCreditos();
            CalcularFolha();
        }

        protected void grdDebitosCreditos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = (int)grdDebitosCreditos.DataKeys[e.RowIndex].Value;
            new debitos_creditosDAL().Deletar(new debito_credito() { id_debito_credito = id });

            BindDebitosCreditos();
            CalcularFolha();
        }

        protected void grdDebitosCreditos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    var ddlTipos = (DropDownList)e.Row.FindControl("ddlTipos");
                    ddlTipos.SelectedValue = DataBinder.Eval(e.Row.DataItem, "tipo").ToString();
                }
                else
                {
                    var lblTipo = (Label)e.Row.FindControl("lblTipo");
                    lblTipo.Text = lblTipo.Text == "1" ? "Crédito" : "Débito";
                }
            }
        }

        protected void btnAddDebitoCredito_Click(object sender, EventArgs e)
        {
            var idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            var dt = new debitos_creditosDAL().ListarParaDatatable(new debito_credito() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) });
            
            var dr = dt.NewRow();
            dt.Rows.InsertAt(dr, 0);

            grdDebitosCreditos.EditIndex = 0;
            grdDebitosCreditos.DataSource = dt;
            grdDebitosCreditos.DataBind();

            ((LinkButton)grdDebitosCreditos.Rows[0].Cells[0].Controls[0]).Text = "Inserir";
        }
    }
}