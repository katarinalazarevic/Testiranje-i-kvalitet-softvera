namespace Models;

public class ProjekatContext : DbContext
{
    public DbSet<Korisnik>? Korisnici { get; set; }
    public DbSet<Narudzbina>? Narudzbine {get;set;}
    public DbSet<Kolekcija>? Kolekcije {get;set;}
    public DbSet<UmetnickoDelo>? UmetnickaDela {get;set;}
    
    public ProjekatContext(DbContextOptions options) : base(options)
    {
        
    }
}
