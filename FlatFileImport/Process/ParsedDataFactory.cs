using System;
using System.Globalization;
using System.Text;
using FlatFileImport.Exception;

namespace FlatFileImport.Process
{
    public class ParsedDataFactory
    {
        private IBlueprintField _blueprintField;
        private static ParsedDataFactory _instance;

        private ParsedDataFactory() { }

        public static ParsedDataFactory GetInstance(IBlueprintField blueprintField)
        {
            if (blueprintField == null)
                throw new ArgumentNullException("blueprintField");

            if(_instance == null)
                _instance = new ParsedDataFactory();

            _instance._blueprintField = blueprintField;

            return _instance;
        }

        public ParsedData GetParsedData(string rawData)
        {
            try
            {
                if (String.IsNullOrEmpty(rawData))
                    throw new ArgumentNullException("rawData");

                if (_blueprintField.Type == typeof(DateTime))
                    return new ParsedData(_blueprintField, ParseDate(rawData));

                if (_blueprintField.Type == typeof(string))
                    return new ParsedData(_blueprintField, ParseString(rawData));

                if (_blueprintField.Type == typeof(int))
                    return new ParsedData(_blueprintField, ParseInt(rawData));

                if (_blueprintField.Type == typeof(decimal))
                    return new ParsedData(_blueprintField, ParseDecimal(rawData));

                throw new NotSupportedDataTypeException(String.Format("VALOR NÃO SUPORTADO [ {0} : {1} : {2} ]", _blueprintField.Attribute, rawData, _blueprintField.Type));
            }
            catch (System.Exception e)
            {
                var sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendFormat("[ NOME: {0} | POSICAO: {1} | TAMANHO: {2} | TIPO: {3} ]", _blueprintField.Attribute, _blueprintField.Position, _blueprintField.Size, _blueprintField.Type);
                sb.AppendLine();
                sb.AppendFormat("[ LINHA: {0} ]", rawData);
                sb.AppendLine();

                throw new System.Exception(sb.ToString() + e);
            }
        }
        
        private string ParseDecimal(string data)
        {
            if (String.IsNullOrEmpty(data) || data.ToUpper() == "undefined".ToUpper())
                return "0";

            data = data.Replace(".", "").Replace(",","");

            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var size = _blueprintField.Size > 0 ? _blueprintField.Size : data.Length;
            var precision = _blueprintField.Precision;
            
            data = data.Insert(size - precision, decimalSeparator);

            var result = Decimal.Parse(data);

            return result.ToString(CultureInfo.InvariantCulture);
        }

        private string ParseString(string data)
        {
            return data.Trim().Replace("'", "´");
        }

        private string ParseInt(string data)
        {
            if (String.IsNullOrEmpty(data) || data.ToUpper() == "undefined".ToUpper())
                return "0";
            var result = Int32.Parse(data);

            return result.ToString(CultureInfo.InvariantCulture);
        }

        // TODO: Definir uma estruta na qual seja possível definir e atribuir tipos customizados de parse para trabalhar com grupos de regex
        private string ParseDate(string data)
        {
            var result = GetDateTimeParsed(data);
            return DateToDataBase(result);
        }

        private DateTime GetDateTimeParsed(string data)
        {
            var regex = _blueprintField.Regex;

            if (regex.Name == "period")
                return PaserPeriod(data);

            if(regex.Name == "date")
                return ParseShortDate(data);

            if (regex.Name == "datetime")
                return ParseDateTime(data);

            throw new InvalidCastException("O formata de data espcificado não é suportado: " + regex.Name);
        }

        private DateTime PaserPeriod(string rawData)
        {
            var match = _blueprintField.Regex.Rule.Match(rawData);

            if(!match.Success)
                throw new InvalidCastException();

            var year = int.Parse(match.Groups["year"].ToString());
            var month = int.Parse(match.Groups["month"].ToString());
            
            return new DateTime(year, month, 1);
        }

        private DateTime ParseShortDate(string rawData)
        {
            var match = _blueprintField.Regex.Rule.Match(rawData);

            if (!match.Success)
                throw new InvalidCastException();

            var year = int.Parse(match.Groups["year"].ToString());
            var month = int.Parse(match.Groups["month"].ToString());
            var day = int.Parse(match.Groups["day"].ToString());

            return new DateTime(year, month, day);
        }

        private DateTime ParseDateTime(string rawData)
        {
            var match = _blueprintField.Regex.Rule.Match(rawData);

            if (!match.Success)
                throw new InvalidCastException();

            var year = int.Parse(match.Groups["year"].ToString());
            var month = int.Parse(match.Groups["month"].ToString());
            var day = int.Parse(match.Groups["day"].ToString());
            var hour = int.Parse(match.Groups["hour"].ToString());
            var minute = int.Parse(match.Groups["minute"].ToString());
            var second = int.Parse(match.Groups["second"].ToString());

            return new DateTime(year, month, day, hour, minute, second);
        }

        private string DateToDataBase(DateTime date)
        {
            return String.Format("{0}-{1}-{2} {3}:{4}:{5}.{6}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
        }
    }
}
