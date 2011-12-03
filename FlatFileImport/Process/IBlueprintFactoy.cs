using System;
using FileInfo = FlatFileImport.Input.FileInfo;

namespace FlatFileImport.Process
{
    public interface IBlueprintFactoy
    {
        Blueprint GetBluePrint(Type type, FileInfo toParse);
    }
}
