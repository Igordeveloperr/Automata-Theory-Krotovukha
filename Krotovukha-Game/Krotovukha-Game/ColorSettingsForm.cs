using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Krotovukha_Game
{
    public partial class ColorSettingsForm : Form
    {
        private readonly Dictionary<string, Color> _availableColors = new Dictionary<string, Color>
        {
            { "Красный", Color.Red },
            { "Синий", Color.Blue },
            { "Желтый", Color.Yellow },
            { "Зеленый", Color.Green },
            { "Фиолетовый", Color.Purple },
            { "Голубой", Color.LightBlue },
            { "Розовый", Color.Pink },
            { "Коричневый", Color.Brown },
            { "Черный", Color.Black },
            { "Серебряный", Color.Silver },
            { "Золотой", Color.Gold },
            { "Ораньжевый", Color.Orange },
            { "Серый", Color.Gray },
            { "Малиновый", Color.Crimson },
            { "Белый", Color.White }
        };

        private readonly List<Color> _selectedColors = new List<Color>();
        private readonly List<Color> _draftColors;
        private readonly List<Color> _appliedColors;
        private readonly ComboBox[] _colorBoxes;

        internal List<Color> AppliedColors => _appliedColors;

        public ColorSettingsForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            _draftColors = new List<Color>
            {
                Color.Red, Color.Blue, Color.Yellow, Color.Green, Color.Purple,
                Color.LightBlue, Color.Pink, Color.Brown, Color.Black, Color.Silver
            };
            _appliedColors = new List<Color>(_draftColors);
            _colorBoxes = new[]
            {
                colorBox1, colorBox2, colorBox3, colorBox4, colorBox5,
                colorBox6, colorBox7, colorBox8, colorBox9, colorBox10
            };
            FillColorBoxes(_draftColors);
        }

        internal void LoadColors(IReadOnlyList<Color> colors)
        {
            // Mutate the existing list: the field view holds this palette by reference.
            for (int i = 0; i < _appliedColors.Count; i++)
            {
                _appliedColors[i] = colors[i];
            }
            _selectedColors.Clear();
            _draftColors.Clear();
            _draftColors.AddRange(_appliedColors);
            _selectedColors.AddRange(_appliedColors);
            SetColorEventsEnabled(false);
            try
            {
                FillColorBoxes(_appliedColors);
            }
            finally
            {
                SetColorEventsEnabled(true);
            }
        }

        private void SetColorEventsEnabled(bool enabled)
        {
            foreach (ComboBox box in _colorBoxes)
            {
                if (enabled)
                {
                    box.SelectedIndexChanged += ColorBox_SelectedIndexChanged;
                }
                else
                {
                    box.SelectedIndexChanged -= ColorBox_SelectedIndexChanged;
                }
            }
        }

        private void FillColorBoxes(IReadOnlyList<Color> colors)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                _colorBoxes[i].Text = GetColorName(colors[i]);
            }
        }

        private string GetColorName(Color color)
        {
            return _availableColors.FirstOrDefault(item => item.Value == color).Key ?? string.Empty;
        }

        private void ColorBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;
            int index = Convert.ToInt32(box.Tag);
            string previousName = GetColorName(_draftColors[index]);
            if (box.Text.Length == 0)
            {
                return;
            }

            Color color = _availableColors[box.Text];
            if (!_draftColors.Contains(color) && _selectedColors.Contains(color))
            {
                _selectedColors.Remove(color);
            }
            if (!_selectedColors.Contains(color))
            {
                _selectedColors.Add(color);
                _draftColors[index] = color;
                return;
            }

            MessageBox.Show("Такой цвет уже выбран!");
            SetColorEventsEnabled(false);
            try
            {
                box.Text = previousName;
            }
            finally
            {
                SetColorEventsEnabled(true);
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (_colorBoxes.Any(box => box.Text.Length == 0))
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }
            for (int i = 0; i < _appliedColors.Count; i++)
            {
                _appliedColors[i] = _draftColors[i];
            }
            Hide();
        }

        private void noBtn_Click(object sender, EventArgs e)
        {
            // Preserve the original event-driven cancellation behavior.
            FillColorBoxes(_appliedColors);
            Hide();
        }
    }
}
