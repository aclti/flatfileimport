using System;
using System.Text.RegularExpressions;

namespace FlatFileImport.Core
{
    public class Occurrence : IOccurrence
    {
        private static Regex _regex = new Regex(@"^(Range\[)(?<min>[0-9]+)-(?<max>[0-9]+)(\])$");

        public Occurrence(IBlueprintLine blueprintLine, EnumOccurrence occurrence)
        {
            if(blueprintLine == null)
                throw new ArgumentNullException("blueprintLine");

            if (occurrence == EnumOccurrence.None)
                throw new ArgumentNullException("occurrence");

            BlueprintLine = blueprintLine;
            Type = occurrence;
        }

        public Occurrence(IBlueprintLine blueprintLine, EnumOccurrence occurrence, string RangeDefinition) :this(blueprintLine, occurrence)
        {
            SetMimMax(RangeDefinition);
        }

        #region IOccurrence Members

        public IBlueprintLine BlueprintLine { get; private set; }
        public int? Min { get; private set; }
        public int? Max { get; private set; }
        public EnumOccurrence Type { get; private set; }

        #endregion

        private void SetMimMax(string values)
        {
            if (Type != EnumOccurrence.Range)
                throw new System.Exception("O tipo de ocorrencia definido não possui Min e Max");

            var match = _regex.Match(values);

            if (!match.Success)
                throw new FormatException("O intervalo informado para o tipo de ocorrencia não está correto. Consulte a documentação.");

            var max = int.Parse(match.Groups["max"].ToString());
            var min = int.Parse(match.Groups["min"].ToString());

            if (min >= max)
                throw new System.Exception("O intervalo definido para o tipo de ocorrenia está errado, o min é maior ou igual ao maximo.");

            Min = min;
            Max = max;
        }
    }
}