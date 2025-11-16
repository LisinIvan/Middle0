using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Middle0.Domain.Common.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Options;

namespace Middle0.Application.Hangfire
{
	public class HangfireEventTasks
	{
		private readonly EmailSettings _emailSettings;
		public HangfireEventTasks(IOptions<EmailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value;
		}
		public async Task<string?> EventEmail(EventDTO entityDTO)
		{
			if (entityDTO.SendEmail!=null)
			{
				string idHangFire = BackgroundJob.Schedule<HangfireEventTasks>(
					x => x.SendEventEmail(entityDTO),
					entityDTO.SendEmail.Value.Date - DateTime.Now.Date);
				return idHangFire;
			}
			return "0";
		}
		public async Task SendEventEmail(EventDTO entityDTO)
		{
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(_emailSettings.From));
			email.To.Add(MailboxAddress.Parse(entityDTO.UserEmail));
			email.Subject = $"Уведомление о событии: {entityDTO.Name}";
			email.Body = new TextPart(TextFormat.Html)
			{
				Text = $"<h3>Hi !</h3><p> You create  <b>{entityDTO.Name}</b>, Place {entityDTO.Place}.</p>"
			};
			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
			await smtp.AuthenticateAsync(_emailSettings.From, _emailSettings.Password);
			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}

		public async Task<string> UpdateDateSendEmail(EventDTO entityDTO)
		{
			DeleteJobById(entityDTO.jobId);
			return await EventEmail(entityDTO);
		}
		public void DeleteJobById(string jobId)
		{
			 BackgroundJob.Delete(jobId);
		}
		public async Task<DateTime?> GetDateJob(string jobId)
		{
			var monitor = JobStorage.Current.GetMonitoringApi();
			var scheduledJobs = monitor.ScheduledJobs(0, int.MaxValue);

			var job = scheduledJobs.FirstOrDefault(x => x.Key == jobId);
			if (job.Value != null)
			{
				return job.Value.EnqueueAt;
			}
			return null;
		}
	}
}
