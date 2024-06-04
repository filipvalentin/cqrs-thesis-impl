﻿// <auto-generated />
using System;
using Lunatic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Lunatic.Infrastructure.Migrations.LunaticML
{
    [DbContext(typeof(LunaticMLContext))]
    [Migration("20240603174932_MLMigration2")]
    partial class MLMigration2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Lunatic.Domain.MLModel.DaysToCompleteTaskEntry", b =>
                {
                    b.Property<Guid>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AssigneesCount")
                        .HasColumnType("int");

                    b.Property<double>("AverageCommentLength")
                        .HasColumnType("float");

                    b.Property<int>("CommentsCount")
                        .HasColumnType("int");

                    b.Property<int>("DaysTookToComplete")
                        .HasColumnType("int");

                    b.Property<int>("DescriptionLength")
                        .HasColumnType("int");

                    b.Property<int>("ExpectedDaysToComplete")
                        .HasColumnType("int");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.HasKey("TaskId");

                    b.ToTable("DaysToCompleteTaskEntries");
                });
#pragma warning restore 612, 618
        }
    }
}