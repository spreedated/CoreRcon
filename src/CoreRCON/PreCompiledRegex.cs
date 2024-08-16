using System.Text.RegularExpressions;

namespace CoreRCON
{
    internal static partial class PreCompiledRegex
    {
        [GeneratedRegex(@"L (\d{2}/\d{2}/\d{4} - \d{2}:\d{2}:\d{2}):", RegexOptions.Compiled)]
        internal static partial Regex GetTimestamp();

        [GeneratedRegex(@"\r\n|\n\r|\n|\r", RegexOptions.Compiled)]
        internal static partial Regex FindCarriageAndLineFeedReturns();

        [GeneratedRegex(".*(\\[.*\\]).*", RegexOptions.Compiled)]
        internal static partial Regex FindBrackets();

        [GeneratedRegex("(\\d+) \\((\\d+) max\\).*", RegexOptions.Compiled)]
        internal static partial Regex FindPlayersOldPattern();

        [GeneratedRegex("(\\d+) humans, (\\d+) bots\\((\\d+)/\\d+ max\\) (\\(not hibernating\\))?.*", RegexOptions.Compiled)]
        internal static partial Regex FindPlayers();

        [GeneratedRegex("((\\d|\\.)+:(\\d|\\.)+)\\(public ip: (.*)\\).*", RegexOptions.Compiled)]
        internal static partial Regex FindIpOldPattern();

        [GeneratedRegex("\\((.*:.*)\\)\\s+\\(public ip: (.*)\\).*", RegexOptions.Compiled)]
        internal static partial Regex FindIp();
    }
}
