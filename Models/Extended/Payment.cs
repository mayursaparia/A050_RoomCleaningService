using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_Admin_Prac.Models
{
    [MetadataType(typeof(PaymentMetadata))]
    public partial class Payment
    {
    }

    public class PaymentMetadata
    {
        public int Id { get; set; }

        [Display(Name = "Card Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Card Number Required")]
        //[MinLength(16, ErrorMessage = "Minimum 16 characters")]
        //[DataType(DataType.CreditCard)]
        public string cardNumber { get; set; }

        [Display(Name = "Month Expire")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Month Expiry Required")]
        
        public Nullable<int> ExpMonth { get; set; }

        [Display(Name = "Year Expire")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Year Expiry Required")]
        public Nullable<int> ExpYear { get; set; }

        [Display(Name = "CVV")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "CVV Required")]
        //[MinLength(3, ErrorMessage = "Enter Valid CVV")]
        //[MaxLength(3, ErrorMessage = "Enter Valid CVV")]
        public Nullable<int> cvv { get; set; }

        [Display(Name = "Name on Card")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Card Name Required")]
        public string name { get; set; }
        public Nullable<double> amount { get; set; }
        public string Method { get; set; }

    }
}