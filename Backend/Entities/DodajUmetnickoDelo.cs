namespace Entities;
public class DodajUmetnickoDelo
{

   public string Naziv { get; set; }=null!;
    public string Tehnika { get; set; }=null!;
    public string Dimenzije { get; set; }=null!;
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Cena mora biti veća od 0.")]
    public int Cena { get; set; }
    public string Ram { get; set; }=null!;
    public string NazivKolekcije { get; set; }=null!;
    public string Slika { get; set; } = null!;

}