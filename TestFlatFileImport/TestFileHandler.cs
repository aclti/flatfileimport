using System;
using System.IO;
using FlatFileImport.Input;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestFileHandler
    {
        private string _pathSamples;
        private string _sigleDasn;
        private string _multDasn;
        private string _sigleDas;
        private string _multDas;

        [SetUp]
        public void Setup()
        {
            _pathSamples = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\Files");
            _sigleDas = Path.Combine(_pathSamples, @"Dasn\Single");
            _sigleDasn = Path.Combine(_pathSamples, @"Das\Single");
            _multDas = Path.Combine(_pathSamples, @"Das\Mult");
            _multDasn = Path.Combine(_pathSamples, @"Dasn\Mult");
        }

        [TearDown]
        public void End()
        {
            _pathSamples = String.Empty;
            _sigleDasn = String.Empty;
            _multDasn = String.Empty;
            _sigleDas = String.Empty;
            _multDas = String.Empty;
        }

        [Test]
        public void TesDirExist()
        {
            Assert.IsTrue(Directory.Exists(_pathSamples));
            Assert.IsTrue(Directory.Exists(_sigleDas));
            Assert.IsTrue(Directory.Exists(_sigleDasn));
            Assert.IsTrue(Directory.Exists(_multDas));
            Assert.IsTrue(Directory.Exists(_multDasn));
        }

        [Test]
        public void TestFlatTextFile()
        {
            var handler = new HandlerText(Path.Combine(_sigleDasn, "02-3105-DASN10-20100915-01.txt"));
        }
    }
}
