﻿using System;
using System.IO;
using FlatFileImport.Process;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestParserPositional
    {
        private string _path;
        private string _blueprintPath;
        private IBlueprint _blueprint;

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
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            var bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");

            var rawData = "20000000920111116201112052011DR80025220035000001200350003944940029379841239748122000011698437109999M2011110000000000002901500000000000000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            var p = new ParserPositional(bLine, rawData);
            Assert.IsTrue(p.IsValid);

            // conteudo diferente do definido na regex de validação
            rawData = "20000000920111116201155555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            p = new ParserPositional(bLine, rawData);
            Assert.IsFalse(p.IsValid);

            rawData = "D1000|010428182009003|w|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0";
            p = new ParserPositional(bLine, rawData);
            Assert.IsFalse(p.IsValid);

            // tamanho menor
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            rawData = "20000000920111116201112052011DR800252200350000012003944940029379841239748122000011698437109999M2011110000000000002901500000000000000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            p = new ParserPositional(bLine, rawData);
            Assert.IsFalse(p.IsValid);

            // conteudo diferente do definido na regex de validação
            rawData = "20000000920111116201155555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            p = new ParserPositional(bLine, rawData);
            Assert.IsFalse(p.IsValid);
        }

        [Test]
        public void TestParseRawDataValid()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            var bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            const string rawData = "20000000920111116201112052011DR80025220035000001200350003944940029379841239748122000011698437109999M2011110000000000002901500000000000000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";

            var p = new ParserPositional(bLine, rawData);
            Assert.IsTrue(p.IsValid);
            var data = p.GetParsedData();

            Assert.AreEqual(bLine.BlueprintFields.Count, data.Attributes.Count);
            Assert.AreEqual("2", data.Attributes[0].Value);
            Assert.AreEqual("9", data.Attributes[1].Value);
            Assert.AreEqual("16/11/2011 00:00:00", data.Attributes[2].Value);
            Assert.AreEqual("05/12/2011 00:00:00", data.Attributes[3].Value);
            Assert.AreEqual("2011DR800252", data.Attributes[4].Value);
            Assert.AreEqual("200350", data.Attributes[5].Value);
            Assert.AreEqual("1", data.Attributes[6].Value);
            Assert.AreEqual("200350", data.Attributes[7].Value);
            Assert.AreEqual("00394494002937", data.Attributes[8].Value);
            Assert.AreEqual("984123", data.Attributes[9].Value);
            Assert.AreEqual("97481220000116", data.Attributes[10].Value);
            Assert.AreEqual("984371", data.Attributes[11].Value);
            Assert.AreEqual("09999", data.Attributes[12].Value);
            Assert.AreEqual("M", data.Attributes[13].Value);
            Assert.AreEqual("01/11/2011 00:00:00", data.Attributes[14].Value);
            Assert.AreEqual("290,15", data.Attributes[15].Value);
            Assert.AreEqual("0,00", data.Attributes[16].Value);
            Assert.AreEqual("0,00", data.Attributes[17].Value);
            Assert.AreEqual("0000000967", data.Attributes[18].Value);
            Assert.AreEqual("", data.Attributes[19].Value);
            Assert.AreEqual("00", data.Attributes[20].Value);
            Assert.AreEqual("03/11/2011 00:00:00", data.Attributes[21].Value);
            Assert.AreEqual("9671,70", data.Attributes[22].Value);
            Assert.AreEqual("3,000", data.Attributes[23].Value);
            Assert.AreEqual("9671,66", data.Attributes[24].Value);
            Assert.AreEqual("RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.", data.Attributes[25].Value);
            Assert.AreEqual("985401", data.Attributes[26].Value);
            Assert.AreEqual("", data.Attributes[27].Value);
        }

        [Test]
        public void TestConverterParsedDatas()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            var rawData = "20000000920111116201112052011DR8002522003588888888888888888888888888888888888888888888888888888888888888888888888888888888888888888800000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            var bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");

            var p = new ParserPositional(bLine, rawData);
            Assert.IsFalse(p.IsValid);

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            rawData = "20000000920111116201112052011DR80025220035000001200350003944940029379841239748122000011698437109999M2011110000000000002901500000000000000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");

            p = new ParserPositional(bLine, rawData);
            Assert.IsTrue(p.IsValid);
            var data = p.GetParsedData();

            Assert.AreEqual(28, data.Attributes.Count);
            Assert.AreEqual("2", data.Attributes[0].Value);
            Assert.AreEqual(9, int.Parse(data.Attributes[1].Value));
            Assert.AreEqual(new DateTime(2011, 11, 16, 0, 0, 0), DateTime.Parse(data.Attributes[2].Value));
            Assert.AreEqual(new DateTime(2011, 12, 5, 0, 0, 0), DateTime.Parse(data.Attributes[3].Value));
            Assert.AreEqual("2011DR800252", data.Attributes[4].Value);
            Assert.AreEqual(200350, int.Parse(data.Attributes[5].Value));
            Assert.AreEqual(1, int.Parse(data.Attributes[6].Value));
            Assert.AreEqual(200350, int.Parse(data.Attributes[7].Value));
            Assert.AreEqual("00394494002937", data.Attributes[8].Value);
            Assert.AreEqual("984123", data.Attributes[9].Value);
            Assert.AreEqual("97481220000116", data.Attributes[10].Value);
            Assert.AreEqual("984371", data.Attributes[11].Value);
            Assert.AreEqual("09999", data.Attributes[12].Value);
            Assert.AreEqual("M", data.Attributes[13].Value);
            Assert.AreEqual(new DateTime(2011, 11, 1, 0, 0, 0), DateTime.Parse(data.Attributes[14].Value));
            Assert.AreEqual(290.15, decimal.Parse(data.Attributes[15].Value));
            Assert.AreEqual(0.00, decimal.Parse(data.Attributes[16].Value));
            Assert.AreEqual(0.00, decimal.Parse(data.Attributes[17].Value));
            Assert.AreEqual("0000000967", data.Attributes[18].Value);
            Assert.AreEqual("", data.Attributes[19].Value);
            Assert.AreEqual("00", data.Attributes[20].Value);
            Assert.AreEqual(new DateTime(2011, 11, 3, 0, 0, 0), DateTime.Parse(data.Attributes[21].Value));
            Assert.AreEqual(9671.70, decimal.Parse(data.Attributes[22].Value));
            Assert.AreEqual(3.000, decimal.Parse(data.Attributes[23].Value));
            Assert.AreEqual(9671.66, decimal.Parse(data.Attributes[24].Value));
            Assert.AreEqual("RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.", data.Attributes[25].Value);
            Assert.AreEqual("985401", data.Attributes[26].Value);
            Assert.AreEqual("", data.Attributes[27].Value);
        }
    }
}