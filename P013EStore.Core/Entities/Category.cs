﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P013EStore.Core.Entities
{
	public class Category : IEntity
	{
        public int Id { get; set; }
		[Display(Name = "Kategori Adı")]
        public string? Name { get; set; }
		[Display(Name = "Kategori Açıklaması")]
		public string? Description { get; set; }
		[Display(Name = "Kategori Resmi")]
		public string? Image { get; set; }
		[Display(Name = "Kategori Aktif mi?")]
		public bool IsActive { get; set; }
		[Display(Name = "Üst Menüde Göster")]
		public bool IsTopMenu { get; set; }
		[Display(Name = "Üst Kategori")]
		public int ParentId { get; set; }
		[Display(Name = "Sıra No")]
		public int OrderNo { get; set; }
		[Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)]
		public DateTime CreateDate { get; set; } = DateTime.Now;
        public List<Product>? Products { get; set; }

    }
}
