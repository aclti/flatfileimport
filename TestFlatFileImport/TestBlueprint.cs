using System;
using System.IO;
using FlatFileImport.Process;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public  class TestBlueprint
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
        public void TestBlueprintSiafi()
        {
            //TODO: Testar todas as linhas e campos.
            _blueprint = new Blueprint(Path.Combine(_blueprintPath, "siafi.xml"));

            Assert.AreEqual(_blueprint.BluePrintCharSepartor, '\0');
            Assert.AreEqual(_blueprint.BlueprintLines.Count, 1);
            Assert.AreEqual(_blueprint.BlueprintRegistires.Count, 0);
            Assert.AreEqual(_blueprint.FieldSeparationType, EnumFieldSeparationType.Position);
            Assert.IsNotNull(_blueprint.Footer);
            Assert.IsNotNull(_blueprint.Header);
            Assert.AreEqual(_blueprint.UseRegistries, false);
            
            // Teste HEADER
            var bLine = _blueprint.Header;
            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bLine.BlueprintFields.Count, 9);
            Assert.AreEqual(bLine.Class, "Header");
            Assert.AreEqual(bLine.Mandatory, true);
            Assert.AreEqual(bLine.Regex.ToString(), "^1.{72}0[1-3].{425}$");

            // Teste HEADER FIELDS
            var bFiled = bLine.BlueprintFields[0];
            Assert.AreEqual("CodRegistro", bFiled.Attribute);
            Assert.AreEqual(bLine, bFiled.BlueprintLine);
            Assert.AreEqual(false, bFiled.Persist);
            Assert.AreEqual(1, bFiled.Position);
            Assert.AreEqual(-1, bFiled.Precision);
            Assert.AreEqual(null, bFiled.Regex);
            Assert.AreEqual(1, bFiled.Size);
            Assert.AreEqual(typeof(string), bFiled.Type);

            bFiled = bLine.BlueprintFields[1];
            Assert.AreEqual("NumSeqRegistro", bFiled.Attribute);
            Assert.AreEqual(bLine, bFiled.BlueprintLine);
            Assert.AreEqual(false, bFiled.Persist);
            Assert.AreEqual(2, bFiled.Position);
            Assert.AreEqual(-1, bFiled.Precision);
            Assert.AreEqual(null, bFiled.Regex);
            Assert.AreEqual(8, bFiled.Size);
            Assert.AreEqual(typeof(int), bFiled.Type);

            bFiled = bLine.BlueprintFields[2];
            Assert.AreEqual("CodConvenio", bFiled.Attribute);
            Assert.AreEqual(bLine, bFiled.BlueprintLine);
            Assert.AreEqual(true, bFiled.Persist);
            Assert.AreEqual(10, bFiled.Position);
            Assert.AreEqual(-1, bFiled.Precision);
            Assert.AreEqual(null, bFiled.Regex);
            Assert.AreEqual(20, bFiled.Size);
            Assert.AreEqual(typeof(string), bFiled.Type);

            bFiled = bLine.BlueprintFields[3];
            Assert.AreEqual("DtGeracao", bFiled.Attribute);
            Assert.AreEqual(bLine, bFiled.BlueprintLine);
            Assert.AreEqual(true, bFiled.Persist);
            Assert.AreEqual(30, bFiled.Position);
            Assert.AreEqual(-1, bFiled.Precision);
            Assert.AreEqual("date", bFiled.Regex.Name);
            Assert.AreEqual("(?<year>[12][0-9]{3})(?<month>1[0-2]|0[1-9])(?<day>0[1-9]|[1-2][0-9]|3[0-1])", bFiled.Regex.Rule.ToString());
            Assert.AreEqual(8, bFiled.Size);
            Assert.AreEqual(typeof(DateTime), bFiled.Type);

            bFiled = bLine.BlueprintFields[4];
            Assert.AreEqual("NumRemessa", bFiled.Attribute);
            Assert.AreEqual(bLine, bFiled.BlueprintLine);
            Assert.AreEqual(true, bFiled.Persist);
            Assert.AreEqual(38, bFiled.Position);
            Assert.AreEqual(-1, bFiled.Precision);
            Assert.AreEqual(null, bFiled.Regex);
            Assert.AreEqual(6, bFiled.Size);
            Assert.AreEqual(typeof(int), bFiled.Type);

            bFiled = bLine.BlueprintFields[5];
            Assert.AreEqual("NumVersao", bFiled.Attribute);
            Assert.AreEqual(bLine, bFiled.BlueprintLine);
            Assert.AreEqual(true, bFiled.Persist);
            Assert.AreEqual(44, bFiled.Position);
            Assert.AreEqual(-1, bFiled.Precision);
            Assert.AreEqual(null, bFiled.Regex);
            Assert.AreEqual(2, bFiled.Size);
            Assert.AreEqual(typeof(string), bFiled.Type);

            bFiled = bLine.BlueprintFields[6];
            Assert.AreEqual("FillerA", bFiled.Attribute);
            Assert.AreEqual(bLine, bFiled.BlueprintLine);
            Assert.AreEqual(false, bFiled.Persist);
            Assert.AreEqual(46, bFiled.Position);
            Assert.AreEqual(-1, bFiled.Precision);
            Assert.AreEqual(null, bFiled.Regex);
            Assert.AreEqual(22, bFiled.Size);
            Assert.AreEqual(typeof(string), bFiled.Type);
        }
    }
}
