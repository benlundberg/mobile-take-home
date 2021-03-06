﻿using System.Collections.Generic;
using System.Globalization;

namespace GuestlogixTestXF.Core
{
    public interface ILocalizeHelper
    {
        string GetCurrentCountry();
        CultureInfo GetCurrentCultureInfo();
        void SetLocale();
        IComparer<string> CreateStringComparer(CultureInfo cultureInfo = null);
    }
}
