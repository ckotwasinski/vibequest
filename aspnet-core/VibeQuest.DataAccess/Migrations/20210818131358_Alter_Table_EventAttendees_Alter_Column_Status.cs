using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VibeQuest.DataAccess.Migrations
{
    public partial class Alter_Table_EventAttendees_Alter_Column_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "EventAttendees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0d50f040-9e48-45a7-8dd7-4b26322384db"),
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "GIXkzD+bPeQ7X0N0CjodcWvvsBQ=", "SQ/qKneX1HEkc4tcY8hj828l/S4YSlIF" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "EventAttendees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0d50f040-9e48-45a7-8dd7-4b26322384db"),
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "8w8UxYmWk74MQ+H0dMVfNTRNL+I=", "OmeP6nxKOw/4OkQz46s6UmNUA4GNOwP2" });
        }
    }
}
