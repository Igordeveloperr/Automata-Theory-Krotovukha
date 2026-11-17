using System;
using System.Windows.Forms;
using Krotovukha_Game.Presentation;

namespace Krotovukha_Game
{
    public partial class AuthorForm : Form
    {
        private readonly ImageCatalog _images;

        public AuthorForm() : this(new ImageCatalog())
        {
            Disposed += (sender, args) => _images.Dispose();
        }

        internal AuthorForm(ImageCatalog images)
        {
            _images = images;
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            pictureBox1.Image = images.Get("me.png");
        }

        private void button1_Click(object sender, EventArgs e) => Close();
    }
}
