using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlatFileImport.Input
{
	public class HandlerDirectory : IHandlerCollection
    {
		public IList<string> Paths { get; private set; }
		public IList<IHandler> Handlers { get; private set; }

		private readonly IHandlerFactory _factory;

		public HandlerDirectory(string path)
			: this(path, new HandlerFacotry())
		{
			
		}
		
        public HandlerDirectory(string path, IHandlerFactory factory) 
        {
			Handlers = new List<IHandler>();
			Paths = new List<string>();

			_factory = factory;

			ProcessDir(path);
			ProcessFile();
        }

		private IHandlerFactory GetFactory()
		{
			if (_factory == null)
				return new HandlerFacotry();

			return _factory;
		}

        private void ProcessDir(string sourceDir)
        {
            var fileEntries = Directory.GetFiles(sourceDir);
            foreach (var fileName in fileEntries)
				Paths.Add(fileName);

            var subdirEntries = Directory.GetDirectories(sourceDir);

            foreach (var subdir in subdirEntries)
                if ((File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                    ProcessDir(subdir);
        }

        private void ProcessFile()
        {
	        var ignore = ((HandlerFacotry) _factory).IgnoreExtensions;

			foreach (var s in Paths)
			{
				if (ignore != null && ignore.Contains(Path.GetExtension(s)))
					continue;

				Handlers.Add(new HandlerProxy(s, GetFactory()));
			}
				
        }
	}
}
