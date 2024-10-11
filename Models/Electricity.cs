using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication9Municipal_Billing_System.Models
{
    public enum EStatus{
        paid,
        unpaid,
        overdue
    }
    public class Electricity
    {

    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ElectricityId{get;set;}
    public decimal Usage {get;set;}
    public decimal Rate {get;set;} //Cost Per kWh
    public decimal Cost {get;set;} 
    public DateTime DueDate{get;set;} = DateTime.Now;
    public EStatus status {get;set;}

    // Foreign key for User
     [ForeignKey("RegUserId")]
    public int RegUserId { get; set; }
    public virtual Reg Reg { get; set; }
  }

}