using FlatFileImport.Core;
using System;
using System.Globalization;
using System.Text;

namespace FlatFileImport.Process
{
    public class ConverterNumber : Converter
    {
        // TODO: Utilizar os grupos para formatar o numero de acordo com a necessidade
        public override string Data
        {
            get
            {
                if (String.IsNullOrEmpty(RawData) || RawData == "0")
                    return "0";

                if (BlueprintField.Type == typeof(int))
                    return int.Parse(RawData).ToString(CultureInfo.InvariantCulture);

                if (FieldSeparationType == EnumFieldSeparationType.Character && BlueprintField.Type == typeof(decimal))
                    return SetDecimalSepartorCulture();

                if (FieldSeparationType == EnumFieldSeparationType.Position && BlueprintField.Type == typeof(decimal))
                    return InsertDecimalSeparator();

                throw new System.Exception("Não possivel indentificar o tipo de Caracter de separação de campos da blueprint.");
            }
        }

        private string SetDecimalSepartorCulture()
        {
            var cultureDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if (RawData.IndexOf(cultureDecimalSeparator, StringComparison.Ordinal) > 0)
                return decimal.Parse(RawData).ToString(CultureInfo.CurrentCulture);

            var data = RawData.Replace(GetNoCultureDecimalSeparator(), cultureDecimalSeparator);
            return decimal.Parse(data).ToString(CultureInfo.CurrentCulture);
        }

        private string InsertDecimalSeparator()
        {
                var sb = new StringBuilder();
                var sizeIntPart = BlueprintField.Size - BlueprintField.Precision;
                var intPart = RawData.Substring(0, sizeIntPart);
                sb.Append(intPart);
                sb.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                sb.Append(RawData.Substring(sizeIntPart, BlueprintField.Precision));

                return decimal.Parse(sb.ToString()).ToString(CultureInfo.CurrentCulture);
        }

        private string GetNoCultureDecimalSeparator()
        {
            var cultureSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if ("." == cultureSeparator)
                return ",";

            return ".";
        }
    }
}