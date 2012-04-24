using System;
using System.IO;
using FlatFileImport.Core;
using FlatFileImport.Process;
using TestFlatFileImport.Dominio;
using TestFlatFileImport.Dominio.Siafi;
using FileInfo = FlatFileImport.Input.FileInfo;

namespace TestFlatFileImport
{
    public class BlueprintFactory : IBlueprintFactoy
    {
        public IBlueprintSetter BlueprintSetter;

        #region IBlueprintFactoy Members

        public IBlueprint GetBlueprint(FileInfo toParse)
        {
            throw new NotImplementedException();
        }

        public IBlueprint GetBlueprint(object selectParam)
        {
            throw new NotImplementedException();
        }

        public IBlueprint GetBlueprint(object selectParam, FileInfo toParse)
        {
            var type = (Type) selectParam;

            if (type == typeof(Movie))
            {
                BlueprintSetter = new BlueprintSetterXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blue-print-movie.xml"));
                return BlueprintSetter.GetBlueprint();
            }

            if (type == typeof(Music))
            {
                BlueprintSetter = new BlueprintSetterXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blue-print-music.xml"));
                return BlueprintSetter.GetBlueprint();
            }

            if (type == typeof(Das))
                return GetBlueprintDas(toParse);

            if (type == typeof(Das))
                return GetBlueprintDasn(toParse);

            if (type == typeof(Siafi))
            {
                BlueprintSetter = new BlueprintSetterXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blue-print-siaf.xml"));
                return BlueprintSetter.GetBlueprint();
            }

            throw new NotImplementedException();
        }

        #endregion

        private IBlueprint GetBlueprintDasn(FileInfo toParse)
        {
            string pathXml;

            switch (GetVersion(toParse))
            {
                case "108":
                    pathXml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blue-print-dasn_108.xml");
                    break;

                default:
                    throw new NotImplementedException(toParse.Header + " [ " + toParse.Path + " ] [ " + toParse.Path + " ]");
            }

            BlueprintSetter = new BlueprintSetterXml(pathXml);
            return BlueprintSetter.GetBlueprint();
        }

        private IBlueprint GetBlueprintDas(FileInfo toParse)
        {
            throw new NotImplementedException();
        }

        private string GetVersion(FileInfo toParse)
        {
            if (String.IsNullOrEmpty(toParse.Header))
                throw new NullReferenceException("A definição do Header, do FileInfo está fazia." + toParse.Path);

            string[] values = toParse.Header.Split("|".ToCharArray());

            if (values == null || values.Length <= 0)
                throw new NullReferenceException("Não foi possivel fazer o parse para verificar a versão do arquivo" + toParse.Path);

            if (values[0] != "AAAAA")
                throw new System.Exception("O campo AAAAA não existe no arquivo");

            return values[1];
        }
    }
}
