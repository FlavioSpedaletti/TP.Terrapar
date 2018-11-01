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
    public class debitos_creditosDAL : IDAL<debito_credito>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public debitos_creditosDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(debito_credito DTO)
        {
            var sql =
                "INSERT INTO debitos_creditos (tipo, descricao, valor, id_usuario, data)" +
                " VALUES (" + DTO.tipo + ", '" + DTO.descricao.ToStringSegura() + "', " + DTO.valor.ToDecimalMySql() + ", " + DTO.id_usuario + ", '" + DTO.data.ToString("yyyy-MM-dd HH:mm") + "')";

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

        public int Deletar(debito_credito DTO)
        {
            var sql = "DELETE FROM debitos_creditos" +
                      " WHERE id_debito_credito = " + DTO.id_debito_credito;

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

        public int Atualizar(debito_credito DTO)
        {
            var sql = "UPDATE debitos_creditos SET tipo = " + DTO.tipo + ", descricao = '" + DTO.descricao.ToStringSegura() +
                      "', valor = " + DTO.valor.ToDecimalMySql() + ", id_usuario = " + DTO.id_usuario + ", data = '" + DTO.data.ToString("yyyy-MM-dd HH:mm") + "'" +
                      " WHERE id_debito_credito = " + DTO.id_debito_credito;

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

        public IEnumerable<debito_credito> Listar(debito_credito DTO)
        {
            ICollection<debito_credito> debitos_creditos = new List<debito_credito>();
            var sql = "SELECT *" +
                      "  FROM debitos_creditos";

            if (DTO != null)
                sql += " WHERE id_usuario = " + DTO.id_usuario;
            if (DTO.data != DateTime.MinValue)
                sql += " AND DATE_FORMAT(data,'%m/%Y') = '" + DTO.data.ToString("MM/yyyy") + "'";

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
                        debitos_creditos.Add(new debito_credito()
                        {
                            id_debito_credito = Convert.ToInt32(dr["id_debito_credito"].ToString()),
                            tipo = Convert.ToInt32(dr["tipo"].ToString()),
                            descricao = dr["descricao"].ToString(),
                            valor = Convert.ToDecimal(dr["valor"].ToString()),
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            data = Convert.ToDateTime(dr["data"].ToString())
                        });
                    }
                    return debitos_creditos;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable ListarParaDatatable(debito_credito DTO)
        {
            var sql = "SELECT *" +
                      "  FROM debitos_creditos";

            if (DTO != null)
                sql += " WHERE id_usuario = " + DTO.id_usuario;
            if (DTO.data != DateTime.MinValue)
                sql += " AND DATE_FORMAT(data,'%m/%Y') = '" + DTO.data.ToString("MM/yyyy") + "'";

            try
            {
                using (con = new MySqlConnection(_conexaoMySQL))
                {
                    var da = new MySqlDataAdapter();
                    var dt = new DataTable();
                    da.SelectCommand = new MySqlCommand(sql, con);
                    da.Fill(dt);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public debito_credito RecuperarPorID(int id)
        {
            var sql = "SELECT *" +
                      "  FROM debitos_creditos WHERE id_debito_credito = " + id;

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
                    return new debito_credito()
                    {
                        id_debito_credito = Convert.ToInt32(dr["id_debito_credito"].ToString()),
                        tipo = Convert.ToInt32(dr["tipo"].ToString()),
                        descricao = dr["descricao"].ToString(),
                        valor = Convert.ToDecimal(dr["valor"].ToString()),
                        id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                        data = Convert.ToDateTime(dr["periodo"].ToString())
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