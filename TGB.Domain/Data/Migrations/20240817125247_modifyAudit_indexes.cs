using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

#nullable disable

namespace TGB.Domain.Migrations
{
    /// <inheritdoc />
    public partial class modifyAudit_indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string?>(
                name: "UserId",
                table: "Audits",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Audits",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Audits_TypeId",
                table: "Audits",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_UserId",
                table: "Audits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_Time",
                table: "Audits",
                column: "Time");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_TypeName",
                table: "Audits",
                column: "TypeName");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_Action",
                table: "Audits",
                column: "Action");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Audits_TypeId",
                table: "Audits");

            migrationBuilder.DropIndex(
                name: "IX_Audits_UserId",
                table: "Audits");

            migrationBuilder.DropIndex(
                name: "IX_Audits_Time",
                table: "Audits");

            migrationBuilder.DropIndex(
                name: "IX_Audits_TypeName",
                table: "Audits");

            migrationBuilder.DropIndex(
                name: "IX_Audits_Action",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Audits");



        }
    }
}
