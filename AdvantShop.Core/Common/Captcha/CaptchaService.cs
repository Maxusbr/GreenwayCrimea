using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Common.Captcha
{
    public static class CaptchaService_old
    {
        private const int LettersCount = 4;

        #region Help methods

        private static void GetOutputImage(Stream stream, string captchaText)
        {
            if (captchaText == null) return;

            var letter = new List<Letter>();
            int totalWidth = 0;
            int maxHeight = 0;
            foreach (char c in captchaText)
            {
                var ltr = new Letter(c, 20);
                letter.Add(ltr);
                int space = (new Random()).Next(1, 5) + 1;
                ltr.Space = space;
                totalWidth += ltr.LetterSize.Width + space;
                if (maxHeight < ltr.LetterSize.Height)
                {
                    maxHeight = ltr.LetterSize.Height;
                }
            }
            const int hMargin = 5;
            const int vMargin = 3;

            using (var bmp = new Bitmap(totalWidth + hMargin, maxHeight + vMargin))
            {
                using (var grph = Graphics.FromImage(bmp))
                {
                    grph.FillRectangle(new SolidBrush(Color.Lavender), 0, 0, bmp.Width, bmp.Height);
                    using (var grp = Graphics.FromImage(bmp))
                    {
                        var file = HostingEnvironment.MapPath("~/images/captcha/captcha1.png");
                        if (string.IsNullOrWhiteSpace(file)) return;
                        using (var background = Image.FromFile(file))
                            grp.DrawImage(background, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    }
                    grph.CompositingQuality = CompositingQuality.HighQuality;
                    grph.SmoothingMode = SmoothingMode.HighQuality;
                    grph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    grph.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    var xPos = hMargin;
                    foreach (var ltr in letter)
                    {
                        grph.DrawString(ltr.Symbol.ToString(), ltr.Font, new SolidBrush(Color.Navy), xPos, vMargin);
                        xPos += ltr.LetterSize.Width + ltr.Space;
                        ltr.Dispose();
                    }
                }
                bmp.Save(stream, ImageFormat.Jpeg);
            }
        }

        private static void GetOutputErrorImage(Stream stream, string captchaText)
        {
            if (captchaText == null) return;

            using (var bmp = new Bitmap(140, 22))
            {
                using (var grph = Graphics.FromImage(bmp))
                {
                    grph.FillRectangle(new SolidBrush(Color.Lavender), 0, 0, bmp.Width, bmp.Height);

                    grph.CompositingQuality = CompositingQuality.HighQuality;
                    grph.SmoothingMode = SmoothingMode.HighQuality;
                    grph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    grph.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    grph.DrawString(captchaText, new Font("Eccentric Std", 10), new SolidBrush(Color.Navy), 2, 2);
                }
                bmp.Save(stream, ImageFormat.Jpeg);
            }
        }

        #endregion

        public static Stream GetImage(string captchatext)
        {
            if (string.IsNullOrEmpty(captchatext))
                return null;

            Stream stream = new MemoryStream();

            try
            {
                GetOutputImage(stream, SecurityHelper.DecryptString(Convert.FromBase64String(captchatext)));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                GetOutputErrorImage(stream, LocalizationService.GetResource("Captcha.ImageError"));
            }
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
        
        public static Captcha_old GetNewCaptcha()
        {
            var captcha = string.Empty;
            var validchars = "123456789qwertyupasdfghjkzxcvbnm".ToCharArray();
            var rand = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);

            for (int i = 0; i <= LettersCount - 1; i++)
            {
                char newChar;
                do
                {
                    newChar = char.ToUpper(validchars[rand.Next(0, validchars.Length)]);
                }
                while (captcha.Contains(newChar));

                captcha += newChar;
            }
            
            var base64 = Convert.ToBase64String(SecurityHelper.EncryptString(captcha));
            var captchaMd5 = (captcha + Customers.CustomerContext.CustomerId).Md5();

            return new Captcha_old()
            {
                Base64Text = base64,
                EncodedBase64Text = HttpUtility.UrlEncode(base64),
                Source = HttpUtility.UrlEncode(captchaMd5)
            };
        }

        public static bool IsValidCode(string code, string source)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(source))
                return false;

            //var enteredText = HttpUtility.UrlEncode(code.ToUpper().Md5());
            var enteredText = HttpUtility.UrlEncode((code.ToUpper() + Customers.CustomerContext.CustomerId).Md5());
            return enteredText == source;
        }
    }
}
