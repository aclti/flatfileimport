using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlatFileImport.Input
{
	public class SimpleRawLine : IRawLine
	{
		public SimpleRawLine(string rawDataLine, int lineNumber)
		{
			Number = lineNumber;
			Value = rawDataLine;
		}

		public int Number { get; private set; }
		public string Value { get; private set; }
	}
}
