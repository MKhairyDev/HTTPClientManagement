namespace APIConsumer
{
    public class ApiConstants
    {
        public static string FundaUrlBase { get; } = "http://partnerapi.funda.nl";

        public static string FundaRequestUrl { get; } =
            "/feeds/Aanbod.svc/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&zo=/amsterdam";
    }
}