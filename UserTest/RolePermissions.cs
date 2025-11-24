using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace UtilTest
{
    [TestClass]
    public class RolePermissions
    {
        private Dictionary<Roles, List<Permissions>> rolePermissions = null;

        [TestInitialize]
        public void Setup()
        {
            rolePermissions = new Dictionary<Roles, List<Permissions>>
            {
                { Roles.Viewer, new List<Permissions>{ Permissions.ExecuteTest } },
                { Roles.Tester, new List<Permissions>{ Permissions.ExecuteTest, Permissions.CreateTest } },
                { Roles.Lead, new List<Permissions>{ Permissions.ExecuteTest, Permissions.CreateTest, Permissions.ManageProject } },
                { Roles.Admin, new List<Permissions>{ Permissions.ExecuteTest, Permissions.CreateTest, Permissions.ManageProject, Permissions.ManageUsers } }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            rolePermissions = null;
        }

        [TestMethod]
        public void setupRolePermissionsTest()
        {
            foreach (var kv in rolePermissions)
            {
                var perms = Utils.RolePermissions.GetPermissions(kv.Key);
                CollectionAssert.AreEquivalent(kv.Value, perms.ToList());
            }
        }

        [TestMethod]
        public void checkPermissionsTest()
        {
            var testerPerms = Utils.RolePermissions.GetPermissions(Roles.Tester).ToList();
            Assert.IsTrue(testerPerms.Contains(Permissions.CreateTest));
            Assert.IsFalse(testerPerms.Contains(Permissions.ManageUsers));
        }

        [TestMethod]
        public void addRoleTest()
        {
            var roles = new List<Roles> { Roles.Viewer, Roles.Tester, Roles.Lead, Roles.Admin };
            Assert.AreEqual(4, roles.Count);
        }

        [TestMethod]
        public void removeRoleTest()
        {
            var roles = new List<Roles> { Roles.Viewer, Roles.Tester, Roles.Lead, Roles.Admin };
            Assert.AreEqual(4, roles.Count);
            roles.Remove(Roles.Lead);
            Assert.AreEqual(3, roles.Count);
            Assert.IsFalse(roles.Contains(Roles.Lead));
        }

        [TestMethod]
        public void addPermissionToRoleTest()
        {
            var leadPerms = Utils.RolePermissions.GetPermissions(Roles.Lead).ToList();
            Assert.IsFalse(leadPerms.Contains(Permissions.ManageUsers));
            leadPerms.Add(Permissions.ManageUsers);
            Assert.IsTrue(leadPerms.Contains(Permissions.ManageUsers));
        }

        [TestMethod]
        public void removePermissionFromRoleTest()
        {
            var adminPerms = Utils.RolePermissions.GetPermissions(Roles.Admin).ToList();
            Assert.IsTrue(adminPerms.Contains(Permissions.ManageUsers));
            adminPerms.Remove(Permissions.ManageUsers);
            Assert.IsFalse(adminPerms.Contains(Permissions.ManageUsers));
        }
    }
}
