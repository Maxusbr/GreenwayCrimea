using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Controls;

namespace AdvantShop.Module.AdvQrCode.Controllers
{
    [Module(Type = "advqrcode")]
    public class AdvQrCodeController : ModuleController
    {
        public ActionResult ProductRightBlock()
        {
            var url = (UrlService.IsSecureConnection(System.Web.HttpContext.Current.Request) ? "https://" : "http://") +
                      System.Web.HttpContext.Current.Request.Url.Authority +
                      System.Web.HttpContext.Current.Request.RawUrl;

            return Content("<img src =\"./advqrcode/getqrcode?url=" + HttpUtility.UrlEncode(url) + "\" alt=\"qrcode\">");
        }

        public ActionResult GetQrCode(string url)
        {
            QrCode qrCode;
            var encoder = new QrEncoder(ErrorCorrectionLevel.M);
            encoder.TryEncode(url.Split(new[] { "?" }, StringSplitOptions.None)[0], out qrCode);

            var renderer = new Renderer(2, Brushes.Black, Brushes.White);

            Stream stream = new MemoryStream();

            //using (stream)
            //{
                renderer.WriteToStream(qrCode.Matrix, stream, ImageFormat.Png);
            //}
            stream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(stream, "image/png");
        }
    }
}
