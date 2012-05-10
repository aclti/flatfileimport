using Ionic.Zip;

namespace FlatFileImport.Input
{
    // TODO: Fazer com que o handler consiga trabalhar com zip que contenham mais de um arquivo ou diretorio
    public class HandlerZip : Handler
    {
        private string _dataFile;

        public HandlerZip(string path) : base(path)
        {
            // TODO Melhorar a estrutura de parent do file info, talvez trabalhar com parent no handler também
            var parentFileInfo = new FileInfo(path, SupportedExtension.GetFileExtension(path));
            var file = ExtractZip(path);
            // TODO Implementar para chamar novamente o handler;
            var fileInfo = new FileInfo(file, SupportedExtension.GetFileExtension(file), parentFileInfo);
            
            FileInfos.Add(fileInfo);
        }

        private string ExtractZip(string path)
        {
            using (var zip = ZipFile.Read(path))
            {
                if (zip.Entries.Count > 1)
                    throw new System.Exception("ARQUIVO ZIP INCOMPATIVEL (Varios arquivos no Zip) | " + path);

                foreach (var e in zip)
                {
                    if (e.IsDirectory)
                        throw new System.Exception("ARQUIVO ZIP INCOMPATIVEL (Zip com diretório) | " + path);

                    e.Extract(System.IO.Path.GetTempPath(), ExtractExistingFileAction.OverwriteSilently);
                    _dataFile = e.FileName;

                    return System.IO.Path.Combine(System.IO.Path.GetTempPath(), _dataFile);
                }

                zip.Dispose();
                return null;
            }
        }   
    }
}
