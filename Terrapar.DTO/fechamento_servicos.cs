using System;

namespace Terrapar.DTO
{
    public class fechamento_servicos
    {
        public int id_fechamento_servicos { get; set; }
        public int id_usuario { get; set; }
        public string placa { get; set; }
        public DateTime periodo { get; set; }
        public decimal soma_valor { get; set; }
    }
}