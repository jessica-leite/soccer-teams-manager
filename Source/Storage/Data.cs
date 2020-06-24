using Source.Domain;
using System.Collections.Generic;

namespace Source.Data
{
    public static class Data
    {
        public static IDictionary<long, Team> teams = new SortedDictionary<long, Team>();
        public static IDictionary<long, Player> players = new SortedDictionary<long, Player>();
    }
}
