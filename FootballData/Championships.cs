using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballData
{
    public class Championships
    {
        private static Dictionary<string, int> Teams;

        public Championships()
        {
            if (Teams == null || Teams.Count == 0)
                InitializeTeams();
        }

        private void InitializeTeams()
        {
            Teams = new Dictionary<string, int>();
            Teams.Add("New England", 1);
            Teams.Add("Cincinnati", 2);
            Teams.Add("Denver", 3);
            Teams.Add("Kansas", 4);
            Teams.Add("New York", 5);
            Teams.Add("Pittsburgh", 6);
            Teams.Add("Houston", 7);
            Teams.Add("Buffalo", 8);
            Teams.Add("Indianapolis", 9);
            Teams.Add("Oakland", 10);
            Teams.Add("Miami", 11);
            Teams.Add("Baltimore", 12);
            Teams.Add("Jacksonville", 13);
            Teams.Add("San Diego", 14);
            Teams.Add("Cleveland", 15);
            Teams.Add("Tennessee", 16);
        }

        public List<string> GetTeams()
        {
            var sorted = Teams.OrderBy(p => p.Value);
            List<string> orderedTeams = new List<string>();
            foreach (var e in sorted)
            {
                orderedTeams.Add(e.Key);
            }

            return orderedTeams;
        }

        public int GetTeamCount()
        {
            return Teams.Count;
        }

        public void Reset()
        {
            try
            {
                InitializeTeams();
            }
            catch (Exception)
            {
                throw new FootballDataException();
            }
        }

        public void RemoveTeam(string name)
        {
            string Key = "";
            foreach (var key in Teams.Keys)
            {
                if (key.ToLower().Equals(name.ToLower()))
                {
                    Key = key;
                }
            }
            if (!string.IsNullOrEmpty(Key))
                Teams.Remove(Key);
            else
                throw new TeamNotFoundException();

        }

        public bool DoesTeamExist(string name)
        {
            foreach (var key in Teams.Keys)
            {
                if (key.ToLower().Equals(name.ToLower()))
                    return true;
            }
            return false;
        }

        public string GetHighestRatedTeam()
        {
            var sorted = Teams.OrderBy(p => p.Value);
            return sorted.First().Key;
        }

        public List<string> GetTopThreeTeams()
        {
            var sorted = Teams.OrderBy(p => p.Value);
            List<string> rtns = new List<string>();
            rtns.Add(sorted.ElementAt(0).Key);
            rtns.Add(sorted.ElementAt(1).Key);
            rtns.Add(sorted.ElementAt(2).Key);

            return rtns;
        }

        public string GetLowestRatedTeam()
        {
            var sorted = Teams.OrderBy(p => p.Value);
            return sorted.Last().Key;
        }
    }
}
