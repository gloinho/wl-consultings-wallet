namespace WlConsultings.BankChallenge.Application.Interfaces
{
    public interface IValidatorService
    {
        void ValidateAndThrow<T>(T request);
    }
}
