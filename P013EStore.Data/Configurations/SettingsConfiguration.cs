using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P013EStore.Core.Entities;

namespace P013EStore.Data.Configurations
{
	internal class SettingsConfiguration : IEntityTypeConfiguration<Settings>
	{
		public void Configure(EntityTypeBuilder<Settings> builder)
		{
			builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
			builder.Property(x => x.Logo).HasMaxLength(50);
			builder.Property(x => x.Favicon).HasMaxLength(50);
			builder.Property(x => x.Description).HasMaxLength(500);
			builder.Property(x => x.Email).HasMaxLength(100);
			builder.Property(x => x.MailServer).HasMaxLength(100);
			builder.Property(x => x.Phone).HasMaxLength(20);
			builder.Property(x => x.Description).HasMaxLength(20);
			builder.Property(x => x.UserName).HasMaxLength(100);
		}
	}
}
