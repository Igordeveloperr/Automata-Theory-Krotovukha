using System;
using System.Windows.Forms;
using Krotovukha_Game.Math;
using Krotovukha_Game.Presentation;
using Krotovukha_Game.Settings;

namespace Krotovukha_Game
{
    public partial class MainWindow : Form
    {
        private readonly ImageCatalog _images;
        private readonly ColorSettingsForm _colorSettings;
        private readonly FieldView _field;
        private readonly FieldEditorForm _fieldEditor;
        private bool _isStartButton = true;
        private bool _isPauseButton = true;
        private bool _simulationIsInactive = true;
        private bool _isZeroSpeed;

        public MainWindow() : this(new ImageCatalog())
        {
            Disposed += (sender, args) => _images.Dispose();
        }

        internal MainWindow(ImageCatalog images)
        {
            _images = images;
            _colorSettings = new ColorSettingsForm();
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            _field = new FieldView(basePanel, _colorSettings.AppliedColors, _images);
            _field.GenerateField(50);
            lamp.BackgroundImage = _images.Get("offLamp.png");
            _fieldEditor = new FieldEditorForm(_field.Cells, _field, _images);
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!_isStartButton)
            {
                StopSimulation();
                setMenuItem.Enabled = true;
                fileMenuItem.Enabled = true;
                return;
            }

            _simulationIsInactive = false;
            ButtonAppearance.ShowStop(startBtn);
            _isStartButton = false;

            if (_isZeroSpeed)
            {
                // Preserve zero-speed behavior, including the enabled menus.
                mainTimer_Tick(sender, e);
                return;
            }

            setMenuItem.Enabled = false;
            fileMenuItem.Enabled = false;
            if (_isPauseButton)
            {
                mainTimer.Start();
            }
        }

        private void StopSimulation()
        {
            lamp.BackgroundImage = _images.Get("offLamp.png");
            mainTimer.Stop();
            _simulationIsInactive = true;
            _field.RestoreInitialState();
            _isStartButton = true;
            ButtonAppearance.ShowStart(startBtn);
            _isPauseButton = true;
            ButtonAppearance.ShowPause(pauseBtn);
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            _simulationIsInactive = _field.ExecuteLogic(lamp, mainTimer);
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            if (_isPauseButton)
            {
                _isPauseButton = false;
                ButtonAppearance.ShowContinue(pauseBtn);
                mainTimer.Stop();
                return;
            }

            _isPauseButton = true;
            ButtonAppearance.ShowPause(pauseBtn);
            if (!_isZeroSpeed && !_simulationIsInactive)
            {
                mainTimer.Start();
            }
        }

        private void probTrack_Scroll(object sender, EventArgs e)
        {
            probValue.Text = probTrack.Value.ToString();
            _field.Percent = probTrack.Value;
        }

        private void speedTrack_Scroll(object sender, EventArgs e)
        {
            int interval = SimulationSpeed.ConvertToMilliseconds(speedTrack.Value);
            speedValue.Text = speedTrack.Value.ToString();

            if (interval == 0)
            {
                _isZeroSpeed = true;
                mainTimer.Stop();
                return;
            }

            if (_isZeroSpeed && !_simulationIsInactive && _isPauseButton)
            {
                mainTimer.Start();
            }
            _isZeroSpeed = false;
            mainTimer.Interval = interval;
        }

        private void speedValue_TextChanged(object sender, EventArgs e)
        {
            NumericInput.Synchronize(speedValue, speedTrack, () => speedTrack_Scroll(sender, e));
        }

        private void probValue_TextChanged(object sender, EventArgs e)
        {
            NumericInput.Synchronize(probValue, probTrack, () => probTrack_Scroll(sender, e));
        }

        private void colorMenuItem_Click(object sender, EventArgs e)
        {
            if (!_colorSettings.Visible)
            {
                _colorSettings.Show();
            }
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            var settings = new GameSettings
            {
                Speed = int.Parse(speedValue.Text),
                Probability = int.Parse(probValue.Text)
            };
            settings.SetColors(_colorSettings.AppliedColors);
            for (int i = 0; i < _field.Cells.Count; i++)
            {
                settings.Field[i] = (string)_field.Cells[i].Tag;
            }
            SettingsDialogs.Save(settings);
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            GameSettings settings = SettingsDialogs.Open();
            if (settings == null)
            {
                return;
            }

            ApplySettings(settings);
        }

        private void ApplySettings(GameSettings settings)
        {
            speedValue.Text = settings.Speed.ToString();
            probValue.Text = settings.Probability.ToString();
            _colorSettings.LoadColors(settings.GetColors());
            _field.UpdateField(settings.Field);
            _fieldEditor.FillField(_field.Cells);
        }

        private void progInfoMenuItem_Click(object sender, EventArgs e)
        {
            new AboutGameForm(_images).Show();
        }

        private void authorInfoMenuItem_Click(object sender, EventArgs e)
        {
            new AuthorForm(_images).Show();
        }

        private void fillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_fieldEditor.Visible)
            {
                _fieldEditor.Show();
            }
        }

        private void exitBtn_Click(object sender, EventArgs e) => Close();
        private void exitMenuItem_Click(object sender, EventArgs e) => Close();

        private void DisposeManagedResources()
        {
            _fieldEditor?.Dispose();
            _colorSettings?.Dispose();
        }
    }
}
