using System;
using System.IO;
using FlatFileImport.Input;
using NUnit.Framework;

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
            if(!enumerator.MoveNext())
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
    }
}