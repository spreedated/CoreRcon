using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoreRCON.Parsers.Factorio
{
    public class Playercollection : IParseable, ICollection<Player>
    {
        private readonly List<Player> players;

        public DateTime DateTime { get; } = DateTime.Now;

        #region Constructor
        public Playercollection()
        {
            this.players = [];
        }

        public Playercollection(IEnumerable<Player> playerlist)
        {
            this.players = playerlist.ToList();
        }
        #endregion

        public Player this[int index]
        {
            get
            {
                if (index <= -1)
                {
                    index = 0;
                }

                return this.players[index];
            }
        }

        public Player this[string name]
        {
            get
            {
                return this.players.Find(x => x.Name == name);
            }
        }

        public int Count => this.players.Count;

        public bool IsReadOnly => true;

        public void Add(Player item)
        {
            throw new MemberAccessException("This collection is read-only");
        }

        public void Clear()
        {
            throw new MemberAccessException("This collection is read-only");
        }

        public bool Contains(Player item)
        {
            return this.players.Contains(item);
        }

        public void CopyTo(Player[] array, int arrayIndex)
        {
            this.players.ToArray().CopyTo(array, arrayIndex);
        }

        public IEnumerator<Player> GetEnumerator()
        {
            return this.players.GetEnumerator();
        }

        public bool Remove(Player item)
        {
            throw new MemberAccessException("This collection is read-only");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.players.GetEnumerator();
        }
    }

    public class PlayercollectionParser : DefaultParser<Playercollection>
    {
        public override string Pattern => ".*";

        public override Playercollection Load(GroupCollection groups)
        {
            string raw = groups[0].Value;

            return new(raw.Split('\n').Skip(1).Where(x => !string.IsNullOrEmpty(x)).Select(x => new Player() { Name = x.Replace("(online)", "").Trim(), Online = x.Contains("(online)") }));
        }
    }
}
