using System;
using BDD_Bank_Accounts;
using Machine.Specifications;
using MOQ = Moq;

namespace BankAccounts.Specs
{
    [Subject("Transferring Money")]
    public class When_transferring_between_two_accounts : TransferSpecsBase
    {
        Because of = () =>
            Transfer(.5m);

        It Should_debit_the_from_account_by_the_amount_transferred = () =>
            FromAccount.Balance.ShouldEqual(.5m);

        It Should_credit_the_to_account_by_the_amount_transferred = () =>
            ToAccount.Balance.ShouldEqual(1.5m);

        It Should_send_an_email_with_the_subject_funds_transferred_to_the_account_holder = () =>
            EmailGateway.Verify(g => g.SendEmail("Funds transferred", "bdd@bank.com", FromAccount.EmailAddress, MOQ.It.IsAny<string>()), MOQ.Times.Once());
    }

    [Subject("Transferring Money")]
    public class When_transferring_an_amount_greater_than_the_balance_of_the_from_account : TransferSpecsBase
    {
        static Exception TransferException;

        Because of = () =>
            TransferException = Catch.Exception(() => Transfer(1.1m));

        It Should_not_allow_the_transfer = () =>
            TransferException.ShouldNotBeNull();

        It Should_throw_an_invalid_operation_exception = () =>
            TransferException.ShouldBeOfExactType<InvalidOperationException>();

        It Should_include_the_requested_amount_in_the_exception_message = () =>
            TransferException.Message.ShouldContain("$1.10");

        It Should_include_the_current_balance_in_the_exception_message = () =>
            TransferException.Message.ShouldContain("$1.00");
    }

    public abstract class TransferSpecsBase
    {
        protected static Account FromAccount;
        protected static Account ToAccount;
        protected static Transfer TransferManager;
        protected static MOQ.Mock<IEmailGateway> EmailGateway;

        Establish context = () =>
        {
            FromAccount = new Account(1m);
            ToAccount = new Account(1m);
            EmailGateway = new MOQ.Mock<IEmailGateway>();
            TransferManager = new Transfer(EmailGateway.Object);
        };

        protected static void Transfer(decimal amountToTransfer)
        {
            TransferManager.TransferFunds(FromAccount, ToAccount, amountToTransfer);
        }
    }
}