using System;

namespace FlatFileImport.Aggregate
{
    public class Sum : IAggregate
    {
        private IAggregateSubject _subject;
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
                var aux = _result;
                _result = 0;
                return aux;
            }
        }

        public IAggregateSubject Subject { get; private set; }

        #endregion
    }
}
