﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DawgResolver.Model;

namespace Dawg.Resolver.Winform.Test
{
    public partial class FormTile : TextBox, VTile
    {
        public VTile Tile { get; private set; }
        public Game Game { get; set; }
        public FormTile(Game g, VTile t)
        {
            Game = g;
            Tile = t;
            this.Ligne = t.Ligne;
            this.Col = t.Col;
            this.AnchorLeftMinLimit = t.AnchorLeftMinLimit;
            this.AnchorLeftMaxLimit = t.AnchorLeftMaxLimit;
            this.Controlers = t.Controlers;
            this.Width = 30;
            this.Height = 25;
            this.MaxLength = 1;
            this.Font = new Font("Verdana", 14);
            this.CharacterCasing = CharacterCasing.Upper;
            this.BackColor = GetBackColor(t);
            Text = t.Letter.Char.ToString();
            Name = $"t{t.Ligne}_{t.Col}";
            Click += FormTile_Click;

            Location = new Point(10 + t.Col * this.Width + 1, 10 + t.Ligne * this.Height + 1);
        }

        private void FormTile_Click(object sender, EventArgs e)
        {
            var txt = sender as FormTile;
            var t = txt.Tile;
            var frm = this.Parent.Parent as Form2;
            var txtProps = frm.txtTileProps;
            txtProps.Text = $"[{t.Ligne},{t.Col}] => IsAnchor:{t.IsAnchor} IsEmpty :{t.IsEmpty} => {t.Letter.Char}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += $"AnchorLeftMinLimit = {t.AnchorLeftMinLimit}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += $"AnchorLeftMaxLimit = {t.AnchorLeftMaxLimit}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += "Controlers:" + Environment.NewLine;
            foreach (var c in t.Controlers)
                txtProps.Text += $"{c.Key}:{c.Value}{Environment.NewLine}";
        }

        private Color GetBackColor(VTile t)
        {
            switch (t.TileType)
            {
                case TileType.TripleWord:
                    return Color.OrangeRed;
                case TileType.DoubleWord:
                    return Color.Coral;
                case TileType.DoubleLetter:
                    return Color.LightSkyBlue;
                case TileType.TripleLetter:
                    return Color.MediumBlue;
                case TileType.Center:
                    return Color.Gold;
                default:
                    return Color.Bisque;
            }
        }

        public Color Background { get; set; }
        public bool IsValidated { get; set; } = true;
        public bool FromJoker { get; set; } = false;
        public Dictionary<int, int> Controlers { get; set; } = new Dictionary<int, int>(27);
        public int Ligne { get; set; }
        public int Col { get; set; }
        public int LetterMultiplier { get; set; }
        public int WordMultiplier { get; set; }
        public Letter Letter { get; set; }
        public int AnchorLeftMaxLimit { get; set; }
        public int AnchorLeftMinLimit { get; set; }
        public bool IsAnchor
        {
            get
            {
                return Tile.IsAnchor;
            }
        }

        public TileType TileType
        {
            get
            {
                return Tile.TileType;

            }
        }

        public bool IsEmpty
        {
            get
            {
                return Tile.IsEmpty;
            }
        }

        public VTile LeftTile
        {
            get
            {
                return Tile.LeftTile;
            }
        }
        public VTile RightTile
        {
            get
            {
                return Tile.RightTile;
            }
        }
        public VTile DownTile
        {
            get
            {
                return Tile.DownTile;
            }
        }
        public VTile UpTile
        {
            get
            {
                return Tile.UpTile;
            }
        }

    }
}
