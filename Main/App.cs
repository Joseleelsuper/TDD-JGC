using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Main
{
    public class App
    {
        private readonly User _user;
        private readonly Dictionary<Project, HashSet<Role>> _participations = new Dictionary<Project, HashSet<Role>>();

        public App(User user)
        {
            _user = user;
        }

        public User User => _user;

        /*
         * Añade un rol para el usuario en el proyecto especificado.
         */
        public bool AddProjectRole(Project project, Role role)
        {
            if (project == null || role == null) return false;
            if (!_participations.TryGetValue(project, out var roles))
            {
                roles = new HashSet<Role>(new RoleNameComparer());
                _participations[project] = roles;
            }

            return roles.Add(role);
        }

        /*
         * Añade varios roles para el usuario en el proyecto especificado.
         * Devuelve el número de roles añadidos (sin contar los duplicados).
         */
        public int AddProjectRoles(Project project, IEnumerable<Role> roles)
        {
            if (project == null || roles == null) return 0;
            int added = 0;
            foreach (var r in roles)
            {
                if (AddProjectRole(project, r)) added++;
            }
            return added;
        }

        /*
         * Obtiene los roles del usuario en el proyecto especificado.
         */
        public IReadOnlyCollection<Role> GetRoles(Project project)
        {
            if (project == null) return Array.Empty<Role>();
            if (_participations.TryGetValue(project, out var roles))
                return roles.ToList().AsReadOnly();
            return Array.Empty<Role>();
        }

        /*
         * Obtiene los proyectos en los que el usuario participa.
         */
        public IReadOnlyCollection<Project> GetProjects()
        {
            return _participations.Keys.ToList().AsReadOnly();
        }

        public string GetProjectRolesDisplay(Project project)
        {
            if (project == null) return string.Empty;
            var roles = GetRoles(project);
            var rolesList = roles.Select(r => r.Name).OrderBy(n => n).ToArray();
            return rolesList.Length == 0 ? project.Name + ": (sin roles)" : project.Name + ": " + string.Join(", ", rolesList);
        }

        public string GetAllParticipationsDisplay()
        {
            if (_participations.Count == 0) return _user.Name + " " + _user.Surnames + " no participa en proyectos.";
            var lines = new List<string>();
            foreach (var kv in _participations)
            {
                lines.Add(GetProjectRolesDisplay(kv.Key));
            }
            return string.Join(Environment.NewLine, lines);
        }

        private class RoleNameComparer : IEqualityComparer<Role>
        {
            public bool Equals(Role x, Role y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x == null || y == null) return false;
                return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
            }
            public int GetHashCode(Role obj)
            {
                return obj?.Name == null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
            }
        }
    }
}
