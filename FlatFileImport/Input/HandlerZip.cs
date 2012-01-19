using Ionic.Zip;

namespace FlatFileImport.Input
{
    // TODO: Fazer com que o handler consiga trabalhar com zip que contenham mais de um arquivo ou diretorio
    // TODO: Tratar de uma forma melhor o armazanamento do nome original do zip, talvez encadear os handlers Handler -> ParentHandler. Atualmente essa informação fica armazenada no campo comments
    public class HandlerZip : Handler
    {
        private string _dataFile;

        public HandlerZip(string path) : base(path)
        {
            var fileInfo = new FileInfo(ExtractZip(path), SupportedExtension.GetFileExtension(path)) { Comment = path };
            FileInfos.Add(fileInfo);
        }

        private string ExtractZip(string path)
        {
            var zip = ZipFile.Read(path);

            if (zip.Entries.Count > 1)
                throw new System.Exception("ARQUIVO ZIP INCOMPATIVEL");

            foreach (var e in zip)
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
