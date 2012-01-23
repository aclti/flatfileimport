using System;

namespace TestFlatFileImport.Dominio.Siafi
{
    public class Details
    {
        public int NumSeq { set; get; }
        public DateTime DtVencimento { set; get; }
        public DateTime DtEmissao { set; get; }
        public string NumDocumento { set; get; }
    }
}
