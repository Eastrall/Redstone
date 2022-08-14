using System;
using System.IO;

namespace Redstone.NBT.Tests;

class PartialReadStream : Stream
{
    private readonly Stream _baseStream;
    private readonly int _increment;

    public PartialReadStream(Stream baseStream)
        : this(baseStream, 1) { }


    public PartialReadStream(Stream baseStream, int increment)
    {
        _baseStream = baseStream ?? throw new ArgumentNullException("baseStream");
        _increment = increment;
    }


    public override void Flush()
    {
        _baseStream.Flush();
    }


    public override long Seek(long offset, SeekOrigin origin)
    {
        return _baseStream.Seek(offset, origin);
    }


    public override void SetLength(long value)
    {
        _baseStream.SetLength(value);
    }


    public override int Read(byte[] buffer, int offset, int count)
    {
        int bytesToRead = Math.Min(_increment, count);
        return _baseStream.Read(buffer, offset, bytesToRead);
    }


    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }


    public override bool CanRead
    {
        get { return true; }
    }

    public override bool CanSeek
    {
        get { return _baseStream.CanSeek; }
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override long Length
    {
        get { return _baseStream.Length; }
    }

    public override long Position
    {
        get { return _baseStream.Position; }
        set { _baseStream.Position = value; }
    }
}
