using System;
using System.Text.RegularExpressions;

namespace FlatFileImport.Input
{
    public class FileExtension
    {
        private static readonly Regex Regex = new Regex(@"^\.[a-zA-Z0-9]{3,}$");

        public string Extension { private set; get; }
        public FileType Type { private set; get; }

        public FileExtension(string extension, FileType type)
        {
            if (extension == null)
                throw new ArgumentNullException("extension");

            if(type == FileType.Undefined)
                throw new ArgumentException("O type não pode ser Undefined");

            extension = NormalizeExtension(extension);

            if(!IsValid(extension))
                throw new System.Exception("A extensão informada não é válida");

            Extension = extension;
            Type = type;
        }

        public static bool IsValid(string extension)
        {
            return Regex.Match(extension).Success;
        }
        
        private string NormalizeExtension(string extension)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;

            return extension.ToLower();
        }
    }
}
