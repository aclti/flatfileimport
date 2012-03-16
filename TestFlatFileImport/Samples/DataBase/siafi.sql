use luberabamg

--EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'

--drop table SiafiEmitente
--drop table SiafiPrestador
--drop table SiafiTomador
--drop table SiafiRegistro
--drop table siafi

CREATE TABLE Siafi
( 
	IdSiafi              int IDENTITY ( 1,1 ) ,
	CodConvenio          varchar(20)  NULL ,
	NumRemessa           int  NULL ,
	Mes                  int  NULL ,
	Ano                  int  NULL ,
	Decendio             int  NULL ,
	TotalRegistros       int  NULL ,
	ValorTotalRecebido   money  NULL ,
	DtGeracao            datetime  NULL ,
	DtRecebimento        datetime  NULL ,
	DtProcessamento      datetime  NULL ,
	Arquivo              varchar(128)  NULL ,
	Status               char(1)  NULL ,
	NumVersao            varchar(5)  NULL 
)
go

ALTER TABLE Siafi
	ADD CONSTRAINT XPKSiafi PRIMARY KEY (IdSiafi ASC)
go

CREATE TABLE SiafiEmitente
( 
	IdSiafiEmitente      int IDENTITY ( 1,1 ) ,
	IdSiafiRegistro      int  NULL ,
	CodUnidadeGestora    int  NULL ,
	CodGestao            int  NULL 
)
go

ALTER TABLE SiafiEmitente
	ADD CONSTRAINT XPKSiafiEmitente PRIMARY KEY (IdSiafiEmitente ASC)
go

CREATE NONCLUSTERED INDEX XIF1SiafiEmitente ON SiafiEmitente
( 
	IdSiafiRegistro       ASC
)
go

CREATE TABLE SiafiPrestador
( 
	IdSiafiPrestador     int IDENTITY ( 1,1 ) ,
	IdSiafiRegistro      int  NULL ,
	IdContribuinte       int  NULL ,
	NumDocReceita        varchar(14)  NULL 
)
go

ALTER TABLE SiafiPrestador
	ADD CONSTRAINT XPKSiafiPrestador PRIMARY KEY (IdSiafiPrestador ASC)
go

CREATE NONCLUSTERED INDEX XIF1SiafiPrestador ON SiafiPrestador
( 
	IdContribuinte        ASC
)
go

CREATE NONCLUSTERED INDEX XIF2SiafiPrestador ON SiafiPrestador
( 
	IdSiafiRegistro       ASC
)
go

CREATE TABLE SiafiRegistro
( 
	IdSiafiRegistro      int IDENTITY ( 1,1 ) ,
	IdSiafi              int  NULL ,
	DtEmissao            datetime  NULL ,
	DtVencimento         datetime  NULL ,
	NumDocumento         varchar(12)  NULL ,
	CodMunNfse           varchar(6)  NULL ,
	CodigoReceita        varchar(5)  NULL ,
	EsferaReceita        char(1)  NULL ,
	Mes                  int  NULL ,
	Ano                  int  NULL ,
	ValorPrincipal       money  NULL ,
	ValorMulta           money  NULL ,
	ValorJuros           money  NULL ,
	NumeroNota           varchar(10)  NULL ,
	SerieNota            varchar(5)  NULL ,
	SubSerieNota         varchar(2)  NULL ,
	DtEmissaoNota        datetime  NULL ,
	ValorNota            money  NULL ,
	Aliquota             money  NULL ,
	ValorBaseCalc        money  NULL ,
	Observacao           varchar(234)  NULL ,
	CodMunFavorecido     varchar(6)  NULL 
)
go

ALTER TABLE SiafiRegistro
	ADD CONSTRAINT XPKSiafiDetails PRIMARY KEY (IdSiafiRegistro ASC)
go

CREATE NONCLUSTERED INDEX XIF1SiafiDetails ON SiafiRegistro
( 
	IdSiafi               ASC
)
go

CREATE TABLE SiafiTomador
( 
	IdSiafiTomador       int IDENTITY ( 1,1 ) ,
	IdSiafiRegistro      int  NULL ,
	IdSubstituto         int  NULL ,
	CodUnidGestora       int  NULL ,
	Cnpj                 varchar(14)  NULL ,
	CodMunicipio         int  NULL 
)
go

ALTER TABLE SiafiTomador
	ADD CONSTRAINT XPKSiafiTomador PRIMARY KEY (IdSiafiTomador ASC)
go

CREATE NONCLUSTERED INDEX XIF1SiafiTomador ON SiafiTomador
( 
	IdSiafiRegistro       ASC
)
go

CREATE NONCLUSTERED INDEX XIF2SiafiTomador ON SiafiTomador
( 
	IdSubstituto          ASC
)
go

ALTER TABLE SiafiEmitente
	ADD CONSTRAINT FK_SiafiEmitenteFromSiafiRegistro FOREIGN KEY (IdSiafiRegistro) REFERENCES SiafiRegistro(IdSiafiRegistro)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE SiafiPrestador
	ADD CONSTRAINT FK_SiafiPrestadorFromContribuinte FOREIGN KEY (IdContribuinte) REFERENCES Contribuinte(IdContribuinte)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE SiafiPrestador
	ADD CONSTRAINT FK_SiafiPrestadorFromSiafiRegistro FOREIGN KEY (IdSiafiRegistro) REFERENCES SiafiRegistro(IdSiafiRegistro)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE SiafiRegistro
	ADD CONSTRAINT FK_SiafiRegistroFromSiafi FOREIGN KEY (IdSiafi) REFERENCES Siafi(IdSiafi)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE SiafiTomador
	ADD CONSTRAINT FK_SiafiTomadorFromSiafiRegistro FOREIGN KEY (IdSiafiRegistro) REFERENCES SiafiRegistro(IdSiafiRegistro)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE SiafiTomador
	ADD CONSTRAINT FK_SiafiTomadorFromContribuinte FOREIGN KEY (IdSubstituto) REFERENCES Contribuinte(IdContribuinte)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go