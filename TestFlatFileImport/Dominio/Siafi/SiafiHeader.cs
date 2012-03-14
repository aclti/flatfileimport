using System;

namespace TestFlatFileImport.Dominio.Siafi
{
    public class SiafiHeader
    {
        public Siafi Siafi { set; get; }
        public int IdSiafiHeader { set; get; }
	    public string CodConvenio { set; get; }
	    public DateTime DtGeracao { set; get; }
	    public int NumRemessa { set; get; }
	    public string NumVersao { set; get; }
	    public int Mes { set; get; }
	    public int Ano { set; get; }
        public int Decendio { set; get; }
    }
}
