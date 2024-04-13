﻿// <auto-generated />
using System;
using Logic.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logic.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240413084006_Add_sync_table")]
    partial class Add_sync_table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Logic.DAL.Synchronization", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<bool>("IsSyncRequired")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_sync_required");

                    b.Property<int>("RowVersion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1)
                        .HasColumnName("row_version");

                    b.HasKey("Name");

                    b.ToTable("sync", (string)null);
                });

            modelBuilder.Entity("Logic.Students.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Credits")
                        .HasColumnType("integer")
                        .HasColumnName("credits");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("courses", (string)null);
                });

            modelBuilder.Entity("Logic.Students.Disenrollment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_time");

                    b.Property<int?>("course_id")
                        .HasColumnType("integer");

                    b.Property<int?>("student_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("course_id");

                    b.HasIndex("student_id");

                    b.ToTable("disenrollments", (string)null);
                });

            modelBuilder.Entity("Logic.Students.Enrollment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Grade")
                        .HasColumnType("integer")
                        .HasColumnName("grade");

                    b.Property<int>("course_id")
                        .HasColumnType("integer");

                    b.Property<int?>("student_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("course_id");

                    b.HasIndex("student_id");

                    b.ToTable("enrollments", (string)null);
                });

            modelBuilder.Entity("Logic.Students.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("students", (string)null);
                });

            modelBuilder.Entity("Logic.Students.Disenrollment", b =>
                {
                    b.HasOne("Logic.Students.Course", "Course")
                        .WithMany()
                        .HasForeignKey("course_id");

                    b.HasOne("Logic.Students.Student", "Student")
                        .WithMany("Disenrollments")
                        .HasForeignKey("student_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Logic.Students.Enrollment", b =>
                {
                    b.HasOne("Logic.Students.Course", "Course")
                        .WithMany()
                        .HasForeignKey("course_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Logic.Students.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("student_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Logic.Students.Student", b =>
                {
                    b.Navigation("Disenrollments");

                    b.Navigation("Enrollments");
                });
#pragma warning restore 612, 618
        }
    }
}
