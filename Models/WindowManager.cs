using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace Naorai.Models
{
  public class WindowManager
  {
    private static readonly string[] IgnoredProcessNames = { "Naorai", "ApplicationFrameHost", "SystemSettings" };

    private ObservableCollection<Window> _windows;

    public ObservableCollection<Window> GetWindows()
    {
      _windows = new ObservableCollection<Window>();
      NativeMethods.EnumWindows(EnumWindowProcess, IntPtr.Zero);

      return _windows;
    }

    private static string GetTitle(IntPtr hWnd)
    {
      if (NativeMethods.IsWindowVisible(hWnd) == false) return "";

      int textLength = NativeMethods.GetWindowTextLength(hWnd);
      if (textLength == 0) return "";

      StringBuilder title = new StringBuilder(textLength + 1);
      NativeMethods.GetWindowText(hWnd, title, title.Capacity);

      return title.ToString();
    }

    private static Process GetProcess(IntPtr hWnd)
    {
      uint processId;
      NativeMethods.GetWindowThreadProcessId(hWnd, out processId);

      return Process.GetProcessById((int)processId);
    }

    private static Rect GetRect(IntPtr hWnd)
    {
      NativeMethods.Rect win32Rect;
      NativeMethods.GetWindowRect(hWnd, out win32Rect);

      Rect rect = new Rect();
      rect.Width = win32Rect.right - win32Rect.left;
      rect.Height = win32Rect.bottom - win32Rect.top;
      rect.X = win32Rect.left;
      rect.Y = win32Rect.top;

      return rect;
    }

    private bool EnumWindowProcess(IntPtr hWnd, IntPtr lParam)
    {
      string title = GetTitle(hWnd);
      if (title == "") return true;

      Process process = GetProcess(hWnd);
      if (IgnoredProcessNames.Contains(process.ProcessName)) return true;
      if (process.ProcessName == "explorer" && title == "Program Manager") return true;

      Rect rect = GetRect(hWnd);

      Window window = new Window();
      window.ProcessName = process.ProcessName;
      window.Title = title;
      window.Rect = rect;
      window.Handler = hWnd;
      _windows.Add(window);

      return true;
    }
  }
}
