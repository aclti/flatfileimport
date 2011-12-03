using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using FlatFileImport.Exception;

namespace FlatFileImport.Process
{
    public sealed class Blueprint
    {
        public BlueprintLine Header { get { return GetHeader(); } }
        public BlueprintLine Footer { get { return GetFooter(); } }
        public List<BlueprintRegister> BlueprintRegistires { get { return GetRegistries(); } }
        public List<BlueprintLine> BlueprintLines { get { return GetBluePrintLines(); } }
        public EnumFieldSeparationType FieldSeparationType { get { return GetSeparationType(); } }
        public char BluePrintCharSepartor { get { return GetSeparaionCharacter(); } }
        public bool UseRegistries { get { return UseRegister(); } }

        private readonly string _path;
        private readonly XPathDocument _xmlDoc;
        private readonly XPathNavigator _navigator;
        private readonly XmlDocument _xDoc;

        public  Blueprint(string path)
        {
            _path = path;
            _xmlDoc = new XPathDocument(_path);
            _navigator = _xmlDoc.CreateNavigator();
            _navigator.MoveToRoot();
            _navigator.MoveToFirstChild();

            _xDoc = new XmlDocument();
            _xDoc.Load(path);
        }

        private XmlNode GetConfiguration(EnumConfigurationItem item)
        {
            var node = _xDoc.SelectSingleNode("//Config/" + item);

            if(node == null)
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

            return char.Parse(node.InnerText);
        }

        private bool UseRegister()
        {
            var node = GetConfiguration(EnumConfigurationItem.UseResgister);

            bool b;
            bool.TryParse(node.InnerText, out b);

            return b;
        }

        private bool IsValidTypeSeparationCharacter(XmlNode node)
        {
            if (node == null)
                return false;

            if (node.InnerText != EnumFieldSeparationType.Character.ToString() && node.InnerText != EnumFieldSeparationType.Position.ToString())
                return false;

            return true;
        }

        private BlueprintLine GetFooter()
        {
            var nodes = _xDoc.SelectNodes("//Footer/Line");
            var lines = GetLines(nodes);

            return lines.Count > 0 ? lines[0] : null;
        }

        private BlueprintLine GetHeader()
        {
            var nodes = _xDoc.SelectNodes("//Header/Line");
            var lines = GetLines(nodes);

            return lines.Count > 0 ? lines[0] : null;
        }

        private List<BlueprintRegister> GetRegistries()
        {
            var registries = new List<BlueprintRegister>();
            var nodes = _xDoc.SelectNodes("//Registries/Register");

            foreach (XmlNode node in nodes)
            {
                var attr = node.Attributes;
                var register = new BlueprintRegister();

                register.Begin = HasAttribute(attr, "begin") ? new Regex(GetAttributeValue(attr, "begin")) : null;
                register.End = HasAttribute(attr, "end") ? new Regex(GetAttributeValue(attr, "end")) : null;
                register.Class = GetAttributeValue(attr, "class");
                registries.Add(register);
            }

            return registries;
        }

        private List<BlueprintLine> GetLines(XmlNodeList nodes)
        {
            var lines = new List<BlueprintLine>();

            foreach (XmlNode node in nodes)
            {
                var line = new BlueprintLine();

                if (node != null && node.Attributes != null && node.Attributes.Count > 0)
                {
                    line.Class = node.Attributes["class"].Value;
                    line.Regex = new Regex(node.Attributes["regex"].Value);
                    
                    bool aux;
                    bool.TryParse(node.Attributes["mandatory"].Value, out aux);
                    line.Mandatory = aux;
                    
                    line.BlueprintFields = GetListFieldParser(node);
                    lines.Add(line);
                }
            }

            return lines;
        }

        private List<BlueprintLine> GetBluePrintLines()
        {
            var nodes = _xDoc.SelectNodes("//Root/Line");
            return GetLines(nodes);
        }

        private List<BlueprintField> GetListFieldParser(XmlNode line)
        {
            var nodes = line.SelectNodes("Fields/Field");
            var className = line.Attributes["class"].Value;
            var fields = new List<BlueprintField>();
            
            foreach (XmlNode node in nodes)
            {
                var field = new BlueprintField();

                if (node != null && node.Attributes != null && node.Attributes.Count > 0)
                {
                    var attr = node.Attributes;
                    
                    try
                    {
                        field.Position = HasAttribute(attr, "position") ? Convert.ToInt32(GetAttributeValue(attr, "position")) : -1;
                        field.Type = ParseType(GetAttributeValue(attr, "type"));
                        field.Size = HasAttribute(attr, "size") ? Convert.ToInt32(GetAttributeValue(attr, "size")) : -1;
                        field.Precision = HasAttribute(attr, "precision") ? Convert.ToInt32(GetAttributeValue(attr, "precision")) : -1;
                        field.Persist = HasAttribute(attr, "persit") && Convert.ToBoolean(GetAttributeValue(attr, "persit"));
                    }
                    catch (System.Exception ex)
                    {
                        throw new System.Exception(ex.ToString());
                    }
                    
                    field.Regex = HasAttribute(attr, "regex") ? GetRegex(GetAttributeValue(attr, "regex")).Rule : null;

                    field.Class = className;
                    field.Attribute = GetAttributeValue(attr, "attribute");

                    fields.Add(field);
                }
            }
            //<Field position="9" type="date" size="14" persit="true" regex="datetime" name="dasn_dt_transmissao" />
            return fields;
        }

        private string GetAttributeValue(XmlAttributeCollection attributes, string name)
        {
            if (HasAttribute(attributes, name))
                return attributes[name].Value;

            return null;
        }

        private bool HasAttribute(XmlAttributeCollection attributes, string name)
        {
            if (attributes != null)
                return attributes.Cast<XmlAttribute>().Any(att => att.Name.ToUpper() == name.ToUpper());

            return false;
        }

        private Type ParseType(string type)
        {
            switch (type)
            {
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

                var attributes = node.Attributes;
                var ht = new Hashtable();

                if (attributes != null)
                    foreach (XmlAttribute att in attributes)
                        ht.Add(att.Name, att.Value);

                var rules = new RegexRule(ht["id"].ToString(), ht["expression"].ToString());

                fields.Add(rules);
            }

            return fields;
        }
    }
}
