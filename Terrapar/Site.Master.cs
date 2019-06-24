using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using Terrapar.Classes;
using Terrapar.DTO;

namespace Terrapar
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //verifica se tem usuário logado
            if (Context.User.Identity.IsAuthenticated && Session["usuario"] != null)
            {
                var usuario = ((usuario) Session["usuario"]);
                lblUsuario.Text = string.Format("{0} - {1}", usuario.nome_motorista.PrimeiroNome(), usuario.placa);
            }
            else
                Response.Redirect("~/login.aspx");

            //adiciona menus se administrador
            if(((usuario)Session["usuario"]).admin)
            {
                liServicos.Visible =
                    liFolha.Visible =
                    liFechamento.Visible = liPlacas.Visible = liLojas.Visible = liParametros.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
        }

        protected void lnkSair_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/login.aspx");
        }
    }
}
