using Codenation.Challenge.Exceptions;
using Source.Data;
using Source.Domain;
using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }

        public long GetTeamCaptain(long teamId)
        {
            throw new NotImplementedException();
        }

        public string GetPlayerName(long playerId)
        {
            throw new NotImplementedException();
        }

        public string GetTeamName(long teamId)
        {
            throw new NotImplementedException();
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            throw new NotImplementedException();
        }

        public long GetBestTeamPlayer(long teamId)
        {
            throw new NotImplementedException();
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            throw new NotImplementedException();
        }

        public List<long> GetTeams()
        {
            throw new NotImplementedException();
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
