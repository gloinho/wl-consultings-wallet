using FluentValidation;
using WlConsultings.BankChallenge.Application.Dtos.Request;

namespace WlConsultings.BankChallenge.Application.Validators
{
    public class TransferRequestValidator : AbstractValidator<TransferRequest>
    {
        public TransferRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.ReceiverUserEmail)
                .NotEmpty().WithMessage("Receiver user email is required.")
                .EmailAddress().WithMessage("Receiver user email must be a valid email address.");
        }
    }
}
