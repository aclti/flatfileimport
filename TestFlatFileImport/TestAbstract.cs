using System;
using System.IO;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public abstract class TestAbstract
    {
        protected string PathSamples;
        protected string SigleDasn;
        protected string MultDasn;
        protected string SigleDas;
        protected string MultDas;

        [SetUp]
        public void Setup()
        {
            PathSamples = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\Files");
            SigleDas = Path.Combine(PathSamples, @"Das\Single");
            SigleDasn = Path.Combine(PathSamples, @"Dasn\Single");
            MultDas = Path.Combine(PathSamples, @"Das\Mult");
            MultDasn = Path.Combine(PathSamples, @"Dasn\Mult");
        }

        [TearDown]
        public void End()
        {
            PathSamples = String.Empty;
            SigleDasn = String.Empty;
            MultDasn = String.Empty;
            SigleDas = String.Empty;
            MultDas = String.Empty;
        }
    }
}
