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
        private Data _data;
        public SoccerTeamsManager()
        {
            _data = new Data();
        }

        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            if (_data.teams.ContainsKey(id))
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

            _data.teams.Add(id, team);
        }

        public void AddPlayer(long id, long teamId, string name, DateTime birthDate, int skillLevel, decimal salary)
        {
            if (_data.players.ContainsKey(id))
            {
                throw new UniqueIdentifierException();
            }

            ThrowExceptionIfTeamsNotExists(teamId);

            var player = new Player
            {
                Id = id,
                TeamId = teamId,
                Name = name,
                BirthDate = birthDate,
                SkillLevel = skillLevel,
                Salary = salary
            };

            _data.players.Add(id, player);
        }

        public void SetCaptain(long playerId)
        {
            ThrowExceptionIfPlayerNotExists(playerId);

            var teamId = _data.players[playerId].TeamId;

            _data.teams[teamId].CaptainId = playerId;
        }

        public long GetTeamCaptain(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            var captain = _data.teams[teamId].CaptainId;

            if (captain == null)
            {
                throw new CaptainNotFoundException();
            }

            return (long)captain;
        }

        public string GetPlayerName(long playerId)
        {
            ThrowExceptionIfPlayerNotExists(playerId);

            return _data.players[playerId].Name;
        }

        public string GetTeamName(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            return _data.teams[teamId].Name;
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            return _data.players.Where(player => player.Value.TeamId == teamId)
                .Select(player => player.Key)
                .ToList();
        }

        public long GetBestTeamPlayer(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            return _data.players.Where(player => player.Value.TeamId == teamId)
                .OrderByDescending(player => player.Value.SkillLevel)
                .FirstOrDefault()
                .Value.Id;
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            return _data.players.Where(player => player.Value.TeamId == teamId)
                .OrderBy(player => player.Value.BirthDate)
                .FirstOrDefault()
                .Value.Id;
        }

        public List<long> GetTeams()
        {
            return _data.teams.Select(team => team.Value.Id).ToList();
        }

        public long GetHigherSalaryPlayer(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            return _data.players.OrderByDescending(player => player.Value.Salary)
                .FirstOrDefault()
                .Value.Id;
        }

        public decimal GetPlayerSalary(long playerId)
        {
            ThrowExceptionIfPlayerNotExists(playerId);

            return _data.players[playerId].Salary;
        }

        public List<long> GetTopPlayers(int top)
        {
            return _data.players.OrderByDescending(player => player.Value.SkillLevel)
                .Select(player => player.Value.Id)
                .Take(3)
                .ToList();
        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId, visitorTeamId);

            var mainColorHome = _data.teams[teamId].MainShirtColor;
            var mainColorVisitor = _data.teams[visitorTeamId].MainShirtColor;

            return mainColorVisitor != mainColorHome ? mainColorVisitor : _data.teams[visitorTeamId].SecondaryShirtColor;
        }

        private void ThrowExceptionIfPlayerNotExists(long id)
        {
            if (!_data.players.ContainsKey(id))
            {
                throw new PlayerNotFoundException();
            }
        }

        private void ThrowExceptionIfTeamsNotExists(params long[] ids)
        {
            foreach (var id in ids)
            {
                if (!_data.teams.ContainsKey(id))
                {
                    throw new TeamNotFoundException();
                }
            }
        }
    }
}