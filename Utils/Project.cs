using System.Collections.Generic;

namespace Utils
{
    // Los roles y permisos acá serían los por defecto que también tiene TestLink, como ejemplo.
    public enum Roles
    {
        Viewer,
        Tester,
        Lead,
        Admin
    }

    public enum Permissions
    {
        ExecuteTest,
        CreateTest,
        ManageProject,
        ManageUsers
    }

    public static class RolePermissions
    {
        private static readonly Dictionary<Roles, List<Permissions>> _map = new Dictionary<Roles, List<Permissions>>
        {
            { Roles.Viewer, new List<Permissions>{ Permissions.ExecuteTest } },
            { Roles.Tester, new List<Permissions>{ Permissions.ExecuteTest, Permissions.CreateTest } },
            { Roles.Lead, new List<Permissions>{ Permissions.ExecuteTest, Permissions.CreateTest, Permissions.ManageProject } },
            { Roles.Admin, new List<Permissions>{ Permissions.ExecuteTest, Permissions.CreateTest, Permissions.ManageProject, Permissions.ManageUsers } }
        };

        public static IList<Permissions> GetPermissions(Roles role) => _map[role].AsReadOnly();
    }

    public class Project
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Prefix { get; private set; }
        public string Description { get; private set; }
        public bool IsPublic { get; private set; }

        private Dictionary<User, Roles> _userRoles = new Dictionary<User, Roles>();
        private static int _idSeq = 0;

        public Project(string name, string prefix, string description, bool isPublic)
        {
            Id = NextId();
            Name = name;
            Prefix = prefix;
            Description = description;
            IsPublic = isPublic;
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
            // Validar solo letras mayúsculas y dígitos, longitud 2..6
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

        public bool AssignUserRole(User user, Roles role)
        {
            if (user == null) return false;
            _userRoles[user] = role;
            return true;
        }

        public Roles GetUserRole(User user)
        {
            if (user != null && _userRoles.TryGetValue(user, out var role)) return role;
            return Roles.Viewer;
        }
    }
}