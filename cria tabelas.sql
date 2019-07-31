//tive que fazer algumas alterçaões do script gerado pelo migrations
//	substituir "[", "]" e "dbo" por ""
//	IDENTITY -> AUTO_INCREMENT
//	nvarchar(max) -> varchar(5000)
//	colocar ";" no final de cada criação de tabela

CREATE TABLE debito_credito (
    id_debito_credito int NOT NULL AUTO_INCREMENT,
    tipo int NOT NULL,
    descricao varchar(5000),
    valor decimal(18, 2) NOT NULL,
    id_usuario int NOT NULL,
    data datetime NOT NULL,
    CONSTRAINT PK_debito_credito PRIMARY KEY (id_debito_credito)
);
//isso aqui na verdade é uma view, mas não tenho o script :(
CREATE TABLE fechamento_servicos (
    id_fechamento_servicos int NOT NULL AUTO_INCREMENT,
    id_usuario int NOT NULL,
    placa varchar(5000),
    periodo datetime NOT NULL,
    soma_valor decimal(18, 2) NOT NULL,
    CONSTRAINT PK_fechamento_servicos PRIMARY KEY (id_fechamento_servicos)
);
//isso aqui na verdade é uma view, mas não tenho o script :(
CREATE TABLE fechamento_viagens (
    id_fechamento_viagens int NOT NULL AUTO_INCREMENT,
    id_usuario int NOT NULL,
    placa varchar(5000),
    periodo datetime NOT NULL,
    tipo_viagem int NOT NULL,
    soma_frete decimal(18, 2) NOT NULL,
    soma_abastecimento decimal(18, 2) NOT NULL,
    CONSTRAINT PK_fechamento_viagens PRIMARY KEY (id_fechamento_viagens)
);
CREATE TABLE fechamento_pao_acucar (
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
CREATE TABLE fechamento_pedreira (
    id_fechamento_pedreira int NOT NULL AUTO_INCREMENT,
    valor_refeicao decimal(18, 2) NOT NULL,
    salario_pedreira decimal(18, 2) NOT NULL,
    valor_vale decimal(18, 2) NOT NULL,
    inss_pedreira decimal(18, 2) NOT NULL,
    id_usuario int NOT NULL,
    data datetime NOT NULL,
    CONSTRAINT PK_fechamento_pedreira PRIMARY KEY (id_fechamento_pedreira)
);
CREATE TABLE loja (
    id_loja int NOT NULL AUTO_INCREMENT,
    identificacao varchar(5000),
    nome varchar(5000),
    identificacaoEnome varchar(5000),
    CONSTRAINT PK_loja PRIMARY KEY (id_loja)
);
CREATE TABLE loja_viagem_pao_acucar (
    id_loja_viagem_pao_acucar int NOT NULL AUTO_INCREMENT,
    id_viagem_pao_acucar int NOT NULL,
    id_loja int NOT NULL,
    valor_frete decimal(18, 2) NOT NULL,
    numero_ordem_coleta varchar(5000),
    CONSTRAINT PK_loja_viagem_pao_acucar PRIMARY KEY (id_loja_viagem_pao_acucar)
);
CREATE TABLE parametro_geral (
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
CREATE TABLE servico (
    id_servico int NOT NULL AUTO_INCREMENT,
    descricao varchar(5000),
    valor decimal(18, 2) NOT NULL,
    km int NOT NULL,
    tipo int NOT NULL,
    id_usuario int NOT NULL,
    data datetime NOT NULL,
    CONSTRAINT PK_servico PRIMARY KEY (id_servico)
);
CREATE TABLE usuario (
    id_usuario int NOT NULL AUTO_INCREMENT,
    admin bit NOT NULL,
    nome_motorista varchar(5000),
    nome_caminhao varchar(5000),
    placa varchar(5000),
    senha varchar(5000),
    tipo int NOT NULL,
    meta_comissao decimal(18, 2) NOT NULL,
    troca_oleo int NOT NULL,
    CONSTRAINT PK_usuario PRIMARY KEY (id_usuario)
);
CREATE TABLE viagem_pao_acucar (
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
CREATE TABLE viagem_pao_acucar_destacada (
    id_viagem_pao_acucar_destacada int NOT NULL AUTO_INCREMENT,
    id_viagem_pao_acucar int NOT NULL,
    id_usuario int NOT NULL,
    CONSTRAINT PK_viagem_pao_acucar_destacada PRIMARY KEY (id_viagem_pao_acucar_destacada)
);
CREATE TABLE viagem_pedreira (
    id_viagem_pedreira int NOT NULL AUTO_INCREMENT,
    id_usuario int NOT NULL,
    data datetime NOT NULL,
    origem varchar(5000),
    destino varchar(5000),
    numero_nota_fiscal varchar(5000),
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
CREATE TABLE viagem_pedreira_destacada (
    id_viagem_pedreira_destacada int NOT NULL AUTO_INCREMENT,
    id_viagem_pedreira int NOT NULL,
    id_usuario int NOT NULL,
    CONSTRAINT PK_viagem_pedreira_destacada PRIMARY KEY (id_viagem_pedreira_destacada)
);