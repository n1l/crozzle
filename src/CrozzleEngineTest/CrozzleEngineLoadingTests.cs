using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CrozzleEngine.Loaders;
using CrozzleEngine.Model;
using CrozzleEngine.Validators;
using NUnit.Framework;

namespace CrozzleEngineTest
{
    [TestFixture]
    public class CrozzleEngineLoadingTests
    {
        [Test]
        public void EasyConfigValidationSuccess()
        {
            var configValidator = new ConfigurationFileValidator();

            var validationResult = configValidator.Validate(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Configuration EASY.txt");

            Assert.IsTrue(validationResult.IsValid);
        }

        [Test]
        public void ConfigLoadingSuccessTest()
        {
            var configLoader = new ConfigurationLoader(new ConfigurationFileValidator());

            Assert.DoesNotThrow(() => configLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Configuration EASY.txt"));

            var config = configLoader.GetModel();
            Assert.AreEqual(config.PointPerLetters.Count, 52);
            Assert.AreEqual(config.PointsPerWord, 0);
            Assert.AreEqual(config.GrupsLimit, 1000);
        }

        [TestCase("\\Data\\Test 1 - crozzle.txt")]
        [TestCase("\\Data\\Test 2 - crozzle.txt")]
        [TestCase("\\Data\\Test 3 - crozzle.txt")]
        public void CrozzleValidationSuccess(string filePath)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + filePath;
            var crozzleValidator = new CrozzleFileValidator();
            var result = crozzleValidator.Validate(path);
            Console.WriteLine(string.Join("\n", result.Messages));
            Assert.IsTrue(result.IsValid);
        }

        [TestCase("\\Data\\Test 4 - crozzle invalid.txt")]
        [TestCase("\\Data\\Test 5 - crozzle invalid.txt")]
        public void CrozzleValidationFail(string filePath)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + filePath;
            var crozzleValidator = new CrozzleFileValidator();
            var result = crozzleValidator.Validate(path);
            Console.WriteLine(string.Join("\n", result.Messages));
            Assert.IsFalse(result.IsValid);
        }


        [TestCase("\\Data\\Test 4 - wrong erros count config.txt", "\\Data\\Test 4 - wrong erros count crozzle.txt")]
        public void ErrosCountTest(string configPath, string crozzlePath)
        {

            var configLoader = new ConfigurationLoader(new ConfigurationFileValidator());

            try
            {
                configLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + configPath);
            }
            catch
            {
                // ignored
            }
            var config = configLoader.GetModel();
            var crozzleLoader = new CrozzleLoader(new CrozzleFileValidator()).SetConfig(config);

            try
            {
                crozzleLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + crozzlePath);

            }
            catch
            {
                // ignored
            }

            var crozzle = crozzleLoader.GetModel();

            Assert.Greater(crozzle.ValidationResult.ErrorsCount, 30);
        }


        [Test]
        public void TestEnums()
        {
            Assert.IsTrue(Difficult.Easy.ToString()
                     .ToUpper()
                     .Equals("EASY"));
        }
    }
}
