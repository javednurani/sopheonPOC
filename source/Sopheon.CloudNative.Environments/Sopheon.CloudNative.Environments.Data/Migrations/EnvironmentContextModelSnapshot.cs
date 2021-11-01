﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sopheon.CloudNative.Environments.Data;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    [DbContext(typeof(EnvironmentContext))]
    partial class EnvironmentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("ENV")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.BusinessService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("BusinessServiceId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("BusinessServices");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "ProductManagement"
                        });
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.BusinessServiceDependency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("BusinessServiceDependencyId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BusinessServiceId")
                        .HasColumnType("int");

                    b.Property<string>("DependencyName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("DomainResourceTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DomainResourceTypeId");

                    b.HasIndex("BusinessServiceId", "DependencyName")
                        .IsUnique();

                    b.ToTable("BusinessServiceDependencies");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.DedicatedEnvironmentResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DedicatedEnvironmentResourceId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EnvironmentId")
                        .HasColumnType("int");

                    b.Property<int>("ResourceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EnvironmentId");

                    b.HasIndex("ResourceId")
                        .IsUnique();

                    b.ToTable("DedicatedEnvironmentResources");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.DomainResourceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DomainResourceTypeId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDedicated")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("DomainResourceTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDedicated = true,
                            Name = "AzureSqlDb"
                        },
                        new
                        {
                            Id = 2,
                            IsDedicated = false,
                            Name = "AzureBlobStorage"
                        });
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.Environment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EnvironmentId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<Guid>("EnvironmentKey")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("Owner")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("EnvironmentKey")
                        .IsUnique();

                    b.ToTable("Environments");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.EnvironmentResourceBinding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EnvironmentResourceBindingId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BusinessServiceDependencyId")
                        .HasColumnType("int");

                    b.Property<int>("EnvironmentId")
                        .HasColumnType("int");

                    b.Property<int>("ResourceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BusinessServiceDependencyId");

                    b.HasIndex("ResourceId");

                    b.HasIndex("EnvironmentId", "BusinessServiceDependencyId")
                        .IsUnique();

                    b.ToTable("EnvironmentResourceBindings");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ResourceId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DomainResourceTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DomainResourceTypeId");

                    b.HasIndex("Uri")
                        .IsUnique();

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.BusinessServiceDependency", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Environments.Domain.Models.BusinessService", "BusinessService")
                        .WithMany("BusinessServiceDependencies")
                        .HasForeignKey("BusinessServiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Sopheon.CloudNative.Environments.Domain.Models.DomainResourceType", "DomainResourceType")
                        .WithMany("BusinessServiceDependencies")
                        .HasForeignKey("DomainResourceTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BusinessService");

                    b.Navigation("DomainResourceType");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.DedicatedEnvironmentResource", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Environments.Domain.Models.Environment", "Environment")
                        .WithMany("DedicatedEnvironmentResources")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Sopheon.CloudNative.Environments.Domain.Models.Resource", "Resource")
                        .WithOne("DedicatedEnvironmentResource")
                        .HasForeignKey("Sopheon.CloudNative.Environments.Domain.Models.DedicatedEnvironmentResource", "ResourceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Environment");

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.EnvironmentResourceBinding", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Environments.Domain.Models.BusinessServiceDependency", "BusinessServiceDependency")
                        .WithMany("EnvironmentResourceBindings")
                        .HasForeignKey("BusinessServiceDependencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Sopheon.CloudNative.Environments.Domain.Models.Environment", "Environment")
                        .WithMany("EnvironmentResourceBindings")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Sopheon.CloudNative.Environments.Domain.Models.Resource", "Resource")
                        .WithMany("EnvironmentResourceBindings")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BusinessServiceDependency");

                    b.Navigation("Environment");

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.Resource", b =>
                {
                    b.HasOne("Sopheon.CloudNative.Environments.Domain.Models.DomainResourceType", "DomainResourceType")
                        .WithMany("Resources")
                        .HasForeignKey("DomainResourceTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DomainResourceType");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.BusinessService", b =>
                {
                    b.Navigation("BusinessServiceDependencies");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.BusinessServiceDependency", b =>
                {
                    b.Navigation("EnvironmentResourceBindings");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.DomainResourceType", b =>
                {
                    b.Navigation("BusinessServiceDependencies");

                    b.Navigation("Resources");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.Environment", b =>
                {
                    b.Navigation("DedicatedEnvironmentResources");

                    b.Navigation("EnvironmentResourceBindings");
                });

            modelBuilder.Entity("Sopheon.CloudNative.Environments.Domain.Models.Resource", b =>
                {
                    b.Navigation("DedicatedEnvironmentResource");

                    b.Navigation("EnvironmentResourceBindings");
                });
#pragma warning restore 612, 618
        }
    }
}
