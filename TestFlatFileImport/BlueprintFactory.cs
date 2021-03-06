﻿using System;
using System.IO;
using FlatFileImport.Core;
using FlatFileImport.Input;
using TestFlatFileImport.Dominio;
using TestFlatFileImport.Dominio.Siafi;

namespace TestFlatFileImport
{
    public class BlueprintFactory : IBlueprintFactoy
    {
        private static readonly string _path = AppDomain.CurrentDomain.BaseDirectory;
        private static string _blueprintPath = Path.Combine(_path, @"Samples\Blueprints\");

        public IBlueprintSetter BlueprintSetter;

        #region IBlueprintFactoy Members

        public IBlueprint GetBlueprint(IFileInfo toParse)
        {
            return GetBlueprint(null, toParse);
        }

        public IBlueprint GetBlueprint(object selectParam, IFileInfo toParse)
        {
            if (!(selectParam is Type))
                return null;

            var type = (Type) selectParam;

            if (type == typeof(Movie))
            {
                BlueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "blue-print-movie.xml"));
                return BlueprintSetter.GetBlueprint();
            }

            if (type == typeof(Music))
            {
                BlueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "blue-print-music.xml"));
                return BlueprintSetter.GetBlueprint();
            }

            if (type == typeof(Das))
                return GetBlueprintDas(toParse);

            if (type == typeof(Dasn))
                return GetBlueprintDasn(toParse);

            if (type == typeof(Siafi))
            {
                BlueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "blue-print-siaf.xml"));
                return BlueprintSetter.GetBlueprint();
            }

            return null;
        }

        #endregion

        private IBlueprint GetBlueprintDasn(IFileInfo toParse)
        {
            string pathXml;

            switch (GetVersion(toParse))
            {
                case "108":
                    pathXml = Path.Combine(_blueprintPath, "blue-print-dasn_108.xml");
                    break;

                default:
                    throw new NotImplementedException(toParse.Header + " [ " + toParse.Path + " ] [ " + toParse.Path + " ]");
            }

            BlueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));
            return BlueprintSetter.GetBlueprint();
        }

        private IBlueprint GetBlueprintDas(IFileInfo toParse)
        {
            throw new NotImplementedException();
        }

        private string GetVersion(IFileInfo toParse)
        {
            if (String.IsNullOrEmpty(toParse.Header))
                throw new NullReferenceException("A definição do Header, do FileInfo está fazia." + toParse.Path);

            string[] values = toParse.Header.Split("|".ToCharArray());

            if (values == null || values.Length <= 0)
                throw new NullReferenceException("Não foi possivel fazer o parse para verificar a versão do arquivo" + toParse.Path);

            if (values[0] != "AAAAA")
                throw new Exception("O campo AAAAA não existe no arquivo");

            return values[1];
        }
    }
}
