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
    public class parametros_geraisDAL : IDAL<parametro_geral>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public parametros_geraisDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(parametro_geral DTO)
        {
            var sql = "INSERT INTO parametros_gerais (valor_refeicao, valor_vale, salario_pao_acucar, valor_hora_extra_pao_acucar, valor_diaria_pao_acucar, inss_pao_acucar, salario_pedreira, inss_pedreira)" +
                      " VALUES (" + DTO.valor_refeicao.ToDecimalMySql() + ", " +
                      DTO.valor_vale.ToDecimalMySql() + ", " + DTO.salario_pao_acucar.ToDecimalMySql() + ", " +
                      DTO.valor_hora_extra_pao_acucar.ToDecimalMySql() + ", " + DTO.valor_diaria_pao_acucar.ToDecimalMySql() + ", " +
                      DTO.inss_pao_acucar.ToDecimalMySql() + ", " + DTO.salario_pedreira.ToDecimalMySql() + ", " + DTO.inss_pedreira.ToDecimalMySql() + ")";

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

        public int Deletar(parametro_geral DTO)
        {
            var sql = "DELETE FROM parametros_gerais" +
                      " WHERE id_parametro = " + DTO.id_parametro;

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

        public int Atualizar(parametro_geral DTO)
        {
            var sql = "UPDATE parametros_gerais SET valor_refeicao = " + DTO.valor_refeicao.ToDecimalMySql() + ", " +
                      "valor_vale = " + DTO.valor_vale.ToDecimalMySql() + ", salario_pao_acucar = " +
                      DTO.salario_pao_acucar.ToDecimalMySql() + ", " +
                      "valor_hora_extra_pao_acucar = " + DTO.valor_hora_extra_pao_acucar.ToDecimalMySql() +
                      ", valor_diaria_pao_acucar = " + DTO.valor_diaria_pao_acucar.ToDecimalMySql() + ", " +
                      "inss_pao_acucar = " + DTO.inss_pao_acucar.ToDecimalMySql() + ", salario_pedreira = " +
                      DTO.salario_pedreira.ToDecimalMySql() + ", inss_pedreira = " + DTO.inss_pedreira.ToDecimalMySql();
                      //" WHERE id_parametro = (select max(id_parametro) from parametros_gerais)";

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

        public IEnumerable<parametro_geral> Listar(parametro_geral DTO)
        {
            ICollection<parametro_geral> parametros_gerais = new List<parametro_geral>();
            var sql = "SELECT *" +
                      "  FROM parametros_gerais";

            //if (DTO != null)
            //    sql +=
            //        " WHERE ...";

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
                        parametros_gerais.Add(new parametro_geral()
                        {
                            id_parametro = Convert.ToInt32(dr["id_parametro"].ToString()),
                            valor_refeicao = Convert.ToDecimal(dr["valor_refeicao"].ToString()),
                            valor_vale = Convert.ToDecimal(dr["valor_vale"].ToString()),
                            salario_pao_acucar = Convert.ToDecimal(dr["salario_pao_acucar"].ToString()),
                            valor_hora_extra_pao_acucar = Convert.ToDecimal(dr["valor_hora_extra_pao_acucar"].ToString()),
                            valor_diaria_pao_acucar = Convert.ToDecimal(dr["valor_diaria_pao_acucar"].ToString()),
                            inss_pao_acucar = Convert.ToDecimal(dr["inss_pao_acucar"].ToString()),
                            salario_pedreira = Convert.ToDecimal(dr["salario_pedreira"].ToString()),
                            inss_pedreira = Convert.ToDecimal(dr["inss_pedreira"].ToString())
                        });
                    }
                    return parametros_gerais;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public parametro_geral RecuperarPorID(int id)
        {
            var sql = "SELECT * FROM parametros_gerais WHERE id_parametro = " + id;

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
                    return new parametro_geral()
                    {
                        id_parametro = Convert.ToInt32(dr["id_parametro"].ToString()),
                        valor_refeicao = Convert.ToDecimal(dr["valor_refeicao"].ToString()),
                        valor_vale = Convert.ToDecimal(dr["valor_vale"].ToString()),
                        salario_pao_acucar = Convert.ToDecimal(dr["salario_pao_acucar"].ToString()),
                        valor_hora_extra_pao_acucar = Convert.ToDecimal(dr["valor_hora_extra_pao_acucar"].ToString()),
                        valor_diaria_pao_acucar = Convert.ToDecimal(dr["valor_diaria_pao_acucar"].ToString()),
                        inss_pao_acucar = Convert.ToDecimal(dr["inss_pao_acucar"].ToString()),
                        salario_pedreira = Convert.ToDecimal(dr["salario_pedreira"].ToString()),
                        inss_pedreira = Convert.ToDecimal(dr["inss_pedreira"].ToString())
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