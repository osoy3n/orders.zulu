using System.Net;

namespace Orders.Frontend.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponceMessage)
        {
            Response = response;
            Error = error;
            HttpResponceMessage = httpResponceMessage;
        }

        public T? Response { get; }
        public bool Error { get; }
        public HttpResponseMessage HttpResponceMessage { get; }

        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            var statusCode = HttpResponceMessage.StatusCode;
            if (statusCode == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado.";
            }
            if (statusCode == HttpStatusCode.BadRequest)
            {
                return await HttpResponceMessage.Content.ReadAsStringAsync();
            }
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                return "Debes estar logueado para ejecutar esta operación.";
            }
            if (statusCode == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para ejecutar esta operación.";
            }

            return "Ha ocurrido un error inesperado";
        }
    }
}
