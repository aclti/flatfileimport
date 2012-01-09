using System.Collections.Generic;
using System.Text.RegularExpressions;
using FlatFileImport.Process;
using Moq;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestParserRawLinePositionalCharacter
    {
        private IBlueprintLine _blueprintLine;

        [SetUp]
        public void Setup()
        {
            var mBF1 = new Mock<IBlueprintField>();
            mBF1.Setup(f => f.Attribute).Returns("Nome");
            mBF1.Setup(f => f.Class).Returns("Filme");
            mBF1.Setup(f => f.Persist).Returns(true);
            mBF1.Setup(f => f.Position).Returns(0);
            mBF1.Setup(f => f.Precision).Returns(0);
            mBF1.Setup(f => f.Regex).Returns(new Regex("^F"));
            mBF1.Setup(f => f.Size).Returns(5);
            mBF1.Setup(f => f.Type).Returns(typeof(string));

            var mBF2 = new Mock<IBlueprintField>();
            mBF2.Setup(f => f.Attribute).Returns("Diretor");
            mBF2.Setup(f => f.Class).Returns("Filme");
            mBF2.Setup(f => f.Persist).Returns(true);
            mBF2.Setup(f => f.Position).Returns(0);
            mBF2.Setup(f => f.Precision).Returns(0);
            mBF2.Setup(f => f.Regex).Returns(new Regex("^F"));
            mBF2.Setup(f => f.Size).Returns(5);
            mBF2.Setup(f => f.Type).Returns(typeof(string));

            var mBluePrintLine = new Mock<IBlueprintLine>();
            mBluePrintLine.Setup(l => l.Mandatory).Returns(true);
            mBluePrintLine.Setup(l => l.Class).Returns("Filme");
            mBluePrintLine.Setup(l => l.Regex).Returns(new Regex("^F"));

            var lbf = new List<IBlueprintField> {mBF1.Object, mBF2.Object};

            mBluePrintLine.Setup(l => l.BlueprintFields).Returns(lbf);

            _blueprintLine = mBluePrintLine.Object;
        }

        [TearDown]
        public void End()
        {
            _blueprintLine = null;
        }

        [Test]    
        public void TestMock()
        {
            Assert.True(_blueprintLine.Mandatory);
            Assert.AreEqual(2, _blueprintLine.BlueprintFields.Count);
        }
    }
}
