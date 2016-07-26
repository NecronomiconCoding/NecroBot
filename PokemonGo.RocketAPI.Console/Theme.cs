using System;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace PokemonGo.RocketAPI.Console
{
  

    // Xylos Theme.
    // Made by AeroRev9.
    // 9 Controls.

    internal sealed class Helpers
    {
        public enum RoundingStyle : byte
        {
            All,
            Top,
            Bottom,
            Left,
            Right,
            TopRight,
            BottomRight
        }

        public static void CenterString(Graphics G, string T, Font F, Color C, Rectangle R)
        {
            SizeF sizeF = G.MeasureString(T, F);
            using (SolidBrush solidBrush = new SolidBrush(C))
            {
                G.DrawString(T, F, solidBrush, checked(new Point((int)Math.Round(unchecked((double)R.Width / 2.0 - (double)(sizeF.Width / 2f))), (int)Math.Round(unchecked((double)R.Height / 2.0 - (double)(sizeF.Height / 2f))))));
            }
        }

        public static Color ColorFromHex(string Hex)
        {
            return Color.FromArgb(checked((int)long.Parse(string.Format("FFFFFFFFFF{0}", Hex.Substring(1)), NumberStyles.HexNumber)));
        }

        public static Rectangle FullRectangle(Size S, bool Subtract)
        {
            Rectangle result;
            if (Subtract)
            {
                result = checked(new Rectangle(0, 0, S.Width - 1, S.Height - 1));
            }
            else
            {
                result = new Rectangle(0, 0, S.Width, S.Height);
            }
            return result;
        }

        public static GraphicsPath RoundRect(Rectangle Rect, int Rounding, Helpers.RoundingStyle Style = Helpers.RoundingStyle.All)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            checked
            {
                int num = Rounding * 2;
                graphicsPath.StartFigure();
                bool flag = Rounding == 0;
                GraphicsPath result;
                if (flag)
                {
                    graphicsPath.AddRectangle(Rect);
                    graphicsPath.CloseAllFigures();
                    result = graphicsPath;
                }
                else
                {
                    switch (Style)
                    {
                        case Helpers.RoundingStyle.All:
                            graphicsPath.AddArc(new Rectangle(Rect.X, Rect.Y, num, num), -180f, 90f);
                            graphicsPath.AddArc(new Rectangle(Rect.Width - num + Rect.X, Rect.Y, num, num), -90f, 90f);
                            graphicsPath.AddArc(new Rectangle(Rect.Width - num + Rect.X, Rect.Height - num + Rect.Y, num, num), 0f, 90f);
                            graphicsPath.AddArc(new Rectangle(Rect.X, Rect.Height - num + Rect.Y, num, num), 90f, 90f);
                            break;
                        case Helpers.RoundingStyle.Top:
                            graphicsPath.AddArc(new Rectangle(Rect.X, Rect.Y, num, num), -180f, 90f);
                            graphicsPath.AddArc(new Rectangle(Rect.Width - num + Rect.X, Rect.Y, num, num), -90f, 90f);
                            graphicsPath.AddLine(new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y + Rect.Height));
                            break;
                        case Helpers.RoundingStyle.Bottom:
                            graphicsPath.AddLine(new Point(Rect.X, Rect.Y), new Point(Rect.X + Rect.Width, Rect.Y));
                            graphicsPath.AddArc(new Rectangle(Rect.Width - num + Rect.X, Rect.Height - num + Rect.Y, num, num), 0f, 90f);
                            graphicsPath.AddArc(new Rectangle(Rect.X, Rect.Height - num + Rect.Y, num, num), 90f, 90f);
                            break;
                        case Helpers.RoundingStyle.Left:
                            graphicsPath.AddArc(new Rectangle(Rect.X, Rect.Y, num, num), -180f, 90f);
                            graphicsPath.AddLine(new Point(Rect.X + Rect.Width, Rect.Y), new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height));
                            graphicsPath.AddArc(new Rectangle(Rect.X, Rect.Height - num + Rect.Y, num, num), 90f, 90f);
                            break;
                        case Helpers.RoundingStyle.Right:
                            graphicsPath.AddLine(new Point(Rect.X, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y));
                            graphicsPath.AddArc(new Rectangle(Rect.Width - num + Rect.X, Rect.Y, num, num), -90f, 90f);
                            graphicsPath.AddArc(new Rectangle(Rect.Width - num + Rect.X, Rect.Height - num + Rect.Y, num, num), 0f, 90f);
                            break;
                        case Helpers.RoundingStyle.TopRight:
                            graphicsPath.AddLine(new Point(Rect.X, Rect.Y + 1), new Point(Rect.X, Rect.Y));
                            graphicsPath.AddArc(new Rectangle(Rect.Width - num + Rect.X, Rect.Y, num, num), -90f, 90f);
                            graphicsPath.AddLine(new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height - 1), new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height));
                            graphicsPath.AddLine(new Point(Rect.X + 1, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y + Rect.Height));
                            break;
                        case Helpers.RoundingStyle.BottomRight:
                            graphicsPath.AddLine(new Point(Rect.X, Rect.Y + 1), new Point(Rect.X, Rect.Y));
                            graphicsPath.AddLine(new Point(Rect.X + Rect.Width - 1, Rect.Y), new Point(Rect.X + Rect.Width, Rect.Y));
                            graphicsPath.AddArc(new Rectangle(Rect.Width - num + Rect.X, Rect.Height - num + Rect.Y, num, num), 0f, 90f);
                            graphicsPath.AddLine(new Point(Rect.X + 1, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y + Rect.Height));
                            break;
                    }
                    graphicsPath.CloseAllFigures();
                    result = graphicsPath;
                }
                return result;
            }
        }
    }
    public class XylosTabControl : TabControl
    {
        private Graphics G;

        private Rectangle Rect;

        private int _OverIndex;

        private bool _FirstHeaderBorder;

        public bool FirstHeaderBorder
        {
            get;
            set;
        }

        private int OverIndex
        {
            get
            {
                return this._OverIndex;
            }
            set
            {
                this._OverIndex = value;
                base.Invalidate();
            }
        }

        public XylosTabControl()
        {
            this._OverIndex = -1;
            this.DoubleBuffered = true;
            base.Alignment = TabAlignment.Left;
            base.SizeMode = TabSizeMode.Fixed;
            base.ItemSize = new Size(40, 180);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            base.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control.BackColor = Color.White;
            e.Control.ForeColor = Helpers.ColorFromHex("#7C858E");
            e.Control.Font = new Font("Segoe UI", 9f);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.G = e.Graphics;
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
            this.G.Clear(Helpers.ColorFromHex("#33373B"));
            checked
            {
                int num = base.TabPages.Count - 1;
                for (int i = 0; i <= num; i++)
                {
                    this.Rect = base.GetTabRect(i);
                    bool flag = string.IsNullOrEmpty(Conversions.ToString(base.TabPages[i].Tag));
                    if (flag)
                    {
                        bool flag2 = base.SelectedIndex == i;
                        if (flag2)
                        {
                            using (SolidBrush solidBrush = new SolidBrush(Helpers.ColorFromHex("#2B2F33")))
                            {
                                using (SolidBrush solidBrush2 = new SolidBrush(Helpers.ColorFromHex("#BECCD9")))
                                {
                                    using (Font font = new Font("Segoe UI semibold", 9f))
                                    {
                                        this.G.FillRectangle(solidBrush, new Rectangle(this.Rect.X - 5, this.Rect.Y + 1, this.Rect.Width + 7, this.Rect.Height));
                                        this.G.DrawString(base.TabPages[i].Text, font, solidBrush2, new Point(this.Rect.X + 50 + (base.ItemSize.Height - 180), this.Rect.Y + 12));
                                    }
                                }
                            }
                        }
                        else
                        {
                            using (SolidBrush solidBrush3 = new SolidBrush(Helpers.ColorFromHex("#919BA6")))
                            {
                                using (Font font2 = new Font("Segoe UI semibold", 9f))
                                {
                                    this.G.DrawString(base.TabPages[i].Text, font2, solidBrush3, new Point(this.Rect.X + 50 + (base.ItemSize.Height - 180), this.Rect.Y + 12));
                                }
                            }
                        }
                        bool flag3 = this.OverIndex != -1 & base.SelectedIndex != this.OverIndex;
                        if (flag3)
                        {
                            using (SolidBrush solidBrush4 = new SolidBrush(Helpers.ColorFromHex("#2F3338")))
                            {
                                using (SolidBrush solidBrush5 = new SolidBrush(Helpers.ColorFromHex("#919BA6")))
                                {
                                    using (Font font3 = new Font("Segoe UI semibold", 9f))
                                    {
                                        this.G.FillRectangle(solidBrush4, new Rectangle(base.GetTabRect(this.OverIndex).X - 5, base.GetTabRect(this.OverIndex).Y + 1, base.GetTabRect(this.OverIndex).Width + 7, base.GetTabRect(this.OverIndex).Height));
                                        this.G.DrawString(base.TabPages[this.OverIndex].Text, font3, solidBrush5, new Point(base.GetTabRect(this.OverIndex).X + 50 + (base.ItemSize.Height - 180), base.GetTabRect(this.OverIndex).Y + 12));
                                    }
                                }
                            }
                            bool flag4 = !Information.IsNothing(base.ImageList);
                            if (flag4)
                            {
                                bool flag5 = base.TabPages[this.OverIndex].ImageIndex >= 0;
                                if (flag5)
                                {
                                    this.G.DrawImage(base.ImageList.Images[base.TabPages[this.OverIndex].ImageIndex], new Rectangle(base.GetTabRect(this.OverIndex).X + 25 + (base.ItemSize.Height - 180), (int)Math.Round(unchecked((double)base.GetTabRect(this.OverIndex).Y + ((double)base.GetTabRect(this.OverIndex).Height / 2.0 - 9.0))), 16, 16));
                                }
                            }
                        }
                        bool flag6 = !Information.IsNothing(base.ImageList);
                        if (flag6)
                        {
                            bool flag7 = base.TabPages[i].ImageIndex >= 0;
                            if (flag7)
                            {
                                this.G.DrawImage(base.ImageList.Images[base.TabPages[i].ImageIndex], new Rectangle(this.Rect.X + 25 + (base.ItemSize.Height - 180), (int)Math.Round(unchecked((double)this.Rect.Y + ((double)this.Rect.Height / 2.0 - 9.0))), 16, 16));
                            }
                        }
                    }
                    else
                    {
                        using (SolidBrush solidBrush6 = new SolidBrush(Helpers.ColorFromHex("#6A7279")))
                        {
                            using (Font font4 = new Font("Segoe UI", 7f, FontStyle.Bold))
                            {
                                using (Pen pen = new Pen(Helpers.ColorFromHex("#2B2F33")))
                                {
                                    bool firstHeaderBorder = this.FirstHeaderBorder;
                                    if (firstHeaderBorder)
                                    {
                                        this.G.DrawLine(pen, new Point(this.Rect.X - 5, this.Rect.Y + 1), new Point(this.Rect.Width + 7, this.Rect.Y + 1));
                                    }
                                    else
                                    {
                                        bool flag8 = i != 0;
                                        if (flag8)
                                        {
                                            this.G.DrawLine(pen, new Point(this.Rect.X - 5, this.Rect.Y + 1), new Point(this.Rect.Width + 7, this.Rect.Y + 1));
                                        }
                                    }
                                    this.G.DrawString(base.TabPages[i].Text.ToUpper(), font4, solidBrush6, new Point(this.Rect.X + 25 + (base.ItemSize.Height - 180), this.Rect.Y + 16));
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnSelecting(TabControlCancelEventArgs e)
        {
            base.OnSelecting(e);
            bool flag = !Information.IsNothing(e.TabPage);
            if (flag)
            {
                bool flag2 = !string.IsNullOrEmpty(Conversions.ToString(e.TabPage.Tag));
                if (flag2)
                {
                    e.Cancel = true;
                }
                else
                {
                    this.OverIndex = -1;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            checked
            {
                int num = base.TabPages.Count - 1;
                for (int i = 0; i <= num; i++)
                {
                    bool flag = base.GetTabRect(i).Contains(e.Location) & base.SelectedIndex != i & string.IsNullOrEmpty(Conversions.ToString(base.TabPages[i].Tag));
                    if (flag)
                    {
                        this.OverIndex = i;
                        break;
                    }
                    this.OverIndex = -1;
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.OverIndex = -1;
        }
    }
    [DefaultEvent("TextChanged")]
    public class XylosTextBox : Control
    {
        public enum MouseState : byte
        {
            None,
            Over,
            Down
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never), AccessedThroughProperty("TB"), CompilerGenerated]
        private TextBox _TB;

        private Graphics G;

        private XylosTextBox.MouseState State;

        private bool IsDown;

        private bool _EnabledCalc;

        private bool _allowpassword;

        private int _maxChars;

        private HorizontalAlignment _textAlignment;

        private bool _multiLine;

        private bool _readOnly;

        public virtual TextBox TB
        {
            [CompilerGenerated]
            get
            {
                return this._TB;
            }
            [CompilerGenerated]
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler value2 = delegate (object a0, EventArgs a1)
                {
                    this.TextChangeTb();
                };
                TextBox tB = this._TB;
                if (tB != null)
                {
                    tB.TextChanged -= value2;
                }
                this._TB = value;
                tB = this._TB;
                if (tB != null)
                {
                    tB.TextChanged += value2;
                }
            }
        }

        public new bool Enabled
        {
            get
            {
                return this.EnabledCalc;
            }
            set
            {
                this.TB.Enabled = value;
                this._EnabledCalc = value;
                base.Invalidate();
            }
        }

        [DisplayName("Enabled")]
        public bool EnabledCalc
        {
            get
            {
                return this._EnabledCalc;
            }
            set
            {
                this.Enabled = value;
                base.Invalidate();
            }
        }

        public bool UseSystemPasswordChar
        {
            get
            {
                return this._allowpassword;
            }
            set
            {
                this.TB.UseSystemPasswordChar = this.UseSystemPasswordChar;
                this._allowpassword = value;
                base.Invalidate();
            }
        }

        public int MaxLength
        {
            get
            {
                return this._maxChars;
            }
            set
            {
                this._maxChars = value;
                this.TB.MaxLength = this.MaxLength;
                base.Invalidate();
            }
        }

        public HorizontalAlignment TextAlign
        {
            get
            {
                return this._textAlignment;
            }
            set
            {
                this._textAlignment = value;
                base.Invalidate();
            }
        }

        public bool MultiLine
        {
            get
            {
                return this._multiLine;
            }
            set
            {
                this._multiLine = value;
                this.TB.Multiline = value;
                this.OnResize(EventArgs.Empty);
                base.Invalidate();
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this._readOnly;
            }
            set
            {
                this._readOnly = value;
                bool flag = this.TB != null;
                if (flag)
                {
                    this.TB.ReadOnly = value;
                }
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            base.Invalidate();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            base.Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            this.TB.ForeColor = this.ForeColor;
            base.Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.TB.Font = this.Font;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.TB.Focus();
        }

        private void TextChangeTb()
        {
            this.Text = this.TB.Text;
        }

        private void TextChng()
        {
            this.TB.Text = this.Text;
        }

        public void NewTextBox()
        {
            TextBox tB = this.TB;
            tB.Text = string.Empty;
            tB.BackColor = Color.White;
            tB.ForeColor = Helpers.ColorFromHex("#7C858E");
            tB.TextAlign = HorizontalAlignment.Left;
            tB.BorderStyle = BorderStyle.None;
            tB.Location = new Point(3, 3);
            tB.Font = new Font("Segoe UI", 9f);
            tB.Size = checked(new Size(base.Width - 3, base.Height - 3));
            tB.UseSystemPasswordChar = this.UseSystemPasswordChar;
        }

        public XylosTextBox()
        {
            base.TextChanged += delegate (object a0, EventArgs a1)
            {
                this.TextChng();
            };
            this.TB = new TextBox();
            this._allowpassword = false;
            this._maxChars = 32767;
            this._multiLine = false;
            this._readOnly = false;
            this.NewTextBox();
            base.Controls.Add(this.TB);
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            this.TextAlign = HorizontalAlignment.Left;
            this.ForeColor = Helpers.ColorFromHex("#7C858E");
            this.Font = new Font("Segoe UI", 9f);
            base.Size = new Size(130, 29);
            this.Enabled = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.G = e.Graphics;
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
            this.G.Clear(Color.White);
            bool enabled = this.Enabled;
            if (enabled)
            {
                this.TB.ForeColor = Helpers.ColorFromHex("#7C858E");
                bool flag = this.State == XylosTextBox.MouseState.Down;
                if (flag)
                {
                    using (Pen pen = new Pen(Helpers.ColorFromHex("#78B7E6")))
                    {
                        this.G.DrawPath(pen, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 12, Helpers.RoundingStyle.All));
                    }
                }
                else
                {
                    using (Pen pen2 = new Pen(Helpers.ColorFromHex("#D0D5D9")))
                    {
                        this.G.DrawPath(pen2, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 12, Helpers.RoundingStyle.All));
                    }
                }
            }
            else
            {
                this.TB.ForeColor = Helpers.ColorFromHex("#7C858E");
                using (Pen pen3 = new Pen(Helpers.ColorFromHex("#E1E1E2")))
                {
                    this.G.DrawPath(pen3, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 12, Helpers.RoundingStyle.All));
                }
            }
            this.TB.TextAlign = this.TextAlign;
            this.TB.UseSystemPasswordChar = this.UseSystemPasswordChar;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            bool flag = !this.MultiLine;
            checked
            {
                if (flag)
                {
                    int height = this.TB.Height;
                    this.TB.Location = new Point(10, (int)Math.Round(unchecked((double)base.Height / 2.0 - (double)height / 2.0 - 0.0)));
                    this.TB.Size = new Size(base.Width - 20, height);
                }
                else
                {
                    this.TB.Location = new Point(10, 10);
                    this.TB.Size = new Size(base.Width - 20, base.Height - 20);
                }
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.State = XylosTextBox.MouseState.Down;
            base.Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.State = XylosTextBox.MouseState.None;
            base.Invalidate();
        }
    }
    public class XylosButton : Control
    {
        public enum MouseState : byte
        {
            None,
            Over,
            Down
        }

        public delegate void ClickEventHandler(object sender, EventArgs e);

        private Graphics G;

        private XylosButton.MouseState State;

        private bool _EnabledCalc;

        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private XylosButton.ClickEventHandler ClickEvent;

        public new event XylosButton.ClickEventHandler Click
        {
            [CompilerGenerated]
            add
            {
                XylosButton.ClickEventHandler clickEventHandler = this.ClickEvent;
                XylosButton.ClickEventHandler clickEventHandler2;
                do
                {
                    clickEventHandler2 = clickEventHandler;
                    XylosButton.ClickEventHandler value2 = (XylosButton.ClickEventHandler)Delegate.Combine(clickEventHandler2, value);
                    clickEventHandler = Interlocked.CompareExchange<XylosButton.ClickEventHandler>(ref this.ClickEvent, value2, clickEventHandler2);
                }
                while (clickEventHandler != clickEventHandler2);
            }
            [CompilerGenerated]
            remove
            {
                XylosButton.ClickEventHandler clickEventHandler = this.ClickEvent;
                XylosButton.ClickEventHandler clickEventHandler2;
                do
                {
                    clickEventHandler2 = clickEventHandler;
                    XylosButton.ClickEventHandler value2 = (XylosButton.ClickEventHandler)Delegate.Remove(clickEventHandler2, value);
                    clickEventHandler = Interlocked.CompareExchange<XylosButton.ClickEventHandler>(ref this.ClickEvent, value2, clickEventHandler2);
                }
                while (clickEventHandler != clickEventHandler2);
            }
        }

        public new bool Enabled
        {
            get
            {
                return this.EnabledCalc;
            }
            set
            {
                this._EnabledCalc = value;
                base.Invalidate();
            }
        }

        [DisplayName("Enabled")]
        public bool EnabledCalc
        {
            get
            {
                return this._EnabledCalc;
            }
            set
            {
                this.Enabled = value;
                base.Invalidate();
            }
        }

        public XylosButton()
        {
            this.DoubleBuffered = true;
            this.Enabled = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.G = e.Graphics;
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
            bool enabled = this.Enabled;
            if (enabled)
            {
                XylosButton.MouseState state = this.State;
                if (state != XylosButton.MouseState.Over)
                {
                    if (state != XylosButton.MouseState.Down)
                    {
                        using (SolidBrush solidBrush = new SolidBrush(Helpers.ColorFromHex("#F6F6F6")))
                        {
                            this.G.FillPath(solidBrush, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 3, Helpers.RoundingStyle.All));
                        }
                    }
                    else
                    {
                        using (SolidBrush solidBrush2 = new SolidBrush(Helpers.ColorFromHex("#F0F0F0")))
                        {
                            this.G.FillPath(solidBrush2, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 3, Helpers.RoundingStyle.All));
                        }
                    }
                }
                else
                {
                    using (SolidBrush solidBrush3 = new SolidBrush(Helpers.ColorFromHex("#FDFDFD")))
                    {
                        this.G.FillPath(solidBrush3, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 3, Helpers.RoundingStyle.All));
                    }
                }
                using (Font font = new Font("Segoe UI", 9f))
                {
                    using (Pen pen = new Pen(Helpers.ColorFromHex("#C3C3C3")))
                    {
                        this.G.DrawPath(pen, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 3, Helpers.RoundingStyle.All));
                        Helpers.CenterString(this.G, this.Text, font, Helpers.ColorFromHex("#7C858E"), Helpers.FullRectangle(base.Size, false));
                    }
                }
            }
            else
            {
                using (SolidBrush solidBrush4 = new SolidBrush(Helpers.ColorFromHex("#F3F4F7")))
                {
                    using (Pen pen2 = new Pen(Helpers.ColorFromHex("#DCDCDC")))
                    {
                        using (Font font2 = new Font("Segoe UI", 9f))
                        {
                            this.G.FillPath(solidBrush4, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 3, Helpers.RoundingStyle.All));
                            this.G.DrawPath(pen2, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 3, Helpers.RoundingStyle.All));
                            Helpers.CenterString(this.G, this.Text, font2, Helpers.ColorFromHex("#D0D3D7"), Helpers.FullRectangle(base.Size, false));
                        }
                    }
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.State = XylosButton.MouseState.Over;
            base.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.State = XylosButton.MouseState.None;
            base.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            bool enabled = this.Enabled;
            if (enabled)
            {
                XylosButton.ClickEventHandler clickEvent = this.ClickEvent;
                if (clickEvent != null)
                {
                    clickEvent(this, e);
                }
            }
            this.State = XylosButton.MouseState.Over;
            base.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.State = XylosButton.MouseState.Down;
            base.Invalidate();
        }
    }
    [DefaultEvent("CheckedChanged")]
    public class XylosCheckBox : Control
    {
        public delegate void CheckedChangedEventHandler(object sender, EventArgs e);

        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private XylosCheckBox.CheckedChangedEventHandler CheckedChangedEvent;

        private bool _Checked;

        private bool _EnabledCalc;

        private Graphics G;

        private string B64Enabled;

        private string B64Disabled;

        public event XylosCheckBox.CheckedChangedEventHandler CheckedChanged
        {
            [CompilerGenerated]
            add
            {
                XylosCheckBox.CheckedChangedEventHandler checkedChangedEventHandler = this.CheckedChangedEvent;
                XylosCheckBox.CheckedChangedEventHandler checkedChangedEventHandler2;
                do
                {
                    checkedChangedEventHandler2 = checkedChangedEventHandler;
                    XylosCheckBox.CheckedChangedEventHandler value2 = (XylosCheckBox.CheckedChangedEventHandler)Delegate.Combine(checkedChangedEventHandler2, value);
                    checkedChangedEventHandler = Interlocked.CompareExchange<XylosCheckBox.CheckedChangedEventHandler>(ref this.CheckedChangedEvent, value2, checkedChangedEventHandler2);
                }
                while (checkedChangedEventHandler != checkedChangedEventHandler2);
            }
            [CompilerGenerated]
            remove
            {
                XylosCheckBox.CheckedChangedEventHandler checkedChangedEventHandler = this.CheckedChangedEvent;
                XylosCheckBox.CheckedChangedEventHandler checkedChangedEventHandler2;
                do
                {
                    checkedChangedEventHandler2 = checkedChangedEventHandler;
                    XylosCheckBox.CheckedChangedEventHandler value2 = (XylosCheckBox.CheckedChangedEventHandler)Delegate.Remove(checkedChangedEventHandler2, value);
                    checkedChangedEventHandler = Interlocked.CompareExchange<XylosCheckBox.CheckedChangedEventHandler>(ref this.CheckedChangedEvent, value2, checkedChangedEventHandler2);
                }
                while (checkedChangedEventHandler != checkedChangedEventHandler2);
            }
        }

        public bool Checked
        {
            get
            {
                return this._Checked;
            }
            set
            {
                this._Checked = value;
                base.Invalidate();
            }
        }

        public new bool Enabled
        {
            get
            {
                return this.EnabledCalc;
            }
            set
            {
                this._EnabledCalc = value;
                bool enabled = this.Enabled;
                if (enabled)
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
                base.Invalidate();
            }
        }

        [DisplayName("Enabled")]
        public bool EnabledCalc
        {
            get
            {
                return this._EnabledCalc;
            }
            set
            {
                this.Enabled = value;
                base.Invalidate();
            }
        }

        public XylosCheckBox()
        {
            this.B64Enabled = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA00lEQVQ4T6WTwQ2CMBSG30/07Ci6gY7gxZoIiYADuAIrsIDpQQ/cHMERZBOuXHimDSWALYL01EO/L//724JmLszk6S+BCOIExFsmL50sEH4kAZxVciYuJgnacD16Plpgg8tFtYMILntQdSXiZ3aXqa1UF/yUsoDw4wKglQaZZPa4RW3JEKzO4RjEbyJaN1BL8gvWgsMp3ADeq0lRJ2FimLZNYWpmFbudUJdolXTLyG2wTmDODUiccEfgSDIIfwmMxAMStS+XHPZn7l/z6Ifk+nSzBR8zi2d9JmVXSgAAAABJRU5ErkJggg==";
            this.B64Disabled = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA1UlEQVQ4T6WTzQ2CQBCF56EnLpaiXvUAJBRgB2oFtkALdEAJnoVEMIGzdEIFjNkFN4DLn+xpD/N9efMWQAsPFvL0lyBMUg8MiwzyZwuiJAuI6CyTMxezBC24EuSTBTp4xaaN6JWdqKQbge6udfB1pfbBjrMvEMZZAdCm3ilw7eO1KRmCxRyiOH0TsFUQs5KMwVLweKY7ALFKUZUTECD6qdquCxM7i9jNhLJEraQ5xZzrYJngO9crGYBbAm2SEfhHoCQGeeK+Ls1Ld+fuM0/+kPp+usWCD10idEOGa4QuAAAAAElFTkSuQmCC";
            this.DoubleBuffered = true;
            this.Enabled = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.G = e.Graphics;
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
            this.G.Clear(Color.White);
            bool enabled = this.Enabled;
            if (enabled)
            {
                using (SolidBrush solidBrush = new SolidBrush(Helpers.ColorFromHex("#F3F4F7")))
                {
                    using (Pen pen = new Pen(Helpers.ColorFromHex("#D0D5D9")))
                    {
                        using (SolidBrush solidBrush2 = new SolidBrush(Helpers.ColorFromHex("#7C858E")))
                        {
                            using (Font font = new Font("Segoe UI", 9f))
                            {
                                this.G.FillPath(solidBrush, Helpers.RoundRect(new Rectangle(0, 0, 16, 16), 3, Helpers.RoundingStyle.All));
                                this.G.DrawPath(pen, Helpers.RoundRect(new Rectangle(0, 0, 16, 16), 3, Helpers.RoundingStyle.All));
                                this.G.DrawString(this.Text, font, solidBrush2, new Point(25, 0));
                            }
                        }
                    }
                }
                bool @checked = this.Checked;
                if (@checked)
                {
                    using (Image image = Image.FromStream(new MemoryStream(Convert.FromBase64String(this.B64Enabled))))
                    {
                        this.G.DrawImage(image, new Rectangle(3, 3, 11, 11));
                    }
                }
            }
            else
            {
                using (SolidBrush solidBrush3 = new SolidBrush(Helpers.ColorFromHex("#F5F5F8")))
                {
                    using (Pen pen2 = new Pen(Helpers.ColorFromHex("#E1E1E2")))
                    {
                        using (SolidBrush solidBrush4 = new SolidBrush(Helpers.ColorFromHex("#D0D3D7")))
                        {
                            using (Font font2 = new Font("Segoe UI", 9f))
                            {
                                this.G.FillPath(solidBrush3, Helpers.RoundRect(new Rectangle(0, 0, 16, 16), 3, Helpers.RoundingStyle.All));
                                this.G.DrawPath(pen2, Helpers.RoundRect(new Rectangle(0, 0, 16, 16), 3, Helpers.RoundingStyle.All));
                                this.G.DrawString(this.Text, font2, solidBrush4, new Point(25, 0));
                            }
                        }
                    }
                }
                bool checked2 = this.Checked;
                if (checked2)
                {
                    using (Image image2 = Image.FromStream(new MemoryStream(Convert.FromBase64String(this.B64Disabled))))
                    {
                        this.G.DrawImage(image2, new Rectangle(3, 3, 11, 11));
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            bool enabled = this.Enabled;
            if (enabled)
            {
                this.Checked = !this.Checked;
                XylosCheckBox.CheckedChangedEventHandler checkedChangedEvent = this.CheckedChangedEvent;
                if (checkedChangedEvent != null)
                {
                    checkedChangedEvent(this, e);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            base.Size = new Size(base.Width, 18);
        }
    }
    public class XylosCombobox : ComboBox
    {
        private Graphics G;

        private Rectangle Rect;

        private bool _EnabledCalc;

        public new bool Enabled
        {
            get
            {
                return this.EnabledCalc;
            }
            set
            {
                this._EnabledCalc = value;
                base.Invalidate();
            }
        }

        [DisplayName("Enabled")]
        public bool EnabledCalc
        {
            get
            {
                return this._EnabledCalc;
            }
            set
            {
                base.Enabled = value;
                this.Enabled = value;
                base.Invalidate();
            }
        }

        public XylosCombobox()
        {
            this.DoubleBuffered = true;
            base.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cursor = Cursors.Hand;
            this.Enabled = true;
            base.DrawMode = DrawMode.OwnerDrawFixed;
            base.ItemHeight = 20;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            base.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.G = e.Graphics;
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
            this.G.Clear(Color.White);
            bool enabled = this.Enabled;
            checked
            {
                if (enabled)
                {
                    using (Pen pen = new Pen(Helpers.ColorFromHex("#D0D5D9")))
                    {
                        using (SolidBrush solidBrush = new SolidBrush(Helpers.ColorFromHex("#7C858E")))
                        {
                            using (Font font = new Font("Marlett", 13f))
                            {
                                this.G.DrawPath(pen, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 6, Helpers.RoundingStyle.All));
                                this.G.DrawString("6", font, solidBrush, new Point(base.Width - 22, 3));
                            }
                        }
                    }
                }
                else
                {
                    using (Pen pen2 = new Pen(Helpers.ColorFromHex("#E1E1E2")))
                    {
                        using (SolidBrush solidBrush2 = new SolidBrush(Helpers.ColorFromHex("#D0D3D7")))
                        {
                            using (Font font2 = new Font("Marlett", 13f))
                            {
                                this.G.DrawPath(pen2, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 6, Helpers.RoundingStyle.All));
                                this.G.DrawString("6", font2, solidBrush2, new Point(base.Width - 22, 3));
                            }
                        }
                    }
                }
                bool flag = !Information.IsNothing(base.Items);
                if (flag)
                {
                    using (Font font3 = new Font("Segoe UI", 9f))
                    {
                        using (SolidBrush solidBrush3 = new SolidBrush(Helpers.ColorFromHex("#7C858E")))
                        {
                            bool enabled2 = this.Enabled;
                            if (enabled2)
                            {
                                bool flag2 = this.SelectedIndex != -1;
                                if (flag2)
                                {
                                    this.G.DrawString(base.GetItemText(RuntimeHelpers.GetObjectValue(base.Items[this.SelectedIndex])), font3, solidBrush3, new Point(7, 4));
                                }
                                else
                                {
                                    try
                                    {
                                        this.G.DrawString(base.GetItemText(RuntimeHelpers.GetObjectValue(base.Items[0])), font3, solidBrush3, new Point(7, 4));
                                    }
                                    catch (Exception arg_272_0)
                                    {
                                        ProjectData.SetProjectError(arg_272_0);
                                        ProjectData.ClearProjectError();
                                    }
                                }
                            }
                            else
                            {
                                using (SolidBrush solidBrush4 = new SolidBrush(Helpers.ColorFromHex("#D0D3D7")))
                                {
                                    bool flag3 = this.SelectedIndex != -1;
                                    if (flag3)
                                    {
                                        this.G.DrawString(base.GetItemText(RuntimeHelpers.GetObjectValue(base.Items[this.SelectedIndex])), font3, solidBrush4, new Point(7, 4));
                                    }
                                    else
                                    {
                                        this.G.DrawString(base.GetItemText(RuntimeHelpers.GetObjectValue(base.Items[0])), font3, solidBrush4, new Point(7, 4));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            this.G = e.Graphics;
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            bool enabled = this.Enabled;
            checked
            {
                if (enabled)
                {
                    e.DrawBackground();
                    this.Rect = e.Bounds;
                    try
                    {
                        using (new Font("Segoe UI", 9f))
                        {
                            using (new Pen(Helpers.ColorFromHex("#D0D5D9")))
                            {
                                bool flag = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                                if (flag)
                                {
                                    using (new SolidBrush(Color.White))
                                    {
                                        using (SolidBrush solidBrush2 = new SolidBrush(Helpers.ColorFromHex("#78B7E6")))
                                        {
                                            this.G.FillRectangle(solidBrush2, this.Rect);
                                            this.G.DrawString(base.GetItemText(RuntimeHelpers.GetObjectValue(base.Items[e.Index])), new Font("Segoe UI", 9f), Brushes.White, new Point(this.Rect.X + 5, this.Rect.Y + 1));
                                        }
                                    }
                                }
                                else
                                {
                                    using (SolidBrush solidBrush3 = new SolidBrush(Helpers.ColorFromHex("#7C858E")))
                                    {
                                        this.G.FillRectangle(Brushes.White, this.Rect);
                                        this.G.DrawString(base.GetItemText(RuntimeHelpers.GetObjectValue(base.Items[e.Index])), new Font("Segoe UI", 9f), solidBrush3, new Point(this.Rect.X + 5, this.Rect.Y + 1));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception arg_1F1_0)
                    {
                        ProjectData.SetProjectError(arg_1F1_0);
                        ProjectData.ClearProjectError();
                    }
                }
            }
        }

        protected override void OnSelectedItemChanged(EventArgs e)
        {
            base.OnSelectedItemChanged(e);
            base.Invalidate();
        }
    }
    public class XylosNotice : TextBox
    {
        private Graphics G;

        private string B64;

        public XylosNotice()
        {
            this.B64 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAABL0lEQVQ4T5VT0VGDQBB9e2cBdGBSgTIDEr9MCw7pI0kFtgB9yFiC+KWMmREqMOnAAuDWOfAiudzhyA/svtvH7Xu7BOv5eH2atVKtwbwk0LWGGVyDqLzoRB7e3u/HJTQOdm+PGYjWNuk4ZkIW36RbkzsS7KqiBnB1Usw49DHh8oQEXMfJKhwgAM4/Mw7RIp0NeLG3ScCcR4vVhnTPnVCf9rUZeImTdKnz71VREnBnn5FKzMnX95jA2V6vLufkBQFESTq0WBXsEla7owmcoC6QJMKW2oCUePY5M0lAjK0iBAQ8TBGc2/d7+uvnM/AQNF4Rp4bpiGkRfTb2Gigx12+XzQb3D9JfBGaQzHWm7HS000RJ2i/av5fJjPDZMplErwl1GxDpMTbL1YC5lCwze52/AQFekh7wKBpGAAAAAElFTkSuQmCC";
            this.DoubleBuffered = true;
            base.Enabled = false;
            base.ReadOnly = true;
            base.BorderStyle = BorderStyle.None;
            this.Multiline = true;
            this.Cursor = Cursors.Default;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            base.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.G = e.Graphics;
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
            this.G.Clear(Color.White);
            using (SolidBrush solidBrush = new SolidBrush(Helpers.ColorFromHex("#FFFDE8")))
            {
                using (Pen pen = new Pen(Helpers.ColorFromHex("#F2F3F7")))
                {
                    using (SolidBrush solidBrush2 = new SolidBrush(Helpers.ColorFromHex("#B9B595")))
                    {
                        using (Font font = new Font("Segoe UI", 9f))
                        {
                            this.G.FillPath(solidBrush, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 3, Helpers.RoundingStyle.All));
                            this.G.DrawPath(pen, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 3, Helpers.RoundingStyle.All));
                            this.G.DrawString(this.Text, font, solidBrush2, new Point(30, 6));
                        }
                    }
                }
            }
            using (Image image = Image.FromStream(new MemoryStream(Convert.FromBase64String(this.B64))))
            {
                this.G.DrawImage(image, new Rectangle(8, checked((int)Math.Round(unchecked((double)base.Height / 2.0 - 8.0))), 16, 16));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }
    }
    public class XylosProgressBar : Control
    {
        private int _Val;

        private int _Min;

        private int _Max;

        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private Color _Stripes;

        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private Color _BackgroundColor;

        public Color Stripes
        {
            get;
            set;
        }

        public Color BackgroundColor
        {
            get;
            set;
        }

        public int Value
        {
            get
            {
                return this._Val;
            }
            set
            {
                this._Val = value;
                base.Invalidate();
            }
        }

        public int Minimum
        {
            get
            {
                return this._Min;
            }
            set
            {
                this._Min = value;
                base.Invalidate();
            }
        }

        public int Maximum
        {
            get
            {
                return this._Max;
            }
            set
            {
                this._Max = value;
                base.Invalidate();
            }
        }

        public XylosProgressBar()
        {
            this._Val = 0;
            this._Min = 0;
            this._Max = 100;
            this.Stripes = Color.DarkGreen;
            this.BackgroundColor = Color.Green;
            this.DoubleBuffered = true;
            this.Maximum = 100;
            this.Minimum = 0;
            this.Value = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
            graphics.Clear(Color.White);
            using (Pen pen = new Pen(Helpers.ColorFromHex("#D0D5D9")))
            {
                graphics.DrawPath(pen, Helpers.RoundRect(Helpers.FullRectangle(base.Size, true), 6, Helpers.RoundingStyle.All));
            }
            bool flag = this.Value != 0;
            if (flag)
            {
                using (HatchBrush hatchBrush = new HatchBrush(HatchStyle.LightUpwardDiagonal, this.Stripes, this.BackgroundColor))
                {
                    graphics.FillPath(hatchBrush, Helpers.RoundRect(checked(new Rectangle(0, 0, (int)Math.Round(unchecked((double)this.Value / (double)this.Maximum * (double)base.Width - 1.0)), base.Height - 1)), 6, Helpers.RoundingStyle.All));
                }
            }
        }
    }
    [DefaultEvent("CheckedChanged")]
    public class XylosRadioButton : Control
    {
        public delegate void CheckedChangedEventHandler(object sender, EventArgs e);

        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private XylosRadioButton.CheckedChangedEventHandler CheckedChangedEvent;

        private bool _Checked;

        private bool _EnabledCalc;

        private Graphics G;

        public event XylosRadioButton.CheckedChangedEventHandler CheckedChanged
        {
            [CompilerGenerated]
            add
            {
                XylosRadioButton.CheckedChangedEventHandler checkedChangedEventHandler = this.CheckedChangedEvent;
                XylosRadioButton.CheckedChangedEventHandler checkedChangedEventHandler2;
                do
                {
                    checkedChangedEventHandler2 = checkedChangedEventHandler;
                    XylosRadioButton.CheckedChangedEventHandler value2 = (XylosRadioButton.CheckedChangedEventHandler)Delegate.Combine(checkedChangedEventHandler2, value);
                    checkedChangedEventHandler = Interlocked.CompareExchange<XylosRadioButton.CheckedChangedEventHandler>(ref this.CheckedChangedEvent, value2, checkedChangedEventHandler2);
                }
                while (checkedChangedEventHandler != checkedChangedEventHandler2);
            }
            [CompilerGenerated]
            remove
            {
                XylosRadioButton.CheckedChangedEventHandler checkedChangedEventHandler = this.CheckedChangedEvent;
                XylosRadioButton.CheckedChangedEventHandler checkedChangedEventHandler2;
                do
                {
                    checkedChangedEventHandler2 = checkedChangedEventHandler;
                    XylosRadioButton.CheckedChangedEventHandler value2 = (XylosRadioButton.CheckedChangedEventHandler)Delegate.Remove(checkedChangedEventHandler2, value);
                    checkedChangedEventHandler = Interlocked.CompareExchange<XylosRadioButton.CheckedChangedEventHandler>(ref this.CheckedChangedEvent, value2, checkedChangedEventHandler2);
                }
                while (checkedChangedEventHandler != checkedChangedEventHandler2);
            }
        }

        public bool Checked
        {
            get
            {
                return this._Checked;
            }
            set
            {
                this._Checked = value;
                base.Invalidate();
            }
        }

        public new bool Enabled
        {
            get
            {
                return this.EnabledCalc;
            }
            set
            {
                this._EnabledCalc = value;
                bool enabled = this.Enabled;
                if (enabled)
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
                base.Invalidate();
            }
        }

        [DisplayName("Enabled")]
        public bool EnabledCalc
        {
            get
            {
                return this._EnabledCalc;
            }
            set
            {
                this.Enabled = value;
                base.Invalidate();
            }
        }

        public XylosRadioButton()
        {
            this.DoubleBuffered = true;
            this.Enabled = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.G = e.Graphics;
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
            this.G.Clear(Color.White);
            bool enabled = this.Enabled;
            if (enabled)
            {
                using (SolidBrush solidBrush = new SolidBrush(Helpers.ColorFromHex("#F3F4F7")))
                {
                    using (Pen pen = new Pen(Helpers.ColorFromHex("#D0D5D9")))
                    {
                        using (SolidBrush solidBrush2 = new SolidBrush(Helpers.ColorFromHex("#7C858E")))
                        {
                            using (Font font = new Font("Segoe UI", 9f))
                            {
                                this.G.FillEllipse(solidBrush, new Rectangle(0, 0, 16, 16));
                                this.G.DrawEllipse(pen, new Rectangle(0, 0, 16, 16));
                                this.G.DrawString(this.Text, font, solidBrush2, new Point(25, 0));
                            }
                        }
                    }
                }
                bool @checked = this.Checked;
                if (@checked)
                {
                    using (SolidBrush solidBrush3 = new SolidBrush(Helpers.ColorFromHex("#575C62")))
                    {
                        this.G.FillEllipse(solidBrush3, new Rectangle(4, 4, 8, 8));
                    }
                }
            }
            else
            {
                using (SolidBrush solidBrush4 = new SolidBrush(Helpers.ColorFromHex("#F5F5F8")))
                {
                    using (Pen pen2 = new Pen(Helpers.ColorFromHex("#E1E1E2")))
                    {
                        using (SolidBrush solidBrush5 = new SolidBrush(Helpers.ColorFromHex("#D0D3D7")))
                        {
                            using (Font font2 = new Font("Segoe UI", 9f))
                            {
                                this.G.FillEllipse(solidBrush4, new Rectangle(0, 0, 16, 16));
                                this.G.DrawEllipse(pen2, new Rectangle(0, 0, 16, 16));
                                this.G.DrawString(this.Text, font2, solidBrush5, new Point(25, 0));
                            }
                        }
                    }
                }
                bool checked2 = this.Checked;
                if (checked2)
                {
                    using (SolidBrush solidBrush6 = new SolidBrush(Helpers.ColorFromHex("#BCC1C6")))
                    {
                        this.G.FillEllipse(solidBrush6, new Rectangle(4, 4, 8, 8));
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            bool enabled = this.Enabled;
            if (enabled)
            {
                try
                {
                    IEnumerator enumerator = base.Parent.Controls.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Control control = (Control)enumerator.Current;
                        bool flag = control is XylosRadioButton;
                        if (flag)
                        {
                            ((XylosRadioButton)control).Checked = false;
                        }
                    }
                }
                finally
                {

                }
                this.Checked = !this.Checked;
                XylosRadioButton.CheckedChangedEventHandler checkedChangedEvent = this.CheckedChangedEvent;
                if (checkedChangedEvent != null)
                {
                    checkedChangedEvent(this, e);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            base.Size = new Size(base.Width, 18);
        }
    }
    public class XylosSeparator : Control
    {
        private Graphics G;

        public XylosSeparator()
        {
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.G = e.Graphics;
            this.G.SmoothingMode = SmoothingMode.HighQuality;
            this.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
            using (Pen pen = new Pen(Helpers.ColorFromHex("#EBEBEC")))
            {
                this.G.DrawLine(pen, new Point(0, 0), new Point(base.Width, 0));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            base.Size = new Size(base.Width, 2);
        }
    }
}