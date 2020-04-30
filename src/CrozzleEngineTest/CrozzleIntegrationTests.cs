using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CrozzleEngine.Loaders;
using CrozzleEngine.Model;
using CrozzleEngine.Validators;
using NUnit.Framework;

namespace CrozzleEngineTest
{
    [TestFixture]
    public class CrozzleIntegrationTests
    {
        [Test]
        public void CrozlleLoadingSuccessTest()
        {
            var configLoader = new ConfigurationLoader(new ConfigurationFileValidator());
            Assert.DoesNotThrow(() => configLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Configuration EASY.txt"));
            var config = configLoader.GetModel();
            var crozzleLoader = new CrozzleLoader(new CrozzleFileValidator()).SetConfig(config);
            Assert.DoesNotThrow(() => crozzleLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Test 1 - crozzle.txt"));
            var crozzle = crozzleLoader.GetModel();

            var html = crozzle.SerializeToHtml();
            Console.WriteLine(html);
            Assert.IsTrue(!string.IsNullOrEmpty(html));
        }


        [Test]
        public void SerializeValidationResultInHtmlTest()
        {
            var configLoader = new ConfigurationLoader(new ConfigurationFileValidator());
            Assert.DoesNotThrow(() => configLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Configuration EASY.txt"));
            var config = configLoader.GetModel();
            var crozzleLoader = new CrozzleLoader(new CrozzleFileValidator()).SetConfig(config);
            Assert.DoesNotThrow(() => crozzleLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Test 1 - crozzle.txt"));
            var crozzle = crozzleLoader.GetModel();

            var html = crozzle.SerializeToHtml();
            Console.WriteLine(html);
            Assert.IsTrue(!string.IsNullOrEmpty(html));
        }


        [Test]
        public void CrozzleConstraintsFailTests()
        {
            var configLoader = new ConfigurationLoader(new ConfigurationFileValidator());
            Assert.DoesNotThrow(() => configLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Configuration HARD.txt"));
            var config = configLoader.GetModel();
            var crozzleLoader = new CrozzleLoader(new CrozzleFileValidator()).SetConfig(config);
            Assert.DoesNotThrow(() => crozzleLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Test 6 - crozzle invalid.txt"));
            var crozzle = crozzleLoader.GetModel();
            Assert.IsFalse(crozzle.ValidationResult.IsValid);
        }

        [Test]
        public void CrozzleConstraintsPassTests()
        {
            var configLoader = new ConfigurationLoader(new ConfigurationFileValidator());
            Assert.DoesNotThrow(() => configLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Configuration EASY.txt"));
            var config = configLoader.GetModel();
            var crozzleLoader = new CrozzleLoader(new CrozzleFileValidator()).SetConfig(config);
            Assert.DoesNotThrow(() => crozzleLoader.CreateModel(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Test 1 - crozzle.txt"));
            var crozzle = crozzleLoader.GetModel();
            Assert.IsTrue(crozzle.ValidationResult.IsValid);
        }
    }
}
