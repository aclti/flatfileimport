using System;
using System.IO;
using FlatFileImport.Input;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestFileExtension
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
        public void TestValidExtension()
        {
            Assert.True(FileExtension.IsValid(".txt"));
            Assert.True(FileExtension.IsValid(".txt64"));
            Assert.True(FileExtension.IsValid(Path.GetExtension(".zip")));
            Assert.True(FileExtension.IsValid(Path.GetExtension(Path.Combine(_sigleDasn, "02-3105-DASN10-20100415-01.zip"))));
            Assert.True(FileExtension.IsValid(Path.GetExtension(Path.Combine(_sigleDasn, "02-3105-DASN10-20100415-01.zipp"))));

            Assert.True(!FileExtension.IsValid(".tx"));
            Assert.True(!FileExtension.IsValid("058.txt"));
            Assert.True(!FileExtension.IsValid(""));
            Assert.True(!FileExtension.IsValid("txt"));
            Assert.True(!FileExtension.IsValid(Path.GetExtension("zip")));
        }

        [Test]
        public void TestCreateExtension()
        {
            var ex = new FileExtension(".Txt", FileType.Text);

            Assert.AreEqual(ex.Extension, ".txt");
            Assert.AreEqual(ex.Type, FileType.Text);


            ex = new FileExtension(".RAR", FileType.Binary);

            Assert.AreEqual(ex.Extension, ".rar");
            Assert.AreEqual(ex.Type, FileType.Binary);

            ex = new FileExtension(".zIp", FileType.Binary);

            Assert.AreEqual(ex.Extension, ".zip");
            Assert.AreEqual(ex.Type, FileType.Binary);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void TestCreateExtesionExceptionNullArgumentExtension()
        {
            var ex = new FileExtension(null, FileType.Text);
        }
        
        [Test]
        [ExpectedException("System.ArgumentException")]
        public void TestCreateExtesionExceptionInvalidArguementType()
        {
            var ex = new FileExtension(".txt", FileType.Undefined);
        }

        [Test]
        [ExpectedException("System.Exception")]
        public void TestCreateExtesionExceptionInvalidExtension()
        {
            var ex = new FileExtension("", FileType.Text);
        }
    }
}


