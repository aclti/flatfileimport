using System.Collections;

namespace TestFlatFileImport.Dominio.Siafi
{
    /// <summary>
    /// Status possiveis:
    /// P - Processado com sucesso
    /// Q - Aguradando Processamento
    /// E - Processado com erro
    /// X - Remessa já foi processada
    /// </summary>
    public class Siafi
    {
        public int oid { set; get; }
        public string Arquivo { set; get; }
        public string Status { set; get; }
        public SiafiHeader Header { set; get; }
        public ArrayList Details { set; get; }
        public SiafiTrailer Trailer { set; get; }
    }
}
