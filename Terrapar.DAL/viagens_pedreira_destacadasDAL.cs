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
    public class viagens_pedreira_destacadasDAL : IDAL<viagem_pedreira_destacada>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public viagens_pedreira_destacadasDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(viagem_pedreira_destacada DTO)
        {
            var sql =
                "INSERT INTO viagens_pedreira_destacadas (id_viagem_pedreira, id_usuario)" +
                " VALUES (" + DTO.id_viagem_pedreira + ", " + DTO.id_usuario + ")";

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

        public int Deletar(viagem_pedreira_destacada DTO)
        {
            var sql = "DELETE FROM viagens_pedreira_destacadas" +
                      " WHERE id_viagem_pedreira = " + DTO.id_viagem_pedreira + " AND id_usuario = " + DTO.id_usuario;

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

        public int Atualizar(viagem_pedreira_destacada DTO)
        {
            var sql = "UPDATE viagens_pedreira_destacadas SET id_viagem_pedreira = " + DTO.id_viagem_pedreira + ", id_usuario = " + DTO.id_usuario +
                      " WHERE id_viagem_pedreira_destacada = " + DTO.id_viagem_pedreira_destacada;

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

        public IEnumerable<viagem_pedreira_destacada> Listar(viagem_pedreira_destacada DTO)
        {
            ICollection<viagem_pedreira_destacada> viagensDestacadas = new List<viagem_pedreira_destacada>();
            var sql = "SELECT *" +
                      "  FROM viagens_pedreira_destacadas";

            if (DTO != null)
            {
                sql += " WHERE id_usuario = " + DTO.id_usuario;
                sql += " AND id_viagem_pedreira = " + DTO.id_viagem_pedreira;
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
                        viagensDestacadas.Add(new viagem_pedreira_destacada()
                        {
                            id_viagem_pedreira = Convert.ToInt32(dr["id_viagem_pedreira"].ToString()),
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

        public viagem_pedreira_destacada RecuperarPorID(int id)
        {
            var sql = "SELECT *" +
                      "  FROM viagens_pedreira_destacadas WHERE id_viagem_pedreira_destacada = " + id;

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
                    return new viagem_pedreira_destacada()
                    {
                        id_viagem_pedreira = Convert.ToInt32(dr["id_viagem_pedreira"].ToString()),
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