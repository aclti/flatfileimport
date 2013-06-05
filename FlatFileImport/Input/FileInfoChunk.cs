using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlatFileImport.Input
{
	public class FileInfoChunk : IFileInfo
	{
		private IList<IRawLine> _lines;
		private int _lineIndex;
		private const int INITIAL_INDEX_VALUE = -1;

		public FileInfoChunk(IList<IRawLine> lines)
		{
			_lines = lines;
			_lineIndex = INITIAL_INDEX_VALUE;
		}

		[Obsolete("Use o Handler para obter essas informações")]
		public string Name { get; private set; }
		[Obsolete("Use o Handler para obter essas informações")]
		public string Path { get; private set; }
		[Obsolete("Use o Handler para obter essas informações")]
		public string Directory { get; private set; }

		public string Line { get { return _lines[_lineIndex].Value; } }
		public int LineNumber { get { return _lines[_lineIndex].Number; } }
		public FileExtension Extesion { get; private set; }
		public string Header { get { return _lines.First().Value; } }

		public bool MoveToNext()
		{
			_lineIndex++;

			return _lineIndex < _lines.Count;
		}

		public void Reset()
		{
			_lineIndex = INITIAL_INDEX_VALUE;
		}

		[Obsolete("Use Dispose")]
		public void Release()
		{
			Dispose();
		}

		public void Dispose()
		{
			_lineIndex = INITIAL_INDEX_VALUE;
			_lines = null;
		}
	}
}
