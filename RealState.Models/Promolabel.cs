namespace RealState.Models
{
    public class Promolabel
    {
        public bool HasPromotionLabel { get; set; }
        public string[] PromotionPhotos { get; set; }
        public string[] PromotionPhotosSecure { get; set; }
        public int PromotionType { get; set; }
        public int RibbonColor { get; set; }
        public object RibbonText { get; set; }
        public string Tagline { get; set; }
    }
}