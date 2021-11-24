﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sopheon.CloudNative.Products.Domain;

#nullable disable

namespace Sopheon.CloudNative.Products.DataAccess.Migrations
{
    [DbContext(typeof(ProductManagementContext))]
    partial class ProductManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("SPM")
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Attribute", b =>
                {
                    b.Property<int>("AttributeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttributeId"), 1L, 1);

                    b.Property<int>("AttributeValueTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AttributeId");

                    b.HasIndex("AttributeValueTypeId");

                    b.ToTable("Attributes", "SPM");

                    b.HasData(
                        new
                        {
                            AttributeId = -1,
                            AttributeValueTypeId = 2,
                            Name = "Industry",
                            ShortName = "IND"
                        });
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.AttributeValueType", b =>
                {
                    b.Property<int>("AttributeValueTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttributeValueTypeId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AttributeValueTypeId");

                    b.ToTable("AttributeValueType", "SPM");

                    b.HasData(
                        new
                        {
                            AttributeValueTypeId = 1,
                            Name = "String"
                        },
                        new
                        {
                            AttributeValueTypeId = 2,
                            Name = "Int32"
                        },
                        new
                        {
                            AttributeValueTypeId = 3,
                            Name = "Decimal"
                        },
                        new
                        {
                            AttributeValueTypeId = 4,
                            Name = "Money"
                        },
                        new
                        {
                            AttributeValueTypeId = 5,
                            Name = "UtcDateTime"
                        },
                        new
                        {
                            AttributeValueTypeId = 6,
                            Name = "MarkdownString"
                        });
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.FileAttachment", b =>
                {
                    b.Property<int>("FileAttachmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FileAttachmentId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("FileAttachmentId");

                    b.HasIndex("ProductId");

                    b.ToTable("FileAttachment", "SPM");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Goal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Goal", "SPM");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.KeyPerformanceIndicator", b =>
                {
                    b.Property<int>("KeyPerformanceIndicatorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KeyPerformanceIndicatorId"), 1L, 1);

                    b.Property<int>("AttributeId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("KeyPerformanceIndicatorId");

                    b.HasIndex("AttributeId");

                    b.HasIndex("ProductId");

                    b.ToTable("KeyPerformanceIndicator", "SPM");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Products", "SPM");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.ProductItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("ProductItemTypeId")
                        .HasColumnType("int");

                    b.Property<int>("RankId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProductItemTypeId");

                    b.HasIndex("RankId");

                    b.ToTable("ProductItem", "SPM");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.ProductItemType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductItemType", "SPM");

                    b.HasData(
                        new
                        {
                            Id = -3,
                            Name = "Risk"
                        },
                        new
                        {
                            Id = -2,
                            Name = "Feature"
                        },
                        new
                        {
                            Id = -1,
                            Name = "Task"
                        });
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Rank", b =>
                {
                    b.Property<int>("RankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RankId"), 1L, 1);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RankId");

                    b.ToTable("Rank", "SPM");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Release", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Release", "SPM");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Status", "SPM");

                    b.HasData(
                        new
                        {
                            Id = -2,
                            Name = "Closed"
                        },
                        new
                        {
                            Id = -1,
                            Name = "Open"
                        });
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.UrlLink", b =>
                {
                    b.Property<int>("UrlLinkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UrlLinkId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("UrlLinkId");

                    b.HasIndex("ProductId");

                    b.ToTable("UrlLink", "SPM");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Attribute", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Products.Domain.AttributeValueType", "AttributeValueType")
                        .WithMany()
                        .HasForeignKey("AttributeValueTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AttributeValueType");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.FileAttachment", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Products.Domain.Product", null)
                        .WithMany("FileAttachments")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Goal", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Products.Domain.Product", null)
                        .WithMany("Goals")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.KeyPerformanceIndicator", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sopheon.CloudNative.Products.Domain.Product", null)
                        .WithMany("KeyPerformanceIndicators")
                        .HasForeignKey("ProductId");

                    b.Navigation("Attribute");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Product", b =>
                {
                    b.OwnsMany("Sopheon.CloudNative.Products.Domain.DecimalAttributeValue", "DecimalAttributeValues", b1 =>
                        {
                            b1.Property<int>("ProductId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<int>("AttributeId")
                                .HasColumnType("int");

                            b1.Property<decimal?>("Value")
                                .HasColumnType("decimal(18,2)");

                            b1.HasKey("ProductId", "Id");

                            b1.HasIndex("AttributeId");

                            b1.ToTable("Products_DecimalAttributeValues", "SPM");

                            b1.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                                .WithMany()
                                .HasForeignKey("AttributeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("ProductId");

                            b1.Navigation("Attribute");
                        });

                    b.OwnsMany("Sopheon.CloudNative.Products.Domain.Int32AttributeValue", "IntAttributeValues", b1 =>
                        {
                            b1.Property<int>("ProductId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<int>("AttributeId")
                                .HasColumnType("int");

                            b1.Property<int?>("Value")
                                .HasColumnType("int");

                            b1.HasKey("ProductId", "Id");

                            b1.HasIndex("AttributeId");

                            b1.ToTable("Products_IntAttributeValues", "SPM");

                            b1.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                                .WithMany()
                                .HasForeignKey("AttributeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("ProductId");

                            b1.Navigation("Attribute");
                        });

                    b.OwnsMany("Sopheon.CloudNative.Products.Domain.MoneyAttributeValue", "MoneyAttributeValues", b1 =>
                        {
                            b1.Property<int>("ProductId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<int>("AttributeId")
                                .HasColumnType("int");

                            b1.HasKey("ProductId", "Id");

                            b1.HasIndex("AttributeId");

                            b1.ToTable("Products_MoneyAttributeValues", "SPM");

                            b1.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                                .WithMany()
                                .HasForeignKey("AttributeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("ProductId");

                            b1.OwnsOne("Sopheon.CloudNative.Products.Domain.MoneyValue", "Value", b2 =>
                                {
                                    b2.Property<int>("MoneyAttributeValueProductId")
                                        .HasColumnType("int");

                                    b2.Property<int>("MoneyAttributeValueId")
                                        .HasColumnType("int");

                                    b2.Property<string>("CurrencyCode")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)")
                                        .HasColumnName("CurrencyCode");

                                    b2.Property<decimal?>("Value")
                                        .HasColumnType("decimal(18,2)")
                                        .HasColumnName("Value");

                                    b2.HasKey("MoneyAttributeValueProductId", "MoneyAttributeValueId");

                                    b2.ToTable("Products_MoneyAttributeValues", "SPM");

                                    b2.WithOwner()
                                        .HasForeignKey("MoneyAttributeValueProductId", "MoneyAttributeValueId");
                                });

                            b1.Navigation("Attribute");

                            b1.Navigation("Value")
                                .IsRequired();
                        });

                    b.OwnsMany("Sopheon.CloudNative.Products.Domain.StringAttributeValue", "StringAttributeValues", b1 =>
                        {
                            b1.Property<int>("ProductId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<int>("AttributeId")
                                .HasColumnType("int");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ProductId", "Id");

                            b1.HasIndex("AttributeId");

                            b1.ToTable("Products_StringAttributeValues", "SPM");

                            b1.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                                .WithMany()
                                .HasForeignKey("AttributeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("ProductId");

                            b1.Navigation("Attribute");
                        });

                    b.OwnsMany("Sopheon.CloudNative.Products.Domain.UtcDateTimeAttributeValue", "UtcDateTimeAttributeValues", b1 =>
                        {
                            b1.Property<int>("ProductId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<int>("AttributeId")
                                .HasColumnType("int");

                            b1.Property<DateTime?>("Value")
                                .HasColumnType("datetime2");

                            b1.HasKey("ProductId", "Id");

                            b1.HasIndex("AttributeId");

                            b1.ToTable("Products_UtcDateTimeAttributeValues", "SPM");

                            b1.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                                .WithMany()
                                .HasForeignKey("AttributeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("ProductId");

                            b1.Navigation("Attribute");
                        });

                    b.Navigation("DecimalAttributeValues");

                    b.Navigation("IntAttributeValues");

                    b.Navigation("MoneyAttributeValues");

                    b.Navigation("StringAttributeValues");

                    b.Navigation("UtcDateTimeAttributeValues");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.ProductItem", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Products.Domain.Product", null)
                        .WithMany("Items")
                        .HasForeignKey("ProductId");

                    b.HasOne("Sopheon.CloudNative.Products.Domain.ProductItemType", "ProductItemType")
                        .WithMany()
                        .HasForeignKey("ProductItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sopheon.CloudNative.Products.Domain.Rank", "Rank")
                        .WithMany()
                        .HasForeignKey("RankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Sopheon.CloudNative.Products.Domain.DecimalAttributeValue", "DecimalAttributeValues", b1 =>
                        {
                            b1.Property<int>("ProductItemId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<int>("AttributeId")
                                .HasColumnType("int");

                            b1.Property<decimal?>("Value")
                                .HasColumnType("decimal(18,2)");

                            b1.HasKey("ProductItemId", "Id");

                            b1.HasIndex("AttributeId");

                            b1.ToTable("ProductItem_DecimalAttributeValues", "SPM");

                            b1.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                                .WithMany()
                                .HasForeignKey("AttributeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("ProductItemId");

                            b1.Navigation("Attribute");
                        });

                    b.OwnsMany("Sopheon.CloudNative.Products.Domain.Int32AttributeValue", "IntAttributeValues", b1 =>
                        {
                            b1.Property<int>("ProductItemId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<int>("AttributeId")
                                .HasColumnType("int");

                            b1.Property<int?>("Value")
                                .HasColumnType("int");

                            b1.HasKey("ProductItemId", "Id");

                            b1.HasIndex("AttributeId");

                            b1.ToTable("ProductItem_IntAttributeValues", "SPM");

                            b1.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                                .WithMany()
                                .HasForeignKey("AttributeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("ProductItemId");

                            b1.Navigation("Attribute");
                        });

                    b.OwnsMany("Sopheon.CloudNative.Products.Domain.MoneyAttributeValue", "MoneyAttributeValues", b1 =>
                        {
                            b1.Property<int>("ProductItemId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<int>("AttributeId")
                                .HasColumnType("int");

                            b1.HasKey("ProductItemId", "Id");

                            b1.HasIndex("AttributeId");

                            b1.ToTable("ProductItem_MoneyAttributeValues", "SPM");

                            b1.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                                .WithMany()
                                .HasForeignKey("AttributeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("ProductItemId");

                            b1.OwnsOne("Sopheon.CloudNative.Products.Domain.MoneyValue", "Value", b2 =>
                                {
                                    b2.Property<int>("MoneyAttributeValueProductItemId")
                                        .HasColumnType("int");

                                    b2.Property<int>("MoneyAttributeValueId")
                                        .HasColumnType("int");

                                    b2.Property<string>("CurrencyCode")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)")
                                        .HasColumnName("CurrencyCode");

                                    b2.Property<decimal?>("Value")
                                        .HasColumnType("decimal(18,2)")
                                        .HasColumnName("Value");

                                    b2.HasKey("MoneyAttributeValueProductItemId", "MoneyAttributeValueId");

                                    b2.ToTable("ProductItem_MoneyAttributeValues", "SPM");

                                    b2.WithOwner()
                                        .HasForeignKey("MoneyAttributeValueProductItemId", "MoneyAttributeValueId");
                                });

                            b1.Navigation("Attribute");

                            b1.Navigation("Value")
                                .IsRequired();
                        });

                    b.OwnsMany("Sopheon.CloudNative.Products.Domain.StringAttributeValue", "StringAttributeValues", b1 =>
                        {
                            b1.Property<int>("ProductItemId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<int>("AttributeId")
                                .HasColumnType("int");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ProductItemId", "Id");

                            b1.HasIndex("AttributeId");

                            b1.ToTable("ProductItem_StringAttributeValues", "SPM");

                            b1.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                                .WithMany()
                                .HasForeignKey("AttributeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("ProductItemId");

                            b1.Navigation("Attribute");
                        });

                    b.OwnsMany("Sopheon.CloudNative.Products.Domain.UtcDateTimeAttributeValue", "UtcDateTimeAttributeValues", b1 =>
                        {
                            b1.Property<int>("ProductItemId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<int>("AttributeId")
                                .HasColumnType("int");

                            b1.Property<DateTime?>("Value")
                                .HasColumnType("datetime2");

                            b1.HasKey("ProductItemId", "Id");

                            b1.HasIndex("AttributeId");

                            b1.ToTable("ProductItem_UtcDateTimeAttributeValues", "SPM");

                            b1.HasOne("Sopheon.CloudNative.Products.Domain.Attribute", "Attribute")
                                .WithMany()
                                .HasForeignKey("AttributeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("ProductItemId");

                            b1.Navigation("Attribute");
                        });

                    b.Navigation("DecimalAttributeValues");

                    b.Navigation("IntAttributeValues");

                    b.Navigation("MoneyAttributeValues");

                    b.Navigation("ProductItemType");

                    b.Navigation("Rank");

                    b.Navigation("StringAttributeValues");

                    b.Navigation("UtcDateTimeAttributeValues");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Release", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Products.Domain.Product", null)
                        .WithMany("Releases")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.UrlLink", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Products.Domain.Product", null)
                        .WithMany("UrlLinks")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Products.Domain.Product", b =>
                {
                    b.Navigation("FileAttachments");

                    b.Navigation("Goals");

                    b.Navigation("Items");

                    b.Navigation("KeyPerformanceIndicators");

                    b.Navigation("Releases");

                    b.Navigation("UrlLinks");
                });
#pragma warning restore 612, 618
        }
    }
}
