using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace PlaywrightTIKS
{
    [TestFixture]
    public class APITestsKolekcija: PlaywrightTest
    {
        IAPIRequestContext Request;

        [SetUp]
        public async Task SetUpAPITesting()
        {
            var headers = new Dictionary<string, string>
        {
            { "Accept", "application/json" }
        };

            Request = await Playwright.APIRequest.NewContextAsync(new()
            {
                BaseURL = "https://localhost:7193",
                ExtraHTTPHeaders = headers,
                IgnoreHTTPSErrors = true
            });
        }

     
        [Test]
        public async Task TestDodajKolekcijuOk()
        {
            await using var response = await Request.PostAsync("/Kolekcija/DodajKolekciju", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                DataObject = new
                {
                    NazivKolekcije = "Kolekcija Playwright",
                    Godina = 2022,
                    Stil = "Moderan",
                    Umetnik = "Kosta Kostic"
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));
        }

        [Test]
        public async Task DodajKolekcijuBadRequest()
        {
            await using var response = await Request.PostAsync("/Kolekcija/DodajKolekciju", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                DataObject = new
                {
                    NazivKolekcije = "",
                    Godina = 2022,
                    Stil = "Moderan",
                    Umetnik = "Kosta Kostic"
                }
            });

            Assert.That(response.Status, Is.EqualTo(400));
        }

        [Test]
        [TestCase(2)]
        public async Task TestIzmeniKolekcijuOk(int id)
        {
            await using var response = await Request.PutAsync($"/Kolekcija/IzmeniKolekciju/{id}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                DataObject = new
                {
                    NazivKolekcije = "Novi naziv playwright",
                    Godina = 2023,
                    Stil = "Avangarda",
                    Umetnik = "Nikola Markovic"

                }
            });

            Assert.That(response.Status, Is.EqualTo(200));
        }

        [Test]
        [TestCase(3)]
        public async Task TestIzmeniKolekcijuNotFound(int id)
        {
            await using var response = await Request.PutAsync($"/Kolekcija/IzmeniKolekciju/{id}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                DataObject = new
                {
                    NazivKolekcije = "Novo",
                    Godina = 2022,
                    Stil = "Moderan",
                    Umetnik = "Kosta Kostic"
                }
            });

            Assert.That(response.Status, Is.EqualTo(404));
        }

        [Test]
        public async Task TestVratiKolekcijeOK()
        {
            await using var response = await Request.GetAsync("/Kolekcija/VratiKolekcije");

            var body = await response.BodyAsync();
            var jsonString = Encoding.UTF8.GetString(body);
            var resultArr = JsonNode.Parse(jsonString)?.AsArray();

            Assert.Multiple(() =>
            {
                Assert.That(response.Status, Is.EqualTo(200));
                Assert.That(resultArr?.Count, Is.Not.EqualTo(0));
            });
        }

        [Test]
        public async Task TestVratiKolekcijeBadRequest()
        {

            await using var response = await Request.GetAsync("/Kolekcija/VratiKolekcije");

            Assert.That(response.Status, Is.EqualTo(400));
        }
        [Test]

        public async Task TestObrisiKolekcijuOK()
        {
            int id = 13;
            await using var response = await Request.DeleteAsync($"/Kolekcija/ObrisiKolekciju/{id}");

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task TestObrisiKolekcijuBadRequest()
        {
            int id = -1;
            await using var response = await Request.DeleteAsync($"/Kolekcija/ObrisiKolekciju/{id}");

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task TestDodajUmetnickoDeloOk()
        {
            await using var response = await Request.PostAsync("/Kolekcija/DodajUmetnickoDelo", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                DataObject = new
                {
                    Naziv= "Playwright delo",
                    Tehnika= "Vodene boje",
                    Dimenzije= "50x50",
                    Cena=5000,
                    Ram= "Drvo",
                    NazivKolekcije= "Kolekcija Playwright",
                    Slika= "playwright.jpg"
                   
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));
        }

        [Test]
        public async Task TestDodajUmetnickoDeloBadRequest()
        {
            await using var response = await Request.PostAsync("/Kolekcija/DodajUmetnickoDelo", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                DataObject = new
                {
                    Naziv = "Playwright delo",
                    Tehnika = "Vodene boje",
                    Dimenzije = "50x50",
                    Cena = 5000,
                    Ram = "Drvo",
                    NazivKolekcije = "Kolekcija Playwright Nepostojeca",
                    Slika = "playwright.jpg"

                }
            });

            Assert.That(response.Status, Is.EqualTo(400));
        }

        [Test]
        [TestCase(16,"Nova Playwright slika")]
        public async Task TestImeniUmetnickoDeloOk(int id, string noviNaziv)
        {
            await using var response = await Request.PutAsync($"/Kolekcija/IzmeniUmetnickoDelo/{id}/{noviNaziv}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                
            });

            Assert.That(response.Status, Is.EqualTo(200));
        }

        [Test]
        [TestCase(1, "Nova Playwright slika")]
        public async Task TestImeniUmetnickoDeloNotFound(int id, string noviNaziv)
        {
            await using var response = await Request.PutAsync($"/Kolekcija/IzmeniUmetnickoDelo/{id}/{noviNaziv}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },

            });

            Assert.That(response.Status, Is.EqualTo(404));
        }
        [Test]
        public async Task TestObrisiUmetnickoDeloOk()
        {
            int id = 16;
            await using var response = await Request.DeleteAsync($"/Kolekcija/ObrisiUmetnickoDelo/{id}");

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK));
        }
        [Test]
        public async Task TestObrisiUmetnickoDeloBadRequest()
        {
            int id = -1;
            await using var response = await Request.DeleteAsync($"/Korisnik/ObrisiKorisnika/{id}");

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task TestVratiUmetnickaDelaOK()
        {
            await using var response = await Request.GetAsync("/Kolekcija/VratiUmetnickaDela");

            var body = await response.BodyAsync();
            var jsonString = Encoding.UTF8.GetString(body);
            var resultArr = JsonNode.Parse(jsonString)?.AsArray();

            Assert.Multiple(() =>
            {
                Assert.That(response.Status, Is.EqualTo(200));
                Assert.That(resultArr?.Count, Is.Not.EqualTo(0));
            });
        }

        [Test]
        public async Task TestVratiUmetnickaDelaBadRequest()
        {
            await using var response = await Request.GetAsync("/Kolekcija/VratiUmetnickaDela");

            var body = await response.BodyAsync();
            var jsonString = Encoding.UTF8.GetString(body);
            var resultArr = JsonNode.Parse(jsonString)?.AsArray();

            Assert.Multiple(() =>
            {
                Assert.That(response.Status, Is.EqualTo(400));
            });
        }

    }
}
