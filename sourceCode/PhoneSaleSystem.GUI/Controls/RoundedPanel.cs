using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PhoneSaleSystem.GUI.Controls
{
    public class RoundedPanel : Panel
    {
        private int _borderRadius = 10;
        private Color _borderColor = Color.FromArgb(226, 232, 240); // Slate 200
        private int _borderWidth = 1;

        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        public int BorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = value; Invalidate(); }
        }

        public RoundedPanel()
        {
            this.BackColor = Color.White;
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using var path = GetRoundPath(rect, _borderRadius);

            // Tô nền
            using var brush = new SolidBrush(this.BackColor);
            g.FillPath(brush, path);

            // Vẽ viền tinh tế
            if (_borderWidth > 0 && _borderColor != Color.Transparent)
            {
                using var pen = new Pen(_borderColor, _borderWidth);
                g.DrawPath(pen, path);
            }
        }

        private static GraphicsPath GetRoundPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            float d = radius * 2;

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}

