using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication9Municipal_Billing_System.Models
{
    public class Electricity
    {

    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ElectricityId{get;set;}
    public decimal Usage {get;set;}
    public decimal Rate {get;set;} //Cost Per kWh
    public decimal Cost {get;set;} 

    // Foreign key for User
    public int UserId { get; set; }
    // Navigation property to associate this tariff with a User
    public Reg Reg  { get; set; }

    public decimal ElectricCost()
    {
        return Usage * Rate;
    }
    }
}