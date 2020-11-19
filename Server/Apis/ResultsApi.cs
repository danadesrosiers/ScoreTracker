using System.Threading.Tasks;
using Grpc.Core;
using ScoreTracker.Server.Services.Results;
using ScoreTracker.Shared;
using Result = ScoreTracker.Shared.Result;

namespace ScoreTracker.Server.Apis
{
    public class ResultsApi : Results.ResultsBase
    {
        private readonly MeetResultService _resultService;

        public ResultsApi(
            MeetResultService resultService)
        {
            _resultService = resultService;
        }

        public override async Task GetResults(ResultsRequest request, IServerStreamWriter<Result> responseStream, ServerCallContext context)
        {
            foreach (var result in await _resultService.GetResultsAsync(request.ToResultsQuery()))
            {
                await responseStream.WriteAsync(result.ToResultApi());
            }
        }
    }
}