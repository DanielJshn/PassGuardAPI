using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace V2iSafe.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Dbo");

            migrationBuilder.CreateTable(
                name: "Note",
                schema: "Dbo",
                columns: table => new
                {
                    noteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastEdit = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.noteId);
                });

            migrationBuilder.CreateTable(
                name: "Password",
                schema: "Dbo",
                columns: table => new
                {
                    passwordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    organization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastEdit = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Password", x => x.passwordId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalFields",
                schema: "Dbo",
                columns: table => new
                {
                    additionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    passwordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalFields", x => x.additionalId);
                    table.ForeignKey(
                        name: "FK_AdditionalFields_Password_passwordId",
                        column: x => x.passwordId,
                        principalSchema: "Dbo",
                        principalTable: "Password",
                        principalColumn: "passwordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFields_passwordId",
                schema: "Dbo",
                table: "AdditionalFields",
                column: "passwordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalFields",
                schema: "Dbo");

            migrationBuilder.DropTable(
                name: "Note",
                schema: "Dbo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Dbo");

            migrationBuilder.DropTable(
                name: "Password",
                schema: "Dbo");
        }
    }
}
