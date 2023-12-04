using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eVeterinarskaAmbulanta.Migrations
{
    public partial class appointmentIzmena : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Appointments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "0iXg7BcLiV/nLRIKABoVR/A/GuKcDnOpr1X6mmqmroE=", "ydGc8+PVnap4HN6bFkIHBA==" });

            migrationBuilder.UpdateData(
                table: "Veterinarians",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "MaL7o+Q52pAQtg1fLxxU27ZzFF3db5jAdsUT6Xr/A3Y=", "U6xIUt/qZeyhLS+SY/JKbg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "Appointments",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "1SgsoW3IvJf68saYjUFjmd3jYALkvLW+x3/AQtxNQuE=", "fiMxiMH6EKNSgHxY5lQsqQ==" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "PasswordSalt", "Role" },
                values: new object[] { 2, "markomarkovic@gmail.com", "Marko", "Markovic", "cIVi6y+xFV8YnrMpXhq0BDPvFc3DRGNle04/0dqPAts=", "CSLv9PTrZhDjhsBGCucw/Q==", 1 });

            migrationBuilder.UpdateData(
                table: "Veterinarians",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "NsyNPLJ5oKBZOdaOEqXqloBHoYIsXf3veUMwO3mzpt0=", "o1qnffIQon7UxVSJgCG2oA==" });
        }
    }
}
