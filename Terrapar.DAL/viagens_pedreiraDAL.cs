using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Terrapar.Classes;
using Terrapar.DTO;

namespace Terrapar.DAL
{
    public class viagens_pedreiraDAL : IDAL<viagem_pedreira>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public viagens_pedreiraDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(viagem_pedreira DTO)
        {
            var sql = "INSERT INTO viagens_pedreira (id_usuario, data, origem, destino, numero_nota_fiscal, peso, valor_frete, base_calculo_icms, valor_icms, " +
                      " valor_produtos, valor_nota_fiscal, qtd_diesel, km_abastecimento, valor_abastecimento, noite, domingo_feriado)" +
                      " VALUES (" + DTO.id_usuario + ", '" + DTO.data.ToString("yyyy-MM-dd HH:mm") + "', '" + DTO.origem.ToStringSegura() + "', '" + DTO.destino.ToStringSegura() + "', '" +
                      DTO.numero_nota_fiscal.ToStringSegura() + "', " + DTO.peso.ToDecimalMySql() + ", " +
                      DTO.valor_frete.ToDecimalMySql() + ", " + DTO.base_calculo_icms.ToDecimalMySql() + ", " +
                      DTO.valor_icms.ToDecimalMySql() + ", " + DTO.valor_produtos.ToDecimalMySql() + ", " +
                      DTO.valor_nota_fiscal.ToDecimalMySql() + ", " + DTO.qtd_diesel.ToDecimalMySql() + ", " +
                      DTO.km_abastecimento.ToIntMySql() + ", " + DTO.valor_abastecimento.ToDecimalMySql() + ", " +
                      DTO.noite + ", " + DTO.domingo_feriado + ")";

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

