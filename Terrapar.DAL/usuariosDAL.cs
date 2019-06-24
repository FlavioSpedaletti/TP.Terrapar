using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Terrapar.Classes;
using Terrapar.DTO;

namespace Terrapar.DAL
{
    public class usuariosDAL : IDAL<usuario>
    {
        String _conexaoMySQL = "";
        MySqlConnection con = null;

        public usuariosDAL()
        {
            _conexaoMySQL = ConfigurationManager.ConnectionStrings["terrapar"].ConnectionString;
        }

        public long Adicionar(usuario DTO)
        {
            var sql = "INSERT INTO usuarios (admin, nome_motorista, nome_caminhao, placa, senha, tipo, meta_comissao, troca_oleo)" +
                      " VALUES (" + DTO.admin + ", '" + DTO.nome_motorista.ToStringSegura() + "', '" +
                      DTO.nome_caminhao.ToStringSegura() + "', '" + DTO.placa.ToStringSegura() + "', '" +
                      DTO.senha.ToStringSegura() + "', " + DTO.tipo + ", " + DTO.meta_comissao.ToDecimalMySql() + ", " + DTO.troca_oleo.ToIntMySql() + ")";

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

        public int Deletar(usuario DTO)
        {
            var sql = "DELETE FROM usuarios" +
                      " WHERE id_usuario = " + DTO.id_usuario;

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

        public int Atualizar(usuario DTO)
        {
            var sql = "UPDATE usuarios SET admin = " + DTO.admin + ", nome_motorista = '" + DTO.nome_motorista.ToStringSegura() + "', nome_caminhao = '" +
                      DTO.nome_caminhao.ToStringSegura() + "', placa = '" + DTO.placa.ToStringSegura() + "', senha = '" +
                      DTO.senha.ToStringSegura() + "', tipo = " + DTO.tipo + ", meta_comissao = " + DTO.meta_comissao.ToDecimalMySql() + ", troca_oleo = " + DTO.troca_oleo.ToIntMySql() +
                      " WHERE id_usuario = " + DTO.id_usuario;

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

        public IEnumerable<usuario> Listar(usuario DTO)
        {
            ICollection<usuario> usuarios = new List<usuario>();
            var sql = "SELECT *" +
                      "  FROM usuarios";

            if (DTO != null)
            {
                sql += " WHERE placa like '%" + DTO.placa.ToStringSegura() + "%'" +
                        "   AND nome_motorista like '%" + DTO.nome_motorista.ToStringSegura() + "%'" +
                        "   AND nome_caminhao like '%" + DTO.nome_caminhao.ToStringSegura() + "%'" +
                        "   AND (tipo = '" + DTO.tipo + "' or '" +
                        DTO.tipo.ToString().ToStringSegura() + "' = '-1')";
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
                        usuarios.Add(new usuario
                        {
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            admin = dr["admin"].ToString() == "1" ? true : false,
                            nome_motorista = dr["nome_motorista"].ToString(),
                            nome_caminhao = dr["nome_caminhao"].ToString(),
                            placa = dr["placa"].ToString(),
                            senha = dr["senha"].ToString(),
                            tipo = Convert.ToInt32(dr["tipo"].ToString()),
                            meta_comissao = Convert.ToDecimal(dr["meta_comissao"].ToString()),
                            troca_oleo = Convert.ToInt32(dr["troca_oleo"].ToString())
                        });
                    }
                    return usuarios;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<usuario> ListarComViagem()
        {
            ICollection<usuario> usuarios = new List<usuario>();
            var sql = "SELECT DISTINCT * " +
                      "  FROM usuarios u " +
                      " WHERE EXISTS (SELECT 1 FROM viagens_pao_acucar vpa WHERE vpa.id_usuario = u.id_usuario) " +
                      "    OR EXISTS (SELECT 1 FROM viagens_pedreira vp WHERE vp.id_usuario = u.id_usuario) ";
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
                        usuarios.Add(new usuario
                        {
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            admin = dr["admin"].ToString() == "1" ? true : false,
                            nome_motorista = dr["nome_motorista"].ToString(),
                            nome_caminhao = dr["nome_caminhao"].ToString(),
                            placa = dr["placa"].ToString(),
                            senha = dr["senha"].ToString(),
                            tipo = Convert.ToInt32(dr["tipo"].ToString()),
                            meta_comissao = Convert.ToDecimal(dr["meta_comissao"].ToString()),
                            troca_oleo = Convert.ToInt32(dr["troca_oleo"].ToString())
                        });
                    }
                    return usuarios;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<usuario> ListarComServico()
        {
            ICollection<usuario> usuarios = new List<usuario>();
            var sql = "SELECT DISTINCT * " +
                      "  FROM usuarios u " +
                      " WHERE EXISTS (SELECT 1 FROM servicos s WHERE s.id_usuario = u.id_usuario) ";
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
                        usuarios.Add(new usuario
                        {
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                            admin = dr["admin"].ToString() == "1" ? true : false,
                            nome_motorista = dr["nome_motorista"].ToString(),
                            nome_caminhao = dr["nome_caminhao"].ToString(),
                            placa = dr["placa"].ToString(),
                            senha = dr["senha"].ToString(),
                            tipo = Convert.ToInt32(dr["tipo"].ToString()),
                            meta_comissao = Convert.ToDecimal(dr["meta_comissao"].ToString()),
                            troca_oleo = Convert.ToInt32(dr["troca_oleo"].ToString())
                        });
                    }
                    return usuarios;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public usuario RecuperarPorID(int id)
        {
            var sql = "SELECT * FROM usuarios WHERE id_usuario = " + id;

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
                    return new usuario
                    {
                        id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                        admin = dr["admin"].ToString() == "1" ? true : false,
                        nome_motorista = dr["nome_motorista"].ToString(),
                        nome_caminhao = dr["nome_caminhao"].ToString(),
                        placa = dr["placa"].ToString(),
                        senha = dr["senha"].ToString(),
                        tipo = Convert.ToInt32(dr["tipo"].ToString()),
                        meta_comissao = Convert.ToDecimal(dr["meta_comissao"].ToString()),
                        troca_oleo = Convert.ToInt32(dr["troca_oleo"].ToString())
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public usuario RecuperarParaLogin(string placa, string senha)
        {
            var sql = "SELECT * FROM usuarios WHERE placa = '" + placa.ToStringSegura() + "' AND senha ='" + senha.ToStringSegura() + "'";

            try
            {
                using (con = new MySqlConnection(_conexaoMySQL))
                {
                    var da = new MySqlDataAdapter();
                    var dt = new DataTable();
                    da.SelectCommand = new MySqlCommand(sql, con);
                    da.Fill(dt);

                    if(dt.Rows.Count==0) return null;

                    var dr = dt.Rows[0];
                    return new usuario
                    {
                        id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()),
                        admin = dr["admin"].ToString() == "1" ? true : false,
                        nome_motorista = dr["nome_motorista"].ToString(),
                        nome_caminhao = dr["nome_caminhao"].ToString(),
                        placa = dr["placa"].ToString(),
                        senha = dr["senha"].ToString(),
                        tipo = Convert.ToInt32(dr["tipo"].ToString()),
                        meta_comissao = Convert.ToDecimal(dr["meta_comissao"].ToString()),
                        troca_oleo = Convert.ToInt32(dr["troca_oleo"].ToString())
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