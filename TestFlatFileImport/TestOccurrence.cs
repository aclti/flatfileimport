using System;
using System.IO;
using FlatFileImport.Core;
using FlatFileImport.Process;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestOccurrence
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
        public void TestSetRangeForOccurrence()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();

            IBlueprintLine bline = new BlueprintLineHeader(_blueprint, null);
            
            IOccurrence occurrence = new Occurrence(bline, EnumOccurrence.AtLeastOne);
            Assert.AreEqual(EnumOccurrence.AtLeastOne, occurrence.Type);

            occurrence = new Occurrence(bline, EnumOccurrence.NoOrMany);
            Assert.AreEqual(EnumOccurrence.NoOrMany, occurrence.Type);

            occurrence = new Occurrence(bline, EnumOccurrence.NoOrOne);
            Assert.AreEqual(EnumOccurrence.NoOrOne, occurrence.Type);

            occurrence = new Occurrence(bline, EnumOccurrence.One);
            Assert.AreEqual(EnumOccurrence.One, occurrence.Type);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetRangeForOccurrenceNone()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();

            IBlueprintLine bline = new BlueprintLineHeader(_blueprint, null);

            IOccurrence occurrence = new Occurrence(bline, EnumOccurrence.None);
            Assert.AreEqual(EnumOccurrence.None, occurrence.Type);
        }

        [Test]
        public void TestSetRangeForOccurrenceRange()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();
            IBlueprintLine bline = new BlueprintLineHeader(_blueprint, null);

            IOccurrence occurrence = new Occurrence(bline, EnumOccurrence.Range, "Range[1-5]");
            
            Assert.AreEqual(EnumOccurrence.Range, occurrence.Type);
            Assert.AreEqual(1, occurrence.Min);
            Assert.AreEqual(5, occurrence.Max);

            occurrence = new Occurrence(bline, EnumOccurrence.Range, "Range[10-90]");
            Assert.AreEqual(EnumOccurrence.Range, occurrence.Type);
            Assert.AreEqual(10, occurrence.Min);
            Assert.AreEqual(90, occurrence.Max);

            occurrence = new Occurrence(bline, EnumOccurrence.Range, "Range[1000-90000]");
            Assert.AreEqual(EnumOccurrence.Range, occurrence.Type);
            Assert.AreEqual(1000, occurrence.Min);
            Assert.AreEqual(90000, occurrence.Max);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestSetRangeForOccurrenceRangeWrongFormat()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();
            IBlueprintLine bline = new BlueprintLineHeader(_blueprint, null);

            IOccurrence occurrence = new Occurrence(bline, EnumOccurrence.Range, "Range[1-E]");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestSetRangeForOccurrenceRangeFormatNoRange()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();
            IBlueprintLine bline = new BlueprintLineHeader(_blueprint, null);

            IOccurrence occurrence = new Occurrence(bline, EnumOccurrence.AtLeastOne, "Range[1-5]");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestSetRangeForOccurrenceMinGreatMax()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();
            IBlueprintLine bline = new BlueprintLineHeader(_blueprint, null);

            IOccurrence occurrence = new Occurrence(bline, EnumOccurrence.Range, "Range[10-5]");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestSetRangeForOccurrenceMinEqualtMax()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();
            IBlueprintLine bline = new BlueprintLineHeader(_blueprint, null);

            IOccurrence occurrence = new Occurrence(bline, EnumOccurrence.Range, "Range[5-5]");
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void TestSetRangeForOccurrenceIntBiggerThan32Bit()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "siafi-simplicaficado.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();
            IBlueprintLine bline = new BlueprintLineHeader(_blueprint, null);

            IOccurrence occurrence = new Occurrence(bline, EnumOccurrence.Range, "Range[5555555555555555555555555555555555555555555-888888888888888888888888888888888888888888888888888888888888888888]");
        }
    }
}