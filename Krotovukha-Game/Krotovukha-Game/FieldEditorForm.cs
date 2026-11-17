using System;
using System.Collections.Generic;
using System.Media;
using System.Windows.Forms;
using Krotovukha_Game.Domain;
using Krotovukha_Game.Presentation;

namespace Krotovukha_Game
{
    public partial class FieldEditorForm : Form
    {
        private readonly IReadOnlyList<PictureBox> _sourceCells;
        private readonly List<PictureBox> _cells = new List<PictureBox>(GameField.CellCount);
        private readonly List<PictureBox> _moleCells = new List<PictureBox>(GameField.CellCount);
        private readonly List<PictureBox> _cabbageCells = new List<PictureBox>(GameField.CellCount);
        private readonly FieldView _field;
        private readonly ImageCatalog _images;
        private int _moleCount = 1;

        internal FieldEditorForm(IReadOnlyList<PictureBox> cells, FieldView field, ImageCatalog images)
        {
            _sourceCells = cells;
            _field = field;
            _images = images;
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            FillField(cells);
        }

        internal void FillField(IReadOnlyList<PictureBox> cells)
        {
            ClearField();
            foreach (PictureBox source in cells)
            {
                PictureBox cell = CellView.Create();
                cell.Image = source.Image;
                cell.Tag = source.Tag;
                AddCell(cell);
                panel.Controls.Add(cell);
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            var cell = (PictureBox)sender;
            if (_cabbageCells.Contains(cell))
            {
                _cabbageCells.Remove(cell);
                SetKind(cell, CellKind.Mole);
                _moleCells.Add(cell);
            }
            else if (_moleCells.Count > 1 && _moleCells.Contains(cell))
            {
                _moleCells.Remove(cell);
                SetKind(cell, CellKind.Cabbage);
                _cabbageCells.Add(cell);
            }
            else
            {
                MessageBox.Show("Больше нельзя убирать кротов!");
            }
        }

        private void GenerateField(int moleCount)
        {
            ClearField();
            var random = new Random();
            for (int i = 0; i < GameField.CellCount; i++)
            {
                PictureBox cell = CellView.Create();
                cell.Name = "cell" + i;
                SetKind(cell, i < moleCount ? CellKind.Mole : CellKind.Cabbage);
                AddCell(cell);
            }
            for (int i = 0; i < _cells.Count; i++)
            {
                int other = random.Next(GameField.CellCount);
                PictureBox cell = _cells[i];
                _cells[i] = _cells[other];
                _cells[other] = cell;
            }
            foreach (PictureBox cell in _cells)
            {
                panel.Controls.Add(cell);
            }
        }

        private void SetKind(PictureBox cell, CellKind kind)
        {
            cell.Tag = CellView.ToTag(kind);
            cell.Image = _images.ForCell(kind);
        }

        private void AddCell(PictureBox cell)
        {
            cell.Click += Cell_Click;
            _cells.Add(cell);
            ((string)cell.Tag == CellView.MoleTag ? _moleCells : _cabbageCells).Add(cell);
        }

        private void ClearField()
        {
            CellView.ClearControls(panel);
            _cells.Clear();
            _moleCells.Clear();
            _cabbageCells.Clear();
        }

        private void speedValue_TextChanged(object sender, EventArgs e)
        {
            if (NumericInput.IsDigits(textbox.Text))
            {
                int value = int.Parse(textbox.Text);
                if (value >= 1 && value < GameField.CellCount)
                {
                    _moleCount = value;
                    return;
                }
            }
            else if (textbox.Text.Length == 0)
            {
                SystemSounds.Beep.Play();
                return;
            }

            textbox.Text = _moleCount.ToString();
            SystemSounds.Beep.Play();
        }

        private void genBtn_Click(object sender, EventArgs e) => GenerateField(_moleCount);

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Hide();
            FillField(_sourceCells);
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            Hide();
            _field.UpdateField(_cells);
        }
    }
}
