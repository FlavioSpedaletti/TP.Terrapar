using System;
using System.Web.UI.WebControls;
using Terrapar.DAL;
using Terrapar.DTO;

namespace Terrapar
{
    public partial class usuarios : System.Web.UI.Page
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
                    int idUsuario;
                    int.TryParse(id, out idUsuario);
                    
                    //edição
                    if (idUsuario != 0)
                    {
                        btnEditar.Text = "Salvar";
                        var usuario = new usuariosDAL().RecuperarPorID(idUsuario);
                        if (usuario == null)
                        {
                            ClientScript.RegisterStartupScript(GetType(), "usuNaoEncontrado", "alert('Usuário não encontrado');window.location.href='usuarios.aspx';", true);
                        }
                        else
                        {
                            txtPlacaEdit.Text = usuario.placa;
                            txtNomeMotoristaEdit.Text = usuario.nome_motorista;
                            txtNomeCaminhaoEdit.Text = usuario.nome_caminhao;
                            txtSenhaEdit.Text = usuario.senha;
                            ddlTiposEdit.SelectedValue = usuario.tipo.ToString();
                            txtMetaComissaoEdit.Text = usuario.meta_comissao.ToString();
                            txtTrocaOleoEdit.Text = usuario.troca_oleo.ToString();
                            chkAdminEdit.Checked = usuario.admin;
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
            grdUsuarios.DataSource =
                new usuariosDAL().Listar(new usuario()
                                             {
                                                 placa = txtPlaca.Text,
                                                 nome_motorista = txtNomeMotorista.Text,
                                                 nome_caminhao = txtNomeCaminhao.Text,
                                                 tipo = Convert.ToInt32(ddlTipos.SelectedValue)
                                             });
            grdUsuarios.DataBind();
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
                ClientScript.RegisterStartupScript(GetType(), "validaUsu", "alert('" + validacao + "');", true);
                return;
            }

            var id = Request["id"];
            int idUsuario;
            int.TryParse(id, out idUsuario);
            
            //novo
            if (idUsuario == 0)
            {
                var ok = new usuariosDAL().Adicionar(new usuario()
                                               {
                                                   placa = txtPlacaEdit.Text,
                                                   nome_motorista = txtNomeMotoristaEdit.Text,
                                                   nome_caminhao = txtNomeCaminhaoEdit.Text,
                                                   senha = txtSenhaEdit.Text,
                                                   tipo = int.Parse(ddlTiposEdit.SelectedValue),
                                                   admin = chkAdminEdit.Checked,
                                                   meta_comissao = decimal.Parse(txtMetaComissaoEdit.Text),
                                                   troca_oleo = int.Parse(txtTrocaOleoEdit.Text)
                                               });
                ClientScript.RegisterStartupScript(GetType(), "cadastrou", ok > 0 ? "alert('Usuário cadastrado com sucesso.');window.location.href='usuarios.aspx';" : "alert('Ocorreu algum erro ao tentar cadastrar. Tente novamente.');", true);
            }
            //editar
            else
            {
                var ok = new usuariosDAL().Atualizar(new usuario()
                {
                    id_usuario = idUsuario,
                    placa = txtPlacaEdit.Text,
                    nome_motorista = txtNomeMotoristaEdit.Text,
                    nome_caminhao = txtNomeCaminhaoEdit.Text,
                    senha = txtSenhaEdit.Text,
                    tipo = int.Parse(ddlTiposEdit.SelectedValue),
                    admin = chkAdminEdit.Checked,
                    meta_comissao = decimal.Parse(txtMetaComissaoEdit.Text),
                    troca_oleo = int.Parse(txtTrocaOleoEdit.Text)
                });
                ClientScript.RegisterStartupScript(GetType(), "atualizou", ok > 0 ? "alert('Usuário atualizado com sucesso.');window.location.href='usuarios.aspx';" : "alert('Ocorreu algum erro ao tentar atualizar. Tente novamente.');", true);
            }
        }

        protected string Validar()
        {
            var ret = "";
            if (string.IsNullOrEmpty(txtPlacaEdit.Text))
                ret += "Preencha a placa.\\n";
            if (string.IsNullOrEmpty(txtNomeMotoristaEdit.Text))
                ret += "Preencha o nome do motorista.\\n";
            if (string.IsNullOrEmpty(txtNomeCaminhaoEdit.Text))
                ret += "Preencha o nome do caminhão.\\n";
            if (string.IsNullOrEmpty(txtSenhaEdit.Text))
                ret += "Preencha a senha.\\n";
            if (string.IsNullOrEmpty(ddlTiposEdit.SelectedValue))
                ret += "Preencha o tipo.\\n";
            if (string.IsNullOrEmpty(txtMetaComissaoEdit.Text) || (Convert.ToDecimal(txtMetaComissaoEdit.Text) <= 0))
                ret += "Preencha a meta da comissão.\\n";
            if (string.IsNullOrEmpty(txtTrocaOleoEdit.Text) || (Convert.ToDecimal(txtTrocaOleoEdit.Text) <= 0))
                ret += "Preencha a troca de óleo.\\n";
            return ret;
        }

        protected void lnkDeletar_Click(object sender, EventArgs e)
        {
            var lnkRemove = (LinkButton)sender;
            var ok = new usuariosDAL().Deletar(new usuario()
            {
                id_usuario = int.Parse(lnkRemove.CommandArgument)
            });
            ClientScript.RegisterStartupScript(GetType(), "deletou", ok > 0 ? "alert('Usuário excluído com sucesso.');" : "alert('Ocorreu algum erro ao tentar excluir. Tente novamente.');", true);

            Buscar();
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtPlaca.Text = txtNomeMotorista.Text = txtNomeCaminhao.Text = "";
            ddlTipos.SelectedIndex = 0;
        }
    }
}