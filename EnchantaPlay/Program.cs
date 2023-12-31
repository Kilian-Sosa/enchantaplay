﻿using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace EnchantaPlay {
    public class Program {
        static List<Game> Games = new List<Game>();
        static List<GameCase> GameCases = new List<GameCase>();
        static string basePath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName;
        static string gamesPath = Path.Combine(basePath, "games.json");
        static string logoString = @"                ___________             .__                   __        __________.__                
                \_   _____/ ____   ____ |  |__ _____    _____/  |______ \______   \  | _____  ___.__.
                 |    __)_ /    \_/ ___\|  |  \\__  \  /    \   __\__  \ |     ___/  | \__  \<   |  |
                 |        \   |  \  \___|   Y  \/ __ \|   |  \  |  / __ \|    |   |  |__/ __ \\___  |
                /_______  /___|  /\___  >___|  (____  /___|  /__| (____  /____|   |____(____  / ____|
                        \/     \/     \/     \/     \/     \/          \/                   \/\/     
            ";
        //TODO: Allow possibility to save nickname & store score (Leaderboard)
        public static void Main(string[] args) {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // For the special symbols to work
            PrintIntro();
            GetInput();
        }

        private static void PrintIntro() {
            ShowHeader();
            Console.WriteLine("Welcome to EnchantaPlay!");
            Console.WriteLine("Press any key to continue..."); 
            Console.ReadKey();
        }

        private static void PrintOptions() {
            ShowHeader();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Play a game");
            Console.WriteLine("2. Develop your own game (coming soon)");
            Console.WriteLine("3. View the leaderboard");
            Console.WriteLine("4. Exit");
        }

        private static void GetInput() {
            PrintOptions();
            var input = Console.ReadLine();
            switch (input) {
                case "1":
                    SetGames();
                    ShowGames();
                    SelectGame();
                    break;
                case "2":
                    //DevelopGame();
                    GetInput();
                    break;
                case "3":
                    //ViewLeaderboard();
                    GetInput();
                    break;
                case "4":
                    Exit();
                    break;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    GetInput();
                    break;
            }
        }
        private static void ShowHeader() {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(logoString);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static List<Game> GetGames() {
            var games = new List<Game>();
            string jsonResult = File.ReadAllText(gamesPath);
            return JsonSerializer.Deserialize<List<Game>>(jsonResult);
        }

        private static void SetGames() {
            Games = GetGames();
            GameCases = new List<GameCase>();
            foreach (var game in Games) GameCases.Add(new GameCase(game));
        }

        private static void ShowGames() {
            ShowHeader();

            for (int i = 0; i < 20; i++) {
                Console.Write("    ");
                foreach (var gameCase in GameCases) {
                    Console.ForegroundColor = gameCase.Color;
                    Console.Write($"{gameCase.ToString().Split('\n')[i]} ");
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void SelectGame() {            
            do {
                Console.Write("Please select a game writting the title. Ex: Hangman");
                Console.Write("\n>> ");
                string answer = Console.ReadLine() ?? "".ToLower();
                foreach (var game in Games) {
                    if (game.Title == answer) {
                        string gameExePath = Path.Combine(basePath, $@"Games\{game.Title}\bin\Debug\net7.0\{game.Title}.exe");
                        while (true) {
                            Console.Clear();
                            Process gameProcess = new Process();
                            gameProcess.StartInfo.FileName = gameExePath;
                            gameProcess.Start();
                            gameProcess.WaitForExit();

                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\nDo you want to play again? (y/n)");
                            Console.ForegroundColor = ConsoleColor.White;
                            answer = Console.ReadLine() ?? "".ToLower();
                            if (answer != "y") break;
                        }
                        ShowGames();
                    }
                }
            } while (true);
        }

        private static void Exit() {
            ShowHeader();
            Console.WriteLine("Thanks for playing!");
            Environment.Exit(0);
        }
    }
}