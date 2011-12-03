using Ionic.Zip;

namespace FlatFileImport.Input
{
    public class HandlerZip : Handler
    {
        private string _dataFile;

        public HandlerZip(string path) : base(path)
        {
            var fileInfo = new FileInfo(ExtractZip(path)) {Comment = path};
            FileInfos.Add(fileInfo);
        }

        private string ExtractZip(string path)
        {
            ZipFile zip = ZipFile.Read(path);

            if (zip.Entries.Count > 1)
                throw new System.Exception("ARQUIVO ZIP INCOMPATIVEL");

            foreach (ZipEntry e in zip)
            {
                if (e.IsDirectory)
                    throw new System.Exception("ARQUIVO ZIP INCOMPATIVEL");

                e.Extract(System.IO.Path.GetTempPath(), ExtractExistingFileAction.OverwriteSilently);
                _dataFile = e.FileName;

                return System.IO.Path.Combine(System.IO.Path.GetTempPath(), _dataFile);
            }

            return null;
        }   
    }
}
