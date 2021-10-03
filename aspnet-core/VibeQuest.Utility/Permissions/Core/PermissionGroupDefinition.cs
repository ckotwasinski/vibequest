﻿using VibeQuest.Utility.Extentions;
using VibeQuest.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public class PermissionGroupDefinition
    {
        /// <summary>
        /// Unique name of the group.
        /// </summary>
        public string Name { get; }

        public Dictionary<string, object> Properties { get; }

        public string DisplayName { get; set; }

        public IReadOnlyList<PermissionDefinition> Permissions => _permissions.ToImmutableList();
        private readonly List<PermissionDefinition> _permissions;

        /// <summary>
        /// Gets/sets a key-value on the <see cref="Properties"/>.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <returns>
        /// Returns the value in the <see cref="Properties"/> dictionary by given <see cref="name"/>.
        /// Returns null if given <see cref="name"/> is not present in the <see cref="Properties"/> dictionary.
        /// </returns>
        public object this[string name]
        {
            get => Properties.GetOrDefault(name);
            set => Properties[name] = value;
        }

        protected internal PermissionGroupDefinition(
            string name,
            string displayName = null)
        {
            Name = name;
            DisplayName = displayName;

            Properties = new Dictionary<string, object>();
            _permissions = new List<PermissionDefinition>();
        }

        public virtual PermissionDefinition AddPermission(
            string name,
            string displayName = null,
            bool isEnabled = true)
        {
            var permission = new PermissionDefinition(
                name,
                displayName,
                isEnabled
            );

            _permissions.Add(permission);

            return permission;
        }

        public virtual List<PermissionDefinition> GetPermissionsWithChildren()
        {
            var permissions = new List<PermissionDefinition>();

            foreach (var permission in _permissions)
            {
                AddPermissionToListRecursively(permissions, permission);
            }

            return permissions;
        }

        private void AddPermissionToListRecursively(List<PermissionDefinition> permissions, PermissionDefinition permission)
        {
            permissions.Add(permission);

            foreach (var child in permission.Children)
            {
                AddPermissionToListRecursively(permissions, child);
            }
        }

        public override string ToString()
        {
            return $"[{nameof(PermissionGroupDefinition)} {Name}]";
        }

        public PermissionDefinition GetPermissionOrNull(string name)
        {
            Check.NotNull(name, nameof(name));

            return GetPermissionOrNullRecursively(Permissions, name);
        }

        private PermissionDefinition GetPermissionOrNullRecursively(
            IReadOnlyList<PermissionDefinition> permissions, string name)
        {
            foreach (var permission in permissions)
            {
                if (permission.Name == name)
                {
                    return permission;
                }

                var childPermission = GetPermissionOrNullRecursively(permission.Children, name);
                if (childPermission != null)
                {
                    return childPermission;
                }
            }

            return null;
        }
    }
}
