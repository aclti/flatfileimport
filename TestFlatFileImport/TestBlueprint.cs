using FlatFileImport.Process;
using Moq;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public  class TestBlueprint
    {
        private IBlueprint _blueprint;

        [SetUp]
        public void Setup()
        {
            _blueprint = new Blueprint("");
        }

        [TearDown]
        public void End()
        {

        }

        [Test]
        public void TestBlueprintHeader()
        {
            var mBlueprint = new Mock<IBlueprint>();
            mBlueprint.Setup(b => b.BluePrintCharSepartor).Returns('|');
        }
    }
}
