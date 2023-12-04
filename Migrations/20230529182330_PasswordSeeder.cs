using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eVeterinarskaAmbulanta.Migrations
{
    public partial class PasswordSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "m2p71OsglOJcflR8+P4qKnkE1sG3m0/jP6lWGYgy7NM=", "m8ydA0OoJZs27Ci1oLMmQA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Password", "PasswordSalt", "Role" },
                values: new object[] { "q4XojNIw185dfJCzRmDjpsCFT0if8ekoolUHhhoIrRY=", "D+gMCv4yXegvgAmtpV8gag==", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "sifra", "62tu4eWHWrDgzvi5VnNsDQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Password", "PasswordSalt", "Role" },
                values: new object[] { "secret", "XQt+nJuLyEu8VOmFX7lsJQ==", 0 });
        }
    }
}
