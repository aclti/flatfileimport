using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestFlatFileImport.Dominio.Siafi
{
    /// <summary>
    /// Status possiveis:
    /// P - Processado com sucesso
    /// Q - Aguradando Processamento
    /// E - Processado com erro
    /// X - Remessa já foi processada
    /// </summary>
    //public class Siafi
    //{
    //    public int oid { set; get; }
    //    public string Arquivo { set; get; }
    //    public string Status { set; get; }
    //    public SiafiHeader Header { set; get; }
    //    public ArrayList Details { set; get; }
    //    public SiafiTrailer Trailer { set; get; }
    //}


    public class SiafiStatus
    {
        private static List<SiafiStatus> _statusPossiveis;
        // ReSharper disable InconsistentNaming
        private static readonly SiafiStatus _novo = new SiafiStatus { Descricao = "Novo", Id = "N" };
        private static readonly SiafiStatus _processadoSucesso = new SiafiStatus { Descricao = "Processado com Sucesso", Id = "P" };
        private static readonly SiafiStatus _aguardandoProcessamento = new SiafiStatus { Descricao = "Aguardando Processamento", Id = "Q" };
        private static readonly SiafiStatus _processadoErro = new SiafiStatus { Descricao = "Processado com Erro", Id = "E" };
        private static readonly SiafiStatus _erroJaProcessado = new SiafiStatus { Descricao = "Remessa já foi processada", Id = "X" };
        // ReSharper restore InconsistentNaming

        public string Id { set; get; }
        public string Descricao { set; get; }

        public static SiafiStatus Novo { get { return _novo; } }
        public static SiafiStatus ProcessadoSucesso { get { return _processadoSucesso; } }
        public static SiafiStatus ProcessadoErro { get { return _processadoErro; } }
        public static SiafiStatus AguardandoProcessamento { get { return _aguardandoProcessamento; } }
        public static SiafiStatus ErroRemessaJaProcessada { get { return _erroJaProcessado; } }

        public static ReadOnlyCollection<SiafiStatus> StatusPossiveis
        {
            get
            {
                if (_statusPossiveis == null)
                    _statusPossiveis = new List<SiafiStatus>
                                       {
                                           _aguardandoProcessamento, _erroJaProcessado, _novo, _processadoErro, _processadoSucesso
                                       };

                return _statusPossiveis.AsReadOnly();
            }
        }
    }

    /// <summary>
    /// Status possiveis:
    /// N - Novo
    /// P - Processado com sucesso
    /// Q - Aguradando Processamento
    /// E - Processado com erro
    /// X - Remessa já foi processada
    /// </summary>
    public class Siafi
    {
        public int Oid { set; get; }
        public string codConvenio { set; get; }
        public int numRemessa { set; get; }
        public int mes { set; get; }
        public int ano { set; get; }
        public int decendio { set; get; }
        public int totalRegistros { set; get; }
        public decimal valorTotalRecebido { set; get; }
        public DateTime dataGeracao { set; get; }
        public DateTime dataRecebimento { set; get; }
        public DateTime dataProcessamento { set; get; }
        public string arquivo { set; get; }
        public string status { set; get; }
        public string numVersao { set; get; }
        public ArrayList registros { set; get; }
    }

    public class SiafiEmitente
    {
        public SiafiRegistro registro { set; get; }
        public int codUnidadeGestora { set; get; }
        public int codGestao { set; get; }
    }

    public class SiafiTomador
    {
        public SiafiRegistro registro { set; get; }
        public int codUnidGestora { set; get; }
        public string cnpj { set; get; }
        public int codMunicipio { set; get; }
    }

    public class SiafiPrestador
    {
        public SiafiRegistro registro { set; get; }
        public string numDocReceita { set; get; }
    }

    public class SiafiRegistro
    {
        public Siafi siafi { set; get; }
        public SiafiEmitente emitente { set; get; }
        public SiafiTomador tomador { set; get; }
        public SiafiPrestador prestador { set; get; }

        public DateTime dtEmissao { set; get; }
        public DateTime dtVencimento { set; get; }
        public string numDocumento { set; get; }
        public string codMunNfse { set; get; }
        public string codigoReceita { set; get; }
        public string esferaReceita { set; get; }
        public int mes { set; get; }
        public int ano { set; get; }
        public decimal valorPrincipal { set; get; }
        public decimal valorMulta { set; get; }
        public decimal valorJuros { set; get; }
        public string numeroNota { set; get; }
        public string serieNota { set; get; }
        public string subSerieNota { set; get; }
        public DateTime dtEmissaoNota { set; get; }
        public decimal valorNota { set; get; }
        public decimal aliquota { set; get; }
        public decimal valorBaseCalc { set; get; }
        public String observacao { set; get; }
        public string codMunFavorecido { set; get; }
    }
}
