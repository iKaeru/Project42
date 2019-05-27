using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class Trainings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Collectons",
                table: "Collectons");

            migrationBuilder.RenameTable(
                name: "Collectons",
                newName: "Collections");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collections",
                table: "Collections",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Collections",
                table: "Collections");

            migrationBuilder.RenameTable(
                name: "Collections",
                newName: "Collectons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collectons",
                table: "Collectons",
                column: "Id");
        }
    }
}
