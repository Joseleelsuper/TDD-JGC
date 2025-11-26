using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Utils;
using System.IO;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace UtilTest
{
    [TestClass]
    public class UsuarioTest
    {
        private User u = null;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            u = new User(
                username: "testuser",
                name: "Test User",
                surnames: "Surname Test",
                email: "testUser@ubu.es",
                rawPassword: "TestP@ssword1",
                language: Language.ES,
                isActive: true,
                expirationDate: DateTime.Now.AddMonths(1));
        }

        [TestCleanup]
        public void Cleanup()
        {
            u = null;
        }

        [TestMethod]
        public void checkIdTest()
        {
            Assert.IsTrue(u.Id > 0);
        }

        [TestMethod]
        public void changeUsernameTest()
        {
            Assert.AreEqual("testuser", u.Username);
            var ok = u.SetUsername("newtestuser");
            Assert.IsTrue(ok);
            Assert.AreEqual("newtestuser", u.Username);
            Assert.IsFalse(u.SetUsername("newTestUser")); // contiene mayúsculas
            Assert.IsFalse(u.SetUsername("invalid username!")); // contiene espacios y símbolos
            Assert.AreEqual("newtestuser", u.Username); // sin cambios tras intentos inválidos
        }

        [TestMethod]
        public void changeNameTest()
        {
            Assert.AreEqual("Test User", u.Name);
            u.SetName("New Test User");
            Assert.AreEqual("New Test User", u.Name);
        }

        [TestMethod]
        public void changeSurnamesTest()
        {
            Assert.AreEqual("Surname Test", u.Surnames);
            u.SetSurnames("New Surname Test");
            Assert.AreEqual("New Surname Test", u.Surnames);
        }

        [TestMethod]
        public void changeEmailTest()
        {
            Assert.AreEqual("testUser@ubu.es", u.Email);
            Assert.IsTrue(u.SetEmail("newTestUser@ubu.es"));
            Assert.AreEqual("newTestUser@ubu.es", u.Email);
            Assert.IsFalse(u.SetEmail("invalid email"));
            Assert.AreEqual("newTestUser@ubu.es", u.Email);
        }

        [TestMethod]
        public void changeLanguageTest()
        {
            Assert.AreEqual(Language.ES, u.Language);
            u.SetLanguage(Language.EN);
            Assert.AreEqual(Language.EN, u.Language);
        }

        [TestMethod]
        public void deactivateAccountTest()
        {
            Assert.IsTrue(u.IsActive);
            u.DeactivateAccount();
            Assert.IsFalse(u.IsActive);
            u.ActivateAccount();
            Assert.IsTrue(u.IsActive);
        }

        [TestMethod]
        public void HashPasswordTest()
        {
            var raw = "SampleP@ssword3";
            var hashed = u.HashPassword(raw);
            Assert.AreEqual(u.HashPassword(raw), hashed);
            Assert.AreNotEqual(u.HashPassword("DifferentP@ssword4"), hashed);
        }

        [TestMethod]
        public void changePasswordTest()
        {
            var originalHash = u.Password; 
            Assert.IsTrue(u.SetPassword("NewP@ssword2"));
            Assert.AreNotEqual(originalHash, u.Password);
        }
        public static IEnumerable<object[]> LeerEmailCsv()
        {
            var filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "DatosTest", "email.csv"));
            if (!File.Exists(filePath)) yield break;
            foreach (var linea in File.ReadAllLines(filePath).Where(l => l != null))
            {
                var campos = linea.Split(';');
                if (campos.Length >= 2)
                {
                    yield return new object[]
                    {
                        campos[0] == "null" ? null : campos[0],
                        int.Parse(campos[1])
                    };
                }
            }
        }

        [TestMethod]
        [DynamicData(nameof(LeerEmailCsv), DynamicDataSourceType.Method)]
        public void EmailTest(string email, int expected)
        {
            u.SetEmail("baseline@ubu.es");
            bool ok = u.SetEmail(email);
            Assert.AreEqual(expected == 1, ok, $"Email '{email}' esperado {expected}");
            if (expected == 1)
            {
                Assert.AreEqual(email, u.Email);
            }
            else
            {
                Assert.AreEqual("baseline@ubu.es", u.Email);
            }
        }
    }
}
