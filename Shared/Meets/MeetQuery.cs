namespace ScoreTracker.Shared.Meets;

[DataContract]
public record MeetQuery : IQuery<Meet, Meet>
{
    [DataMember(Order = 1)]
    public string? Name { get; init; }
    [DataMember(Order = 2)]
    public DateTime? StartDate { get; init; }
    [DataMember(Order = 3)]
    public DateTime? EndDate { get; init; }
    [DataMember(Order = 4)]
    public int? Year { get; init; }
    [DataMember(Order = 5)]
    public StateCode? StateCode { get; init; }
    [DataMember(Order = 6)]
    public string? City { get; init; }
    [DataMember(Order = 7)]
    public Discipline? Discipline { get; init; }

    public IQueryable<Meet> ConfigureQuery(IQueryable<Meet> queryable)
    {
        if (!string.IsNullOrEmpty(Name))
        {
            queryable = queryable.Where(m => m.Name.ToLower().Contains(Name.ToLower()));
        }

        if (Year != null)
        {
            queryable = queryable.Where(m => m.Season == Year);
        }

        if (StateCode != null)
        {
            queryable = queryable.Where(m => m.State == StateCode);
        }

        if (Discipline != null)
        {
            queryable = queryable.Where(m => m.Discipline == Discipline);
        }

        if (StartDate != null)
        {
            queryable = queryable.Where(m => m.StartDate >= StartDate);
        }

        if (EndDate != null)
        {
            queryable = queryable.Where(m => m.EndDate <= EndDate);
        }

        return queryable;
    }
}