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
    public class viagens_pao_acucar_destacadasDAL : IDAL<viagem_pao_acucar_destacada>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public viagens_pao_acucar_destacadasDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(viagem_pao_acucar_destacada DTO)
        {
            var sql =
                "INSERT INTO viagens_pao_acucar_destacadas (id_viagem_pao_acucar, id_usuario)" +
                " VALUES (" + DTO.id_viagem_pao_acucar + ", " + DTO.id_usuario + ")";

            try
            {
                using (con = new MySqlConnection(_conexaoMySQL))
                {
                    new MySqlDataAdapter { InsertCommand = new MySqlCommand(sql, con) };
                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return cmd.LastInsertedId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int Deletar(viagem_pao_acucar_destacada DTO)
        {
            var sql = "DELETE FROM viagens_pao_acucar_destacadas" +
                      " WHERE id_viagem_pao_acucar = " + DTO.id_viagem_pao_acucar + " AND id_usuario = " + DTO.id_usuario;

            try
            {
                using (con = new MySqlConnection(_conexaoMySQL))
                {
                    new MySqlDataAdapter { DeleteCommand = new MySqlCommand(sql, con) };
                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        con.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int Atualizar(viagem_pao_acucar_destacada DTO)
        {
            var sql = "UPDATE viagens_pao_acucar_destacadas SET id_viagem_pao_acucar = " + DTO.id_viagem_pao_acucar + ", id_usuario = " + DTO.id_usuario +
                      " WHERE id_viagem_pao_acucar_destacada = " + DTO.id_viagem_pao_acucar_destacada;

            try
            {
                using (con = new MySqlConnection(_conexaoMySQL))
                {
                    new MySqlDataAdapter { UpdateCommand = new MySqlCommand(sql, con) };
                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        con.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<viagem_pao_acucar_destacada> Listar(viagem_pao_acucar_destacada DTO)
        {
            ICollection<viagem_pao_acucar_destacada> viagensDestacadas = new List<viagem_pao_acucar_destacada>();
            var sql = "SELECT *" +
                      "  FROM viagens_pao_acucar_destacadas";

            if (DTO != null)
            {
                sql += " WHERE id_usuario = " + DTO.id_usuario;
                sql += " AND id_viagem_pao_acucar = " + DTO.id_viagem_pao_acucar;
            }
            
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
                        viagensDestacadas.Add(new viagem_pao_acucar_destacada()
                        {
                            id_viagem_pao_acucar = Convert.ToInt32(dr["id_viagem_pao_acucar"].ToString()),
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString())
                        });
                    }
                    return viagensDestacadas;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public viagem_pao_acucar_destacada RecuperarPorID(int id)
        {
            var sql = "SELECT *" +
                      "  FROM viagens_pao_acucar_destacadas WHERE id_viagem_pao_acucar_destacada = " + id;

            try
            {
                using (con = new MySqlConnection(_conexaoMySQL))
                {
                    var da = new MySqlDataAdapter();
                    var dt = new DataTable();
                    da.SelectCommand = new MySqlCommand(sql, con);
                    da.Fill(dt);

                    if (dt.Rows.Count == 0) return null;

                    var dr = dt.Rows[0];
                    return new viagem_pao_acucar_destacada()
                    {
                        id_viagem_pao_acucar = Convert.ToInt32(dr["id_viagem_pao_acucar"].ToString()),
                        id_usuario = Convert.ToInt32(dr["id_usuario"].ToString())
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}