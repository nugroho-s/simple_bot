using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_bot
{
    class Bot
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetCursorPos(int x, int y);

        public struct POINT
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void DoXMouseClick(int x)
        {
            for (int i=0;i< x; i++)
            {
                DoMouseClick();
            }
        }

        public static void mouse_move(int xDelta=0, int yDelta=0)
        {
            SetCursorPos(xDelta, yDelta);
        }

        public static Point CurrentMouse()
        {
            return Cursor.Position;
        }

        public static Point get_loc(String x)
        {
            Process[] processlist = Process.GetProcesses();
            Process p = processlist[0];
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    if (process.MainWindowTitle == "LINE")
                    {
                        p = process;
                    }
                }
            }
            IntPtr ptr = p.MainWindowHandle;
            Rect xRect = new Rect();
            GetWindowRect(ptr, ref xRect);
            Point ret = new Point(xRect.Left, xRect.Top);
            return ret;
        }

        public static void move_to(String x)
        {
            Point abs = get_loc(x);
            mouse_move(abs.X, abs.Y);
        }
    }
}
