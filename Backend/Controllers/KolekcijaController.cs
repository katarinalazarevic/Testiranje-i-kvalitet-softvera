namespace WebTemplate.Controllers;
using Models;
using Entities;

[ApiController]
[Route("[controller]")]
public class KolekcijaController : ControllerBase
{
    public ProjekatContext Context { get; set; }

    public KolekcijaController(ProjekatContext context)
    {
        Context = context;
    }

    [HttpPost("DodajKolekciju")]
    public async Task<IActionResult> DodajKolekciju([FromBody] Kolekcija kolekcija)
    {
        try{
            if(string.IsNullOrEmpty(kolekcija.NazivKolekcije)){
                return BadRequest("Morate uneti naziv");
            }
            if(string.IsNullOrEmpty(kolekcija.Umetnik))
            {
                return BadRequest("Morate uneti umetnika");

            }
            if(string.IsNullOrEmpty(kolekcija.Stil))
            {
                return BadRequest("Morate uneti stil");

            }
            var ko= await Context.Kolekcije!.Where(p=> p.NazivKolekcije ==kolekcija.NazivKolekcije).FirstOrDefaultAsync();
            if(ko!=null){
                return BadRequest("Kolekcija sa unetim nazivom vec postoji");
            }
            else{
                var k=new Kolekcija();
                k!.NazivKolekcije=kolekcija.NazivKolekcije;
                k.Godina=kolekcija.Godina;
                k.Stil=kolekcija.Stil;
                k.Umetnik=kolekcija.Umetnik;

                Context.Kolekcije!.Add(k);
                await Context.SaveChangesAsync(); 
                return Ok("Dodata je nova kolekcija");
            }

        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }

    [HttpGet("VratiKolekcije")]
    public async Task<IActionResult> VratiKolekcije()
    {
        try{
            var kolekcije= await Context.Kolekcije!.Include(p=>p.UmetnickaDela).ToListAsync();
            if (kolekcije.Count == 0)
            {
                return BadRequest("Nepostoji ni jedna kolekcija.");
            }
            return Ok(kolekcije);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("ObrisiKolekciju/{id}")]

    public async Task<IActionResult> ObrisiKolekciju(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("ID mora biti veći od nule");
            }
            var kolekcija = await Context.Kolekcije!
                .Include(u =>u.UmetnickaDela ) 
                .FirstOrDefaultAsync(p=> p.ID==id);

            if (kolekcija == null)
            {
                return BadRequest("Nepostojeca kolekcija");
            }

            
            foreach (var delo in kolekcija.UmetnickaDela!.ToList())
            {
                Context.UmetnickaDela!.Remove(delo);
            }

            Context.Kolekcije!.Remove(kolekcija);
            await Context.SaveChangesAsync();
            
            return Ok($"Kolekcija je obrisana: {kolekcija.NazivKolekcije}");
        }
        catch (Exception e)
        {
            return BadRequest(e.InnerException?.Message ?? e.Message);
        }
    }

    [HttpPut("IzmeniKolekciju/{id}")]
    public async Task<IActionResult> IzmeniKolekciju(int id, [FromBody] Kolekcija izmena)
    {
        try
        {
            // Provera da li usluga sa datim ID postoji
            var kol = await Context.Kolekcije!.FindAsync(id);
            if (string.IsNullOrEmpty(izmena.NazivKolekcije))
                return BadRequest("Nedostaje naziv kolekcije");
            if (string.IsNullOrEmpty(izmena.Stil))
                return BadRequest("Nedostaje stil kolekcije");
            if (string.IsNullOrEmpty(izmena.Umetnik))
                return BadRequest("Nedostaje naziv umetnika kolekcije");
            if (kol == null)
            {
                return NotFound("Kolekcija nije pronađena");
            }



            kol.NazivKolekcije = izmena.NazivKolekcije;
            kol.Umetnik=izmena.Umetnik;
            kol.Godina = izmena.Godina;

            kol.Stil = izmena.Stil;
            Context.Kolekcije.Update(kol);
            await Context.SaveChangesAsync();

            return Ok("Kolekcija uspešno izmenjena");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("DodajUmetnickoDelo")]
    public async Task<IActionResult> DodajUmetnickoDelo([FromBody] DodajUmetnickoDelo delo)
    {
        try{
            var kolekcija=await Context.Kolekcije!.Where(p=>p.NazivKolekcije ==delo.NazivKolekcije).FirstOrDefaultAsync();
            if(kolekcija==null){
                return BadRequest("Kolekcija sa datim imenom ne postoji");
            }
            if (delo.Cena <= 0)
            {
                return BadRequest
                    ("Cena mora biti veća od 0.");
            }
            var novoUmetnickoDelo = new UmetnickoDelo
            {
                Slika = delo.Slika,
            Naziv = delo.Naziv,
            Tehnika = delo.Tehnika,
            Dimenzije = delo.Dimenzije,
            Cena = delo.Cena,
            Ram = delo.Ram 
        };

        novoUmetnickoDelo.Kolekcija=kolekcija;
        Context.UmetnickaDela!.Add(novoUmetnickoDelo);
        await Context.SaveChangesAsync();

        return Ok("Umetničko delo uspešno dodato u kolekciju");
            
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
     [HttpGet("VratiUmetnickaDela")]
    public async Task<IActionResult> VratiUmetnickaDela()
    {
        try{
            var dela= await Context.UmetnickaDela!.Include(p=>p.Kolekcija).ToListAsync();
            if (dela.Count == 0)
            {
                return BadRequest("Nepostoji umetnicko delo.");
            }
            return Ok(dela);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("IzmeniUmetnickoDelo/{id}/{noviNaziv}")]
public async Task<IActionResult> IzmeniUmetnickoDelo(int id, string noviNaziv)
{
    try
    {
        var umetnickoDelo = await Context.UmetnickaDela!.Where(p=>p.ID==id).FirstOrDefaultAsync();

        if (umetnickoDelo == null)
        {
            return NotFound("Umetničko delo nije pronađeno");
        }

        umetnickoDelo.Naziv = noviNaziv;
        
        Context.UmetnickaDela!.Update(umetnickoDelo);
        await Context.SaveChangesAsync();

        return Ok("Umetničko delo uspešno izmenjeno");
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}


    [HttpDelete("ObrisiUmetnickoDelo/{id}")]

    public async Task<IActionResult> ObrisiUmetnickoDelo(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("ID mora biti pozitivan broj");
            }
            var delo = await Context.UmetnickaDela!
                .Include(u =>u.Kolekcija ) 
                .FirstOrDefaultAsync(p=> p.ID==id);

            if (delo == null)
            {
                return BadRequest("Nepostojece delo");
            }

            Context.UmetnickaDela!.Remove(delo);
            await Context.SaveChangesAsync();
            
            return Ok($"Umetnicko delo je obrisano: {delo.Naziv}");
        }
        catch (Exception e)
        {
            return BadRequest(e.InnerException?.Message ?? e.Message);
        }
    }

}