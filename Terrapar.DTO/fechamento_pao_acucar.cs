using System;

namespace Terrapar.DTO
{
    public class fechamento_pao_acucar
    {
        public int id_fechamento_pao_acucar { get; set; }
        public decimal qtd_horas_extras { get; set; }
        public decimal qtd_diarias { get; set; }
        public decimal valor_hora_extra_pao_acucar { get; set; }
        public decimal valor_diaria_pao_acucar { get; set; }
        public decimal salario_pao_acucar { get; set; }
        public decimal valor_vale { get; set; }
        public decimal inss_pao_acucar { get; set; }
        public int id_usuario { get; set; }
        public DateTime data { get; set; }
    }
}