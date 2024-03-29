﻿using Amazon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Amazon.Repository.Data.Config
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(P => P.Name).IsRequired().HasMaxLength(100); 
            builder.Property(P => P.Description).IsRequired(); 
            builder.Property(P => P.PictureUrl).IsRequired();

            builder.HasOne(P => P.ProductBrand).WithMany()
                .HasForeignKey(P => P.ProductBrandId);

            builder.HasOne(P => P.ProductType).WithMany()
                .HasForeignKey(P => P.ProductTypeId);

            builder.Property(P => P.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
