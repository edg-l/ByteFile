using System;
using System.IO;
using System.Text;

namespace ByteFile
{
    public class BFile
    {
        private string Path;
        private FileStream stream = null;
        private Encoding encoding = Encoding.UTF8;
        private bool CloseStream = true;

        public BFile(string path, bool clean = false)
        {
            Path = path;

            if(!File.Exists(Path))
                File.Create(Path).Close();
            else
            {
                if(clean)
                {
                    File.Delete(path);
                    File.Create(Path).Close();
                }
            }
        }

        public void SetEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public void BeginRead()
        {
            stream = File.OpenRead(Path);
            stream.Seek(0, SeekOrigin.Begin);
        }

        public void EndRead()
        {
            stream.Close();
            stream = null;
        }

        /// <summary>
        /// Write to the file.
        /// </summary>
        /// <param name="data">Can be: byte, char, bool, short, ushort, int, uint, long, ulong, float, double, string</param>
        /// <exception cref="NotImplementedException">Thrown when an unsuported data type is given.</exception>
        public void Write(params object[] data)
        {
            if(stream == null)
                stream = File.OpenWrite(Path);

            if (!stream.CanWrite)
                stream = File.OpenWrite(Path);

            foreach (var d in data)
            {
                if(d is string)
                {
                    // Length = ? bits, need to write before
                    var str = (string)d;
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = encoding.GetBytes(str);
                    CloseStream = false;
                    Write(str.Length); // Save the length of the string
                    stream.Write(bytes, 0, bytes.Length);
                    CloseStream = true;
                    continue;
                }
                if (d is bool)
                {
                    // Length = 1 bits
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = BitConverter.GetBytes((bool)d);
                    stream.Write(bytes, 0, bytes.Length);
                    continue;
                }
                if (d is int)
                {
                    // Length = 4 bits
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = BitConverter.GetBytes((int)d);
                    stream.Write(bytes, 0, bytes.Length);
                    continue;
                }
                if (d is uint)
                {
                    // Length = 4 bits
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = BitConverter.GetBytes((uint)d);
                    stream.Write(bytes, 0, bytes.Length);
                    continue;
                }
                if (d is float)
                {
                    // Length = 4 bits
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = BitConverter.GetBytes((float)d);
                    stream.Write(bytes, 0, bytes.Length);
                    continue;
                }
                if(d is short)
                {
                    // Length = 2 bits
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = BitConverter.GetBytes((short)d);
                    stream.Write(bytes, 0, bytes.Length);
                    continue;
                }
                if (d is ushort)
                {
                    // Length = 2 bits
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = BitConverter.GetBytes((ushort)d);
                    stream.Write(bytes, 0, bytes.Length);
                    continue;
                }
                if (d is char)
                {
                    // Length = 2 bits
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = BitConverter.GetBytes((char)d);
                    stream.Write(bytes, 0, bytes.Length);
                    continue;
                }
                if (d is byte)
                {
                    stream.Seek(0, SeekOrigin.End);
                    stream.WriteByte((byte)d);
                    continue;
                }
                if (d is long)
                {
                    // Length = 8 bytes
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = BitConverter.GetBytes((long)d);
                    stream.Write(bytes, 0, bytes.Length);
                    continue;
                }
                if (d is ulong)
                {
                    // Length = 8 bytes
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = BitConverter.GetBytes((ulong)d);
                    stream.Write(bytes, 0, bytes.Length);
                    continue;
                }
                if (d is double)
                {
                    // Length = 8 bytes
                    stream.Seek(0, SeekOrigin.End);
                    var bytes = BitConverter.GetBytes((double)d);
                    stream.Write(bytes, 0, bytes.Length);
                    continue;
                }

                stream.Close();
                stream = null;
                throw new NotImplementedException("This datatype is not implemented.");
            }

            if(CloseStream)
            {
                stream.Close();
                stream = null;
            }
        }

        /// <summary>
        /// Sets the read position at start.
        /// </summary>
        public void ResetRead()
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        public void SkipRead(int bytes)
        {
            stream.Seek(bytes, SeekOrigin.Current);
        }

        public T Read<T>()
        {
            Type type = typeof(T);

            if (type == typeof(string))
            {
                var length = Read<int>();
                var buffer = new byte[length];
                stream.Read(buffer, 0, length);
                var value = Encoding.UTF8.GetString(buffer, 0, length);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            if (type == typeof(int))
            {
                var buffer = new byte[4];
                stream.Read(buffer, 0, 4);
                var value = BitConverter.ToInt32(buffer, 0);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            if (type == typeof(float))
            {
                var buffer = new byte[4];
                stream.Read(buffer, 0, 4);
                var value = BitConverter.ToSingle(buffer, 0);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            if (type == typeof(short))
            {
                var buffer = new byte[2];
                stream.Read(buffer, 0, 2);
                var value = BitConverter.ToInt16(buffer, 0);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            if (type == typeof(double))
            {
                var buffer = new byte[8];
                stream.Read(buffer, 0, 8);
                var value = BitConverter.ToDouble(buffer, 0);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            if (type == typeof(double))
            {
                var buffer = new byte[8];
                stream.Read(buffer, 0, 8);
                var value = BitConverter.ToInt64(buffer, 0);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            if (type == typeof(bool))
            {
                var buffer = new byte[1];
                stream.Read(buffer, 0, 1);
                var value = BitConverter.ToBoolean(buffer, 0);
                return (T)Convert.ChangeType(value, typeof(T));
            }

            return (T)Convert.ChangeType(null, typeof(T));
        }
    }
}
