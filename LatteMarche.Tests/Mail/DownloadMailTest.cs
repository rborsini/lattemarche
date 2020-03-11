using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using S22.Imap;
using System.Net.Mail;

namespace LatteMarche.Tests.Mail
{
    [TestFixture]
    public class DownloadMailTest
    {

        [Test]
        public void DownloadMail_Test()
        {
            //var hostname = "we-code.it";
            //var username = "lattemarche@we-code.it";
            //var password = "Lattemarche123";

            using (ImapClient client = new ImapClient(hostname, 993, username, password, AuthMethod.Login, true))
            {

                var searchCondition = SearchCondition.SentSince(new DateTime(2012, 8, 23));
                //var searchCondition = SearchCondition.From("roberto.borsini@gmail.com");

                IEnumerable<uint> uids = client.Search(searchCondition);                
                IEnumerable<MailMessage> messages = client.GetMessages(uids);

                foreach(var message in messages)
                {
                    foreach(var attachment in message.Attachments)
                    {
                        var fileStream = File.Create(@"C:\tmp\" + attachment.Name);
                        attachment.ContentStream.Seek(0, SeekOrigin.Begin);
                        attachment.ContentStream.CopyTo(fileStream);
                        fileStream.Close();
                    }
                }

                Console.WriteLine("We are connected!");
            }

            Assert.IsTrue(true);
        }

        public void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

    }



}
