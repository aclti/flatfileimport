using FlatFileImport.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlatFileImport.Process
{
	public class DefaultCompilerPolicy : IComplierPolicy
	{
		private IBlueprint _blueprint;

		public DefaultCompilerPolicy(IBlueprint blueprint)
		{
			if (blueprint == null)
				throw new ArgumentNullException("blueprint", "Chame SetBlueprint");

			_blueprint = blueprint;
		}

		public bool IsValid { get { return true; } }

		public IBlueprintLine Header
		{
			get { return _blueprint.BlueprintLines.First(l => l.Parent == null); }
		}

		public IBlueprintLine Footer
		{
			get { return _blueprint.BlueprintLines.Last(l => l.Parent == null); }
		}
	}
}
