using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eVeterinarskaAmbulanta.Migrations
{
    public partial class VetReg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "1SgsoW3IvJf68saYjUFjmd3jYALkvLW+x3/AQtxNQuE=", "fiMxiMH6EKNSgHxY5lQsqQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "cIVi6y+xFV8YnrMpXhq0BDPvFc3DRGNle04/0dqPAts=", "CSLv9PTrZhDjhsBGCucw/Q==" });

            migrationBuilder.UpdateData(
                table: "Veterinarians",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "NsyNPLJ5oKBZOdaOEqXqloBHoYIsXf3veUMwO3mzpt0=", "o1qnffIQon7UxVSJgCG2oA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "k3iAosRElKxXFr2J+vsDxONpMTa1/dNCy5DOogNfpZk=", "NAmfNjdPvOoF6yq2N7Hu5Q==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "QtMASgx91TJczbt/8mpdO/kYgTqDoSTpP+KDS8cSRcE=", "oHVC9lDEUrIw0/stzQArwA==" });

            migrationBuilder.UpdateData(
                table: "Veterinarians",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Password", "PasswordSalt" },
                values: new object[] { "bWIBCrZVdE6E2FXGANDbIGikzjJsucMdaurLNCzBxFM=", "AMCyNzCU0ChInhbJr8ECMA==" });
        }
    }
}
