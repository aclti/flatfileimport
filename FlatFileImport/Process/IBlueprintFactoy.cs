using System;
using FileInfo = FlatFileImport.Input.FileInfo;

namespace FlatFileImport.Process
{
    public interface IBlueprintFactoy
    {
        /// <summary>
        /// Métedo quer irá retornar a Blueprint que será usada para interpretar o arquivo.
        /// </summary>
        /// <param name="type">O tipo de objeto (dominio) que representa os dados do arquivo que será analisado</param>
        /// <param name="toParse">Recebe o header ou a informação necessário para a seleção da Blueprint necessária para ler o restante do arquivo</param>
        /// <returns>A Blueprint que será utlizada para interpretar o arquivo</returns>
        IBlueprint GetBlueprint(Type type, FileInfo toParse);
    }
}
