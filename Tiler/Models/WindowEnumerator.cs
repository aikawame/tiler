using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace Tiler.Models;

public static class WindowEnumerator
{
  private static readonly string[] IgnoredProcessNames =
  {
    "Tiler",
    "ApplicationFrameHost",
    "SystemSettings",
    "TextInputHost"
  };

  private static List<Window> _windows;

  static WindowEnumerator()
  {
    _windows = new List<Window>();
  }

  public static Window GetActiveWindow()
  {
    EnumWindow(NativeMethods.GetActiveWindow(), IntPtr.Zero);

    return _windows.First();
  }

  public static List<Window> GetAllWindows()
  {
    _windows = new List<Window>();
    NativeMethods.EnumWindows(EnumWindow, IntPtr.Zero);

    return _windows.OrderBy(window => window.ProcessName.Value).ToList();
  }

  private static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
  {
    string title = _GetTitle(hWnd);
    if (title == "") return true;

    Process process = _GetProcess(hWnd);
    if (IgnoredProcessNames.Contains(process.ProcessName)) return true;
    if (process.ProcessName == "explorer" && title == "Program Manager") return true;

    Rect rect = _GetRect(hWnd);

    var window = new Window
    {
      ProcessName = { Value = process.ProcessName },
      Title       = { Value = title },
      X           = { Value = (int)rect.X },
      Y           = { Value = (int)rect.Y },
      Width       = { Value = (int)rect.Width },
      Height      = { Value = (int)rect.Height },
      Handler     = hWnd
    };
    _windows.Add(window);

    return true;
  }

  private static string _GetTitle(IntPtr hWnd)
  {
    if (NativeMethods.IsWindowVisible(hWnd) == false) return "";

    int textLength = NativeMethods.GetWindowTextLength(hWnd);
    if (textLength == 0) return "";

    var title = new StringBuilder(textLength + 1);
    NativeMethods.GetWindowText(hWnd, title, title.Capacity);

    return title.ToString();
  }

  private static Process _GetProcess(IntPtr hWnd)
  {
    NativeMethods.GetWindowThreadProcessId(hWnd, out var processId);

    return Process.GetProcessById((int)processId);
  }

  private static Rect _GetRect(IntPtr hWnd)
  {
    NativeMethods.GetWindowRect(hWnd, out var win32Rect);

    var rect = new Rect
    {
      Width  = win32Rect.right - win32Rect.left,
      Height = win32Rect.bottom - win32Rect.top,
      X      = win32Rect.left,
      Y      = win32Rect.top
    };

    return rect;
  }
}
