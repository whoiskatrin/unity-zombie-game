﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestTask_game
{
    public class Program
    {


        private static int _overallAmountOfAttempts = 0;
        static void Main(string[] args)
        {

            bool isGameOver = false;

            int weightOfBasket = Game.SetWeightForBasket();
            Console.WriteLine("Real weight : " + weightOfBasket);

            Console.WriteLine("Count of players in this game?");
            var countOfPlayers = Convert.ToInt32(Console.ReadLine());

            var players = new List<IPlayer>();
            for (int i = 0; i < countOfPlayers; i++)
            {
                IPlayer player = null;
                Console.WriteLine("Player's name:");
                string playerName = Console.ReadLine();
                Console.WriteLine("Player's type:");
                string playerType = Console.ReadLine();
                bool isExist = playerType != null && Enum.IsDefined(typeof(Game.Type), value: playerType);
                if (isExist)
                {
                    Type type = Type.GetType("TestTask_game." + playerType + "Player");
                    if (type != null)
                    {
                        player = (IPlayer)Activator.CreateInstance(type);
                    }
                    if (player != null)
                    {
                        player.Name = playerName;
                        player.Type = playerType;
                        players.Add(player);
                    }
                }
            }
            foreach (var player in players)
            {
                Thread thread = new Thread(() => TryToGuess(player, weightOfBasket));
                thread.Start();
            }

            Console.ReadLine();
        }

        private static void TryToGuess(IPlayer player, int weightOfBasket, object objectToLoc)
        {
            object objectToLock = new object();
            lock (objectToLock)
            {
                bool IsWinner = false;
                while (!IsWinner)
                {
                    if (_overallAmountOfAttempts == Game.AttemptsLimit)
                    {
                        IsWinner = true;
                        // FindClosestToWinPlayer(players, weightOfBasket);
                        Thread.CurrentThread.Join();
                        return;
                    }
                    _overallAmountOfAttempts++;

                    int numberToGuess = player.GetNumberToGuess();
                    Game.AllNumberAttempts.Add(numberToGuess);

                    Console.WriteLine("Number is " + numberToGuess + "?");
                    Console.WriteLine("Attempt was made by " + player.Name);

                    if (numberToGuess == weightOfBasket)
                    {
                        Console.WriteLine(player.Name + " is a winner. Count of attempts = " + player.CountOfAttempts);
                        IsWinner = true;
                        Thread.CurrentThread.Join();
                    }
                    else
                    {
                        int delta = weightOfBasket - numberToGuess;
                        Thread.Sleep(Math.Abs(delta) * 1000);
                        IsWinner = false;
                    }
                    Console.WriteLine(_overallAmountOfAttempts);

                }
            }
        }

        public static void FindClosestToWinPlayer(List<IPlayer> playesrList, int weight)
        {
            int closestNumberPosition = Int32.MaxValue;
            string winnerName = String.Empty;
            foreach (var player in playesrList)
            {
                int currentPlayerClosestNumberPosition = player.AlreadyTriedNumbers.FindIndex(item => item == player.AlreadyTriedNumbers.Aggregate((x, y) => Math.Abs(x - weight) < Math.Abs(y - weight) ? x : y));
                if (currentPlayerClosestNumberPosition < closestNumberPosition)
                {
                    closestNumberPosition = currentPlayerClosestNumberPosition;
                    winnerName = player.Name;
                }
            }
            Console.WriteLine("Winner is " + winnerName);
        }
    }
}
