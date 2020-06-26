using Codenation.Challenge.Exceptions;
using Source.Data;
using Source.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codenation.Challenge
{
    public class SoccerTeamsManager : IManageSoccerTeams
    {
        public SoccerTeamsManager()
        {
        }

        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            if (Data.teams.ContainsKey(id))
            {
                throw new UniqueIdentifierException();
            }

            var team = new Team
            {
                Id = id,
                Name = name,
                CreateDate = createDate,
                MainShirtColor = mainShirtColor,
                SecondaryShirtColor = secondaryShirtColor
            };

            Data.teams.Add(id, team);
        }

        public void AddPlayer(long id, long teamId, string name, DateTime birthDate, int skillLevel, decimal salary)
        {
            if (Data.players.ContainsKey(id))
            {
                throw new UniqueIdentifierException();
            }

            if (!Data.teams.ContainsKey(teamId))
            {
                throw new TeamNotFoundException();
            }

            var player = new Player
            {
                Id = id,
                TeamId = teamId,
                Name = name,
                BirthDate = birthDate,
                SkillLevel = skillLevel,
                Salary = salary
            };

            Data.players.Add(id, player);
        }

        public void SetCaptain(long playerId)
        {
            if (!Data.players.ContainsKey(playerId))
            {
                throw new PlayerNotFoundException();
            }

            var teamId = Data.players[playerId].TeamId;

            Data.teams[teamId].CaptainId = playerId;
        }

        public long GetTeamCaptain(long teamId)
        {
            if (!Data.teams.ContainsKey(teamId))
            {
                throw new TeamNotFoundException();
            }

            var captain = Data.teams[teamId].CaptainId;

            if (captain == null)
            {
                throw new CaptainNotFoundException();
            }

            return (long)captain;
        }

        public string GetPlayerName(long playerId)
        {
            if (!Data.players.ContainsKey(playerId))
            {
                throw new PlayerNotFoundException();
            }

            return Data.players[playerId].Name;
        }

        public string GetTeamName(long teamId)
        {
            if (!Data.teams.ContainsKey(teamId))
            {
                throw new TeamNotFoundException();
            }

            return Data.teams[teamId].Name;
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            if (!Data.teams.ContainsKey(teamId))
            {
                throw new TeamNotFoundException();
            }

            return Data.players.Where(player => player.Value.TeamId == teamId)
                .Select(player => player.Key)
                .ToList();
        }

        public long GetBestTeamPlayer(long teamId)
        {
            throw new NotImplementedException();
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            if (!Data.teams.ContainsKey(teamId))
            {
                throw new TeamNotFoundException();
            }

            return Data.players.Where(player => player.Value.TeamId == teamId)
                .OrderBy(player => player.Value.BirthDate)
                .Select(player => player.Value.Id)
                .FirstOrDefault();
        }

        public List<long> GetTeams()
        {
            return Data.teams.Select(team => team.Value.Id).ToList();
        }

        public long GetHigherSalaryPlayer(long teamId)
        {
            throw new NotImplementedException();
        }

        public decimal GetPlayerSalary(long playerId)
        {
            throw new NotImplementedException();
        }

        public List<long> GetTopPlayers(int top)
        {
            throw new NotImplementedException();
        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            throw new NotImplementedException();
        }

    }
}
