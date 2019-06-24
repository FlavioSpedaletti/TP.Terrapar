using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Terrapar.DAL;
using Terrapar.DTO;

namespace Terrapar
{
	public partial class fechamento : System.Web.UI.Page
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
		    
		}

        [WebMethod]
        public static List<fechamentoDTO> RecuperaFechamentos()
        {
            var parGerais = new parametros_geraisDAL().RecuperarPorID(1);
            var fechamentoDTO = new List<fechamentoDTO>();

            var viagensPorMes = new fechamento_viagensDAL().Listar(null);
            foreach (var v in viagensPorMes)
            {
                var servicos = new fechamento_servicosDAL().Listar(new fechamento_servicos() { id_usuario = v.id_usuario, periodo = v.periodo });
                var creditos = new debitos_creditosDAL().Listar(new debito_credito() { id_usuario = v.id_usuario, data = v.periodo }).Where(c => c.tipo == 1);
                var usuario = new usuariosDAL().RecuperarPorID(v.id_usuario);
                decimal salario = 0M;
                
                //pão de açúcar
                if (usuario.tipo == 0)
                {
                    var fechamentoPaoAcucar =
                        new fechamentos_pao_acucarDAL().Listar(new fechamento_pao_acucar()
                                                                   {
                                                                       id_usuario = v.id_usuario,
                                                                       data = v.periodo
                                                                   }).
                            SingleOrDefault();

                    var qtdHorasExtras = fechamentoPaoAcucar != null ? fechamentoPaoAcucar.qtd_horas_extras : Convert.ToDecimal("52,00");
                    var qtdDiarias = fechamentoPaoAcucar != null ? fechamentoPaoAcucar.qtd_diarias : Convert.ToDecimal("26,00");
                    var valorHoraExtra = fechamentoPaoAcucar != null
                                             ? fechamentoPaoAcucar.valor_hora_extra_pao_acucar
                                             : parGerais.valor_hora_extra_pao_acucar;
                    var valorDiaria = fechamentoPaoAcucar != null
                                          ? fechamentoPaoAcucar.valor_diaria_pao_acucar
                                          : parGerais.valor_diaria_pao_acucar;
                    var salarioPaoAcucar = fechamentoPaoAcucar != null
                                               ? fechamentoPaoAcucar.salario_pao_acucar
                                               : parGerais.salario_pao_acucar;
                    
                    var valorHorasExtras = qtdHorasExtras * valorHoraExtra;
                    var valorDiarias = qtdDiarias * valorDiaria;
                    var viagensPaoAcucar = new viagens_pao_acucarDAL().Listar(new viagem_pao_acucar()
                                                                                  {
                                                                                      id_usuario = v.id_usuario,
                                                                                      data = v.periodo
                                                                                  });
                    var domingo70reais = viagensPaoAcucar.Where(vpao => vpao.domingo).Count()*70;
                    var excedenteMetaFrete = v.soma_frete - usuario.meta_comissao;
                    var comissao = excedenteMetaFrete > 0 ? (excedenteMetaFrete * (decimal)0.1) : 0;

                    salario += salarioPaoAcucar + valorHorasExtras + valorDiarias + domingo70reais + comissao;
                }
                else
                {
                    //pedreira
                    var fechamentoPedreira = new fechamentos_pedreiraDAL().Listar(new fechamento_pedreira()
                                                                                      {
                                                                                          id_usuario = v.id_usuario,
                                                                                          data = v.periodo
                                                                                      }).
                        SingleOrDefault();

                    var valorRefeicao = fechamentoPedreira != null
                                            ? fechamentoPedreira.valor_refeicao
                                            : parGerais.valor_refeicao;
                    var salarioPedreira = fechamentoPedreira != null
                                              ? fechamentoPedreira.salario_pedreira
                                              : parGerais.salario_pedreira;

                    var viagensPedreira = new viagens_pedreiraDAL().ListarParaFolhaPgto(new viagem_pedreira()
                                                                                            {
                                                                                                id_usuario = v.id_usuario,
                                                                                                data = v.periodo
                                                                                            });
                    var comissao7ou15 =
                        viagensPedreira.Sum(
                            vp => vp.total_frete*(vp.noite || vp.domingo_feriado ? (decimal) 0.15 : (decimal) 0.07));
                    var qtdRefeicoes = viagensPedreira.Where(vp => !vp.domingo_feriado).Count();
                    var refeicao = qtdRefeicoes * valorRefeicao;
                    var excedenteMetaFrete = v.soma_frete - usuario.meta_comissao;
                    var comissao5 = excedenteMetaFrete > 0 ? (excedenteMetaFrete * (decimal)0.05) : 0;

                    salario += salarioPedreira + comissao7ou15 + refeicao + comissao5;
                }

                salario += creditos.Sum(c => c.valor);
                var valor_liquido = (v.soma_frete - salario -
                                     v.soma_abastecimento - servicos.Sum(s => s.soma_valor));
                var composicao_valor_liquido =
                    string.Format("Frete: R$ {0}  -  Salário: R$ {1}  -  Diesel: R$ {2}  -  Serviços: R$ {3}",
                                  v.soma_frete,
                                  salario, v.soma_abastecimento,
                                  servicos.Sum(s => s.soma_valor));

                fechamentoDTO.Add(new fechamentoDTO()
                                      {
                                          placa = v.placa,
                                          periodo = v.periodo.ToString("MM/yyyy"),
                                          ano = v.periodo.ToString("yyyy"),
                                          valor_liquido = valor_liquido,
                                          composicao_valor_liquido = composicao_valor_liquido,
                                          valor_bruto = v.soma_frete
                                      });
            }
            return fechamentoDTO;
        }
	}

    public class fechamentoDTO
    {
        public string placa { get; set; }
        public string periodo { get; set; }
        public string ano { get; set; }
        public decimal valor_liquido { get; set; }
        public string composicao_valor_liquido { get; set; }
        public decimal valor_bruto { get; set; }
    }
}