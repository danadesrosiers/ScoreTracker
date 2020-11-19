namespace ScoreTracker.Shared
{
    public partial class Decimal
    {
        private const decimal NanoFactor = 1_000_000_000;
        public Decimal(long units, int nanos)
        {
            Units = units;
            Nanos = nanos;
        }

        public static implicit operator decimal(Decimal grpcDecimal)
        {
            return (grpcDecimal?.Units ?? 0) + (grpcDecimal?.Nanos ?? 0) / NanoFactor;
        }

        public static implicit operator Decimal(decimal value)
        {
            var units = decimal.ToInt64(value);
            var nanos = decimal.ToInt32((value - units) * NanoFactor);
            return new Decimal(units, nanos);
        }
    }
}