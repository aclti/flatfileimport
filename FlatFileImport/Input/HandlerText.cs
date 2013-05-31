namespace FlatFileImport.Input
{
    public class HandlerText : Handler
    {
	    private readonly IFileInfo _file;
		private readonly string _path;

        public HandlerText(string path, ISupportedExtension supportedExtension) : base(path, supportedExtension)
        {
	        _path = path;
			_file = new FileInfo(path, SupportedExtension.GetFileExtension(path));
        }

		public override IFileInfo FileInfo { get { return _file; } }

		public override string Path
		{
			get { return _path; }
		}

		public override void Dispose()
		{
			_file.Dispose();
		}
	}
}
