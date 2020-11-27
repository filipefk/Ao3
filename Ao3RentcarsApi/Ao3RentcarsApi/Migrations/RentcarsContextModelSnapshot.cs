﻿// <auto-generated />
using System;
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

            modelBuilder.Entity("Ao3RentcarsApi.Models.Locacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataFim")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataInclusao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("TEXT");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdVeiculo")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.HasIndex("IdVeiculo");

                    b.ToTable("Locacao");
                });

            modelBuilder.Entity("Ao3RentcarsApi.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataInclusao")
                        .HasColumnType("TEXT");

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
                            DataAlteracao = new DateTime(2020, 11, 27, 14, 24, 27, 172, DateTimeKind.Local).AddTicks(1327),
                            DataInclusao = new DateTime(2020, 11, 27, 14, 24, 27, 170, DateTimeKind.Local).AddTicks(6039),
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

                    b.Property<DateTime>("DataAlteracao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataInclusao")
                        .HasColumnType("TEXT");

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
                            DataAlteracao = new DateTime(2020, 11, 27, 14, 24, 27, 174, DateTimeKind.Local).AddTicks(2721),
                            DataInclusao = new DateTime(2020, 11, 27, 14, 24, 27, 174, DateTimeKind.Local).AddTicks(2037),
                            Marca = "VOLKSWAGEN",
                            Modelo = "Gol 1.0 Flex 12V 5p",
                            Placa = "BRA0S17"
                        },
                        new
                        {
                            Id = 2,
                            AnoFabricacao = 2020,
                            AnoModelo = 2021,
                            DataAlteracao = new DateTime(2020, 11, 27, 14, 24, 27, 174, DateTimeKind.Local).AddTicks(3380),
                            DataInclusao = new DateTime(2020, 11, 27, 14, 24, 27, 174, DateTimeKind.Local).AddTicks(3376),
                            Marca = "FIAT",
                            Modelo = "UNO DRIVE 1.0 Flex 6V 5p",
                            Placa = "BEE4R22"
                        });
                });

            modelBuilder.Entity("Ao3RentcarsApi.Models.Locacao", b =>
                {
                    b.HasOne("Ao3RentcarsApi.Models.Usuario", "Usuario")
                        .WithMany("Locacoes")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ao3RentcarsApi.Models.Veiculo", "Veiculo")
                        .WithMany("Locacoes")
                        .HasForeignKey("IdVeiculo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");

                    b.Navigation("Veiculo");
                });

            modelBuilder.Entity("Ao3RentcarsApi.Models.Usuario", b =>
                {
                    b.Navigation("Locacoes");
                });

            modelBuilder.Entity("Ao3RentcarsApi.Models.Veiculo", b =>
                {
                    b.Navigation("Locacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
