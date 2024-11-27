using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace V2iSafe.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Удаляем старое ограничение для связи AdditionalFields -> Password
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalFields_Password_passwordId",
                schema: "Dbo",
                table: "AdditionalFields");

            // Добавляем каскадное удаление для связи AdditionalFields -> Password
            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFields_Password_passwordId",
                schema: "Dbo",
                table: "AdditionalFields",
                column: "passwordId",
                principalSchema: "Dbo",
                principalTable: "Password",
                principalColumn: "passwordId",
                onDelete: ReferentialAction.Cascade);

            // Добавляем каскадное удаление для всех данных User -> Note
            migrationBuilder.AddForeignKey(
                name: "FK_Note_User_id",
                schema: "Dbo",
                table: "Note",
                column: "id",
                principalSchema: "Dbo",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            // Добавляем каскадное удаление для всех данных User -> Password
            migrationBuilder.AddForeignKey(
                name: "FK_Password_User_id",
                schema: "Dbo",
                table: "Password",
                column: "id",
                principalSchema: "Dbo",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            // Добавляем каскадное удаление для всех данных User -> BankAccounts
            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_User_id",
                schema: "Dbo",
                table: "BankAccounts",
                column: "id",
                principalSchema: "Dbo",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удаляем каскадное удаление AdditionalFields -> Password
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalFields_Password_passwordId",
                schema: "Dbo",
                table: "AdditionalFields");

            // Удаляем каскадное удаление Note -> User
            migrationBuilder.DropForeignKey(
                name: "FK_Note_User_id",
                schema: "Dbo",
                table: "Note");

            // Удаляем каскадное удаление Password -> User
            migrationBuilder.DropForeignKey(
                name: "FK_Password_User_id",
                schema: "Dbo",
                table: "Password");

            // Удаляем каскадное удаление BankAccounts -> User
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_User_id",
                schema: "Dbo",
                table: "BankAccounts");

            // Восстанавливаем старое поведение AdditionalFields -> Password
            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalFields_Password_passwordId",
                schema: "Dbo",
                table: "AdditionalFields",
                column: "passwordId",
                principalSchema: "Dbo",
                principalTable: "Password",
                principalColumn: "passwordId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
