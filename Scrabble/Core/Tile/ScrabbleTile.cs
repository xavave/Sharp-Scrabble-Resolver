﻿using DawgResolver;
using DawgResolver.Model;
using Scrabble.Core.Tile;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble.Core
{
    public interface ITile : VTile
    {
        void ClearHighlight();
        void OnHighlight(bool valid);
        ScrabbleForm ScrabbleForm { get; set; }
        TileType TileType { get; set; }
        bool TileInPlay { get; set; }
        string Text { get; set; }
    }
    //public class VirtualTile : ITile
    //{
    //    public ScrabbleForm ScrabbleForm { get; set; }
    //    public VirtualTile(ScrabbleForm scrabbleForm)
    //    {
    //        ScrabbleForm = scrabbleForm;
    //    }
    //    public int XLoc { get; set; }
    //    public int YLoc { get; set; }
    //    public TileType TileType { get; set; }
    //    public string Text { get; set; }
    //    public bool TileInPlay { get; set; }

    //    public void ClearHighlight()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void OnHighlight(bool valid)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class ScrabbleTile : TextBox, ITile
    {
        public ScrabbleTile(ScrabbleForm scrabbleForm)
        {
            InitializeComponent();
            ScrabbleForm = scrabbleForm;
            this.CharacterCasing = CharacterCasing.Upper;

            this.TextChanged += (s, e) =>
            {
                var tile = (ScrabbleTile)s;

                tile.OnLetterPlaced(this.Text);
                var rackTile = ScrabbleForm.PlayerManager.CurrentPlayer.Tiles.Find(r => r.Text == this.Text);
                if (rackTile != null)
                    rackTile.Text = "";
            };
        }
        public ScrabbleForm ScrabbleForm { get; set; }
        public int Ligne { get; set; }
        public int Col { get; set; }

        public bool TileInPlay { get; set; }
        public TileType TileType { get; set; }
        public Letter Letter { get; set; }
        ScrabbleForm ITile.ScrabbleForm { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        TileType ITile.TileType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool ITile.TileInPlay { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string ITile.Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool VTile.IsValidated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        TileType VTile.TileType => throw new NotImplementedException();

        int VTile.Ligne { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int VTile.Col { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Letter VTile.Letter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int VTile.LetterMultiplier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int VTile.WordMultiplier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int VTile.AnchorLeftMinLimit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int VTile.AnchorLeftMaxLimit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        bool VTile.IsAnchor => throw new NotImplementedException();

        Dictionary<int, int> VTile.Controlers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool VTile.FromJoker { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        bool VTile.IsEmpty => throw new NotImplementedException();

        bool? VTile.IsPlayedByPlayer1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        VTile VTile.LeftTile => throw new NotImplementedException();

        VTile VTile.RightTile => throw new NotImplementedException();

        VTile VTile.UpTile => throw new NotImplementedException();

        VTile VTile.DownTile => throw new NotImplementedException();

        VTile VTile.WordMostRightTile => throw new NotImplementedException();

        VTile VTile.WordMostLeftTile => throw new NotImplementedException();

        VTile VTile.WordLowerTile => throw new NotImplementedException();

        VTile VTile.WordUpperTile => throw new NotImplementedException();

        string VTile.Serialize => throw new NotImplementedException();

        public void OnLetterPlaced(string letter)
        {
            this.Text = letter;
            this.TileInPlay = true;

            SetRegularBackgroundColour();
        }

        public void OnLetterRemoved()
        {
            this.Text = string.Empty;
            this.TileInPlay = false;
            SetRegularBackgroundColour();
        }

        public void OnHighlight(bool valid)
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            this.BackColor = valid ? Color.LimeGreen : Color.DarkRed;
            //this.FlatAppearance.BorderSize = 5;
        }

        public void ClearHighlight()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            SetRegularBackgroundColour();
        }

        public void SetRegularBackgroundColour()
        {
            switch (this.TileType)
            {
                case TileType.Regular:
                    this.BackColor = SystemColors.ButtonFace;
                    break;
                case TileType.Center:
                    this.BackColor = Color.Purple;
                    break;
                case TileType.TripleWord:
                    this.BackColor = Color.Orange;
                    break;
                case TileType.TripleLetter:
                    this.BackColor = Color.ForestGreen;
                    break;
                case TileType.DoubleWord:
                    this.BackColor = Color.OrangeRed;
                    break;
                case TileType.DoubleLetter:
                    this.BackColor = Color.RoyalBlue;
                    break;
                default:
                    this.BackColor = SystemColors.ButtonFace;
                    break;
            }

            if (!string.IsNullOrEmpty(this.Text))
                this.BackColor = Color.Goldenrod;

            if (this.TileInPlay)
                this.BackColor = Color.Yellow;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ScrabbleTile
            // 
            this.AllowDrop = true;
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ScrabbleTile_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ScrabbleTile_DragEnter);


            this.ResumeLayout(false);

        }


        private void ScrabbleTile_DragDrop(object sender, DragEventArgs e)
        {
            this.OnLetterPlaced(e.Data.GetData(DataFormats.Text).ToString());

        }

        private void ScrabbleTile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        void ITile.ClearHighlight()
        {
            throw new NotImplementedException();
        }

        void ITile.OnHighlight(bool valid)
        {
            throw new NotImplementedException();
        }

        void VTile.Initialize()
        {
            throw new NotImplementedException();
        }

        Word VTile.GetWordFromTile(MovementDirection direction)
        {
            throw new NotImplementedException();
        }
    }
}
