﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20221222130101_AddDeliveryPoint")]
    partial class AddDeliveryPoint
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.HasSequence<int>("ProductPrice", "sequences")
                .StartsAt(100L)
                .IncrementsBy(33)
                .HasMin(10L)
                .HasMax(300L)
                .IsCyclic();

            modelBuilder.Entity("Models.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("s_Name");

                    b.HasKey("Id");

                    b.ToTable("Manufacturer");
                });

            modelBuilder.Entity("Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("DateTime")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime2");

                    b.Property<Point>("DeliveryPoint")
                        .HasColumnType("geography");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Models.OrdersSummary", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToView("View_OrdersSummary");
                });

            modelBuilder.Entity("Models.Person", b =>
                {
                    b.Property<long?>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long?>("Key"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("s_Name");

                    b.HasKey("Key");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Info")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("s_Info")
                        .HasComputedColumnSql("[s_Name] + ' ' + Str([Price]) + 'zł'", true);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int?>("ManufacturerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("s_Name");

                    b.Property<float>("Price")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("real")
                        .HasDefaultValueSql("NEXT VALUE FOR sequences.ProductPrice");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Models.ShadowId", b =>
                {
                    b.Property<int>("Ident")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Ident"), 1L, 1);

                    b.HasKey("Ident");

                    b.ToTable("ShadowId");
                });

            modelBuilder.Entity("OrderProduct", b =>
                {
                    b.Property<int>("OrdersId")
                        .HasColumnType("int");

                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.HasKey("OrdersId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("OrderProduct");
                });

            modelBuilder.Entity("Models.Product", b =>
                {
                    b.HasOne("Models.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId");

                    b.Navigation("Manufacturer");
                });

            modelBuilder.Entity("OrderProduct", b =>
                {
                    b.HasOne("Models.Order", null)
                        .WithMany()
                        .HasForeignKey("OrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
