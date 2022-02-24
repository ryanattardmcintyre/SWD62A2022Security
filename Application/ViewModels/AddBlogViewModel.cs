using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.ViewModels
{
    public class AddBlogViewModel
    {
        [Required(AllowEmptyStrings =false, ErrorMessage ="Blog name cannot be left blank")]
        [RegularExpression(@"^[a-zA-Z\s]*", ErrorMessage ="Blog name should contain only alphabets")]

        public string Name { get; set; }
        public string LogoImagePath { get; set; }
       
        [Required(ErrorMessage ="Category needs to be selected")]
        
        public int CategoryId { get; set; }

    }
}
