using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Terrapar.Classes;
using Terrapar.DTO;

namespace Terrapar.DAL
{
    public class lojasDAL : IDAL<loja>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public lojasDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(loja DTO)
        {
            var sql = "INSERT INTO lojas (identificacao, nome)" +
                      " VALUES ('" + DTO.identificacao.ToStringSegura() + "', '" + DTO.nome.ToStringSegura() + "')";

            try
            {
                using (con = new MySqlConnection(_conexaoMySQL))
                {
                    new MySqlDataAdapter {InsertCommand = new MySqlCommand(sql, con)};
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

        public int Deletar(loja DTO)
        {
            var sql = "DELETE FROM lojas" +
                      " WHERE id_loja = " + DTO.id_loja;

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

        public int Atualizar(loja DTO)
        {
            var sql = "UPDATE lojas SET identificacao = '" + DTO.identificacao + "', nome = '" + DTO.nome.ToStringSegura() + "'" +
                      " WHERE id_loja = " + DTO.id_loja;

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

        public IEnumerable<loja> Listar(loja DTO)
        {
            ICollection<loja> lojas = new List<loja>();
            var sql = "SELECT *" +
                      "  FROM lojas";

            if (DTO != null)
                sql +=
                    " WHERE " + (!string.IsNullOrEmpty(DTO.identificacao) ? "identificacao = '" + DTO.identificacao + "'" : "1=1") +
                    "   AND nome like '%" + DTO.nome.ToStringSegura() + "%'";

            sql += " ORDER BY concat(repeat(" + "0" + ", 18-length(identificacao)), identificacao)";

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
                        lojas.Add(new loja
                        {
                            id_loja = Convert.ToInt32(dr["id_loja"].ToString()),
                            identificacao = dr["identificacao"].ToString(),
                            nome = dr["nome"].ToString(),
                            identificacaoEnome = dr["identificacao"] + " - " + dr["nome"]
                        });
                    }
                    return lojas;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public loja RecuperarPorID(int id)
        {
            var sql = "SELECT * FROM lojas WHERE id_loja = " + id;

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
                    return new loja
                    {
                        id_loja = Convert.ToInt32(dr["id_loja"].ToString()),
                        identificacao = dr["identificacao"].ToString(),
                        nome = dr["nome"].ToString(),
                        identificacaoEnome = dr["identificacao"] + " - " + dr["nome"]
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