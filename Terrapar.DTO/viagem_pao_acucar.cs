using System;

namespace Terrapar.DTO
{
    public class viagem_pao_acucar
    {
        public int id_viagem_pao_acucar { get; set; }
        public int id_usuario { get; set; }
        public DateTime data { get; set; }
        public int km_final { get; set; }
        public decimal qtd_diesel { get; set; }
        public int km_abastecimento { get; set; }
        public decimal valor_abastecimento { get; set; }
        public bool domingo { get; set; }
    }
}