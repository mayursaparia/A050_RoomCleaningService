//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_Admin_Prac.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Service
    {
        public int OrderId { get; set; }
        public int RoomCount { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public int TimeSlot { get; set; }
        public string ContactNumber { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<bool> Status_Admin { get; set; }
        public Nullable<bool> Status_Cleaner { get; set; }
        public string Service_Status { get; set; }
        public string Cleaner_Id { get; set; }
        public Nullable<bool> Payment { get; set; }
        public string UserEmail { get; set; }
    }
}