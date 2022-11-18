﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TornLogDownloader.Entities;

#nullable disable

namespace TornLogDownloader.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221118141755_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TornLogDownloader.Entities.RawLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Data")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("smalldatetime")
                        .HasComputedColumnSql("DATEADD(S, [Timestamp], '1970-01-01')");

                    b.Property<string>("LogId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("LogTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Params")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("RunApiCallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("RunApiCallId");

                    b.ToTable("RawLogs");
                });

            modelBuilder.Entity("TornLogDownloader.Entities.Run", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("Ended")
                        .HasColumnType("datetimeoffset(3)");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("Started")
                        .HasColumnType("datetimeoffset(3)");

                    b.HasKey("Id");

                    b.ToTable("Runs");
                });

            modelBuilder.Entity("TornLogDownloader.Entities.RunApiCall", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("Ended")
                        .HasColumnType("datetimeoffset(3)");

                    b.Property<Guid>("RunId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Started")
                        .HasColumnType("datetimeoffset(3)");

                    b.HasKey("Id");

                    b.HasIndex("RunId");

                    b.ToTable("RunApiCalls");
                });

            modelBuilder.Entity("TornLogDownloader.Entities.RawLog", b =>
                {
                    b.HasOne("TornLogDownloader.Entities.RunApiCall", "RunApiCall")
                        .WithMany("RawLogs")
                        .HasForeignKey("RunApiCallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RunApiCall");
                });

            modelBuilder.Entity("TornLogDownloader.Entities.RunApiCall", b =>
                {
                    b.HasOne("TornLogDownloader.Entities.Run", "Run")
                        .WithMany("RunApiCalls")
                        .HasForeignKey("RunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Run");
                });

            modelBuilder.Entity("TornLogDownloader.Entities.Run", b =>
                {
                    b.Navigation("RunApiCalls");
                });

            modelBuilder.Entity("TornLogDownloader.Entities.RunApiCall", b =>
                {
                    b.Navigation("RawLogs");
                });
#pragma warning restore 612, 618
        }
    }
}