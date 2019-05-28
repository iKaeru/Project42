﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Models.Migrations
{
    [DbContext(typeof(PostgreContext))]
    partial class PostgreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Models.CardItem.CardContent", b =>
                {
                    b.Property<string>("Text")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Text");

                    b.Property<string>("Code")
                        .HasColumnName("Code");

                    b.HasKey("Text");

                    b.ToTable("CardsContent");
                });

            modelBuilder.Entity("Models.CardItem.CardItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("AnswerText");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("CreationDate");

                    b.Property<string>("QuestionText");

                    b.Property<Guid>("UserId")
                        .HasColumnName("User");

                    b.HasKey("Id");

                    b.HasIndex("AnswerText");

                    b.HasIndex("QuestionText");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Models.CardsCollection.CardsCollection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnName("Date");

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasMaxLength(20);

                    b.Property<Guid>("UserId")
                        .HasColumnName("User");

                    b.HasKey("Id");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("Models.Training.Training", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Box")
                        .HasColumnName("MemorizationLevel");

                    b.Property<Guid>("CardId")
                        .HasColumnName("Card");

                    b.Property<DateTime>("CompletedAt")
                        .HasColumnName("CompletionDate");

                    b.Property<Guid>("UserId")
                        .HasColumnName("User");

                    b.HasKey("Id");

                    b.ToTable("Training");
                });

            modelBuilder.Entity("Models.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAdress");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Login");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Models.CardItem.CardItem", b =>
                {
                    b.HasOne("Models.CardItem.CardContent", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerText");

                    b.HasOne("Models.CardItem.CardContent", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionText");
                });
#pragma warning restore 612, 618
        }
    }
}
