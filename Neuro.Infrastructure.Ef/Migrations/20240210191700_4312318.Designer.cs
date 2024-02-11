﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Neuro.Infrastructure.Ef.Contexts;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240210191700_4312318")]
    partial class _4312318
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Neuro.Domain.Entities.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<List<string>>("ActivityImagePaths")
                        .HasColumnType("text[]");

                    b.Property<byte[]>("AlzheimerLevel")
                        .HasColumnType("smallint[]");

                    b.Property<List<string>>("Benefits")
                        .HasColumnType("text[]");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<List<string>>("Materials")
                        .HasColumnType("text[]");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("RiskGroup")
                        .HasColumnType("text");

                    b.Property<List<string>>("Steps")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("Suggestions")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("Warnings")
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.ActivityImageDescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ActivityId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("ActivityImageDescriptions");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ArticleImagePath")
                        .HasColumnType("text");

                    b.Property<string>("AuthorImagePath")
                        .HasColumnType("text");

                    b.Property<List<int>>("RecommendedArticles")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.Disease", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Diseases");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<List<string>>("Areas")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("AreasImagePath")
                        .HasColumnType("text");

                    b.Property<string>("AverageDuration")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("AverageDurationInSeconds")
                        .HasColumnType("integer");

                    b.Property<List<string>>("Benefits")
                        .HasColumnType("text[]");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("EligibleAlzheimerDegrees")
                        .IsRequired()
                        .HasColumnType("smallint[]");

                    b.Property<List<string>>("ExerciseImageUrls")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("ExerciseSteps")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("GifPath")
                        .HasColumnType("text");

                    b.Property<List<string>>("InappropriateDiseases")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("InappropriateDrugs")
                        .HasColumnType("text[]");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NumberRepetitionsSets")
                        .HasColumnType("text");

                    b.Property<List<string>>("RequiredTools")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("Suggestions")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("Warnings")
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.FoodPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Calories")
                        .HasColumnType("integer");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<List<string>>("Instructions")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<List<string>>("Materials")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<int>>("RecommendedRecipes")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("SubCategory")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("VideoPath")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FoodPages");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.Medication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Medications");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.MedicationDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<int>("UserMedicineId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserMedicineId");

                    b.ToTable("MedicationDays");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.MedicationTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("UserMedicineId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserMedicineId");

                    b.ToTable("MedicationTimes");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.RecommendedRoutine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ActivityId")
                        .HasColumnType("integer");

                    b.Property<int?>("ArticleId")
                        .HasColumnType("integer");

                    b.Property<int?>("Calories")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("DescriptionImagePath")
                        .HasColumnType("text");

                    b.Property<int?>("ExerciseId")
                        .HasColumnType("integer");

                    b.Property<int?>("FoodId")
                        .HasColumnType("integer");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<byte>("RecommendationType")
                        .HasColumnType("smallint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("ArticleId");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("FoodId");

                    b.ToTable("RecommendedRoutines");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<byte>("AlzheimerStage")
                        .HasColumnType("smallint");

                    b.Property<byte?>("BloodType")
                        .HasColumnType("smallint");

                    b.Property<string>("CountryCallingCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirebaseToken")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("HavePet")
                        .HasColumnType("boolean");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("TimeZone")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<bool>("WantVirtualPet")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.UserMedicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("BeginningDate")
                        .HasColumnType("date");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<int?>("MedicationId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("integer");

                    b.Property<int>("PillNumber")
                        .HasColumnType("integer");

                    b.Property<byte[]>("Timestamp")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("Usage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MedicationId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMedicines");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.UserMood", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("integer");

                    b.Property<byte>("Mood")
                        .HasColumnType("smallint");

                    b.Property<byte[]>("Timestamp")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UserMoods");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.UserProgress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("EveningLastFoodId")
                        .HasColumnType("integer");

                    b.Property<int?>("LastActivityId")
                        .HasColumnType("integer");

                    b.Property<int?>("LastArticleId")
                        .HasColumnType("integer");

                    b.Property<int?>("LastExerciseId")
                        .HasColumnType("integer");

                    b.Property<int?>("MorningLastFoodId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EveningLastFoodId");

                    b.HasIndex("LastActivityId");

                    b.HasIndex("LastArticleId");

                    b.HasIndex("LastExerciseId");

                    b.HasIndex("MorningLastFoodId");

                    b.HasIndex("UserId");

                    b.ToTable("UserProgresses");
                });

            modelBuilder.Entity("TimeOfDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UserMedicineId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserMedicineId");

                    b.ToTable("TimesOfDay");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Neuro.Domain.Entities.ActivityImageDescription", b =>
                {
                    b.HasOne("Neuro.Domain.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.Disease", b =>
                {
                    b.HasOne("Neuro.Domain.Entities.User", null)
                        .WithMany("Diseases")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.MedicationDay", b =>
                {
                    b.HasOne("Neuro.Domain.Entities.UserMedicine", "UserMedicine")
                        .WithMany("Days")
                        .HasForeignKey("UserMedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserMedicine");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.MedicationTime", b =>
                {
                    b.HasOne("Neuro.Domain.Entities.UserMedicine", null)
                        .WithMany("MedicationTimes")
                        .HasForeignKey("UserMedicineId");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.RecommendedRoutine", b =>
                {
                    b.HasOne("Neuro.Domain.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId");

                    b.HasOne("Neuro.Domain.Entities.Article", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleId");

                    b.HasOne("Neuro.Domain.Entities.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId");

                    b.HasOne("Neuro.Domain.Entities.FoodPage", "Food")
                        .WithMany()
                        .HasForeignKey("FoodId");

                    b.Navigation("Activity");

                    b.Navigation("Article");

                    b.Navigation("Exercise");

                    b.Navigation("Food");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.UserMedicine", b =>
                {
                    b.HasOne("Neuro.Domain.Entities.Medication", "Medication")
                        .WithMany("UserMedicines")
                        .HasForeignKey("MedicationId");

                    b.HasOne("Neuro.Domain.Entities.User", "User")
                        .WithMany("UserMedicines")
                        .HasForeignKey("UserId");

                    b.Navigation("Medication");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.UserProgress", b =>
                {
                    b.HasOne("Neuro.Domain.Entities.FoodPage", "EveningLastFood")
                        .WithMany()
                        .HasForeignKey("EveningLastFoodId");

                    b.HasOne("Neuro.Domain.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("LastActivityId");

                    b.HasOne("Neuro.Domain.Entities.Article", "Article")
                        .WithMany()
                        .HasForeignKey("LastArticleId");

                    b.HasOne("Neuro.Domain.Entities.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("LastExerciseId");

                    b.HasOne("Neuro.Domain.Entities.FoodPage", "MorningLastFood")
                        .WithMany()
                        .HasForeignKey("MorningLastFoodId");

                    b.HasOne("Neuro.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Article");

                    b.Navigation("EveningLastFood");

                    b.Navigation("Exercise");

                    b.Navigation("MorningLastFood");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TimeOfDay", b =>
                {
                    b.HasOne("Neuro.Domain.Entities.UserMedicine", "UserMedicine")
                        .WithMany("Times")
                        .HasForeignKey("UserMedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserMedicine");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.Medication", b =>
                {
                    b.Navigation("UserMedicines");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.User", b =>
                {
                    b.Navigation("Diseases");

                    b.Navigation("UserMedicines");
                });

            modelBuilder.Entity("Neuro.Domain.Entities.UserMedicine", b =>
                {
                    b.Navigation("Days");

                    b.Navigation("MedicationTimes");

                    b.Navigation("Times");
                });
#pragma warning restore 612, 618
        }
    }
}
