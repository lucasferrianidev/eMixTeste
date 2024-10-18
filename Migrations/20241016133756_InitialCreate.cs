using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMixApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CEP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CEP = table.Column<string>(type: "varchar(8)", nullable: false),
                    LOGRADOURO = table.Column<string>(type: "varchar(500)", nullable: false),
                    COMPLEMENTO = table.Column<string>(type: "varchar(500)", nullable: false),
                    BAIRRO = table.Column<string>(type: "varchar(500)", nullable: false),
                    LOCALIDADE = table.Column<string>(type: "varchar(500)", nullable: false),
                    UF = table.Column<string>(type: "varchar(2)", nullable: false),
                    UNIDADE = table.Column<int>(type: "int", nullable: false),
                    IBGE = table.Column<int>(type: "int", nullable: false),
                    GIA = table.Column<string>(type: "varchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("IDX_Id", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CEP");
        }
    }
}
