using System.Collections.Generic;
using System.IO;

namespace FlatFileImport.Input
{
    public class HandlerDirectory : Handler
    {
        private readonly List<string> _paths;

        public HandlerDirectory(string path) : base(path)
        {
            _paths = new List<string>();
            ProcessDir(path);
            ProcessFile();
        }

        private void ProcessDir(string sourceDir)
        {
            string[] fileEntries = Directory.GetFiles(sourceDir);
            foreach (string fileName in fileEntries)
                _paths.Add(fileName);

            string[] subdirEntries = Directory.GetDirectories(sourceDir);

            foreach (string subdir in subdirEntries)
                if ((File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                    ProcessDir(subdir);
        }

        private void ProcessFile()
        {
            foreach (var s in _paths)
            {
                IEnumerable<FileInfo> h = GetHandler(s);

                foreach (var fileInfo in h)
                    FileInfos.Add(fileInfo);
            }
        }
    }
}
