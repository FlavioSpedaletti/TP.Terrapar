using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Terrapar.DTO
{
    //Essa classe foi criada apenas para gerar os scripts SQL
    //Não uso EF nesse projeto, a conexão com DB é feita via ADO puro
    //Para gerar scripts:
    //  Enable-Migrations
    //  Add-Migration inicial
    //  Update-Database -script
    public class contexto : DbContext
    {
        public contexto() : base()
        {

        }

        DbSet<debito_credito> debitos_creditos { get; set; }
        DbSet<fechamento_pao_acucar> fechamentos_pao_acucar { get; set; }
        DbSet<fechamento_pedreira> fechamentos_pedreira { get; set; }
        DbSet<fechamento_servicos> fechamento_servicos { get; set; }
        DbSet<fechamento_viagens> fechamento_viagens { get; set; }
        DbSet<loja> lojas { get; set; }
        DbSet<loja_viagem_pao_acucar> lojas_viagem_pao_acucar { get; set; }
        DbSet<parametro_geral> parametros_gerais { get; set; }
        DbSet<servico> servicos { get; set; }
        DbSet<usuario> usuarios { get; set; }
        DbSet<viagem_pao_acucar> viagens_pao_acucar { get; set; }
        DbSet<viagem_pao_acucar_destacada> viagens_pao_acucar_destacadas { get; set; }
        DbSet<viagem_pedreira> viagens_pedreira { get; set; }
        DbSet<viagem_pedreira_destacada> viagens_pedreira_destacadas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Properties()
                .Where(p => p.Name == "id_" + p.ReflectedType.Name)
                .Configure(p => p.IsKey());
        }
    }
}