using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTemplate.Controllers;

namespace NUnitTestiranje
{
    internal class UnitTest3
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
        public async Task TestNapraviNarudzbinuOk()
        {
            var controller = new NarudzbinaController(_context);

            var narudzbinaData = new DodajNarudzbinu
            {
                emailKorisnik = "test1@example.com",
                NazivaDela = new List<string> { "Poljubac", "Zagrljaj" },
                Kolicina = 2,
                UkupnaCena = 100000
            };

            var result = await controller.NapraviNarudzbinu(narudzbinaData) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Narudzbina je dodata", result.Value);
        }

        [Test]
        public async Task TestNapraviNarudzbinuNepostojeciKorisnik()
        {
            var controller = new NarudzbinaController(_context);

            var narudzbinaData = new DodajNarudzbinu
            {
                emailKorisnik = "test@example.com",
                NazivaDela = new List<string> { "Poljubac", "Zagrljaj" },
                Kolicina = 2,
                UkupnaCena = 10000
            };

            var result = await controller.NapraviNarudzbinu(narudzbinaData) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Korisnik nije pronadjen", result.Value);
        }

        [Test]
        public async Task TestNapraviNarudzbinuNepostojeceDelo()
        {
            var controller = new NarudzbinaController(_context);

            var narudzbinaData = new DodajNarudzbinu
            {
                emailKorisnik = "test1@example.com",
                NazivaDela = new List<string> { "Poljubac", "NazivDela2" },
                Kolicina = 2,
                UkupnaCena = 100000
            };

            var result = await controller.NapraviNarudzbinu(narudzbinaData) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Neki od naziva dela nisu pronađeni", result.Value);
        }

        [Test]
        public async Task TestVratiNarudzbineOk()
        {
            var controller = new NarudzbinaController(_context);

            
            var result = await controller.VratiNarudzbine() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

                      
        }
        [Test]
        public async Task TestVratiNarudzbineNemaNarudzbina()
        {
            var controller = new NarudzbinaController(_context);

            var result = await controller.VratiNarudzbine() as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("Nema narudzbina", result.Value);
        }
        

        [Test]
        public async Task TestObrisiNarudzbinuOk()
        {
            var controller = new NarudzbinaController(_context);

            var obrisiNarudzbinuResult = await controller.ObrisiNarudzbinu(12) as OkObjectResult;

            Assert.IsNotNull(obrisiNarudzbinuResult);
            Assert.AreEqual(StatusCodes.Status200OK, obrisiNarudzbinuResult.StatusCode);
            Assert.AreEqual($"Narudzbina sa ID {12} je uspešno obrisana", obrisiNarudzbinuResult.Value);

        }
        [Test]
        public async Task TestObrisiNarudzbinuNotFoind()
        {

            var controller = new NarudzbinaController(_context);

            var result = await controller.ObrisiNarudzbinu(12) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("Narudzbina nije pronađena", result.Value);
        }
        [Test]
        public async Task TestObrisiNarudzbinuNegativanId()
        {

            var controller = new NarudzbinaController(_context);

            var result = await controller.ObrisiNarudzbinu(-1) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("ID narudžbine ne može biti manji od 0.", result.Value);
        }

        [Test]
        public async Task TestIzmeniNarudzbinuOk()
        {

            var controller = new NarudzbinaController(_context);

            var result = await controller.IzmeniNarudzbinu(14, 15000) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual($"Narudzbina sa ID {1} je uspešno izmenjena", result.Value);
        }

        [Test]
        public async Task TestIzmeniNarudzbinuNepostojecaNarudzbina()
        {

            var controller = new NarudzbinaController(_context);

            var result = await controller.IzmeniNarudzbinu(12, 15000) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("Narudzbina nije pronađena", result.Value);
        }

        [Test]
        public async Task TestIzmeniNarudzbinuNegativnaCena()
        {
            var controller = new NarudzbinaController(_context);

            var result = await controller.IzmeniNarudzbinu(14, -500) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Cena mora biti pozitivan broj", result.Value);
        }

    }
}
