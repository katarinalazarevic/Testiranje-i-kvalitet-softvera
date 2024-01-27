using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTemplate.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kolekcije",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazivKolekcije = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Umetnik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Godina = table.Column<int>(type: "int", nullable: false),
                    Stil = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kolekcije", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KontaktTelefon = table.Column<int>(type: "int", nullable: false),
                    Ulica = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Broj = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Narudzbine",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    KorisnikID = table.Column<int>(type: "int", nullable: false),
                    UkupnaCena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Narudzbine", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Narudzbine_Korisnici_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnici",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UmetnickaDela",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slika = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Tehnika = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dimenzije = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    Ram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KolekcijaID = table.Column<int>(type: "int", nullable: true),
                    NarudzbinaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UmetnickaDela", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UmetnickaDela_Kolekcije_KolekcijaID",
                        column: x => x.KolekcijaID,
                        principalTable: "Kolekcije",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UmetnickaDela_Narudzbine_NarudzbinaID",
                        column: x => x.NarudzbinaID,
                        principalTable: "Narudzbine",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Narudzbine_KorisnikID",
                table: "Narudzbine",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_UmetnickaDela_KolekcijaID",
                table: "UmetnickaDela",
                column: "KolekcijaID");

            migrationBuilder.CreateIndex(
                name: "IX_UmetnickaDela_NarudzbinaID",
                table: "UmetnickaDela",
                column: "NarudzbinaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UmetnickaDela");

            migrationBuilder.DropTable(
                name: "Kolekcije");

            migrationBuilder.DropTable(
                name: "Narudzbine");

            migrationBuilder.DropTable(
                name: "Korisnici");
        }
    }
}
