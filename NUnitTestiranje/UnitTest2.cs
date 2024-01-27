using Microsoft.AspNetCore.Mvc;
using Models;
using WebTemplate.Controllers;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace NUnitTestiranje
{
    internal class UnitTest2
    {
        private ProjekatContext _context;
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
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
        [Test]
        public async Task DodajKolekciju()
        {
            var controller = new KolekcijaController(_context);

            var novaKolekcija = new Kolekcija
            {
                NazivKolekcije = "Prolece 1900",
                Umetnik = "Pablo Pikaso",
                Stil = "Kubizam",
                Godina = 1902
                
            };

            var result = await controller.DodajKolekciju(novaKolekcija) as OkObjectResult;

            
            Assert.IsNotNull(result);
            Assert.AreEqual("Dodata je nova kolekcija", result.Value);
        }

        [Test]
        public async Task DodajKolekciju_PokusajDuplikata()
        {
            var controller = new KolekcijaController(_context);

            var postojećaKolekcija = new Kolekcija
            {
                NazivKolekcije = "Jesen 1900",
                Umetnik = "Pablo Pikaso",
                Stil = "Kubizam",
                Godina = 1902
                
            };

            var result = await controller.DodajKolekciju(postojećaKolekcija) as BadRequestObjectResult;


            Assert.IsNotNull(result);
            Assert.AreEqual("Kolekcija sa unetim nazivom vec postoji", result.Value);
        }

        [Test]
        public async Task DodajKolekcijuNeispravanUnos()
        {
            var controller = new KolekcijaController(_context);
            var neispravnaKolekcija = new Kolekcija
            {
                Umetnik = "Umetnik",
                Stil = "Moderan",
                Godina = 2022
            };
            var result = await controller.DodajKolekciju(neispravnaKolekcija) as BadRequestObjectResult;
            
            Assert.IsNotNull(result);
            Assert.AreEqual("Morate uneti naziv", result.Value);
        }

        [Test]

        public async Task VratiKolekcijeOkResult()
        {

            var controller = new KolekcijaController(_context);

            var result = await controller.VratiKolekcije();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task VratiKolekcijeIUmetnickaDela()
        {
            var controller = new KolekcijaController(_context);

            var result = await controller.VratiKolekcije() as OkObjectResult;
            var kolekcije = result?.Value as List<Kolekcija>; 

            Assert.IsNotNull(kolekcije);
            Assert.IsTrue(kolekcije.All(k => k.UmetnickaDela != null));
        }

        [Test]
        public async Task VratiKolekcijePrazanNiz()
        {
            var controller = new KolekcijaController(_context);

            var result = await controller.VratiKolekcije() as BadRequestObjectResult;
            var kolekcije = result?.Value as List<Kolekcija>;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);

            Assert.AreEqual("Nepostoji ni jedna kolekcija.", result.Value);
        }

        [Test]
        public async Task ObrisiKolekciju()
        {
            var controller = new KolekcijaController(_context);
            var novaKolekcija = new Kolekcija
            {
                NazivKolekcije = "Leto 1900",
                Umetnik = "Pablo Pikaso",
                Stil = "Kubizam",
                Godina = 1905

            };
            var dodajResult = await controller.DodajKolekciju(novaKolekcija);
            var kolekcija = await _context.Kolekcije!.Where(p=>p.NazivKolekcije==novaKolekcija.NazivKolekcije).FirstOrDefaultAsync();
            var kolekcijaId = kolekcija.ID;

            var obrisiResult = await controller.ObrisiKolekciju(kolekcijaId) as OkObjectResult;

           
            Assert.IsNotNull(obrisiResult);
            Assert.AreEqual($"Kolekcija je obrisana: {novaKolekcija.NazivKolekcije}", obrisiResult!.Value);
        }

        [Test]
        public async Task ObrisiKolekcijuNepostojecaKolekcija()
        {
            var controller = new KolekcijaController(_context);
            var nepostojeciId = 1;
            var result = await controller.ObrisiKolekciju(nepostojeciId);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Nepostojeca kolekcija", badRequestResult.Value);
        }


        [Test]
        public async Task TestObrisiKolekcijuLosID()
        {

            
            var controller = new KolekcijaController(_context);


            var kolekcijaId = -2;

            var result = await controller.ObrisiKolekciju(kolekcijaId) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("ID mora biti veći od nule", result.Value);
        }

        [Test]
        public async Task TestIzmeniKolekciju()
        {
            var controller = new KolekcijaController(_context);

            var postojecaKolekcija = new Kolekcija
            {
                NazivKolekcije = "Nova Jesen 1900",
                Umetnik = "Pablo Pikaso",
                Godina = 1900,
                Stil = "Kubizam"
            };


            var result = await controller.IzmeniKolekciju(2, postojecaKolekcija);

            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Kolekcija uspešno izmenjena", okResult.Value);

        }

        [Test]
        public async Task TestIzmeniKolekcijuNepostojecaKolekcija()
        {
            var controller = new KolekcijaController(_context);

            var postojecaKolekcija = new Kolekcija
            {
                NazivKolekcije = "Nova Jesen 1900",
                Umetnik = "Pablo Pikaso",
                Godina = 1900,
                Stil = "Kubizam"
            };


            var result = await controller.IzmeniKolekciju(1, postojecaKolekcija);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var badResult = result as NotFoundObjectResult;
            Assert.IsNotNull(badResult);
            Assert.AreEqual("Kolekcija nije pronađena", badResult.Value);

        }

        [Test]
        public async Task TestIzmeniKolekcijuNeispravanUnos()
        {

            var controller = new KolekcijaController(_context);
            var neispravniPodaci = new Kolekcija
            {
                // Nedostaje obavezno polje NazivKolekcije
                Godina = 2023,
                Stil = "Novi Stil"
            };

            var result = await controller.IzmeniKolekciju(2, neispravniPodaci) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Nedostaje naziv kolekcije", result.Value);
        }

        [Test]
        public async Task TestDodajUmetnickoDelo()
        {
            var controller = new KolekcijaController(_context);
            
            var umetnickoDelo = new DodajUmetnickoDelo
            {
                Slika = "Slika2.jpg",
                NazivKolekcije = "Nova Jesen 1900",
                Naziv = "Slika 2",
                Tehnika = "Akrilik",
                Dimenzije = "50x70",
                Cena = 1000,
                Ram = "Drvo"
            };

            var result = await controller.DodajUmetnickoDelo(umetnickoDelo) as OkObjectResult;

            
            Assert.IsNotNull(result);
            Assert.AreEqual("Umetničko delo uspešno dodato u kolekciju", result.Value);
        }
        [Test]
        public async Task TestDodajUmetnickoDeloNepostojecaKolekcija()
        {
            var controller = new KolekcijaController(_context);

            var delo = new DodajUmetnickoDelo
            {
                Slika = "Nepostojeca.jpg",
                NazivKolekcije = "Nepostojeca Kolekcija",
                Naziv = "Novo Umetnicko Delo",
                Tehnika = "Akrilik",
                Dimenzije = "50x70",
                Cena = 100,
                Ram = "Drvo"
            };

            var result = await controller.DodajUmetnickoDelo(delo) as BadRequestObjectResult;

            
            Assert.IsNotNull(result);
            Assert.AreEqual("Kolekcija sa datim imenom ne postoji", result.Value);
        }

        [Test]
        public async Task TestDodajUmetnickoDeloNeispravniPodaci()
        {
            var controller = new KolekcijaController(_context);
      
            var neispravniPodaci = new DodajUmetnickoDelo
            {
                Slika="NovaJesen.jpg",
                NazivKolekcije = "Sreca",
                Naziv = "Novo Umetnicko Delo",
                Tehnika = "Akrilik",
                Dimenzije = "50x70",
                Cena = -100, // negativna cena
                Ram = "Drvo"
            };

            var result = await controller.DodajUmetnickoDelo(neispravniPodaci) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode); // Provera statusa BadRequest
            Assert.AreEqual("Cena mora biti veća od 0.", result.Value);
        }

        [Test]
        public async Task TestVratiUmetnickaDelaOkResult()
        {
            var controller = new KolekcijaController(_context);
            var result = await controller.VratiUmetnickaDela();

            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            

        }

        [Test]
        public async Task TestVratiUmetnickaDelIKolekcije()
        {
            var controller = new KolekcijaController(_context);
            var result = await controller.VratiUmetnickaDela();

            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var umetnickaDela = okResult.Value as List<UmetnickoDelo>;
            Assert.IsNotNull(umetnickaDela);
            Assert.IsTrue(umetnickaDela.Count > 0);

        }

        [Test]
        public async Task TestVratiUmetnickaDelaPrazanNiz()
        {

            var controller = new KolekcijaController(_context);

            var result = await controller.VratiUmetnickaDela() as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);

            Assert.AreEqual("Nepostoji umetnicko delo.", result.Value);
        }

        [Test]
        public async Task TestIzmeniUmetnickoDeloOk()
        {
            var controller = new KolekcijaController(_context);
            
            var result = await controller.IzmeniUmetnickoDelo(2, "Slika 1") as OkObjectResult;

      
            Assert.IsNotNull(result);
            Assert.AreEqual("Umetničko delo uspešno izmenjeno", result.Value);
            
        }

        [Test]
        public async Task TestIzmeniUmetnickoDeloNepostojecaStavka()
        {

            var controller = new KolekcijaController(_context);
            var nepostojeciId = 1;

            var result = await controller.IzmeniUmetnickoDelo(nepostojeciId, "Novi Naziv");
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Umetničko delo nije pronađeno", notFoundResult.Value);
        }

        [Test]
        public async Task ObrisiUmetnickoDeloOk()
        {
            var controller = new KolekcijaController(_context);
                //mora prvo da ima u bazi id 6
                var brisanje = await controller.ObrisiUmetnickoDelo(6);

                Assert.IsInstanceOf<OkObjectResult>(brisanje);

                var okResult = brisanje as OkObjectResult;
                Assert.IsNotNull(okResult);
            
        }
        [Test]
        public async Task TestObrisiUmetnickoDeloPogresanID()
        {
            var controller = new KolekcijaController(_context);
            var nepostojeciArtworkId = 2;

            var result = await controller.ObrisiUmetnickoDelo(nepostojeciArtworkId);


            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var notFoundResult = result as BadRequestObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Nepostojece delo", notFoundResult.Value);
        }

        [Test]
        public async Task TestObrisiUmetnickoDeloNegativanID()
        {
            var controller = new KolekcijaController(_context);
            var nepostojeciArtworkId = -2;

            var result = await controller.ObrisiUmetnickoDelo(nepostojeciArtworkId);


            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var notFoundResult = result as BadRequestObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("ID mora biti pozitivan broj", notFoundResult.Value);
        }
    }

}
