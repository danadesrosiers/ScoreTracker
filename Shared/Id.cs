using System.Runtime.Serialization;

namespace ScoreTracker.Shared
{
    [DataContract]
    public record Id
    {
        public Id(string value)
        {
            Value = value;
        }

        public Id() {}

        [DataMember(Order = 1)]
        public string Value { get; init; } = null!;
    }
}