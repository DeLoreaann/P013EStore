using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace P013EStore.Core.Entities
{
	public class Brand : IEntity
	{
        public int Id { get; set; }
		[Display(Name = "Ad")]
		public string? Name { get; set; }
		[Display(Name = "Açıklama")]
		public string? Description { get; set; }
        public string? Logo { get; set; }
		[Display(Name = "Aktif?")]
		public bool IsActive { get; set; }
		[Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)]
		public DateTime? CreateDate { get; set; } = DateTime.Now;
		public List<Product>? Products { get; set; }
	}
}
