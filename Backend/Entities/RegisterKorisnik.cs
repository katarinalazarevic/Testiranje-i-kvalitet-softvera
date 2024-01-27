namespace Entities;
public class RegisterKorisnik
{
    private const int MaxStringLength = 255;
    [Required]
    private const string AllowedSpecialCharacters = "*-+_@&%$";
    [MaxLength(MaxStringLength, ErrorMessage = "Ime ne sme biti duže od 255 karaktera.")]
    public string Ime { get; set; } = null!;
    [Required]
    [MaxLength(MaxStringLength, ErrorMessage = "Prezime ne sme biti duže od 255 karaktera.")]
    public string Prezime { get; set; } = null!;
    [Required]
    [MaxLength(MaxStringLength, ErrorMessage = "Email adresa ne sme biti duža od 255 karaktera.")]
    public string Email { get; set; } = null!;
    [Required]

    [StringLength(30, MinimumLength = 6, ErrorMessage = "Lozinka mora biti između 6 i 30 karaktera.")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[" + AllowedSpecialCharacters + "])[A-Za-z\\d" + AllowedSpecialCharacters + "]+$", 
        ErrorMessage = "Lozinka mora sadržati barem jedno malo slovo, jedno veliko slovo, jedan broj i jedan od specijalnih karaktera: *-+_@&%$")]
    public string Password { get; set; } = null!;
    [Required]
    public int KontaktTelefon { get; set; } 
    [Required]

    [StringLength(MaxStringLength, ErrorMessage = "Ulica ne sme biti duža od 255 karaktera.")]
    public string Ulica { get; set; } = null!;
    [Required]

    [StringLength(MaxStringLength, ErrorMessage = "Grad ne sme biti duži od 255 karaktera.")]
    public string Grad { get; set; } = null!;
    [Required]

    [Range(1, int.MaxValue, ErrorMessage = "Broj mora biti pozitivan.")]
    public int Broj { get; set; }
}
