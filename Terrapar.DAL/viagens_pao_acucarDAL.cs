using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Terrapar.Classes;
using Terrapar.DTO;

namespace Terrapar.DAL
{
    public class viagens_pao_acucarDAL : IDAL<viagem_pao_acucar>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public viagens_pao_acucarDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(viagem_pao_acucar DTO)
        {
            var sql =
                "INSERT INTO viagens_pao_acucar (id_usuario, data, km_final, qtd_diesel, km_abastecimento, valor_abastecimento, domingo)" +
                " VALUES (" + DTO.id_usuario + ", '" + DTO.data.ToString("yyyy-MM-dd HH:mm") +
                "', " + DTO.km_final.ToIntMySql() + ", " +
                DTO.qtd_diesel.ToDecimalMySql() + ", " + DTO.km_abastecimento.ToIntMySql() + ", " +
                DTO.valor_abastecimento.ToDecimalMySql() + ", " + DTO.domingo + ")";

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

        public int Deletar(viagem_pao_acucar DTO)
        {
            var sqlDeletaViagem = "DELETE FROM viagens_pao_acucar" +
                                  " WHERE id_viagem_pao_acucar = " + DTO.id_viagem_pao_acucar;

            var sqlDeletaLojasViagem = "DELETE FROM lojas_viagem_pao_acucar" +
                                       " WHERE id_viagem_pao_acucar = " + DTO.id_viagem_pao_acucar;

            try
            {
                using (con = new MySqlConnection(_conexaoMySQL))
                {
                    new MySqlDataAdapter { DeleteCommand = new MySqlCommand(sqlDeletaLojasViagem, con) };
                    using (var cmd = new MySqlCommand(sqlDeletaLojasViagem, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    new MySqlDataAdapter { DeleteCommand = new MySqlCommand(sqlDeletaViagem, con) };
                    using (var cmd = new MySqlCommand(sqlDeletaViagem, con))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int DeletarLojas(viagem_pao_acucar DTO)
        {
            var sqlDeletaLojasViagem = "DELETE FROM lojas_viagem_pao_acucar" +
                                       " WHERE id_viagem_pao_acucar = " + DTO.id_viagem_pao_acucar;

            try
            {
                using (con = new MySqlConnection(_conexaoMySQL))
                {
                    new MySqlDataAdapter { DeleteCommand = new MySqlCommand(sqlDeletaLojasViagem, con) };
                    using (var cmd = new MySqlCommand(sqlDeletaLojasViagem, con))
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

        public int Atualizar(viagem_pao_acucar DTO)
        {
            var sql = "UPDATE viagens_pao_acucar SET data = '" + DTO.data.ToString("yyyy-MM-dd HH:mm") + "', id_usuario = " + DTO.id_usuario +
                      ", km_final = " + DTO.km_final.ToIntMySql() + ", qtd_diesel = " +
                      DTO.qtd_diesel.ToDecimalMySql() + ", " +
                      "km_abastecimento = " + DTO.km_abastecimento.ToIntMySql() +
                      ", valor_abastecimento = " + DTO.valor_abastecimento.ToDecimalMySql() + ", " +
                      "domingo = " + DTO.domingo +
                      " WHERE id_viagem_pao_acucar = " + DTO.id_viagem_pao_acucar;

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

        public IEnumerable<viagem_pao_acucar> Listar(viagem_pao_acucar DTO)
        {
            ICollection<viagem_pao_acucar> viagens = new List<viagem_pao_acucar>();
            var sql = "SELECT *" +
                      "  FROM viagens_pao_acucar";

            if (DTO != null)
                sql += " WHERE id_usuario = " + DTO.id_usuario;
            if (DTO.data != DateTime.MinValue)
                sql += " AND DATE_FORMAT(data,'%m/%Y') = '" + DTO.data.ToString("MM/yyyy") + "'";

            sql += " ORDER BY data";

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
                        viagens.Add(new viagem_pao_acucar()
                        {
                            id_viagem_pao_acucar = Convert.ToInt32(dr["id_viagem_pao_acucar"].ToString()),
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            data = Convert.ToDateTime(dr["data"].ToString()),
                            km_final = Convert.ToInt32(dr["km_final"].ToString()),
                            qtd_diesel = Convert.ToDecimal(dr["qtd_diesel"].ToString()),
                            km_abastecimento = Convert.ToInt32(dr["km_abastecimento"].ToString()),
                            valor_abastecimento = Convert.ToDecimal(dr["valor_abastecimento"].ToString()),
                            domingo = Convert.ToBoolean(dr["domingo"].ToString())
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

        public viagem_pao_acucar RecuperarPorID(int id)
        {
            var sql = "SELECT *" +
                      "  FROM viagens_pao_acucar WHERE id_viagem_pao_acucar = " + id;

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
                    return new viagem_pao_acucar()
                    {
                        id_viagem_pao_acucar = Convert.ToInt32(dr["id_viagem_pao_acucar"].ToString()),
                        id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                        data = Convert.ToDateTime(dr["data"].ToString()),
                        km_final = Convert.ToInt32(dr["km_final"].ToString()),
                        qtd_diesel = Convert.ToDecimal(dr["qtd_diesel"].ToString()),
                        km_abastecimento = Convert.ToInt32(dr["km_abastecimento"].ToString()),
                        valor_abastecimento = Convert.ToDecimal(dr["valor_abastecimento"].ToString()),
                        domingo = Convert.ToBoolean(dr["domingo"].ToString())
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