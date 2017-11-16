using System.Collections.Generic;
using FluentValidation.Results;
using System.Linq;

namespace <%= ProjectName %>.Validators
{
    public static class Extensions
    {
        public static string ToText(this IList<ValidationFailure> errors)
        {
            return string.Join("; ", errors.Select(e => e.ErrorMessage));
        }
    }
}