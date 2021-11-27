using System.Collections.ObjectModel;
using SyncMe.Lib.Models;

namespace SyncMe.Lib.Extensions;

public static class NamespaceViewModelExtensions
{
    public static void RemoveAllChildren(this ObservableCollection<NamespaceViewModel> items, string fullName)
    {
        var itemToRemove = items.FirstOrDefault(n => n.FullName.Contains($"{fullName}."));

        if (itemToRemove is null) return;

        items.Remove(itemToRemove);
        items.RemoveAllChildren(fullName);
    }

    public static void ApplyToAllChildren(this ObservableCollection<NamespaceViewModel> items, string fullName, Action<NamespaceViewModel> action)
    {
        items.Where(n => n.FullName.Contains($"{fullName}.")).ToList().ForEach(action);
    }

    public static void CollapseAllButtons(this ObservableCollection<NamespaceViewModel> items)
    {
        foreach (var item in items)
        {
            item.IsSuspendButtonsVisible = false;
            item.IsRestoreButtonsVisible = false;
        }
    }

    public static void InsertRange(this ObservableCollection<NamespaceViewModel> items, int index, IList<(string FullName, bool IsActive, bool HasChildren)> children)
    {
        for (int i = 0; i < children.Count; i++)
        {
            items.Insert(index + i + 1, new NamespaceViewModel(children[i].FullName, children[i].IsActive, children[i].HasChildren));
        }
    }

    public static void RemoveAndInsertItem(this ObservableCollection<NamespaceViewModel> items, NamespaceViewModel item)
    {
        var index = items.IndexOf(item);
        items.RemoveAt(index);
        items.Insert(index, item);
    }
}
