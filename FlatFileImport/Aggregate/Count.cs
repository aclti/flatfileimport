using System;

namespace FlatFileImport.Aggregate
{
    public class Count : IAggregate
    {
        private int _incremment;

        public Count(IAggregateSubject subject)
        {
            if (subject == null)
                throw new ArgumentNullException("subject");

            Subject = subject;
        }

        #region ITotal Members

        public void AddOperand(long operand)
        {
            _incremment++;
        }

        public long Result
        {
            get
            {
                Cache = _incremment;
                _incremment = 0;
                return Cache;
            }
        }

        public long Cache { get; private set; }
        public IAggregateSubject Subject { get; private set; }

        #endregion
    }
}
