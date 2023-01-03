using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Zdm_management.ViewModel.CustomValidation
{
    public class IPAddressValidation : ValidationAttribute
    {

        protected override ValidationResult IsValid(
        object? value, ValidationContext validationContext)
        {
            IPAddress? ip;
            string newip = value as string;
            if (newip != null)
            {
                bool ValidateIP = IPAddress.TryParse(newip, out ip);
                if (!ValidateIP)
                {
                    return new ValidationResult(GetErrorMessage());
                }
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage());
        }

        public string GetErrorMessage()
        {
            return "Invalid Ip Address!";
        }
    }
}
