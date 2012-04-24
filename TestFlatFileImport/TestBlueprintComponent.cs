using System;
using System.IO;
using FlatFileImport.Core;
using FlatFileImport.Process;
using NUnit.Framework;
using System.Linq;


namespace TestFlatFileImport
{
    public class TestBlueprintComponent
    {
        private string _path;
        private string _blueprintPath;
        private IBlueprintSetter _blueprintSetter;

        private IBlueprint _blueprint;

        [SetUp]
        public void Setup()
        {
            _path = AppDomain.CurrentDomain.BaseDirectory;
            _blueprintPath = Path.Combine(_path, @"Samples\Blueprints\blueprint-dasn.xml"); ;

            _blueprintSetter = new BlueprintSetterXml(_blueprintPath);
            _blueprint = _blueprintSetter.GetBlueprint();
        }

        [TearDown]
        public void End()
        {
            _path = String.Empty;
            _blueprintPath = String.Empty;
        }

        [Test]
        public void TestBluePrint()
        {
            Assert.AreEqual(_blueprint.BluePrintCharSepartor, '|');
            //Assert.AreEqual(49, _blueprint.BlueprintLines.Count(l => l.Parent != null));
            //Assert.AreEqual(_blueprint.BlueprintRegistires.Count, 1);
            Assert.AreEqual(_blueprint.FieldSeparationType, EnumFieldSeparationType.Character);
            //Assert.IsNotNull(_blueprint.Footer);
            //Assert.IsNotNull(_blueprint.Header);
            //Assert.IsTrue(_blueprint.UseRegistries);
        }

        [Test]
        public void TestBlueprintHeader()
        {
            var bField = _blueprint.BlueprintLines.FirstOrDefault(l => l.Parent == null && l.GetType() == typeof(BlueprintLineHeader));
            Assert.AreEqual(bField.BlueprintFields.Count, 4);
            Assert.AreEqual(bField.Name, "AAAAA");
            Assert.IsTrue(/*_blueprintLine.Mandatory*/true);
            Assert.AreEqual(bField.Regex.ToString(), "^AAAAA");
            Assert.AreEqual(bField.Blueprint, _blueprint);    
        }

        [Test]
        public void TestBlueprintFooter()
        {
            var bField = _blueprint.BlueprintLines.FirstOrDefault(l => l.Name == "ZZZZZ");
            Assert.AreEqual(bField.BlueprintFields.Count, 2);
            Assert.AreEqual(bField.Name, "ZZZZZ");
            Assert.IsTrue(/*_blueprintLine.Mandatory*/true);
            Assert.AreEqual(bField.Regex.ToString(), "^ZZZZZ");
            Assert.AreEqual(bField.Blueprint, _blueprint);    
        }

        [Test]
        public void TestBlueprintLine()
        {
            var bLine = _blueprint.BlueprintLines.FirstOrDefault(l => l.Name == "D2000");

            Assert.AreEqual(bLine.BlueprintFields.Count, 3);
            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bLine.Name, "D2000");
            Assert.AreEqual(/*_blueprintLine.Mandatory*/false, false);
            Assert.AreEqual(bLine.Regex.ToString(), "^D2000");

            bLine = _blueprint.BlueprintLines.FirstOrDefault(l => l.Name == "D4000");

            Assert.AreEqual(bLine.BlueprintFields.Count, 6);
            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bLine.Name, "D4000");
            Assert.AreEqual(/*_blueprintLine.Mandatory*/false, false);
            Assert.AreEqual(bLine.Regex.ToString(), "^D4000");

            bLine = _blueprint.BlueprintLines.FirstOrDefault(l => l.Name == "P4000");

            Assert.AreEqual(bLine.BlueprintFields.Count, 9);
            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bLine.Name, "P4000");
            Assert.AreEqual(/*_blueprintLine.Mandatory*/false, false);
            Assert.AreEqual(bLine.Regex.ToString(), "^04000");

            bLine = _blueprint.BlueprintLines.FirstOrDefault(l => l.Name == "P3110");

            Assert.AreEqual(bLine.BlueprintFields.Count, 32);
            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bLine.Name, "P3110");
            Assert.AreEqual(/*_blueprintLine.Mandatory*/false, false);
            Assert.AreEqual(bLine.Regex.ToString(), "^03110");
        }

        [Test]
        public void TestBlueprintFields()
        {
            var bLine = _blueprint.BlueprintLines.FirstOrDefault(l => l.Name == "D4000");
            var bField = bLine.BlueprintFields[1];

            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bField.Parent, bLine);
            Assert.AreEqual(bField.Name, "NUM_SEQ");
            Assert.AreEqual(bField.Persist, true);
            Assert.AreEqual(bField.Position, 1);
            Assert.AreEqual(bField.Precision, -1);
            Assert.AreEqual(bField.Regex, null);
            Assert.AreEqual(bField.Size, 17);
            Assert.AreEqual(bField.Type, typeof(string));

            bLine = _blueprint.BlueprintLines.FirstOrDefault(l => l.Name == "D3002");
            bField = bLine.BlueprintFields[2];

            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bField.Parent, bLine);
            Assert.AreEqual(bField.Name, "VALOR");
            Assert.AreEqual(bField.Persist, true);
            Assert.AreEqual(bField.Position, 2);
            Assert.AreEqual(bField.Precision, 2);
            Assert.AreEqual(bField.Regex.Name, "decimal");
            Assert.AreEqual(bField.Size, 17);
            Assert.AreEqual(bField.Type, typeof(decimal));

            bLine = _blueprint.BlueprintLines.FirstOrDefault(l => l.Name == "D4000");
            bField = bLine.BlueprintFields[2];

            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bField.Parent, bLine);
            Assert.AreEqual(bField.Name, "PERIODO");
            Assert.AreEqual(bField.Persist, true);
            Assert.AreEqual(bField.Position, 2);
            Assert.AreEqual(bField.Precision, -1);
            Assert.AreEqual(bField.Regex.Name, "period");
            Assert.AreEqual(bField.Regex.Rule.ToString(), "(?<year>[1-9][0-9]{3})(?<month>1[0-2]|0[1-9])");
            Assert.AreEqual(bField.Size, 6);
            Assert.AreEqual(bField.Type, typeof(DateTime));

            bLine = _blueprint.BlueprintLines.FirstOrDefault(l => l.Name == "D2000");
            bField = bLine.BlueprintFields[1];

            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bField.Parent, bLine);
            Assert.AreEqual(bField.Name, "ID_EVENTO");
            Assert.AreEqual(bField.Persist, true);
            Assert.AreEqual(bField.Position, 1);
            Assert.AreEqual(bField.Precision, -1);
            Assert.AreEqual(bField.Regex.Name, "rangenumber_1-6");
            Assert.AreEqual(bField.Regex.Rule.ToString(), "[1-6]");
            Assert.AreEqual(bField.Size, -1);
            Assert.AreEqual(bField.Type, typeof(int));

        }
    }
}
