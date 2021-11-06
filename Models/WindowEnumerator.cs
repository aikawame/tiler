using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace Naorai.Models
{
  public class WindowEnumerator
  {
    private static readonly string[] IgnoredProcessNames = { "Naorai", "ApplicationFrameHost", "SystemSettings" };

    private static List<Window> _windows;

    public static Window GetActiveWindow()
    {
      EnumWindow(NativeMethods.GetActiveWindow(), IntPtr.Zero);

      return _windows.First();
    }

    public static List<Window> GetAllWindows()
    {
      _windows = new List<Window>();
      NativeMethods.EnumWindows(EnumWindow, IntPtr.Zero);

      return _windows.OrderBy(window => window.ProcessName).ToList();
    }

    private static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
    {
      string title = _GetTitle(hWnd);
      if (title == "") return true;

      Process process = _GetProcess(hWnd);
      if (IgnoredProcessNames.Contains(process.ProcessName)) return true;
      if (process.ProcessName == "explorer" && title == "Program Manager") return true;

      Rect rect = _GetRect(hWnd);

      var window = new Window();
      window.ProcessName = process.ProcessName;
      window.Title = title;
      window.Rect = rect;
      window.Handler = hWnd;
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
      uint processId;
      NativeMethods.GetWindowThreadProcessId(hWnd, out processId);

      return Process.GetProcessById((int)processId);
    }

    private static Rect _GetRect(IntPtr hWnd)
    {
      NativeMethods.Rect win32Rect;
      NativeMethods.GetWindowRect(hWnd, out win32Rect);

      var rect = new Rect();
      rect.Width = win32Rect.right - win32Rect.left;
      rect.Height = win32Rect.bottom - win32Rect.top;
      rect.X = win32Rect.left;
      rect.Y = win32Rect.top;

      return rect;
    }
  }
}
