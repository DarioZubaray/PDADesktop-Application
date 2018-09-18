﻿using System.Globalization;
using System.Windows.Controls;

namespace PDADesktop.Classes
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "El campo es requerido.")
                : ValidationResult.ValidResult;
        }
    }
}
