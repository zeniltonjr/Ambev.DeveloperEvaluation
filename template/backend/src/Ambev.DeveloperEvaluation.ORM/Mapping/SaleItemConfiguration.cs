using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(si => si.Id);
            builder.Property(si => si.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(si => si.Quantity)
                .IsRequired()
                .HasColumnType("integer");

            builder.Property(si => si.UnitPrice)
                .IsRequired()
                .HasColumnType("numeric");

            builder.Property(si => si.Discount)
                .IsRequired()
                .HasColumnType("numeric");

            builder.Property(si => si.TotalPrice)
                .IsRequired()
                .HasColumnType("numeric");

            builder.HasOne(si => si.Product)
                .WithMany() 
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(si => si.Sale)
                .WithMany(s => s.SaleItens) 
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
