using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.SaleNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.SaleDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.Property(s => s.TotalAmount)
                .IsRequired()
                .HasColumnType("numeric");

            builder.Property(s => s.IsCancelled)
                .IsRequired()
                .HasColumnType("boolean");

            builder.HasOne(s => s.Customer)
                .WithMany() 
                .HasForeignKey(s => s.CustomerId) 
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Branch)
                .WithMany() 
                .HasForeignKey(s => s.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.SaleItens)
                .WithOne(si => si.Sale)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
