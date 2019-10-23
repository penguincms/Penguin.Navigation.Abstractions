using System.Collections.Generic;

namespace Penguin.Navigation.Abstractions
{
    public interface INavigationMenu<TNav> : INavigationMenu where TNav : INavigationMenu
    {
        new IList<TNav> Children { get; }

        new TNav Parent { get; set; }
    }

    public interface INavigationMenu
    {
        IList<INavigationMenu> Children { get; }

        string Href { get; }

        string Icon { get; }

        string Name { get; set; }

        int Ordinal { get; set; }

        INavigationMenu Parent { get; }

        string Text { get; }

        string Uri { get; set; }
    }
}