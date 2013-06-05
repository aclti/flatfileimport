using FlatFileImport.Core;
using FlatFileImport.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatFileImport.Process
{
	public class Complier
	{
		private readonly IList<IRawLine> _lines;
		private readonly IComplierPolicy _compilerPolicy;
		private readonly IBlueprint _bluprint;

		public Complier(IComplierPolicy compilerPolicy, IBlueprint bluprint)
		{
			_lines = new List<IRawLine>();
			_compilerPolicy = compilerPolicy;
			_bluprint = bluprint;
		}

		public bool IsVald
		{
			get
			{
				_compilerPolicy.LookUp(_lines);
				return _compilerPolicy.IsValid;
			}
		}

		public IFileInfo GetDataToImport()
		{
			return new FileInfoChunk(_lines);
		}

		public void AddRawData(IRawLine line)
		{
			_lines.Add(line);
		}

		public bool IsHead
		{
			get
			{
				var head = _bluprint.BlueprintLines.First(l => l.Name == _compilerPolicy.HeaderIdentifier);

				if (head == null)
					throw new NullReferenceException("O Identificador da blueprint não foi encontrado");

				return head.Regex.IsMatch(_lines.Last().Value);
			}
		}

		public void Feed(IFileInfo file)
		{
			var footer = _bluprint.BlueprintLines.First(l => l.Name == _compilerPolicy.FooterIdentifier);

			if (footer == null)
				throw new NullReferenceException("O Identificador da blueprint não foi encontrado");

			while (file.MoveToNext())
			{
				AddRawData(new SimpleRawLine(file.Line, file.LineNumber));

				if (footer.Regex.IsMatch(_lines.Last().Value))
					break;
			}
		}
	}
}
