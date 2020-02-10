namespace RealState.Models
{
    public class Project
    {
        public object AantalKamersTotEnMet { get; set; }
        public object AantalKamersVan { get; set; }
        public object AantalKavels { get; set; }
        public object Adres { get; set; }
        public object FriendlyUrl { get; set; }
        public object GewijzigdDatum { get; set; }
        public object GlobalId { get; set; }
        public string HoofdFoto { get; set; }
        public bool IndIpix { get; set; }
        public bool IndPDF { get; set; }
        public bool IndPlattegrond { get; set; }
        public bool IndTop { get; set; }
        public bool IndVideo { get; set; }
        public string InternalId { get; set; }
        public object MaxWoonoppervlakte { get; set; }
        public object MinWoonoppervlakte { get; set; }
        public object Naam { get; set; }
        public object Omschrijving { get; set; }
        public object[] OpenHuizen { get; set; }
        public object Plaats { get; set; }
        public object Prijs { get; set; }
        public object PrijsGeformatteerd { get; set; }
        public object PublicatieDatum { get; set; }
        public int Type { get; set; }
        public object Woningtypen { get; set; }
    }
}