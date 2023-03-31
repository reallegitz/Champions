using System;
using System.Collections.Generic;
using System.Security.AccessControl;

public class Team
{
    public string Name { get; set; }
    public int AttackRating { get; set; }
    public int DefenseRating { get; set; }
    public int OverallRating { get; set; }

    public Team(string name, int attackRating, int defenseRating, int overallRating)
    {
        Name = name;
        AttackRating = attackRating;
        DefenseRating = defenseRating;
        OverallRating = overallRating;
    }

    public class GroupStage
    {
        private List<Team> teams;

        public GroupStage(List<Team> teams)
        {
            if (teams.Count != 32)
            {
                throw new ArgumentException("There must be exactly 32 teams to create a group stage.");
            }

            this.teams = teams;
        }

        public Dictionary<char, List<Team>> Simulate()
        {
            var groups = new Dictionary<char, List<Team>>();

            // dar shuffle as equipas
            var shuffledTeams = teams.OrderBy(t => Guid.NewGuid()).ToList();

            // Dividir as equipas em 8 grupos
            for (char groupLetter = 'A'; groupLetter <= 'H'; groupLetter++)
            {
                var groupTeams = shuffledTeams.Skip((groupLetter - 'A') * 4).Take(4).ToList();
                groups.Add(groupLetter, groupTeams);
            }

            return groups;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var teams = new List<Team>
        {
            new Team("Barcelona", 90, 80, 85),
            new Team("Real Madrid", 92, 78, 85),
            new Team("Paris Saint-Germain", 88, 82, 85),
            new Team("Bayern Munich", 95, 85, 90),
            new Team("Liverpool", 91, 84, 88),
            new Team("Manchester City", 89, 87, 88),
            new Team("Juventus", 87, 81, 84),
            new Team("Chelsea", 86, 83, 85),
            new Team("Atletico Madrid", 85, 88, 87),
            new Team("Borussia Dortmund", 84, 76, 80),
            new Team("Manchester United", 83, 82, 83),
            new Team("Tottenham Hotspur", 82, 79, 81),
            new Team("Inter Milan", 80, 78, 79),
            new Team("AC Milan", 79, 77, 78),
            new Team("Napoli", 78, 75, 77),
            new Team("AS Roma", 77, 74, 76),
            new Team("RB Leipzig", 76, 73, 75),
            new Team("Ajax", 75, 72, 74),
            new Team("Sevilla", 74, 76, 75),
            new Team("FC Porto", 73, 70, 72),
            new Team("Benfica", 72, 69, 71),
            new Team("Olympique Lyon", 71, 68, 70),
            new Team("Shakhtar Donetsk", 70, 67, 69),
            new Team("Besiktas", 69, 66, 68),
            new Team("PSV Eindhoven", 68, 65, 67),
            new Team("Celtic", 67, 64, 66),
            new Team("Dinamo Zagreb", 66, 63, 65),
            new Team("Galatasaray", 65, 62, 64),
            new Team("Sporting CP", 200, 200, 200),
            new Team("Club Brugge", 63, 60, 62),
            new Team("CSKA Moscow", 62, 59, 61),
            new Team("Olympiacos", 61, 58, 60),
        };

            var groupStage = new GroupStage(teams);
            var groups = groupStage.Simulate();
            List<List<Team>> qualifiedList = groups.Values.ToList();
            List<Team> qualified = new List<Team>();
            foreach (List<Team> list in qualifiedList)
                foreach (Team team in list)
                    qualified.Add(team);
           
                
            var knockoutStage = new Knockout(qualified);
            var winner = knockoutStage.Simulate();

            Console.WriteLine($"The winner of the tournament is {winner.Name}!");


            foreach (var group in groups)
            {
                Console.WriteLine($"Group {group.Key}:");
                foreach (var team in group.Value)
                {
                    Console.WriteLine($"- {team.Name}");
                }
                Console.WriteLine();
              
            }

        }
    }
    public class Knockout
    {
        private List<Team> teams;

        public Knockout(List<Team> teams)
        {
            this.teams = teams;
        }

        public Team Simulate()
        {
            while (teams.Count > 1)
            {
                var winners = new List<Team>();

                for (int i = 0; i < teams.Count; i += 2)
                {
                    var team1 = teams[i];
                    var team2 = teams[i + 1];

                    var winner = SimulateMatch(team1, team2);

                    winners.Add(winner);
                }

                teams = winners;

            }

            return teams[0];
        }

        private Team SimulateMatch(Team team1, Team team2)
        {
            // Compare the overall ratings of the two teams
            if (team1.OverallRating > team2.OverallRating)
            {
                return team1;
            }
            else if (team2.OverallRating > team1.OverallRating)
            {
                return team2;
            }
            else
            {
                // In the case of a tie, simulate a penalty shootout to determine the winner
                Random random = new Random();
                int team1PenaltyScore = random.Next(0, 5);
                int team2PenaltyScore = random.Next(0, 5);

                while (team1PenaltyScore == team2PenaltyScore)
                {
                    team1PenaltyScore = random.Next(0, 5);
                    team2PenaltyScore = random.Next(0, 5);
                }

                return (team1PenaltyScore > team2PenaltyScore) ? team1 : team2;
            }
        }
    }
}