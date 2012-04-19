using System;
using System.IO;
using FlatFileImport.Process;
using TestFlatFileImport.Dominio;
using TestFlatFileImport.Dominio.Siafi;
using FileInfo = FlatFileImport.Input.FileInfo;

namespace TestFlatFileImport
{
    public class BlueprintFactory : IBlueprintFactoy
    {
        #region IBlueprintFactoy Members

        public IBlueprint GetBlueprint(Type type, FileInfo toParse)
        {
            if (type == typeof(Movie))
                return new Blueprint(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blue-print-movie.xml"));

            if (type == typeof(Music))
                return new Blueprint(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blue-print-music.xml"));

            if (type == typeof(Das))
                return GetBlueprintDas(toParse);

            if (type == typeof(Das))
                return GetBlueprintDasn(toParse);

            if (type == typeof(Siafi))
                return new Blueprint(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blue-print-siaf.xml"));

            throw new NotImplementedException();
        }

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

            return new Blueprint(pathXml);
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
        #endregion
    }
}
