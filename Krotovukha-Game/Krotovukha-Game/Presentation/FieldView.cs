using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Krotovukha_Game.Domain;
using Krotovukha_Game.Math;

namespace Krotovukha_Game.Presentation
{
    internal sealed class FieldView
    {
        private readonly GameField _model;
        private readonly Panel _panel;
        private readonly IList<Color> _columnColors;
        private readonly ImageCatalog _images;
        private readonly Dictionary<GameCell, PictureBox> _cellViews = new Dictionary<GameCell, PictureBox>();
        private readonly List<PictureBox> _cells = new List<PictureBox>(GameField.CellCount);
        private readonly List<PictureBox> _highlightedCells = new List<PictureBox>();

        public IReadOnlyList<PictureBox> Cells => _cells;
        public int Percent { get; set; } = 5;

        public FieldView(Panel panel, IList<Color> columnColors, ImageCatalog images)
        {
            _panel = panel;
            _columnColors = columnColors;
            _images = images;
            _model = new GameField(new Random(Guid.NewGuid().GetHashCode()));
        }

        public void GenerateField(int moleCount)
        {
            _model.Generate(moleCount, new Random());
            RebuildViews(null, true);
        }

        public void UpdateField(IReadOnlyList<PictureBox> cells)
        {
            // Materialize before rebuilding: the editor may share images with this view.
            var snapshots = cells.Select(cell => new CellSnapshot(cell.Image, cell.Tag)).ToArray();
            _model.Load(snapshots.Select(cell => CellView.ParseTag((string)cell.Tag)));
            RebuildViews(snapshots, false);
        }

        public void UpdateField(string[] tags)
        {
            _model.Load(tags.Select(CellView.ParseTag));
            RebuildViews(tags.Select(tag => new CellSnapshot(null, tag)).ToArray(), false);
        }

        public bool ExecuteLogic(Panel lamp, Timer timer)
        {
            foreach (PictureBox cell in _highlightedCells)
            {
                cell.BackColor = Color.Empty;
            }
            _highlightedCells.Clear();

            if (_model.MoleCount == 0)
            {
                timer.Stop();
                MessageBox.Show("Игра окончена!");
                return true;
            }

            // Zero speed in the original application does not advance the field.
            if (!timer.Enabled)
            {
                return false;
            }

            Render(_model.PlantCabbage());
            bool lampIsOn = new ProbabilityTrial(Percent).Randomize();
            lamp.BackgroundImage = _images.Get(lampIsOn ? "onLamp.png" : "offLamp.png");
            if (lampIsOn)
            {
                int column = _model.ChooseColumn();
                foreach (GameCell cell in _model.ClearColumn(column))
                {
                    PictureBox view = _cellViews[cell];
                    view.BackColor = _columnColors[column];
                    _highlightedCells.Add(view);
                    Render(cell);
                }
            }
            return false;
        }

        public void RestoreInitialState()
        {
            _model.RestoreInitialState();
            foreach (GameCell cell in _model.Cells)
            {
                Render(cell);
            }
        }

        private void RebuildViews(CellSnapshot[] snapshots, bool assignNames)
        {
            CellView.ClearControls(_panel);
            _cells.Clear();
            _cellViews.Clear();
            // Previously highlighted controls can outlive a replacement until the next tick.
            // They have no visible effect once removed from the panel.
            _highlightedCells.Clear();

            for (int index = 0; index < _model.Cells.Count; index++)
            {
                GameCell cell = _model.Cells[index];
                PictureBox view = CellView.Create();
                view.Image = snapshots?[index].Image ?? _images.ForCell(cell.Kind);
                view.Tag = snapshots == null ? CellView.ToTag(cell.Kind) : snapshots[index].Tag;
                if (assignNames)
                {
                    // Generated names follow creation order, before the field is shuffled.
                    view.Name = "cell" + cell.CreationIndex;
                }
                _cellViews.Add(cell, view);
                _cells.Add(view);
                _panel.Controls.Add(view);
            }
        }

        private void Render(GameCell cell)
        {
            // Tags retain the configured starting state, as in the original application.
            _cellViews[cell].Image = _images.ForCell(cell.Kind);
        }

        private sealed class CellSnapshot
        {
            public Image Image { get; }
            public object Tag { get; }

            public CellSnapshot(Image image, object tag)
            {
                Image = image;
                Tag = tag;
            }
        }
    }
}
