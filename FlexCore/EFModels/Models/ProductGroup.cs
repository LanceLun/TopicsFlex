﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EFModels.Models
{
    public partial class ProductGroup
    {
        public ProductGroup()
        {
            CartItems = new HashSet<CartItem>();
            ProductComments = new HashSet<ProductComment>();
        }

        public int ProductGroupId { get; set; }
        public string fk_ProductId { get; set; }
        public int fk_ColorId { get; set; }
        public int fk_SizeId { get; set; }
        public int Qty { get; set; }

        public virtual ColorCategory fk_Color { get; set; }
        public virtual Product fk_Product { get; set; }
        public virtual SizeCategory fk_Size { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<ProductComment> ProductComments { get; set; }
    }
}