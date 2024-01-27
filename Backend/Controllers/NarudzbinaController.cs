namespace WebTemplate.Controllers;
using Models;
using Entities;

[ApiController]
[Route("[controller]")]
public class NarudzbinaController : ControllerBase
{
    public ProjekatContext Context { get; set; }

    public NarudzbinaController(ProjekatContext context)
    {
        Context = context;
    }

    [HttpPost("NapraviNarudzbinu")]
    public async Task<IActionResult> NapraviNarudzbinu([FromBody] DodajNarudzbinu nar){
        try{
            var korisnik=await Context.Korisnici!.Where(p=>p.Email==nar.emailKorisnik).FirstOrDefaultAsync();
            if(korisnik==null){
                return BadRequest("Korisnik nije pronadjen");

            }
            var svaUmetnickaDela = await Context.UmetnickaDela!
            .Where(p => nar.NazivaDela.Contains(p.Naziv))
            .ToListAsync();

            // Proveri da li sva umetnička dela postoje
            if (svaUmetnickaDela.Count != nar.NazivaDela.Count)
            {
                return BadRequest("Neki od naziva dela nisu pronađeni");
            }

            Narudzbina novaNar = new Narudzbina();
            novaNar.Datum = DateTime.Now;
            novaNar.Kolicina = nar.Kolicina;
            novaNar.Korisnik = korisnik;
            novaNar.UkupnaCena = nar.UkupnaCena;

            novaNar.UmetnickaDela = svaUmetnickaDela;

            Context.Narudzbine!.Add(novaNar);
            await Context.SaveChangesAsync();

            return Ok("Narudzbina je dodata");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet("VratiNarudzbine")]
    public async Task<IActionResult> VratiNarudzbine()
    {
        try
        {
            var narudzbine = await Context.Narudzbine!
    .Include(p => p.Korisnik)
    .Include(p => p.UmetnickaDela)
    .Select(n => new
    {
        NarudzbinaId = n.ID,
        Datum = n.Datum,
        UkupnaCena = n.UkupnaCena,
        NarucilacEmail = n.Korisnik.Email,
        UmetnickaDela = n.UmetnickaDela.Select(um => um.Naziv)
    })
    .ToListAsync();
            if (narudzbine.Count < 0)
            {
                return NotFound("Nema narudzbina");
            }
            else
            {
                return Ok(narudzbine);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpDelete("ObrisiNarudzbinu/{narudzbinaId}")]
    public async Task<IActionResult> ObrisiNarudzbinu(int narudzbinaId)
    {
         if (narudzbinaId <= 0)
    {
        return BadRequest("ID narudžbine ne može biti manji od 0.");
    }

    var narudzbina = await Context.Narudzbine!.FindAsync(narudzbinaId);

    if (narudzbina == null)
    {
        return NotFound("Narudzbina nije pronađena");
    }

    var umetnickaDelaSaNarudzbinom = await Context.UmetnickaDela!
        .Where(umetnickoDelo => umetnickoDelo.Narudzbina!.ID == narudzbinaId)
        .ToListAsync();

    foreach (var umetnickoDelo in umetnickaDelaSaNarudzbinom)
    {
        umetnickoDelo.Narudzbina = null;
    }

    Context.Narudzbine.Remove(narudzbina);
    await Context.SaveChangesAsync();

    return Ok($"Narudzbina sa ID {narudzbinaId} je uspešno obrisana");

    }
    [HttpPost("IzmeniNarudzbina")]
    public async Task<IActionResult> IzmeniNarudzbinu(int narudzbinaId,  int novaCena)
    {
        var narudzbina = await Context.Narudzbine!.FindAsync(narudzbinaId);

        if (narudzbina == null)
        {
            return NotFound("Narudzbina nije pronađena");
        }
        
       if(novaCena<0)
        {
            return BadRequest("Cena mora biti pozitivan broj");
        }
        narudzbina.UkupnaCena = novaCena;
        

        Context.Update(narudzbina);
        await Context.SaveChangesAsync();

        return Ok($"Narudzbina sa ID {narudzbinaId} je uspešno izmenjena");
    }



}
