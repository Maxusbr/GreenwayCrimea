using System;
using System.Linq;
using AdvantShop.Localization;
using MailKit;
using MimeKit;


namespace AdvantShop.Core.Services.Loging.Emails
{
    public class Email
    {
        public DateTime CreateOn { get; set; }
        public Guid CustomerId { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailStatus Status { get; set; }
        public string ShopId { get; set; }
    }

    public class EmailImap
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string DateStr { get { return Culture.ConvertDateWithoutSeconds(Date); } }
        public string From { get; set; }
        public string FromEmail { get; set; }
        public string To { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }

        public string Folder { get; set; }

        public EmailImap()
        {
            
        }

        public EmailImap(IMessageSummary summary, string folder)
        {
            var envelope = summary.Envelope;

            Id = summary.UniqueId.ToString();
            Folder = folder;

            if (envelope != null)
            {
                Subject = envelope.Subject;

                Date = summary.InternalDate.HasValue
                    ? summary.InternalDate.Value.LocalDateTime
                    : envelope.Date.HasValue ? envelope.Date.Value.LocalDateTime : DateTime.Now;

                From = envelope.From != null ? envelope.From.ToString() : "";

                if (envelope.From != null)
                {
                    var mailBox = envelope.From.Mailboxes.FirstOrDefault();
                    if (mailBox != null)
                        FromEmail = mailBox.Address;
                }

                To = envelope.To != null ? envelope.To.ToString() : "";
            }
        }

        public EmailImap(UniqueId uid, MimeMessage msg, string folder)
        {
            Id = uid.ToString();
            Folder = folder;

            Subject = msg.Subject;
            Date = msg.Date.LocalDateTime;
            From = msg.From.ToString();

            if (msg.From != null)
            {
                var mailBox = msg.From.Mailboxes.FirstOrDefault();
                if (mailBox != null)
                    FromEmail = mailBox.Address;
            }

            To = msg.To.ToString();
            TextBody = msg.TextBody;
            HtmlBody = msg.HtmlBody;
        }
    }
}
