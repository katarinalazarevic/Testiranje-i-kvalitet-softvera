namespace Models;
public class Narudzbina
{
    [Key]
    public int ID{get; set;}

    public DateTime Datum { get; set; }
    public int Kolicina { get; set; }
    public List<UmetnickoDelo> UmetnickaDela { get; set; }=null!;
    public Korisnik Korisnik { get; set; }=null!;
    public int UkupnaCena {get; set;}
    
}