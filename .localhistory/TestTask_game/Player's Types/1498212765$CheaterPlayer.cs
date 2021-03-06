﻿using System;
using System.Collections.Generic;

namespace TestTask_game
{
    public class CheaterPlayer : IPlayer
    {
        public CheaterPlayer()
        {
            AlreadyTriedNumbers = new List<int>();
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public int CountOfAttempts { get; set; }
        public List<int> AlreadyTriedNumbers { get; set; }

        public int GetNumberToGuess()
        {
            Random random = new Random();
            int guessNumber = random.Next(10, 100);
        Check: if (AlreadyTriedNumbers.Contains(guessNumber))
            {
                guessNumber = random.Next(10, 40);
                goto Check;
            }
            AlreadyTriedNumbers.Add(guessNumber);
            CountOfAttempts++;
            return guessNumber;
        }
    }
}
