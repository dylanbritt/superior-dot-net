using Superior.BusinessLogic;
using Superior.BusinessLogic.Interfaces;
using Superior.DataAccess.Repositories;
using Superior.Domain.Models;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace _console
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateTestMessage();
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

        public static void CreateTestMessage()
        {
            string to = "admin@superior.world";
            string from = ConfigurationManager.AppSettings["SmtpFromAddress"];
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Welcome to Superior.World - Verify Email Address";
            message.Body = @"Thank you for creating your account. Please verify your email address.";

            SmtpClient client = new SmtpClient();
            // Credentials are necessary if the server requires the client 
            // to authenticate before it will send e-mail on the client's behalf.

            // TODO: fix
            //ServicePointManager.ServerCertificateValidationCallback =
            //    delegate(object s, X509Certificate certificate,
            //    X509Chain chain, SslPolicyErrors sslPolicyErrors)
            //    { return true; };

            // TODO: fix
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return CertificateValidationCallBack(s, certificate, chain, sslPolicyErrors);
                };

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

        private static bool CertificateValidationCallBack(
            object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }
    }
}
