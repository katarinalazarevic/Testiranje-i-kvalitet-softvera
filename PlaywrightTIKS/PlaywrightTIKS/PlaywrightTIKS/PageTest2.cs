using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTIKS
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    internal class PageTest2 : PageTest
    {
        IPage page;
        IBrowser browser;
        [SetUp]
        public async Task Setup()
        {
            browser = await Playwright.Chromium.LaunchAsync(new()
            {
                Headless = false,
                SlowMo = 2000
            });

            page = await browser.NewPageAsync(new()
            {
                ViewportSize = new()
                {
                    Width = 1280,
                    Height = 720
                },
                ScreenSize = new()
                {
                    Width = 1280,
                    Height = 720
                },
                RecordVideoSize = new()
                {
                    Width = 1280,
                    Height = 720
                },
                RecordVideoDir = "../../../Videos"
            });
        }

        [Test]
        public async Task ProveriKorisnika()
        {
            await page.GotoAsync("http://localhost:3000/");

            await page.ClickAsync("input[type=\"text\"]");
            await page.FillAsync("input[type=\"text\"]", "ilija@admin.com");
            await page.PressAsync("input[type=\"text\"]", "Tab");

            await page.FillAsync("input[type=\"password\"]", "IlijaPetrivic@123");
            await page.ClickAsync("button:has-text('Prijava')");

            await page.ClickAsync("button:has-text('Korisnik')");
            await page.ClickAsync("button:has-text('Vrati sve korisnike')");

            await page.ClickAsync("input[placeholder='ID za brisanje']");
            await page.FillAsync("input[placeholder='ID za brisanje']", "12");
            await page.ClickAsync("button:has-text('Obrisi')");
            page.Dialog += (_, dialog) =>
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.AcceptAsync();
            };
            await page.ClickAsync("button:has-text('Vrati sve korisnike')");


        }

        [Test]
        public async Task ProveriKolekciju()
        {
            await page.GotoAsync("http://localhost:3000/");

            await page.ClickAsync("input[type=\"text\"]");
            await page.FillAsync("input[type=\"text\"]", "ilija@admin.com");
            await page.PressAsync("input[type=\"text\"]", "Tab");

            await page.FillAsync("input[type=\"password\"]", "IlijaPetrivic@123");
            await page.ClickAsync("button:has-text('Prijava')");

            await page.ClickAsync("button:has-text('Kolekcija')");
            await page.ClickAsync("button:has-text('Vrati sve kolekcije')");

            await page.ClickAsync("input[placeholder='Naziv kolekcije novo']");
            await page.FillAsync("input[placeholder='Naziv kolekcije']", "Front test");
            await page.ClickAsync("input[placeholder='Umetnik']");
            await page.FillAsync("input[placeholder='Umetnik']", "Front testic");
            await page.ClickAsync("input[placeholder='Godina']");
            await page.FillAsync("input[placeholder='Godina']", "2023");
            await page.ClickAsync("input[placeholder='Stil']");
            await page.FillAsync("input[placeholder='Stil']", "front ");
            
            await page.ClickAsync("button:has-text('Dodaj')");
            page.Dialog += (_, dialog) =>
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.AcceptAsync();
            };
            await page.ClickAsync("button:has-text('Vrati sve kolekcije')");

            //bice otkomentarisano na odbrani, da bi bio validan id za brisanje
            //await page.ClickAsync("input[placeholder='ID za brisanje']");
            //await page.FillAsync("input[placeholder='ID za brisanje']", "14");
            //await page.ClickAsync("button:has-text('Obrisi')");

            await page.ClickAsync("input[placeholder='ID']");
            await page.FillAsync("input[placeholder='ID']", "2");

            await page.ClickAsync("input[placeholder='Novi naziv kolekcije']");
            await page.FillAsync("input[placeholder='Novi naziv kolekcije']", "Promena");
            await page.ClickAsync("input[placeholder='Novi umetnik']");
            await page.FillAsync("input[placeholder='Novi umetnik']", "novi umetnik");
            await page.ClickAsync("input[placeholder='Nova godina']");
            await page.FillAsync("input[placeholder='Nova godina']", "2024");
            await page.ClickAsync("input[placeholder='Novi stil']");
            await page.FillAsync("input[placeholder='Novi stil']", "novi ");
            await page.ClickAsync("button:has-text('Izmeni')");
            page.Dialog += (_, dialog) =>
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.AcceptAsync();
            };
            await page.ClickAsync("button:has-text('Vrati sve kolekcije')");

        }

        [Test]
        public async Task ProveriUmetnickodelo()
        {
            await page.GotoAsync("http://localhost:3000/");

            await page.ClickAsync("input[type=\"text\"]");
            await page.FillAsync("input[type=\"text\"]", "ilija@admin.com");
            await page.PressAsync("input[type=\"text\"]", "Tab");

            await page.FillAsync("input[type=\"password\"]", "IlijaPetrivic@123");
            await page.ClickAsync("button:has-text('Prijava')");

            await page.ClickAsync("button:has-text('Umetnicko Delo')");
            await page.ClickAsync("button:has-text('Vrati Umetnicka Dela')");

            await page.ClickAsync("input[placeholder='Naziv kolekcije']");
            await page.FillAsync("input[placeholder='Naziv kolekcije']", "Novo radjanje");
            await page.ClickAsync("input[placeholder='Naziv umetnickog dela']");
            await page.FillAsync("input[placeholder='Naziv umetnickog dela']", "Novo delo 3");
            await page.ClickAsync("input[placeholder='Tehnika']");
            await page.FillAsync("input[placeholder='Tehnika']", "Vodene boje");
            await page.ClickAsync("input[placeholder='Dimenzije']");
            await page.FillAsync("input[placeholder='Dimenzije']", "50x50");
            await page.ClickAsync("input[placeholder='Cena']");
            await page.FillAsync("input[placeholder='Cena']", "15000");
            await page.ClickAsync("input[placeholder='Ram']");
            await page.FillAsync("input[placeholder='Ram']", "Nema");
            await page.ClickAsync("input[placeholder='Slika']");
            await page.FillAsync("input[placeholder='Slika']", "zaBrisanje.jpg");
            await page.ClickAsync("button:has-text('Dodaj')");
            page.Dialog += (_, dialog) =>
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.AcceptAsync();
            };

            await page.ClickAsync("button:has-text('Vrati Umetnicka Dela')");

            await page.ClickAsync("input[placeholder='ID umetnickog dela']");
            await page.FillAsync("input[placeholder='ID umetnickog dela']", "17");

            await page.FillAsync("input[placeholder='Novi naziv umetnickog dela']", "Novi naziv dela");
           

            await page.ClickAsync("button:has-text('Izmeni')");
            page.Dialog += (_, dialog) =>
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.AcceptAsync();
            };
            await page.ClickAsync("button:has-text('Vrati sve kolekcije')");

            //bice otkomentarisano na odbrani, da bi bio validan id za brisanje
            //await page.ClickAsync("input[placeholder='ID za brisanje']");
            //await page.FillAsync("input[placeholder='ID za brisanje']", "18");
            //await page.ClickAsync("button:has-text('Obrisi')");
            //await page.ClickAsync("button:has-text('Vrati sve kolekcije')");


        }


        [Test]
        public async Task ProveriNarudzbinu()
        {
            await page.GotoAsync("http://localhost:3000/");

            await page.ClickAsync("input[type=\"text\"]");
            await page.FillAsync("input[type=\"text\"]", "ilija@admin.com");
            await page.PressAsync("input[type=\"text\"]", "Tab");

            await page.FillAsync("input[type=\"password\"]", "IlijaPetrivic@123");
            await page.ClickAsync("button:has-text('Prijava')");

            await page.ClickAsync("button:has-text('Narudzbina')");
            await page.ClickAsync("button:has-text('Vrati Narudzbine')");

            await page.ClickAsync("input[placeholder='ID za brisanje']");
            await page.FillAsync("input[placeholder='ID za brisanje']", "23"); //za proveru i 24, 25
            
            await page.ClickAsync("button:has-text('Obrisi')");
            page.Dialog += (_, dialog) =>
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.AcceptAsync();
            };
            await page.ClickAsync("button:has-text('Vrati Narudzbine')");

            

            await page.ClickAsync("input[placeholder='ID narudžbine']");
            await page.FillAsync("input[placeholder='ID narudžbine']", "32"); //34,35

            await page.FillAsync("input[placeholder='Nova ukupna cena']", "100000");


            await page.ClickAsync("button:has-text('Izmeni')");
            page.Dialog += (_, dialog) =>
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.AcceptAsync();
            };
            await page.ClickAsync("button:has-text('Vrati Narudzbine')");



        }

    }
}
