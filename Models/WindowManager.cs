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
    private ObservableCollection<ActiveWindow> CurrentWindows = new ObservableCollection<ActiveWindow>();

    private ObservableCollection<Window> Windows = new ObservableCollection<Window>();

    private static readonly string[] IgnoredProcessNames = { "Naorai", "ApplicationFrameHost", "SystemSettings" };

    public WindowManager()
    {
      NativeMethods.EnumWindows(EnumWindowProcess, IntPtr.Zero);
    }

    public ObservableCollection<ActiveWindow> GetActiveWindows()
    {
      return CurrentWindows;
    }

    public ObservableCollection<Window> GetWindows()
    {
      return Windows;
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
      rect.Y = win32Rect.bottom;

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

      ActiveWindow activeWindow = new ActiveWindow();
      activeWindow.ProcessName = process.ProcessName;
      activeWindow.Title = title;
      activeWindow.Rect = rect;
      activeWindow.Handler = hWnd;
      CurrentWindows.Add(activeWindow);

      Window window = new Window();
      window.ProcessName = process.ProcessName;
      window.Title = title;
      window.Rect = rect;
      Windows.Add(window);

      return true;
    }
  }
}
