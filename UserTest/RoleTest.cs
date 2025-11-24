using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UtilTest
{
    [TestClass]
    public class RoleTest
    {
        private DynamicRoleRegistry _registry;

        [TestInitialize]
        public void Setup()
        {
            _registry = new DynamicRoleRegistry();
        }

        [TestMethod]
        public void AddRole_Should_Add_New_Role()
        {
            var ok = _registry.AddRole("Analyst", new [] { Permissions.ExecuteTest, Permissions.CreateTest });
            Assert.IsTrue(ok, "AddRole debe devolver true para rol nuevo");
            var role = _registry.TryGetRole("Analyst");
            Assert.IsNotNull(role);
            CollectionAssert.AreEquivalent(new [] { Permissions.ExecuteTest, Permissions.CreateTest }, role.Permissions.ToList());
            Assert.AreEqual(1, _registry.Roles.Count);
        }

        [TestMethod]
        public void AddRole_Duplicate_Should_Fail()
        {
            Assert.IsTrue(_registry.AddRole("Analyst", new [] { Permissions.ExecuteTest }));
            Assert.IsFalse(_registry.AddRole("Analyst", new [] { Permissions.ExecuteTest }), "No debe permitir añadir el mismo nombre de rol dos veces");
            Assert.AreEqual(1, _registry.Roles.Count);
        }

        [TestMethod]
        public void RemoveRole_Should_Remove_Existing_Role()
        {
            _registry.AddRole("Analyst", new [] { Permissions.ExecuteTest });
            _registry.AddRole("LeadPlus", new [] { Permissions.ExecuteTest, Permissions.CreateTest, Permissions.ManageProject });
            Assert.AreEqual(2, _registry.Roles.Count);
            var removed = _registry.RemoveRole("Analyst");
            Assert.IsTrue(removed);
            Assert.AreEqual(1, _registry.Roles.Count);
            Assert.IsNull(_registry.TryGetRole("Analyst"));
        }

        [TestMethod]
        public void RemoveRole_NonExisting_Should_Return_False()
        {
            _registry.AddRole("Analyst", new [] { Permissions.ExecuteTest });
            var removed = _registry.RemoveRole("Ghost");
            Assert.IsFalse(removed);
            Assert.AreEqual(1, _registry.Roles.Count);
        }
    }
}
