using Source.Domain;
using System.Collections.Generic;

namespace Source.Data
{
    public class Data
    {
        public IDictionary<long, Team> teams = new SortedDictionary<long, Team>();
        public IDictionary<long, Player> players = new SortedDictionary<long, Player>();
    }
}
