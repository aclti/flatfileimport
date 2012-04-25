﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatFileImport;
using FlatFileImport.Data;

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

        public void Notify(IParsedData data)
        {
            var headers = data.Headers.Where(h => h.Name == "D1000");

            var sb = new StringBuilder();

            sb.Append("".PadLeft(80, '*'));
            sb.AppendLine();

            foreach (var f in data.Fields)
            {
                sb.AppendFormat("{0} : {1} // ", f.Name, f.Value);
            }

            sb.AppendLine();
            sb.Append("".PadLeft(80, '*'));
            sb.AppendLine();

            foreach (var h in headers)
            {
                foreach(var f in h.Fields)
                {
                    sb.AppendFormat("{0} : {1}", f.Name, f.Value);
                    sb.AppendLine();
                }
                sb.AppendLine("".PadLeft(80, '-'));
                sb.AppendLine("".PadLeft(80, '-'));
            }

            
            Console.WriteLine(sb.ToString());
        }

        public void Notify(IParsedObjetct data)
        {
            throw new NotImplementedException();
        }

        public void Notify(string[] data)
        {
            if (_results == null)
                _results = new List<Result>();

            _results.Add(new Result { Num = data[0], Line = data[1] });
        }

        #endregion
    }
}
