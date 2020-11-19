using System;
using ScoreTracker.Client.IndexedDb;
using TG.Blazor.IndexedDB;

namespace ScoreTracker.IndexedDb
{
    public class IndexAttribute : Attribute
    {
        public bool? Unique { get; set; }

        public IndexSpec ToIndexSpec(string name)
        {
            return new IndexSpec
            {
                Name = name,
                KeyPath = name.ToLowerFirstChar(),
                Auto = false,
                Unique = Unique
            };
        }
    }
}