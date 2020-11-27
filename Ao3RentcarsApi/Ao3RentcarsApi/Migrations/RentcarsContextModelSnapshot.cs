﻿// <auto-generated />
using Ao3RentcarsApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ao3RentcarsApi.Migrations
{
    [DbContext(typeof(RentcarsContext))]
    partial class RentcarsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Ao3RentcarsApi.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Usuario");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Login = "Admin",
                            Nome = "Administrador",
                            Senha = "nxVrOLgeXbfKd0o0Qz8OUA=="
                        });
                });

            modelBuilder.Entity("Ao3RentcarsApi.Models.Veiculo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AnoFabricacao")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AnoModelo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Veiculo");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AnoFabricacao = 2020,
                            AnoModelo = 2021,
                            Marca = "VOLKSWAGEN",
                            Modelo = "Gol 1.0 Flex 12V 5p",
                            Placa = "BRA0S17"
                        },
                        new
                        {
                            Id = 2,
                            AnoFabricacao = 2020,
                            AnoModelo = 2021,
                            Marca = "FIAT",
                            Modelo = "UNO DRIVE 1.0 Flex 6V 5p",
                            Placa = "BEE4R22"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
