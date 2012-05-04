using System;

namespace FlatFileImport.Aggregate
{
    public class Sum : IAggregate
    {
        private long _result;
        
        public Sum(IAggregateSubject subject)
        {
            if (subject == null)
                throw new ArgumentNullException("subject");

            Subject = subject;
        }

        #region ITotal Members

        public void AddOperand(long operand)
        {
            _result += operand;
        }

        public long Result
        {
            get
            {
                Cache = _result;
                _result = 0;
                return Cache;
            }
        }

        public long Cache { get; private set; }
        public IAggregateSubject Subject { get; private set; }

        #endregion
    }
}
