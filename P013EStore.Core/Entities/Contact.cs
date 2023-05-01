using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace P013EStore.Core.Entities
{
	public class Contact : IEntity
	{
        public int Id { get; set; }
		[Display(Name = "Ad")]
		public string Name { get; set; }
		[Display(Name = "Soyad")]
		public string? Surname { get; set; }
		public string Email { get; set; }
		[Display(Name = "Telefon")]
		public string? Phone { get; set; }
		[Display(Name = "Mesaj")]
		public string Message { get; set; }
        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)]
		public DateTime? CreateDate { get; set; } = DateTime.Now;
	}
}
