using System;
using System.Collections.Generic;

namespace FlatFileImport.Input
{
	public interface IHandler : IDisposable
	{
		string Path { get; }
		IFileInfo FileInfo { get; }
	}

	public interface IHandlerCollection
	{
		IList<IHandler> Handlers { get; }
		IList<string> Paths { get; }
	}
}
