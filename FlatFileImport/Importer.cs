using System;
using FlatFileImport.Process;
using FileInfo = FlatFileImport.Input.FileInfo;

namespace FlatFileImport
{
    public class Importer
    {
        public Parser Parser { set; get; }
        public IBlueprintFactoy BlueprintFactoy { set; get; }
        public Type Type { set; get; }

        public Importer(IBlueprintFactoy factoy, Type type)
        {
            BlueprintFactoy = factoy;
            Type = type;
        }

        public void Process(FileInfo fileInfo, IObserver observer)
        {
            Parser = new Parser(BlueprintFactoy.GetBluePrint(Type, fileInfo), fileInfo, observer);
            Parser.ProcessHeader();

            Console.WriteLine("".PadLeft(80, '*'));
            Console.WriteLine(String.Format("INICIO DO PROCESSAMENTO: ARQUIVO [{0}]", fileInfo.Comment));
            Console.WriteLine("".PadLeft(80, '*'));

            //string l;
            //while ((l = fileInfo.Stream.ReadLine()) != null)
            Parser.Process();

            Parser.UnRegisterObserver(observer);

            Console.WriteLine("".PadLeft(80, '*'));
            Console.WriteLine(String.Format("FINAL DO PROCESSAMENTO: ARQUIVO [{0}]", fileInfo.Comment));
            Console.WriteLine("".PadLeft(80, '*'));
        }
    }
}