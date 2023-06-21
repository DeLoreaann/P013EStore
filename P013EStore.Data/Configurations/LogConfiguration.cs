using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P013EStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P013EStore.Data.Configurations
{
	public class LogConfiguration : IEntityTypeConfiguration<AppLog>
	{
		public void Configure(EntityTypeBuilder<AppLog> builder)
		{
			builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
		}
	}
}
