using Microsoft.EntityFrameworkCore.Migrations;

namespace Ao3RentcarsApi.Migrations
{
    public partial class CadastrandoPrimeirosVeiculos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "nxVrOLgeXbfKd0o0Qz8OUA==");

            migrationBuilder.InsertData(
                table: "Veiculo",
                columns: new[] { "Id", "AnoFabricacao", "AnoModelo", "Marca", "Modelo", "Placa" },
                values: new object[] { 2, 2020, 2021, "FIAT", "UNO DRIVE 1.0 Flex 6V 5p", "BEE4R22" });

            migrationBuilder.InsertData(
                table: "Veiculo",
                columns: new[] { "Id", "AnoFabricacao", "AnoModelo", "Marca", "Modelo", "Placa" },
                values: new object[] { 1, 2020, 2021, "VOLKSWAGEN", "Gol 1.0 Flex 12V 5p", "BRA0S17" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Veiculo",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Veiculo",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "nxVrOLgeXbfKd0o0Qz8OUA==");
        }
    }
}
