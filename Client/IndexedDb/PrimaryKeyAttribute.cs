using System;
using ScoreTracker.IndexedDb;
using TG.Blazor.IndexedDB;

namespace ScoreTracker.Client.IndexedDb
{
    public class PrimaryKeyAttribute : Attribute
    {
        public bool? Unique { get; set; }
        public bool Auto { get; set; } = true;

        public IndexSpec ToIndexSpec(string name)
        {
            return new IndexSpec
            {
                Name = name,
                KeyPath = Auto ? null : name.ToLowerFirstChar(),
                Auto = Auto,
                Unique = Unique
            };
        }
    }
}