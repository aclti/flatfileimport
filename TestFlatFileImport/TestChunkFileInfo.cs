using FlatFileImport;
using FlatFileImport.Input;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestFlatFileImport
{
	[TestFixture]
	public class TestChunkFileInfo
	{
		private FileInfoChunk _target;
		private List<Mock<IRawLine>> _mocks;
		private List<string> _expectedsValues;

		[SetUp]
		public void Setup()
		{
			_mocks = new List<Mock<IRawLine>> { 
				new Mock<IRawLine>(MockBehavior.Strict),
				new Mock<IRawLine>(MockBehavior.Strict),
				new Mock<IRawLine>(MockBehavior.Strict),
				new Mock<IRawLine>(MockBehavior.Strict),
				new Mock<IRawLine>(MockBehavior.Strict),
			};			

			_expectedsValues = new List<string> 
			{ 
				"Lorem ipsum dolor sit amet", 
				"Quisque laoreet erat sed enim blandit porttitor", 
				"Sed varius ipsum nec dui dapibus consectetur", 
				"Cras et elit urna", 
				"Suspendisse eget feugiat tellus" 
			};

			var index = 0;
			var lineNumber = 50;

			foreach(var mock in _mocks)
			{
				mock.SetupGet(foo => foo.Value).Returns(_expectedsValues[index++]);
				mock.SetupGet(foo => foo.Number).Returns(lineNumber++);
			}

			_target = new FileInfoChunk(_mocks.Select(m => m.Object).ToList<IRawLine>());
		}

		[Test]
		public void TestDeveCorresponderAoValorELinhaEsperada()
		{
			Assert.AreEqual(_expectedsValues[0], _target.Header);

			_target.MoveToNext();

			Assert.AreEqual(50, _target.LineNumber);

			_target.MoveToNext();
			_target.MoveToNext();
			_target.MoveToNext();

			Assert.AreEqual(53, _target.LineNumber);
			Assert.AreEqual(_expectedsValues[3], _target.Line);
			Assert.AreEqual(_expectedsValues[0], _target.Header);

			_target.Reset();

			var index = 0;
			var number = 50;

			while(_target.MoveToNext())
			{
				Assert.AreEqual(_expectedsValues[index], _target.Line);
				Assert.AreEqual(_expectedsValues[0], _target.Header);
				Assert.AreEqual(number, _target.LineNumber);
				
				number++;
				index++;
			}

			_mocks.ForEach(m => m.VerifyAll());
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestDeveLancarUmaExceptionCasoNaoChameMoveNextAntesDeLerALinha()
		{
			Assert.AreEqual(_expectedsValues[0], _target.Header);
			Assert.AreEqual(50, _target.LineNumber);
		}
	}
}
