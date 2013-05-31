namespace FlatFileImport.Input
{
    public class HandlerDummy : Handler
    {
		public HandlerDummy(string path, ISupportedExtension supportedExtension) : base(path, supportedExtension) { }

		public override IFileInfo FileInfo
		{
			get { throw new System.NotImplementedException(); }
		}

		public override string Path
		{
			get { throw new System.NotImplementedException(); }
		}

		public override void Dispose()
		{
			throw new System.NotImplementedException();
		}
	}
}
