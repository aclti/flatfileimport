using System;
using System.Collections.Generic;
using System.IO;
using FlatFileImport.Core;
using FlatFileImport.Data;
using FlatFileImport.Exception;
using FlatFileImport.Validate;
using FileInfo = FlatFileImport.Input.FileInfo;

namespace FlatFileImport.Process
{
    public class Parser
    {
        //NOVOS
        //public IParsedRegister ParsedRegister { get; private set; }
        //private IParserRawDataLine _parserRawData;
        private List<ParsedLine> _parsedDatas;
        public IValidate Validate;
        private FileInfo _fileInfo;

        // TODO: Mandar sinal que o processamento do arquivo acabou
        private IBlueprint _blueprint;
        private IBlueprintLine _currentBlueprintLine;
        private IBlueprintRegister _register;

        public List<IBlueprintLine> BlueprintLines { get { return null; } }
        public List<IBlueprintRegister> Registers { get { return null; } }
        public IBlueprintLine Footer { get { return _blueprint.Footer; } }
        public IBlueprintLine Header { get { return _blueprint.Header; } }

        private string _rawLineData;

        public Parser(IBlueprint blueprint, FileInfo file, IObserver observer)
        {
            //RegisterObserver(observer);
            _fileInfo = file;
            _blueprint = blueprint;
            _register = _blueprint.BlueprintRegistires[0];
            //_parserRawData = GetParserRawDataLine();
        }

        //private IParserRawDataLine GetParserRawDataLine()
        //{
        //    //if (_blueprint.FieldSeparationType == EnumFieldSeparationType.Character)
        //    //    return new ParserRawLinePositionalCharacter(_currentBlueprintLine);

        //    //if (_blueprint.FieldSeparationType == EnumFieldSeparationType.Position)
        //    //    return new ParserRawLinePositional(_currentBlueprintLine);

        //    throw new ParserRawDataNotFound();
        //}

        private void ValidLine()
        {
            Validate = null;//new ValidateLine(_parserRawData.RawDataCollection, _currentBlueprintLine);

            if (!Validate.IsValid())
            {
                if (Validate.ValidateResult.Severity == ExceptionSeverity.Fatal && /*_blueprintLine.Mandatory*/true)
                    throw new DataLengthDontMatchWithBlueprintDefinition(Validate.ValidateResult);

                using (var file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Warnning.txt", true))
                {
                    file.WriteLine(_fileInfo.Path + Validate.ValidateResult.Message);
                }
            }
        }

        private void ValidateField()
        {
            Validate = null; // new ValidateFieldPositionalCharacter(_parserRawData.RawDataCollection, _currentBlueprintLine);

            if (!Validate.IsValid())
            {
                if (/*_blueprintLine.Mandatory*/true)
                    throw new System.Exception(Validate.ValidateResult.Message);

                using (var file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Warnning.txt", true))
                {
                    file.WriteLine(_fileInfo.Path + Validate.ValidateResult.Message);
                }
            }

        }

        private void ValidateRegister()
        {
            Validate = new ValidateRegister(_parsedDatas, _currentBlueprintLine.BlueprintFields);

            if (!Validate.IsValid())
                throw new System.Exception("");
        }

        public void ProcessHeader()
        {
            _rawLineData = _fileInfo.Header;
            _currentBlueprintLine = _blueprint.Header;

            //_parserRawData.ParseRawLineData(_rawLineData);

            ValidLine();

            if(Validate.IsValid())
            {
                ValidateField();
               // _parsedDatas = _parserRawData.ParsedDatas;    
            }

            if (_parsedDatas.Count > 0)
            {
                //var aux = new ParsedData(_currentBlueprintLine.Class, "ARQUIVO", _fileInfo.Comment, typeof(string));
                //_parsedDatas.Add(aux);
                //NotifyObservers();
            }

            _parsedDatas = null;
        }

        //public void ProcessFooter(FileInfo fileInfo)
        //{

        //}

        public void Process()
        {
            while (_fileInfo.MoveToNext())
            {
                _rawLineData = _fileInfo.Line;
                _currentBlueprintLine = GetBlueprintLine();

                if (_blueprint.UseRegistries)
                    ProcessRegister();
                else
                    ProcessLine();
            }
        }

        private void ProcessRegister()
        {
            //TODO: Verificar uma forma mais infomativa de tratar a situação quando uma matchline não for encontrada. Provavelmente um exceção, pois isso numca deve acontecer
            if (_currentBlueprintLine == null)
                return;

            if (_register.Begin.Match(_rawLineData).Success)
            {
                _register.IsComplet = false;
                _parsedDatas = new List<ParsedLine>();
            }

            //_parserRawData.ParseRawLineData(_rawLineData);

            ValidLine();

            if(Validate.IsValid())
            {
                ValidateField();
                //_parsedDatas.AddRange(_parserRawData.ParsedDatas);    
            }
            
            if (_register.End.Match(_rawLineData).Success)
                _register.IsComplet = true;

            if (_register.IsComplet)
            {
                //NotifyObservers();
                _parsedDatas = null;
            }
        }

        private void ProcessLine()
        {
            //TODO: Verificar uma forma mais infomativa de tratar a situação quando uma matchline não for encontrada. Provavelmente um exceção, pois isso numca deve acontecer
            if (_currentBlueprintLine == null)
                return;

            //_parserRawData.ParseRawLineData(_rawLineData);
            _parsedDatas = new List<ParsedLine>();

            ValidLine();

            if (Validate.IsValid())
            {
                ValidateField();

                //_parsedDatas = _parserRawData.ParsedDatas;
                //NotifyObservers();
            }

            _parsedDatas = null;
        }

        private bool MatchBlueprintLine(IBlueprintLine line)
        {
            return line.Regex.Match(_rawLineData).Success;
        }

        private IBlueprintLine GetBlueprintLine()
        {
            foreach (var l in BlueprintLines)
                if (MatchBlueprintLine(l))
                    return l;

            return null;
        }

        private List<IObserver> _observers;
    }
}
