using System;
using System.Collections.Generic;
using System.Linq;
using FlatFileImport.Aggregate;
using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Input;
using FlatFileImport.Log;
using FlatFileImport.Process;
using FlatFileImport.Validate;

namespace FlatFileImport
{
    public class Importer : ISubject
    {
        private List<IObserver> _observers;
        private List<IAggregate> _aggregates;
        private IParser _parser;
        private IBlueprint _blueprint;
        private IParsedData[] _parsedDatas;
        private IEventLog _loger;
        private IValidate _validate;
        private IFileInfo _file;
        private List<IParsedData> _headers;
        private List<IParsedObjetct> _details;

        public IEventLog Loger { set { _loger = value; } get { return _loger ?? new DefaultEventLog(); } }


        public Importer()
        {
            _parsedDatas = new ParsedData[2];
            _headers = new List<IParsedData>();
            _details = new List<IParsedObjetct>();
        }

        public void Process()
        {
            if (_blueprint == null)
                throw new System.Exception("A Blueprint não foi definida.");

            if (_file == null)
                throw new System.Exception("Não foi definido um arquivo para ser processado.");

            //var result = new[] { _file.LineNumber.ToString(""), _file.Header };
            //NotifyObservers(result);
            _parser = SetParser();
            
            var bline = GetBlueprintLine(_file.Header);

            _parser.SetBlueprintLine(bline);
            _parser.SetDataToParse(_file.Header);

            if (!_parser.IsValid)
                throw new System.Exception("O cabeçalho do arquivo não está correto: " + _file.Path);

            _headers.Add((IParsedData)_parser.GetParsedData(null));

            

            while (_file.MoveToNext())
            {
                bline = GetBlueprintLine(_file.Line);

                if(bline == null)
                    continue;

                var parent = _headers.FindLast(p => bline.Parent != null && p.Name == bline.Parent.Name);

                _parser.SetBlueprintLine(bline);
                _parser.SetDataToParse(_file.Line);

                if(!_parser.IsValid)
                    throw new System.Exception("Dados corrompidos............");

                if (bline is BlueprintLineHeader)
                {
                    var pdata = (IParsedData) _parser.GetParsedData(parent);
                    parent.AddParsedData(pdata);

                    _headers.Add(pdata);
                }
                    

                //if (bline is BlueprintLineFooter)
                //{
                //    if (bline.Parent == null)
                //        _parsedDatas[0] = new ParsedData(bline.Name, null);
                //}

                if (bline is BlueprintLineDetails)
                {
                    var pdata = _parser.GetParsedLine(parent);
                    parent.AddLine(pdata);
                    _details.Add(pdata);
                }
                    

                //result = new[] {_file.LineNumber.ToString(""), _file.Line};
                //NotifyObservers(result);
            }

            NotifyObservers(_headers.First());

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

        public void SetBlueprint(IBlueprint blueprint)
        {
            if (blueprint == null)
                throw new ArgumentNullException("blueprint");

            _blueprint = blueprint;
        }

        public void SetFileToProcess(IFileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            _file = file;
        }

        private IParsedData GetParent(IParsedData node, IParsedObjetct blueprint)
        {
            return _parsedDatas[0].Headers.Last(h => h.Name == blueprint.Name);
        }

        private IParser SetParser()
        {
            if (_blueprint.FieldSeparationType == EnumFieldSeparationType.Character)
                return new ParserSeparatedCharacter();

            if (_blueprint.FieldSeparationType == EnumFieldSeparationType.Position)
                return new ParserPositional();

            throw new System.Exception("Parser não encontrado...........");
        }

        private IBlueprintLine GetBlueprintLine(string rawLine)
        {
            return _blueprint.BlueprintLines.FirstOrDefault(bl => bl.Regex.IsMatch(rawLine));
        }

        private ParsedData GetParsedData(string rawLine, ParsedData parent)
        {
            throw new NotImplementedException();
        }

        private IParsedObjetct GetParsedLine(string rawLine, ParsedData parent)
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

        public void NotifyObservers(IParsedData data)
        {
            if (_observers != null && _observers.Count > 0)
                _observers.ForEach(o => o.Notify(data));
        }

        public void NotifyObservers(IParsedObjetct data)
        {
            if (_observers != null && _observers.Count > 0)
                _observers.ForEach(o => o.Notify(data));
        }

        public void NotifyObservers(string[] data)
        {
            if (_observers != null && _observers.Count > 0)
                _observers.ForEach(o => o.Notify(data));
        }

        #endregion
    }
}