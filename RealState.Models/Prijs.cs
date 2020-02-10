namespace RealState.Models
{
    public class Prijs
    {
        public bool GeenExtraKosten { get; set; }
        public string HuurAbbreviation { get; set; }
        public object Huurprijs { get; set; }
        public string HuurprijsOpAanvraag { get; set; }
        public object HuurprijsTot { get; set; }
        public string KoopAbbreviation { get; set; }
        public int Koopprijs { get; set; }
        public string KoopprijsOpAanvraag { get; set; }
        public int KoopprijsTot { get; set; }
        public int? OriginelePrijs { get; set; }
        public string VeilingText { get; set; }
    }
}