using FlatFileImport.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatFileImport.Process
{
	public class DefaultCompilerPolicy : ICompilerPolicy
	{
		private readonly IBlueprint _blueprint;

		public DefaultCompilerPolicy(IBlueprint blueprint)
		{
			if (blueprint == null)
				throw new ArgumentNullException("blueprint", "Chame SetBlueprint");

			_blueprint = blueprint;
		}

		public bool IsValid { get { return true; } }

		public string HeaderIdentifier
		{
			get { return _blueprint.BlueprintLines.First(l => l.Parent == null).Name; }
		}

		public string FooterIdentifier
		{
			get { return _blueprint.BlueprintLines.Last(l => l.Parent == null).Name; }
		}

		public void OnChunkRead(IList<Input.IRawLine> rawLines)
		{
			
		}	
	}
}
