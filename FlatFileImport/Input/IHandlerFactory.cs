using System.Collections.Generic;

namespace FlatFileImport.Input
{
	public interface IHandlerFactory
	{	
		IHandler Get(string path);
	}
}
