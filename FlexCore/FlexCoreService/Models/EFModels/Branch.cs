﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FlexCoreService.Models.EFModels
{
    public partial class Branch
    {
        public Branch()
        {
            OneToOneReservations = new HashSet<OneToOneReservation>();
            Speakers = new HashSet<Speaker>();
        }

        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }

        public virtual ICollection<OneToOneReservation> OneToOneReservations { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; }
    }
}