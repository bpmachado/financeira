using System;
using System.ComponentModel.DataAnnotations;

namespace Financeira.DTO
{
    public class FutureOrPresentAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date.Date >= DateTime.Today;
            }

            return false;
        }
    }
}