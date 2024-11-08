﻿// <auto-generated />
using InsuranceBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InsuranceBot.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InsuranceBot.Models.QuestionAnswer", b =>
                {
                    b.Property<string>("Question")
                        .HasColumnType("text");

                    b.Property<string>("Answer")
                        .HasColumnType("text");

                    b.Property<float[]>("Embedding")
                        .HasColumnType("real[]");

                    b.HasKey("Question");

                    b.ToTable("QuestionAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}