using System;
using System.IO;
using System.Text;

namespace ByteFile
{
    public class BFile
    {
        private string Path;
        private Stream stream = null;

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

        public void BeginWrite()
        {
            stream = File.OpenWrite(Path);
        }

        public void BeginRead()
        {
            stream = File.OpenRead(Path);
            stream.Seek(0, SeekOrigin.Begin);
        }

        public void End()
        {
            stream.Close();
            stream = null;
        }

        public void Write(params string[] data)
        {

            foreach (var d in data)
            {
                // Length = ? bytes, need to write before
                stream.Seek(0, SeekOrigin.End);
                var bytes = Encoding.UTF8.GetBytes(d);
                Write(d.Length); // Save the length of the string
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(params long[] data)
        {
            foreach (var d in data)
            {
                // Length = 8 bytes
                stream.Seek(0, SeekOrigin.End);
                var bytes = BitConverter.GetBytes(d);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(params double[] data)
        {
            foreach (var d in data)
            {
                // Length = 8 bytes
                stream.Seek(0, SeekOrigin.End);
                var bytes = BitConverter.GetBytes(d);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(params float[] data)
        {
            foreach (var d in data)
            {
                // Length = 4 bytes
                stream.Seek(0, SeekOrigin.End);
                var bytes = BitConverter.GetBytes(d);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(params int[] data)
        {
            foreach (var d in data)
            {
                // Length = 4 bytes
                stream.Seek(0, SeekOrigin.End);
                var bytes = BitConverter.GetBytes(d);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(params byte[] data)
        {
            // Length = 1 byte
            foreach (var d in data)
            {
                stream.Seek(0, SeekOrigin.End);
                stream.WriteByte(d);
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
