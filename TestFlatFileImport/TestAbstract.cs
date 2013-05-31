using System;
using System.IO;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public abstract class TestAbstract
    {
        protected string PathSamples;
		protected string Dasn;
		protected string Das;
		protected string SigleDasn;
        protected string MultDasn;
        protected string SigleDas;
        protected string MultDas;
        protected string IgnoreExtensions;

        [SetUp]
        public virtual void Setup()
        {
            PathSamples      = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Samples\Files");
	        Das              = Path.Combine(PathSamples, "Das");
			Dasn             = Path.Combine(PathSamples, "Dasn");
            SigleDas         = Path.Combine(Das, "Single");
            SigleDasn        = Path.Combine(Dasn, "Single");
            MultDas          = Path.Combine(Das, "Mult");
            MultDasn         = Path.Combine(Dasn, "Mult");
            IgnoreExtensions = Path.Combine(PathSamples, "IgnoreExtensions");
        }

        [TearDown]
		public virtual void End()
        {
            PathSamples = String.Empty;
            SigleDasn   = String.Empty;
            MultDasn    = String.Empty;
            SigleDas    = String.Empty;
            MultDas     = String.Empty;
        }
    }
}
