using System;
namespace Terrapar.DTO
{
    public class fechamento_viagens
    {
        public int id_usuario { get; set; }
        public string placa { get; set; }
        public DateTime periodo { get; set; }
        public int tipo_viagem { get; set; }
        public decimal soma_frete { get; set; }
        public decimal soma_abastecimento { get; set; }
    }
}