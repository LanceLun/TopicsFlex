﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EFModels.Models
{
    public partial class ReservationStatus
    {
        public ReservationStatus()
        {
            OneToOneReservations = new HashSet<OneToOneReservation>();
        }

        public int ReservationId { get; set; }
        public string ReservationStatusDescription { get; set; }

        public virtual ICollection<OneToOneReservation> OneToOneReservations { get; set; }
    }
}