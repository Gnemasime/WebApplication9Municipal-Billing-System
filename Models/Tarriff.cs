using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication9Municipal_Billing_System.Models
{
    public class Tarriff
    {

    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TarriffId{get;set;}
    public string Name {get;set;}
    public decimal DiscRate {get;set;}

 
    
    
    }
}