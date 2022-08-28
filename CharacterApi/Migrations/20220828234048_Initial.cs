using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CharacterApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    LastName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    MediaTitle = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    MediaType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.CharacterId);
                });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "CharacterId", "Age", "FirstName", "LastName", "MediaTitle", "MediaType" },
                values: new object[,]
                {
                    { 1, 7, "Matilda", "Woolly Mammoth", "The Land Before Time", "Film" },
                    { 2, 10, "Rexie", "Dinosaur", "The Land Before Time", "Film" },
                    { 3, 2, "Matilda", "Dinosaur", "The Land Before Time", "Film" },
                    { 4, 4, "Pip", "Shark", "The Land Before Time", "Film" },
                    { 5, 22, "Bartholomew", "Dinosaur", "The Land Before Time", "Film" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
