using System;
using System.Collections.Generic;
using System.IO;
using FlatFileImport.Input;
using Moq;
using NUnit.Framework;

namespace TestFlatFileImport
{
	[TestFixture]
	public class TestHandlerFactory : TestAbstract
	{
		private HandlerFacotry _target;
		private Mock<ISupportedExtension> _mockSupportedExtension;

		private List<FileExtension> _extension;

		[SetUp]
		public override void Setup()
		{
			base.Setup();

			_extension = new List<FileExtension>
							 {
								 new FileExtension(".txt", FileType.Text)
								 , new FileExtension(".web", FileType.Text) 
								 , new FileExtension(".ret", FileType.Text) 
								 , new FileExtension(".zip", FileType.Binary)
							 };

			_mockSupportedExtension = new Mock<ISupportedExtension>(MockBehavior.Strict);
			_mockSupportedExtension.SetupGet(ex => ex.Extensions).Returns(_extension.AsReadOnly());
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void TestDeveCriarUmHandler()
		{
			_target = new HandlerFacotry(_mockSupportedExtension.Object);
			_target.Get(Dasn);

			_mockSupportedExtension.VerifyAll();
		}

		[Test]
		public void TestDeveCriarUmHandlerZip()
		{
			FileExtension extension = null;

			_mockSupportedExtension.Setup(ex => ex.GetFileExtension(It.IsAny<string>()))
								   .Callback<string>(input =>
									   {
										   extension = input.EndsWith(".zip")
														   ? new FileExtension(".zip", FileType.Binary)
														   : input.EndsWith(".txt") ? new FileExtension(".txt", FileType.Text) : null;
									   })
								   .Returns(() => extension);


			_target = new HandlerFacotry(_mockSupportedExtension.Object);

			var handler = _target.Get(Path.Combine(SigleDasn, "02-3105-DASN10-20100415-01.zip"));

			Assert.IsNotNull(handler);
			Assert.True(typeof(HandlerZip) == handler.GetType());

			handler.Dispose();
			_mockSupportedExtension.VerifyAll();
		}

		[Test]
		public void TestDeveCriarUmHandlerTex()
		{
			_mockSupportedExtension.Setup(ex => ex.GetFileExtension(It.IsAny<string>())).Returns(new FileExtension(".txt", FileType.Text));

			_target = new HandlerFacotry(_mockSupportedExtension.Object);

			var handler = _target.Get(Path.Combine(SigleDasn, "02-3105-DASN10-20100715-01.txt"));

			Assert.IsNotNull(handler);
			Assert.True(typeof(HandlerText) == handler.GetType());

			_mockSupportedExtension.VerifyAll();
		}
	}
}
