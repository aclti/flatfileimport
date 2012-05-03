using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FlatFileImport.Aggregate;
using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Exception;
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
        private IFileInfo _file;
        private List<IParsedData> _headers;
        private List<IParsedObjetct> _details;
        private List<IResult> _results;

        public IEventLog Loger { set { _loger = value; } get { return _loger ?? new DefaultEventLog(); } }


        private List<IBlueprintLine> _stack;
        private IBlueprintLine _active;

        public Importer()
        {
            _parsedDatas = new IParsedData[2];
            _headers = new List<IParsedData>();
            _details = new List<IParsedObjetct>();
            _results = new List<IResult>();
            _stack = new List<IBlueprintLine>();
        }

        public ReadOnlyCollection<IResult> Results { get { return _results.AsReadOnly(); } }
        public bool IsValid { get { return _results.Count == 0 || _results.Count(r => r.Type == ExceptionType.Error) == 0; } }
        

        public void Valid()
        {
            CheckFileAndBluprint();
            _file.Restart();
            _parser = SetParser();

            while (_file.MoveToNext())
            {
                var newLine = MatchBlueprintLine(_file.Line);
                
                if (newLine == null)
                {
                    _results.Add(new Result(_file.LineNumber, "A linha importada não está definida na Blueprint. Consulte a documentação de importação do arquivo e verifique a definição da Blueprint", ExceptionType.Error, ExceptionSeverity.Critical)
                                     {
                                         Value = _file.Line
                                     });
                    continue;
                }

                _parser.SetBlueprintLine(newLine);
                _parser.SetDataToParse(new RawLine(_file.LineNumber, _file.Line));

                if (newLine.Aggregate != null && newLine.Aggregate is Count)
                    newLine.Aggregate.AddOperand(1);

                if (IsRoot(newLine))
                  {
                    _active = newLine;
                    continue;
                }

                if (!_parser.IsValid && _parser.Result.Any(r => r.Severity != ExceptionSeverity.Information && r.Type != ExceptionType.Warnning))
                    _results.AddRange(_parser.Result);

                var flag = true;
                while (flag)
                {
                    if(_active == null)
                    {
                        _results.Add(new Result(_file.LineNumber, "Hieraquia de registro não está de acordo com a Blueprint.", ExceptionType.Error, ExceptionSeverity.Fatal) { Value = _file.Line });
                        break;
                    }

                    if (IsSibiling(newLine))
                    {
                        _active = newLine;
                        flag = false;
                    }

                    if (IsParent(newLine))
                    {
                        StackUp(newLine);
                        flag = false;
                    }

                    if (!flag) 
                        continue;

                    var c = _blueprint.BlueprintLines.Where(b => b.Parent == _active.Parent).ToList();

                    foreach (var bline in c)
                        if(!CheckOccurrence(bline))
                            _results.Add(new Result(_file.LineNumber, "Quantidade errada de regsitros.", ExceptionType.Error, ExceptionSeverity.Fatal)
                                             {
                                                 LineName = bline.Name,
                                                 Expected = bline.Occurrence.Type.ToString()
                                             });

                    Unstacking();
                    // _results.Add(new Result("", "", _file.Line, String.Format("Linha: {0} | Quantidade errada de regsitros.", _file.LineNumber), ExceptionType.Error, ExceptionSeverity.Fatal));
                }
            }

            if (_results.Count > 0)
                NotifyObservers(_results);
        }


        public void Process()
        {
            //CheckFileAndBluprint();
            //_file.Restart();
            //_parser = SetParser();

            //while (_file.MoveToNext())
            //{
            //    var bline = MatchBlueprintLine(_file.Line);

            //    if (bline == null)
            //        continue;

            //    SetAggergates();
            //    _parser.SetBlueprintLine(bline);
            //    _parser.SetDataToParse(new RawLine(_file.LineNumber, _file.Line));

            //    IParsedData parent = null;
            //    if (bline.Parent != null)
            //        parent = _headers.FindLast(p => p.Name == bline.Parent.Name);

            //    if (!_parser.IsValid)
            //    {
            //        NotifyObservers(_parser.Result.ToList());
            //    }

            //    if (bline is BlueprintLineHeader || bline is BlueprintLineFooter)
            //    {
            //        var pdata = (IParsedData)_parser.GetParsedData(parent);

            //        if (parent != null)
            //            parent.AddParsedData(pdata);

            //        _headers.Add(pdata);
            //    }

            //    if (bline is BlueprintLineDetails && parent != null)
            //    {
            //        var pdata = _parser.GetParsedLine(parent);
            //        parent.AddLine(pdata);
            //        _details.Add(pdata);
            //    }

            //    var aggregate = GetAggregate(bline);
            //    if (aggregate != null)
            //        aggregate.AddOperand(1);

            //}





















            //_parsedDatas[0] = _headers.Find(h => h.Parent == null && h.Name == GetRootHeader().Name);
            //_parsedDatas[1] = _headers.Find(h => h.Parent == null && h.Name == GetRootFooter().Name);
            //var px = _headers.Count(h => h.Name == "D1000");
            //NotifyObservers(_parsedDatas);
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

        private void StackUp(IBlueprintLine newLine)
        {
            _stack.Add(_active);
            _active = newLine;
        }

        private void Unstacking()
        {
            _active = _stack.LastOrDefault();
            _stack.Remove(_active);
        }

        private bool IsParent(IBlueprintLine newLine)
        {
            if (_active == newLine.Parent)
                return true;

            return false;
        }

        private bool IsSibiling(IBlueprintLine newLine)
        {
            if (newLine.Parent == _active.Parent)
                return true;

            return false;
        }

        private bool IsRoot(IBlueprintLine newLine)
        {
            if (newLine.Parent == null && _active == null && _stack.Count == 0)
                return true;

            return false;
        }

        private bool CheckOccurrence(IBlueprintLine line)
        {
            var ocuurence = line.Occurrence;
            var count = line.Aggregate != null ? line.Aggregate.Result : 0;

            if (ocuurence.Type == EnumOccurrence.One && count != 1)
                return false;

            if (ocuurence.Type == EnumOccurrence.AtLeastOne && count < 1)
                return false;

            if (ocuurence.Type == EnumOccurrence.Range && (count < ocuurence.Min || count > ocuurence.Max))
                return false;

            return true;
        }

        private void CheckFileAndBluprint()
        {
            if (_blueprint == null)
                throw new System.Exception("A Blueprint não foi definida.");

            if (_file == null)
                throw new System.Exception("Não foi definido um arquivo para ser processado.");
        }

        private void SetAggergates()
        {
            _aggregates = new List<IAggregate>();

            foreach (var bline in _blueprint.BlueprintLines)
                if (bline.Aggregate != null)
                    _aggregates.Add(bline.Aggregate);

        }

        private IAggregate GetAggregate(IBlueprintLine bline)
        {
            return _aggregates.FirstOrDefault(ag => ag.Subject.Name == bline.Name);
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

        private IParser SetParser()
        {
            if (_blueprint.FieldSeparationType == EnumFieldSeparationType.Character)
                return new ParserSeparatedCharacter();

            if (_blueprint.FieldSeparationType == EnumFieldSeparationType.Position)
                return new ParserPositional();

            throw new System.Exception("Parser não encontrado...........");
        }

        private IBlueprintLine MatchBlueprintLine(string rawLine)
        {
            return _blueprint.BlueprintLines.FirstOrDefault(bl => bl.Regex.IsMatch(rawLine));
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

        public void NotifyObservers(IParsedData[] data)
        {
            if (_observers != null && _observers.Count > 0)
                _observers.ForEach(o => o.Notify(data));
        }

        public void NotifyObservers(string[] data)
        {
            if (_observers != null && _observers.Count > 0)
                _observers.ForEach(o => o.Notify(data));
        }

        public void NotifyObservers(List<IResult> results)
        {
            if (_observers != null && _observers.Count > 0)
                _observers.ForEach(o => o.Notify(results));
        }
        #endregion
    }
}