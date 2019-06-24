using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Terrapar.Classes;
using Terrapar.DTO;

namespace Terrapar.DAL
{
    public class fechamentos_pedreiraDAL : IDAL<fechamento_pedreira>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public fechamentos_pedreiraDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(fechamento_pedreira DTO)
        {
            var sql =
                "INSERT INTO fechamentos_pedreira (valor_refeicao, salario_pedreira, valor_vale, inss_pedreira, id_usuario, data)" +
                " VALUES (" + DTO.valor_refeicao.ToDecimalMySql() + ", " +
                                DTO.salario_pedreira.ToDecimalMySql() + ", " + DTO.valor_vale.ToDecimalMySql() + ", " +
                                DTO.inss_pedreira.ToDecimalMySql() + ", " + DTO.id_usuario + ", '" + DTO.data.ToString("yyyy-MM-dd HH:mm") + "')";

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

        public int Deletar(fechamento_pedreira DTO)
        {
            var sql = "DELETE FROM fechamentos_pedreira";

            if (DTO != null)
                sql += " WHERE id_usuario = " + DTO.id_usuario;
            if (DTO.data != DateTime.MinValue)
                sql += " AND DATE_FORMAT(data,'%m/%Y') = '" + DTO.data.ToString("MM/yyyy") + "'";

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

        public int Atualizar(fechamento_pedreira DTO)
        {
            var sql = "UPDATE fechamentos_pedreira SET valor_refeicao = " + DTO.valor_refeicao.ToDecimalMySql() +
                      ", salario_pedreira = " + DTO.salario_pedreira.ToDecimalMySql() +
                      ", valor_vale = " + DTO.valor_vale.ToDecimalMySql() +
                      ", inss_pedreira = " + DTO.inss_pedreira.ToDecimalMySql() +
                      ", id_usuario = " + DTO.id_usuario + ", data = '" + DTO.data.ToString("yyyy-MM-dd HH:mm") + "'";

            if (DTO != null)
                sql += " WHERE id_usuario = " + DTO.id_usuario;
            if (DTO.data != DateTime.MinValue)
                sql += " AND DATE_FORMAT(data,'%m/%Y') = '" + DTO.data.ToString("MM/yyyy") + "'";

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

        public IEnumerable<fechamento_pedreira> Listar(fechamento_pedreira DTO)
        {
            ICollection<fechamento_pedreira> fechamentos = new List<fechamento_pedreira>();
            var sql = "SELECT *" +
                      "  FROM fechamentos_pedreira";

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
                        fechamentos.Add(new fechamento_pedreira()
                        {
                            id_fechamento_pedreira = Convert.ToInt32(dr["id_fechamento_pedreira"].ToString()),
                            valor_refeicao = Convert.ToDecimal(dr["valor_refeicao"].ToString()),
                            salario_pedreira = Convert.ToDecimal(dr["salario_pedreira"].ToString()),
                            valor_vale = Convert.ToDecimal(dr["valor_vale"].ToString()),
                            inss_pedreira = Convert.ToDecimal(dr["inss_pedreira"].ToString()),
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            data = Convert.ToDateTime(dr["data"].ToString())
                        });
                    }
                    return fechamentos;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public fechamento_pedreira RecuperarPorID(int id)
        {
            var sql = "SELECT *" +
                      "  FROM fechamentos_pedreira WHERE id_fechamento_pedreira = " + id;

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
                    return new fechamento_pedreira()
                    {
                        id_fechamento_pedreira = Convert.ToInt32(dr["id_fechamento_pedreira"].ToString()),
                        valor_refeicao = Convert.ToDecimal(dr["valor_refeicao"].ToString()),
                        salario_pedreira = Convert.ToDecimal(dr["salario_pedreira"].ToString()),
                        valor_vale = Convert.ToDecimal(dr["valor_vale"].ToString()),
                        inss_pedreira = Convert.ToDecimal(dr["inss_pedreira"].ToString()),
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