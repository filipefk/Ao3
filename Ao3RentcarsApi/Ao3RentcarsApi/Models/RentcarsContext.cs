﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;

namespace Ao3RentcarsApi.Models
{
    public class RentcarsContext : DbContext
    {
        // Get key and IV from a Base64String or any other ways.
        // You can generate a key and IV using "AesProvider.GenerateKey()"
        private readonly byte[] _encryptionKey = new byte[] { 0x28, 0x12, 0xbd, 0xd6, 0xe2, 0xab, 0x41, 0x0c, 0x54, 0xf2, 0x10, 0x11, 0x7e, 0x0f, 0xed, 0xd9 };
        private readonly byte[] _encryptionIV = new byte[] { 0x11, 0xc0, 0xc4, 0xf6, 0xa2, 0x2d, 0x0a, 0xb1, 0xb7, 0xda, 0xb9, 0x38, 0x1e, 0x25, 0x0c, 0x21 };
        private readonly IEncryptionProvider _provider;

        public RentcarsContext(DbContextOptions<RentcarsContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
            this._provider = new AesProvider(this._encryptionKey, this._encryptionIV);
        }

        public DbSet<Veiculo> Veiculos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEncryption(this._provider);
            modelBuilder.Entity<Usuario>()
                .HasData(
                    new Usuario
                    {
                        Id = 1,
                        Nome = "Administrador",
                        Login = "Admin",
                        Senha = "SenhaAdmin"
                    }
                );
            modelBuilder.Entity<Veiculo>()
                .HasData(
                    new Veiculo
                    {
                        Id = 1,
                        Modelo = "Gol 1.0 Flex 12V 5p",
                        Marca = "VOLKSWAGEN",
                        Placa = "BRA0S17",
                        AnoModelo = 2021,
                        AnoFabricacao = 2020
                    },
                    new Veiculo
                    {
                        Id = 2,
                        Modelo = "UNO DRIVE 1.0 Flex 6V 5p",
                        Marca = "FIAT",
                        Placa = "BEE4R22",
                        AnoModelo = 2021,
                        AnoFabricacao = 2020
                    }
                );
        }
    }
}
