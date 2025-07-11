using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Listening.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class description : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descriptions",
                table: "T_Kind",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Descriptions",
                table: "T_Episode",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Descriptions",
                table: "T_Category",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Descriptions",
                table: "T_Album",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "T_Kind",
                newName: "Descriptions");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "T_Episode",
                newName: "Descriptions");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "T_Category",
                newName: "Descriptions");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "T_Album",
                newName: "Descriptions");
        }
    }
}
