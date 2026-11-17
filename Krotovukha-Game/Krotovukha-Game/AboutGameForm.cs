using System;
using System.Windows.Forms;
using Krotovukha_Game.Presentation;

namespace Krotovukha_Game
{
    public partial class AboutGameForm : Form
    {
        public AboutGameForm() : this(new ImageCatalog())
        {
            Disposed += (sender, args) => _images.Dispose();
        }

        private readonly ImageCatalog _images;

        internal AboutGameForm(ImageCatalog images)
        {
            _images = images;
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            logo.Image = images.Get("logo.png");
            leftImg.Image = images.Get("KROTLOGO.png");
            rightImg.Image = images.Get("krot-kopija.png");
        }

        private void button1_Click(object sender, EventArgs e) => Close();
    }
}
