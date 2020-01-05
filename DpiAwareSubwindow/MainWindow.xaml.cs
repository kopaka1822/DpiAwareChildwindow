using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpDX.Mathematics.Interop;

namespace DpiAwareSubwindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChildWindow child;
        private float childColor = 1.0f;

        public MainWindow()
        {
            InitializeComponent();

        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            child = new ChildWindow(BorderHost);
            BorderHost.Child = child;
        }

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (child == null) return;
            if (child.SwapChain == null) return;

            // switch between black and white background
            childColor = 1.0f - childColor;

            // clear child background
            child.SwapChain.BeginFrame();
            Device.Get().ClearRenderTargetView(child.SwapChain.Rtv, new RawColor4(childColor, childColor, childColor, childColor));
            child.SwapChain.EndFrame();
        }
    }
}
