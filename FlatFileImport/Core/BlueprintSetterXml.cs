using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Xml;
using FlatFileImport.Exception;

namespace FlatFileImport.Core
{
    public class BlueprintSetterXml : IBlueprintSetter
    {
        private const string RgxDate = @"^(?<year>[1-9][0-9]{3})(?<month>1[0-2]|0[1-9])(?<day>0[1-9]|[1-2][0-9]|3[0-1])((?<hour>[01][0-9]|2[0-3])(?<minute>[0-5][0-9])(?<second>[0-5][0-9]))?$";
        private const string RgxInt = @"^[0-9]+$";
        private const string RgxDecimal = @"^[0-9]+([.,][0-9]+)?$";

        private readonly string _path;
        private readonly XPathDocument _xmlDoc;
        private readonly XPathNavigator _navigator;
        private readonly XmlDocument _xDoc;
        private static IBlueprint _blueprint;

        public BlueprintSetterXml(string path)
        {
            _path = path;
            _xmlDoc = new XPathDocument(_path);
            _navigator = _xmlDoc.CreateNavigator();
            _navigator.MoveToRoot();
            _navigator.MoveToFirstChild();

            _xDoc = new XmlDocument();
            _xDoc.Load(path);

            _blueprint = new BlueprintXml(GetSeparationType(), GetSeparaionCharacter());
        }

        #region IBlueprintSetter Members

        private XmlNode GetConfiguration(EnumConfigurationItem item)
        {
            var node = _xDoc.SelectSingleNode("//Config/" + item);

            if (node == null)
                throw new BlueprintMissingConfigItem(item.ToString());

            return node;
        }

        private EnumFieldSeparationType GetSeparationType()
        {
            var node = GetConfiguration(EnumConfigurationItem.FieldSeparationType);

            if (!IsValidTypeSeparationCharacter(node))
                throw new BlueprintMissingConfigItem(EnumConfigurationItem.FieldSeparationType.ToString());

            return (EnumFieldSeparationType)Enum.Parse(typeof(EnumFieldSeparationType), node.InnerText);
        }

        private char GetSeparaionCharacter()
        {
            var node = GetConfiguration(EnumConfigurationItem.Splitter);

            return String.IsNullOrEmpty(node.InnerText) ? '\0' : char.Parse(node.InnerText);
        }

        private bool IsValidTypeSeparationCharacter(XmlNode node)
        {
            if (node == null)
                return false;

            if (node.InnerText != EnumFieldSeparationType.Character.ToString() && node.InnerText != EnumFieldSeparationType.Position.ToString())
                return false;

            return true;
        }

        public IBlueprint GetBlueprint()
        {
            _blueprint.AddBlueprintLines(GetBlueprintLines());
            return _blueprint;
        }

        #endregion

        private List<IBlueprintLine> GetBlueprintLines()
        {
            var nodes = _xDoc.SelectNodes("//Root/Line");

            if(nodes == null)
                throw new System.Exception("Nenhuma Line foi definida para esta Blueprint. Consulte a documentação.");

            var lines = new List<IBlueprintLine>();

            foreach (XmlNode node in nodes)
            {
                var parent = lines.Find(bl => bl.Name == GetAttributeValue(node.Attributes, EnumLineAttributes.Parent));
                var line = parent == null ? GetBlueprintLine(node) : GetBlueprintLine(node, parent);

                line.Name = GetAttributeValue(node.Attributes, EnumLineAttributes.Name);
                line.Occurrence = GetOccurrence(node, line);

                var regex = GetAttributeValue(node.Attributes, EnumLineAttributes.Regex);
                line.Regex = !String.IsNullOrEmpty(regex) ? new Regex(regex) : null;
                line.BlueprintFields = GetListFiels(node, line);
                lines.Add(line);
            }

            return lines;
        }

        private IOccurrence GetOccurrence(XmlNode node, IBlueprintLine blueprintLine)
        {
            var attrColl = node.Attributes;

            if (!HasAttribute(attrColl, EnumLineAttributes.Occurrence))
                return new Occurrence(blueprintLine, EnumOccurrence.AtLeastOne);

            var value = GetAttributeValue(attrColl, EnumLineAttributes.Occurrence);

            EnumOccurrence type;
            if (value.StartsWith(EnumOccurrence.Range.ToString()))
                type = EnumOccurrence.Range;
            else
                type = (EnumOccurrence)Enum.Parse(typeof(EnumOccurrence), value);

            return type == EnumOccurrence.Range ? new Occurrence(blueprintLine, type, value) : new Occurrence(blueprintLine, type);
        }

        private bool HasParent(XmlNode node)
        {
            var attr = node.Attributes;
            var name = EnumLineAttributes.Parent;

            if (!HasAttribute(attr, name))
                return false;

            var parent = GetAttributeValue(attr, name);

            return !String.IsNullOrEmpty(parent);
        }

        private IBlueprintLine GetBlueprintLine(XmlNode node)
        {
            return GetBlueprintLine(node, null);
        }

        private IBlueprintLine GetBlueprintLine(XmlNode node, IBlueprintLine parent)
        {
            var attributeColl = node.Attributes;
            var type = GetLineType(attributeColl);

            if (type == EnumLineType.None || type == EnumLineType.Details)
                return new BlueprintLineDetails(_blueprint, parent);

            if (type == EnumLineType.Header)
                return new BlueprintLineHeader(_blueprint, parent);

            if (type == EnumLineType.Footer)
                return new BlueprintLineFooter(_blueprint, parent);

            throw new System.Exception("O Type para a linha da Blueprint não está definido corretamente. Verifique a documentação.");
        }

