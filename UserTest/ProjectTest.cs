using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            // Asignar rol existente (Tester)
            Assert.IsTrue(p.AssignUserRole(u, "Tester"));
            Assert.AreEqual("Tester", p.GetUserRole(u).Name);
            // Cambiar a Admin
            Assert.IsTrue(p.AssignUserRole(u, "Admin"));
            Assert.AreEqual("Admin", p.GetUserRole(u).Name);
            // Intentar rol inexistente
            Assert.IsFalse(p.AssignUserRole(u, "NoExiste"));
            Assert.AreEqual("Admin", p.GetUserRole(u).Name);
        }

        [TestMethod]
        public void addDynamicRoleAndAssignTest()
        {
            var added = p.RoleRegistry.AddRole("Analyst", new[] { Permissions.ExecuteTest, Permissions.CreateTest });
            Assert.IsTrue(added);
            Assert.IsTrue(p.AssignUserRole(u, "Analyst"));
            Assert.AreEqual("Analyst", p.GetUserRole(u).Name);
        }

        [TestMethod]
        public void getUserPermissionsTest()
        {
            p.AssignUserRole(u, "Lead");
            var perms = p.GetUserPermissions(u);
            CollectionAssert.AreEquivalent(new[] { Permissions.ExecuteTest, Permissions.CreateTest, Permissions.ManageProject }, (System.Collections.ICollection)perms);
        }
    }
}
