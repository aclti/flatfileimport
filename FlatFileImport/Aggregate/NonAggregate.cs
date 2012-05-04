using System;

namespace FlatFileImport.Aggregate
{
    public class NonAggregate : IAggregate
    {
        public NonAggregate(IAggregateSubject subject)
        {
            if (subject == null)
                throw new ArgumentNullException("subject");

            Subject = subject;
        }

        #region IAggregate Members

        public void AddOperand(long operand){}
        public long Result { get { return 0; } }
        public IAggregateSubject Subject { get; private set; }
        public long Cache { get { return 0; } }

        #endregion
        }
}
