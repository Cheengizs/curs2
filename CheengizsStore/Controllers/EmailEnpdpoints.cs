using Microsoft.AspNetCore.Mvc;
using CheengizsStore.Services;

namespace CheengizsStore.Controllers;

public static class EmailEnpdpoints
{
    public class Contact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
    
    public static RouteGroupBuilder MapEmailEnpdpoints(this RouteGroupBuilder group)
    {
        group.MapPost("", async ([FromBody] Contact form, IEmailService emailSender) =>
        {
            await emailSender.SendEmailAsync(form.Email, "Автоматическое письмо ответ", $"{form.Name}: {form.Message }");
            return Results.Ok();
        });
        
        return group;
    }
}