﻿using DawgResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver.Model
{
    public enum MovementDirection
    {
        Across, Down, None
    };
    public class Word
    {
        public bool IsPlayedByPlayer1 { get; set; }
        public int Index { get; set; }
        public Game Game { get; set; }
        public Word(Game g)
        {
            Game = g;
            StartTile = new Tile(Game,(int) Game.BoardSize/2, (int)Game.BoardSize / 2);
        }
        public int SetWord(Player p, bool validate)
        {
            this.StartTile.SetWord(p, Text, Direction, validate);
            return this.Points;
        }
        public bool Scramble { get; set; }

        public MovementDirection Direction { get; set; }
        public VTile StartTile { get; set; }
        public int Points
        {
            get; set;
        }
        public string Text { get; set; }

        public string DisplayInList
        {
            get
            {
                return $"{DateTime.Now:H:mm:ss} - {(Game.IsPlayer1 ? $"Player 1:{Game.Player1.Rack.String()}" : $"Player 2:{Game.Player2.Rack.String()}")} --> {this}";
            }
        }
        public string Serialize
        {
            get
            {
                return $"{StartTile.Ligne};{StartTile.Col};{Text};{Points};{Direction};{(Scramble ? "*" : "-")};{Index}" + Environment.NewLine;
            }
        }
        public string DisplayText
        {
            get
            {
                var pos = $"{Game.Alphabet[StartTile.Ligne]}{StartTile.Col + 1}";
                if (Direction == MovementDirection.Down)
                    pos = $"{StartTile.Col + 1}{Game.Alphabet[StartTile.Ligne]}";
                return $"[{pos}] {Text} ({Points}){(Scramble ? "*" : "")}";
            }
        }
        public bool IsAllowed
        {
            get
            {
                return Game.Dico.MotAdmis(Text);
            }
        }
        public List<VTile> GetTiles()
        {
            var ret = new List<VTile>();
            VTile t = null;
            if (StartTile.Col == Game.BoardSize - 1)
                t = StartTile.LeftTile.RightTile;
            else
                t = StartTile.RightTile.LeftTile;

            ret.Add(t);
            for (int i = 1; i < Text.Length; i++)
            {
                if (t != null)
                    if (Direction == MovementDirection.Across)
                    {
                        t = t.RightTile;
                    }
                    else
                    {
                        t = t.DownTile;
                    }
                if (t != null)
                    ret.Add(t);
            }
            return ret;
        }


        public override string ToString()
        {
            return DisplayText;
        }
        public bool Equals(Word w)
        {
            return w.Direction == Direction && w.StartTile.Col == StartTile.Col && w.StartTile.Ligne == StartTile.Ligne && w.Text == Text;
        }
    }
}
