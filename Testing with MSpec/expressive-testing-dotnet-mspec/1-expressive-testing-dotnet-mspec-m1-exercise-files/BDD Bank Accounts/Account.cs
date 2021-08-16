namespace BDD_Bank_Accounts
{
    public class Account
    {
        public decimal Balance { get; private set; }
        public string EmailAddress { get; set; }

        public Account(decimal initialBalance)
        {
            Balance = initialBalance;
        }
        
        public void Debit(decimal amountToDebit)
        {
            Balance -= amountToDebit;
        }

        public void Credit(decimal amountToCredit)
        {
            Balance += amountToCredit;
        }
    }
}