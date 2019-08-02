//tive que fazer algumas alterçaões do script gerado pelo migrations
//	substituir "[", "]" e "dbo" por ""
//	IDENTITY -> AUTO_INCREMENT
//	nvarchar(max) -> varchar(500)
//	colocar ";" no final de cada criação de tabela
//	verificar plural no nome das tabelas

CREATE DATABASE terrapartransp;
USE terrapartransp;

CREATE TABLE debitos_creditos (
    id_debito_credito int NOT NULL AUTO_INCREMENT,
    tipo int NOT NULL,
    descricao varchar(500),
    valor decimal(18, 2) NOT NULL,
    id_usuario int NOT NULL,
    data datetime NOT NULL,
    CONSTRAINT PK_debito_credito PRIMARY KEY (id_debito_credito)
);
/*isso aqui na verdade é uma view, mas não tenho o script :(*/
CREATE TABLE fechamento_servicos (
    id_fechamento_servicos int NOT NULL AUTO_INCREMENT,
    id_usuario int NOT NULL,
    placa varchar(500),
    periodo datetime NOT NULL,
    soma_valor decimal(18, 2) NOT NULL,
    CONSTRAINT PK_fechamento_servicos PRIMARY KEY (id_fechamento_servicos)
);
/*isso aqui na verdade é uma view, mas não tenho o script :(*/
CREATE TABLE fechamento_viagens (
    id_fechamento_viagens int NOT NULL AUTO_INCREMENT,
    id_usuario int NOT NULL,
    placa varchar(500),
    periodo datetime NOT NULL,
    tipo_viagem int NOT NULL,
    soma_frete decimal(18, 2) NOT NULL,
    soma_abastecimento decimal(18, 2) NOT NULL,
    CONSTRAINT PK_fechamento_viagens PRIMARY KEY (id_fechamento_viagens)
);
CREATE TABLE fechamentos_pao_acucar (
    id_fechamento_pao_acucar int NOT NULL AUTO_INCREMENT,
    qtd_horas_extras decimal(18, 2) NOT NULL,
    qtd_diarias decimal(18, 2) NOT NULL,
    valor_hora_extra_pao_acucar decimal(18, 2) NOT NULL,
    valor_diaria_pao_acucar decimal(18, 2) NOT NULL,
    salario_pao_acucar decimal(18, 2) NOT NULL,
    valor_vale decimal(18, 2) NOT NULL,
    inss_pao_acucar decimal(18, 2) NOT NULL,
    id_usuario int NOT NULL,
    data datetime NOT NULL,
    CONSTRAINT PK_fechamento_pao_acucar PRIMARY KEY (id_fechamento_pao_acucar)
);
CREATE TABLE fechamentos_pedreira (
    id_fechamento_pedreira int NOT NULL AUTO_INCREMENT,
    valor_refeicao decimal(18, 2) NOT NULL,
    salario_pedreira decimal(18, 2) NOT NULL,
    valor_vale decimal(18, 2) NOT NULL,
    inss_pedreira decimal(18, 2) NOT NULL,
    id_usuario int NOT NULL,
    data datetime NOT NULL,
    CONSTRAINT PK_fechamento_pedreira PRIMARY KEY (id_fechamento_pedreira)
);
CREATE TABLE lojas (
    id_loja int NOT NULL AUTO_INCREMENT,
    identificacao varchar(500),
    nome varchar(500),
    identificacaoEnome varchar(500),
    CONSTRAINT PK_loja PRIMARY KEY (id_loja)
);
CREATE TABLE lojas_viagem_pao_acucar (
    id_loja_viagem_pao_acucar int NOT NULL AUTO_INCREMENT,
    id_viagem_pao_acucar int NOT NULL,
    id_loja int NOT NULL,
    valor_frete decimal(18, 2) NOT NULL,
    numero_ordem_coleta varchar(500),
    CONSTRAINT PK_loja_viagem_pao_acucar PRIMARY KEY (id_loja_viagem_pao_acucar)
);
CREATE TABLE parametros_gerais (
    id_parametro_geral int NOT NULL AUTO_INCREMENT,
    valor_refeicao decimal(18, 2) NOT NULL,
    valor_vale decimal(18, 2) NOT NULL,
    salario_pao_acucar decimal(18, 2) NOT NULL,
    valor_hora_extra_pao_acucar decimal(18, 2) NOT NULL,
    valor_diaria_pao_acucar decimal(18, 2) NOT NULL,
    inss_pao_acucar decimal(18, 2) NOT NULL,
    salario_pedreira decimal(18, 2) NOT NULL,
    inss_pedreira decimal(18, 2) NOT NULL,
    CONSTRAINT PK_parametro_geral PRIMARY KEY (id_parametro_geral)
);
CREATE TABLE servicos (
    id_servico int NOT NULL AUTO_INCREMENT,
    descricao varchar(500),
    valor decimal(18, 2) NOT NULL,
    km int NOT NULL,
    tipo int NOT NULL,
    id_usuario int NOT NULL,
    data datetime NOT NULL,
    CONSTRAINT PK_servico PRIMARY KEY (id_servico)
);
CREATE TABLE usuarios (
    id_usuario int NOT NULL AUTO_INCREMENT,
    admin bit NOT NULL,
    nome_motorista varchar(500),
    nome_caminhao varchar(500),
    placa varchar(500),
    senha varchar(500),
    tipo int NOT NULL,
    meta_comissao decimal(18, 2) NOT NULL,
    troca_oleo int NOT NULL,
    CONSTRAINT PK_usuario PRIMARY KEY (id_usuario)
);
CREATE TABLE viagens_pao_acucar (
    id_viagem_pao_acucar int NOT NULL AUTO_INCREMENT,
    id_usuario int NOT NULL,
    data datetime NOT NULL,
    km_final int NOT NULL,
    qtd_diesel decimal(18, 2) NOT NULL,
    km_abastecimento int NOT NULL,
    valor_abastecimento decimal(18, 2) NOT NULL,
    domingo bit NOT NULL,
    CONSTRAINT PK_viagem_pao_acucar PRIMARY KEY (id_viagem_pao_acucar)
);
CREATE TABLE viagens_pao_acucar_destacadas (
    id_viagem_pao_acucar_destacada int NOT NULL AUTO_INCREMENT,
    id_viagem_pao_acucar int NOT NULL,
    id_usuario int NOT NULL,
    CONSTRAINT PK_viagem_pao_acucar_destacada PRIMARY KEY (id_viagem_pao_acucar_destacada)
);
CREATE TABLE viagens_pedreira (
    id_viagem_pedreira int NOT NULL AUTO_INCREMENT,
    id_usuario int NOT NULL,
    data datetime NOT NULL,
    origem varchar(500),
    destino varchar(500),
    numero_nota_fiscal varchar(500),
    peso decimal(18, 2) NOT NULL,
    valor_frete decimal(18, 2) NOT NULL,
    base_calculo_icms decimal(18, 2) NOT NULL,
    valor_icms decimal(18, 2) NOT NULL,
    valor_produtos decimal(18, 2) NOT NULL,
    valor_nota_fiscal decimal(18, 2) NOT NULL,
    qtd_diesel decimal(18, 2) NOT NULL,
    km_abastecimento int NOT NULL,
    valor_abastecimento decimal(18, 2) NOT NULL,
    noite bit NOT NULL,
    domingo_feriado bit NOT NULL,
    CONSTRAINT PK_viagem_pedreira PRIMARY KEY (id_viagem_pedreira)
);
CREATE TABLE viagens_pedreira_destacadas (
    id_viagem_pedreira_destacada int NOT NULL AUTO_INCREMENT,
    id_viagem_pedreira int NOT NULL,
    id_usuario int NOT NULL,
    CONSTRAINT PK_viagem_pedreira_destacada PRIMARY KEY (id_viagem_pedreira_destacada)
);
INSERT INTO usuarios VALUES (
	NULL, 1, 'Flavio Pao', 'Caminhao Pao', 'pao1111', '123456', 0, 950, 1
);
 INSERT INTO parametros_gerais VALUES (
	NULL, 15, 22, 1200, 156, 58, 3, 1350, 4
);