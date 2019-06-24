using System;
using Terrapar.DAL;
using Terrapar.DTO;

namespace Terrapar
{
    public partial class parametros_gerais : System.Web.UI.Page
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
                    Buscar();
                }
                else
                {
                    vwEstados.ActiveViewIndex = 1;
                    int idParametro;
                    int.TryParse(id, out idParametro);

                    //edição
                    if (idParametro != 0)
                    {
                        btnEditar.Text = "Editar";
                        var parametro = new parametros_geraisDAL().RecuperarPorID(idParametro);
                        if (parametro == null)
                        {
                            ClientScript.RegisterStartupScript(GetType(), "parametroNaoEncontrado",
                                                               "alert('Parâmetro geral não encontrada');window.location.href='parametros-gerais.aspx';",
                                                               true);
                        }
                        else
                        {
                            txtValorRefeicaoEdit.Text = parametro.valor_refeicao.ToString();
                            txtValorValeEdit.Text = parametro.valor_vale.ToString();
                            txtSalarioPaoAcucarEdit.Text = parametro.salario_pao_acucar.ToString();
                            txtValorHoraExtraPaoAcucarEdit.Text = parametro.valor_hora_extra_pao_acucar.ToString();
                            txtValorDiariaPaoAcucarEdit.Text = parametro.valor_diaria_pao_acucar.ToString();
                            txtInssPaoAcucarEdit.Text = parametro.inss_pao_acucar.ToString();
                            txtSalarioPedreiraEdit.Text = parametro.salario_pedreira.ToString();
                            txtInssPedreiraEdit.Text = parametro.inss_pedreira.ToString();
                        }
                    }
                    else
                    {
                        btnEditar.Text = "Cadastrar";
                    }
                }
            }
            catch
            {
                vwEstados.ActiveViewIndex = 0;
            }


        }

        protected void Buscar()
        {
            grdParametrosGerais.DataSource =
                new parametros_geraisDAL().Listar(new parametro_geral());
            grdParametrosGerais.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            string validacao = Validar();
            if (!string.IsNullOrEmpty(validacao))
            {
                ClientScript.RegisterStartupScript(GetType(), "validaParametro", "alert('" + validacao + "');", true);
                return;
            }

            var id = Request["id"];
            int idParametro;
            int.TryParse(id, out idParametro);

            //editar
            if (idParametro != 0)
            {
                var ok = new parametros_geraisDAL().Atualizar(new parametro_geral()
                {
                    id_parametro = idParametro,
                    valor_refeicao = decimal.Parse(txtValorRefeicaoEdit.Text),
                    valor_vale = decimal.Parse(txtValorValeEdit.Text),
                    salario_pao_acucar = decimal.Parse(txtSalarioPaoAcucarEdit.Text),
                    valor_hora_extra_pao_acucar = decimal.Parse(txtValorHoraExtraPaoAcucarEdit.Text),
                    valor_diaria_pao_acucar = decimal.Parse(txtValorDiariaPaoAcucarEdit.Text),
                    inss_pao_acucar = decimal.Parse(txtInssPaoAcucarEdit.Text),
                    salario_pedreira = decimal.Parse(txtSalarioPedreiraEdit.Text),
                    inss_pedreira = decimal.Parse(txtInssPedreiraEdit.Text)
                });
                ClientScript.RegisterStartupScript(GetType(), "atualizou",
                                                   ok > 0
                                                       ? "alert('Parâmetro geral atualizado com sucesso.');window.location.href='parametros-gerais.aspx';"
                                                       : "alert('Ocorreu algum erro ao tentar atualizar. Tente novamente.');",
                                                   true);
            }
        }

        protected string Validar()
        {
            var ret = "";
            if (string.IsNullOrEmpty(txtValorRefeicaoEdit.Text) || (Convert.ToDecimal(txtValorRefeicaoEdit.Text) <= 0))
                ret += "Preencha o valor da refeição.\\n";
            if (string.IsNullOrEmpty(txtValorValeEdit.Text) || (Convert.ToDecimal(txtValorValeEdit.Text) <= 0))
                ret += "Preencha o valor do vale.\\n";
            if (string.IsNullOrEmpty(txtSalarioPaoAcucarEdit.Text) || (Convert.ToDecimal(txtSalarioPaoAcucarEdit.Text) <= 0))
                ret += "Preencha o salário Pão de Açúcar.\\n";
            if (string.IsNullOrEmpty(txtValorHoraExtraPaoAcucarEdit.Text) || (Convert.ToDecimal(txtValorHoraExtraPaoAcucarEdit.Text) <= 0))
                ret += "Preencha o valor da hora extra Pão de Açúcar.\\n";
            if (string.IsNullOrEmpty(txtValorDiariaPaoAcucarEdit.Text) || (Convert.ToDecimal(txtValorDiariaPaoAcucarEdit.Text) <= 0))
                ret += "Preencha o valor da diária Pão de Açúcar.\\n";
            if (string.IsNullOrEmpty(txtInssPaoAcucarEdit.Text) || (Convert.ToDecimal(txtInssPaoAcucarEdit.Text) <= 0))
                ret += "Preencha o valor do INSS Pão de Açúcar.\\n";
            if (string.IsNullOrEmpty(txtInssPedreiraEdit.Text) || (Convert.ToDecimal(txtInssPedreiraEdit.Text) <= 0))
                ret += "Preencha o valor do INSS Pedreira.\\n";
            return ret;
        }
    }
}