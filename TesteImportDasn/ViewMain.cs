using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatFileImport;
using FlatFileImport.Data;
using FlatFileImport.Validate;
using System.IO;

namespace TesteImportDasn
{
    class Result
    {
        public string Num { set; get; }
        public string Line { set; get; }
    }

    class ViewMain : IObserver
    {
        private List<Result> _results;

        public string FilePath { set; get; }

        public void ShowHeader(string header)
        {
            var sb = new StringBuilder();

            sb.Append("".PadLeft(200, '/'));
            sb.AppendLine();
            sb.AppendLine(header);
            sb.Append("".PadLeft(200, '\\'));

            Console.WriteLine(sb.ToString());
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey(true);
        }

        public void ShowContent()
        {
            foreach (var result in _results)
                Console.WriteLine(String.Format("Linha:[{0}] - {1}", result.Num, result.Line));

            _results = new List<Result>();
        }

        public void ShowFooter()
        {
            var sb = new StringBuilder();
            sb.Append("".PadLeft(200, '\\'));
            sb.Append("".PadLeft(200, '/'));
            sb.Append("".PadLeft(200, '\\'));
            sb.Append("".PadLeft(200, '/'));

            Console.WriteLine(sb.ToString());
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey(true);
        }


        #region IObserver Members

        public void Notify(IParsedData[] data)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            var header = data[0];
            var fotter = data[1];

            var sb = new StringBuilder();
            sb.AppendLine("".PadLeft(160, ':'));
            sb.AppendLine("".PadLeft(160, ':'));
            sb.AppendLine("SUCESSO NA IMPORTAÇÃO: " + FilePath);
            sb.AppendLine("".PadLeft(160, ':'));
            sb.AppendLine("".PadLeft(160, ':'));

            using (var file = new StreamWriter(path + "Log.txt", true))
            {
                file.WriteLine(sb.ToString());
            }
        }

        public void Notify(List<IResult> data)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var sb = new StringBuilder();

            sb.AppendLine("".PadLeft(160, '*'));
            sb.AppendLine(FilePath);
            sb.AppendLine("".PadLeft(160, '*'));

            foreach (var s in data)
            {
                sb.Append(s.FullMessage);
            }

            sb.AppendLine("".PadLeft(160, '*'));
            sb.AppendLine("".PadLeft(160, '*'));

            Console.WriteLine(FilePath + " ".PadLeft(30, '.'));

            using (var file = new StreamWriter(path + "Log_" +DateTime.Now.Ticks+ ".txt", true))
            {
                file.WriteLine(sb.ToString());
            }
        }


        public void Notify(IParsedObjetct data)
        {
            Console.WriteLine(data.Name + " >> " + data.GetType().Name);
        }

        #endregion
    }
}