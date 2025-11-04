
namespace Financeira.DTO
{
    using System;
    using System.ComponentModel.DataAnnotations;

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