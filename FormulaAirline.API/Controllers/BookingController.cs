using FormulaAirline.API.Models;
using FormulaAirline.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormulaAirline.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IMessageProducer _messageProducer;
    
    // in memory db
    public static readonly List<Booking> _booking = new();

    public BookingController(ILogger<BookingController> logger, IMessageProducer messageProducer)
    {
        _logger = logger;
        _messageProducer = messageProducer;
    }

    [HttpPost]
    public IActionResult CreateBooking([FromBody] Booking booking)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        
        _booking.Add(booking);
        
        _messageProducer.SendingMessage<Booking>(booking);

        return Ok();
    }
}