using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace PlaywrightTIKS
{
    [TestFixture]

    public class APITestsUser : PlaywrightTest
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
        public async Task TestLoginOK()
        {
            await using var response = await Request.PostAsync("/Korisnik/Login",new()
            {
                Headers= new Dictionary<string, string>() {
                    {
                        "Content-Type", "application/json"
                    } },
                DataObject= new
                {
                    email="kaca@proba.com",
                    password= "Katarin@123"
                }
            });
            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task TestLoginBadRequest()
        {
            await using var response = await Request.PostAsync("/Korisnik/Login", new()
            {
                Headers = new Dictionary<string, string>() {
                    {
                        "Content-Type", "application/json"
                    } },
                DataObject = new
                {
                    email = "kaca@gmail.com",
                    password = "Katarin@123"
                }
            });
            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task TestRegisterOk()
        {
            await using var response = await Request.PostAsync("/Korisnik/Register", new()
            {
                Headers = new Dictionary<string, string>() {
                    {
                        "Content-Type", "application/json"
                    } },
                DataObject = new
                {
                    email = "tijana@proba.com",
                    password = "Tijan@123",
                    ime="Tijana",
                    prezime="Ilic",
                    broj=34,
                    grad="Nis",
                    ulica="Petra Petrovica",
                    kontaktTelefon=643578999
                }
            });
            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK));
        }
        [Test]
        public async Task TestRegisterBadRequest()
        {
            await using var response = await Request.PostAsync("/Korisnik/Register", new()
            {
                Headers = new Dictionary<string, string>() {
                    {
                        "Content-Type", "application/json"
                    } },
                DataObject = new
                {
                    email = "tijana@proba.com",
                    password = "Tijan@123",
                    ime = "Tijana",
                    prezime = "Ilic",
                    broj = 34,
                    grad = "Nis",
                    ulica = "Petra Petrovica",
                    kontaktTelefon = 643578999
                }
            });
            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task TestPromeniSifruOk()
        {
            await using var response = await Request.PostAsync("/Korisnik/PromeniSifru", new()
            {
                Headers = new Dictionary<string, string>() {
                    {
                        "Content-Type", "application/json"
                    } },
                DataObject = new
                {
                    Email = "tica@proba.com",
                    NewPassword = "Tijan@!123"
                }
            });
            Console.WriteLine($"Status odgovora: {response.Status}");

            var responseContent = await response.TextAsync();
            Console.WriteLine($"Odgovor sa servera: {responseContent}");
            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK));
        }
        [Test]
        public async Task TestPromeniSifruBadRequest()
        {
            await using var response = await Request.PostAsync("/Korisnik/PromeniSifru", new()
            {
                Headers = new Dictionary<string, string>() {
                    {
                        "Content-Type", "application/json"
                    } },
                DataObject = new
                {
                    email = "tijana@gmail.com",
                    password = "Tijana!123"
                }
            });
            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]

        public async Task TestObrisiKorisnikaOk()
        {
            int id = 9;
            await using var response = await Request.DeleteAsync($"/Korisnik/ObrisiKorisnika/{id}");

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task TestObrisiKorisnikaBadRequest()
        {
            int id = 10000;
            await using var response = await Request.DeleteAsync($"/Korisnik/ObrisiKorisnika/{id}");

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task TestVratiKorisnikeOK()
        {
            await using var response = await Request.GetAsync("/Korisnik/PrikaziKorisnike");

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
        public async Task TestVratiKorisnikeBadRequest()
        {

            await using var response = await Request.GetAsync("/Korisnik/PrikaziKorisnike");

            Assert.That(response.Status, Is.EqualTo(400));
        }

    }
            

}
