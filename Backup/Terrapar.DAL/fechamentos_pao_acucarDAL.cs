using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Terrapar.Classes;
using Terrapar.DTO;

namespace Terrapar.DAL
{
    public class fechamentos_pao_acucarDAL : IDAL<fechamento_pao_acucar>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public fechamentos_pao_acucarDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(fechamento_pao_acucar DTO)
        {
            var sql =
                "INSERT INTO fechamentos_pao_acucar (qtd_horas_extras, qtd_diarias, valor_hora_extra_pao_acucar, valor_diaria_pao_acucar, salario_pao_acucar, valor_vale, inss_pao_acucar, id_usuario, data)" +
                " VALUES (" + DTO.qtd_horas_extras.ToDecimalMySql() + ", " + DTO.qtd_diarias.ToDecimalMySql() + ", " +
                                DTO.valor_hora_extra_pao_acucar.ToDecimalMySql() + ", " + DTO.valor_diaria_pao_acucar.ToDecimalMySql() + ", " +
                                DTO.salario_pao_acucar.ToDecimalMySql() + ", " + DTO.valor_vale.ToDecimalMySql() + ", " +
                                DTO.inss_pao_acucar.ToDecimalMySql() + ", " + DTO.id_usuario + ", '" + DTO.data.ToString("yyyy-MM-dd HH:mm") + "')";

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

        public int Deletar(fechamento_pao_acucar DTO)
        {
            var sql = "DELETE FROM fechamentos_pao_acucar";

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

        public int Atualizar(fechamento_pao_acucar DTO)
        {
            var sql = "UPDATE fechamentos_pao_acucar SET qtd_horas_extras = " + DTO.qtd_horas_extras.ToDecimalMySql() +
                      ", qtd_diarias = " + DTO.qtd_diarias.ToDecimalMySql() +
                      ", valor_hora_extra_pao_acucar = " + DTO.valor_hora_extra_pao_acucar.ToDecimalMySql() +
                      ", valor_diaria_pao_acucar = " + DTO.valor_diaria_pao_acucar.ToDecimalMySql() +
                      ", salario_pao_acucar = " + DTO.salario_pao_acucar.ToDecimalMySql() +
                      ", valor_vale = " + DTO.valor_vale.ToDecimalMySql() +
                      ", inss_pao_acucar = " + DTO.inss_pao_acucar.ToDecimalMySql() +
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

        public IEnumerable<fechamento_pao_acucar> Listar(fechamento_pao_acucar DTO)
        {
            ICollection<fechamento_pao_acucar> fechamentos = new List<fechamento_pao_acucar>();
            var sql = "SELECT *" +
                      "  FROM fechamentos_pao_acucar";

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
                        fechamentos.Add(new fechamento_pao_acucar()
                        {
                            id_fechamento_pao_acucar = Convert.ToInt32(dr["id_fechamento_pao_acucar"].ToString()),
                            qtd_horas_extras = Convert.ToDecimal(dr["qtd_horas_extras"].ToString()),
                            qtd_diarias = Convert.ToDecimal(dr["qtd_diarias"].ToString()),
                            valor_hora_extra_pao_acucar = Convert.ToDecimal(dr["valor_hora_extra_pao_acucar"].ToString()),
                            valor_diaria_pao_acucar = Convert.ToDecimal(dr["valor_diaria_pao_acucar"].ToString()),
                            salario_pao_acucar = Convert.ToDecimal(dr["salario_pao_acucar"].ToString()),
                            valor_vale = Convert.ToDecimal(dr["valor_vale"].ToString()),
                            inss_pao_acucar = Convert.ToDecimal(dr["inss_pao_acucar"].ToString()),
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

        public fechamento_pao_acucar RecuperarPorID(int id)
        {
            var sql = "SELECT *" +
                      "  FROM fechamentos_pao_acucar WHERE id_fechamento_pao_acucar = " + id;

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
                    return new fechamento_pao_acucar()
                    {
                        id_fechamento_pao_acucar = Convert.ToInt32(dr["id_fechamento_pao_acucar"].ToString()),
                        qtd_horas_extras = Convert.ToDecimal(dr["qtd_horas_extras"].ToString()),
                        qtd_diarias = Convert.ToDecimal(dr["qtd_diarias"].ToString()),
                        valor_hora_extra_pao_acucar = Convert.ToDecimal(dr["valor_hora_extra_pao_acucar"].ToString()),
                        valor_diaria_pao_acucar = Convert.ToDecimal(dr["valor_diaria_pao_acucar"].ToString()),
                        salario_pao_acucar = Convert.ToDecimal(dr["salario_pao_acucar"].ToString()),
                        valor_vale = Convert.ToDecimal(dr["valor_vale"].ToString()),
                        inss_pao_acucar = Convert.ToDecimal(dr["inss_pao_acucar"].ToString()),
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