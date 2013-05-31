namespace FlatFileImport.Input
{
	public class HandlerProxy : IHandler
	{
		private readonly string _path;
		private readonly IHandlerFactory _factory;

		public HandlerProxy(string path, IHandlerFactory factory)
		{
			_path = path;
			_factory = factory;
		}

		#region IHandler Members

		public string Path
		{
			get { return _factory.Get(_path).Path; }
		}

		public IFileInfo FileInfo
		{
			get { return _factory.Get(_path).FileInfo; }
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_factory.Get(_path).Dispose();
		}

		#endregion
	}
}
