use luberabamg

CREATE TABLE SiafiDetails
( 
	IdSiafiDetails       int IDENTITY ( 1,1 ) ,
	IdSiafiHeader        int  NULL ,
	DtEmissao            datetime  NULL ,
	DtVenciemnto         datetime  NULL ,
	NumDocumento         varchar(12)  NULL ,
	CodUnidGestEmit      int  NULL ,
	CodGestEmit          int  NULL ,
	CotUnidGestTom       int  NULL ,
	CnpjUnidGestTom      varchar(14)  NULL ,
	CodMunUnidGestTom    varchar(6)  NULL ,
	NumDocReceitaSubstituto varchar(14)  NULL ,
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



ALTER TABLE SiafiDetails
	ADD CONSTRAINT XPKSiafiDetails PRIMARY KEY (IdSiafiDetails ASC)
go



CREATE NONCLUSTERED INDEX XIF1SiafiDetails ON SiafiDetails
( 
	IdSiafiHeader         ASC
)
go



CREATE TABLE SiafiHeader
( 
	IdSiafiHeader        int IDENTITY ( 1,1 ) ,
	CodConvenio          varchar(20)  NULL ,
	DtGeracao            datetime  NULL ,
	NumRemessa           int  NULL ,
	NumVersao            varchar(2)  NULL ,
	Mes                  int  NULL ,
	Ano                  int  NULL ,
	Decendio             int  NULL 
)
go



ALTER TABLE SiafiHeader
	ADD CONSTRAINT XPKSiafiHeader PRIMARY KEY (IdSiafiHeader ASC)
go



CREATE TABLE SiafiTrailer
( 
	IdSiafiTrailer       int IDENTITY ( 1,1 ) ,
	IdSiafiHeader        int  NULL ,
	NumSeqRegistro       int  NULL ,
	TotalRegistros       int  NULL ,
	ValorTotalRecebido   money  NULL 
)
go



ALTER TABLE SiafiTrailer
	ADD CONSTRAINT XPKSiafiTriller PRIMARY KEY (IdSiafiTrailer ASC)
go



CREATE NONCLUSTERED INDEX XIF1SiafiTriller ON SiafiTrailer
( 
	IdSiafiHeader         ASC
)
go




ALTER TABLE SiafiDetails
	ADD CONSTRAINT R_714 FOREIGN KEY (IdSiafiHeader) REFERENCES SiafiHeader(IdSiafiHeader)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE SiafiTrailer
	ADD CONSTRAINT R_715 FOREIGN KEY (IdSiafiHeader) REFERENCES SiafiHeader(IdSiafiHeader)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