        private List<IBlueprintField> GetListFiels(XmlNode line, IBlueprintLine blueprintLine)
        {
            var nodes = line.SelectNodes("Fields/Field");
            var fields = new List<IBlueprintField>();

            if (nodes == null)
                throw new System.Exception("Nenhum Field foi definido para a Line: " + blueprintLine.Name);

            foreach (XmlNode node in nodes)
            {
                if (node.Attributes == null || node.Attributes.Count <= 0)
                    throw new System.Exception("Nenhum atributo foir definido para o Field " + node.Name);

                var field = new BlueprintField(blueprintLine);
                var attr = node.Attributes;


                if (!HasAttribute(attr, EnumFieldAttributes.Position))
                    throw new System.Exception("O Atributo: " + EnumFieldAttributes.Position + " é obrigatorio");

                if (!HasAttribute(attr, EnumFieldAttributes.Type))
                    throw new System.Exception("O Atributo: " + EnumFieldAttributes.Type + " é obrigatorio");

                if (!HasAttribute(attr, EnumFieldAttributes.Name))
                    throw new System.Exception("O Atributo: " + EnumFieldAttributes.Name + " é obrigatorio");

                field.Name = GetAttributeValue(attr, EnumFieldAttributes.Name);

                var persist = GetAttributeValue(attr, EnumFieldAttributes.Persist);
                field.Persist = String.IsNullOrEmpty(persist) || bool.Parse(persist);

                field.Position = GetInt(GetAttributeValue(attr, EnumFieldAttributes.Position));
                field.Precision = GetNullableInt(GetAttributeValue(attr, EnumFieldAttributes.Precision)) ?? -1;
                field.Type = ParseType(GetAttributeValue(attr, EnumFieldAttributes.Type));

                if (HasAttribute(attr, EnumFieldAttributes.Regex))
                    field.Regex = GetRegex(GetAttributeValue(attr, EnumFieldAttributes.Regex));
                else
                {
                    if(field.Type == typeof(int))
                        field.Regex = new RegexRule("int", RgxInt);
                    else if(field.Type == typeof(decimal))
                        field.Regex = new RegexRule("decimal", RgxDecimal);
                    else if (field.Type == typeof(DateTime))
                        field.Regex = new RegexRule("datetime", RgxDate);
                }

                field.Size = GetNullableInt(GetAttributeValue(attr, EnumFieldAttributes.Size)) ?? -1;
                

                fields.Add(field);
            }

            return fields;
        }

        #region Util

        private RegexRule GetRegex(string ruleName)
        {
            List<RegexRule> l = GetRulesSet();

            foreach (RegexRule regex in l)
                if (regex.Name.ToUpper() == ruleName.ToUpper())
                    return regex;

            throw new System.Exception("Regra não implemtada");
        }

        private List<RegexRule> GetRulesSet()
        {
            var fields = new List<RegexRule>();
            var xNode = _xDoc.SelectNodes("//Root/Regex/Rule");

            if (xNode == null)
                return fields;

            foreach (XmlNode node in xNode)
            {
                if (node.Attributes == null)
                    continue;

                var rules = new RegexRule(node.Attributes["id"].Value, node.InnerText);

                fields.Add(rules);
            }

            return fields;
        }

        private Type ParseType(string type)
        {
            switch (type)
            {
                case "datetime":
                case "date":
                    return typeof(DateTime);
                case "string":
                    return typeof(string);
                case "int":
                    return typeof(int);
                case "decimal":
                    return typeof(decimal);
                default:
                    return null;
            }
        }

        private EnumLineType GetLineType(XmlAttributeCollection attributeColl)
        {
            var attValue = GetAttributeValue(attributeColl, EnumLineAttributes.Type);

            if (String.IsNullOrEmpty(attValue))
                return EnumLineType.None;

            return (EnumLineType)Enum.Parse(typeof(EnumLineType), attValue);
        }

        private bool HasAttribute(XmlAttributeCollection attributeColl, EnumFieldAttributes attribute)
        {
            return HasAttribute(attributeColl, attribute.ToString());
        }

        private bool HasAttribute(XmlAttributeCollection attributes, EnumLineAttributes attribute)
        {
            return HasAttribute(attributes, attribute.ToString());
        }

        private bool HasAttribute(XmlAttributeCollection attributeColl, string attribute)
        {
            if (attributeColl == null)
                return false;

            var attr = attributeColl.Cast<XmlAttribute>().FirstOrDefault(att => att.Name.ToUpper() == attribute.ToUpper());
            return attr != null && attribute.ToUpper() == attr.Name.ToUpper() && !String.IsNullOrEmpty(attr.Value);
        }

        private string GetAttributeValue(XmlAttributeCollection attributeColl, EnumLineAttributes attribute)
        {
            return GetAttributeValue(attributeColl, attribute.ToString());
        }

        private string GetAttributeValue(XmlAttributeCollection attributeColl, EnumFieldAttributes attribute)
        {
            return GetAttributeValue(attributeColl, attribute.ToString());
        }

        private string GetAttributeValue(XmlAttributeCollection attributeColl, string attribute)
        {
            attribute = attribute.ToLower();

            if (HasAttribute(attributeColl, attribute))
                return attributeColl[attribute].Value;

            return String.Empty;
        }

        private int GetInt(string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new System.FormatException();

            return int.Parse(value);
        }

        private int? GetNullableInt(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            return int.Parse(value);
        }

        private decimal? GetNullableDecimal(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            return decimal.Parse(value);
        }

        private bool? GetNullableBool(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            return bool.Parse(value);
        }

        #endregion
    }
}