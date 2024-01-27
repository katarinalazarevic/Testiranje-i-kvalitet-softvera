namespace Entities;
public class DodajNarudzbinu
{
    public List<string> NazivaDela { get; set; } = new List<string>();
    public string emailKorisnik { get; set; }=null!;
    public int Kolicina { get; set; }
   
    public int UkupnaCena {get; set;}
}