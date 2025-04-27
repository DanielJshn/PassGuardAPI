using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace V2iSafe.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "UserData",
                schema: "Dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hashedPK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hashedPKSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    encryptedSK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hashedRK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    recoverySK = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
