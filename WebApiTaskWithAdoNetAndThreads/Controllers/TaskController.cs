using Microsoft.AspNetCore.Mvc;
using WebApiTaskWithAdoNetAndThreads.DbConnections;
using WebApiTaskWithAdoNetAndThreads.Models;

namespace WebApiTaskWithAdoNetAndThreads.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly IServiceScopeFactory _scopeFactory;

    public TaskController(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    [HttpPost("run-parallel")]
    public IActionResult RunParallelTasks([FromBody] CreateOrderDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var task1 = Task.Run(() => ProcessOrdersTask(dto));
        var task2 = Task.Run(() => GenerateReportTask());
        var task3 = Task.Run(() => SendNotificationsTask());

        Task.WhenAll(task1, task2, task3).Wait();

        int totalOrdersCount = task2.Result;

        return Ok(new
        {
            Message = "სამუშაო წარმატებით დასრულდა",
            TotalOrdersInDb = totalOrdersCount
        });
    }
    
    private void ProcessOrdersTask(CreateOrderDto dto)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Thread.Sleep(1000); 

            context.Orders.Add(new Order
            {
                CustomerName = dto.CustomerName,
                Amount = dto.Amount,
                OrderDate = DateTime.Now
            });

            context.SaveChanges(); 
        }
    }

    private int GenerateReportTask()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Thread.Sleep(3000); 

            int count = context.Orders.Count();
            return count;
        }
    }

    private void SendNotificationsTask()
    {
        Thread.Sleep(2000); 
    }
}