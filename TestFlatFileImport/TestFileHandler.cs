using System;
using System.IO;
using FlatFileImport.Input;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestFileHandler
    {
        [Test]
        public void TestFlatTextFile()
        {
            var handler = new HandlerText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples/Files/Dasn/02-3105-DASN10-20100915-01.txt"));
        }
    }
}
