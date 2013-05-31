using System.Collections.Generic;
using System.IO;

namespace FlatFileImport.Input
{
	public class HandlerDirectory : IHandlerCollection
    {
		public IList<string> Paths { get; private set; }
		public IList<IHandler> Handlers { get; private set; }

		private readonly IHandlerFactory _factory;

		public HandlerDirectory(string path)
		{
			Handlers = new List<IHandler>();
			Paths = new List<string>();
			
			ProcessDir(path);
			ProcessFile();
		}
		
        public HandlerDirectory(string path, IHandlerFactory factory) : this(path)
        {
			_factory = factory;
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
			foreach (var s in Paths)
				Handlers.Add(new HandlerProxy(s, GetFactory()));
        }
	}
}
