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
    internal class APITestsNarudzbina : PlaywrightTest
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
        public async Task TestDodajNarudzbinuOk()
        {
            await using var response = await Request.PostAsync("/Narudzbina/NapraviNarudzbinu", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                DataObject = new
                {
                    datum = DateTime.Now,
                    kolicina = 1,
                    emailKorisnik = "tica@proba.com",
                    ukupnaCena = "200000",
                    nazivaDela= new List<string> { "Poljubac", "Porodica" }
                }
            });
            Console.WriteLine($"Status odgovora: {response.Status}");

            var responseContent = await response.TextAsync();
            Console.WriteLine($"Odgovor sa servera: {responseContent}");

            Assert.That(response.Status, Is.EqualTo(200));
        }

        [Test]
        public async Task TestDodajNarudzbinuBadRequest()
        {
            await using var response = await Request.PostAsync("/Narudzbina/NapraviNarudzbinu", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                DataObject = new
                {
                    datum = DateTime.Now,
                    kolicina = 1,
                    emailKorisnik = "tica@gmail.com",
                    ukupnaCena = "200000",
                    nazivaDela = new List<string> { "Poljubac", "Porodica" }

                }
            });
            var responseContent = await response.TextAsync();
            Console.WriteLine($"Odgovor sa servera: {responseContent}");

            Assert.That(response.Status, Is.EqualTo(400));
        }
        [Test]
        public async Task TestIzmeniNarudzbinuBadRequest()
        {
            int narudzbinaId = 1;
            int novaCena = 1500;

            await using var response = await Request.PostAsync("/Narudzbina/IzmeniNarudzbina", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                DataObject = new
                {
                    narudzbinaId,
                    novaCena
                }
            });
            Console.WriteLine($"Status odgovora: {response.Status}");

            var responseContent = await response.TextAsync();
            Console.WriteLine($"Odgovor sa servera: {responseContent}");

            Assert.That(response.Status, Is.EqualTo(404));
        }

        [Test]
        public async Task TestVratiNarudzbinuOK()
        {
            await using var response = await Request.GetAsync("/Narudzbina/VratiNarudzbine");

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
        public async Task TestVratiNarudzbinuBadRequest()
        {

            await using var response = await Request.GetAsync("/Narudzbina/VratiNarudzbine");

            Assert.That(response.Status, Is.EqualTo(400));
        }
        [Test]
        public async Task TestObrisiNarudzbinuOK()
        {
            int id = 33;
            await using var response = await Request.DeleteAsync($"/Narudzbina/ObrisiNarudzbinu/{id}");

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task TestObrisiNarudzbinuBadRequest()
        {
            int id = -1;
            await using var response = await Request.DeleteAsync($"/Narudzbina/ObrisiNarudzbinu/{id}");

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

    }
}
