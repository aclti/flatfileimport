﻿using System.Collections.Generic;

namespace FlatFileImport.Validate
{
    public interface IValidate
    {
        bool IsValid();
        ValidateResult Valid();
        ValidateResult ValidateResult { get; }
    }
}
