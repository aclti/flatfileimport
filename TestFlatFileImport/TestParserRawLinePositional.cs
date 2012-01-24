using System;
using System.IO;
using FlatFileImport.Process;
using Moq;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestParserRawLinePositional
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
        public void TestParseRawData()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            var rawData = "20000000920111116201112052011DR80025220035000001200350003944940029379841239748122000011698437109999M2011110000000000002901500000000000000000000000000000000000000000967     0020111103000000000009671700300000000000000967166RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 985401                                       ";
            var bLine = _blueprint.BlueprintLines[0];

            var p = new ParserRawLinePositional();
            p.ParseRawLineData(rawData, bLine);
            var data = p.RawDataCollection;

            Assert.AreEqual(bLine.BlueprintFields.Count, data.Length);
            Assert.AreEqual("2", data[0]);
            Assert.AreEqual("00000009", data[1]);
            Assert.AreEqual("20111116", data[2]);
            Assert.AreEqual("20111205", data[3]);
            Assert.AreEqual("2011DR800252", data[4]);
            Assert.AreEqual("200350", data[5]);
            Assert.AreEqual("00001", data[6]);
            Assert.AreEqual("200350", data[7]);
            Assert.AreEqual("00394494002937", data[8]);
            Assert.AreEqual("984123", data[9]);
            Assert.AreEqual("97481220000116", data[10]);
            Assert.AreEqual("984371", data[11]);
            Assert.AreEqual("09999", data[12]);
            Assert.AreEqual("M", data[13]);
            Assert.AreEqual("201111", data[14]);
            Assert.AreEqual("00000000000029015", data[15]);
            Assert.AreEqual("00000000000000000", data[16]);
            Assert.AreEqual("00000000000000000", data[17]);
            Assert.AreEqual("0000000967", data[18]);
            Assert.AreEqual("     ", data[19]);
            Assert.AreEqual("00", data[20]);
            Assert.AreEqual("20111103", data[21]);
            Assert.AreEqual("00000000000967170", data[22]);
            Assert.AreEqual("03000", data[23]);
            Assert.AreEqual("00000000000967166", data[24]);
            Assert.AreEqual("RETENÇÃO DE TRIBUTOS FEDERAIS SOBRE NF 967 EMITIDA PELA SETSYS                SERVIÇOS GERAIS LTDA - CRONOGRAMA 008/2010.                                                                                                                 ", data[25]);
            Assert.AreEqual("985401", data[26]);
            Assert.AreEqual("                                       ", data[27]);
        }
    }
}
