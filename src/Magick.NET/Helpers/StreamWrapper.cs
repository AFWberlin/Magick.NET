﻿// Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
// Licensed under the Apache License, Version 2.0.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ImageMagick
{
    internal sealed unsafe class StreamWrapper : IDisposable
    {
        private const int BufferSize = 8192;

        private readonly byte[] _buffer;
        private readonly byte* _bufferStart;
        private readonly long _streamStart;
        private readonly GCHandle _handle;
        private Stream _stream;

        private StreamWrapper(Stream stream)
        {
            _stream = stream;
            _buffer = new byte[BufferSize];
            _handle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
            _bufferStart = (byte*)_handle.AddrOfPinnedObject().ToPointer();

            try
            {
                _streamStart = _stream.Position;
            }
            catch
            {
                _streamStart = 0;
            }
        }

        public static StreamWrapper CreateForReading(Stream stream)
        {
            Throw.IfFalse(nameof(stream), stream.CanRead, "The stream should be readable.");

            return new StreamWrapper(stream);
        }

        public static StreamWrapper CreateForWriting(Stream stream)
        {
            Throw.IfFalse(nameof(stream), stream.CanWrite, "The stream should be writable.");

            return new StreamWrapper(stream);
        }

        public void Dispose()
            => _handle.Free();

        public long Read(IntPtr data, UIntPtr count, IntPtr user_data)
        {
            var total = (long)count;
            if (total == 0)
                return 0;

            if (data == IntPtr.Zero)
                return 0;

            byte* destination = (byte*)data.ToPointer();
            long bytesRead = 0;

            while (total > 0)
            {
                var length = Math.Min(total, BufferSize);

                try
                {
                    length = _stream.Read(_buffer, 0, (int)length);
                }
                catch
                {
                    return -1;
                }

                if (length == 0)
                    break;

                bytesRead += length;

                destination = ReadBuffer(destination, length);

                total -= length;
            }

            return bytesRead;
        }

        public long Seek(long offset, IntPtr whence, IntPtr user_data)
        {
            try
            {
                switch ((SeekOrigin)whence)
                {
                    case SeekOrigin.Begin:
                        return _stream.Seek(_streamStart + offset, SeekOrigin.Begin) - _streamStart;
                    case SeekOrigin.Current:
                        return _stream.Seek(offset, SeekOrigin.Current) - _streamStart;
                    case SeekOrigin.End:
                        return _stream.Seek(offset, SeekOrigin.End) - _streamStart;
                    default:
                        return -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public long Tell(IntPtr user_data)
        {
            try
            {
                return _stream.Position - _streamStart;
            }
            catch
            {
                return -1;
            }
        }

        public long Write(IntPtr data, UIntPtr count, IntPtr user_data)
        {
            var total = (long)count;
            if (total == 0)
                return 0;

            if (data == IntPtr.Zero)
                return 0;

            byte* source = (byte*)data.ToPointer();

            while (total > 0)
            {
                var length = Math.Min(total, BufferSize);

                source = FillBuffer(source, length);

                try
                {
                    _stream.Write(_buffer, 0, (int)length);
                }
                catch
                {
                    return -1;
                }

                total -= length;
            }

            return (long)count;
        }

        private byte* FillBuffer(byte* source, long length)
        {
            byte* destination = _bufferStart;
            while (length >= 4)
            {
                *(destination++) = *(source++);
                *(destination++) = *(source++);
                *(destination++) = *(source++);
                *(destination++) = *(source++);
                length -= 4;
            }

            while (length-- > 0)
            {
                *(destination++) = *(source++);
            }

            return source;
        }

        private byte* ReadBuffer(byte* destination, long length)
        {
            byte* source = _bufferStart;
            while (length >= 4)
            {
                *(destination++) = *(source++);
                *(destination++) = *(source++);
                *(destination++) = *(source++);
                *(destination++) = *(source++);
                length -= 4;
            }

            while (length-- > 0)
            {
                *(destination++) = *(source++);
            }

            return destination;
        }
    }
}
