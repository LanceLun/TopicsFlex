﻿namespace FlexCoreService.CustomeShoes.Models.Dtos
{
	public class CustomeShoesDto
	{
		public int? ShoesProductId { get; set; }

		public string? ShoesName { get; set; }

		public int? ShoesCategoryId { get; set; }

        public string? ShoesCategoryName { get; set; }

		public int? ShoesUnitPrice { get; set; }

		public string? FirstImgPath { get; set; }

		public string Remark { get; set; }

	}
}
