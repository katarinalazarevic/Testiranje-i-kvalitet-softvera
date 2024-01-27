namespace Models;
public class UmetnickoDelo
{
    [Key]
    public int ID{get; set;}
    public string Slika { get; set; } = null!;
    [MaxLength(255)]
    public string Naziv { get; set; }=null!;
    public string Tehnika { get; set; }=null!;
    public string Dimenzije { get; set; }=null!;
    
    [Required]
    public int Cena { get; set; }
    public string Ram { get; set; }=null!;
    public Kolekcija? Kolekcija { get; set; }
    [JsonIgnore]
    public Narudzbina? Narudzbina { get; set; }
}