﻿using DawgResolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver.Model
{
    public class Bag
    {
        public Bag()
        {
            Letters = new List<Letter>(Game.GameStyle == 'S' ? Game.AlphabetScrabbleAvecJoker : Game.AlphabetWWFAvecJoker);
        }

        public List<Letter> Letters { get; set; }
        public string FlatList
        {
            get
            {
                var ret = "";
                foreach (var l in Letters)
                {
                    ret += new string(l.Char, l.Count);
                }
                return ret;
            }
        }
        // On compte le nombre de lettres restantes dans le sac et on établit la liste des choix
        public int LeftLettersCount
        {
            get => Letters.Sum(t => t.Count);

        }
        public void RemoveLetterFromBag(char c)
        {
            var letter = Game.GameStyle == 'S' ? Game.AlphabetScrabbleAvecJoker.Find(cc => cc.Char == c) : Game.AlphabetWWFAvecJoker.Find(cc => cc.Char == c);
            if (letter.Count > 0) letter.Count = --letter.Count;
        }

        public Letter GetLetterInFlatList(Random r)
        {
            if (!FlatList.Any())
            {
                throw new ArgumentException("No more letters in bag!");
            }

            int charIdx = r.Next(0, FlatList.Length - 1);
            var c = FlatList[charIdx];
            if (c == Game.EmptyChar) throw new ArgumentException(nameof(c));
            var letter = Game.GameStyle == 'S' ? Game.AlphabetScrabbleAvecJoker.First(cc => cc.Char == c) : Game.AlphabetWWFAvecJoker.First(cc => cc.Char == c);

            var le = Letters.Find(l => l.Char == letter.Char);
            if (letter.Count > 0) letter.Count = --letter.Count;
            return le;

        }
        public void PutBackLetterInBag(Letter l)
        {
            var letter = Game.GameStyle == 'S' ? Game.AlphabetScrabbleAvecJoker.Find(cc => cc == l) : Game.AlphabetWWFAvecJoker.Find(cc => cc == l);
            letter.Count++;
        }

        public string GetBagContent(int split = 5)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{LeftLettersCount} lettres restantes");
            int idx = 0;
            foreach (var l in Letters)
            {
                sb.Append($"{l.Char}({l.Value}):{l.Count}\t");
                idx++;
                if (idx % split == 0) sb.AppendLine();
            }
            return sb.ToString();
        }

        public void GetLetters(Player p, string forcedLetters = null)
        {
            if (p.Rack.Count() > 7) return;

            if (!string.IsNullOrWhiteSpace(forcedLetters))
            {
                p.Rack = forcedLetters.Select(c => Game.GameStyle == 'S' ? Game.AlphabetScrabbleAvecJoker.Find(cc => cc.Char == c) : Game.AlphabetWWFAvecJoker.Find(cc => cc.Char == c)).ToList();
                return;
            }
            // Si le sac est vide
            if (LeftLettersCount == 0) return;
            //throw new ArgumentException("Il n'y a plus de lettres dans le sac");

            // S'il reste 7 lettres ou moins dans le sac, on n'a pas le choix, on les prend toutes

            int lettersToTakeCount = 7 - p.Rack.Count();
            if (Math.Abs(lettersToTakeCount) > 7) lettersToTakeCount = 0;
            Random rnd = new Random();
            // Sinon on tire 7 lettres du sac à condition qu'il en reste suffisament
            for (int i = 0; i <= Math.Min(FlatList.Length, lettersToTakeCount - 1); i++)
            {
                var letter = GetLetterInFlatList(rnd);

                p.Rack.Add(letter);
            }
       

        }
    }
}
