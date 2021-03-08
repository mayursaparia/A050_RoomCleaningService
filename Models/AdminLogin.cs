using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Project_Admin_Prac.Models
{
    public class AdminLogin
    {
        [Display(Name ="Admin ID")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Admin ID required")]
        public string AdminID { set; get; }

        [Required(AllowEmptyStrings =false,ErrorMessage ="Password Required")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Minimum Length 6 characters")]
        public string Password { set; get; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { set; get; }
    }
}