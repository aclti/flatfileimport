using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FlatFileImport.Process;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestBlueprintComponent
    {
        private string _path;
        private string _blueprintPath;

        private IBlueprint _blueprint;

        [SetUp]
        public void Setup()
        {
            _path = AppDomain.CurrentDomain.BaseDirectory;
            _blueprintPath = Path.Combine(_path, @"Samples\Blueprints\blueprint-dasn.xml"); ;

            _blueprint = new Blueprint(_blueprintPath);
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
            Assert.AreEqual(_blueprint.BlueprintLines.Count, 49);
            Assert.AreEqual(_blueprint.BlueprintRegistires.Count, 1);
            Assert.AreEqual(_blueprint.FieldSeparationType, EnumFieldSeparationType.Character);
            Assert.IsNotNull(_blueprint.Footer);
            Assert.IsNotNull(_blueprint.Header);
            Assert.IsTrue(_blueprint.UseRegistries);
        }

        [Test]
        public void TestBlueprintHeader()
        {
            var bField = _blueprint.Header;
            Assert.AreEqual(bField.BlueprintFields.Count, 4);
            Assert.AreEqual(bField.Class, "AAAAA");
            Assert.IsTrue(bField.Mandatory);
            Assert.AreEqual(bField.Regex.ToString(), "^AAAAA");
            Assert.AreEqual(bField.Blueprint, _blueprint);    
        }

        [Test]
        public void TestBlueprintFooter()
        {
            var bField = _blueprint.Footer;
            Assert.AreEqual(bField.BlueprintFields.Count, 2);
            Assert.AreEqual(bField.Class, "ZZZZZ");
            Assert.IsTrue(bField.Mandatory);
            Assert.AreEqual(bField.Regex.ToString(), "^ZZZZZ");
            Assert.AreEqual(bField.Blueprint, _blueprint);    
        }

        [Test]
        public void TestBlueprintLine()
        {
            var bLine = _blueprint.BlueprintLines[1];

            Assert.AreEqual(bLine.BlueprintFields.Count, 3);
            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bLine.Class, "D2000");
            Assert.AreEqual(bLine.Mandatory, false);
            Assert.AreEqual(bLine.Regex.ToString(), "^D2000");

            bLine = _blueprint.BlueprintLines[6];

            Assert.AreEqual(bLine.BlueprintFields.Count, 6);
            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bLine.Class, "D4000");
            Assert.AreEqual(bLine.Mandatory, false);
            Assert.AreEqual(bLine.Regex.ToString(), "^D4000");

            bLine = _blueprint.BlueprintLines[11];

            Assert.AreEqual(bLine.BlueprintFields.Count, 9);
            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bLine.Class, "P4000");
            Assert.AreEqual(bLine.Mandatory, false);
            Assert.AreEqual(bLine.Regex.ToString(), "^04000");

            bLine = _blueprint.BlueprintLines[13];

            Assert.AreEqual(bLine.BlueprintFields.Count, 32);
            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bLine.Class, "P3110");
            Assert.AreEqual(bLine.Mandatory, false);
            Assert.AreEqual(bLine.Regex.ToString(), "^03110");
        }

        [Test]
        public void TestBlueprintFields()
        {
            var bLine = _blueprint.BlueprintLines[6];
            var bField = bLine.BlueprintFields[1];

            Assert.AreEqual(bLine.Blueprint, _blueprint);
            Assert.AreEqual(bField.BlueprintLine, bLine);
            Assert.AreEqual(bField.Attribute, "NUM_SEQ");
            Assert.AreEqual(bField.Persist, true);
            Assert.AreEqual(bField.Position, 1);
            Assert.AreEqual(bField.Precision, -1);
            Assert.AreEqual(bField.Regex, null);
            Assert.AreEqual(bField.Size, 17);
            Assert.AreEqual(bField.Type, typeof(string));
        }

    //<Line class="D4000" regex="^D4000" mandatory="false">
    //    <Fields>
    //        <Field position="0" type="string" size="5" persit="false" attribute="TIPO_REGISTRO" />
    //        <Field position="1" type="string" size="17" persit="true" attribute="NUM_SEQ" />
    //        <Field position="2" type="date" size="6" persit="true" regex="period" attribute="PERIODO" />
    //        <Field position="3" type="decimal" size="2" persit="true" attribute="VL_RECEITA_APURADA" />
    //        <Field position="4" type="decimal" size="2" persit="true" attribute="VL_DEBITO_APURADO" />
    //        <Field position="5" type="decimal" size="2" persit="true" attribute="VL_TOTAL_DAS_PAGOS" />
    //    </Fields>
    //</Line>
    }
}
