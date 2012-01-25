using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FlatFileImport.Process;
using Moq;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestParserRawLinePositionalCharacter
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
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));
            var rawData = "D1000|010428182009003|2|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0";
            var bLine = _blueprint.BlueprintLines[0];

            var p = new ParserRawLinePositionalCharacter(bLine);
            p.ParseRawLineData(rawData);
            var data = p.RawDataCollection;

            Assert.AreEqual(bLine.BlueprintFields.Count, data.Length);
            Assert.AreEqual("D1000", data[0]);
            Assert.AreEqual("010428182009003", data[1]);
            Assert.AreEqual("2", data[2]);
            Assert.AreEqual("2009", data[3]);
            Assert.AreEqual("RENOTINTAS COMERCIO E REPRESENTACOES LTDA", data[4]);
            Assert.AreEqual("19960208", data[5]);
            Assert.AreEqual("19960208", data[6]);
            Assert.AreEqual("02071018801526456", data[7]);
            Assert.AreEqual("01406041942879518599", data[8]);
            Assert.AreEqual("20100707161153", data[9]);
            Assert.AreEqual("1.0.7.0", data[10]);
            Assert.AreEqual("0", data[11]);

            bLine = _blueprint.BlueprintLines[7];
            rawData = "00000|07491222000101|MATOS DISTRIBUIDORA DE COSMETICOS LTDA, ME|3105|S|20050712|200907|6389,32|0,000|1,00|A|1|6389,32";
            p = new ParserRawLinePositionalCharacter(bLine);
            p.ParseRawLineData(rawData);
            data = p.RawDataCollection;

            Assert.AreEqual(bLine.BlueprintFields.Count, data.Length);
            Assert.AreEqual("00000", data[0]);
            Assert.AreEqual("07491222000101", data[1]);
            Assert.AreEqual("MATOS DISTRIBUIDORA DE COSMETICOS LTDA, ME", data[2]);
            Assert.AreEqual("3105", data[3]);
            Assert.AreEqual("S", data[4]);
            Assert.AreEqual("20050712", data[5]);
            Assert.AreEqual("200907", data[6]);
            Assert.AreEqual("6389,32", data[7]);
            Assert.AreEqual("0,000", data[8]);
            Assert.AreEqual("1,00", data[9]);
            Assert.AreEqual("A", data[10]);
            Assert.AreEqual("1", data[11]);
            Assert.AreEqual("6389,32", data[12]);

            bLine = _blueprint.BlueprintLines[47];
            rawData = "D7000|074912222009001|0520100|3105|4|2442,52|8,000|0|97,70|200,00|200,00|20100713|20100728|LUCIA ROSA SILVA SANTOS|DELEGADO DA RECEITA FEDERAL DO BRASIL|00024390|ARACAJU|0|0|0";
            p = new ParserRawLinePositionalCharacter(bLine);
            p.ParseRawLineData(rawData);
            data = p.RawDataCollection;

            Assert.AreEqual(bLine.BlueprintFields.Count, data.Length);
            Assert.AreEqual("D7000", data[0]);
            Assert.AreEqual("074912222009001", data[1]);
            Assert.AreEqual("0520100", data[2]);
            Assert.AreEqual("3105", data[3]);
            Assert.AreEqual("4", data[4]);
            Assert.AreEqual("2442,52", data[5]);
            Assert.AreEqual("8,000", data[6]);
            Assert.AreEqual("0", data[7]);
            Assert.AreEqual("97,70", data[8]);
            Assert.AreEqual("200,00", data[9]);
            Assert.AreEqual("200,00", data[10]);
            Assert.AreEqual("20100713", data[11]);
            Assert.AreEqual("20100728", data[12]);
            Assert.AreEqual("LUCIA ROSA SILVA SANTOS", data[13]);
            Assert.AreEqual("DELEGADO DA RECEITA FEDERAL DO BRASIL", data[14]);
            Assert.AreEqual("00024390", data[15]);
            Assert.AreEqual("ARACAJU", data[16]);
            Assert.AreEqual("0", data[17]);
            Assert.AreEqual("0", data[18]);
            Assert.AreEqual("0", data[19]);
        }

        [Test]
        public void TestParsedData()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));
            var rawData = "D1000|010428182009003|2|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0";
            var bLine = _blueprint.BlueprintLines[0];

            var p = new ParserRawLinePositionalCharacter(bLine);
            p.ParseRawLineData(rawData);
            var parsedData = p.ParsedDatas;

            Assert.AreEqual(12, parsedData.Count);
            Assert.AreEqual("D1000", parsedData[0].Value);
            Assert.AreEqual("010428182009003", parsedData[1].Value);
            Assert.AreEqual("2", parsedData[2].Value);
            Assert.AreEqual("2009", parsedData[3].Value);
            Assert.AreEqual("RENOTINTAS COMERCIO E REPRESENTACOES LTDA", parsedData[4].Value);
            Assert.AreEqual("1996-2-8 0:0:0.0", parsedData[5].Value);
            Assert.AreEqual("1996-2-8 0:0:0.0", parsedData[6].Value);
            Assert.AreEqual("02071018801526456", parsedData[7].Value);
            Assert.AreEqual("01406041942879518599", parsedData[8].Value);
            Assert.AreEqual("2010-7-7 16:11:53.0", parsedData[9].Value);
            Assert.AreEqual("1.0.7.0", parsedData[10].Value);
            Assert.AreEqual("0", parsedData[11].Value);


            bLine = _blueprint.BlueprintLines[7];
            rawData = "00000|07491222000101|MATOS DISTRIBUIDORA DE COSMETICOS LTDA, ME|3105|S|20050712|200907|6389,32|0,000|1,00|A|1|6389,32";
            p = new ParserRawLinePositionalCharacter(bLine);
            p.ParseRawLineData(rawData);
            parsedData = p.ParsedDatas;

            Assert.AreEqual(13, parsedData.Count);
            Assert.AreEqual("00000", parsedData[0].Value);
            Assert.AreEqual("07491222000101", parsedData[1].Value);
            Assert.AreEqual("MATOS DISTRIBUIDORA DE COSMETICOS LTDA, ME", parsedData[2].Value);
            Assert.AreEqual("3105", parsedData[3].Value);
            Assert.AreEqual("S", parsedData[4].Value);
            Assert.AreEqual("2005-7-12 0:0:0.0", parsedData[5].Value);
            Assert.AreEqual("2009-7-1 0:0:0.0", parsedData[6].Value);
            Assert.AreEqual("6389.32", parsedData[7].Value);
            Assert.AreEqual("0.000", parsedData[8].Value);
            Assert.AreEqual("1.00", parsedData[9].Value);
            Assert.AreEqual("A", parsedData[10].Value);
            Assert.AreEqual("1", parsedData[11].Value);
            Assert.AreEqual("6389.32", parsedData[12].Value);

            bLine = _blueprint.BlueprintLines[47];
            rawData = "D7000|074912222009001|0520100|3105|4|2442,52|8,000|0,00|97,70|200,00|200,00|20100713|20100728|LUCIA ROSA SILVA SANTOS|DELEGADO DA RECEITA FEDERAL DO BRASIL|00024390|ARACAJU|0|0|0";
            p = new ParserRawLinePositionalCharacter(bLine);
            p.ParseRawLineData(rawData);
            parsedData = p.ParsedDatas;

            Assert.AreEqual(20, parsedData.Count);
            Assert.AreEqual("D7000", parsedData[0].Value);
            Assert.AreEqual("074912222009001", parsedData[1].Value);
            Assert.AreEqual("0520100", parsedData[2].Value);
            Assert.AreEqual("3105", parsedData[3].Value);
            Assert.AreEqual("4", parsedData[4].Value);
            Assert.AreEqual("2442.52", parsedData[5].Value);
            Assert.AreEqual("8.000", parsedData[6].Value);
            Assert.AreEqual("0.00", parsedData[7].Value);
            Assert.AreEqual("97.70", parsedData[8].Value);
            Assert.AreEqual("200.00", parsedData[9].Value);
            Assert.AreEqual("200.00", parsedData[10].Value);
            Assert.AreEqual("2010-7-13 0:0:0.0", parsedData[11].Value);
            Assert.AreEqual("2010-7-28 0:0:0.0", parsedData[12].Value);
            Assert.AreEqual("LUCIA ROSA SILVA SANTOS", parsedData[13].Value);
            Assert.AreEqual("DELEGADO DA RECEITA FEDERAL DO BRASIL", parsedData[14].Value);
            Assert.AreEqual("00024390", parsedData[15].Value);
            Assert.AreEqual("ARACAJU", parsedData[16].Value);
            Assert.AreEqual("0", parsedData[17].Value);
            Assert.AreEqual("0", parsedData[18].Value);
            Assert.AreEqual("0", parsedData[19].Value);
        }
    }
}
