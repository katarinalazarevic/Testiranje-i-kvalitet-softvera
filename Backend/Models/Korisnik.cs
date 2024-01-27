namespace Models;

public class Korisnik{
    [Key]
    public int ID{get; set;}
    [Required(ErrorMessage ="Morate uneti Ime")]
    public string Ime { get; set; }=null!;

    [Required(ErrorMessage ="Morate uneti Prezime")]
    public string Prezime { get; set; }=null!;

    [Required(ErrorMessage ="Morate uneti email")]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    [MaxLength(320)]
    public string Email { get; set; }=null!;
    public string PasswordHash { get; set; }=null!;

    [Required]
    public int KontaktTelefon { get; set; }

    public string Ulica {get; set;}=null!;
    public string Grad {get; set;}=null!;
    public int Broj {get; set;} 

    [JsonIgnore]
    public List<Narudzbina>? Narudzbine { get; set; }
}