﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieDatabase.Context;

namespace MovieDatabase.Migrations
{
    [DbContext(typeof(MovieContext))]
    [Migration("20210325024727_InsertRatings0")]
    partial class InsertRatings0
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MovieDatabase.DataModels.Genre", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.Movie", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.MovieGenre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("GenreId")
                        .HasColumnType("bigint");

                    b.Property<long?>("MovieId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieGenres");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.Occupation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Occupations");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Age")
                        .HasColumnType("bigint");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("OccupationId")
                        .HasColumnType("bigint");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OccupationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.UserMovie", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("MovieId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("RatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("Rating")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMovies");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.MovieGenre", b =>
                {
                    b.HasOne("MovieDatabase.DataModels.Genre", "Genre")
                        .WithMany("MovieGenres")
                        .HasForeignKey("GenreId");

                    b.HasOne("MovieDatabase.DataModels.Movie", "Movie")
                        .WithMany("MovieGenres")
                        .HasForeignKey("MovieId");

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.User", b =>
                {
                    b.HasOne("MovieDatabase.DataModels.Occupation", "Occupation")
                        .WithMany()
                        .HasForeignKey("OccupationId");

                    b.Navigation("Occupation");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.UserMovie", b =>
                {
                    b.HasOne("MovieDatabase.DataModels.Movie", "Movie")
                        .WithMany("UserMovies")
                        .HasForeignKey("MovieId");

                    b.HasOne("MovieDatabase.DataModels.User", "User")
                        .WithMany("UserMovies")
                        .HasForeignKey("UserId");

                    b.Navigation("Movie");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.Genre", b =>
                {
                    b.Navigation("MovieGenres");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.Movie", b =>
                {
                    b.Navigation("MovieGenres");

                    b.Navigation("UserMovies");
                });

            modelBuilder.Entity("MovieDatabase.DataModels.User", b =>
                {
                    b.Navigation("UserMovies");
                });
#pragma warning restore 612, 618
        }
    }
}
