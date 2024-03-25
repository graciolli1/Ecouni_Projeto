using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecouni_Projeto.Data.Migrations
{
    /// <inheritdoc />
    public partial class DbModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cadastrar",
                columns: table => new
                {
                    Cadastrarid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Telefone = table.Column<int>(type: "int", nullable: false),
                    Senha = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConfirmarSenha = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cadastrar", x => x.Cadastrarid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cadastrar");
        }
    }
}
