using System;
using System.IO;
using System.Linq;
using FlatFileImport.Input;
using NUnit.Framework;

namespace TestFlatFileImport
{
    class TestSuportedExtension
    {
        [Test]
        public void TestDefaultExtensions()
        {
            var supportedExtensions = new SupportedExtension();

            Assert.Greater(supportedExtensions.Extensions.Count,0);
            
            var l = supportedExtensions.Extensions;
            Assert.IsTrue(l.Any(e => e.Name == ".txt" && e.Type == FileType.Text));
            Assert.IsTrue(l.Any(e => e.Name == ".ret" && e.Type == FileType.Text));
            Assert.IsTrue(l.Any(e => e.Name == ".web" && e.Type == FileType.Text));
            Assert.IsTrue(l.Any(e => e.Name == ".zip" && e.Type == FileType.Binary));
        }

        [Test]
        public void TestGetExtesionsFromXmlConfig()
        {
            var sExtensions = new SupportedExtension();
            var qtd = sExtensions.Extensions.Count;

            sExtensions.AddExtensionFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\config-extension.xml"));

            var l = sExtensions.Extensions;
            Assert.AreEqual(qtd + 2, sExtensions.Extensions.Count);
            Assert.IsTrue(l.Any(e => e.Name == ".new" && e.Type == FileType.Text));
            Assert.IsTrue(l.Any(e => e.Name == ".rar" && e.Type == FileType.Binary));
        }

        [Test]
        [ExpectedException("System.NullReferenceException")]
        public void TestConfigMissConfigTagCase()
        {
            var sExtensions = new SupportedExtension();
            sExtensions.AddExtensionFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\config-extension-missconfig-tagcase.xml"));
        }

        [Test]
        [ExpectedException("System.ArgumentException")]
        public void TestConfigMissConfigAttributeCase()
        {
            var sExtensions = new SupportedExtension();
            sExtensions.AddExtensionFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\config-extension-missconfig-attributecase.xml"));
        }

        [Test]
        public void TestIsExtensionSupported()
        {
            var sExtensions = new SupportedExtension();

            Assert.IsTrue(sExtensions.IsSupported(".txt", FileType.Text));
            Assert.IsTrue(sExtensions.IsSupported(".Web", FileType.Text));
            Assert.IsTrue(sExtensions.IsSupported(".Zip", FileType.Binary));
            Assert.IsTrue(sExtensions.IsSupported(".ret", FileType.Text));
            Assert.IsTrue(!sExtensions.IsSupported(".new", FileType.Text));
        }

        [Test]
        public void TestAddExtensionFromHandler()
        {
	        Assert.Inconclusive();
	        //var l = Handler.Extensions;

	        //Assert.AreEqual(4, l.Count);
	        //Handler.AddExtension("jar", FileType.Binary);
	        //Assert.AreEqual(5, l.Count);
	        //Handler.AddExtensionFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\config-extension.xml"));
	        //Assert.AreEqual(7, l.Count);
        }

        [Test]
        public void TestGetExtension()
        {
            
        }
    }
}
