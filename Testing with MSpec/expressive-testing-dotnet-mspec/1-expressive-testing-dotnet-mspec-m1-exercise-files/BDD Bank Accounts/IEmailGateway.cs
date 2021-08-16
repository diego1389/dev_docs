using System;

namespace BDD_Bank_Accounts
{
    public interface IEmailGateway
    {
        void SendEmail(string subject, string from, string to, string message);
    }
}