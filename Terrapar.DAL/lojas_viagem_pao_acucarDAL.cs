using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Terrapar.Classes;
using Terrapar.DTO;

namespace Terrapar.DAL
{
    public class lojas_viagem_pao_acucarDAL : IDAL<loja_viagem_pao_acucar>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public lojas_viagem_pao_acucarDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(loja_viagem_pao_acucar DTO)
        {
            var sql =
                "INSERT INTO lojas_viagem_pao_acucar (id_viagem_pao_acucar, id_loja, valor_frete, numero_ordem_coleta)" +
                " VALUES (" + DTO.id_viagem_pao_acucar + ", " + DTO.id_loja + ", " + DTO.valor_frete.ToDecimalMySql() + ", '" +
                (!string.IsNullOrEmpty(DTO.numero_ordem_coleta) ? DTO.numero_ordem_coleta.ToStringSegura() : "") + "')";

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

        public int Deletar(loja_viagem_pao_acucar DTO)
        {
            var sql = "DELETE FROM lojas_viagem_pao_acucar" +
                      " WHERE id_loja_viagem_pao_acucar = " + DTO.id_loja_viagem_pao_acucar;

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

        public int Atualizar(loja_viagem_pao_acucar DTO)
        {
            var sql = "UPDATE lojas_viagem_pao_acucar SET id_viagem_pao_acucar = " + DTO.id_viagem_pao_acucar +
                      ", id_loja = " + DTO.id_loja + ", " +
                      "valor_frete = " + DTO.valor_frete.ToDecimalMySql() +
                      ", numero_ordem_coleta = '" + DTO.numero_ordem_coleta.ToStringSegura() + "'" +
                      " WHERE id_loja_viagem_pao_acucar = " + DTO.id_loja_viagem_pao_acucar;

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

        public IEnumerable<loja_viagem_pao_acucar> Listar(loja_viagem_pao_acucar DTO)
        {
            ICollection<loja_viagem_pao_acucar> lojas_viagem = new List<loja_viagem_pao_acucar>();
            var sql = "SELECT *" +
                      "  FROM lojas_viagem_pao_acucar";

            if (DTO != null)
                sql += " WHERE id_viagem_pao_acucar = " + DTO.id_viagem_pao_acucar;

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
                        lojas_viagem.Add(new loja_viagem_pao_acucar()
                        {
                            id_loja_viagem_pao_acucar = Convert.ToInt32(dr["id_loja_viagem_pao_acucar"].ToString()),
                            id_viagem_pao_acucar = Convert.ToInt32(dr["id_viagem_pao_acucar"].ToString()),
                            id_loja = Convert.ToInt32(dr["id_loja"].ToString()),
                            valor_frete = Convert.ToDecimal(dr["valor_frete"].ToString()),
                            numero_ordem_coleta = dr["numero_ordem_coleta"].ToString()
                        });
                    }
                    return lojas_viagem;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public loja_viagem_pao_acucar RecuperarPorID(int id)
        {
            var sql = "SELECT *" +
                      "  FROM lojas_viagem_pao_acucar WHERE id_viagem_pao_acucar = " + id;

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
                    return new loja_viagem_pao_acucar()
                    {
                        id_loja_viagem_pao_acucar = Convert.ToInt32(dr["id_loja_viagem_pao_acucar"].ToString()),
                        id_viagem_pao_acucar = Convert.ToInt32(dr["id_viagem_pao_acucar"].ToString()),
                        id_loja = Convert.ToInt32(dr["id_loja"].ToString()),
                        valor_frete = Convert.ToDecimal(dr["valor_frete"].ToString()),
                        numero_ordem_coleta = dr["numero_ordem_coleta"].ToString()
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