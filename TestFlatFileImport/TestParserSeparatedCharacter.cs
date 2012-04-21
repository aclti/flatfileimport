using System;
using System.Globalization;
using System.IO;
using FlatFileImport.Process;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestParserSeparatedCharacter
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
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));
            var rawData = "D1000|010428182009003|2|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0";
            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));

            var p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsTrue(p.IsValid);

            rawData = "D1000|010428182009003|w|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsFalse(p.IsValid);   

            rawData = "D1000|01042818200900399999|2|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsFalse(p.IsValid);          

            rawData = "D1000||||||19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsTrue(p.IsValid);

            rawData = "D1000||||||19960208|02071018801526456|01406041942879518599|";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsFalse(p.IsValid);

            rawData = "D1000|||||||||||";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsTrue(p.IsValid);

            rawData = "D1000|";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsFalse(p.IsValid);

            rawData = "D1000|||";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsFalse(p.IsValid);
            
            rawData = "D1000";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsFalse(p.IsValid);

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            rawData = "D3001|200810|88168,20";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsTrue(p.IsValid);

            rawData = "D3001|200810|88168.20";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsFalse(p.IsValid);

            rawData = "D3001|200810|efrwqr";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsFalse(p.IsValid);

            rawData = "D3001|200810|88168";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsFalse(p.IsValid);

            rawData = "D3001|200810|0";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsFalse(p.IsValid);
        }

        [Test]
        public void TestParseRawData()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));
            var rawData = "D1000|010428182009003|2|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0";
            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            var p = new ParserSeparatedCharacter(bLine, rawData);
            var data = p.GetParsedData();

            Assert.AreEqual(bLine.BlueprintFields.Count, data.Fields.Count);
            Assert.AreEqual("D1000", data.Fields[0].Value);
            Assert.AreEqual("010428182009003", data.Fields[1].Value);
            Assert.AreEqual("2", data.Fields[2].Value);
            Assert.AreEqual("2009", data.Fields[3].Value);
            Assert.AreEqual("RENOTINTAS COMERCIO E REPRESENTACOES LTDA", data.Fields[4].Value);
            Assert.AreEqual("08/02/1996 00:00:00", data.Fields[5].Value);
            Assert.AreEqual("08/02/1996 00:00:00", data.Fields[6].Value);
            Assert.AreEqual("02071018801526456", data.Fields[7].Value);
            Assert.AreEqual("01406041942879518599", data.Fields[8].Value);
            Assert.AreEqual("07/07/2010 16:11:53", data.Fields[9].Value);
            Assert.AreEqual("1.0.7.0", data.Fields[10].Value);
            Assert.AreEqual("0", data.Fields[11].Value);

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("00000"));
            rawData = "00000|07491222000101|MATOS DISTRIBUIDORA DE COSMETICOS LTDA, ME|3105|S|20050712|200907|6389,32|0,000|1,00|A|1|6389,32";
            p = new ParserSeparatedCharacter(bLine, rawData);
            data = p.GetParsedData();

            Assert.AreEqual(bLine.BlueprintFields.Count, data.Fields.Count);
            Assert.AreEqual("00000", data.Fields[0].Value);
            Assert.AreEqual("07491222000101", data.Fields[1].Value);
            Assert.AreEqual("MATOS DISTRIBUIDORA DE COSMETICOS LTDA, ME", data.Fields[2].Value);
            Assert.AreEqual("3105", data.Fields[3].Value);
            Assert.AreEqual("S", data.Fields[4].Value);
            Assert.AreEqual("12/07/2005 00:00:00", data.Fields[5].Value);
            Assert.AreEqual("01/07/2009 00:00:00", data.Fields[6].Value);
            Assert.AreEqual("6389" + decimalSeparator + "32", data.Fields[7].Value);
            Assert.AreEqual("0" + decimalSeparator + "000", data.Fields[8].Value);
            Assert.AreEqual("1" + decimalSeparator + "00", data.Fields[9].Value);
            Assert.AreEqual("A", data.Fields[10].Value);
            Assert.AreEqual("1", data.Fields[11].Value);
            Assert.AreEqual("6389" + decimalSeparator + "32", data.Fields[12].Value);

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D7000"));
            rawData = "D7000|074912222009001|0520100|3105|4|2442,52|8,000|0|97,70|200,00|200,00|20100713|20100728|LUCIA ROSA SILVA SANTOS|DELEGADO DA RECEITA FEDERAL DO BRASIL|0002439|ARACAJU|0|0|0";
            p = new ParserSeparatedCharacter(bLine, rawData);
            data = p.GetParsedData();

            Assert.AreEqual(bLine.BlueprintFields.Count, data.Fields.Count);
            Assert.AreEqual("D7000", data.Fields[0].Value);
            Assert.AreEqual("074912222009001", data.Fields[1].Value);
            Assert.AreEqual("0520100", data.Fields[2].Value);
            Assert.AreEqual("3105", data.Fields[3].Value);
            Assert.AreEqual("4", data.Fields[4].Value);
            Assert.AreEqual("2442" + decimalSeparator + "52", data.Fields[5].Value);
            Assert.AreEqual("8" + decimalSeparator + "000", data.Fields[6].Value);
            Assert.AreEqual("0", data.Fields[7].Value);
            Assert.AreEqual("97" + decimalSeparator + "70", data.Fields[8].Value);
            Assert.AreEqual("200" + decimalSeparator + "00", data.Fields[9].Value);
            Assert.AreEqual("200" + decimalSeparator + "00", data.Fields[10].Value);
            Assert.AreEqual("13/07/2010 00:00:00", data.Fields[11].Value);
            Assert.AreEqual("28/07/2010 00:00:00", data.Fields[12].Value);
            Assert.AreEqual("LUCIA ROSA SILVA SANTOS", data.Fields[13].Value);
            Assert.AreEqual("DELEGADO DA RECEITA FEDERAL DO BRASIL", data.Fields[14].Value);
            Assert.AreEqual("0002439", data.Fields[15].Value);
            Assert.AreEqual("ARACAJU", data.Fields[16].Value);
            Assert.AreEqual("0", data.Fields[17].Value);
            Assert.AreEqual("0", data.Fields[18].Value);
            Assert.AreEqual("0", data.Fields[19].Value);
        }

        [Test]
        public void TestParseRawDataInclomplet()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));
            var rawData = "D1000|010428182009003||||19960208||02071018801526456|01406041942879518599||1.0.7.0|0";
            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            var p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsTrue(p.IsValid);
            var data = p.GetParsedData();

            Assert.AreEqual(12, data.Fields.Count);
            Assert.AreEqual("D1000", data.Fields[0].Value);
            Assert.AreEqual("010428182009003", data.Fields[1].Value);
            Assert.AreEqual("0", data.Fields[2].Value);
            Assert.AreEqual("0", data.Fields[3].Value);
            Assert.AreEqual(String.Empty, data.Fields[4].Value);
            Assert.AreEqual("08/02/1996 00:00:00", data.Fields[5].Value);
            Assert.AreEqual(String.Empty, data.Fields[6].Value);
            Assert.AreEqual("02071018801526456", data.Fields[7].Value);
            Assert.AreEqual("01406041942879518599", data.Fields[8].Value);
            Assert.AreEqual(String.Empty, data.Fields[9].Value);
            Assert.AreEqual("1.0.7.0", data.Fields[10].Value);
            Assert.AreEqual("0", data.Fields[11].Value);

            bLine = _blueprint.BlueprintLines[7];
            rawData = "00000|07491222000101||3105|S|20050712||0|0||A|1|6389,32";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsTrue(p.IsValid);
            data = p.GetParsedData();

            Assert.AreEqual(13, data.Fields.Count);
            Assert.AreEqual("00000", data.Fields[0].Value);
            Assert.AreEqual("07491222000101", data.Fields[1].Value);
            Assert.AreEqual(String.Empty, data.Fields[2].Value);
            Assert.AreEqual("3105", data.Fields[3].Value);
            Assert.AreEqual("S", data.Fields[4].Value);
            Assert.AreEqual("12/07/2005 00:00:00", data.Fields[5].Value);
            Assert.AreEqual(String.Empty, data.Fields[6].Value);
            Assert.AreEqual("0", data.Fields[7].Value);
            Assert.AreEqual("0", data.Fields[8].Value);
            Assert.AreEqual("0", data.Fields[9].Value);
            Assert.AreEqual("A", data.Fields[10].Value);
            Assert.AreEqual("1", data.Fields[11].Value);
            Assert.AreEqual("6389" + decimalSeparator + "32", data.Fields[12].Value);

            bLine = _blueprint.BlueprintLines[47];
            rawData = "D7000|||3105|4|2442,52|8,000|||200,00|0||20100728|LUCIA ROSA SILVA SANTOS|DELEGADO DA RECEITA FEDERAL DO BRASIL|0002439|ARACAJU|0|0|0";
            p = new ParserSeparatedCharacter(bLine, rawData);
            Assert.IsTrue(p.IsValid);
            data = p.GetParsedData();

            Assert.AreEqual(20, data.Fields.Count);
            Assert.AreEqual("D7000", data.Fields[0].Value);
            Assert.AreEqual(String.Empty, data.Fields[1].Value);
            Assert.AreEqual(String.Empty, data.Fields[2].Value);
            Assert.AreEqual("3105", data.Fields[3].Value);
            Assert.AreEqual("4", data.Fields[4].Value);
            Assert.AreEqual("2442" + decimalSeparator + "52", data.Fields[5].Value);
            Assert.AreEqual("8" + decimalSeparator + "000", data.Fields[6].Value);
            Assert.AreEqual("0", data.Fields[7].Value);
            Assert.AreEqual("0", data.Fields[8].Value);
            Assert.AreEqual("200" + decimalSeparator + "00", data.Fields[9].Value);
            Assert.AreEqual("0", data.Fields[10].Value);
            Assert.AreEqual(String.Empty, data.Fields[11].Value);
            Assert.AreEqual("28/07/2010 00:00:00", data.Fields[12].Value);
            Assert.AreEqual("LUCIA ROSA SILVA SANTOS", data.Fields[13].Value);
            Assert.AreEqual("DELEGADO DA RECEITA FEDERAL DO BRASIL", data.Fields[14].Value);
            Assert.AreEqual("0002439", data.Fields[15].Value);
            Assert.AreEqual("ARACAJU", data.Fields[16].Value);
            Assert.AreEqual("0", data.Fields[17].Value);
            Assert.AreEqual("0", data.Fields[18].Value);
            Assert.AreEqual("0", data.Fields[19].Value);
        }
    }
}
