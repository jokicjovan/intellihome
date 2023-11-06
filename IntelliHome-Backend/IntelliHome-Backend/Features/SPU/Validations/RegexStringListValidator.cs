using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IntelliHome_Backend.Features.SPU.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RegexStringListValidator : ValidationAttribute
    {
        private readonly string _pattern;

        public RegexStringListValidator(string pattern)
        {
            _pattern = pattern;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as List<string>;

            if (list == null)
            {
                return new ValidationResult($"Incorrect input (list of strings)!");
            }

            foreach (var item in list)
            {
                if (!Regex.IsMatch(item, _pattern))
                {
                    return new ValidationResult($"The element '{item}' does not match the required pattern.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
