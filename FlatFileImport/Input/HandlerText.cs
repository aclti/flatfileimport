namespace FlatFileImport.Input
{
    public class HandlerText : Handler
    {
        public HandlerText(string path) : base(path)
        {
            var fileInfo = new FileInfo(path, SupportedExtension.GetFileExtension(path));
            FileInfos.Add(fileInfo);
        }
    }
}
