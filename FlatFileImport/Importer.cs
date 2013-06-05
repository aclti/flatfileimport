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
		//private List<IAggregate> _aggregates;
		private IParser _parser;
		private IBlueprint _blueprint;
		private IParsedData[] _parsedDatas;
		private IEventLog _loger;
		private IFileInfo _file;
		private List<IParsedData> _headers;
		private List<IParsedObjetct> _details;
		private List<IResult> _results;
		private List<IBlueprintLine> _stack;
		private IBlueprintLine _active;
		private IParsePolicy _parserPolicy;
		private ICompilerPolicy _compilerPolicy;

		public ReadOnlyCollection<IResult> Results { get { return _results.AsReadOnly(); } }
		public bool IsValid { get { return _results.Count == 0 || _results.Count(r => r.Type == ExceptionType.Error) == 0; } }
		public IEventLog Loger { set { _loger = value; } get { return _loger ?? new DefaultEventLog(); } }
		public bool IgnoreOpcionalRecordWithError { set; get; }
		public bool NotifyLine { set; get; }
		public IParsePolicy ParsePolicy { set { _parserPolicy = value; } get { return _parserPolicy ?? (_parserPolicy = new DefaultParsePolicy()); } }

		public ICompilerPolicy CompilerPolicy { set { _compilerPolicy = value; } get { return _compilerPolicy ?? (_compilerPolicy = new DefaultCompilerPolicy(_blueprint)); } }

		public Importer()
		{
			_parsedDatas = new IParsedData[2];
			_headers = new List<IParsedData>();
			_details = new List<IParsedObjetct>();
			_results = new List<IResult>();
			_stack = new List<IBlueprintLine>();
		}

		public void Reset()
		{
			_parsedDatas = new IParsedData[2];
			_headers = new List<IParsedData>();
			_details = new List<IParsedObjetct>();
			_results = new List<IResult>();
			_stack = new List<IBlueprintLine>();
		}

		public void Valid()
		{
			CheckFileAndBluprint();
			_file.Reset();
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

				if (_results.Count > 500)
					throw new System.Exception("Arquivo com muito erros.");

				_parser.SetBlueprintLine(newLine);
				_parser.SetDataToParse(new RawLine(_file.LineNumber, _file.Line));

				if (newLine.Aggregate != null && newLine.Aggregate is Count)
					newLine.Aggregate.AddOperand(1);

				if (IsRoot(newLine))
				{
					_active = newLine;
					continue;
				}

				if (!IsRoot(newLine) && _active == null)
					break;

				if (!_parser.IsValid && IsMandatory(newLine.Occurrence) && _parser.Result.Any(r => r.Severity != ExceptionSeverity.Information && r.Type != ExceptionType.Warnning))
					_results.AddRange(_parser.Result);

				var flag = true;
				while (flag)
				{
					if (_active == null)
					{
						_results.Add(new Result(_file.LineNumber, "Hieraquia de registro não está de acordo com a Blueprint.", ExceptionType.Error, ExceptionSeverity.Fatal) { Value = _file.Line });
						break;
					}

					if (IsSibling(newLine))
					{
						_active = newLine;
						flag = false;
					}

					if (IsParent(newLine))
					{
						Push(newLine);
						flag = false;
					}

					if (!flag)
						continue;

					var c = _blueprint.BlueprintLines.Where(b => b.Parent == _active.Parent).ToList();

					foreach (var bline in c)
						if (!CheckOccurrence(bline))
							_results.Add(new Result(_file.LineNumber, "A quantidade de registro não está de acordo com o espcificado na Blueprint. Verifique a documentação de importação do arquivo e a definição da Blueprint.", ExceptionType.Error, ExceptionSeverity.Fatal)
											 {
												 Value = bline.Aggregate.Cache.ToString(""),
												 LineName = bline.Name,
												 Expected = GetValuesOccorence(bline.Occurrence)
											 });

					Pop();
				}
			}

			if (_results.Count > 0)
				NotifyObservers(_results);
		}

		private bool IsMandatory(IOccurrence occurrence)
		{
			return occurrence.Type != EnumOccurrence.NoOrMany || occurrence.Type != EnumOccurrence.NoOrOne;
		}

		private string GetValuesOccorence(IOccurrence occurrence)
		{
			if (occurrence.Type == EnumOccurrence.One)
				return "Um e apenas um.";

			if (occurrence.Type == EnumOccurrence.AtLeastOne)
				return "No mínimo um.";

			return String.Format("No mínimo {0} e no máximo {1}", occurrence.Min, occurrence.Max);
		}

		private void RealProcess(IFileInfo file)
		{
			CheckFileAndBluprint();
			file.Reset();
			_parser = SetParser();

			while (file.MoveToNext())
			{
				var bline = MatchBlueprintLine(file.Line);

				if (ParsePolicy.IgnoreLine(bline))
					continue;

				_parser.SetBlueprintLine(bline);
				_parser.SetDataToParse(new RawLine(file.LineNumber, file.Line));

				if (!_parser.IsValid && IsMandatory(bline.Occurrence) && _parser.Result.Any(r => r.Severity != ExceptionSeverity.Information && r.Type != ExceptionType.Warnning))
					continue;

				IParsedData parent = null;
				if (bline.Parent != null && !NotifyLine)
					parent = _headers.FindLast(p => p.Name == bline.Parent.Name);

				if (bline is BlueprintLineHeader || bline is BlueprintLineFooter)
				{
					var pdata = _parser.GetParsedData(parent);

					if (ParsePolicy.IgnoreData(pdata))
						continue;

					if (NotifyLine)
					{
						NotifyObservers(pdata);
						continue;
					}

					if (parent == null && bline is BlueprintLineHeader)
						_parsedDatas[0] = (IParsedData)pdata;

					if (parent == null && bline is BlueprintLineFooter)
						_parsedDatas[1] = (IParsedData)pdata;

					if (parent != null)
						parent.AddParsedData((IParsedData)pdata);

					_headers.Add((IParsedData)pdata);
				}

				if (bline is BlueprintLineDetails)
				{
					var pdata = _parser.GetParsedLine(parent);

					if (ParsePolicy.IgnoreData(pdata))
						continue;

					if (NotifyLine)
					{
						NotifyObservers(pdata);
						continue;
					}

					parent.AddLine(pdata);
					_details.Add(pdata);
				}
			}

			if (!NotifyLine)
				NotifyObservers(_parsedDatas);
		}

		public void Process()
		{
			CheckFileAndBluprint();

			_file.Reset();
			//_parser = SetParser();

			while (_file.MoveToNext())
			{
				var compiler = new Compiler(CompilerPolicy, _blueprint);

				compiler.AddRawData(new SimpleRawLine(_file.Line, _file.LineNumber));

				if (compiler.IsHead)
					compiler.Feed(_file);

				if (compiler.IsValid)
					RealProcess(compiler.GetDataToImport());
			}
		}

		private void Push(IBlueprintLine newLine)
		{
			_stack.Add(_active);
			_active = newLine;
		}

		private void Pop()
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

		private bool IsSibling(IBlueprintLine newLine)
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

		public void NotifyObservers(IParsedData[] data)
		{
			if (_observers != null && _observers.Count > 0)
				_observers.ForEach(o => o.Notify(data));
		}

		public void NotifyObservers(List<IResult> results)
		{
			if (_observers != null && _observers.Count > 0)
				_observers.ForEach(o => o.Notify(results));
		}

		public void NotifyObservers(IParsedObjetct data)
		{
			if (_observers != null && _observers.Count > 0)
				_observers.ForEach(o => o.Notify(data));
		}

		#endregion
	}
}