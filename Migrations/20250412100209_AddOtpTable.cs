using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace V2iSafe.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OTP",
                schema: "Dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    otpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    expirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTP", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OTP",
                schema: "Dbo");
        }
    }
}
