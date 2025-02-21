﻿// <auto-generated />
using System;
using BTravel.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BTravel.DAL.Migrations
{
    [DbContext(typeof(BTravelDbContext))]
    [Migration("20250215152257_AddCommonUserAndContract")]
    partial class AddCommonUserAndContract
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("BTravel.DAL.Entities.CommonUser", b =>
                {
                    b.Property<int>("CommonUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("CommonUserId"));

                    b.Property<string>("BillingAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CardCVC")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CardExpDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("DefaultSignatureUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DeletedBy")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsPrimaryMailVerified")
                        .HasColumnType("bit(1)");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LastModifiedBy")
                        .HasColumnType("int");

                    b.Property<string>("NameOnCreditCard")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PrimaryMail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("CommonUserId");

                    b.HasIndex("CommonUserId")
                        .IsUnique();

                    b.ToTable("CommonUsers");
                });

            modelBuilder.Entity("BTravel.DAL.Entities.Contract", b =>
                {
                    b.Property<int>("ContractId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ContractId"));

                    b.Property<int>("CommonUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DeletedBy")
                        .HasColumnType("int");

                    b.Property<string>("HotelName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit(1)");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LastModifiedBy")
                        .HasColumnType("int");

                    b.Property<int>("NoOfNights")
                        .HasColumnType("int");

                    b.Property<int>("NoOfRooms")
                        .HasColumnType("int");

                    b.Property<decimal>("RatePerNight")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("SignatureUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("SignedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("ContractId");

                    b.HasIndex("CommonUserId");

                    b.HasIndex("ContractId")
                        .IsUnique();

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("BTravel.DAL.Entities.Contract", b =>
                {
                    b.HasOne("BTravel.DAL.Entities.CommonUser", "CommonUser")
                        .WithMany("Contracts")
                        .HasForeignKey("CommonUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CommonUser");
                });

            modelBuilder.Entity("BTravel.DAL.Entities.CommonUser", b =>
                {
                    b.Navigation("Contracts");
                });
#pragma warning restore 612, 618
        }
    }
}
