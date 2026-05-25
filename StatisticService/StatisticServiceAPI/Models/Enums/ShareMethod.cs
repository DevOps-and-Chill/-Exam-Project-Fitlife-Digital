namespace StatisticServiceAPI.Models.Enums
{
    //JBS: Bruges til at definere hvilken metode der bruges til at dele en statistik
    public enum ShareMethod
    {
        InternalUser, //Deles med en intern bruger i systemet
        ExternalLink, //Deles gennem et eksternt link med udløbsdato
        PublicReport //Gøres offentligt tilgængeligt for alle
    }
}