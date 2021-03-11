using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Project_Admin_Prac.Models
{
    public class Forgotpassword
    {
        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Secret Question 1")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Answer Required")]
        public string Ques1 { get; set; }

        [Display(Name = "Secret Question 2")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Answer Required")]
        public string Ques2 { get; set; }

        [Display(Name = "Secret Question 3")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Answer Required")]
        public string Ques3 { get; set; }

        [Display(Name ="Type Your New Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "No Password Entered")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Minimum Length shoule be 6 characters")]
        public string NewPassword { get; set; }
    }
}