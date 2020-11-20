using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Results;
using static System.String;

namespace ScoreTracker.Server.Services.Results
{
    public class MeetResultService : IResultService
    {
        private readonly ICosmosRepository<Result> _resultRepository;

        public MeetResultService(ICosmosRepository<Result> resultRepository)
        {
            _resultRepository = resultRepository;
        }

        public async IAsyncEnumerable<Result> GetResultsAsync(ResultsQuery request)
        {
            // TODO: Is there any way to query with linq?
            var query = $"c.meetId = {request.MeetId}";
            if (request.Divisions != null)
            {
                query += " AND (" + Join(
                    " OR ",
                    from levelDivision in request.Divisions select $"c.meetIdLevelDivision = \"{request.MeetId}{levelDivision}\"")
                    + ")";
            }

            foreach (var result in await _resultRepository.GetItemsAsync(query, request.Limit))
            {
                yield return result;
            }
        }

        public async Task AddResultAsync(Result result)
        {
            await _resultRepository.AddItemAsync(result);
        }
    }
}