using FlatFileImport.Core;
using FlatFileImport.Process;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestBlueprintFactory
    {
        private IBlueprintFactoy factoy;

        [SetUp]
        public void Setup()
        {
            factoy = new BlueprintFactory();
        }

        [TearDown]
        public void End()
        {

        }
    }
}
