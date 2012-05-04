using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatFileImport;
using FlatFileImport.Data;
using FlatFileImport.Validate;

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
            var header = data[0];
            var fotter = data[1];

            Console.WriteLine("Header e footer recebidos");
        }

        public void Notify(List<IResult> data)
        {
            var sb = new StringBuilder();

            foreach (var s in data)
            {
                sb.AppendLine();
                sb.Append(s.FullMessage);
            }

            Console.WriteLine(sb.ToString());
        }

        #endregion
    }
}