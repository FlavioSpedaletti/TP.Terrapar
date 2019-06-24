using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Terrapar.DTO;

namespace Terrapar.DAL
{
    public class fechamento_viagensDAL
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public fechamento_viagensDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public IEnumerable<fechamento_viagens> Listar(fechamento_viagens DTO)
        {
            ICollection<fechamento_viagens> viagens = new List<fechamento_viagens>();
            var sql = "SELECT *" +
                      "  FROM fechamento_viagens";

            if (DTO != null)
                sql += " WHERE id_usuario = " + DTO.id_usuario;
            if (DTO != null && DTO.periodo != DateTime.MinValue)
                sql += " AND periodo = '" + DTO.periodo.ToString("MM/yyyy") + "'";

            sql += " ORDER BY periodo, id_usuario ";

            try
            {
                using (con = new MySqlConnection(_conexaoMySQL))
                {
                    var da = new MySqlDataAdapter();
                    var dt = new DataTable();
                    da.SelectCommand = new MySqlCommand(sql, con);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        viagens.Add(new fechamento_viagens()
                        {
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            placa = dr["placa"].ToString(),
                            periodo = Convert.ToDateTime(dr["periodo"].ToString()),
                            tipo_viagem = Convert.ToInt32(dr["tipo_viagem"].ToString()),
                            soma_frete = Convert.ToDecimal(dr["soma_frete"].ToString()),
                            soma_abastecimento = Convert.ToDecimal(dr["soma_abastecimento"].ToString())
                        });
                    }
                    return viagens;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}