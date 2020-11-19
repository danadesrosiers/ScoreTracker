using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ScoreTracker.Server.Services.Meets;
using ScoreTracker.Shared;
using Division = ScoreTracker.Shared.Division;
using Enum = System.Enum;
using Level = ScoreTracker.Shared.Level;
using Meet = ScoreTracker.Shared.Meet;
using MeetQuery = ScoreTracker.Shared.MeetQuery;
using Session = ScoreTracker.Shared.Session;

namespace ScoreTracker.Server.Apis
{
    public class MeetApi : Meets.MeetsBase
    {
        private readonly MeetService _meetService;

        public MeetApi(MeetService meetService)
        {
            _meetService = meetService;
        }

        public override async Task<Meet> GetMeet(MeetRequest request, ServerCallContext context)
        {
            return (await _meetService.GetMeetAsync(request.MeetId)).ToMeetApi();

        }

        public override async Task GetMeets(MeetQuery query, IServerStreamWriter<Meet> responseStream, ServerCallContext context)
        {
            foreach (var meet in await _meetService.GetMeetsAsync(query.ToServiceModel()))
            {
                await responseStream.WriteAsync(meet.ToMeetApi());
            }
        }
    }
}