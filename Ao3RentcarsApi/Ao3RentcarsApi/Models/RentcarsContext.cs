using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using System;

namespace Ao3RentcarsApi.Models
{
    public class RentcarsContext : DbContext
    {
        // Recusros para encriptação da senha do usuário no banco
        private readonly byte[] _encryptionKey = new byte[] { 0x28, 0x12, 0xbd, 0xd6, 0xe2, 0xab, 0x41, 0x0c, 0x54, 0xf2, 0x10, 0x11, 0x7e, 0x0f, 0xed, 0xd9 };
        private readonly byte[] _encryptionIV = new byte[] { 0x11, 0xc0, 0xc4, 0xf6, 0xa2, 0x2d, 0x0a, 0xb1, 0xb7, 0xda, 0xb9, 0x38, 0x1e, 0x25, 0x0c, 0x21 };
        private readonly IEncryptionProvider _provider;

        public RentcarsContext(DbContextOptions<RentcarsContext> options)
            : base(options)
        {
            // Recusros para encriptação da senha do usuário no banco
            this._provider = new AesProvider(this._encryptionKey, this._encryptionIV);
        }

        public DbSet<Veiculo> Veiculos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Locacao> Locacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // Recusros para encriptação da senha do usuário no banco
            modelBuilder.UseEncryption(this._provider);

            // Criando Unique Key para o login do Usuário
            modelBuilder.Entity<Usuario>()
                .HasIndex(b => b.Login)
                .IsUnique()
                .HasDatabaseName("IDX_LOGIN_USUARIO");

            // Criando Unique Key para a placa do Veículo
            modelBuilder.Entity<Veiculo>()
                .HasIndex(b => b.Placa)
                .IsUnique()
                .HasDatabaseName("IDX_PLACA_VEICULO");

            // Criando Unique Key para o Cpf do cliente
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Cpf)
                .IsUnique()
                .HasDatabaseName("IDX_CPF_CLIENTE");

            // Criando uma carga inicial de dados
            modelBuilder.Entity<Usuario>()
                .HasData(
                    new Usuario
                    {
                        Id = 1,
                        Tipo = 1, // Todos vão ser do tipo "1 - Funcionario" inicialmente
                        Nome = "Administrador",
                        Login = "Admin",
                        Senha = "SenhaAdmin",
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now
                    }
                );
            modelBuilder.Entity<Cliente>()
                .HasData(
                    new Cliente
                    {
                        Id = 1,
                        Nome = "Filipe",
                        Cpf = "45583420049",
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now
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
                        AnoFabricacao = 2020,
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now
                    },
                    new Veiculo
                    {
                        Id = 2,
                        Modelo = "UNO DRIVE 1.0 Flex 6V 5p",
                        Marca = "FIAT",
                        Placa = "BEE4R22",
                        AnoModelo = 2021,
                        AnoFabricacao = 2020,
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now
                    }
                );
        }
    }
}
