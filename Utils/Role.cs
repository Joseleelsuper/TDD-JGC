using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class Role
    {
        public string Name { get; }
        public IReadOnlyCollection<Permissions> Permissions { get; }

        internal Role(string name, IEnumerable<Permissions> permissions)
        {
            Name = name;
            Permissions = (permissions ?? Enumerable.Empty<Permissions>())
                .Distinct()
                .ToList()
                .AsReadOnly();
        }
    }

    public class DynamicRoleRegistry
    {
        private readonly Dictionary<string, Role> _roles = new Dictionary<string, Role>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<Role> Roles => _roles.Values.ToList().AsReadOnly();

        public bool AddRole(string name, IEnumerable<Permissions> permissions)
        {
            if (string.IsNullOrWhiteSpace(name) || _roles.ContainsKey(name)) return false;
            _roles[name] = new Role(name.Trim(), permissions);
            return true;
        }

        public bool RemoveRole(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            return _roles.Remove(name.Trim());
        }

        public Role TryGetRole(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            _roles.TryGetValue(name.Trim(), out var role);
            return role;
        }
    }
}
