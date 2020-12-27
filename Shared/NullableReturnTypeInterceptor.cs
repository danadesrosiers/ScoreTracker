using System;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace ScoreTracker.Shared
{
    public class NullableReturnTypeInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var response = await continuation(request, context);
            if (response == null)
            {
                throw new RpcException(new Status(
                    StatusCode.NotFound,
                    $"{typeof(TResponse).Name} [{JsonSerializer.Serialize(request)}] not found."));
            }

            return response;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var call = continuation(request, context);

            return new AsyncUnaryCall<TResponse>(
                HandleResponse(call.ResponseAsync)!,
                call.ResponseHeadersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose);
        }

        private async Task<TResponse?> HandleResponse<TResponse>(Task<TResponse> t)
        {
            try
            {
                var response = await t;
                return response;
            }
            catch (RpcException ex) when (ex.Status.StatusCode == StatusCode.NotFound)
            {
                Console.WriteLine($"Not Found: {ex.Message}");
                return default;
            }
        }
    }
}