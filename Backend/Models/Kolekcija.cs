namespace Models;
public class Kolekcija{

    [Key]
    public int ID{get; set;}
    
    [MaxLength(255)]
    public string NazivKolekcije { get; set; }=null!;
    public string Umetnik { get; set; }=null!;
    public int Godina { get; set; }
    public string Stil { get; set; }=null!;
    [JsonIgnore]
    public List<UmetnickoDelo>? UmetnickaDela { get; set; }

}