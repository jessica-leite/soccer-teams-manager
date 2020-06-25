using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Codenation.Challenge.Exceptions;
using Source.Domain;
using Source.Data;

namespace Codenation.Challenge
{
    public class SoccerTeamsManagerTest
    {
        [Fact]
        public void Should_Be_Unique_Ids_For_Teams()
        {
            var manager = new SoccerTeamsManager();
            manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
            Assert.Throws<UniqueIdentifierException>(() =>
                manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2"));
        }

        [Fact]
        public void Should_Be_Unique_Ids_For_Players()
        {
            var manager = new SoccerTeamsManager();
            manager.AddTeam(2, "Time 2", DateTime.Now, "cor 1", "cor 2");
            manager.AddPlayer(1, 2, "Player 1", new DateTime(1992, 5, 24), 100, 20000.99m);
            Assert.Throws<UniqueIdentifierException>(() =>
                manager.AddPlayer(1, 2, "Player 1", new DateTime(1992, 5, 24), 100, 20000.99m));
        }

        [Fact]
        public void Should_Be_Valid_Team_When_Add_Player()
        {
            var manager = new SoccerTeamsManager();
            Assert.Throws<TeamNotFoundException>(() =>
                manager.AddPlayer(1, 2, "Player 1", new DateTime(1992, 5, 24), 100, 20000.99m));
        }

        [Fact]
        public void Should_Be_Valid_Player_When_Set_Captain()
        {
            var manager = new SoccerTeamsManager();
            manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
            manager.AddPlayer(1, 1, "Jogador 1", DateTime.Today, 0, 0);
            manager.SetCaptain(1);
            Assert.Equal(1, manager.GetTeamCaptain(1));
            Assert.Throws<PlayerNotFoundException>(() =>
                manager.SetCaptain(2));
        }
        [Fact]
        public void Should_Be_Valid_Team_When_Get_Captain()
        {
            var manager = new SoccerTeamsManager();
            manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
            manager.AddPlayer(1, 1, "Jogador 1", DateTime.Today, 0, 0);
            manager.SetCaptain(1);
            Assert.Equal(1, manager.GetTeamCaptain(1));
            Assert.Throws<TeamNotFoundException>(() =>
                manager.GetTeamCaptain(2));
        }

        [Fact]
        public void Should_Exist_Captain_When_Get_Captain()
        {
            var manager = new SoccerTeamsManager();
            manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");
            Assert.Throws<CaptainNotFoundException>(() =>
                manager.GetTeamCaptain(1));
        }

        [Fact]
        public void Should_Ensure_Sort_Order_When_Get_Team_Players()
        {
            var manager = new SoccerTeamsManager();
            manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");

            var playersIds = new List<long>() { 15, 2, 33, 4, 13 };
            for (int i = 0; i < playersIds.Count(); i++)
                manager.AddPlayer(playersIds[i], 1, $"Jogador {i}", DateTime.Today, 0, 0);

            playersIds.Sort();
            Assert.Equal(playersIds, manager.GetTeamPlayers(1));
        }

        [Theory]
        [InlineData("10,20,300,40,50", 2)]
        [InlineData("50,240,3,1,50", 1)]
        [InlineData("10,22,24,3,24", 2)]
        public void Should_Choose_Best_Team_Player(string skills, int bestPlayerId)
        {
            var manager = new SoccerTeamsManager();
            manager.AddTeam(1, "Time 1", DateTime.Now, "cor 1", "cor 2");

            var skillsLevelList = skills.Split(',').Select(x => int.Parse(x)).ToList();
            for (int i = 0; i < skillsLevelList.Count(); i++)
                manager.AddPlayer(i, 1, $"Jogador {i}", DateTime.Today, skillsLevelList[i], 0);

            Assert.Equal(bestPlayerId, manager.GetBestTeamPlayer(1));
        }

        [Theory]
        [InlineData("Azul;Vermelho", "Azul;Amarelo", "Amarelo")]
        [InlineData("Azul;Vermelho", "Amarelo;Laranja", "Amarelo")]
        [InlineData("Azul;Vermelho", "Azul;Vermelho", "Vermelho")]
        public void Should_Choose_Right_Color_When_Get_Visitor_Shirt_Color(string teamColors, string visitorColors, string visitorMatchColor)
        {
            long teamId = 1;
            long visitorTeamId = 2;
            var teamColorList = teamColors.Split(";");
            var visitorColorList = visitorColors.Split(";");

            var manager = new SoccerTeamsManager();
            manager.AddTeam(teamId, $"Time {teamId}", DateTime.Now, teamColorList[0], teamColorList[1]);
            manager.AddTeam(visitorTeamId, $"Time {visitorTeamId}", DateTime.Now, visitorColorList[0], visitorColorList[1]);

            Assert.Equal(visitorMatchColor, manager.GetVisitorShirtColor(teamId, visitorTeamId));
        }

        [Theory]
        [InlineData(1, "Lions", "Orange", "Black")]
        [InlineData(2, "Wizzards", "Purple", "White")]
        [InlineData(3, "Potatoes", "Yellow", "Black")]
        public void Should_Include_Team(long id, string name, string mainShirtColor, string secondaryShirtColor)
        {
            var date = DateTime.Now;

            var team = new Team()
            {
                Id = id,
                Name = name,
                CreateDate = date,
                MainShirtColor = mainShirtColor,
                SecondaryShirtColor = secondaryShirtColor
            };


            new SoccerTeamsManager().AddTeam(id, name, date, mainShirtColor, secondaryShirtColor);

            Assert.True(Data.teams.ContainsKey(id));
        }
    }
}
