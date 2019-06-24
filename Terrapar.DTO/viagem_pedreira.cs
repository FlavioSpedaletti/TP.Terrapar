using System;

namespace Terrapar.DTO
{
    public class viagem_pedreira
    {
        public int id_viagem_pedreira { get; set; }
        public int id_usuario { get; set; }
        public DateTime data { get; set; }
        public string origem { get; set; }
        public string destino { get; set; }
        public string numero_nota_fiscal { get; set; }
        public decimal peso { get; set; }
        public decimal valor_frete { get; set; }
        public decimal base_calculo_icms { get; set; }
        public decimal valor_icms { get; set; }
        public decimal valor_produtos { get; set; }
        public decimal valor_nota_fiscal { get; set; }
        public decimal qtd_diesel { get; set; }
        public int km_abastecimento { get; set; }
        public decimal valor_abastecimento { get; set; }
        public bool noite { get; set; }
        public bool domingo_feriado { get; set; }
    }

    public class viagem_pedreira_folha_pgto
    {
        public int id_usuario { get; set; }
        public DateTime dia { get; set; }
        public int qtd_viagens { get; set; }
        public decimal total_frete { get; set; }
        //public string periodo { get; set; }
        public bool noite { get; set; }
        public bool domingo_feriado { get; set; }
    }
}