namespace WebTemplate.Controllers;
using Models;
using Entities;

[ApiController]
[Route("[controller]")]
public class KorisnikController : ControllerBase
{
    public ProjekatContext Context { get; set; }

    public KorisnikController(ProjekatContext context)
    {
        Context = context;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterKorisnik korisnik)
    {
        if(string.IsNullOrEmpty(korisnik.Email)){
            return BadRequest("Morate uneti email");
        }
        var provera= await Context.Korisnici!.Where(p=>p.Email==korisnik.Email).FirstOrDefaultAsync();
        if(provera!=null)
        {
            return BadRequest("Postoji registracija sa unetim emailom");
        }
        string passwordHash=BCrypt.Net.BCrypt.HashPassword(korisnik.Password);
        try{
            var k=new Korisnik();
            k.Broj=korisnik.Broj;
            k.Email=korisnik.Email;
            k.Grad=korisnik.Grad;
            k.Ime=korisnik.Ime;
            k.KontaktTelefon=korisnik.KontaktTelefon;
            k.Prezime=korisnik.Prezime;
            k.Ulica=korisnik.Ulica;
            k.PasswordHash=passwordHash;
            Context.Korisnici!.Add(k);
            await Context.SaveChangesAsync(); 
            return Ok("Registrovan je novi korisnik");

        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login( [FromBody]LoginKorisnik korisnik)
    {
        try{
            
            if(string.IsNullOrWhiteSpace(korisnik.Email)||korisnik.Email.Length > 320)
                return BadRequest("Nevalidni podaci");
            if(string.IsNullOrWhiteSpace(korisnik.Password))
                return BadRequest("Morate da unesete sifru!");
            
            Korisnik user= new Korisnik();
            var k= await Context.Korisnici!.Where(p=> p.Email.Equals(korisnik.Email)).ToListAsync();
            
            if(k.Count<1)
                return BadRequest("Nevazeci username");

            user=k.First();
            var s= BCrypt.Net.BCrypt.Verify(korisnik.Password,user.PasswordHash);

            if(s)
            {
   
    return Ok("Uspesan Login");
            }
            else
            {
                return BadRequest("Pogresna sifra");
            }

        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("PrikaziKorisnike")]
    public async Task<IActionResult> VratiKorisnike()
    {
        try{
            var korisnici = await Context.Korisnici!.ToListAsync();

            if (korisnici.Count == 0)
            {
                // Ako je niz korisnika prazan, vrati BadRequest s odgovarajućom porukom
                return BadRequest("Nema korisnika.");
            }

            return Ok(korisnici);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("ObrisiKorisnika/{id}")]
    public async Task<IActionResult> ObrisiKorisnika(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("ID mora biti veći od nule");
            }
            var korisnik = await Context.Korisnici!.FirstOrDefaultAsync(p => p.ID == id);

            if (korisnik == null)
            {
                return BadRequest("Korisnik sa datim ID ne postoji");
            }

            Context.Korisnici!.Remove(korisnik);
            await Context.SaveChangesAsync();
            return Ok("Uspesno brisanje");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpPost("PromeniSifru")]
    public async Task<IActionResult> PromeniSifru(string email, string newPassword)
    {
        try
        {
            if (newPassword.Length<6) {
                return BadRequest("Sifra mora da bude duza od 6 karaktera!");
            }
            if (!newPassword.Any(p => Char.IsDigit(p)))
            {
                return BadRequest("Sifra mora da sadrzi makar jedan broj!");
            }
            if (!newPassword.Any(p => !Char.IsLetterOrDigit(p)))
            {
                return BadRequest("Sifra mora da sadrzi makar jedan specijalni karakter! (*-+_@&%$)");
            }
            if (!newPassword.Any(p => Char.IsUpper(p)))
            {
                return BadRequest("Sifra mora da sadrzi makar jedno veliko slovo!");
            }

            var user = await Context.Korisnici!.Where(p => p.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("Nepostojeci email!");
            }
            else
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.PasswordHash = passwordHash;

                Context.Korisnici!.Update(user);
                await Context.SaveChangesAsync();

                return Ok("Šifra je uspešno promenjena!");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


}
