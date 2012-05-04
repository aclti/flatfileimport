using System;
using System.IO;
using System.Text;
using FlatFileImport;
using FlatFileImport.Core;
using FlatFileImport.Input;
using FlatFileImport.Exception;

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

            // Chamando o Process
            //var file = new FlatFileImport.Input.FileInfo(Path.Combine(_path, "02-3105-DASN10-20100731-01.txt"), new FileExtension("txt", FileType.Text));
            //var bPrint = factoy.GetBlueprint(typeof(Dasn), file);
            //importer.SetBlueprint(bPrint);
            //importer.SetFileToProcess(file);
            //importer.RegisterObserver(view);
            //importer.Process();
            //view.ShowHeader(file.Name);
            //view.ShowContent();
            //view.ShowFooter();

            var file = new FlatFileImport.Input.FileInfo(Path.Combine(_path, "dasn-resumido-001.txt"), new FileExtension("txt", FileType.Text));
            var bPrint = factoy.GetBlueprint(typeof(Dasn), file);
            importer.SetBlueprint(bPrint);
            importer.SetFileToProcess(file);
            importer.RegisterObserver(view);
            importer.Valid();

            if(importer.IsValid)
                importer.Process();
            else
            {
                var sb = new StringBuilder();

                foreach (var s in importer.Results)
                {
                    
                    sb.AppendLine();
                    sb.AppendFormat("[ {0} ][ {1} ][ {2} ][ {3} ]", s.Value, s.Message, s.Type, s.Severity);
                }

                Console.WriteLine(sb.ToString());
            }
            //view.ShowHeader(file.Name);
            //view.ShowContent();
            //view.ShowFooter();

            Console.WriteLine("Pressione qualquer tecla para continuar.");
            Console.ReadKey();

        }
    }
}