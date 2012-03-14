using System;

namespace TestFlatFileImport.Dominio.Siafi
{
    public class SiafiDetails
    {
        public Siafi Siafi { set; get; }
        public int IdSiafiDetails { set; get; }
        //public int IdSiafiHeader { set; get; }
	    public DateTime DtEmissao { set; get; }
	    public DateTime DtVenciemnto { set; get; }
	    public string NumDocumento { set; get; }
	    public int CodUnidGestEmit { set; get; }
	    public int CodGestEmit { set; get; }
	    public int CotUnidGestTom { set; get; }
	    public string CnpjUnidGestTom { set; get; }
	    public string CodMunUnidGestTom { set; get; }
	    public string NumDocReceitaSubstituto { set; get; }
	    public string CodMunNfse { set; get; }
	    public string CodigoReceita { set; get; }
	    public string EsferaReceita { set; get; }
	    public int Mes { set; get; }
	    public int Ano { set; get; }
	    public decimal ValorPrincipal { set; get; }
	    public decimal ValorMulta { set; get; }
	    public decimal ValorJuros { set; get; }
	    public decimal NumeroNota { set; get; }
	    public string SerieNota { set; get; }
	    public string SubSerieNota { set; get; }
	    public DateTime DtEmissaoNota { set; get; }
	    public decimal ValorNota { set; get; }
	    public decimal Aliquota { set; get; }
	    public decimal ValorBaseCalc { set; get; }
	    public string Observacao { set; get; }
        public string CodMunFavorecido { set; get; }
    }
}
