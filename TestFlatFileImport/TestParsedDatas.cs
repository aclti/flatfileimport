using System.Collections.Generic;
using FlatFileImport.Data;
using System.Linq;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestParsedDatas
    {
        //private string _path;
        //private string _blueprintPath;
        //private IBlueprint _blueprint;
        //private IBlueprintSetter _blueprintSetter;

        //[SetUp]
        //public void Setup()
        //{
        //    _path = AppDomain.CurrentDomain.BaseDirectory;
        //    _blueprintPath = Path.Combine(_path, @"Samples\Blueprints\");
        //}

        //[TearDown]
        //public void End()
        //{
        //    _path = String.Empty;
        //    _blueprintPath = String.Empty;
        //    _blueprint = null;
        //}

        [Test]
        public void TestInterfaceComposition()
        {
            const string AAAAA = "AAAAA";
            const string ZZZZZ = "ZZZZZ";
            const string D1000 = "D1000";
            const string D9999 = "D9999";
            const string D3001 = "D3001";
            const string D4000 = "D4000";
            const string D5000 = "D5000";
            const string D6000 = "D6000";
            const string P0000 = "00000";
            const string P9999 = "99999";
            const string P1000 = "01000";
            const string P2000 = "02000";
            const string P4000 = "04000";

            var root = new List<IParsedData>();
            root.Add(new ParsedData(AAAAA, null));
            root.Last().AddField("versao", "108", typeof(string));
            root.Last().AddParsedData(D1000);
            root.Last().Headers.Last().AddField("id", "010428182009003", typeof(string));
            root.Last().Headers.Last().AddLine(D3001);
            root.Last().Headers.Last().AddLine(D3001);
            root.Last().Headers.Last().AddParsedData(D4000);
            root.Last().Headers.Last().Headers.Last().AddParsedData(P0000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P1000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P2000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().AddLine(P9999);
            root.Last().Headers.Last().AddLine(D6000);
            root.Last().Headers.Last().AddLine(D6000);
            root.Last().Headers.Last().AddLine(D6000);
            root.Last().AddLine(D9999);
            root.Last().AddParsedData(D1000);
            root.Last().Headers.Last().AddField("id", "089241892009002", typeof(string));
            root.Last().Headers.Last().AddLine(D3001);
            root.Last().Headers.Last().AddLine(D3001);
            root.Last().Headers.Last().AddParsedData(D4000);
            root.Last().Headers.Last().Headers.Last().AddParsedData(P0000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P1000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P2000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().AddLine(P9999);
            root.Last().Headers.Last().AddParsedData(D4000);
            root.Last().Headers.Last().Headers.Last().AddParsedData(P0000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P1000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P2000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().AddLine(P9999);
            root.Last().Headers.Last().AddParsedData(D4000);
            root.Last().Headers.Last().Headers.Last().AddParsedData(P0000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P1000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P2000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().AddLine(P9999);
            root.Last().Headers.Last().AddLine(D5000);
            root.Last().Headers.Last().AddLine(D6000);
            root.Last().AddLine(D9999);
            root.Last().AddParsedData(D1000);
            root.Last().Headers.Last().AddField("id", "091478132009002", typeof(string));
            root.Last().Headers.Last().AddLine(D3001);
            root.Last().Headers.Last().AddLine(D3001);
            root.Last().Headers.Last().AddParsedData(D4000);
            root.Last().Headers.Last().Headers.Last().AddParsedData(P0000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P1000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P2000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().AddLine(P9999);
            root.Last().Headers.Last().AddParsedData(D4000);
            root.Last().Headers.Last().Headers.Last().AddParsedData(P0000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P1000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P2000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().Headers.Last().AddLine(P4000);
            root.Last().Headers.Last().Headers.Last().AddLine(P9999);
            root.Last().Headers.Last().AddLine(D6000);
            root.Last().Headers.Last().AddLine(D6000);
            root.Last().AddLine(D9999);
            root.Add(new ParsedData(ZZZZZ, null));

            Assert.AreEqual(2, root.Count());
            Assert.AreEqual(3, root.First().Headers.Count);
            Assert.AreEqual(5, root.First().Headers.First().Details.Count);
            Assert.IsNull(root.Find(p => p.Name == P0000));
        }
    }
}