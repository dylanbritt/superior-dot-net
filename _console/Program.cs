using Superior.BusinessLogic;
using Superior.BusinessLogic.Interfaces;
using Superior.DataAccess.Repositories;
using Superior.Domain.Models;
using System;
using System.Net.Mail;

namespace _console
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        public static void CreateUser()
        {
            IUserService userLogic = new UserService(new UserRepository());

            var user = new User()
            {
                UserName = "test-1",
                UserCredential = new UserCredential
                {
                    Password = "password",
                }
            };
            user.UserCredential.EncryptPassword();

            userLogic.CreateUser(user);
        }

        public static void CreateTestMessage(string server)
        {
            string to = "jane@contoso.com";
            string from = "ben@contoso.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Using the new SMTP client.";
            message.Body = @"Using this new feature, you can send an e-mail message from an application very easily.";
            SmtpClient client = new SmtpClient(server);
            // Credentials are necessary if the server requires the client 
            // to authenticate before it will send e-mail on the client's behalf.
            client.UseDefaultCredentials = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                            ex.ToString());
            }
        }
    }
}
