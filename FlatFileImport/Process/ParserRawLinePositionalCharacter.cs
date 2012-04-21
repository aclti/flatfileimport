//using System;
//using System.Collections.Generic;

//namespace FlatFileImport.Process
//{
//    public class ParserRawLinePositionalCharacter : IParserRawDataLine
//    {
//        private string _rawDataLine;
//        private readonly char _separator;

//        public ParserRawLinePositionalCharacter(IBlueprintLine blueprintLine)
//        {
//            if (blueprintLine == null)
//                throw new ArgumentNullException("blueprintLine");

//            BlueprintLine = blueprintLine;
//            _separator = blueprintLine.Blueprint.BluePrintCharSepartor;
//        }

//        #region IParserRawDataLine Members

//        public IBlueprintLine BlueprintLine { get; private set; }
//        public string[] RawDataCollection { get { return GetRawData(); } }
//        public List<ParsedData> ParsedDatas { get { return GetParseData(RawDataCollection); } }

//        public void ParseRawLineData(string rawDataLine)
//        {
//            if(String.IsNullOrEmpty(rawDataLine))
//                throw new ArgumentNullException("rawDataLine");

//            _rawDataLine = rawDataLine;
//        }      

//        #endregion

//        private string[] GetRawData()
//        {
//            // TODO: Isso nunca deve acontecer, verificar a melhor maneira de tratar com exceção
//            if (BlueprintLine == null)
//                return null;

//            string[] data = _rawDataLine.Split(_separator);

//            return data;
//        }
//        // TODO: Verificar o tratamento de erro quando a valor vem diferente do definido na blueprint
//        private List<ParsedData> GetParseData(string[] data)
//        {
//            var l = new List<ParsedData>();

//            for (int i = 0; i < BlueprintLine.BlueprintFields.Count; i++)
//            {

//                var parsed = ParsedDataFactory.GetInstance(BlueprintLine.BlueprintFields[i]).GetParsedData(data[i]);
//                l.Add(parsed);
//            }

//            return l;

//        }
//    }
//}
