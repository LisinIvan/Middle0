using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Middle0.Domain.Common.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Middle0.Hangfire
{
	public class HangfireEventTasks
	{
		public async Task EventEmail(EventEmailDTO entity)
		{
			BackgroundJob.Enqueue(
			() => SendEventEmail(entity));
		}
		public async Task SendEventEmail(EventEmailDTO entity)
		{
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse("1985interdom@gmail.com"));
			email.To.Add(MailboxAddress.Parse(entity.UserEmail)); // например, Email получателя
			email.Subject = $"Уведомление о событии: {entity.Name}";
			email.Body = new TextPart(TextFormat.Html)
			{
				Text = $"<h3>Hi !</h3><p> You create  <b>{entity.Name}</b>, Place {entity.Date}.</p>"
			};

			using var smtp = new SmtpClient();
			await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync("1985interdom@gmail.com", "password"); // пароль приложения Gmail
			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}
	}
}
