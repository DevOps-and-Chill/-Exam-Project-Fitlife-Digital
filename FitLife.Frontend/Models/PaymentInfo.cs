namespace FitLife.Frontend.Models;

public class PaymentInfo
{
    public string CardNumber { get; set; } = "";
    public string Month { get; set; } = "";
    public string Year { get; set; } = "";
    public string Cvv { get; set; } = "";

    public string PaymentMethod { get; set; } = "";
}