using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Loging.Emails;
using AdvantShop.Diagnostics;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace AdvantShop.Core.Services.Mails
{
    public class ImapMailService
    {
        private readonly string _host;
        private readonly string _userName;
        private readonly string _password;

        public ImapMailService()
        {
            _host = SettingsMail.ImapHost;
            _userName = SettingsMail.Login;
            _password = SettingsMail.Password;
        }

        public bool IsValid()
        {
            return !(string.IsNullOrWhiteSpace(_host) || _host == "imap.mail.ru" ||                // mail.ru не поддерживает команды поиска
                     string.IsNullOrWhiteSpace(_userName) || string.IsNullOrWhiteSpace(_password));
        }


        public List<EmailImap> GetEmails(string email)
        {
            var emails = new List<EmailImap>();

            if (string.IsNullOrWhiteSpace(email) || !IsValid())
                return emails;

            try
            {
                using (var client = new ImapClient())
                {
                    // accept all SSL certificates
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(_host, SettingsMail.ImapPort, SettingsMail.SSL);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(_userName, _password);

                    var searchInSent = false;

                    // Пытаемся взять ВСЕ письма (чтобы не искать отдельно в "отправленных"), если не получилось, то Inbox. (Inbox существует всегда)
                    var inbox = TryGetFolder(client, SpecialFolder.All);
                    if (inbox == null)
                    {
                        inbox = client.Inbox;
                        searchInSent = true;
                    }

                    inbox.Open(FolderAccess.ReadOnly);

                    var uids =
                        inbox.Search(
                            SearchQuery.NotDeleted
                                .And(SearchQuery.FromContains(email).Or(SearchQuery.ToContains(email)))
                                .And(SearchQuery.DeliveredAfter(DateTime.Now.AddYears(-2))));

                    if (uids.Count > 30)
                        uids = uids.Skip(uids.Count - 30).ToList();
                    
                    if (uids.Count > 0)
                        foreach (var summary in inbox.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.InternalDate))
                        {
                            emails.Add(new EmailImap(summary, inbox.FullName));
                        }


                    if (searchInSent)
                    {
                        inbox = TryGetFolder(client, SpecialFolder.Sent);
                        if (inbox != null)
                        {
                            inbox.Open(FolderAccess.ReadOnly);

                            uids = inbox.Search(SearchQuery.ToContains(email));
                            if (uids.Count > 30)
                                uids = uids.Skip(uids.Count - 30).ToList();

                            if (uids.Count > 0)
                                foreach (var summary in inbox.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.InternalDate))
                                {
                                    emails.Add(new EmailImap(summary, inbox.FullName));
                                }
                        }
                    }

                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("GetEmails by " + email, ex);
            }

            return emails;
        }

        public EmailImap GetEmail(string uid, string folder)
        {
            EmailImap email = null;

            if (string.IsNullOrEmpty(uid) || !IsValid())
                return email;

            try
            {
                var id = new UniqueId(Convert.ToUInt32(uid));

                using (var client = new ImapClient())
                {
                    // accept all SSL certificates
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(_host, SettingsMail.ImapPort, SettingsMail.SSL);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(_userName, _password);

                    var searchInSent = false;


                    var inbox = !string.IsNullOrEmpty(folder) ? TryGetFolder(client, folder) : null;
                    if (inbox == null)
                    {
                        inbox = TryGetFolder(client, SpecialFolder.All);
                        if (inbox == null)
                        {
                            inbox = client.Inbox;
                            searchInSent = true;
                        }
                    }

                    inbox.Open(FolderAccess.ReadOnly);
                    
                    var msg = inbox.GetMessage(id);
                    if (msg != null)
                    {
                        email = new EmailImap(id, msg, inbox.FullName);
                    }
                    else if (searchInSent)
                    {
                        inbox = TryGetFolder(client, SpecialFolder.Sent);
                        if (inbox != null)
                        {
                            inbox.Open(FolderAccess.ReadOnly);

                            msg = inbox.GetMessage(id);
                            if (msg != null)
                                email = new EmailImap(id, msg, inbox.FullName);
                        }
                    }

                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("GetEmailImap", ex);
            }

            return email;
        }


        private static IMailFolder TryGetFolder(ImapClient client, SpecialFolder type)
        {
            try {
                return client.GetFolder(type);
            }
            catch {
            }
            return null;
        }

        private static IMailFolder TryGetFolder(ImapClient client, string folder)
        {
            try
            {
                return client.GetFolder(folder);
            }
            catch
            {
            }
            return null;
        }
    }
}
