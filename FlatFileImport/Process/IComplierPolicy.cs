using FlatFileImport.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlatFileImport.Process
{
	public interface IComplierPolicy
	{
		bool IsValid { get; }
		IBlueprintLine Header { get; }
		IBlueprintLine Footer { get; }
	}
}
