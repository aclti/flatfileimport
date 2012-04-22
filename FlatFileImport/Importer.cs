using System;
using System.Collections.Generic;
using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Log;
using FlatFileImport.Process;
using FlatFileImport.Validate;
using FileInfo = FlatFileImport.Input.FileInfo;

namespace FlatFileImport
{
    public class Importer : ISubject
    {
        private List<IObserver> _observers;
        private IParser _parser;
        private IBlueprintFactoy _blueprintFactoy;
        private IBlueprint _blueprint;
        private ParsedData _parsedData;
        private IEventLog _loger;
        private IValidate _validate;

        public IEventLog Loger { set { _loger = value; } get { return _loger ?? new DefaultEventLog(); } }
        public IBlueprintFactoy BlueprintFactoy
        {
            set
            {
                if (value == null)
                    throw new System.Exception("A BlueprintFactory não pode ser null.");

                _blueprintFactoy = value;
            }

            get
            {
                if(_blueprintFactoy ==  null)
                    throw new System.Exception("A BlueprintFactory não foi definida.");

                return _blueprintFactoy;
            }
        }

        public Importer()
        {
            _parsedData = new ParsedData();
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

        private IBlueprintLine GetBlueprintLine(string rawLine)
        {
            throw new NotImplementedException();
        }

        private ParsedData GetParsedData(string rawLine)
        {
            throw new NotImplementedException();
        }

        private ParsedData GetParsedData(string rawLine, ParsedData parent)
        {
            throw new NotImplementedException();
        }

        private ParsedLine GetParsedLine(string rawLine, ParsedData parent)
        {
            throw new NotImplementedException();
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

        public void NotifyObservers(ParsedData parsedData)
        {
            if (_observers != null)
                foreach (var o in _observers)
                    o.Notify(parsedData);
        }

        #endregion
    }
}