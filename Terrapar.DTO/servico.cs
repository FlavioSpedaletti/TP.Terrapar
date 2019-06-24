using System;

namespace Terrapar.DTO
{
    public class servico
    {
        public int id_servico { get; set; }
        public string descricao { get; set; }
        public decimal valor { get; set; }
        public int km { get; set; }
        /// <summary>
        /// 0 => Manutenção, 1 => Despesa
        /// </summary>
        public int tipo { get; set; }
        public int id_usuario { get; set; }
        public DateTime data { get; set; }
    }
}