using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using System.Diagnostics.Eventing.Reader;

namespace ClassLib13 { 
public class EmailSender
{
    public void SentMail(string Usermail)
    {
            string activationLink = $"https://localhost:7132/AddAdmin?email={Usermail}";
            string body = "";

            if (Usermail.EndsWith("@student.pxl.be"))
            {
                string url = "https://media.giphy.com/media/SbM8BK5zf3SQOSA5TJ/giphy.gif?cid=790b76118nqnyl5qrv8gbih12ibu1xduidsgihwmed3eootj&ep=v1_gifs_search&rid=giphy.gif&ct=g";
                body = $@"
                <html>
                    <body>
                        <p>Hello, {Usermail}</p>
                        <p>Your SAMapp admin account is about to be activated.</p>
                        <p>Click the link below to complete the activation process:</p>
                        <p><a href='{activationLink}'>Activate Account</a></p>
                        <p>And here’s a little celebration once your account is activated:</p>
                        <img src='{url}' alt='Success GIF' />
                    </body>
                </html>";
            }
            else
            {
                body = $@"
                <p>Hello, {Usermail}</p>
                <p>Your SAMapp account is activated.</p>";
            }

            try
            {
            
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, 
                Credentials = new NetworkCredential("sam.app.wpl.sup@gmail.com", "hmuu znkv dhss exvz "), 
                EnableSsl = true 
            };

           
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("sam.app.wpl.sup@gmail.com"),
                Subject = "Registration SAMapp",
                Body = body,
                
                IsBodyHtml = true
            };

            
            mail.To.Add(Usermail);

            
            smtpClient.Send(mail);
            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
    }
        public void SentMailPassword(string Usermail)
        {

            string ResetLink = $"https://localhost:7132/api/ChangePassEmail?email={Usermail}";

            string body = $@"
            <p>Hello, {Usermail}</p>
            <p>Change your password by clicking on the <a href={ResetLink}>Reset password Link</a>.</p>
            ";            
            try
            {
                // SMTP Configuration for Gmail
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, // Use 465 for SSL or 587 for TLS
                    Credentials = new NetworkCredential("sam.app.wpl.sup@gmail.com", "hmuu znkv dhss exvz "), // Use App Password here
                    EnableSsl = true 
                };

                // Create email message
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("sam.app.wpl.sup@gmail.com"),
                    Subject = "Change password",
                    Body = body,

                    IsBodyHtml = true
                };

                // Add recipient
                mail.To.Add(Usermail);

                // Send email
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
        public void SentMailOrder(string Usermail)
        {
            string body = $@"
            <p>Hello, {Usermail}</p>
            <p>Your Order has been placed</p>
            ";
            try
            {
               
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, 
                    Credentials = new NetworkCredential("sam.app.wpl.sup@gmail.com", "hmuu znkv dhss exvz "), 
                    EnableSsl = true 
                };

            
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("sam.app.wpl.sup@gmail.com"),
                    Subject = "Order SAMapp",
                    Body = body,

                    IsBodyHtml = true
                };

               
                mail.To.Add(Usermail);

              
                smtpClient.Send(mail);
                Debug.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
        public void SentMailOrderCanceled(string Usermail)
        {
            string body = $@"
            <p>Hello, {Usermail}</p>
            <p>Your Order has been Canceled</p>
            ";
            try
            {

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("sam.app.wpl.sup@gmail.com", "hmuu znkv dhss exvz "),
                    EnableSsl = true
                };


                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("sam.app.wpl.sup@gmail.com"),
                    Subject = "Order SAMapp",
                    Body = body,

                    IsBodyHtml = true
                };


                mail.To.Add(Usermail);


                smtpClient.Send(mail);
                Debug.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
        public void SentMailOrderStatusChanged(string Usermail,string status)
        {
            string body = $@"
            <p>Hello, {Usermail}</p>
            <p>Your Order status hes been changed to {status}</p>
            ";
            try
            {

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("sam.app.wpl.sup@gmail.com", "hmuu znkv dhss exvz "),
                    EnableSsl = true
                };


                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("sam.app.wpl.sup@gmail.com"),
                    Subject = "Order SAMapp",
                    Body = body,

                    IsBodyHtml = true
                };


                mail.To.Add(Usermail);


                smtpClient.Send(mail);
                Debug.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
