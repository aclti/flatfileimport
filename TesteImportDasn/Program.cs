using System;
using System.Collections.Generic;
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

        private static string GetVersion(IFileInfo toParse)
        {
            if (String.IsNullOrEmpty(toParse.Header))
                throw new NullReferenceException("A definição do Header, do FileInfo está fazia." + toParse.Path);

            string[] values = toParse.Header.Split("|".ToCharArray());

            if (values == null || values.Length <= 0)
                throw new NullReferenceException("Não foi possivel fazer o parse para verificar a versão do arquivo" + toParse.Path);

            if (values[0] != "AAAAA")
                throw new Exception("O campo AAAAA não existe no arquivo");

            return values[1];
        }

        static void Main(string[] args)
        {
            var view = new ViewMain();

            IBlueprintFactoy factoy = new BlueprintFactory();
            var importer = new Importer();
            IEnumerable<IFileInfo> handler = null;
            IEnumerator<IFileInfo> enumerator = null;

            try
            {
                //var handler = Handler.GetHandler(Path.Combine(_path, "02-3105-DASN10-20100715-01.txt"));
                //var handler = Handler.GetHandler(Path.Combine(_path, "dasn-resumido-001.txt")) ;
                handler = Handler.GetHandler(@"C:\Temp\_WEBISS\_PROJETOS\ss-importer\DASN\DASN");
                enumerator = handler.GetEnumerator();

                importer.RegisterObserver(view);
            }
            catch (Exception ex)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;

                using (var file = new StreamWriter(path + "ErroFileHandler.txt", true))
                {
                    file.WriteLine(String.Format("\n{0}\n", ex));
                }
            }
            

            while (enumerator.MoveNext())
            {
                try
                {
                    view.FilePath = enumerator.Current.Path;

                    var bPrint = factoy.GetBlueprint(typeof(Dasn), enumerator.Current);

                    if(bPrint == null)
                    {
                        var path = AppDomain.CurrentDomain.BaseDirectory;

                        using (var file = new StreamWriter(path + "Versoes.txt", true))
                        {

                            file.WriteLine(String.Format("\nVersão: {0} | File: {1}\n", GetVersion(enumerator.Current), enumerator.Current.Path));
                        }

                        continue;
                    }
                        

                    importer.SetBlueprint(bPrint);
                    importer.SetFileToProcess(enumerator.Current);
                    importer.Valid();

                    if (importer.IsValid)
                        importer.Process();

                    importer.Reset();
                }
                catch (Exception ex)
                {
                    var path = AppDomain.CurrentDomain.BaseDirectory;

                    using (var file = new StreamWriter(path + "Exception.txt", true))
                    {
                        file.WriteLine(String.Format("\n{0}\n", ex));
                    }
                }
            }


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

            //var file = new FlatFileImport.Input.FileInfo(Path.Combine(_path, "dasn-resumido-001.txt"), new FileExtension("txt", FileType.Text));
            //var bPrint = factoy.GetBlueprint(typeof(Dasn), file);
            //importer.SetBlueprint(bPrint);
            //importer.SetFileToProcess(file);
            //importer.RegisterObserver(view);

            //importer.Valid();

            //if(importer.IsValid)
            //    importer.Process();

            //view.ShowHeader(file.Name);
            //view.ShowContent();
            //view.ShowFooter();

            Console.WriteLine("Pressione qualquer tecla para continuar.");
            Console.ReadKey();

        }
    }
}