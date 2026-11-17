using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Krotovukha_Game.Domain;

namespace Krotovukha_Game.Presentation
{
    /// <summary>Owns shared images for the lifetime of the UI thread.</summary>
    internal sealed class ImageCatalog : IDisposable
    {
        private readonly Dictionary<string, Image> _images = new Dictionary<string, Image>();

        public Image Get(string fileName)
        {
            Image image;
            if (!_images.TryGetValue(fileName, out image))
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", fileName);
                // Detach the bitmap from the file so the application does not keep it locked.
                using (Image source = Image.FromFile(path))
                {
                    image = new Bitmap(source);
                }
                _images.Add(fileName, image);
            }
            return image;
        }

        public Image ForCell(CellKind kind) => Get(kind == CellKind.Mole ? "krot.png" : "kacan.png");

        public void Dispose()
        {
            foreach (Image image in _images.Values)
            {
                image.Dispose();
            }
            _images.Clear();
        }
    }
}
