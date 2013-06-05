using System.Collections.Generic;
using FlatFileImport.Input;

namespace FlatFileImport.Process
{
	public interface ICompilerPolicy
	{
		bool IsValid { get; }
		string HeaderIdentifier { get; }
		string FooterIdentifier { get; }
		void OnChunkRead(IList<IRawLine> rawLines);
	}
}
