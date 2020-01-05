using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace DpiAwareSubwindow
{
    public class ChildWindow : HwndHost
    {
        private readonly Border parent;
        private IntPtr hWnd = IntPtr.Zero;

        public SwapChain SwapChain { get; private set; }

        public ChildWindow(Border parent)
        {
            this.parent = parent;
            parent.SizeChanged += ParentOnSizeChanged;
        }

        private void ParentOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // problem: width and height is not correctly scaled
            SwapChain?.Resize((int)(parent.ActualWidth), (int)(parent.ActualHeight));
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            // create subwindow
            hWnd = CreateWindowEx(
                0, // dwstyle
                "static", // class name
                "", // window name
                WS_CHILD | WS_VISIBLE, // style
                0, // x
                0, // y
                (int)parent.ActualWidth, // renderWidth
                (int)parent.ActualHeight, // renderHeight
                hwndParent.Handle, // parent handle
                IntPtr.Zero, // menu
                IntPtr.Zero, // hInstance
                0 // param
            );

            // directx swap chain
            // problem: width and height is not correctly scaled
            SwapChain = new SwapChain(hWnd, (int)parent.ActualWidth, (int)parent.ActualHeight);

            return new HandleRef(this, hWnd);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyWindow(hWnd);
        }

        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        internal static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateWindowEx(
            int dwExStyle,
            string lpszClassName,
            string lpszWindowName,
            int style,
            int x, int y,
            int width, int height,
            IntPtr hwndParent,
            IntPtr hMenu,
            IntPtr hInst,
            [MarshalAs(UnmanagedType.AsAny)] object pvParam
        );

        internal const int
            WS_CHILD = 0x40000000,
            WS_VISIBLE = 0x10000000;
    }
}
