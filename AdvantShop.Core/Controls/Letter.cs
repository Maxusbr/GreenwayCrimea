//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Drawing;

namespace AdvantShop.Core.Controls
{
    public class Letter : IDisposable
    {
        private readonly string[] _validFonts = { "Eccentric Std" };
        //Private ValidFonts As String() = {"Segoe Script", "Century", "Eccentric Std", "Freestyle Script", "Viner Hand ITC"}

        public Size LetterSize
        {
            get
            {
                using (var bitmap = new Bitmap(1, 1))
                {
                    using (var grph = Graphics.FromImage(bitmap))
                        return grph.MeasureString(Symbol.ToString(), Font).ToSize();
                }
            }
        }

        public Font Font { get; private set; }
        public char Symbol { get; private set; }
        public int Space { get; set; }

        //constructor
        public Letter(char c, int size)
        {
            var rnd = new Random();

            //font = New Font(ValidFonts(rnd.[Next](ValidFonts.Count() - 1)), rnd.[Next](20) + 20, GraphicsUnit.Pixel)
            Font = new Font(_validFonts[rnd.Next(_validFonts.Length)], size, GraphicsUnit.Pixel);
            Symbol = c;
        }

        #region  IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        // IDisposable

        ~Letter()// the finalizer
        {
            Dispose(false);
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (Font != null)
                    {
                        Font.Dispose();
                        Font = null;
                    }
                }
            }
            _disposedValue = true;
        }

        #endregion
    }
}