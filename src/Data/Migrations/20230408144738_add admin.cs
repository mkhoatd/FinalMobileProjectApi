using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Classrooms",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_AdminId",
                table: "Classrooms",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_Users_AdminId",
                table: "Classrooms",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_Users_AdminId",
                table: "Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_Classrooms_AdminId",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Classrooms");
        }
    }
}
