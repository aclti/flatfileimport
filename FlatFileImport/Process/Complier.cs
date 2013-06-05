using FlatFileImport.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlatFileImport.Process
{
	public class Complier
	{
		private IList<IRawLine> _lines;
		private IComplierPolicy _compilerPolicy;


		public Complier(IComplierPolicy compilerPolicy)
		{
			_lines = new List<IRawLine>();
			_compilerPolicy = compilerPolicy;
		}

		public bool IsVald { get { return _compilerPolicy.IsValid; } }

		public IFileInfo GetDataToImport()
		{
			return new FileInfoChunk(_lines);
		}

		public bool GetNextRawLine()
		{
			throw new NotImplementedException();
		}

		public void AddRawData(IRawLine line)
		{
			_lines.Add(line);
		}

		public bool IsHead { get { return _compilerPolicy.Header.Regex.IsMatch(_lines.Last().Value); } }

		public void Feed(IFileInfo file)
		{
			while (file.MoveToNext())
			{
				AddRawData(new SimpleRawLine(file.Line, file.LineNumber));

				if (_compilerPolicy.Footer.Regex.IsMatch(_lines.Last().Value))
					break;
			}
		}
	}
}
