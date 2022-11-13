﻿// <auto-generated />
using System;
using Library.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Library.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20221113091239_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Library.Models.Authenticate.UserTokenData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<bool?>("IsActive")
                        .IsRequired()
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.Property<string>("TokenId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("token_id");

                    b.HasKey("Id");

                    b.ToTable("user_tokens");
                });

            modelBuilder.Entity("Library.Models.Book.Books", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("author");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("ImageLink")
                        .HasColumnType("text")
                        .HasColumnName("image_link");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("NameNormalise")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("normalize_name");

                    b.Property<int>("Pages")
                        .HasColumnType("integer")
                        .HasColumnName("pages");

                    b.Property<int>("PublishYear")
                        .HasColumnType("integer")
                        .HasColumnName("publish_year");

                    b.HasKey("Id");

                    b.ToTable("books");
                });

            modelBuilder.Entity("Library.Models.Othres.BookCatRelations", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("BookId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("book_id");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category_id");

                    b.HasKey("Id");

                    b.ToTable("book_cat_relations");
                });

            modelBuilder.Entity("Library.Models.Othres.Categories", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("Library.Models.Othres.Favourites", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("BookId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("book_id");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("favourites");
                });

            modelBuilder.Entity("Library.Models.Users.Users", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Library.Models.Othres.BookCatRelations", b =>
                {
                    b.HasOne("Library.Models.Book.Books", "Book")
                        .WithMany("BookCategoryRelations")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.Models.Othres.Categories", "Category")
                        .WithMany("BookCategoryRelations")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Library.Models.Othres.Favourites", b =>
                {
                    b.HasOne("Library.Models.Book.Books", "Book")
                        .WithMany("Favourites")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.Models.Users.Users", "User")
                        .WithMany("Favourites")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Library.Models.Book.Books", b =>
                {
                    b.Navigation("BookCategoryRelations");

                    b.Navigation("Favourites");
                });

            modelBuilder.Entity("Library.Models.Othres.Categories", b =>
                {
                    b.Navigation("BookCategoryRelations");
                });

            modelBuilder.Entity("Library.Models.Users.Users", b =>
                {
                    b.Navigation("Favourites");
                });
#pragma warning restore 612, 618
        }
    }
}
