using System;
using System.IO;
using FlatFileImport;
using FlatFileImport.Core;
using FlatFileImport.Input;

namespace TesteImportDasn
{
    class Program
    {
        private static string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\Files");

        static void Main(string[] args)
        {
            var view = new ViewMain();

            IBlueprintFactoy factoy = new BlueprintFactory();
            var importer = new Importer();

            
            //var handler = Handler.GetHandler(Path.Combine(_path, "02-3105-DASN10-20100715-01.txt"));
            //var handler = Handler.GetHandler(_path);
            //var enumerator = handler.GetEnumerator();

            importer.RegisterObserver(view);

            //while (enumerator.MoveNext())
            //{
                
            //    var bPrint = factoy.GetBlueprint(typeof(Dasn), enumerator.Current);
            //    importer.SetBlueprint(bPrint);
            //    importer.SetFileToProcess(enumerator.Current);
            //    importer.Process();
                
            
                
            //}

            var file = new FlatFileImport.Input.FileInfo(Path.Combine(_path, "02-3105-DASN10-20100731-01.txt"), new FileExtension("txt", FileType.Text));
            var bPrint = factoy.GetBlueprint(typeof(Dasn), file);
            importer.SetBlueprint(bPrint);
            importer.SetFileToProcess(file);
            importer.RegisterObserver(view);
            importer.Process();
            view.ShowHeader(file.Name);
            view.ShowContent();
            view.ShowFooter();
        }
    }
}
