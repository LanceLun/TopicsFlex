﻿namespace FlexCoreService.CustomeShoes.Models.Dtos
{
    public class ShoesOrderDto
    {
        public string? ShoesOrderId { get; set; }

        public int? Qty { get; set; }

        public int? fk_ShoesSizeId { get; set; }

        public string? Remark { get; set; }
    }
}
