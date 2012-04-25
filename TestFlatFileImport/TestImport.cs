using System.IO;
using FlatFileImport;
using FlatFileImport.Core;
using FlatFileImport.Input;
using NUnit.Framework;
using TestFlatFileImport.Dominio;

namespace TestFlatFileImport
{   
    [TestFixture]
    public class TestImport : TestAbstract
    {
        [Test]
        public void TestConstrutor()
        {
            //IBlueprintFactoy factoy = new BlueprintFactory();
            //var importer = new Importer();
            //var handler = Handler.GetHandler(Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt"));
            //var enumerator = handler.GetEnumerator();
            //enumerator.MoveNext();

            //var bPrint = factoy.GetBlueprint(typeof(Dasn), enumerator.Current);
            //importer.SetBlueprint(bPrint);
            //var file = new FlatFileImport.Input.FileInfo(Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt"), new FileExtension("txt", FileType.Text));
            //importer.SetFileToProcess(file);
            //importer.Process();
            
        }

    }
}
