using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace StockholderManager.Services
{
  // This class is used by the application to send email for account confirmation and password reset.
  // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
  public class EmailSender : IEmailSender
  {
    private readonly IOptions<EmailOptions> emailOptions;

    public EmailSender(IOptions<EmailOptions> emailOptions)
    {
      this.emailOptions = emailOptions;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
      MailMessage mail = new MailMessage();
      mail.From = new MailAddress(emailOptions.Value.From);

      // Set recipient email address
      mail.To.Add(email);
      mail.Subject = subject;
      mail.Body = message;
      mail.IsBodyHtml = true;

      SmtpClient client = new SmtpClient(emailOptions.Value.Host);
      client.Port = emailOptions.Value.Port;
      client.EnableSsl = true;
      client.UseDefaultCredentials = false;
      NetworkCredential cred = new System.Net.NetworkCredential(emailOptions.Value.UserName, emailOptions.Value.Password);
      client.Credentials = cred;
      await client.SendMailAsync(mail);
    }
  }
}
