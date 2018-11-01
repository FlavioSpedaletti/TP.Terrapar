using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Terrapar.Classes;
using Terrapar.DTO;

namespace Terrapar.DAL
{
    public class servicosDAL : IDAL<servico>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public servicosDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(servico DTO)
        {
            var sql =
                "INSERT INTO servicos (descricao, valor, km, tipo, id_usuario, data)" +
                " VALUES ('" + DTO.descricao.ToStringSegura() + "', " + DTO.valor.ToDecimalMySql() + ", " + DTO.km.ToIntMySql() + ", " + DTO.tipo + ", " + DTO.id_usuario + ", '" + DTO.data.ToString("yyyy-MM-dd HH:mm") + "')";

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

        public int Deletar(servico DTO)
        {
            var sql = "DELETE FROM servicos" +
                      " WHERE id_servico = " + DTO.id_servico;

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

        public int Atualizar(servico DTO)
        {
            var sql = "UPDATE servicos SET descricao = '" + DTO.descricao.ToStringSegura() +
                      "', valor = " + DTO.valor.ToDecimalMySql() + ", km = " + DTO.km.ToIntMySql() + ", tipo = " + DTO.tipo +
                      ", id_usuario = " + DTO.id_usuario + ", data = '" + DTO.data.ToString("yyyy-MM-dd HH:mm") + "'" +
                      " WHERE id_servico = " + DTO.id_servico;

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

        public IEnumerable<servico> Listar(servico DTO)
        {
            ICollection<servico> servicos = new List<servico>();
            var sql = "SELECT *" +
                      "  FROM servicos";

            if (DTO != null)
                sql += " WHERE id_usuario = " + DTO.id_usuario;
            if (DTO.data != DateTime.MinValue)
                sql += " AND DATE_FORMAT(data,'%m/%Y') = '" + DTO.data.ToString("MM/yyyy") + "'";
            if (!string.IsNullOrEmpty(DTO.descricao))
                sql += " AND descricao = '" + DTO.descricao + "'";

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
                        servicos.Add(new servico()
                        {
                            id_servico = Convert.ToInt32(dr["id_servico"].ToString()),
                            descricao = dr["descricao"].ToString(),
                            valor = Convert.ToDecimal(dr["valor"].ToString()),
                            km = Convert.ToInt32(dr["km"].ToString()),
                            tipo = Convert.ToInt32(dr["tipo"].ToString()),
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            data = Convert.ToDateTime(dr["data"].ToString())
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

        public servico RecuperarPorID(int id)
        {
            var sql = "SELECT *" +
                      "  FROM servicos WHERE id_servico = " + id;

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
                    return new servico()
                    {
                        id_servico = Convert.ToInt32(dr["id_servico"].ToString()),
                        descricao = dr["descricao"].ToString(),
                        valor = Convert.ToDecimal(dr["valor"].ToString()),
                        km = Convert.ToInt32(dr["km"].ToString()),
                        tipo = Convert.ToInt32(dr["tipo"].ToString()),
                        id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                        data = Convert.ToDateTime(dr["data"].ToString())
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