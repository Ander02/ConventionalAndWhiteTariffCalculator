﻿// <auto-generated />
using ConventionalAndWhiteTariffCalculator.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ConventionalAndWhiteTariffCalculator.Migrations
{
    [DbContext(typeof(Db))]
    [Migration("20180116131836_removeCityInConcessionary")]
    partial class removeCityInConcessionary
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ConventionalAndWhiteTariffCalculator.Domain.Concessionary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Concessionary");
                });

            modelBuilder.Entity("ConventionalAndWhiteTariffCalculator.Domain.Equipament", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("DefaultPower");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Equipament");
                });

            modelBuilder.Entity("ConventionalAndWhiteTariffCalculator.Domain.Tariff", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("BaseValue");

                    b.Property<Guid>("ConcessionaryId");

                    b.Property<TimeSpan>("FinishTime");

                    b.Property<TimeSpan>("InitTime");

                    b.Property<string>("Name");

                    b.Property<string>("TariffType");

                    b.HasKey("Id");

                    b.HasIndex("ConcessionaryId");

                    b.ToTable("Tariff");
                });

            modelBuilder.Entity("ConventionalAndWhiteTariffCalculator.Domain.Tariff", b =>
                {
                    b.HasOne("ConventionalAndWhiteTariffCalculator.Domain.Concessionary", "Concessionary")
                        .WithMany("Tariffs")
                        .HasForeignKey("ConcessionaryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
