using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication9Municipal_Billing_System.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; } // Unique identifier for the bill
        [ForeignKey("RegUserId")]

        public int RegUserId { get; set; } // Foreign key to the user (registered user)

        [Required]
        public decimal Amount { get; set; } // Amount due for the bill

        [Required]
        public DateTime DueDate { get; set; } // Due date for the bill

        public Status Status { get; set; } // Status of the bill (e.g., unpaid, paid, overdue)

        public DateTime DateIssued { get; set; } // Date when the bill was issued

        [Required]
        public string InvoiceId { get; set; } // Unique identifier for the invoice (for PayPal)

        // Navigation property to related user
        public virtual Reg Reg { get; set; } 
    }

    // Enum for bill status
    public enum Status
    {
        unpaid,
        paid,
        overdue
    }
}