        public int Deletar(viagem_pedreira DTO)
        {
            var sql = "DELETE FROM viagens_pedreira" +
                      " WHERE id_viagem_pedreira = " + DTO.id_viagem_pedreira;

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

        public int Atualizar(viagem_pedreira DTO)
        {
            var sql = "UPDATE viagens_pedreira SET data = '" + DTO.data.ToString("yyyy-MM-dd HH:mm") + "', id_usuario = " + DTO.id_usuario +
                      ", origem = '" + DTO.origem.ToStringSegura() + "', " +
                      "destino = '" + DTO.destino.ToStringSegura() + "', " +
                      "numero_nota_fiscal = '" + DTO.numero_nota_fiscal.ToStringSegura() + "', peso = " +
                      DTO.peso.ToDecimalMySql() + ", " +
                      "valor_frete = " + DTO.valor_frete.ToDecimalMySql() +
                      ", base_calculo_icms = " + DTO.base_calculo_icms.ToDecimalMySql() + ", " +
                      "valor_icms = " + DTO.valor_icms.ToDecimalMySql() + ", valor_produtos = " +
                      DTO.valor_produtos.ToDecimalMySql() + ", valor_nota_fiscal = " + DTO.valor_nota_fiscal.ToDecimalMySql() +
                      ", qtd_diesel = " +
                      DTO.qtd_diesel.ToDecimalMySql() + ", km_abastecimento = " + DTO.km_abastecimento.ToIntMySql() +
                      ", valor_abastecimento = " + DTO.valor_abastecimento.ToDecimalMySql() +
                      ", noite = " + DTO.noite +
                      ", domingo_feriado = " + DTO.domingo_feriado +
                      " WHERE id_viagem_pedreira = " + DTO.id_viagem_pedreira;

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

        public IEnumerable<viagem_pedreira> Listar(viagem_pedreira DTO)
        {
            ICollection<viagem_pedreira> viagens = new List<viagem_pedreira>();
            var sql = "SELECT *" +
                      "  FROM viagens_pedreira";

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
                        viagens.Add(new viagem_pedreira()
                        {
                            id_viagem_pedreira = Convert.ToInt32(dr["id_viagem_pedreira"].ToString()),
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            data = Convert.ToDateTime(dr["data"].ToString()),
                            origem = dr["origem"].ToString(),
                            destino = dr["destino"].ToString(),
                            numero_nota_fiscal = dr["numero_nota_fiscal"].ToString(),
                            peso = Convert.ToDecimal(dr["peso"].ToString()),
                            valor_frete = Convert.ToDecimal(dr["valor_frete"].ToString()),
                            base_calculo_icms = Convert.ToDecimal(dr["base_calculo_icms"].ToString()),
                            valor_icms = Convert.ToDecimal(dr["valor_icms"].ToString()),
                            valor_produtos = Convert.ToDecimal(dr["valor_produtos"].ToString()),
                            valor_nota_fiscal = Convert.ToDecimal(dr["valor_nota_fiscal"].ToString()),
                            qtd_diesel = Convert.ToDecimal(dr["qtd_diesel"].ToString()),
                            km_abastecimento = Convert.ToInt32(dr["km_abastecimento"].ToString()),
                            valor_abastecimento = Convert.ToDecimal(dr["valor_abastecimento"].ToString()),
                            noite = Convert.ToBoolean(dr["noite"].ToString()),
                            domingo_feriado = Convert.ToBoolean(dr["domingo_feriado"].ToString())
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

        public IEnumerable<viagem_pedreira_folha_pgto> ListarParaFolhaPgto(viagem_pedreira DTO)
        {
            ICollection<viagem_pedreira_folha_pgto> viagens = new List<viagem_pedreira_folha_pgto>();
            var sql = "SELECT id_usuario, DATE_FORMAT(data, '%d/%m/%Y') dia, COUNT(id_viagem_pedreira) qtd_viagens, SUM(valor_frete) total_frete, " +
                      //"       CASE WHEN TIME(data) BETWEEN '00:00:00' AND '18:00:00' THEN 'antes_18' " +
                      //"            WHEN TIME(data) BETWEEN '18:00:01' AND '23:59:59' THEN 'depois_18' " +
                      //"       END as periodo " +
                      "       noite, domingo_feriado " +
                      "  FROM viagens_pedreira ";

            if (DTO != null)
                sql += " WHERE id_usuario = " + DTO.id_usuario;
            if (DTO.data != DateTime.MinValue)
                sql += " AND DATE_FORMAT(data,'%m/%Y') = '" + DTO.data.ToString("MM/yyyy") + "'";

            sql += " GROUP BY id_usuario, DATE_FORMAT(data, '%d/%m/%Y'), " +
                   //" CASE WHEN TIME(data) BETWEEN '00:00:00' AND '18:00:00' THEN 'antes_18' " +
                   //"      WHEN TIME(data) BETWEEN '18:00:01' AND '23:59:59' THEN 'depois_18' " +
                   //" END ";
                   " noite, domingo_feriado ";

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
                        viagens.Add(new viagem_pedreira_folha_pgto()
                        {
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            dia = Convert.ToDateTime(dr["dia"].ToString()),
                            qtd_viagens = Convert.ToInt32(dr["qtd_viagens"].ToString()),
                            total_frete = Convert.ToDecimal(dr["total_frete"].ToString()),
                            //periodo = dr["periodo"].ToString()
                            noite = Convert.ToBoolean(dr["noite"].ToString()),
                            domingo_feriado = Convert.ToBoolean(dr["domingo_feriado"].ToString())
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

        public viagem_pedreira RecuperarPorID(int id)
        {
            var sql = "SELECT *" +
                      "  FROM viagens_pedreira WHERE id_viagem_pedreira = " + id;

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
                    return new viagem_pedreira()
                    {
                        id_viagem_pedreira = Convert.ToInt32(dr["id_viagem_pedreira"].ToString()),
                        id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                        data = Convert.ToDateTime(dr["data"].ToString()),
                        origem = dr["origem"].ToString(),
                        destino = dr["destino"].ToString(),
                        numero_nota_fiscal = dr["numero_nota_fiscal"].ToString(),
                        peso = Convert.ToDecimal(dr["peso"].ToString()),
                        valor_frete = Convert.ToDecimal(dr["valor_frete"].ToString()),
                        base_calculo_icms = Convert.ToDecimal(dr["base_calculo_icms"].ToString()),
                        valor_icms = Convert.ToDecimal(dr["valor_icms"].ToString()),
                        valor_produtos = Convert.ToDecimal(dr["valor_produtos"].ToString()),
                        valor_nota_fiscal = Convert.ToDecimal(dr["valor_nota_fiscal"].ToString()),
                        qtd_diesel = Convert.ToDecimal(dr["qtd_diesel"].ToString()),
                        km_abastecimento = Convert.ToInt32(dr["km_abastecimento"].ToString()),
                        valor_abastecimento = Convert.ToDecimal(dr["valor_abastecimento"].ToString()),
                        noite = Convert.ToBoolean(dr["noite"].ToString()),
                        domingo_feriado = Convert.ToBoolean(dr["domingo_feriado"].ToString())
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