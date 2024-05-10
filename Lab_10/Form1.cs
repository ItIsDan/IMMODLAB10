using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Lab_10
{
    public partial class Form1 : Form
    {
        public List<Team> teams;
        public List<GameObject> games = new List<GameObject>();
        Random rand = new Random();
        public int amount = 8;
        public List<DataGridViewRow> rows = new List<DataGridViewRow>();

        public Form1()
        {   
            InitializeComponent();

            teams = new List<Team>
            {
                new Team("Chelsea", 0, 1.5),
                new Team("Arsenal", 0, 1.25),
                new Team("Liverpool", 0, 1.25),
                new Team("Man City", 0, 1.2),
                new Team("Man Utd", 0, 1),
                new Team("Everton", 0, 0.95),
                new Team("Stoke", 0, 0.9),
                new Team("Hull", 0, 0.9)
            };
            table.DataSource = teams;
        }

        public class Team
        {
            public string Name { get; set; }
            public int Points { get; set; }
            private double lambda { get; set; }

            public double getLambda()
            {
                return this.lambda;
            }

            public Team(string Name, int Points, double lambda)
            {
                this.Name = Name;
                this.Points = Points;
                this.lambda = lambda;
            }
        }

        public class GameObject
        {
            public Team team1 { get; set; }
            public Team team2 { get; set; }
            public int team1Score { get; set; }
            public int team2Score { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < amount; i++)
            {
                for(int j = i + 1; j < amount; j++)
                {
                    games.Add(new GameObject
                    {
                        team1 = teams[i],
                        team2 = teams[j]
                    });
                }
            }

            for (int i = 0; i < 28; i++) // 7 + 6 + 5 + 4 + 3 + 2 + 1
            {
                double a = rand.NextDouble();
                double lambda1 = games[i].team1.getLambda();
                double sum = 0;
                int m = 0;

                while (sum > lambda1 * (-1)) { // Датчик Пуассона
                    sum += Math.Log(a);
                    m++;
                }
                games[i].team1Score = m;

                sum = 0;
                m = 0;
                a = rand.NextDouble();

                double lambda2 = games[i].team2.getLambda();

                while (sum > lambda2 * (-1)) { // Датчик Пуассона
                    sum += Math.Log(a);
                    m++;
                }
                games[i].team2Score = m; // x = m;

                if (games[i].team1Score > games[i].team2Score)
                {
                    games[i].team1.Points += 3;
                    continue;
                }

                else if (games[i].team1Score < games[i].team2Score) { 
                    games[i].team2.Points += 3;
                    continue;
                }

                else
                {
                    games[i].team1.Points += 1;
                    games[i].team2.Points += 1;
                    continue;
                }
            }

            table.Refresh();
            
            List<Team> SortedList = teams.OrderByDescending(o => o.Points).ToList();

            table.DataSource = SortedList;
           
            for(int i = 0; i < games.Count; i++)
                results.Rows.Add("1", games[i].team1.Name + " vs " + games[i].team2.Name, games[i].team1Score + " - " + games[i].team2Score);

            int count = 0;
            for(int i = 0; i < amount - 1; i++)
                for(int j = i; j < amount - 1; j++)
                {
                    results.Rows[count].Cells[0].Value = j + 1;
                    count++;
                }
        }

        private void table_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
