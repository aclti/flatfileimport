﻿using FlatFileImport.Data;

namespace FlatFileImport
{
    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void UnRegisterObserver(IObserver observer);
        void NotifyObservers(ParsedData parsedData);
    }
}
