﻿using FlexCoreService.CustomeShoes.Models.Dtos;

namespace FlexCoreService.CustomeShoes.Models.VMs
{
	public class CustomeShoesVM
	{
		public int? ShoesProductId { get; set; }

		public string? ShoesName { get; set; }

        public int? ShoesCategoryId { get; set; }

        public string? ShoesCategoryName { get; set; }

		public int? ShoesUnitPrice { get; set; }

		public string? FirstImgPath { get; set; }

        public IEnumerable<ShoesColorCategoriesDto> ShoesColors { get; set; }

        public IEnumerable<ShoesOptionsDto> ShoesOptions { get; set; }

        public IEnumerable<CustomizedMaterialsDto> ShoesMaterials { get; set; }

		public string Remark { get; set; }

		public CustomeShoesVM()
        {
            ShoesColors = new List<ShoesColorCategoriesDto>();
            ShoesOptions = new List<ShoesOptionsDto>();
            ShoesMaterials = new List<CustomizedMaterialsDto>();
        }
    }
}
