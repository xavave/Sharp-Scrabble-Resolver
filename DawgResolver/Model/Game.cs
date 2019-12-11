﻿using DawgResolver;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace DawgResolver.Model
{
    public class Game
    {
        public const int BoardSize = 11;
        private VTile[,] grid = new VTile[Game.BoardSize, Game.BoardSize];
        public string GenerateHtml(VTile[,] Tiles)
        {
            StringBuilder sb = new StringBuilder("<html><div>");
            for (int ligne = 0; ligne < Tiles.GetLength(0); ligne++)
            {

                sb.Append("|");
                for (int col = 0; col < Tiles.GetLength(1); col++)
                {
                    //TODO 
                }
                sb.Append("|<br/>");
            }
            sb.Append("</div></html>");
            return sb.ToString();
        }

        public string GenerateTextGrid(VTile[,] Tiles, bool? printAnchor = false, bool printLetterValue = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("123456789012345");
            sb.AppendLine("_______________");
            for (int ligne = 0; ligne < Tiles.GetLength(0); ligne++)
            {
                sb.Append($"{Alphabet[ligne].Char}|");
                for (int col = 0; col < Tiles.GetLength(1); col++)
                {

                    var tile = Tiles[ligne, col];

                    if (printAnchor.HasValue && printAnchor.Value)
                    {
                        if (!tile.IsEmpty)
                        {

                        }
                        sb.Append(tile.IsAnchor ? "@" : tile.IsEmpty ? "0" : tile.Letter.Char.ToString());
                        //var lm = tile.LetterMultiplier == 2 ? (char)0xB2 : tile.LetterMultiplier == 3 ? (char)0XB3 : '0';
                        //var wm = tile.WordMultiplier == 2 ? '2' : tile.WordMultiplier == 3 ? '3' : '0';
                        //sb.Append(tile.IsAnchor ? "@" : tile.IsEmpty ? tile.LetterMultiplier > 1 ? lm.ToString() : wm.ToString() : printLetterValue ? (tile.Letter.Value * tile.LetterMultiplier).ToString() : tile.Letter.ToString());
                    }
                    else if (printAnchor.HasValue)
                    {
                        sb.Append(tile.IsEmpty ? "0" : tile.Letter.ToString());
                    }
                    else
                    {
                        var lm = tile.LetterMultiplier == 2 ? (char)0xB2 : tile.LetterMultiplier == 3 ? (char)0XB3 : '0';
                        var wm = tile.WordMultiplier == 2 ? '2' : tile.WordMultiplier == 3 ? '3' : '0';
                        sb.Append(tile.IsValidated ? "V" : "0");
                    }

                }
                sb.AppendLine();
            }
            sb.AppendLine("_______________");
            return sb.ToString();
        }
        public Dictionnaire Dico { get; }
        public const char Joker = '?';
        public bool IsPlayer1 { get; set; } = true;
        public bool EndGame { get; set; } = false;

        public Resolver Resolver { get; }
        public Bag Bag { get; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Game()
        {
            Dico = LoadDico();
            InitBoard();
            Player1 = new Player(this);
            Player2 = new Player(this);
            Resolver = new Resolver(this);
            Bag = new Bag();
            //Bag.Letters = new List<Letter>(Game.AlphabetAvecJoker);
            Bag.Letters.ResetCount();
            NoMoreMovesCount = 0;
            EndGame = false;
            IsPlayer1 = true;

        }
        private Dictionnaire LoadDico()
        {
            var dic = new Dictionnaire();
            dic.ChargerFichierDAWG();

            return dic;
        }
        public static List<Letter> Alphabet
        {
            get { return AlphabetAvecJoker.Take(26).ToList(); }
        }

        public List<Letter> ClearTilesInPlay(Player p)
        {

            for (int i = 0; i < Grid.OfType<VTile>().Count(); i++)
            {
                var tile = Grid.OfType<VTile>().ElementAt(i);
                if (!tile.IsValidated)
                {
                    if (!tile.IsEmpty)
                    {
                        if (tile.FromJoker)
                        {
                            p.Rack.Add(Game.AlphabetAvecJoker[26]);
                        }
                        else
                        {
                            p.Rack.Add(tile.Letter);
                        }
                        tile.IsValidated = true;
                    }
                    Grid[tile.Ligne, tile.Col].Letter = new Letter();
                }
            }
            return p.Rack;
        }

        public static List<Letter> AlphabetAvecJoker { get; } = new List<Letter>()
        {
            new Letter('A',1,9),
            new Letter('B',5,2),
            new Letter('C',4,2),
            new Letter('D',3,5),
            new Letter('E',1,13),
            new Letter('F',5,2),
            new Letter('G',5,3),
            new Letter('H',5,4),
            new Letter('I',1,8),
            new Letter('J',8,1),
            new Letter('K',10,1),
            new Letter('L',2,4),
            new Letter('M',4,2),
            new Letter('N',1,5),
            new Letter('O',4,8),
            new Letter('P',4,2),
            new Letter('Q',10,1),
            new Letter('R',1,6),
            new Letter('S',1,5),
            new Letter('T',1,7),
            new Letter('U',2,4),
            new Letter('V',8,2),
            new Letter('W',10,1),
            new Letter('X',10,1),
            new Letter('Y',10,1),
            new Letter('Z',10,1),
            new Letter(Joker,0,2),

        };





        public VTile[,] InitBoard()
        {
            // Définition des cases bonus
            var assembly = Assembly.GetExecutingAssembly();

            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith($"initial_board{BoardSize}.txt"));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, true))
            {
                string content = reader.ReadToEnd();

                int row = 0;
                int col = 0;
                foreach (var w in content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    foreach (var tp in w.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Grid[row, col] = new Tile(this, row, col);

                        if (string.IsNullOrEmpty(tp))
                            continue;

                        switch (tp.Trim())
                        {
                            case "RE":
                            case "__":
                                Grid[row, col].WordMultiplier = 1;
                                Grid[row, col].LetterMultiplier = 1;
                                break;
                            case "CE":

                                break;
                            case "TW":
                                Grid[row, col].WordMultiplier = 3;
                                break;
                            case "TL":
                                Grid[row, col].LetterMultiplier = 3;
                                break;
                            case "DW":
                                Grid[row, col].WordMultiplier = 2;
                                break;
                            case "DL":
                                Grid[row, col].LetterMultiplier = 2;
                                break;
                            default:
                                throw new Exception($"Unknown tile type in inital_board file: {tp}");
                        }
                        col += 1;
                    }

                    col = 0;
                    row += 1;
                }



                ////'Initialisation du contenu du sac d'où sont tirées les lettres
                //for (int nl = 0; nl < 27; nl++)
                //{
                //    BagContent[nl] = LettersCount[nl];
                //    //LetterValue[nl] = LetterPoints[nl];
                //}
                //Afficher_Contenu_Sac
                return Grid;
            }
        }
        public VTile[,] Grid
        {
            get => grid; set { grid = value; }
        }
        public static bool IsTransposed { get; set; } = false;
        public bool FirstMove
        {
            get
            {
                return Grid.OfType<VTile>().First(t=>t.TileType== TileType.Center).IsEmpty;
            }
        }

        public int NoMoreMovesCount { get; set; } = 0;

        public string Serialise()
        {
            var ret = $"P1?{IsPlayer1}" + Environment.NewLine;
            ret += "letters" + Environment.NewLine;
            foreach (var l in AlphabetAvecJoker)
                ret += l.Serialize + Environment.NewLine;

            ret += "tiles" + Environment.NewLine;
            foreach (var t in Grid)
            {
                ret += t.Serialize + Environment.NewLine;
            }

            ret += "P1moves" + Environment.NewLine;

            foreach (var w in Player1.Moves)
            {
                ret += $"M1{w.Serialize}";
            }
            ret += "P2moves" + Environment.NewLine;
            foreach (var w in Player2.Moves)
            {
                ret += $"M2{w.Serialize}";
            }
            return ret;
        }

        public void Deserialize(string txt)
        {
            InitBoard();
            var alphabet = new List<Letter>();
            var tiles = new List<VTile>();
            var lines = txt.Split(Environment.NewLine.ToCharArray());
            foreach (var l in lines)
            {
                if (l.StartsWith("P1?"))
                {
                    IsPlayer1 = bool.Parse(l.Substring(3));
                }
                if (l.StartsWith("L"))
                {
                    alphabet.Add(l.DeserializeLetter());
                }
                else
                if (l.StartsWith("T"))
                {
                    tiles.Add(l.DeserializeTile(this));
                }
                else
                if (l.StartsWith("M1"))
                {
                    Player1.Moves.Add(l.DeserializeMove(this));
                }
                else
                     if (l.StartsWith("M2"))
                {
                    Player2.Moves.Add(l.DeserializeMove(this));
                }
            }
            for (int idx = 0; idx < Bag.Letters.Count; idx++)
            {
                Bag.Letters[idx].Count = alphabet[idx].Count;
            }
            foreach (var t in tiles)
            {
                Grid[t.Ligne, t.Col] = t;
            }
            Player1.Points = Player1.Moves.Sum(m => m.Points);
            Player2.Points = Player2.Moves.Sum(m => m.Points);

        }
    }
}
