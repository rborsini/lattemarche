using LatteMarche.Application.Assam.Interfaces;
using LatteMarche.Application.Assam.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using RB.Ftp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Assam.Services
{
    public class AssamService : IAssamService
    {

        #region Constant

        private const string EXCEL_MIME_TYPE = "application/vnd.ms-excel";

        #endregion


        #region Methods

        public List<Report> CheckMailBox(MailOptions mailOptions, MailFilters mailFilters, FtpOptions ftpOptions = null)
        {
            var reports = new List<Report>();

            // download allegati dalla casella di posta
            var attachments = DownloadUnseenAttachments(mailOptions.HostName, mailOptions.Port, mailOptions.Username, mailOptions.Password, mailFilters.From, EXCEL_MIME_TYPE).ToList();

            foreach(var attachment in attachments)
            {
                // conversione file in report
                var attachmentReports = ExcelParser.Parse(attachment);
                    
                reports.AddRange(attachmentReports);

                // salvataggio copia di backup su cartella FTP
                if(ftpOptions != null)
                    BackupFile(ftpOptions, attachment.Name, attachment.Content);

            }

            return reports;
        }

        /// <summary>
        /// Download degli allegati delle mail non ancora lette
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="from"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private IEnumerable<Attachment> DownloadUnseenAttachments(string hostname, int port, string username, string password, string from, string mimeType)
        {
            var attachments = new List<Attachment>();

            using (var client = new ImapClient())
            {
                client.Connect(hostname, port, true);
                client.Authenticate(username, password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);
                var senders = from.Split(';');                

                foreach (var uid in inbox.Search(SearchQuery.NotSeen))
                {
                    var message = inbox.GetMessage(uid);
                    if (!senders.Contains(message.From.OfType<MailboxAddress>().Single().Address))
                        continue;

                    var category = GetCategory(message.Subject);

                    inbox.SetFlags(uid, MessageFlags.Seen, true);

                    foreach(var mimeEntity in message.Attachments.Where(a => a.ContentType.MimeType == mimeType))
                    {
                        var attachment = ConverToAttachment(mimeEntity);
                        attachment.Category = category;

                        attachments.Add(attachment);
                    }
                }

                client.Disconnect(true);
            }

            return attachments;
        }

        /// <summary>
        /// Recupero categoria report in base all'oggetto della mail
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        private string GetCategory(string subject)
        {
            if (subject.ToLower().Contains("veterinari"))
                return "veterinari";

            if (subject.ToLower().Contains("autisti"))
                return "autisti";

            return "-";
        }

        private Attachment ConverToAttachment(MimeEntity mimeEntity)
        {
            using (var ms = new MemoryStream())
            {
                if (mimeEntity is MessagePart)
                {
                    var part = (MessagePart)mimeEntity;
                    part.Message.WriteTo(ms);
                }
                else
                {
                    var part = (MimePart)mimeEntity;
                    part.Content.DecodeTo(ms);
                }

                return new Models.Attachment()
                {
                    Name = ((MimePart)mimeEntity).FileName,
                    Content = ms.ToArray()
                };
            }
        }

        private void BackupFile(FtpOptions ftpOptions, string attachmentName, byte[] attachmentBytes)
        {
            var ftpHelper = new FtpHelper(ftpOptions.Url, ftpOptions.Username, ftpOptions.Password);

            var fileInfo = new FileInfo(attachmentName);
            var name = attachmentName.Replace(fileInfo.Extension, "");
            var fileName = $"{name}_{DateTime.Now.ToString("yyyy-MM-dd_HHmmss")}.{fileInfo.Extension}";
            
            ftpHelper.UploadFile(fileName, attachmentBytes);
        }

        #endregion

    }
}
