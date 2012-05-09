using System;
using System.IO;
using FlatFileImport.Core;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestBlueprintSetter
    {
        private string _path;
        private string _blueprintPath;

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
        }
    }
}
