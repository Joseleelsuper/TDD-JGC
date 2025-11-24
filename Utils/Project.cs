using System.Collections.Generic;

namespace Utils
{
    public enum Permissions
    {
        ExecuteTest,
        CreateTest,
        ManageProject,
        ManageUsers
    }

    public class Project
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Prefix { get; private set; }
        public string Description { get; private set; }
        public bool IsPublic { get; private set; }

        public DynamicRoleRegistry RoleRegistry { get; } = new DynamicRoleRegistry();

        private Dictionary<User, Role> _userRoles = new Dictionary<User, Role>();
        private static int _idSeq = 0;

        public Project(string name, string prefix, string description, bool isPublic)
        {
            Id = NextId();
            Name = name;
            Prefix = prefix;
            Description = description;
            IsPublic = isPublic;
            // Cargar roles por defecto.
            RoleRegistry.AddRole("Viewer", new [] { Permissions.ExecuteTest });
            RoleRegistry.AddRole("Tester", new [] { Permissions.ExecuteTest, Permissions.CreateTest });
            RoleRegistry.AddRole("Lead", new [] { Permissions.ExecuteTest, Permissions.CreateTest, Permissions.ManageProject });
            RoleRegistry.AddRole("Admin", new [] { Permissions.ExecuteTest, Permissions.CreateTest, Permissions.ManageProject, Permissions.ManageUsers });
        }

        private int NextId() => ++_idSeq;

        public bool SetName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) return false;
            Name = newName;
            return true;
        }

        public bool SetPrefix(string newPrefix)
        {
            if (string.IsNullOrWhiteSpace(newPrefix)) return false;
            if (newPrefix.Length < 2 || newPrefix.Length > 6) return false;
            foreach (var c in newPrefix)
            {
                if (!(char.IsUpper(c) || char.IsDigit(c))) return false;
            }
            Prefix = newPrefix;
            return true;
        }

        public bool SetDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription)) return false;
            Description = newDescription;
            return true;
        }

        public void SetIsPublic(bool isPublic)
        {
            IsPublic = isPublic;
        }
        public bool AssignUserRole(User user, string roleName)
        {
            if (user == null || string.IsNullOrWhiteSpace(roleName)) return false;
            var role = RoleRegistry.TryGetRole(roleName);
            if (role == null) return false;
            _userRoles[user] = role;
            return true;
        }

        public bool AssignUserRole(User user, Role role)
        {
            if (user == null || role == null) return false;
            _userRoles[user] = role;
            return true;
        }

        public Role GetUserRole(User user)
        {
            if (user != null && _userRoles.TryGetValue(user, out var role)) return role;
            // Rol por defecto
            return RoleRegistry.TryGetRole("Viewer");
        }

        public IReadOnlyCollection<Permissions> GetUserPermissions(User user)
        {
            var r = GetUserRole(user);
            return r != null ? r.Permissions : new Permissions[0];
        }
    }
}