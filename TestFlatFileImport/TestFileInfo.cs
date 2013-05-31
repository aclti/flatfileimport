using System.IO;
using System.Linq;
using FlatFileImport.Input;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestFileInfo : TestAbstract
    {
	    private IHandlerFactory _fac;

		[SetUp]
		public override void Setup()
		{
			base.Setup();
			_fac = new HandlerFacotry();
		}

        [Test]
        public void TestCraeteFileFInfoTextSingle()
        {
			var path = Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt");
			var handler = _fac.Get(path);
			var fileInfo = handler.FileInfo;

			Assert.AreEqual(path, handler.Path);
			Assert.AreEqual(SigleDasn, Path.GetDirectoryName(handler.Path));
			Assert.AreEqual(path, handler.Path);
			Assert.IsNotNull(fileInfo.Extesion);
			Assert.AreEqual(".txt", fileInfo.Extesion.Name);
			Assert.AreEqual(FileType.Text, fileInfo.Extesion.Type);
			Assert.AreEqual("AAAAA|108|20100701|20100715", fileInfo.Header);
        }

        [Test]
        public void TestCraeteFileFInfoZipSingle()
        {
			var path = Path.Combine(SigleDasn, "02-3105-DASN10-20100415-01.zip");
			var handler = _fac.Get(path);

			var fileInfo = handler.FileInfo;

			Assert.AreEqual(path, handler.Path);
			Assert.AreEqual(SigleDasn, Path.GetDirectoryName(handler.Path));
			Assert.AreEqual(Path.GetFileName(path),  Path.GetFileName(handler.Path));
			Assert.IsNotNull(fileInfo.Extesion);
			Assert.AreEqual("AAAAA|108|20100401|20100415", fileInfo.Header);
        }

        [Test]
        public void TestCraeteFileFInfoZipMulti()
        {
			var path = MultDasn;
			var handlers = new HandlerDirectory(path).Handlers.ToList();
	        var handler = handlers[0];
			var fileInfo = handler.FileInfo;

			Assert.IsNotNull(fileInfo);
			Assert.AreEqual(path + "\\02-3105-DASN09-20100515-01.zip", handler.Path);
			Assert.AreEqual(MultDasn, Path.GetDirectoryName(handler.Path));
			Assert.AreEqual("02-3105-DASN09-20100515-01.zip", Path.GetFileName(handler.Path));
			Assert.IsNotNull(fileInfo.Extesion);
			Assert.AreEqual("AAAAA|105|20100501|20100515", fileInfo.Header);

			handler = handlers[1];
			fileInfo = handler.FileInfo;

			Assert.IsNotNull(fileInfo);
			Assert.AreEqual(path + "\\02-3105-DASN10-20100315-01.zip", handler.Path);
			Assert.AreEqual(MultDasn, Path.GetDirectoryName(handler.Path));
			Assert.AreEqual("02-3105-DASN10-20100315-01.zip", Path.GetFileName(handler.Path));
			Assert.IsNotNull(fileInfo.Extesion);
			Assert.AreEqual("AAAAA|108|20100315|20100315", fileInfo.Header);

			handler = handlers[2];
			fileInfo = handler.FileInfo;

			Assert.IsNotNull(fileInfo);
			Assert.AreEqual(path + "\\02-3105-DASN10-20100731-01.txt", handler.Path);
			Assert.AreEqual(MultDasn, Path.GetDirectoryName(handler.Path));
			Assert.AreEqual("02-3105-DASN10-20100731-01.txt", Path.GetFileName(handler.Path));
			Assert.IsNotNull(fileInfo.Extesion);
			Assert.AreEqual("AAAAA|108|20100716|20100731", fileInfo.Header);

			handler = handlers[3];
			fileInfo = handler.FileInfo;

			Assert.IsNotNull(fileInfo);
			Assert.AreEqual(path + "\\02-3105-DASN10-20100915-01.txt", handler.Path);
			Assert.AreEqual(MultDasn, Path.GetDirectoryName(handler.Path));
			Assert.AreEqual("02-3105-DASN10-20100915-01.txt", Path.GetFileName(handler.Path));
			Assert.IsNotNull(fileInfo.Extesion);
			Assert.AreEqual("AAAAA|108|20100901|20100915", fileInfo.Header);
        }

        [Test]
        public void TestCraeteFileFInfoZipSingleLineNumber()
        {
			Assert.Inconclusive();
			//var path = Path.Combine(SigleDas, "02-3105-DAS-20090722-01.zip");
			//var handler = Handler.GetHandler(path);
			//var enumerator = handler.GetEnumerator();
			//enumerator.MoveNext();
			//var fileInfo = enumerator.Current;

			//Assert.IsNotNull(fileInfo);
			//Assert.AreEqual("AAAAA|120|20090722|20090722", fileInfo.Header);
			//Assert.AreEqual(String.Empty, fileInfo.Line);
			//Assert.AreEqual(0, fileInfo.LineNumber);

			//fileInfo.MoveToNext();
			//Assert.AreEqual("AAAAA|120|20090722|20090722", fileInfo.Line);
			//Assert.AreEqual(1, fileInfo.LineNumber);

			//fileInfo.MoveToNext();
			//Assert.AreEqual("00000|00430700000116|SANTOS PROTESE DENTAL COMERCIO E SERVICOS LTDA|3105|S|19950210|200906|3942,20|0,000|1,00|R|0|", fileInfo.Line);
			//Assert.AreEqual(2, fileInfo.LineNumber);

			//fileInfo.MoveToNext();
			//Assert.AreEqual("01000|01070920300051855|177,40|2,92|0,00|180,32|20090724|20090731|180,32", fileInfo.Line);
			//Assert.AreEqual(3, fileInfo.LineNumber);

			//var line = "";
			//for (var i = 0; i < 50; i++)
			//	fileInfo.MoveToNext();

			//line = fileInfo.Line;
			//Assert.AreEqual("03000|00543079000288|SE|3105|73560,77|1,00|1200000,00|0||", line);
			//Assert.AreEqual(53, fileInfo.LineNumber);

			//for (var i = 0; i < 200; i++)
			//	fileInfo.MoveToNext();

			//line = fileInfo.Line;
			//Assert.AreEqual("03110|||5551,00|0|0|0|0|0|0|0|0|10,260|569,53|1,430|79,370|0,430|23,860|||4,070|225,960|||0,480|26,640|3,500|194,280|0,350|19,420|0,04|5", line);
			//Assert.AreEqual(253, fileInfo.LineNumber);

			//for (var i = 0; i < 1000; i++)
			//	fileInfo.MoveToNext();

			//line = fileInfo.Line;
			//Assert.AreEqual("03110|||4350,00|0|0|5|0|0|0|0|0|2,750|119,63|0,000|0,000|0,000|0,000|0,000|0,000|2,750|119,630|||0,000|0,000|||0,000|0,000|0,01|5", line);
			//Assert.AreEqual(1253, fileInfo.LineNumber);


			//while (fileInfo.MoveToNext())
			//	line = fileInfo.Line;

			//Assert.AreEqual("ZZZZZ|6095", fileInfo.Line);
			//fileInfo.MoveToNext();
			//Assert.AreEqual("ZZZZZ|6095", fileInfo.Line);
        }
    }
}