using System;
using System.IO;
using FlatFileImport.Core;
using FlatFileImport.Process;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestBlueprintSetter
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
        public void TempTest()
        {
            var setter = new BlueprintXmlSetter(Path.Combine(_blueprintPath, "blueprint-dasn-resumida.xml"));
            var bine = setter.GetBlueprint();
        }
    }
}
