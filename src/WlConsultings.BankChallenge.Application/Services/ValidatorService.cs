using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WlConsultings.BankChallenge.Application.Interfaces;

namespace WlConsultings.BankChallenge.Application.Services
{
    /// <summary>
    /// Service for validating requests using FluentValidation.
    /// </summary>
    public class ValidatorService : IValidatorService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve validators.</param>
        public ValidatorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Validates the specified request and throws an exception if validation fails.
        /// </summary>
        /// <typeparam name="T">The type of the request to validate.</typeparam>
        /// <param name="request">The request to validate.</param>
        /// <exception cref="NotSupportedException">Thrown when no validator is found for the specified type.</exception>
        public void ValidateAndThrow<T>(T request)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();
            if (validator == null)
                throw new NotSupportedException($"Validator for {typeof(T)} not found.");

            validator.ValidateAndThrow(request);
        }
    }
}
