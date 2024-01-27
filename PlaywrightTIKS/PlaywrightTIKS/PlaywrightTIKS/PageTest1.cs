using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PlaywrightTIKS
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
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
        public async Task IzmenaLozinke()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.Locator("text=Zaboravili ste lozinku?").ClickAsync();
            await page.Locator("input[type=\"text\"]").ClickAsync();
            await page.Locator("input[type=\"text\"]").FillAsync("kaca@proba.com");
            await page.Locator("input[type=\"text\"]").PressAsync("Tab");
            await page.Locator("input[type=\"password\"]").FillAsync("Katarin@123");
            void Page_Dialog_EventHandler(object sender, IDialog dialog)
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.DismissAsync();
                Page.Dialog -= Page_Dialog_EventHandler;
            }
            Page.Dialog += Page_Dialog_EventHandler;
            await page.ClickAsync("button:has-text('Izmeni password')");

        }

        [Test]
        public async Task RegisterKorisnik()
        {
            await page.GotoAsync("http://localhost:3000/");

            await page.Locator("text=Kreirajte nalog").ClickAsync();
            await page.ClickAsync("//label[text()='Ime:']/following-sibling::input");
            await page.FillAsync("//label[text()='Ime:']/following-sibling::input", "Petar");
            await page.ClickAsync("//label[text()='Prezime:']/following-sibling::input");
            await page.FillAsync("//label[text()='Prezime:']/following-sibling::input", "Trajkovic");

           
            await page.ClickAsync("//label[text()='Šifra:']/following-sibling::input[@type='password']");
            await page.FillAsync("//label[text()='Šifra:']/following-sibling::input[@type='password']", "PetarTrajkovic@123");
            await page.ClickAsync("//label[text()='Grad:']/following-sibling::input");
            await page.FillAsync("//label[text()='Grad:']/following-sibling::input", "Nis");
            await page.ClickAsync("//label[text()='Ulica:']/following-sibling::input");
            await page.FillAsync("//label[text()='Ulica:']/following-sibling::input", "Juzni bulevar");
            await page.ClickAsync("//label[text()='Broj:']/following-sibling::input");
            await page.FillAsync("//label[text()='Broj:']/following-sibling::input", "2");
            await page.ClickAsync("//label[text()='Email:']/following-sibling::input");
            await page.FillAsync("//label[text()='Email:']/following-sibling::input", "trajko@gmail.com");
            await page.ClickAsync("//label[text()='Broj telefona:']/following-sibling::input");
            await page.FillAsync("//label[text()='Broj telefona:']/following-sibling::input", "645554332");
            




            await page.ClickAsync("button:has-text('Registruj se')");
            void Page_Dialog1_EventHandler(object sender, IDialog dialog)
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.DismissAsync();
                Page.Dialog -= Page_Dialog1_EventHandler;
            }
            Page.Dialog += Page_Dialog1_EventHandler;
            
        }
        [Test]
        public async Task KupovinaUmetnicogDela()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.ClickAsync("input[type=\"text\"]");
            await page.FillAsync("input[type=\"text\"]", "kaca@proba.com");
            await page.PressAsync("input[type=\"text\"]", "Tab");

            await page.FillAsync("input[type=\"password\"]", "Katarin@123");
            await page.ClickAsync("button:has-text('Prijava')");
            await Expect(page).ToHaveTitleAsync("KA-TI");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/Screenshot1.png" });
            await page.ClickAsync("div.slika-kvadratic:nth-child(2) button");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/Screenshot2.png" });
            await page.ClickAsync(".ukloni-stavku");
            await page.ClickAsync("div.slika-kvadratic:nth-child(3) button"); 

            await page.ClickAsync("button:has-text('Plati')");
            page.Dialog += (_, dialog) =>
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.AcceptAsync();
            };
        }

        
    
        [TearDown]
        public async Task Teardown()
        {
            await page.CloseAsync();
            await browser.DisposeAsync();
        }

    }
}
