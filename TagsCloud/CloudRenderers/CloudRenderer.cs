using System;
using System.Drawing;
using TagsCloud.WordLayouters;

namespace TagsCloud.CloudRenderers
{
    public class CloudRenderer : ICloudRenderer
    {
        private readonly string path;
        private readonly IWordLayouter layouter;
        private readonly int width;
        private readonly int height;
        private readonly string filePath;

        public CloudRenderer(IWordLayouter layouter, int width, int height, string filePath)
        {
            if (width <= 0 || height <= 0) throw new ArgumentException("Not positive width or height");
            this.layouter = layouter ?? throw new ArgumentException("Layouter is null");
            this.width = width;
            this.height = height;
            this.filePath = filePath;
        }
        
        public string RenderCloud()
        {
            var wordsRectangle = layouter.GetCloudRectangle();
            var words = layouter.CloudWords;
            
            using var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.ScaleTransform((float) width / wordsRectangle.Width, (float) height / wordsRectangle.Height);
            graphics.TranslateTransform(-wordsRectangle.X, -wordsRectangle.Y);
            foreach (var word in words)
            {
                graphics.DrawString(word.Value, word.Font, new SolidBrush(word.Color), word.Rectangle.Location);
            }
            
            bitmap.Save(filePath);
            return filePath;
        }
    }
}