using System;
using System.Linq;
using System.IO;
using System.Reflection;

using ByteFile;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class MapTest
    {
        public BFile Map;
        private string FileName = "Test.map";
        private string OutPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [SetUp]
        public void SetUp()
        {
            Map = new BFile(Path.Combine(OutPath, FileName), true);
        }

        [Test]
        public void TestIntegers()
        {
            Random rnd = new Random();
            var n1 = rnd.Next(0, int.MaxValue);
            var n2 = rnd.Next(0, int.MaxValue);
            var n3 = rnd.Next(0, int.MaxValue);

            Map.Write(n1, n2, n3);

            Map.BeginRead();
            var r1 = Map.Read<int>();
            var r2 = Map.Read<int>();
            var r3 = Map.Read<int>();
            Map.EndRead();

            Assert.AreEqual(n1, r1, "Read not equal to write, {0} != {1}", n1, r1);
            Assert.AreEqual(n2, r2, "Read not equal to write, {0} != {1}", n2, r2);
            Assert.AreEqual(n3, r3, "Read not equal to write, {0} != {1}", n3, r3);
        }

        [Test]
        public void TestFloats()
        {
            Random rnd = new Random();
            var n1 = NextFloat(rnd);
            var n2 = NextFloat(rnd);
            var n3 = NextFloat(rnd);

            Map.Write(n1, n2, n3);

            Map.BeginRead();
            var r1 = Map.Read<float>();
            var r2 = Map.Read<float>();
            var r3 = Map.Read<float>();
            Map.EndRead();

            Assert.AreEqual(n1, r1, "Read not equal to write, {0} != {1}", n1, r1);
            Assert.AreEqual(n2, r2, "Read not equal to write, {0} != {1}", n2, r2);
            Assert.AreEqual(n3, r3, "Read not equal to write, {0} != {1}", n3, r3);
        }

        [Test]
        public void TestString()
        {
            Random rnd = new Random();
            var n1 = RandomString(rnd, 5);
            var n2 = RandomString(rnd, 7);
            var n3 = RandomString(rnd, 3);

            Map.Write(n1, n2, n3);

            Map.BeginRead();
            var r1 = Map.Read<string>();
            var r2 = Map.Read<string>();
            var r3 = Map.Read<string>();
            Map.EndRead();

            Assert.AreEqual(n1, r1, "Read not equal to write, {0} != {1}", n1, r1);
            Assert.AreEqual(n2, r2, "Read not equal to write, {0} != {1}", n2, r2);
            Assert.AreEqual(n3, r3, "Read not equal to write, {0} != {1}", n3, r3);
        }

        [Test]
        public void TestMixed()
        {
            Random rnd = new Random();
            var n1 = NextFloat(rnd);
            var n2 = RandomString(rnd, 5);
            var n3 = rnd.Next(0, int.MaxValue);

            Map.Write(n1, n2, n3);

            Map.BeginRead();
            var r1 = Map.Read<float>();
            var r2 = Map.Read<string>();
            var r3 = Map.Read<int>();
            Map.EndRead();

            Assert.AreEqual(n1, r1, "Read not equal to write, {0} != {1}", n1, r1);
            Assert.AreEqual(n2, r2, "Read not equal to write, {0} != {1}", n2, r2);
            Assert.AreEqual(n3, r3, "Read not equal to write, {0} != {1}", n3, r3);
        }

        struct TestStruct
        {
            public int x;
            public int y;
        }

        [Test]
        public void ExceptionTest()
        {
            Assert.Throws<NullReferenceException>(() => Map.Read<string>());
            Assert.Throws<NullReferenceException>(() => Map.SkipRead(2));
            Assert.Throws<NullReferenceException>(() => Map.EndRead());

            TestStruct test;
            test.x = 2; test.y = 4;
            Assert.Throws<NotImplementedException>(() => Map.Write(test));
        }

        static float NextFloat(Random random)
        {
            var buffer = new byte[4];
            random.NextBytes(buffer);
            return BitConverter.ToSingle(buffer, 0);
        }
        public static string RandomString(Random random, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
