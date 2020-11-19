using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;

namespace ScoreTracker.Server.Services.Results
{
    public class MeetResultService
    {
        private readonly ICosmosRepository<Result> _resultRepository;

        public MeetResultService(ICosmosRepository<Result> resultRepository)
        {
            _resultRepository = resultRepository;
        }

        public async Task<IEnumerable<Result>> GetResultsAsync(ResultsQuery request, int? limit = null)
        {
            var (meetId, divisions) = request;
            if (divisions == null)
            {
                return await _resultRepository.GetItemsAsync($"c.meetId = {meetId}", limit);
            }

            var meetLevelDivisions = (from levelDivision in divisions
                select meetId + levelDivision).ToList();

            return await meetLevelDivisions.SelectManyAsync(queryValue =>
                _resultRepository.GetItemsAsync($"c.meetId = {meetId} AND c.meetIdLevelDivision = \"{queryValue}\"", limit));
        }

        public async Task AddResultAsync(Result result)
        {
            await _resultRepository.AddItemAsync(result);
        }
    }
}