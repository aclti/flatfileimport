using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace FlatFileImport.Input
{
    /// <summary>
    /// Classe responsável pela extensões de arquivos suportadas para importação
    /// Valores default: .txt, .web, ret, .zip
    /// </summary>
    public class SupportedExtension
    {
        private readonly List<FileExtension> _extension;
        public ReadOnlyCollection<FileExtension> Extension;

        public SupportedExtension()
        {
            _extension = new List<FileExtension>
                             {
                                 new FileExtension { Extension = ".txt", Type = FileType.Text}
                                 , new FileExtension { Extension = ".web", Type = FileType.Text}
                                 , new FileExtension { Extension = ".ret", Type = FileType.Text}
                                 , new FileExtension { Extension = ".zip", Type = FileType.Binary} 
                             };

            Extension = _extension.AsReadOnly();
        }

        public void AddExtension(string extension, FileType type)
        {
            extension = NormalizeExtension(extension);

            var newEx = new FileExtension {Extension = extension, Type = type};
            if(!ExtensionExist(newEx))
                _extension.Add(newEx);
        }

        public void AddExtensionFromXml(string path)
        {
            var confPath = path;

            var conf = new XmlDocument();
            conf.Load(confPath);

            var nodes = conf.SelectNodes("//Configuration/Extension");
            
            if(nodes == null || nodes.Count == 0)
                throw new NullReferenceException("Não foi possivel carregar o xml de configuração de extensões. Verifica a formatação");

            foreach (XmlNode node in nodes)
            {
                if(node.Attributes == null || node.Attributes.Count == 0)
                    throw new NullReferenceException("Erro na configuração dos atributos no xml de configuração de extensões");

                try
                {
                    var aux = new FileExtension
                              {
                                  Extension = NormalizeExtension(node.InnerText),
                                  Type = (FileType)Enum.Parse(typeof(FileType), node.Attributes["type"].Value)
                              };

                    if (!ExtensionExist(aux))
                        _extension.Add(aux);
                }
                catch (ArgumentNullException arg)
                {
                    throw new ArgumentException("Erro no Xml de configuração das extensões. Verifica a sintax do valor atribuido ao atributo type", arg);
                }
            }
        }

        public bool IsSupported(string extension)
        {
            return _extension.Any(e => e.Extension == extension.ToLower());
        }

        private bool ExtensionExist(FileExtension ex)
        {
            return _extension.Any(e => e.Extension == ex.Extension && e.Type == ex.Type);
        }

        private string NormalizeExtension(string extension)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;

            return extension.ToLower();
        }

        //private List<string> GetSupportedExtesion(FileType type)
        //{
        //    var l = new List<string>();
        //    var confPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config-input.xml");

        //    var conf = new XmlDocument();
        //    conf.Load(confPath);

        //    var nodes = conf.SelectNodes("//Configuration/Extension[@type='" + type.ToString().ToLower() + "']");

        //    if (nodes != null)
        //        l.AddRange(from XmlNode n in nodes select n.InnerText);

        //    return l;
        //}
    }
}