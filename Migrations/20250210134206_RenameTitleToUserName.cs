using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace V2iSafe.Migrations
{
    /// <inheritdoc />
    public partial class RenameTitleToUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Password",
                newName: "userName");

            migrationBuilder.RenameColumn(
                name: "organization",
                table: "Password",
                newName: "title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userName",
                table: "Password",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Password",
                newName: "organization");
        }

    }
}
