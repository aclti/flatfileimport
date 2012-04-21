using System;
using System.Globalization;

namespace FlatFileImport.Process
{
    public class ConverterDate : Converter
    {
        private int _year;
        private int _month;
        private int _day;
        private int _hour;
        private int _minute;
        private int _second;
        private int _millisecond;

        public override string Data
        {
            get
            {
                if (!HasGroup)
                    throw new System.Exception("É obrigatorio informar os grupros para o tipo DateTime");

                if (String.IsNullOrEmpty(RawData))
                    return String.Empty;

                Match = RegexRule.Rule.Match(RawData);

                _year = HasGroupMember("year") ? int.Parse(Match.Groups["year"].ToString()) : DateTime.MinValue.Year;
                _month = HasGroupMember("month") ? int.Parse(Match.Groups["month"].ToString()) : DateTime.MinValue.Month;
                _day = HasGroupMember("day") ? int.Parse(Match.Groups["day"].ToString()) : DateTime.MinValue.Day;
                _hour = HasGroupMember("hour") ? int.Parse(Match.Groups["hour"].ToString()) : DateTime.MinValue.Hour;
                _minute = HasGroupMember("minute") ? int.Parse(Match.Groups["minute"].ToString()) : DateTime.MinValue.Minute;
                _second = HasGroupMember("second") ? int.Parse(Match.Groups["second"].ToString()) : DateTime.MinValue.Second;
                _millisecond = HasGroupMember("mileseconds") ? int.Parse(Match.Groups["millisecond"].ToString()) : DateTime.MinValue.Millisecond;

                var date = new DateTime(_year, _month, _day, _hour, _minute, _second, _millisecond);

                return date.ToString(CultureInfo.CurrentCulture);
            }
        }
    }
}
