using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using PInvoke;

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
    // FIXME: Can't get window from notification area.
    EnumWindow(User32.GetActiveWindow(), IntPtr.Zero);

    return _windows.First();
  }

  public static List<Window> GetAllWindows()
  {
    _windows = new List<Window>();
    User32.EnumWindows(EnumWindow, IntPtr.Zero);

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
    if (User32.IsWindowVisible(hWnd) == false) return "";

    int textLength = User32.GetWindowTextLength(hWnd);
    if (textLength == 0) return "";

    char[] buffer = new char[textLength + 1];
    int bufferLength = buffer.Length;
    User32.GetWindowText(hWnd, buffer, bufferLength);

    return new string(buffer);
  }

  private static Process _GetProcess(IntPtr hWnd)
  {
    User32.GetWindowThreadProcessId(hWnd, out var processId);

    return Process.GetProcessById(processId);
  }

  private static Rect _GetRect(IntPtr hWnd)
  {
    User32.GetWindowRect(hWnd, out var win32Rect);

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
