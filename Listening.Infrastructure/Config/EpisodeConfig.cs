using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listening.Domain.Entities;

namespace Listening.Infrastructure.Config
{
    public class EpisodeConfig : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.ToTable("T_Episode");
           
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.AlbumId, e.IsDel });
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Title).HasMaxLength(500).IsUnicode(false).IsRequired();
            builder.Property(e => e.AudioUrl).HasMaxLength(1000).IsUnicode().IsRequired();
            builder.Property(e => e.SubtitleContent).HasMaxLength(int.MaxValue).IsUnicode().IsRequired();
         
        }
    }
}
