//using System;
//using System.Collections.Generic;

//namespace FlatFileImport.Process
//{
//    public class ParserRawLinePositional : IParserRawDataLine
//    {
//        private string _rawDataLine;

//        public ParserRawLinePositional(IBlueprintLine blueprintLine)
//        {
//            if (blueprintLine == null)
//                throw new ArgumentNullException("blueprintLine");

//            BlueprintLine = blueprintLine;
//        }

//        #region IParserRawDataLine Members

//        public IBlueprintLine BlueprintLine { get; private set; }
//        public string[] RawDataCollection { get { return GetRawDataCollection(); } }
//        public List<ParsedData> ParsedDatas { get { return GetParsedData(RawDataCollection); } }

//        public void ParseRawLineData(string rawDataLine)
//        {
//            if(String.IsNullOrEmpty(rawDataLine))
//                throw new ArgumentNullException("rawDataLine");

//            _rawDataLine = rawDataLine;
//        }

//        #endregion

//        private string[] GetRawDataCollection()
//        {
//            var aux = new string[BlueprintLine.BlueprintFields.Count];

//            for (var i = 0; i <BlueprintLine.BlueprintFields.Count; i++)
//            {
//                var bField = BlueprintLine.BlueprintFields[i];
//                aux[i] = _rawDataLine.Substring(bField.Position - 1, bField.Size);
//            }

//            return aux;
//        }

//        private List<ParsedData> GetParsedData(string[] data)
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
