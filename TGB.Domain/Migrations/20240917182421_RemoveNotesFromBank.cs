using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGB.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotesFromBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_GroupBanks_GroupBankId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_GroupBankId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "GroupBankId",
                table: "Notes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupBankId",
                table: "Notes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_GroupBankId",
                table: "Notes",
                column: "GroupBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_GroupBanks_GroupBankId",
                table: "Notes",
                column: "GroupBankId",
                principalTable: "GroupBanks",
                principalColumn: "Id");
        }
    }
}
