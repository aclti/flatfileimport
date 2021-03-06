﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;

namespace FlatFileImport.Input
{
	public interface ISupportedExtension
	{
		void AddExtension(string extension, FileType type);
		void AddExtensionFromXml(string path);
		FileExtension GetFileExtension(string path);
		bool IsSupported(string extension, FileType type);
		bool IsSupported(FileExtension extension);
		ReadOnlyCollection<FileExtension> Extensions { get; }
	}

	/// <summary>
    /// Classe responsável pela extensões de arquivos suportadas para importação
    /// Valores default: .txt, .web, ret, .zip
    /// </summary>
    public class SupportedExtension : ISupportedExtension
	{
        private readonly List<FileExtension> _extension;
		public ReadOnlyCollection<FileExtension> Extensions { get; private set; }

        public SupportedExtension()
        {
            _extension = new List<FileExtension>
                             {
                                 new FileExtension(".txt", FileType.Text)
                                 , new FileExtension(".web", FileType.Text) 
                                 , new FileExtension(".ret", FileType.Text) 
                                 , new FileExtension(".zip", FileType.Binary)
                             };

            Extensions = _extension.AsReadOnly();
        }

        public void AddExtension(string extension, FileType type)
        {
            var newEx = new FileExtension(extension, type);

            if (!ExtensionExist(newEx))
                _extension.Add(newEx);
        }

        public void AddExtensionFromXml(string path)
        {
            var confPath = path;

            var conf = new XmlDocument();
            conf.Load(confPath);

            var nodes = conf.SelectNodes("//Configuration/Extension");

            if (nodes == null || nodes.Count == 0)
                throw new NullReferenceException("Não foi possivel carregar o xml de configuração de extensões. Verifica a formatação");

            foreach (XmlNode node in nodes)
            {
                if (node.Attributes == null || node.Attributes.Count == 0)
                    throw new NullReferenceException("Erro na configuração dos atributos no xml de configuração de extensões");

                try
                {
                    var aux = new FileExtension(node.InnerText, (FileType)Enum.Parse(typeof(FileType), node.Attributes["type"].Value));

                    if (!ExtensionExist(aux))
                        _extension.Add(aux);
                }
                catch (ArgumentNullException arg)
                {
                    throw new ArgumentException("Erro no Xml de configuração das extensões. Verifica a sintax do valor atribuido ao atributo type", arg);
                }
            }
        }

        public FileExtension GetFileExtension(string path)
        {
            var extension = Path.GetExtension(path);
            return String.IsNullOrEmpty(extension) ? null : _extension.FirstOrDefault(e => e.Name == extension.ToLower());
        }

        public bool IsSupported(string extension, FileType type)
        {
            return _extension.Any(e => e.Name == extension.ToLower() && e.Type == type);
        }

        public bool IsSupported(FileExtension extension)
        {
            return _extension.Any(e => e.Name == extension.Name && e.Type == extension.Type);
        }

        private bool ExtensionExist(string extension)
        {
            return _extension.Any(e => e.Name == extension);
        }

        private bool ExtensionExist(FileExtension ex)
        {
            return ExtensionExist(ex.Name);
        }
    }
}