using System;

namespace Terrapar.DTO
{
    public class debito_credito
    {
        public int id_debito_credito { get; set; }
        /// <summary>
        /// 0 => Débito, 1 => Crédito
        /// </summary>
        public int tipo { get; set; }
        public string descricao { get; set; }
        public decimal valor { get; set; }
        public int id_usuario { get; set; }
        public DateTime data { get; set; }
    }
}