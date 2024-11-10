namespace ResoniteLogCleaner
{
    internal class PseudonymGenerator
    {
        private readonly string Prefix;
        private readonly Dictionary<string, string> pseudonyms = [];

        private readonly Random random = new();

        public PseudonymGenerator(string prefix)
        {
            Prefix = prefix;
        }

        public string Get(string name)
        {
            if (pseudonyms.TryGetValue(name, out var pseudonym))
            {
                return pseudonym;
            }
            pseudonym = $"{Prefix}_{random.Next():x}";
            pseudonyms[name] = pseudonym;
            return pseudonym;
        }
    }
}
