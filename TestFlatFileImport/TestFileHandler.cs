using System;
using System.IO;

using FlatFileImport.Exception;
using FlatFileImport.Input;
using NUnit.Framework;
using System.Linq;

namespace TestFlatFileImport
{
    public class TestFileHandler : TestAbstract
    {
        [Test]
        public void TesDirExist()
        {
            Assert.IsTrue(Directory.Exists(PathSamples));
            Assert.IsTrue(Directory.Exists(SigleDas));
            Assert.IsTrue(Directory.Exists(SigleDasn));
            Assert.IsTrue(Directory.Exists(MultDas));
            Assert.IsTrue(Directory.Exists(MultDasn));
        }

        [Test]
        public void TestFlatTextFile()
        {
            var handler = Handler.GetHandler(Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt"));
            Assert.IsTrue(handler is HandlerText);

            handler = Handler.GetHandler(MultDas);
            Assert.IsTrue(handler is HandlerDirectory);

            handler = Handler.GetHandler(Path.Combine(SigleDasn, "02-3105-DASN10-20100415-01.zip"));
            Assert.IsTrue(handler is HandlerZip);
        }

        [Test]
        public void TestFileInfoText()
        {
            var handler = Handler.GetHandler(Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt"));
            Assert.IsTrue(handler is HandlerText);

            var enumerator = handler.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new Exception("Nenhum FileInfo econtrado.");

            var info = enumerator.Current;

            Assert.IsNotNull(info);
            Assert.AreEqual(SigleDasn, info.Directory);
            Assert.IsTrue(info.Extesion.Equals(new FileExtension(".txt", FileType.Text)));
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Header); // O herader é uma linha especial que não altera o contador
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Header); // verifica se o leitor não passa para a proxima linha
            Assert.AreEqual(0, info.LineNumber); // não aciona o contador até ser chamado o método MoveToNext.
            Assert.AreEqual("02-3105-DASN10-20100715-01.txt", info.Name);
            Assert.AreEqual(Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt"), info.Path);
            info.MoveToNext();
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Line);
            Assert.AreEqual(1, info.LineNumber);
            info.MoveToNext();
            Assert.AreEqual("D1000|010428182009003|2|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0", info.Line);
            Assert.AreEqual(2, info.LineNumber);
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Header);
        }

        [Test]
        public void TestFileInfoTextReset()
        {
            var handler = Handler.GetHandler(Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt"));
            Assert.IsTrue(handler is HandlerText);

            var enumerator = handler.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new Exception("Nenhum FileInfo econtrado.");

            var info = enumerator.Current;

            Assert.IsNotNull(info);
            Assert.AreEqual(SigleDasn, info.Directory);
            Assert.IsTrue(info.Extesion.Equals(new FileExtension(".txt", FileType.Text)));

            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Header);
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Header); // verifica se o leitor não passa para a proxima linha
            Assert.AreEqual(0, info.LineNumber); // O contador não é iniciado.

            Assert.AreEqual("02-3105-DASN10-20100715-01.txt", info.Name);
            Assert.AreEqual(Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt"), info.Path);
            info.MoveToNext();
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Line);
            Assert.AreEqual(1, info.LineNumber);

            info.MoveToNext();
            Assert.AreEqual("D1000|010428182009003|2|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0", info.Line);
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Header);
            Assert.AreEqual(2, info.LineNumber);

            info.Reset();
            Assert.AreEqual(String.Empty, info.Line);
            Assert.AreEqual(0, info.LineNumber);

            info.MoveToNext();
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Line);
            Assert.AreEqual(1, info.LineNumber);

            info.MoveToNext();
            Assert.AreEqual("D1000|010428182009003|2|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0", info.Line);
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Header);
            Assert.AreEqual(2, info.LineNumber);
        }

        [Test]
        public void TestFileInfoTextDispose()
        {
            var pathFileToDelete = Path.Combine(SigleDasn, "teste-delete.txt");
            File.Copy(Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt"), pathFileToDelete, true);
            var handler = Handler.GetHandler(pathFileToDelete);
            Assert.IsTrue(handler is HandlerText);

            var enumerator = handler.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new Exception("Nenhum FileInfo econtrado.");

            var info = enumerator.Current;

            Assert.IsNotNull(info);
            Assert.AreEqual(Path.GetFileName(pathFileToDelete), info.Name);
            Assert.AreEqual(pathFileToDelete, info.Path);

            info.MoveToNext();
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Line);
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Header);
            Assert.AreEqual(1, info.LineNumber);

            info.MoveToNext();
            Assert.AreEqual("D1000|010428182009003|2|2009|RENOTINTAS COMERCIO E REPRESENTACOES LTDA|19960208|19960208|02071018801526456|01406041942879518599|20100707161153|1.0.7.0|0", info.Line);
            Assert.AreEqual("AAAAA|108|20100701|20100715", info.Header);
            Assert.AreEqual(2, info.LineNumber);

            info.Release();
            Assert.AreEqual(String.Empty, info.Line);
            Assert.AreEqual(0, info.LineNumber);
            File.Delete(info.Path);
            Assert.IsFalse(File.Exists(info.Path));
        }

        [Test]
        public void TestFileInfoTextDisposeZipHandler()
        {
            var pathFileToDelete = Path.Combine(SigleDasn, "teste-delete" + DateTime.Now.Ticks + ".zip");
            File.Copy(Path.Combine(SigleDasn, "02-3105-DASN10-20100415-01.zip"), pathFileToDelete, true);
            var handler = Handler.GetHandler(pathFileToDelete);
            Assert.IsTrue(handler is HandlerZip);

            var file = handler.FirstOrDefault();

            Assert.IsNotNull(file);

            Assert.IsNotNull(file);
            Assert.AreEqual(Path.GetFileName(pathFileToDelete), file.Name);
            Assert.AreEqual(pathFileToDelete, file.Path);

            file.MoveToNext();
            Assert.AreEqual("AAAAA|108|20100401|20100415", file.Line);
            Assert.AreEqual("AAAAA|108|20100401|20100415", file.Header);
            Assert.AreEqual(1, file.LineNumber);

            file.MoveToNext();
            Assert.AreEqual("D1000|000689662009001|1|2009|CHAME TAXI LTDA EPP|19940517|19940517|02071009400003615|00206062898925166181|20100404084639|1.0.1.0|0", file.Line);
            Assert.AreEqual("AAAAA|108|20100401|20100415", file.Header);
            Assert.AreEqual(2, file.LineNumber);

            file.Release();
            Assert.AreEqual(String.Empty, file.Line);
            Assert.AreEqual(0, file.LineNumber);

            File.Delete(file.Path);
            Assert.IsFalse(File.Exists(file.Path));
        }


        [Test]
        public void TestHandlerIgnoreExtension()
        {
            Handler.IgnoreExtensions = new[] { ".foo" };
            var handler = Handler.GetHandler(IgnoreExtensions);

            Assert.IsTrue(handler is HandlerDirectory);
            Assert.AreEqual(2, handler.Count());

            Handler.IgnoreExtensions = new[] { ".foo", ".zip" };
            handler = Handler.GetHandler(IgnoreExtensions);

            Assert.IsTrue(handler is HandlerDirectory);
            Assert.AreEqual(1, handler.Count());
        }

        [Test]
        [ExpectedException(typeof(WrongTypeFileException))]
        public void TestHandlerIgnoreExtensionNotInformed()
        {
            Handler.IgnoreExtensions = null;
            var handler = Handler.GetHandler(IgnoreExtensions);

            Assert.IsTrue(handler is HandlerDirectory);
            Assert.AreEqual(2, handler.Count());
        }
    }
}