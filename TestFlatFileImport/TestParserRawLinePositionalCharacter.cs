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
            
        }

        [Test]    
        public void TestMock()
        {
           
        }
    }
}
