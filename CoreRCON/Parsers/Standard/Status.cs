using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoreRCON.Parsers.Standard
{
    public class Status : IParseable
    {
        [Obsolete("No longer part of status message")]
        public string Account { get; set; }
        public byte Bots { get; set; }
        public ulong CommunityID { get; set; }
        public string Hostname { get; set; }
        public byte Humans { get; set; }
        public string LocalHost { get; set; }
        public string Map { get; set; }
        public byte MaxPlayers { get; set; }
        public string PublicHost { get; set; }
        public string SteamID { get; set; }
        [Obsolete("No longer part of status message")]
        public string[] Tags { get; set; }
        public string Version { get; set; }
        public bool Hibernating { get; set; }
        public string Type { get; set; }
    }

    public class StatusParser : IParser<Status>
    {
        public string Pattern => throw new System.NotImplementedException();

        public bool IsMatch(string input)
        {
            return input.Contains("hostname: ") || input.Contains("hibernating");

        }

        public Status Load(GroupCollection groups)
        {
            throw new NotImplementedException();
        }

        public Status Parse(string input)
        {
            Dictionary<string, string> groups = input.Split('\n')
                .Select(x => x.Split(':'))
                .Where(x => x.Length > 1 && !string.IsNullOrEmpty(x[0].Trim())
                    && !string.IsNullOrEmpty(x[1].Trim()))
                .ToDictionary(x => x[0].Trim(), x => string.Join(":", x.Skip(1)).Trim());

            groups.TryGetValue("hostname", out string hostname);
            groups.TryGetValue("version", out string version);
            string steamId = null;
            if (version != null)
            {
                Match match = PreCompiledRegex.FindBrackets().Match(version);
                if (match.Success)
                {
                    steamId = match.Groups[1].Value;
                }
            }
            groups.TryGetValue("map", out string map);
            groups.TryGetValue("type", out string type);

            byte players = 0, bots = 0, maxPlayers = 0;
            bool hibernating = false;
            groups.TryGetValue("players", out string playerString);
            if (playerString != null)
            {
                Match oldMatch = PreCompiledRegex.FindPlayersOldPattern().Match(playerString);
                Match newMatch = PreCompiledRegex.FindPlayers().Match(playerString);
                if (oldMatch.Success)
                {
                    players = byte.Parse(oldMatch.Groups[1].Value);
                    maxPlayers = byte.Parse(oldMatch.Groups[2].Value);
                    bots = 0;
                }
                else if (newMatch.Success)
                {
                    players = byte.Parse(newMatch.Groups[1].Value);
                    maxPlayers = byte.Parse(newMatch.Groups[3].Value);
                    bots = byte.Parse(newMatch.Groups[2].Value);
                    if (newMatch.Groups[4].Success)
                    {
                        hibernating = !newMatch.Groups[4].Value.Contains("not hibernating");
                    }
                }
            }
            else
            {
                hibernating = input.Contains("hibernating") && !input.Contains("not hibernating");
            }

            string localIp = null;
            string publicIp = null;
            groups.TryGetValue("udp / ip", out string ipString);
            if (ipString != null)
            {
                Match oldMatch = PreCompiledRegex.FindIpOldPattern().Match(ipString);
                Match newMatch = PreCompiledRegex.FindIp().Match(ipString);
                if (oldMatch.Success)
                {
                    localIp = oldMatch.Groups[1].Value;
                    publicIp = oldMatch.Groups[4].Value;
                }
                else if (newMatch.Success)
                {
                    localIp = newMatch.Groups[1].Value;
                    publicIp = newMatch.Groups[2].Value;
                }
            }

            return new Status()
            {
                Hostname = hostname,
                Version = version,
                SteamID = steamId,
                Map = map,
                Type = type,
                Humans = players,
                MaxPlayers = maxPlayers,
                Bots = bots,
                Hibernating = hibernating,
                LocalHost = localIp,
                PublicHost = publicIp
            };
        }

        public Status Parse(Group group)
        {
            throw new NotImplementedException();
        }
    }
}
