using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;
using WlConsultings.BankChallenge.Application.Dtos.Response;

namespace WlConsultings.BankChallenge.WebApi.Filters
{
    /// <summary>
    /// Filtro para padronizar a resposta da API.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiResponseFilter : IActionFilter
    {
        /// <summary>
        /// Método executado antes da ação ser executada.
        /// </summary>
        /// <param name="context">Contexto da execução da ação.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Este método é intencionalmente deixado vazio porque nenhuma ação é necessária antes da execução da ação.
            // Se algum pré-processamento for necessário no futuro, ele pode ser implementado aqui.
        }

        /// <summary>
        /// Método executado após a ação ser executada.
        /// </summary>
        /// <param name="context">Contexto da execução da ação.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null && context.Result is ObjectResult objectResult && objectResult.Value != null)
            {
                var statusCode = objectResult.StatusCode ?? 200;
                var success = statusCode >= 200 && statusCode < 300;
                var message = success ? "Operação realizada com sucesso." : "Operação não realizada.";

                var apiResponse = new ApiResponse<object>
                {
                    Data = objectResult.Value,
                    Message = message,
                    StatusCode = statusCode
                };

                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = statusCode
                };
            }
        }
    }
}
