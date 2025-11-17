using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmissaoNFAPI.Migrations
{
    /// <inheritdoc />
    public partial class NotaFiscal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Numero",
                table: "NotasFiscais",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NotasFiscais_Numero",
                table: "NotasFiscais",
                column: "Numero",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotasFiscais_Numero",
                table: "NotasFiscais");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "NotasFiscais");
        }
    }
}
