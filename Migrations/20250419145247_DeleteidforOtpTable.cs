using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace V2iSafe.Migrations
{
    /// <inheritdoc />
    public partial class DeleteidforOtpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OTP",
                schema: "Dbo",
                table: "OTP");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "Dbo",
                table: "OTP");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "Dbo",
                table: "OTP",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OTP",
                schema: "Dbo",
                table: "OTP",
                column: "email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OTP",
                schema: "Dbo",
                table: "OTP");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "Dbo",
                table: "OTP",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                schema: "Dbo",
                table: "OTP",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OTP",
                schema: "Dbo",
                table: "OTP",
                column: "id");
        }
    }
}
