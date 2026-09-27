using System.ComponentModel.DataAnnotations;

namespace WebApiTaskWithAdoNetAndThreads.Models;

public class CreateOrderDto
{
    [Required(ErrorMessage = "Customer name is required")]
    [StringLength(100)]
    public string CustomerName { get; set; }

    [Range(0.01, 1000000, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }
}