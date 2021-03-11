using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Project_Admin_Prac.Models
{
    [MetadataType(typeof(CleanerMetadata))]
    public partial class Cleaner
    {
        public string ConfirmPassword { set; get; }
    }
    public class CleanerMetadata
    {
        [Display(Name = "First Name")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "only characters allowed")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "only characters allowed")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name required")]
        public string LastName { get; set; }

        [Display(Name = "Cleaner ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cleaner ID required")]
        public string CleanerId { get; set; }

        [Display(Name = "Gender(Male/Female/Other)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gender required")]
        public string Gender { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "Date Of Birth required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DOB { get; set; }

        [Display(Name = "Contact Number")]
        [Required(AllowEmptyStrings = false,ErrorMessage = "Contact NUmber Required")]
        [MinLength(10,ErrorMessage ="Minimum 10 characters")]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; } 

        [Display(Name = "Location")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "only characters allowed")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Location required")]
        public string Location { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 character required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password and Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}