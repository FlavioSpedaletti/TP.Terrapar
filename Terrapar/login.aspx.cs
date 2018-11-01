using System;
using Terrapar.DAL;
using System.Web.Security;

namespace Terrapar
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatusLogin.Visible = false;

            //force login
            //int idusu = 1;
            //FormsAuthentication.RedirectFromLoginPage(idusu.ToString(), true);
            //Session["usuario"] = new usuariosDAL().RecuperarPorID(idusu);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            /*Forms authentication customizado, para tentar implementar algum dia*/
            /*http://www.shawnmclean.com/blog/2012/01/storing-strongly-typed-object-user-profile-data-in-asp-net-forms-authentication-cookie*/

            var usuario = new usuariosDAL().RecuperarParaLogin(txtPlaca.Text, txtSenha.Text);
            if (usuario != null)
            {
                FormsAuthentication.RedirectFromLoginPage(usuario.id_usuario.ToString(), true);
                Session["usuario"] = usuario;
            }
            else
            {
                lblStatusLogin.Visible = true;
                lblStatusLogin.Text = "Usuário e/ou senha inválidos.";
            }
        }
    }
}