using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P013EStore.Core.Entities
{
	public class AppLog : IEntity
	{
        public int Id { get; set; }
		[Display(Name = "Hata Başlığı")]

		public string Title { get; set; }
		[Display(Name = "Hata Açıklaması")]
		public string Description { get; set; }
		[Display(Name = "Oluşma Tarihi"), ScaffoldColumn(false)]
		public DateTime? CreateDate { get; set; } = DateTime.Now;
    }
}
