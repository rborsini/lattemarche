using LatteMarche.Application.Assam.Interfaces;
using LatteMarche.Application.Assam.Models;
using RB.Ftp;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Assam.Services
{
    public class AssamService : IAssamService
    {

        #region Methods

        public List<Report> CheckMailBox(MailOptions mailOptions, MailFilters mailFilters, FtpOptions ftpOptions = null)
        {
            var reports = new List<Report>();

            // download messaggi dalla casella di posta
            var messages = DownloadMessages(mailOptions.HostName, mailOptions.Port, mailOptions.Username, mailOptions.Password, mailFilters.From, mailFilters.Since, mailFilters.Before).ToList();

            foreach(var message in messages)
            {
                var excelAttachments = message.Attachments.Where(a => a.ContentType.MediaType == "application/vnd.ms-excel").ToList();
                foreach (var attachment in excelAttachments)
                {
                    // conversione file in report
                    var attechmentContent = ConvertToBytes(attachment.ContentStream);
                    var attachmentReports = ExcelParser.Parse(attachment.ContentStream);
                    
                    reports.AddRange(attachmentReports);

                    // salvataggio copia di backup su cartella FTP
                    if(ftpOptions != null)
                        BackupFile(ftpOptions, attachment.Name, attechmentContent);
                }
            }

            return reports;
        }


        private IEnumerable<MailMessage> DownloadMessages(string hostname, int port, string username, string password, string from, DateTime start, DateTime end)
        {
            using (ImapClient client = new ImapClient(hostname, port, username, password, AuthMethod.Login, true))
            {
                var searchCondition = SearchCondition.SentSince(start);
                searchCondition = searchCondition.And(SearchCondition.SentBefore(end));

                var fromAddresses = from.Split(';').ToList();

                if(fromAddresses.Count > 0)
                {
                    var fromCondition = SearchCondition.From(fromAddresses[0]);

                    for(int i = 1; i < fromAddresses.Count; i++)
                        fromCondition = fromCondition.Or(SearchCondition.From(fromAddresses[i]));

                    searchCondition = searchCondition.And(fromCondition);
                }

                IEnumerable<uint> uids = client.Search(searchCondition);
                
                return client.GetMessages(uids);
            }
        }

        private byte[] ConvertToBytes(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
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
