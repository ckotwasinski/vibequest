using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VibeQuest.DataAccess.Migrations
{
    public partial class Alter_Table_Notifications_Add_Column_IsViewed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsViewed",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0d50f040-9e48-45a7-8dd7-4b26322384db"),
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "UFhUSfD9TRHl1Mbrl8tOKSSTQWM=", "3i/x5Ml/ZrN/zrDNwsOrbYJXhq+G+mJ+" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsViewed",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0d50f040-9e48-45a7-8dd7-4b26322384db"),
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "rErW/RuTLGfd8Oq0NxX437gMpwQ=", "MhcTUfm1F+XOmObPH0oDTrmTsFHYaGH+" });
        }
    }
}
