using System;
using System.Collections.Generic;
using FlatFileImport.Process;
using FileInfo = FlatFileImport.Input.FileInfo;

namespace FlatFileImport
{
    public class Importer : ISubject
    {
        private List<IObserver> _observers;

        private IParser _parser;
        private Blueprint _blueprint;
        private List<ParsedData> _parsedDatas;

        public IBlueprintFactoy BlueprintFactoy { set; get; }
        
        public Importer(IBlueprintFactoy factoy)
        {
            BlueprintFactoy = factoy;
        }

        public void Process(FileInfo fileInfo)
        {
            //Parser = new Parser(BlueprintFactoy.GetBlueprint(Type, fileInfo), fileInfo, observer);
            //Parser.ProcessHeader();

            //Console.WriteLine("".PadLeft(80, '*'));
            //Console.WriteLine(String.Format("INICIO DO PROCESSAMENTO: ARQUIVO [{0}]", fileInfo.Path));
            //Console.WriteLine("".PadLeft(80, '*'));

            //Parser.Process();

            //Parser.UnRegisterObserver(observer);

            //Console.WriteLine("".PadLeft(80, '*'));
            //Console.WriteLine(String.Format("FINAL DO PROCESSAMENTO: ARQUIVO [{0}]", fileInfo.Path));
            //Console.WriteLine("".PadLeft(80, '*'));
        }

        #region ISubject Members

        public void RegisterObserver(IObserver observer)
        {
            if (_observers == null)
                _observers = new List<IObserver>();

            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void UnRegisterObserver(IObserver observer)
        {
            if (_observers != null && _observers.Contains(observer))
                _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            if (_observers != null)
                foreach (var o in _observers)
                    o.Notify(_parsedDatas);
        }

        #endregion
    }
}