using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecouni_Projeto.Migrations
{
    /// <inheritdoc />
    public partial class tabelaColetaAtt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TamanhoSaco",
                table: "Coleta",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TipoResiduo",
                table: "Coleta",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoResiduo",
                table: "Coleta");

            migrationBuilder.AlterColumn<string>(
                name: "TamanhoSaco",
                table: "Coleta",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
