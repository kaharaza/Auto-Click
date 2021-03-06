using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace TFive_Auto_Click
{
    public class Win32Bot
    {

        #region DllImport

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr SetFocus(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern int SendMessage(
           IntPtr hWnd,
           int Msg,
           int wParam,
           int lParam);
        public static int MakeLParam(int LoWord, int HiWord)
        {
            return (HiWord << 16) | (LoWord & 0xFFFF);
        }
        [DllImport("User32.dll")]
        public static extern long SetCursorPos(int x, int y);
        [DllImport("User32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.dll")]


        #endregion

        #region Var

        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        public const int MOUSEEVENTF_COMMAND = 0x111;
        public const int MOUSEEVENTF_MOUSEMOVE = 0x0200;
        public const int MOUSEEVENTF_LBUTTONDOWN = 0x201;
        public const int MOUSEEVENTF_LBUTTONUP = 0x202;
        public const int MOUSEEVENTF_LBUTTONDBLCLK = 0x203;
        public const int MOUSEEVENTF_RBUTTONDOWN = 0x204;
        public const int MOUSEEVENTF_RBUTTONUP = 0x205;
        public const int MOUSEEVENTF_RBUTTONDBLCLK = 0x206;
        public const int MOUSEEVENTF_KEYDOWN = 0x100;
        public const int MOUSEEVENTF_KEYUP = 0x101;
        public const int MOUSEEVENTF_MOUSEWHEEL = 0x020A;
        public const int MOUSEEVENTF_MOUSEHWHEEL = 0x020E;
        public static int WinSizeWidth;
        public static int WinSizeHeight;
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        #endregion
        public static Size GetControlSize(IntPtr iHandle)
        {
            var cSize = new Size();
            GetWindowRect(iHandle, out var pRect);
            cSize.Width = pRect.Right - pRect.Left;
            cSize.Height = pRect.Bottom - pRect.Top;
            WinSizeWidth = cSize.Width;
            WinSizeHeight = cSize.Height;
            return cSize;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        public static void MouseClick(IntPtr iHandle, int Gx, int Gy, string TypeClick = "LEFT")
        {
            var p = new POINT {x = Convert.ToInt16(Gx), y = Convert.ToInt16(Gy)};
            ClientToScreen(iHandle, ref p);
            SetCursorPos(p.x, p.y);
            if (TypeClick == "LEFT")
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, p.x, p.y, 0, 0);
            else if (TypeClick == "RIGHT") mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, p.x, p.y, 0, 0);
        }
        public static void MouseMove(IntPtr iHandle, int x, int y)
        {
            POINT p = new POINT {x = Convert.ToInt16(x), y = Convert.ToInt16(y)};
            ClientToScreen(iHandle, ref p);
            SetCursorPos(p.x, p.y);
        }
        public static void ActiveWindow(IntPtr iHandle)
        {
            IntPtr h = iHandle;
            ShowWindow(h, SW_SHOW);
            SetForegroundWindow(h);
            SetFocus(h);
        }
        public static void HideApp(IntPtr iHandle)
        {
            IntPtr h = iHandle;
            ShowWindow(h, SW_HIDE);
        }
        public static void ShowAPP(IntPtr iHandle)
        {
            IntPtr h = iHandle;
            ShowWindow(h, SW_SHOW);
        }
        public static void MouseClickDragBG(IntPtr iHandle, int XStart, int YStart, int XEnd, int YEnd, int Delay)
        {
            SendMessage(iHandle, MOUSEEVENTF_LBUTTONDOWN, 0x00000001, MakeLParam(XEnd, YEnd));
            Sleep(Delay);
            SendMessage(iHandle, MOUSEEVENTF_MOUSEMOVE, 0x00000001, MakeLParam(XStart, YStart));
            Sleep(Delay);
            SendMessage(iHandle, MOUSEEVENTF_MOUSEMOVE, 0x00000001, MakeLParam(XStart,YStart));
            Sleep(Delay);
            SendMessage(iHandle, MOUSEEVENTF_LBUTTONUP, 0x00000001, MakeLParam(XEnd, YEnd));
            SendMessage(iHandle, MOUSEEVENTF_LBUTTONUP, 0x00000001, MakeLParam(XEnd, YEnd));
        }


        public static void ClickToBg(IntPtr iHandle, int x, int y, int clickCount = 1)
        {
            for (var i = 0; i < clickCount; i++)
            {
                SendMessage(iHandle, MOUSEEVENTF_LBUTTONDOWN, 0x00000001, MakeLParam(x, y));
                SendMessage(iHandle, MOUSEEVENTF_LBUTTONUP, 0x00000000, MakeLParam(x, y));
            }
            
        }
        public static void ClickHold(IntPtr iHandle, int x, int y, int slp)
        {
            var p = new POINT {x = Convert.ToInt16(x), y = Convert.ToInt16(y)};
            ClientToScreen(iHandle, ref p);
            SetCursorPos(p.x, p.y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, p.x, p.y, 0, 0);
            Sleep(slp);
            mouse_event(MOUSEEVENTF_LEFTUP, p.x, p.y, 0, 0);
        }
        public static void Sleep(int slp)
        {
            Thread.Sleep(slp);
        }

        public static void AwaitSleep(int slp)
        {
            var task = WaitMy(slp);

            task.Wait();
        }

        private static Task WaitMy(int delay)
        {
            return Task.Run(async () => await Task.Delay(delay));
        }

        public static void WindowMove(IntPtr iHandle, int x, int y)
        {
            GetControlSize(iHandle);
            MoveWindow(iHandle, x, y, WinSizeWidth, WinSizeHeight, true);
        }
        public static bool CheckProcess(string processName)
        {
            var statusCheck = false;
            var p = Process.GetProcessesByName(processName);
            if (p.Length > 0)
            {
                statusCheck = true;
            }
            return statusCheck;
        }
        public static string GetWinTitle(IntPtr iHandle)
        {
            const int nChars = 256;
            var buff = new StringBuilder(nChars);
            return GetWindowText(iHandle, buff, nChars) > 0 ? buff.ToString() : null;
        }

        public static void SendKeyBG(IntPtr iHandle, Keys key, int delay)
        {
            PostMessage(iHandle, WM_KEYDOWN, (int)key, 0);
            AwaitSleep(delay);
            PostMessage(iHandle, WM_KEYUP, (int)key, 0);
        }
        public static void OpenProgram(string path)
        {
            Process.Start(path);
        }
    }
}
