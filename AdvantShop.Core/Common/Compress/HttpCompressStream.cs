//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.IO.Compression;

namespace AdvantShop.Core.Compress
{
    public class HttpCompressStream : Stream
    {
        public enum CompressionType { None = 0, GZip = 1, Deflate = 2 };
        private readonly Stream _stream;

        public HttpCompressStream(Stream stream, CompressionMode mode, CompressionType type)
        {
            switch (type)
            {
                case CompressionType.GZip:
                    _stream = new GZipStream(stream, mode);
                    break;
                case CompressionType.Deflate:
                    _stream = new DeflateStream(stream, mode);
                    break;
                default:
                    _stream = new StreamWriter(stream).BaseStream;
                    break;
            }
        }

        public Stream BaseStream
        {
            get { return _stream; }
        }
        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }
        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }
        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }
        public override long Length
        {
            get { return _stream.Length; }
        }
        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        public override IAsyncResult BeginRead(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
        {
            return _stream.BeginRead(array, offset, count, asyncCallback, asyncState);
        }
        public override IAsyncResult BeginWrite(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
        {
            return _stream.BeginWrite(array, offset, count, asyncCallback, asyncCallback);
        }
        protected override void Dispose(bool disposing)
        {
            _stream.Close();
            _stream.Dispose();
        }
        public override int EndRead(IAsyncResult asyncResult)
        {
            return _stream.EndRead(asyncResult);
        }
        public override void EndWrite(IAsyncResult asyncResult)
        {
            _stream.EndWrite(asyncResult);
        }
        public override void Flush()
        {
            _stream.Flush();
        }
        public override int Read(byte[] array, int offset, int count)
        {
            return _stream.Read(array, offset, count);
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }
        public override void Write(byte[] array, int offset, int count)
        {
            _stream.Write(array, offset, count);
        }
    }
}
