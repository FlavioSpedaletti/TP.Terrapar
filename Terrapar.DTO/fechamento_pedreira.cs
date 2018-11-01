using System;

namespace Terrapar.DTO
{
    public class fechamento_pedreira
    {
        public int id_fechamento_pedreira { get; set; }
        public decimal valor_refeicao { get; set; }
        public decimal salario_pedreira { get; set; }
        public decimal valor_vale { get; set; }
        public decimal inss_pedreira { get; set; }
        public int id_usuario { get; set; }
        public DateTime data { get; set; }
    }
}