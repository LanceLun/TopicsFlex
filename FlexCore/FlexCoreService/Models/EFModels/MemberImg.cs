﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FlexCoreService.Models.EFModels
{
    public partial class MemberImg
    {
        public int Id { get; set; }
        public int fk_memberId { get; set; }
        public string ImgPath { get; set; }

        public virtual Member fk_member { get; set; }
    }
}