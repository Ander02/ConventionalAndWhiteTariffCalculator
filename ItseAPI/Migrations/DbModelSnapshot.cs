﻿// <auto-generated />
using ConventionalAndWhiteTariffCalculatorAPI.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ConventionalAndWhiteTariffCalculatorAPI.Migrations
{
    [DbContext(typeof(Db))]
    partial class DbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ConventionalAndWhiteTariffCalculator.Domain.Equipment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("DefaultPower");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("ConventionalAndWhiteTariffCalculator.Domain.PowerDistribuitor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("PowerDistribuitor");
                });

            modelBuilder.Entity("ConventionalAndWhiteTariffCalculator.Domain.Tariff", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("BaseValue");

                    b.Property<TimeSpan>("FinishTime");

                    b.Property<TimeSpan>("InitTime");

                    b.Property<string>("Name");

                    b.Property<Guid>("PowerDistribuitorId");

                    b.Property<string>("TariffType");

                    b.HasKey("Id");

                    b.HasIndex("PowerDistribuitorId");

                    b.ToTable("Tariff");
                });

            modelBuilder.Entity("ConventionalAndWhiteTariffCalculator.Domain.Tariff", b =>
                {
                    b.HasOne("ConventionalAndWhiteTariffCalculator.Domain.PowerDistribuitor", "PowerDistribuitor")
                        .WithMany("Tariffs")
                        .HasForeignKey("PowerDistribuitorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
