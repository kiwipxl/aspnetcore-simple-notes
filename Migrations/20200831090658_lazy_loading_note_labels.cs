using Microsoft.EntityFrameworkCore.Migrations;

namespace notes.Migrations
{
    public partial class lazy_loading_note_labels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Labels_Name",
                table: "Labels",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Labels_Name",
                table: "Labels");
        }
    }
}
