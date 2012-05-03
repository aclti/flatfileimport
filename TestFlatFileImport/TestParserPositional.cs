using System;
using System.IO;
using System.Linq;
using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Input;
using FlatFileImport.Process;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestParserPositional
    {
        private string _path;
        private string _blueprintPath;
        private IBlueprint _blueprint;
        private IBlueprintSetter _blueprintSetter;

        [SetUp]
        public void Setup()
        {
            _path = AppDomain.CurrentDomain.BaseDirectory;
            _blueprintPath = Path.Combine(_path, @"Samples\Blueprints\");
        }

        [TearDown]
        public void End()
        {
            _path = String.Empty;
            _blueprintPath = String.Empty;
            _blueprint = null;
        }

        [Test]
        public void TestParseRawDataSintaxLineAndAtributtes()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();

            var bLine = _blueprint.BlueprintLines.FirstOrDefault(b => b.Name == "Details");

            var rawData = "20000000920111116201112052011DR80025220035000001200350003944940029379841239748122000011698437109999M2011110000000000002901500000000000000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            var p = new ParserPositional();
            p.SetBlueprintLine(bLine);
            p.SetDataToParse(new RawLine(1, rawData));
            Assert.IsTrue(p.IsValid);

            // conteudo diferente do definido na regex de validação
            rawData = "20000000920111116201155555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            p = new ParserPositional();
            p.SetBlueprintLine(bLine);
            p.SetDataToParse(new RawLine(1, rawData));
            Assert.IsFalse(p.IsValid);

            rawData = "D1000|010428182009003|w|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0";
            p = new ParserPositional();
            p.SetBlueprintLine(bLine);
            p.SetDataToParse(new RawLine(1, rawData));
            Assert.IsFalse(p.IsValid);

            // tamanho menor
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();

            bLine = _blueprint.BlueprintLines.FirstOrDefault(b => b.Name == "Details");
            rawData = "20000000920111116201112052011DR800252200350000012003944940029379841239748122000011698437109999M2011110000000000002901500000000000000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            p = new ParserPositional();
            p.SetBlueprintLine(bLine);
            p.SetDataToParse(new RawLine(1, rawData));
            Assert.IsFalse(p.IsValid);

            // conteudo diferente do definido na regex de validação
            rawData = "20000000920111116201155555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            p = new ParserPositional();
            p.SetBlueprintLine(bLine);
            p.SetDataToParse(new RawLine(1, rawData));
            Assert.IsFalse(p.IsValid);
        }

        [Test]
        public void TestParseRawDataValid()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();

            var bLine = _blueprint.BlueprintLines.FirstOrDefault(b => b.Name == "Details");
            const string rawData = "20000000920111116201112052011DR80025220035000001200350003944940029379841239748122000011698437109999M2011110000000000002901500000000000000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";

            var p = new ParserPositional();
            p.SetBlueprintLine(bLine);
            p.SetDataToParse(new RawLine(1, rawData));
            Assert.IsTrue(p.IsValid);
            var parent = (IParsedData)p.GetParsedData(null);
            var data = p.GetParsedLine(parent);

            Assert.IsNotNull(bLine);
            Assert.AreEqual(bLine.BlueprintFields.Count, data.Fields.Count);
            Assert.AreEqual("2", data.Fields[0].Value);
            Assert.AreEqual("9", data.Fields[1].Value);
            Assert.AreEqual("16/11/2011 00:00:00", data.Fields[2].Value);
            Assert.AreEqual("05/12/2011 00:00:00", data.Fields[3].Value);
            Assert.AreEqual("2011DR800252", data.Fields[4].Value);
            Assert.AreEqual("200350", data.Fields[5].Value);
            Assert.AreEqual("1", data.Fields[6].Value);
            Assert.AreEqual("200350", data.Fields[7].Value);
            Assert.AreEqual("00394494002937", data.Fields[8].Value);
            Assert.AreEqual("984123", data.Fields[9].Value);
            Assert.AreEqual("97481220000116", data.Fields[10].Value);
            Assert.AreEqual("984371", data.Fields[11].Value);
            Assert.AreEqual("09999", data.Fields[12].Value);
            Assert.AreEqual("M", data.Fields[13].Value);
            Assert.AreEqual("01/11/2011 00:00:00", data.Fields[14].Value);
            Assert.AreEqual("290,15", data.Fields[15].Value);
            Assert.AreEqual("0,00", data.Fields[16].Value);
            Assert.AreEqual("0,00", data.Fields[17].Value);
            Assert.AreEqual("0000000967", data.Fields[18].Value);
            Assert.AreEqual("", data.Fields[19].Value);
            Assert.AreEqual("00", data.Fields[20].Value);
            Assert.AreEqual("03/11/2011 00:00:00", data.Fields[21].Value);
            Assert.AreEqual("9671,70", data.Fields[22].Value);
            Assert.AreEqual("3,000", data.Fields[23].Value);
            Assert.AreEqual("9671,66", data.Fields[24].Value);
            Assert.AreEqual("RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.", data.Fields[25].Value);
            Assert.AreEqual("985401", data.Fields[26].Value);
            Assert.AreEqual("", data.Fields[27].Value);
        }

        [Test]
        public void TestConverterParsedDatas()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();
            
            var rawData = "20000000920111116201112052011DR8002522003588888888888888888888888888888888888888888888888888888888888888888888888888888888888888888800000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            var bLine = _blueprint.BlueprintLines.FirstOrDefault(b => b.Name == "Details");

            var p = new ParserPositional();
            p.SetBlueprintLine(bLine);
            p.SetDataToParse(new RawLine(1, rawData));
            Assert.IsFalse(p.IsValid);

            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();

            rawData = "20000000920111116201112052011DR80025220035000001200350003944940029379841239748122000011698437109999M2011110000000000002901500000000000000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            bLine = _blueprint.BlueprintLines.FirstOrDefault(b => b.Name == "Details");

            p = new ParserPositional();
            p.SetBlueprintLine(bLine);
            p.SetDataToParse(new RawLine(1, rawData));
            Assert.IsTrue(p.IsValid);
            var parent = (IParsedData)p.GetParsedData(null);
            var data = p.GetParsedLine(parent);

            Assert.AreEqual(28, data.Fields.Count);
            Assert.AreEqual("2", data.Fields[0].Value);
            Assert.AreEqual(9, int.Parse(data.Fields[1].Value));
            Assert.AreEqual(new DateTime(2011, 11, 16, 0, 0, 0), DateTime.Parse(data.Fields[2].Value));
            Assert.AreEqual(new DateTime(2011, 12, 5, 0, 0, 0), DateTime.Parse(data.Fields[3].Value));
            Assert.AreEqual("2011DR800252", data.Fields[4].Value);
            Assert.AreEqual(200350, int.Parse(data.Fields[5].Value));
            Assert.AreEqual(1, int.Parse(data.Fields[6].Value));
            Assert.AreEqual(200350, int.Parse(data.Fields[7].Value));
            Assert.AreEqual("00394494002937", data.Fields[8].Value);
            Assert.AreEqual("984123", data.Fields[9].Value);
            Assert.AreEqual("97481220000116", data.Fields[10].Value);
            Assert.AreEqual("984371", data.Fields[11].Value);
            Assert.AreEqual("09999", data.Fields[12].Value);
            Assert.AreEqual("M", data.Fields[13].Value);
            Assert.AreEqual(new DateTime(2011, 11, 1, 0, 0, 0), DateTime.Parse(data.Fields[14].Value));
            Assert.AreEqual(290.15, decimal.Parse(data.Fields[15].Value));
            Assert.AreEqual(0.00, decimal.Parse(data.Fields[16].Value));
            Assert.AreEqual(0.00, decimal.Parse(data.Fields[17].Value));
            Assert.AreEqual("0000000967", data.Fields[18].Value);
            Assert.AreEqual("", data.Fields[19].Value);
            Assert.AreEqual("00", data.Fields[20].Value);
            Assert.AreEqual(new DateTime(2011, 11, 3, 0, 0, 0), DateTime.Parse(data.Fields[21].Value));
            Assert.AreEqual(9671.70, decimal.Parse(data.Fields[22].Value));
            Assert.AreEqual(3.000, decimal.Parse(data.Fields[23].Value));
            Assert.AreEqual(9671.66, decimal.Parse(data.Fields[24].Value));
            Assert.AreEqual("RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.", data.Fields[25].Value);
            Assert.AreEqual("985401", data.Fields[26].Value);
            Assert.AreEqual("", data.Fields[27].Value);
        }
    }
}