using System;

namespace BDD_Bank_Accounts
{
    public class Transfer
    {
        private readonly IEmailGateway _emailGateway;

        public Transfer(IEmailGateway emailGateway)
        {
            _emailGateway = emailGateway;
        }

        public void TransferFunds(Account fromAccount, Account toAccount, decimal amountToTransfer)
        {
            if (amountToTransfer > fromAccount.Balance)
            {
                throw new InvalidOperationException(String.Format("Cannot transer {0:c} from this account because the current balance is only {1:c}", amountToTransfer, fromAccount.Balance));
            }

            fromAccount.Debit(amountToTransfer);
            toAccount.Credit(amountToTransfer);
            _emailGateway.SendEmail("Funds transferred", "bdd@bank.com", fromAccount.EmailAddress, String.Format("Successfully transfered funds in the amoumt of: {0:c}", amountToTransfer));
        }
    }
}