using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Xml.Serialization;
using WebTemplate.Controllers;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;


namespace NUnitTestiranje
{
    public class Tests
    {
        private ProjekatContext _context;
       
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            // Konfiguracija baze podataka za testiranje
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")  
                .Build();

            var connectionString = configuration.GetConnectionString("IspitCS");  

            var options = new DbContextOptionsBuilder<ProjekatContext>()
                .UseSqlServer(connectionString)
                .Options;

            _context = new ProjekatContext(options);

        }

        [SetUp]
        public void Setup()
        {
            

        }
        [Test]
        public async Task TestRegisterUser()
        {
            var controller = new KorisnikController(_context);


            var registerData = new RegisterKorisnik
            {
                Broj = 111,
                Email = "test2@example.com",
                Grad = "Nis",
                Ime = "Nikola",
                KontaktTelefon = 1624567823,
                Prezime = "Petrovic",
                Ulica = "Brestova",
                Password = "Password!Test123"
            };

            var result = await controller.RegisterUser(registerData) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Registrovan je novi korisnik", result.Value);
            
        }
        [Test]
        public async Task TestRegisterUser_DuplicateEmail_ReturnsBadRequest()
        {
            var controller = new KorisnikController(_context);

            var registerData = new RegisterKorisnik
            {
                Broj = 111,
                Email = "test1@example.com",
                Grad = "Nis",
                Ime = "Nikola",
                KontaktTelefon = 1624567823,
                Prezime = "Petrovic",
                Ulica = "Brestova",
                Password = "Password!Test123"
            };

            var result = await controller.RegisterUser(registerData) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Postoji registracija sa unetim emailom", result.Value);
        }

        [Test]
        public async Task TestLoginValidUser()
        {

            var controller = new KorisnikController(_context);

            var loginData = new LoginKorisnik
            {
                Email = "test1@example.com",
                Password = "Password!Test123"
            };

            var result = await controller.Login(loginData) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Uspesan Login", result.Value);
        }
        
        [Test]
        public async Task TestRegisterUser_MissingData()
        {
            //ovaj test nece da prodje ako neko od obaveznih polja fali
            var controller = new KorisnikController(_context);


            var registerData = new RegisterKorisnik
            {
                Broj = 111,
                Email = "test5@example.com",
                Grad = "GradTest",
                //Ime = "Test",
                KontaktTelefon = 123456789,
                Prezime = "PrezimeTest", // Nedostaje obavezno polje
                Ulica = "UlicaTest",
                Password = "Password!Test"
            };

            var result = await controller.RegisterUser(registerData) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Registrovan je novi korisnik", result.Value);

            Assert.IsTrue(controller.ModelState.IsValid, "Očekuje se da su sva obavezna polja prisutna");


        }

        [Test]
        public async Task TestLoginInvalidEmail()
        {
            var controller = new KorisnikController(_context);

            var loginData = new LoginKorisnik
            {
                Email = "",
                Password = "Password!Test"
            };

            var result = await controller.Login(loginData) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Nevalidni podaci", result.Value);
        }
        [Test]
        public async Task TestLoginInvalidPassword()
        {
            
            var controller = new KorisnikController(_context);

            var loginData = new LoginKorisnik
            {
                Email = "test1@example.com",
                Password = "WrongPassword"
            };


            var result = await controller.Login(loginData) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Pogresna sifra", result.Value);
        }
        [Test]
        public async Task TestObrisiKorisnikaSuccess()
        {
            var controller = new KorisnikController(_context);

            await controller.RegisterUser(new RegisterKorisnik
            {
                Broj = 11,
                Email = "testiranje@example.com",
                Grad = "Nis",
                Ime = "Josip",
                KontaktTelefon = 123456789,
                Prezime = "Pancic",
                Ulica = "Somborska",
                Password = "Password!Test"
            });

            var korisnik = await _context.Korisnici.FirstAsync();
            var korisnikId = korisnik.ID;

            var result = await controller.ObrisiKorisnika(korisnikId) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Uspesno brisanje", result.Value);
        }

        

        [Test]
        public async Task TestObrisiNepostojecegKorisnika()
        {

            var controller = new KorisnikController(_context);

            var korisnikId = 1;

            var result = await controller.ObrisiKorisnika(korisnikId) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Korisnik sa datim ID ne postoji", result.Value);
        }
        [Test]
        public async Task TestObrisiKorisnikaLosID()
        {
            var controller = new KorisnikController(_context);

            var korisnikId = -2;
            var result = await controller.ObrisiKorisnika(korisnikId) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("ID mora biti veći od nule", result.Value);
        }
        [Test]
        public async Task TestPromeniSifruUspesnaPromena()
        {
            var controller = new KorisnikController(_context);
            var result = await controller.PromeniSifru("test5@example.com", "novaSifra@123") as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Šifra je uspešno promenjena!", result.Value);

        }

        [Test]
        public async Task TestPromeniSifruNepostojeciEmail()
        {
            var controller = new KorisnikController(_context);

            var result = await controller.PromeniSifru("nonexisting@example.com", "newPassword") as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Nepostojeci email!", result.Value);
        }
        [Test]
        public async Task TestPromeniSifruLosFormatSifre()
        {
            var controller = new KorisnikController(_context);

            var result = await controller.PromeniSifru("test5@example.com", "NovaSifra123") as BadRequestObjectResult;

            
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Sifra mora da sadrzi makar jedan specijalni karakter! (*-+_@&%$)", result.Value);
        }

        [Test]
        public async Task TestVratiKorisnike()
        {
            var controller = new KorisnikController(_context);

            var result = await controller.VratiKorisnike() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            var korisnici = result.Value as List<Korisnik>;
            Assert.IsNotNull(korisnici);
           
        }
        [Test]
        public async Task TestVratiKorisnikePraznaBaza()
        {
            var controller = new KorisnikController(_context);
            var result = await controller.VratiKorisnike() as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);

            Assert.AreEqual("Nema korisnika.", result.Value);
        }


        [Test]
        public async Task VratiKorisnikeTipListe()
        {
            var controller = new KorisnikController(_context);

            
            var result = await controller.VratiKorisnike();

            // da li je rezultat tipa OkObjectResult i da li vraćena vrednost ima očekivani tip
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var korisnici = okResult.Value as List<Korisnik>;

            // provera da li je vraćena lista tipa List<Korisnik>
            Assert.IsNotNull(korisnici);
            Assert.IsInstanceOf<List<Korisnik>>(korisnici);
        }
    


    [Test]
        public void Test1()
        {
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            _context.Dispose();
        }
    }
}