// See https://aka.ms/new-console-template for more information
using ResoniteLogCleaner;
using System.Text.RegularExpressions;

var regexOptions = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

var userDirPattern = new Regex(@"(:[/\\]Users[/\\])([^/\\\r\n]+)", regexOptions);
Regex[] machineIDPatterns = [new Regex(@"MachineID: ([0-9a-z]+)", regexOptions)];
Regex[] usernamePatterns = [
    new Regex(@"Username: ([^,\r\n]+), UserID:", regexOptions),
    new Regex(@"User ([^:,\r\n]+) Role:", regexOptions),
    new Regex(@"User ([^,\r\n(]+) \(ID", regexOptions),
];
Regex[] userIDPatterns = [
    new Regex(@"UserID: ([^,\r\n]+), [^\r\n]+ MachineID", regexOptions),
    new Regex(@"SendStatusToUser: ([^:\n\r]+)\. OnlineStatus", regexOptions),
    new Regex(@"[ /](U-[a-z0-9_\-\+]+)", regexOptions),
];
Regex[] worldNamePatterns = [
    new Regex(@"WorldName set to ([^,\r\n]+)\. LastModifyingUser", regexOptions)
];
Regex[] steamIDPatterns = [
    new Regex(@"Local User SteamID: ([0-9]+)", regexOptions)
];

HashSet<string> nonReplacedKeywords = ["Local", "Userspace", "Home"];

foreach (var path in args) {
    var content = File.ReadAllText(path);
    var pseudoUserdir = new PseudonymGenerator("USER_DIR");
    var pseudoUserID = new PseudonymGenerator("U-USER");
    var pseudoUser = new PseudonymGenerator("USER");
    var pseudoWorld = new PseudonymGenerator("WORLD");
    var pseudoMachine = new PseudonymGenerator("MACHINE_ID");
    var pseudoSteamID = new PseudonymGenerator("STEAM_ID");
    //Replace all user directories with pseudonyms
    content = userDirPattern.Replace(content, m => $"{m.Groups[1].Value}{pseudoUserdir.Get(m.Groups[2].Value)}");
    //Replace all other patterns with pseudonyms
    foreach (var (patternList, pseudonyms) in new[] {
        (machineIDPatterns, pseudoMachine),
        (usernamePatterns, pseudoUser),
        (userIDPatterns, pseudoUserID),
        (worldNamePatterns, pseudoWorld),
        (steamIDPatterns, pseudoSteamID),
    }) {
        var keywords = patternList
            .SelectMany(regex => regex.Matches(content))
            .Select(m => m.Groups[1].Value)
            .Where(m => !string.IsNullOrWhiteSpace(m))
            .ToHashSet();
        foreach (var keyword in keywords)
        {
            if (nonReplacedKeywords.Contains(keyword, StringComparer.InvariantCultureIgnoreCase))
            {
                continue;
            }
            Console.Error.WriteLine($"Replacing {keyword} with {pseudonyms.Get(keyword)}");
            content = content.Replace(keyword, pseudonyms.Get(keyword));
        }
    }

    Console.WriteLine(content);
}