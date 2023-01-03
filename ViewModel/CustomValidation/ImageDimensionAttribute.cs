using Microsoft.EntityFrameworkCore.Migrations;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Zdm_management.ViewModel.CustomValidation
{
    public class ImageDimensionAttribute: ValidationAttribute
    {
        private readonly int _maxwidth;
        private readonly int _maxheight;
        private readonly int _minwidth;
        private readonly int _minheight;
        private readonly string[] _defaultImageExtension = new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" };
        public ImageDimensionAttribute(int maxwidth, int maxheight, int minwidth, int minheight)
        {
            _maxwidth = maxwidth;
            _maxheight = maxheight;
            _minwidth = minwidth;
            _minheight = minheight;
        }
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var File = value as IFormFile; 
            
            if (File != null)
            {
               
                var extension = Path.GetExtension(File.FileName);
                if (!_defaultImageExtension.Contains(extension.ToLower()))
                {
                    return new ValidationResult("Upload image only");
                }

                var image = Image.FromStream(File.OpenReadStream());
                
                    if(image.Width > _maxwidth || image.Height > _maxheight || image.Width < _minwidth || image.Height < _minheight)
                    {

                        return new ValidationResult(GetErrorMessage());
                    }
                
            }

            return ValidationResult.Success;
        }
        public string GetErrorMessage()
        {
            return $"Max width {_maxwidth} px, Max height {_maxheight} px And Min width {_minwidth} px, Min height {_minheight} px.";
        }
    }
}
