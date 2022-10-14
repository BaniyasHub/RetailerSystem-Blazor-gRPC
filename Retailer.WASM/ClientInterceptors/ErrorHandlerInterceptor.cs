using Grpc.Core.Interceptors;
using Grpc.Core;

namespace Retailer.WASM.ClientInterceptors
{
    //Can't find a use case for this interceptor
    public class ErrorHandlerInterceptor : Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var call = continuation(request, context);

            return new AsyncUnaryCall<TResponse>(
                HandleResponse(call.ResponseAsync),
                call.ResponseHeadersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose);
        }

        private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> inner)
        {
            try
            {
                return await inner;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Custom error", ex);
            }
        }
    }
}
