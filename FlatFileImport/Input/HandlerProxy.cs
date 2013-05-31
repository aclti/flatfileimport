namespace FlatFileImport.Input
{
	public class HandlerProxy : IHandler
	{
		private readonly string _path;
		private readonly IHandlerFactory _factory;
		private IHandler _handler;

		public HandlerProxy(string path, IHandlerFactory factory)
		{
			_path = path;
			_factory = factory;
		}

		private IHandler GetHandler()
		{
			return _handler ?? (_handler = _factory.Get(_path));
		}

		#region IHandler Members

		public string Path
		{
			get { return GetHandler().Path; }
		}

		public IFileInfo FileInfo
		{
			get { return GetHandler().FileInfo; }
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			GetHandler().Dispose();
		}

		#endregion
	}
}
