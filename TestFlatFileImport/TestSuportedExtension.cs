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

            Assert.Greater(supportedExtensions.Extension.Count,0);
            
            var l = supportedExtensions.Extension;
            Assert.IsTrue(l.Any(e => e.Extension == ".txt" && e.Type == FileType.Text));
            Assert.IsTrue(l.Any(e => e.Extension == ".ret" && e.Type == FileType.Text));
            Assert.IsTrue(l.Any(e => e.Extension == ".web" && e.Type == FileType.Text));
            Assert.IsTrue(l.Any(e => e.Extension == ".zip" && e.Type == FileType.Binary));
        }

        [Test]
        public void TestGetExtesionsFromXmlConfig()
        {
            var sExtensions = new SupportedExtension();
            var qtd = sExtensions.Extension.Count;

            sExtensions.AddExtensionFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\config-extension.xml"));

            var l = sExtensions.Extension;
            Assert.AreEqual(qtd + 2, sExtensions.Extension.Count);
            Assert.IsTrue(l.Any(e => e.Extension == ".new" && e.Type == FileType.Text));
            Assert.IsTrue(l.Any(e => e.Extension == ".rar" && e.Type == FileType.Binary));
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
            var l = Handler.SupportedExtesion;

            Assert.AreEqual(4, l.Count);
            Handler.AddExtension("jar", FileType.Binary);
            Assert.AreEqual(5, l.Count);
            Handler.AddExtensionFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\config-extension.xml"));
            Assert.AreEqual(7, l.Count);
        }

        [Test]
        public void TestGetExtension()
        {
            
        }
    }
}
