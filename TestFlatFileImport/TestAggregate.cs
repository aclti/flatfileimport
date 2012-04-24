using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FlatFileImport.Core;
using FlatFileImport.Aggregate;
using NUnit.Framework;

namespace TestFlatFileImport
{
    public class TestAggregate
    {
        private string _path;
        private string _blueprintPath;
        private IBlueprint _blueprint;
        private IBlueprintSetter _blueprintSetter;

        [SetUp]
        public void Setup()
        {
            _path = AppDomain.CurrentDomain.BaseDirectory;
            _blueprintPath = Path.Combine(_path, @"Samples\Blueprints\");
        }

        [TearDown]
        public void End()
        {
            _path = String.Empty;
            _blueprintPath = String.Empty;
            _blueprint = null;
        }

        [Test]
        public void TestAggregateSumSimple()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();

            var bline = new BlueprintLineHeader(_blueprint, null);
            IAggregate aggregate = new Sum(bline);

            Assert.AreEqual(0, aggregate.Result);
            Assert.AreEqual(bline.Name, aggregate.Subject.Name);
            Assert.IsTrue(aggregate.Subject is BlueprintLineHeader);

            aggregate.AddOperand(5);
            aggregate.AddOperand(7);
            aggregate.AddOperand(1);

            Assert.AreEqual(13, aggregate.Result);
            Assert.AreEqual(0, aggregate.Result);

            aggregate.AddOperand(-5);
            aggregate.AddOperand(10);
            aggregate.AddOperand(-2);

            Assert.AreEqual(3, aggregate.Result);
            Assert.AreEqual(0, aggregate.Result);
        }

        [Test]
        public void TestAggregateAverageSimple()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();

            var bline = new BlueprintLineHeader(_blueprint, null);
            IAggregate aggregate = new Average(bline);

            Assert.AreEqual(0, aggregate.Result);
            Assert.AreEqual(bline.Name, aggregate.Subject.Name);
            Assert.IsTrue(aggregate.Subject is BlueprintLineHeader);

            aggregate.AddOperand(2);
            aggregate.AddOperand(3);
            aggregate.AddOperand(10);

            Assert.AreEqual(5, aggregate.Result);
            Assert.AreEqual(0, aggregate.Result);

            aggregate.AddOperand(8);
            aggregate.AddOperand(2);
            aggregate.AddOperand(5);
            aggregate.AddOperand(5);

            Assert.AreEqual(5, aggregate.Result);
            Assert.AreEqual(0, aggregate.Result);
        }

        [Test]
        public void TestAggregateCountSimple()
        {
            _blueprintSetter = new BlueprintSetterXml(Path.Combine(_blueprintPath, "blueprint-dasn.xml"));
            _blueprint = _blueprintSetter.GetBlueprint();

            var bline = new BlueprintLineHeader(_blueprint, null);
            IAggregate aggregate = new Count(bline);

            Assert.AreEqual(0, aggregate.Result);
            Assert.AreEqual(bline.Name, aggregate.Subject.Name);
            Assert.IsTrue(aggregate.Subject is BlueprintLineHeader);

            aggregate.AddOperand(0);
            aggregate.AddOperand(0);
            aggregate.AddOperand(0);

            Assert.AreEqual(3, aggregate.Result);
            Assert.AreEqual(0, aggregate.Result);

            aggregate.AddOperand(0);
            aggregate.AddOperand(0);
            aggregate.AddOperand(0);
            aggregate.AddOperand(0);

            Assert.AreEqual(4, aggregate.Result);
            Assert.AreEqual(0, aggregate.Result);
        }
    }
}

