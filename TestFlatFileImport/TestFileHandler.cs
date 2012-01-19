using System;
using System.IO;
using FlatFileImport.Input;
using NUnit.Framework;
using FileInfo = FlatFileImport.Input.FileInfo;

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
            _sigleDas = Path.Combine(_pathSamples, @"Das\Single");
            _sigleDasn = Path.Combine(_pathSamples, @"Dasn\Single");
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
            var handler = Handler.GetHandler(Path.Combine(_sigleDasn, "02-3105-DASN10-20100715-01.txt"));
            Assert.IsTrue(handler is HandlerText);

            handler = Handler.GetHandler(_multDas);
            Assert.IsTrue(handler is HandlerDirectory);

            handler = Handler.GetHandler(Path.Combine(_sigleDasn, "02-3105-DASN10-20100415-01.zip"));
            Assert.IsTrue(handler is HandlerZip);
        }

        [Test]
        public void TestFileInfoText()
        {
            var handler = Handler.GetHandler(Path.Combine(_sigleDasn, "02-3105-DASN10-20100715-01.txt"));
            Assert.IsTrue(handler is HandlerText);
            
            var enumerator = handler.GetEnumerator();
            if(!enumerator.MoveNext())
                throw new Exception("Nenhum FileInfo econtrado.");

            var info = enumerator.Current;

            Assert.AreEqual(info.Comment, Path.Combine(_sigleDasn, "02-3105-DASN10-20100715-01.txt"));
            Assert.AreEqual(info.Directory, _sigleDasn);
            Assert.IsTrue(info.Extesion.Equals(new FileExtension(".txt", FileType.Text)));
            Assert.AreEqual(info.Header, "AAAAA|108|20100701|20100715");
            Assert.AreEqual(info.Header, "AAAAA|108|20100701|20100715"); // verifica se o leitor não passa para a proxima linha
            Assert.AreEqual(info.Name, "02-3105-DASN10-20100715-01.txt");
            Assert.AreEqual(info.Path, Path.Combine(_sigleDasn, "02-3105-DASN10-20100715-01.txt"));
            Assert.IsNotNull(info.Stream);
            Assert.AreEqual(info.Stream.ReadLine(), "D1000|010428182009003|2|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0");
        }
    }
}