using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eVeterinarskaAmbulanta.Migrations
{
    public partial class SeedersAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Ambulances",
                columns: new[] { "Id", "Adress", "Name", "Phone" },
                values: new object[] { 1, "Bulevar Nemanjica", "Vet Medic", "0647061254" });

            migrationBuilder.InsertData(
                table: "Ambulances",
                columns: new[] { "Id", "Adress", "Name", "Phone" },
                values: new object[] { 2, "Bulevar Zorana Djindjica", "Pet Lovers", "0647061251" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role" },
                values: new object[] { 1, "dusandjordjevic@gmail.com", "Dusan", "Djordjevic", "sifra", "admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role" },
                values: new object[] { 2, "markomarkovic@gmail.com", "Marko", "Markovic", "secret", "user" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ambulances",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ambulances",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
