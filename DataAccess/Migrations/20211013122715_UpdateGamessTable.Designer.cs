﻿// <auto-generated />
using System;
using DataAccess.Concret;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211013122715_UpdateGamessTable")]
    partial class UpdateGamessTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Entities.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Entities.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Entities.Models.GameCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("GameId");

                    b.ToTable("GameCategories");
                });

            modelBuilder.Entity("Entities.Models.GameDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Video")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GameId")
                        .IsUnique();

                    b.ToTable("GameDetails");
                });

            modelBuilder.Entity("Entities.Models.GameDetailPlatform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameDetailId")
                        .HasColumnType("int");

                    b.Property<int>("PlatformId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameDetailId");

                    b.HasIndex("PlatformId");

                    b.ToTable("GameDetailPlatforms");
                });

            modelBuilder.Entity("Entities.Models.Platform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("Entities.Models.Slider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sliders");
                });

            modelBuilder.Entity("Entities.Models.GameCategory", b =>
                {
                    b.HasOne("Entities.Models.Category", "Category")
                        .WithMany("GameCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.Game", "Game")
                        .WithMany("GameCategories")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Entities.Models.GameDetail", b =>
                {
                    b.HasOne("Entities.Models.Game", "Game")
                        .WithOne("GameDetail")
                        .HasForeignKey("Entities.Models.GameDetail", "GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Entities.Models.GameDetailPlatform", b =>
                {
                    b.HasOne("Entities.Models.GameDetail", "GameDetail")
                        .WithMany("GameDetailPlatforms")
                        .HasForeignKey("GameDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.Platform", "Platform")
                        .WithMany("GameDetailPlatforms")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameDetail");

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("Entities.Models.Category", b =>
                {
                    b.Navigation("GameCategories");
                });

            modelBuilder.Entity("Entities.Models.Game", b =>
                {
                    b.Navigation("GameCategories");

                    b.Navigation("GameDetail");
                });

            modelBuilder.Entity("Entities.Models.GameDetail", b =>
                {
                    b.Navigation("GameDetailPlatforms");
                });

            modelBuilder.Entity("Entities.Models.Platform", b =>
                {
                    b.Navigation("GameDetailPlatforms");
                });
#pragma warning restore 612, 618
        }
    }
}
