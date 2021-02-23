using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Mail;
using MailKit.Net.Imap;
using MailKit;

namespace LatteMarche.Tests.Mail
{
    [TestFixture]
    public class DownloadMailTest
    {

        [Test]
        public void DownloadMail_Mailkit_Test()
        {
            var hostname = "we-code.it";
            var username = "lattemarche@we-code.it";
            var password = "Lattemarche123";
            var port = 993;

            using (var client = new ImapClient())
            {
                client.Connect(hostname, port, true);
                client.Authenticate(username, password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                Console.WriteLine("Total messages: {0}", inbox.Count);
                Console.WriteLine("Recent messages: {0}", inbox.Recent);

                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = inbox.GetMessage(i);
                    Console.WriteLine("Subject: {0}", message.Subject);
                }

                client.Disconnect(true);
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
