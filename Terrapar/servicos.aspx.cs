using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Terrapar.DAL;
using Terrapar.DTO;

namespace Terrapar
{
    public partial class servicos : System.Web.UI.Page
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

            try
            {
                var id = Request["id"];
                //busca
                if (id == null)
                {
                    vwEstados.ActiveViewIndex = 0;
                    PopulaUsuariosComServico();

                    var usu = Request["usu"];
                    var per = Request["per"];
                    if (usu != null && per != null)
                    {
                        ddlUsuarios.SelectedValue = usu;
                        ddlUsuarios_SelectedIndexChanged(null, null);
                        ddlPeriodo.SelectedValue = per;
                        Buscar();
                    }
                }
                else
                {
                    vwEstados.ActiveViewIndex = 1;
                    int idServico;
                    int.TryParse(id, out idServico);

                    //edição
                    PopulaUsuarios();
                    if (idServico != 0)
                    {
                        ddlUsuariosEdit.Enabled = false;
                        btnEditar.Text = "Salvar";
                        var servico = new servicosDAL().RecuperarPorID(idServico);
                        if (servico == null)
                        {
                            ClientScript.RegisterStartupScript(GetType(), "servicoNaoEncontrado", "alert('Serviço não encontrado');window.location.href='servicos.aspx';", true);
                        }
                        else
                        {
                            txtDataEdit.Text = servico.data.ToString();
                            ddlUsuariosEdit.SelectedValue = servico.id_usuario.ToString();
                            txtDescricaoEdit.Text = servico.descricao;
                            if (servico.descricao.ToLower() == "troca de óleo")
                            {
                                chkOleo.Checked = true;
                                txtDescricaoEdit.Enabled = false;
                            }
                            else
                            {
                                chkOleo.Checked = false;
                                txtDescricaoEdit.Enabled = true;
                            }
                            txtValorEdit.Text = servico.valor.ToString();
                            txtKmEdit.Text = servico.km.ToString();
                            ddlTiposEdit.SelectedValue = servico.tipo.ToString();
                        }
                    }
                    else
                    {
                        txtDataEdit.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        ddlUsuariosEdit.Enabled = true;
                        btnEditar.Text = "Salvar";
                    }
                }
            }
            catch
            {
                vwEstados.ActiveViewIndex = 0;
            }
        }

        protected void PopulaUsuariosComServico()
        {
            var dt = new usuariosDAL().ListarComServico();
            ddlUsuarios.DataSource = dt.OrderBy(u => u.placa);
            ddlUsuarios.DataBind();
            ddlUsuarios.Items.Insert(0, new ListItem("Selecione", "-1"));
        }

        protected void PopulaUsuarios()
        {
            var dt = new usuariosDAL().Listar(null);
            ddlUsuariosEdit.DataSource = dt;
            ddlUsuariosEdit.DataBind();
            ddlUsuariosEdit.Items.Insert(0, new ListItem("Selecione", "-1"));
        }

        protected void ddlUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            ddlPeriodo.DataSource =
                new servicosDAL().Listar(new servico() {id_usuario = idUsuario}).Select(p => p.data).OrderBy(
                    v => v).Select(v => v.ToString("MM/yyyy")).Distinct();
            ddlPeriodo.DataBind();
            ddlPeriodo.SelectedIndex = ddlPeriodo.Items.Count - 1;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        protected void lnkDeletar_Click(object sender, EventArgs e)
        {
            var lnkRemove = (LinkButton)sender;
            var ok = new servicosDAL().Deletar(new servico()
            {
                id_servico = int.Parse(lnkRemove.CommandArgument)
            });
            ClientScript.RegisterStartupScript(GetType(), "deletou", ok > 0 ? "alert('Serviço excluído com sucesso.');" : "alert('Ocorreu algum erro ao tentar excluir. Tente novamente.');", true);

            Buscar();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            string validacao = Validar();
            if (!string.IsNullOrEmpty(validacao))
            {
                ClientScript.RegisterStartupScript(GetType(), "validaServico", "alert('" + validacao + "');", true);
                return;
            }

            var id = Request["id"];
            int idServico;
            int.TryParse(id, out idServico);
            var idUsuario = Convert.ToInt32(ddlUsuariosEdit.SelectedValue);

            //novo
            if (idServico == 0)
            {
                var ok = new servicosDAL().Adicionar(new servico()
                {
                    descricao = txtDescricaoEdit.Text,
                    valor = Convert.ToDecimal(txtValorEdit.Text),
                    km = Convert.ToInt32(txtKmEdit.Text),
                    tipo = int.Parse(ddlTiposEdit.SelectedValue),
                    id_usuario = idUsuario,
                    data = Convert.ToDateTime(txtDataEdit.Text)
                });
                ClientScript.RegisterStartupScript(GetType(), "cadastrou", ok > 0 ? "alert('Serviço cadastrado com sucesso.');window.location.href='servicos.aspx';" : "alert('Ocorreu algum erro ao tentar cadastrar. Tente novamente.');", true);
            }
            //editar
            else
            {
                var ok = new servicosDAL().Atualizar(new servico()
                {
                    id_servico = idServico,
                    descricao = txtDescricaoEdit.Text,
                    valor = Convert.ToDecimal(txtValorEdit.Text),
                    km = Convert.ToInt32(txtKmEdit.Text),
                    tipo = int.Parse(ddlTiposEdit.SelectedValue),
                    id_usuario = idUsuario,
                    data = Convert.ToDateTime(txtDataEdit.Text)
                });
                ClientScript.RegisterStartupScript(GetType(), "atualizou", ok > 0 ? "alert('Serviço atualizado com sucesso.');window.location.href='servicos.aspx';" : "alert('Ocorreu algum erro ao tentar atualizar. Tente novamente.');", true);
            }
        }

        protected string Validar()
        {
            var ret = "";
            DateTime data;

            if (!DateTime.TryParse(txtDataEdit.Text, out data) || txtDataEdit.Text.Length != 10)
                ret += "Preencha a data.\\n";
            if (string.IsNullOrEmpty(ddlUsuariosEdit.SelectedValue))
                ret += "Preencha a placa.\\n";
            if (string.IsNullOrEmpty(txtDescricaoEdit.Text))
                ret += "Preencha a descrição.\\n";
            if (string.IsNullOrEmpty(txtValorEdit.Text) || (Convert.ToDecimal(txtValorEdit.Text) <= 0))
                ret += "Preencha o valor do serviço.\\n";
            if (string.IsNullOrEmpty(txtKmEdit.Text) || (Convert.ToDecimal(txtKmEdit.Text) <= 0))
                ret += "Preencha o Km do serviço.\\n";
            if (string.IsNullOrEmpty(ddlTiposEdit.SelectedValue))
                ret += "Preencha o tipo.\\n";
            return ret;
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

        protected void Buscar()
        {
            string validacao = ValidarBuscar();
            if (!string.IsNullOrEmpty(validacao))
            {
                ClientScript.RegisterStartupScript(GetType(), "validaBuscaServicos", "alert('" + validacao + "');", true);
                return;
            }

            var idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            grdServicos.DataSource = new servicosDAL().Listar(new servico() { id_usuario = idUsuario, data = Convert.ToDateTime("01/" + ddlPeriodo.Text) });
            grdServicos.DataBind();
        }

        protected void chkOleo_CheckedChanged(object sender, EventArgs e)
        {
            if(chkOleo.Checked)
            {
                txtDescricaoEdit.Text = "Troca de óleo";
                txtDescricaoEdit.Enabled = false;
            }
            else
            {
                txtDescricaoEdit.Text = "";
                txtDescricaoEdit.Enabled = true;
            }
        }
    }
}