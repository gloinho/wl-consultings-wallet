using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WlConsultings.BankChallenge.Application.Dtos.Response;

namespace WlConsultings.BankChallenge.WebApi.Filters
{
    /// <summary>
    /// Filtro para lidar com exceções globalmente e retornar uma resposta de erro padronizada.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Chamado de forma assíncrona após uma ação lançar uma <see cref="Exception"/>.
        /// </summary>
        /// <param name="context">O <see cref="ExceptionContext"/>.</param>
        /// <returns>Um <see cref="Task"/> representando a operação assíncrona.</returns>
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                var validationResponse = new ApiResponse<object>
                {
                    Data = validationException.Errors.Select(error => new { error.ErrorMessage, error.PropertyName }),
                    Message = "Validation errors. Check request.",
                    StatusCode = 400
                };
                context.Result = new ObjectResult(validationResponse)
                {
                    StatusCode = validationResponse.StatusCode
                };
                OnException(context);
                return Task.CompletedTask;
            }

            var statusCode = context.Exception is HttpRequestException httpRequestException && httpRequestException.StatusCode.HasValue
                ? (int)httpRequestException.StatusCode
                : 500;

            var errorResponse = new ApiResponse<object>
            {
                Data = null!,
                Message = context.Exception?.Message ?? "Operação não realizada.",
                StatusCode = statusCode
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = errorResponse.StatusCode
            };

            OnException(context);
            return Task.CompletedTask;
        }
    }
}

