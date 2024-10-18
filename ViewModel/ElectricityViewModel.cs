using WebApplication9Municipal_Billing_System.Models;

namespace WebApplication9Municipal_Billing_System.ViewModel
{
    public class ElectricityViewModel
{
    public IEnumerable<Electricity> Electricities { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

}