using Codenation.Challenge.Exceptions;
using Source.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codenation.Challenge
{
    public class SoccerTeamsManager : IManageSoccerTeams
    {
        private IDictionary<long, Team> _teams = new SortedDictionary<long, Team>();
        private IDictionary<long, Player> _players = new SortedDictionary<long, Player>();

        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            if (_teams.ContainsKey(id))
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

            _teams.Add(id, team);
        }

        public void AddPlayer(long id, long teamId, string name, DateTime birthDate, int skillLevel, decimal salary)
        {
            if (_players.ContainsKey(id))
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

            _players.Add(id, player);
        }

        public void SetCaptain(long playerId)
        {
            ThrowExceptionIfPlayerNotExists(playerId);

            var teamId = _players[playerId].TeamId;

            _teams[teamId].CaptainId = playerId;
        }

        public long GetTeamCaptain(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            var captain = _teams[teamId].CaptainId;

            if (captain == null)
            {
                throw new CaptainNotFoundException();
            }

            return (long)captain;
        }

        public string GetPlayerName(long playerId)
        {
            ThrowExceptionIfPlayerNotExists(playerId);

            return _players[playerId].Name;
        }

        public string GetTeamName(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            return _teams[teamId].Name;
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            return _players.Where(player => player.Value.TeamId == teamId)
                .Select(player => player.Key)
                .ToList();
        }

        public long GetBestTeamPlayer(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            return _players.Where(player => player.Value.TeamId == teamId)
                .OrderByDescending(player => player.Value.SkillLevel)
                .FirstOrDefault()
                .Value.Id;
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            return _players.Where(player => player.Value.TeamId == teamId)
                .OrderBy(player => player.Value.BirthDate)
                .FirstOrDefault()
                .Value.Id;
        }

        public List<long> GetTeams()
        {
            return _teams.Select(team => team.Value.Id).ToList();
        }

        public long GetHigherSalaryPlayer(long teamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId);

            return _players.OrderByDescending(player => player.Value.Salary)
                .FirstOrDefault()
                .Value.Id;
        }

        public decimal GetPlayerSalary(long playerId)
        {
            ThrowExceptionIfPlayerNotExists(playerId);

            return _players[playerId].Salary;
        }

        public List<long> GetTopPlayers(int top)
        {
            return _players.OrderByDescending(player => player.Value.SkillLevel)
                .Select(player => player.Value.Id)
                .Take(top)
                .ToList();
        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            ThrowExceptionIfTeamsNotExists(teamId, visitorTeamId);

            var mainColorHome = _teams[teamId].MainShirtColor;
            var mainColorVisitor = _teams[visitorTeamId].MainShirtColor;

            return mainColorVisitor != mainColorHome ? mainColorVisitor : _teams[visitorTeamId].SecondaryShirtColor;
        }

        private void ThrowExceptionIfPlayerNotExists(long id)
        {
            if (!_players.ContainsKey(id))
            {
                throw new PlayerNotFoundException();
            }
        }

        private void ThrowExceptionIfTeamsNotExists(params long[] ids)
        {
            foreach (var id in ids)
            {
                if (!_teams.ContainsKey(id))
                {
                    throw new TeamNotFoundException();
                }
            }
        }
    }
}