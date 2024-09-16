using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TGB.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_GroupBanks_GroupBankId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GroupBanks");

            migrationBuilder.AlterColumn<string>(
                name: "GroupBankId",
                table: "Notes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "CampaignId",
                table: "Notes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    GroupBankId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_GroupBanks_GroupBankId",
                        column: x => x.GroupBankId,
                        principalTable: "GroupBanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CampaignId",
                table: "Notes",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_GroupBankId",
                table: "Campaigns",
                column: "GroupBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Campaigns_CampaignId",
                table: "Notes",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_GroupBanks_GroupBankId",
                table: "Notes",
                column: "GroupBankId",
                principalTable: "GroupBanks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Campaigns_CampaignId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_GroupBanks_GroupBankId",
                table: "Notes");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Notes_CampaignId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "GroupBankId",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GroupBanks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_GroupBanks_GroupBankId",
                table: "Notes",
                column: "GroupBankId",
                principalTable: "GroupBanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
