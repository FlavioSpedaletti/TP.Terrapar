using System;
using System.Web.UI.WebControls;
using Terrapar.DAL;
using Terrapar.DTO;

namespace Terrapar
{
    public partial class lojas : System.Web.UI.Page
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
                }
                else
                {
                    vwEstados.ActiveViewIndex = 1;
                    int idLoja;
                    int.TryParse(id, out idLoja);
                    
                    //edição
                    if (idLoja != 0)
                    {
                        btnEditar.Text = "Salvar";
                        var loja = new lojasDAL().RecuperarPorID(idLoja);
                        if (loja == null)
                        {
                            ClientScript.RegisterStartupScript(GetType(), "lojaNaoEncontrada", "alert('Loja não encontrada');window.location.href='lojas.aspx';", true);
                        }
                        else
                        {
                            txtIdentificacaoEdit.Text = loja.identificacao;
                            txtNomeEdit.Text = loja.nome;
                        }
                    }
                    else
                    {
                        btnEditar.Text = "Salvar";
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
            grdLojas.DataSource =
                new lojasDAL().Listar(new loja()
                                          {
                                              identificacao =
                                                  string.IsNullOrEmpty(txtIdentificacao.Text)
                                                      ? null
                                                      : txtIdentificacao.Text,
                                              nome = txtNome.Text
                                          });
            grdLojas.DataBind();
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
                ClientScript.RegisterStartupScript(GetType(), "validaLoja", "alert('" + validacao + "');", true);
                return;
            }

            var id = Request["id"];
            int idLoja;
            int.TryParse(id, out idLoja);
            
            //novo
            if (idLoja == 0)
            {
                var ok = new lojasDAL().Adicionar(new loja()
                                               {
                                                   identificacao = txtIdentificacaoEdit.Text,
                                                   nome = txtNomeEdit.Text
                                               });
                ClientScript.RegisterStartupScript(GetType(), "cadastrou", ok > 0 ? "alert('Loja cadastrada com sucesso.');window.location.href='lojas.aspx';" : "alert('Ocorreu algum erro ao tentar cadastrar. Tente novamente.');", true);
            }
            //editar
            else
            {
                var ok = new lojasDAL().Atualizar(new loja()
                {
                    id_loja = idLoja,
                    identificacao = txtIdentificacaoEdit.Text,
                    nome = txtNomeEdit.Text
                });
                ClientScript.RegisterStartupScript(GetType(), "atualizou", ok > 0 ? "alert('Loja atualizada com sucesso.');window.location.href='lojas.aspx';" : "alert('Ocorreu algum erro ao tentar atualizar. Tente novamente.');", true);
            }
        }

        protected string Validar()
        {
            var ret = "";
            if (string.IsNullOrEmpty(txtIdentificacaoEdit.Text))
                ret += "Preencha o número.\\n";
            if (string.IsNullOrEmpty(txtNomeEdit.Text))
                ret += "Preencha o nome.\\n";
            return ret;
        }

        protected void lnkDeletar_Click(object sender, EventArgs e)
        {
            var lnkRemove = (LinkButton)sender;
            var ok = new lojasDAL().Deletar(new loja()
            {
                id_loja = int.Parse(lnkRemove.CommandArgument)
            });
            ClientScript.RegisterStartupScript(GetType(), "deletou", ok > 0 ? "alert('Loja excluída com sucesso.');" : "alert('Ocorreu algum erro ao tentar excluir. Tente novamente.');", true);

            Buscar();
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtIdentificacao.Text = txtNome.Text = "";
        }
    }
}