using System;
using System.IO;
using FlatFileImport.Input;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestFileInfo : TestAbstract
    {
        [Test]
        public void TestCraeteFileFInfoTextSingle()
        {
            var path = Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt");
            var handler = Handler.GetHandler(path);
            var enumerator = handler.GetEnumerator();
            enumerator.MoveNext();
            var fileInfo = enumerator.Current;

            Assert.AreEqual(path, fileInfo.Path);
            Assert.AreEqual(SigleDasn, fileInfo.Directory);
            Assert.AreEqual(Path.GetFileName(path), fileInfo.Name);
            Assert.IsNotNull(fileInfo.Extesion);
            Assert.AreEqual(".txt", fileInfo.Extesion.Name);
            Assert.AreEqual(FileType.Text, fileInfo.Extesion.Type);
            Assert.AreEqual("AAAAA|108|20100701|20100715", fileInfo.Header);
        }

        [Test]
        public void TestCraeteFileFInfoZipSingle()
        {
            var path = Path.Combine(SigleDasn, "02-3105-DASN10-20100415-01.zip");
            var handler = Handler.GetHandler(path);
            var enumerator = handler.GetEnumerator();
            enumerator.MoveNext();
            var fileInfo = enumerator.Current;

            Assert.AreEqual(path, fileInfo.Path);
            Assert.AreEqual(SigleDasn, fileInfo.Directory);
            Assert.AreEqual(Path.GetFileName(path), fileInfo.Name);
            Assert.IsNotNull(fileInfo.Extesion);
            Assert.AreEqual(".zip", fileInfo.Extesion.Name);
            Assert.AreEqual(FileType.Binary, fileInfo.Extesion.Type);
            Assert.AreEqual("AAAAA|108|20100401|20100415", fileInfo.Header);
        }

        [Test]
        public void TestCraeteFileFInfoZipMulti()
        {
            var path = MultDasn;
            var handler = Handler.GetHandler(path);
            var enumerator = handler.GetEnumerator();
            
            enumerator.MoveNext();
            var fileInfo = enumerator.Current;

            Assert.IsNotNull(fileInfo);
            Assert.AreEqual(path + "\\02-3105-DASN09-20100515-01.zip", fileInfo.Path);
            Assert.AreEqual(MultDasn, fileInfo.Directory);
            Assert.AreEqual("02-3105-DASN09-20100515-01.zip", fileInfo.Name);
            Assert.IsNotNull(fileInfo.Extesion);
            Assert.AreEqual(".zip", fileInfo.Extesion.Name);
            Assert.AreEqual(FileType.Binary, fileInfo.Extesion.Type);
            Assert.AreEqual("AAAAA|105|20100501|20100515", fileInfo.Header);

            enumerator.MoveNext();
            fileInfo = enumerator.Current;

            Assert.IsNotNull(fileInfo);
            Assert.AreEqual(path + "\\02-3105-DASN10-20100315-01.zip", fileInfo.Path);
            Assert.AreEqual(MultDasn, fileInfo.Directory);
            Assert.AreEqual("02-3105-DASN10-20100315-01.zip", fileInfo.Name);
            Assert.IsNotNull(fileInfo.Extesion);
            Assert.AreEqual(".zip", fileInfo.Extesion.Name);
            Assert.AreEqual(FileType.Binary, fileInfo.Extesion.Type);
            Assert.AreEqual("AAAAA|108|20100315|20100315", fileInfo.Header);

            enumerator.MoveNext();
            fileInfo = enumerator.Current;

            Assert.IsNotNull(fileInfo);
            Assert.AreEqual(path + "\\02-3105-DASN10-20100731-01.txt", fileInfo.Path);
            Assert.AreEqual(MultDasn, fileInfo.Directory);
            Assert.AreEqual("02-3105-DASN10-20100731-01.txt", fileInfo.Name);
            Assert.IsNotNull(fileInfo.Extesion);
            Assert.AreEqual(".txt", fileInfo.Extesion.Name);
            Assert.AreEqual(FileType.Text, fileInfo.Extesion.Type);
            Assert.AreEqual("AAAAA|108|20100716|20100731", fileInfo.Header);

            enumerator.MoveNext();
            fileInfo = enumerator.Current;

            Assert.IsNotNull(fileInfo);
            Assert.AreEqual(path + "\\02-3105-DASN10-20100915-01.txt", fileInfo.Path);
            Assert.AreEqual(MultDasn, fileInfo.Directory);
            Assert.AreEqual("02-3105-DASN10-20100915-01.txt", fileInfo.Name);
            Assert.IsNotNull(fileInfo.Extesion);
            Assert.AreEqual(".txt", fileInfo.Extesion.Name);
            Assert.AreEqual(FileType.Text, fileInfo.Extesion.Type);
            Assert.AreEqual("AAAAA|108|20100901|20100915", fileInfo.Header);
        }

        [Test]
        public void TestCraeteFileFInfoZipSingleLineNumber()
        {
            var path = Path.Combine(SigleDasn, "02-3105-DASN10-20100415-01.zip");
            var handler = Handler.GetHandler(path);
            var enumerator = handler.GetEnumerator();
            enumerator.MoveNext();
            var fileInfo = enumerator.Current;

            Assert.IsNotNull(fileInfo);
            Assert.AreEqual("AAAAA|108|20100401|20100415", fileInfo.Header);
            Assert.AreEqual("AAAAA|108|20100401|20100415", fileInfo.Line);
            Assert.AreEqual(1, fileInfo.LineNumber);

            fileInfo.MoveToNext();
            Assert.AreEqual("D1000|000689662009001|1|2009|CHAME TAXI LTDA EPP|19940517|19940517|02071009400003615|00206062898925166181|20100404084639|1.0.1.0|0", fileInfo.Line);
            Assert.AreEqual(2, fileInfo.LineNumber);

            fileInfo.MoveToNext();
            Assert.AreEqual("D3001|200801|17408,76", fileInfo.Line);
            Assert.AreEqual(3, fileInfo.LineNumber);

            var line = "";
            for (var i = 0; i < 50; i++)
                fileInfo.MoveToNext();

            line = fileInfo.Line;
            Assert.AreEqual("04000|1004|298,89||||||", line);
            Assert.AreEqual(53, fileInfo.LineNumber);

            for (var i = 0; i < 200; i++)
                fileInfo.MoveToNext();

            line = fileInfo.Line;
            Assert.AreEqual("D4000|00090021200901004|200901|44929,68|4237,19|4350,07", line);
            Assert.AreEqual(253, fileInfo.LineNumber);

            for (var i = 0; i < 1000; i++)
                fileInfo.MoveToNext();

            line = fileInfo.Line;
            Assert.AreEqual("02000|585297,32|664972,70|162969,33|585297,32|664972,70||", line);
            Assert.AreEqual(1253, fileInfo.LineNumber);


            while (fileInfo.MoveToNext())
                line = fileInfo.Line;

            Assert.AreEqual("ZZZZZ|284184", fileInfo.Line);
            fileInfo.MoveToNext();
            Assert.AreEqual("ZZZZZ|284184", fileInfo.Line);
        }
    }
}