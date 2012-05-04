using System;

namespace FlatFileImport.Aggregate
{
    public interface IAggregate
    {
        void AddOperand(long operand);
        long Result { get; }
        long Cache { get; }
        IAggregateSubject Subject { get; } //TALVEZ S ISubject, implementando o patterner Observer
    }
}