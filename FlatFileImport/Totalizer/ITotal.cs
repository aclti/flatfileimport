using System;

namespace FlatFileImport.Totalizer
{
    public interface ITotal
    {
        void AddOperand(long operand);
        long Result { get; }
        ITotalSubject Subject { get; } //TALVEZ S ISubject, implementando o patterner Observer
    }

    public interface ITotalSubject
    {
        string Name { get; }
    }

    public class Sum : ITotal
    {
        private ITotalSubject _subject;
        private long _result;

        public Sum(ITotalSubject subject)
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

        public ITotalSubject Subject { get; private set; }

        #endregion
    }

    public class Avrege : ITotal
    {
        private int _qtd;
        private long _result;
        private long _operand;

        public Avrege(ITotalSubject subject)
        {
            if (subject == null)
                throw new ArgumentNullException("subject");

            Subject = subject;
        }

        #region ITotal Members

        public void AddOperand(long operand)
        {
            _operand += operand;
            _qtd++;
        }

        public long Result
        {
            get
            {
                var q = _qtd;
                var o = _operand;
                _qtd = 0;
                _operand = 0;
                return o / q;
            }
        }

        public ITotalSubject Subject { get; private set; }

        #endregion
    }

    public class Count : ITotal
    {
        #region ITotal Members

        public void AddOperand(long operand)
        {

        }

        public long Result
        {
            get { throw new System.NotImplementedException(); }
        }

        public ITotalSubject Subject
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}