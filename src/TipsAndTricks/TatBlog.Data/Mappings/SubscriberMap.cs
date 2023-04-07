using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
    public class SubscriberMap
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {

            builder.ToTable("Subscribers");


            builder.HasKey(s => s.Id);

            builder.Property(s => s.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.SubDated)
                   .IsRequired()
                   .HasColumnType("datetime");

            builder.Property(s => s.UnsubDated)
                   .HasColumnType("datetime");

            builder.Property(s => s.CancelReason)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(s => s.Voluntary)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(s => s.AdminNotes)
                   .IsRequired()
                   .HasMaxLength(1000);
        }
    }
}
