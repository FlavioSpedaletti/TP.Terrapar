namespace Terrapar.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.debito_credito",
                c => new
                    {
                        id_debito_credito = c.Int(nullable: false, identity: true),
                        tipo = c.Int(nullable: false),
                        descricao = c.String(),
                        valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        id_usuario = c.Int(nullable: false),
                        data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_debito_credito);
            
            CreateTable(
                "dbo.fechamento_servicos",
                c => new
                    {
                        id_fechamento_servicos = c.Int(nullable: false, identity: true),
                        id_usuario = c.Int(nullable: false),
                        placa = c.String(),
                        periodo = c.DateTime(nullable: false),
                        soma_valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id_fechamento_servicos);
            
            CreateTable(
                "dbo.fechamento_viagens",
                c => new
                    {
                        id_fechamento_viagens = c.Int(nullable: false, identity: true),
                        id_usuario = c.Int(nullable: false),
                        placa = c.String(),
                        periodo = c.DateTime(nullable: false),
                        tipo_viagem = c.Int(nullable: false),
                        soma_frete = c.Decimal(nullable: false, precision: 18, scale: 2),
                        soma_abastecimento = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id_fechamento_viagens);
            
            CreateTable(
                "dbo.fechamento_pao_acucar",
                c => new
                    {
                        id_fechamento_pao_acucar = c.Int(nullable: false, identity: true),
                        qtd_horas_extras = c.Decimal(nullable: false, precision: 18, scale: 2),
                        qtd_diarias = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_hora_extra_pao_acucar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_diaria_pao_acucar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        salario_pao_acucar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_vale = c.Decimal(nullable: false, precision: 18, scale: 2),
                        inss_pao_acucar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        id_usuario = c.Int(nullable: false),
                        data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_fechamento_pao_acucar);
            
            CreateTable(
                "dbo.fechamento_pedreira",
                c => new
                    {
                        id_fechamento_pedreira = c.Int(nullable: false, identity: true),
                        valor_refeicao = c.Decimal(nullable: false, precision: 18, scale: 2),
                        salario_pedreira = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_vale = c.Decimal(nullable: false, precision: 18, scale: 2),
                        inss_pedreira = c.Decimal(nullable: false, precision: 18, scale: 2),
                        id_usuario = c.Int(nullable: false),
                        data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_fechamento_pedreira);
            
            CreateTable(
                "dbo.loja",
                c => new
                    {
                        id_loja = c.Int(nullable: false, identity: true),
                        identificacao = c.String(),
                        nome = c.String(),
                        identificacaoEnome = c.String(),
                    })
                .PrimaryKey(t => t.id_loja);
            
            CreateTable(
                "dbo.loja_viagem_pao_acucar",
                c => new
                    {
                        id_loja_viagem_pao_acucar = c.Int(nullable: false, identity: true),
                        id_viagem_pao_acucar = c.Int(nullable: false),
                        id_loja = c.Int(nullable: false),
                        valor_frete = c.Decimal(nullable: false, precision: 18, scale: 2),
                        numero_ordem_coleta = c.String(),
                    })
                .PrimaryKey(t => t.id_loja_viagem_pao_acucar);
            
            CreateTable(
                "dbo.parametro_geral",
                c => new
                    {
                        id_parametro_geral = c.Int(nullable: false, identity: true),
                        valor_refeicao = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_vale = c.Decimal(nullable: false, precision: 18, scale: 2),
                        salario_pao_acucar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_hora_extra_pao_acucar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_diaria_pao_acucar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        inss_pao_acucar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        salario_pedreira = c.Decimal(nullable: false, precision: 18, scale: 2),
                        inss_pedreira = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id_parametro_geral);
            
            CreateTable(
                "dbo.servico",
                c => new
                    {
                        id_servico = c.Int(nullable: false, identity: true),
                        descricao = c.String(),
                        valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        km = c.Int(nullable: false),
                        tipo = c.Int(nullable: false),
                        id_usuario = c.Int(nullable: false),
                        data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_servico);
            
            CreateTable(
                "dbo.usuario",
                c => new
                    {
                        id_usuario = c.Int(nullable: false, identity: true),
                        admin = c.Boolean(nullable: false),
                        nome_motorista = c.String(),
                        nome_caminhao = c.String(),
                        placa = c.String(),
                        senha = c.String(),
                        tipo = c.Int(nullable: false),
                        meta_comissao = c.Decimal(nullable: false, precision: 18, scale: 2),
                        troca_oleo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_usuario);
            
            CreateTable(
                "dbo.viagem_pao_acucar",
                c => new
                    {
                        id_viagem_pao_acucar = c.Int(nullable: false, identity: true),
                        id_usuario = c.Int(nullable: false),
                        data = c.DateTime(nullable: false),
                        km_final = c.Int(nullable: false),
                        qtd_diesel = c.Decimal(nullable: false, precision: 18, scale: 2),
                        km_abastecimento = c.Int(nullable: false),
                        valor_abastecimento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        domingo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_viagem_pao_acucar);
            
            CreateTable(
                "dbo.viagem_pao_acucar_destacada",
                c => new
                    {
                        id_viagem_pao_acucar_destacada = c.Int(nullable: false, identity: true),
                        id_viagem_pao_acucar = c.Int(nullable: false),
                        id_usuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_viagem_pao_acucar_destacada);
            
            CreateTable(
                "dbo.viagem_pedreira",
                c => new
                    {
                        id_viagem_pedreira = c.Int(nullable: false, identity: true),
                        id_usuario = c.Int(nullable: false),
                        data = c.DateTime(nullable: false),
                        origem = c.String(),
                        destino = c.String(),
                        numero_nota_fiscal = c.String(),
                        peso = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_frete = c.Decimal(nullable: false, precision: 18, scale: 2),
                        base_calculo_icms = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_icms = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_produtos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valor_nota_fiscal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        qtd_diesel = c.Decimal(nullable: false, precision: 18, scale: 2),
                        km_abastecimento = c.Int(nullable: false),
                        valor_abastecimento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        noite = c.Boolean(nullable: false),
                        domingo_feriado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_viagem_pedreira);
            
            CreateTable(
                "dbo.viagem_pedreira_destacada",
                c => new
                    {
                        id_viagem_pedreira_destacada = c.Int(nullable: false, identity: true),
                        id_viagem_pedreira = c.Int(nullable: false),
                        id_usuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_viagem_pedreira_destacada);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.viagem_pedreira_destacada");
            DropTable("dbo.viagem_pedreira");
            DropTable("dbo.viagem_pao_acucar_destacada");
            DropTable("dbo.viagem_pao_acucar");
            DropTable("dbo.usuario");
            DropTable("dbo.servico");
            DropTable("dbo.parametro_geral");
            DropTable("dbo.loja_viagem_pao_acucar");
            DropTable("dbo.loja");
            DropTable("dbo.fechamento_pedreira");
            DropTable("dbo.fechamento_pao_acucar");
            DropTable("dbo.fechamento_viagens");
            DropTable("dbo.fechamento_servicos");
            DropTable("dbo.debito_credito");
        }
    }
}
