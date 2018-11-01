using System;

namespace Terrapar.DTO
{
    public class loja_viagem_pao_acucar
    {
        public int id_loja_viagem_pao_acucar { get; set; }
        public int id_viagem_pao_acucar { get; set; }
        public int id_loja { get; set; }
        public decimal valor_frete { get; set; }
        public string numero_ordem_coleta { get; set; }
    }
}