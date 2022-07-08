using System;
using System.Collections.Generic;
using System.Text;
using Eto;
using Eto.Forms;
using Eto.Drawing;

namespace Host_File_Editor
{
    public class GenericGridViewCell<T> : DrawableCell
    {
        public Func<T, Color>? BackgroundColorSelector { get; set; }
        public Func<T, string>? ObjectToString { get; set; }

        public GenericGridViewCell(Func<T, Color>? backgroundColorSelector = null, Func<T, string>? objectToString = null) {
            BackgroundColorSelector = backgroundColorSelector;
            ObjectToString = objectToString;
        }

        protected override void OnPaint(CellPaintEventArgs e) {
            if (!(e.Item is T))
                throw new NotSupportedException($"Item in cell {ID} is not of type {typeof(T).FullName}, instead was {e.Item.GetType().FullName}");
            T obj = (T)e.Item;
            Color cellColor = BackgroundColorSelector is null? SystemColors.ControlBackground : BackgroundColorSelector(obj);
            if (e.IsSelected)
                cellColor = SystemColors.Selection;
            ColorHSL outlineColor = cellColor.ToHSL();
            outlineColor.L *= 0.75f;
            string text = ObjectToString is null ? (obj is null? string.Empty : (obj.ToString() ?? string.Empty)) : ObjectToString(obj);
            var graphics = e.Graphics;
            var font = SystemFonts.Default(10);
            var pen = new Pen(outlineColor, 2);
            graphics.FillRectangle(cellColor, e.ClipRectangle);
            graphics.DrawRectangle(pen, e.ClipRectangle);
            graphics.DrawText(font, Brushes.Black, e.ClipRectangle.X + 3, (e.ClipRectangle.Y + e.ClipRectangle.Height / 2) - font.LineHeight / 2, text);
            pen.Dispose();
        }
    }
}
