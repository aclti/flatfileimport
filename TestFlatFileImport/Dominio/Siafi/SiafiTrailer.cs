namespace TestFlatFileImport.Dominio.Siafi
{
    public class SiafiTrailer
    {
        public Siafi Siafi { set; get; }
        public int IdSiafiTrailer { set; get; }
        //public int IdSiafiHeader { set; get; }
        public int NumSeqRegistro { set; get; }
        public int TotalRegistros { set; get; }
        public decimal ValorTotalRecebido { set; get; }
    }
}
