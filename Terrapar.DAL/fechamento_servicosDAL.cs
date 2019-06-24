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
    public class fechamento_servicosDAL
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public fechamento_servicosDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public IEnumerable<fechamento_servicos> Listar(fechamento_servicos DTO)
        {
            ICollection<fechamento_servicos> servicos = new List<fechamento_servicos>();
            var sql = "SELECT *" +
                      "  FROM fechamento_servicos";

            if (DTO != null)
                sql += " WHERE id_usuario = " + DTO.id_usuario;
            if (DTO.periodo != DateTime.MinValue)
                sql += " AND periodo = '" + DTO.periodo.ToString("MM/yyyy") + "'";

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
                        servicos.Add(new fechamento_servicos()
                        {
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            placa = dr["placa"].ToString(),
                            periodo = Convert.ToDateTime(dr["periodo"].ToString()),
                            soma_valor = Convert.ToDecimal(dr["soma_valor"].ToString())
                        });
                    }
                    return servicos;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}