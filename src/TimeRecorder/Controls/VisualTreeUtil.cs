using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace TimeRecorder.Controls;
internal static class VisualTreeUtil
{
    public static T FindParent<T>(this FrameworkElement child) where T : DependencyObject
    {
        T parent = null;
        var currentParent = VisualTreeHelper.GetParent(child);

        while (currentParent != null)
        {

            // check the current parent
            if (currentParent is T)
            {
                parent = (T)currentParent;
                break;
            }

            // find the next parent
            currentParent = VisualTreeHelper.GetParent(currentParent);
        }

        return parent;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static T FindChild<T>(this FrameworkElement parent) where T : DependencyObject
    {
        T child = null;

        var count = VisualTreeHelper.GetChildrenCount(parent);
        if (count == 0)
        {
            return null;
        }

        // check the children
        for (var i = 0; i < count; i++)
        {
            var currentChild = VisualTreeHelper.GetChild(parent, i);
            if (currentChild is T)
            {
                child = (T)currentChild;
                break;
            }

            // iterate over child sub-tree if nothing is yet found
            currentChild = FindChild<T>((FrameworkElement)currentChild);
            if (currentChild != null)
            {
                child = (T)currentChild;
                break;
            }
        }

        return child;
    }


    public static void PrintVisualTree(DependencyObject obj)
    {
        Debug.WriteLine("PrintVisualTree");
        PrintVisualTreeCore(0, obj);
    }

    // VisualTreeを表示する。
    // DependencyObjectの場合はVisualTree上の子要素も再帰的に出力していく
    private static void PrintVisualTreeCore(int level, DependencyObject obj)
    {
        PrintObject(level, obj);
        foreach (var child in GetVisualChildren(obj))
        {
            if (child is DependencyObject)
            {
                PrintVisualTreeCore(level + 1, (DependencyObject)child);
            }
            else
            {
                PrintObject(level + 1, child);
            }
        }
    }

    // VisualTreeの子要素の列挙を返す
    private static IEnumerable<object> GetVisualChildren(DependencyObject obj)
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            yield return VisualTreeHelper.GetChild(obj, i);
        }
    }

    // ToStringの結果をインデントつきで出力
    private static void PrintObject(int level, object obj)
    {
        Debug.WriteLine(new string('\t', level) + obj);
    }
}
