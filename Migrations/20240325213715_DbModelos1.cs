using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecouni_Projeto.Migrations
{
    /// <inheritdoc />
    public partial class DbModelos1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    ContatoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Mensagem = table.Column<decimal>(type: "decimal(18,2)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.ContatoId);
                });

            migrationBuilder.CreateTable(
                name: "DownloadApps",
                columns: table => new
                {
                    DownloadAppId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadApps", x => x.DownloadAppId);
                });

            migrationBuilder.CreateTable(
                name: "SobreNos",
                columns: table => new
                {
                    SobreNosId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SobreNos", x => x.SobreNosId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contatos");

            migrationBuilder.DropTable(
                name: "DownloadApps");

            migrationBuilder.DropTable(
                name: "SobreNos");
        }
    }
}
