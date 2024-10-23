using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication9Municipal_Billing_System.Models
{
    public enum WStatus{
        paid,
        unpaid,
        overdue
    }
    public class Water 
    {

    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WaterId{get;set;}
    public decimal Usage {get;set;}
    public decimal Rate {get;set;} //Cost Per litre
    public DateTime DueDate{get;set;} 
    public WStatus status {get;set;}
    public decimal Cost {get;set;} 
 // Foreign key for User
    [ForeignKey("RegUserId")]
    public int RegUserId { get; set; }
    public virtual Reg Reg { get; set; }

    public decimal CalcRate()
    {
        return 0.50m; // 50c per litre
    }

    public decimal WaterCost()
    {
        return Usage * 0.50m ;
    }
   

    
    }
}