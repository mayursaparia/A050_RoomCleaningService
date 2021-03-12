using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Project_Admin_Prac.Models
{
    [MetadataType(typeof(ServiceMetadata))]
    public partial class Service
    {
    }

    public class ServiceMetadata
    {
        [Display(Name = "Room Count")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Room Count Required")]
        public int RoomCount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Address Required")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Location Required")]
        public string Location { get; set; }

        [Display(Name = "Time Slot")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Time Slot Required")]
        public int TimeSlot { get; set; }

        [Display(Name = "Contact Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contact NUmber Required")]
        [MinLength(10, ErrorMessage = "Minimum 10 characters")]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Date required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

    }
}