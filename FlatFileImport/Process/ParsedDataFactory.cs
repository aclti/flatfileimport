using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FlatFileImport.Exception;

namespace FlatFileImport.Process
{
    public class ParsedDataFactory
    {
        public static ParsedData GetParsedData(string rawData, IBlueprintField blueprintField)
        {
                try
                {
                    if (blueprintField.Type == typeof(DateTime))
                        return new ParsedData(blueprintField.Class, blueprintField.Attribute, DateToDataBase(ParseDate(rawData, blueprintField.Regex)), blueprintField.Type);

                    if (blueprintField.Type == typeof(string))
                        return new ParsedData(blueprintField.Class, blueprintField.Attribute, rawData.Replace("'", "´"), blueprintField.Type);

                    if (blueprintField.Type == typeof(int))
                        return new ParsedData(blueprintField.Class, blueprintField.Attribute, ParseInt(rawData).ToString(), blueprintField.Type);

                    if (blueprintField.Type == typeof(decimal))
                        return new ParsedData(blueprintField.Class, blueprintField.Attribute, ParseDecimal(rawData).ToString().Replace(',', '.'), blueprintField.Type);

                    throw new NotSupportedDataTypeException(String.Format("VALOR NÃO SUPORTADO [ {0} : {1} : {2} ]", blueprintField.Attribute, rawData, blueprintField.Type));
                }
                catch (System.Exception e)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine();
                    sb.AppendFormat("[ NOME: {0} | POSICAO: {1} | TAMANHO: {2} | TIPO: {3} ]", blueprintField.Attribute, blueprintField.Position, blueprintField.Size, blueprintField.Type);
                    sb.AppendLine();
                    sb.AppendFormat("[ LINHA: {0} ]", rawData);
                    sb.AppendLine();

                    throw new System.Exception(sb.ToString() + e);
                }
        }

        private static decimal ParseDecimal(string data)
        {
            if (String.IsNullOrEmpty(data) || data.ToUpper() == "undefined".ToUpper())
                return 0M;

            return Convert.ToDecimal(data);
        }

        private static int ParseInt(string data)
        {
            if (String.IsNullOrEmpty(data) || data.ToUpper() == "undefined".ToUpper())
                return 0;

            return Convert.ToInt32(data);
        }

        private static DateTime ParseDate(string data, Regex regex)
        {
            string[] aux = String.Join("-", regex.Split(data)).TrimEnd("-".ToArray()).TrimStart("-".ToArray()).Split('-');

            return aux.Length == 6 ? new DateTime(Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[2]), Convert.ToInt32(aux[3]), Convert.ToInt32(aux[4]), Convert.ToInt32(aux[5])) : Convert.ToDateTime(String.Join("-", aux));
        }

        private static string DateToDataBase(DateTime date)
        {
            return String.Format("{0}-{1}-{2}", date.Year, date.Month, date.Day);
        }
    }
}
