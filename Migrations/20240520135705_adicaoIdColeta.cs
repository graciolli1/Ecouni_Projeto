using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecouni_Projeto.Migrations
{
    /// <inheritdoc />
    public partial class adicaoIdColeta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "Coleta",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Cadastrarid",
                table: "Coleta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Coleta_Cadastrarid",
                table: "Coleta",
                column: "Cadastrarid");

            migrationBuilder.AddForeignKey(
                name: "FK_Coleta_Cadastrar_Cadastrarid",
                table: "Coleta",
                column: "Cadastrarid",
                principalTable: "Cadastrar",
                principalColumn: "Cadastrarid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coleta_Cadastrar_Cadastrarid",
                table: "Coleta");

            migrationBuilder.DropIndex(
                name: "IX_Coleta_Cadastrarid",
                table: "Coleta");

            migrationBuilder.DropColumn(
                name: "Cadastrarid",
                table: "Coleta");

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "Coleta",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
