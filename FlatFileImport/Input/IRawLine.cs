using System.Collections.ObjectModel;

namespace FlatFileImport.Input
{
	public interface IRawLine
	{
		int Number { get; }
		string Value { get; }
	}

	public interface IRawLineAndFields : IRawLine
	{
		ReadOnlyCollection<IRawField> RawFields { get; }
		void AddRawFiled(string rawValue);
	}
}