using System;
using System.IO;
using FlatFileImport.Core;
using FlatFileImport.Input;

namespace TesteImportDasn
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

            return type == typeof(Dasn) ? GetBlueprintDasn(toParse) : null;
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
                    throw new Exception(toParse.Header + " [ " + toParse.Path + " ] [ " + toParse.Path + " ]");
            }

            BlueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "blueprint-dasn-resumida.xml"));
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
