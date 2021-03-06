﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TFive
{
    public class TFiveChatBubble_R : Control
    {

        #region Variables

        private GraphicsPath Shape;
        private Color _TextColor = Color.FromArgb(0, 100, 255);
        private Color _BubbleColor = Color.FromArgb(220, 230, 250);
        private bool _DrawBubbleArrow = true;

        #endregion
        #region Properties

        public override Color ForeColor
        {
            get => _TextColor;
            set
            {
                _TextColor = value;
                Invalidate();
            }
        }

        public Color BubbleColor
        {
            get => _BubbleColor;
            set
            {
                _BubbleColor = value;
                Invalidate();
            }
        }

        public bool DrawBubbleArrow
        {
            get => _DrawBubbleArrow;
            set
            {
                _DrawBubbleArrow = value;
                Invalidate();
            }
        }

        #endregion

        public TFiveChatBubble_R()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Size = new Size(152, 38);
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(0, 100, 255);
            Font = new Font("Segoe UI", 10);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Shape = new GraphicsPath();

            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 18, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 18, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var B = new Bitmap(Width, Height);
            var G = Graphics.FromImage(B);

            var _G = G;
            _G.SmoothingMode = SmoothingMode.HighQuality;
            _G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _G.Clear(BackColor);

            // Fill the body of the bubble with the specified color
            _G.FillPath(new SolidBrush(_BubbleColor), Shape);
            // Draw the string specified in 'Text' property
            _G.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(6, 4, Width - 15, Height));

            // Draw a polygon on the right side of the bubble
            if (_DrawBubbleArrow == true)
            {
                Point[] p = {
                    new Point(Width - 8, Height - 19),
                    new Point(Width, Height - 25),
                    new Point(Width - 8, Height - 30)
                };
                _G.FillPolygon(new SolidBrush(_BubbleColor), p);
                _G.DrawPolygon(new Pen(new SolidBrush(_BubbleColor)), p);
            }

            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(B, 0, 0);
            B.Dispose();
        }
    }
}