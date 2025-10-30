using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Middle0.Domain.Common.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Options;

namespace Middle0.Hangfire
{
	public class HangfireEventTasks
	{
		private readonly EmailSettings _emailSettings;

		public HangfireEventTasks(IOptions<EmailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value;
		}
		public async Task EventEmail(EventEmailDTO entity)
		{
			/*BackgroundJob.Enqueue(
			() => SendEventEmail(entity));*/
			BackgroundJob.Enqueue<HangfireEventTasks>(x => x.SendEventEmail(entity));
		}
		public async Task SendEventEmail(EventEmailDTO entity)
		{
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(_emailSettings.From));
			email.To.Add(MailboxAddress.Parse(entity.UserEmail)); 
			email.Subject = $"Уведомление о событии: {entity.Name}";
			email.Body = new TextPart(TextFormat.Html)
			{
				Text = $"<h3>Hi !</h3><p> You create  <b>{entity.Name}</b>, Place {entity.Place}.</p>"
			};

			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
			await smtp.AuthenticateAsync(_emailSettings.From, _emailSettings.Password);
			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}
	}
}
