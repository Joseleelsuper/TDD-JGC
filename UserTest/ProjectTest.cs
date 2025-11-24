using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Utils;

namespace UtilTest
{
    [TestClass]
    public class ProjectTest
    {
        private Project p;
        private User u;

        [TestInitialize]
        public void Setup()
        {
            p = new Project(
                name: "Test Project",
                prefix: "TP",
                description: "This is a test project.",
                isPublic: false);

            u = new User("testuser", "Test", "User", "test@ubu.es", "TestP@ssword1", Language.ES, true, DateTime.Now.AddMonths(1));
        }

        [TestCleanup]
        public void Cleanup()
        {
            p = null;
            u = null;
        }

        [TestMethod]
        public void checkIdTest()
        {
            Assert.IsTrue(p.Id > 0);
        }

        [TestMethod]
        public void changeNameTest()
        {
            Assert.AreEqual("Test Project", p.Name);
            Assert.IsTrue(p.SetName("New Test Project"));
            Assert.AreEqual("New Test Project", p.Name);
            Assert.IsFalse(p.SetName(""));
        }

        [TestMethod]
        public void changePrefixTest()
        {
            Assert.AreEqual("TP", p.Prefix);
            Assert.IsTrue(p.SetPrefix("NTP"));
            Assert.AreEqual("NTP", p.Prefix);
            Assert.IsFalse(p.SetPrefix("invalid prefix!"));
            Assert.AreEqual("NTP", p.Prefix);
        }

        [TestMethod]
        public void changeDescriptionTest()
        {
            Assert.AreEqual("This is a test project.", p.Description);
            Assert.IsTrue(p.SetDescription("This is a new test project description."));
            Assert.AreEqual("This is a new test project description.", p.Description);
        }

        [TestMethod]
        public void changeVisibilityTest()
        {
            Assert.IsFalse(p.IsPublic);
            p.SetIsPublic(true);
            Assert.IsTrue(p.IsPublic);
        }

        [TestMethod]
        public void changeUserRoleTest()
        {
            // Assign first role
            Assert.IsTrue(p.AssignUserRole(u, Roles.Tester));
            Assert.AreEqual(Roles.Tester, p.GetUserRole(u));
            // Change role
            Assert.IsTrue(p.AssignUserRole(u, Roles.Admin));
            Assert.AreEqual(Roles.Admin, p.GetUserRole(u));
        }
    }
}
