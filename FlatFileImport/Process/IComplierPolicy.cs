using System.Collections.Generic;
using FlatFileImport.Core;
using FlatFileImport.Input;

namespace FlatFileImport.Process
{
	public interface IComplierPolicy
	{
		bool IsValid { get; }
		string HeaderIdentifier { get; }
		string FooterIdentifier { get; }
		void LookUp(IList<IRawLine> rawLines);
	}
}
