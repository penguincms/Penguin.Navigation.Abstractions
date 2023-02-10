using System;
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Navigation.Abstractions.Extensions
{
    public static class INavigationMenuItemExtensions
    {
        public static void AddChild<T>(this T parent, T child) where T : INavigationMenu<T>
        {
            if (child == null)
            {
                throw new System.ArgumentNullException(nameof(child));
            }

            if (child.Parent != null)
            {
                _ = child.Parent.Children.Remove(child);
            }

            parent.Children.Add(child);
            child.Parent = parent;
            parent.UpdateProperties();
        }

        public static void AddChildren<T>(this T parent, IList<T> children) where T : INavigationMenu<T>
        {
            if (children is null)
            {
                throw new System.ArgumentNullException(nameof(children));
            }

            foreach (T child in children)
            {
                parent.AddChild(child);
            }
        }

        public static T Merge<T>(this T target, T source, Func<T, IEnumerable<T>> GetChildren = null) where T : INavigationMenu<T>
        {
            if (GetChildren != null)
            {
                target.Children.Clear();

                foreach (T child in GetChildren.Invoke(target).ToList())
                {
                    target.Children.Add(child);
                }
            }

            foreach (T thisNav in source.Children.ToList())
            {
                INavigationMenu<T> existingNav = target.Children.Where(n => n.Uri == thisNav.Uri).FirstOrDefault();

                if (existingNav == null)
                {
                    target.AddChild(thisNav);
                }
                else
                {
                    _ = Merge((T)existingNav, thisNav, GetChildren);
                }
            }

            return target;
        }

        public static void RemoveChild<T>(this T parent, T child) where T : INavigationMenu<T>
        {
            if (child == null)
            {
                throw new System.ArgumentNullException(nameof(child));
            }

            child.Parent = default;
            _ = parent.Children.Remove(child);
            child.UpdateProperties();
        }

        public static void UpdateProperties<T>(this T parent) where T : INavigationMenu<T>
        {
            if (string.IsNullOrWhiteSpace(parent.Name))
            {
                parent.Name = new string(parent.Text.Where(char.IsLetterOrDigit).ToArray());
            }

            parent.Uri = parent.Parent == null ? "/" + parent.Name : parent.Parent.Uri + "/" + parent.Name;

            foreach (T child in parent.Children)
            {
                child.Parent = parent;
                child.Ordinal = parent.Children.IndexOf(child);
                child.UpdateProperties();
            }
        }
    }
}