

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P013EStore.Core.Entities;
using System.Security.Cryptography.X509Certificates;

namespace P013EStore.Data.Configurations
{
	internal class ContactConfigurations : IEntityTypeConfiguration<Contact>
	{
		public void Configure(EntityTypeBuilder<Contact> builder)
		{
			builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
			builder.Property(x => x.Surname).HasMaxLength(50);
			builder.Property(x => x.Email).HasMaxLength(50);
			builder.Property(x => x.Phone).HasMaxLength(20);
			builder.Property(x => x.Message).HasMaxLength(500);
		}
	}
}
