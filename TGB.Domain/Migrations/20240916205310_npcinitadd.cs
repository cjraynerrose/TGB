using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGB.Domain.Migrations
{
    /// <inheritdoc />
    public partial class npcinitadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Npcs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Npcs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NpcAttribites",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    NpcId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NpcAttribites", x => x.Key);
                    table.ForeignKey(
                        name: "FK_NpcAttribites_Npcs_NpcId",
                        column: x => x.NpcId,
                        principalTable: "Npcs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NpcAttribites_NpcId",
                table: "NpcAttribites",
                column: "NpcId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NpcAttribites");

            migrationBuilder.DropTable(
                name: "Npcs");
        }
    }
}
