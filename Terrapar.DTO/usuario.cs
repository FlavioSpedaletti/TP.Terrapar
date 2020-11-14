using System;

namespace Terrapar.DTO
{
    [Serializable()]
    public class usuario
    {
        public int id_usuario { get; set; }
        public bool admin { get; set; }
        public string nome_motorista { get; set; }
        public string nome_caminhao { get; set; }
        public string placa { get; set; }
        public string senha { get; set; }
        /// <summary>
        /// 0 => Pão de açúcar, 1 => Pedreira
        /// </summary>
        public int tipo { get; set; }
        public decimal meta_comissao { get; set; }
        public int troca_oleo { get; set; }
    }
}