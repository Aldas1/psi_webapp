﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuizAppApi.Data;

#nullable disable

namespace QuizAppApi.Data.Migrations
{
    [DbContext(typeof(QuizContext))]
    [Migration("20231113111354_AddComments")]
    partial class AddComments
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuizAppApi.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("QuizId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CommentId");

                    b.HasIndex("QuizId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("QuizAppApi.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuizId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("Questions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Question");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("QuizAppApi.Models.Questions.Option", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<bool>("Correct")
                        .HasColumnType("bit");

                    b.HasKey("Name", "QuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Options");
                });

            modelBuilder.Entity("QuizAppApi.Models.Quiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfSubmitters")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("QuizAppApi.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("NumberOfSubmissions")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalScore")
                        .HasColumnType("int");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("QuizAppApi.Models.Questions.OpenTextQuestion", b =>
                {
                    b.HasBaseType("QuizAppApi.Models.Question");

                    b.Property<string>("CorrectAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("OpenTextQuestion");
                });

            modelBuilder.Entity("QuizAppApi.Models.Questions.OptionQuestion", b =>
                {
                    b.HasBaseType("QuizAppApi.Models.Question");

                    b.HasDiscriminator().HasValue("OptionQuestion");
                });

            modelBuilder.Entity("QuizAppApi.Models.Questions.MultipleChoiceQuestion", b =>
                {
                    b.HasBaseType("QuizAppApi.Models.Questions.OptionQuestion");

                    b.HasDiscriminator().HasValue("MultipleChoiceQuestion");
                });

            modelBuilder.Entity("QuizAppApi.Models.Questions.SingleChoiceQuestion", b =>
                {
                    b.HasBaseType("QuizAppApi.Models.Questions.OptionQuestion");

                    b.HasDiscriminator().HasValue("SingleChoiceQuestion");
                });

            modelBuilder.Entity("QuizAppApi.Models.Comment", b =>
                {
                    b.HasOne("QuizAppApi.Models.Quiz", null)
                        .WithMany("Comments")
                        .HasForeignKey("QuizId");
                });

            modelBuilder.Entity("QuizAppApi.Models.Question", b =>
                {
                    b.HasOne("QuizAppApi.Models.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("QuizAppApi.Models.Questions.Option", b =>
                {
                    b.HasOne("QuizAppApi.Models.Questions.OptionQuestion", "Question")
                        .WithMany("Options")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("QuizAppApi.Models.Quiz", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("QuizAppApi.Models.Questions.OptionQuestion", b =>
                {
                    b.Navigation("Options");
                });
#pragma warning restore 612, 618
        }
    }
}
