using System;
using System.Globalization;
using System.IO;
using FlatFileImport.Process;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestConverter
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
        public void TestConverterString()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));

            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            var converter = Converter.GetConvert(typeof(string));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "RAZAO_SOCIAL"), "RENOTINTAS D'COMERCIO LTDA");
            Assert.AreEqual("RENOTINTAS D´COMERCIO LTDA", converter.Data);

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            converter = Converter.GetConvert(typeof(string));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "RAZAO_SOCIAL"), "");
            Assert.AreEqual(String.Empty, converter.Data);
        }

        [Test]
        public void TestConverterInt()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            var bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            var converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "000135");
            Assert.AreEqual("135", converter.Data);

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "523135");
            Assert.AreEqual("523135", converter.Data);

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "000000");
            Assert.AreEqual("0", converter.Data);

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "0");
            Assert.AreEqual("0", converter.Data);

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "");
            Assert.AreEqual("0", converter.Data);
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void TestConverterIntlInvalidSize()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            var bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            var converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "00088888888888888888888888888888888888888888888888888888135");
            var d = converter.Data;
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestConverterIntlInvalid()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            var bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            var converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "62asd");
            var d = converter.Data;
        }

        [Test]
        public void TestConverterIntStringToInt()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            var bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            var converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "000135");
            Assert.AreEqual(135, int.Parse(converter.Data));

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "523135");
            Assert.AreEqual(523135, int.Parse(converter.Data));

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "000000");
            Assert.AreEqual(0, int.Parse(converter.Data));

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "0");
            Assert.AreEqual(0, int.Parse(converter.Data));

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));
            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "CodUnidGestEmit"), "");
            Assert.AreEqual(0, int.Parse(converter.Data));
        }

        [Test]
        public void TestConverterDecimal()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));
            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            var converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "VALOR"), "51297,22");
            Assert.AreEqual("51297" + decimalSeparator  + "22", converter.Data);

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "VALOR"), "51297,22");
            Assert.AreEqual("51297" + decimalSeparator + "22", converter.Data);

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "VALOR"), "51297.22");
            Assert.AreEqual("51297" + decimalSeparator + "22", converter.Data);

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "VALOR"), "0");
            Assert.AreEqual("0", converter.Data);

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "VALOR"), "");
            Assert.AreEqual("0", converter.Data);

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));

            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "ValorBaseCalc"), "00000000000012355");
            Assert.AreEqual("123" + decimalSeparator + "55", converter.Data);            
        }

        [Test]
        public void TestConverterDecimalStringToDecimal()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));

            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            var converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "VALOR"), "51297,22");
            Assert.AreEqual(51297.22, decimal.Parse(converter.Data));

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "VALOR"), "51297.22");
            Assert.AreEqual(51297.22, decimal.Parse(converter.Data));

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "VALOR"), "0");
            Assert.AreEqual(0, decimal.Parse(converter.Data));

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "VALOR"), "");
            Assert.AreEqual(0, decimal.Parse(converter.Data));

            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));

            bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "ValorBaseCalc"), "00000000000012355");
            Assert.AreEqual(123.55, decimal.Parse(converter.Data));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConverterDecimalInvalidSize()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));

            var bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            var converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "ValorBaseCalc"), "0000000000012355");
            var d = converter.Data;
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestConverterDecimalInvalid()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));

            var bLine = _blueprint.BlueprintLines.Find(b => b.Class == "Details");
            var converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "ValorBaseCalc"), "00000000000123qq5");
            var d = converter.Data;
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestConverterDecimalInvalidSeparator()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));

            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            var converter = Converter.GetConvert(typeof(decimal));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "VALOR"), "51297/22");
            var d = converter.Data;
        }

        [Test]
        public void TestConverterDate()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));

            // TESTE PARA DATETIME
            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            var converter = Converter.GetConvert(typeof (DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "DT_TRANSMISSAO"), "20100707161153");
            Assert.AreEqual("07/07/2010 16:11:53", converter.Data);

            // TESTE PARA DATA DD/MM/AA
            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D2000"));
            converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "DT_EVENTO"), "20100531");
            Assert.AreEqual("31/05/2010 00:00:00", converter.Data);

            // TESTE PARA PERIODO/COMPETENCIA
            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "PERIODO_APURACAO"), "200906");
            Assert.AreEqual("01/06/2009 00:00:00", converter.Data);

            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "DT_TRANSMISSAO"), "");
            Assert.AreEqual(String.Empty, converter.Data);
        }

        [Test]
        public void TestConverterDateStringToDate()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));

            // TESTE PARA DATETIME
            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            var converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "DT_TRANSMISSAO"), "20100707161153");
            Assert.AreEqual(new DateTime(2010,7,7,16,11,53), DateTime.Parse(converter.Data));

            // TESTE PARA DATA DD/MM/AA
            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D2000"));
            converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "DT_EVENTO"), "20100531");
            Assert.AreEqual(new DateTime(2010, 05, 31,0,0,0), DateTime.Parse(converter.Data));

            // TESTE PARA PERIODO/COMPETENCIA
            bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D3001"));
            converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "PERIODO_APURACAO"), "200906");
            Assert.AreEqual(new DateTime(2009, 6,1, 00,00,00), DateTime.Parse(converter.Data));
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestConverterInvalidDateInPut()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));

            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            var converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "DT_TRANSMISSAO"), "4568desa44wwaa");
            var d = converter.Data;
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestConverterInvalidDateNotPassInRegexValidadtion()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));

            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            var converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "DT_ABERTURA"), "20115030");
            var d = converter.Data;
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConverterInvalidDate()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));

            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            var converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "DT_ABERTURA"), "20110230");
            var d = converter.Data;
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestConverterInvalidDateTime()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));

            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            var converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "DT_TRANSMISSAO"), "00000000000000");
            var d = converter.Data;

        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestConverterInvalidPeriodCompetencia()
        {
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));

            var bLine = _blueprint.BlueprintLines.Find(b => b.Regex.IsMatch("D1000"));
            var converter = Converter.GetConvert(typeof(DateTime));
            converter.Init(bLine.BlueprintFields.Find(f => f.Attribute == "DT_TRANSMISSAO"), "000000");
            var d = converter.Data;

        }
    }
}
