using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ao3RentcarsApi.Migrations
{
    public partial class MigrationOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataInclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Senha = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataInclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modelo = table.Column<string>(type: "TEXT", nullable: false),
                    Marca = table.Column<string>(type: "TEXT", nullable: false),
                    Placa = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false),
                    AnoModelo = table.Column<int>(type: "INTEGER", nullable: false),
                    AnoFabricacao = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataInclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false),
                    IdVeiculo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locacao_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locacao_Veiculo_IdVeiculo",
                        column: x => x.IdVeiculo,
                        principalTable: "Veiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "DataAlteracao", "DataInclusao", "Login", "Nome", "Senha" },
                values: new object[] { 1, new DateTime(2020, 11, 27, 14, 24, 27, 172, DateTimeKind.Local).AddTicks(1327), new DateTime(2020, 11, 27, 14, 24, 27, 170, DateTimeKind.Local).AddTicks(6039), "Admin", "Administrador", "nxVrOLgeXbfKd0o0Qz8OUA==" });

            migrationBuilder.InsertData(
                table: "Veiculo",
                columns: new[] { "Id", "AnoFabricacao", "AnoModelo", "DataAlteracao", "DataInclusao", "Marca", "Modelo", "Placa" },
                values: new object[] { 1, 2020, 2021, new DateTime(2020, 11, 27, 14, 24, 27, 174, DateTimeKind.Local).AddTicks(2721), new DateTime(2020, 11, 27, 14, 24, 27, 174, DateTimeKind.Local).AddTicks(2037), "VOLKSWAGEN", "Gol 1.0 Flex 12V 5p", "BRA0S17" });

            migrationBuilder.InsertData(
                table: "Veiculo",
                columns: new[] { "Id", "AnoFabricacao", "AnoModelo", "DataAlteracao", "DataInclusao", "Marca", "Modelo", "Placa" },
                values: new object[] { 2, 2020, 2021, new DateTime(2020, 11, 27, 14, 24, 27, 174, DateTimeKind.Local).AddTicks(3380), new DateTime(2020, 11, 27, 14, 24, 27, 174, DateTimeKind.Local).AddTicks(3376), "FIAT", "UNO DRIVE 1.0 Flex 6V 5p", "BEE4R22" });

            migrationBuilder.CreateIndex(
                name: "IX_Locacao_IdUsuario",
                table: "Locacao",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Locacao_IdVeiculo",
                table: "Locacao",
                column: "IdVeiculo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locacao");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Veiculo");
        }
    }
}
