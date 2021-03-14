using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Project_Admin_Prac.Models
{
    [MetadataType(typeof(FeedbackMetadata))]
    public partial class Feedback
    {
    }

    public class FeedbackMetadata
    {
        [Display(Name ="Dusting Rating")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Dusting Rating Required")]
        [Range(1, 5, ErrorMessage = "Rating should be given between(1-5)")]
        public int RatingDusting { get; set; }
        [Display(Name ="Vacuuming Rating")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vacuuming Rating Required")]
        [Range(1, 5, ErrorMessage = "Rating should be given between(1-5)")]
        public int RatingVacuuming { get; set; }
        [Display(Name ="Overall Impression")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Overall Impression Required")]
        public int OverallImpression { get; set; }
        [Display(Name ="Additional Remarks")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Additional Remarks Required")]
        public string AdditionalInformation { get; set; }
    }
}