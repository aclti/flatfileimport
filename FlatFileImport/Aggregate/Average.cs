using System;

namespace FlatFileImport.Aggregate
{
    public class Average : IAggregate
    {
        private int? _qtd;
        private long _operand;

        public Average(IAggregateSubject subject)
        {
            if (subject == null)
                throw new ArgumentNullException("subject");

            Subject = subject;
        }

        #region ITotal Members

        public void AddOperand(long operand)
        {
            _operand += operand;

            if (_qtd == null)
                _qtd = 1;
            else
                _qtd++;
        }

        public long Result
        {
            get
            {
                var q = _qtd ?? 1;
                var o = _operand;
                _qtd = null;
                _operand = 0;
                return o / q;
            }
        }

        public IAggregateSubject Subject { get; private set; }

        #endregion
    }
}
