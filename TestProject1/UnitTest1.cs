using NUnit.Framework.Interfaces;

namespace ManifestDestiny
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("Warps/WarpMap00.json")]
        [TestCase("Warps/WarpMap01.jso")]
        [TestCase("WarpMap01.json")]
        public void TestWarpNull(string path)
        {

            CustomJson<WarpContainer> jsonReader = new CustomJson<WarpContainer>(path);

            // Lire les données JSON et obtenir la liste de warps
            WarpContainer warp = jsonReader.Read();
            Assert.IsNull(warp);
        }

        [Test]
        [TestCase("Warps/WarpMap01.json")]
        [TestCase("Warps/WarpHouse02.json")]
        [TestCase("Warps/WarpHeal01.json")]
        public void TestWarpNotNull(string path)
        {

            CustomJson<WarpContainer> jsonReader = new CustomJson<WarpContainer>(path);

            // Lire les données JSON et obtenir la liste de warps
            WarpContainer warp = jsonReader.Read();
            Assert.IsNotNull(warp);
        }
    }
}