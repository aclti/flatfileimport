using FlatFileImport.Input;
using Moq;
using NUnit.Framework;

namespace TestFlatFileImport
{
	[TestFixture]
	public class TestHandlerDirectory : TestAbstract
	{
		private HandlerDirectory _target;
		private Mock<IHandlerFactory> _mockHandlerFactory;
		private Mock<IHandler> _mockHandler;

		public override void Setup()
		{
			base.Setup();

			_mockHandlerFactory = new Mock<IHandlerFactory>();
			_mockHandler        = new Mock<IHandler>();
			
			_target = new HandlerDirectory(Dasn, _mockHandlerFactory.Object);
			_mockHandlerFactory.Setup(f => f.Get(It.IsAny<string>())).Returns(_mockHandler.Object);
		}

		[Test]
		public void TesteBasicDirectoryRead()
		{
			Assert.IsNotNull(_target.Handlers);
			Assert.IsNotNull(_target.Paths);
			Assert.GreaterOrEqual(_target.Handlers.Count, 6);
			Assert.GreaterOrEqual(_target.Paths.Count, 6);
		}
	}
}
