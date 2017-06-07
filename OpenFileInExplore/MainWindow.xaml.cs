using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Application = System.Windows.Application;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace OpenFileInExplore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon;
        public MainWindow()
        {
            InitializeComponent();
            
            var OpenCmdBinding = new CommandBinding(
                    ApplicationCommands.Paste,
                    OpenCmdExecuted,
                    OpenCmdCanExecute);
            txtFilePath.CommandBindings.Add(OpenCmdBinding);

            txtFilePath.Focus();
            InitNotifyIcon();
        }

        void OpenCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            txtFilePath.Text = System.Windows.Clipboard.GetText();
            btnOpen_Click(null, null);
            txtFilePath.SelectAll();
        }

        void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #region Form Events
        private void Window_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Escape))
            {
                HideWindow();
            }
            else if (e.Key.Equals(Key.Enter))
            {
                btnOpen_Click(null, null);
            }
            
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            string filePath = txtFilePath.Text;
            if (!string.IsNullOrEmpty(filePath) && !filePath.Equals("Input folder or file path here..."))
            {
                System.Diagnostics.Process.Start("Explorer", "/select," + filePath);   
            }
        }

        #region TextBox获得焦点自动选中所有文本
        private void txtFilePath_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                TextBox tbx = sender as TextBox;
                tbx.SelectAll();
                tbx.PreviewMouseDown -= new MouseButtonEventHandler(txtFilePath_PreviewMouseDown);

            }
        }

        private void txtFilePath_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                tb.PreviewMouseDown += new MouseButtonEventHandler(txtFilePath_PreviewMouseDown);
            }
        }

        private void txtFilePath_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                tb.Focus();
                e.Handled = true;
            }
        }
        #endregion

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //KeyBinding vBinding = new KeyBinding(ApplicationCommands.Paste, new KeyGesture(Key.V, ModifierKeys.Control));
            //txtFilePath.InputBindings.Add(vBinding);
        }

      

        private void txtFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
           // btnOpen_Click(null, null);
        }

        #endregion

        #region NotifyIcon

        private void InitNotifyIcon()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "OpienFleInExplorer";
            this.notifyIcon.ShowBalloonTip(2000);
            this.notifyIcon.Text = "OpienFleInExplorer";
            this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            this.notifyIcon.Visible = true;
            //打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("Open");
            open.Click += new EventHandler(ShowWindow);
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += new EventHandler(btnExit_Click);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == MouseButtons.Left) ShowWindow(null, null);
            });
        }

        private void ShowWindow(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            this.Activate();
            //this.Topmost = true;  // important
            //this.Topmost = false; // important
            //this.Focus(); 
        }

        private void HideWindow()
        {
            this.ShowInTaskbar = false;
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region HotKey
         /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr m_Hwnd = new IntPtr();

        /// <summary>
        /// WPF窗体的资源初始化完成，并且可以通过WindowInteropHelper获得该窗体的句柄用来与Win32交互后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // 获取窗体句柄
            m_Hwnd = new WindowInteropHelper(this).Handle;
            HwndSource hWndSource = HwndSource.FromHwnd(m_Hwnd);
            // 添加处理程序
            if (hWndSource != null) hWndSource.AddHook(WndProc);
        }

         /// <summary>
        /// 窗体回调函数，接收所有窗体消息的事件处理函数
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            //switch (msg)
            //{
            //    case HotKeyManager.WM_HOTKEY:
            //        int sid = wideParam.ToInt32();


            //        handled = true;
            //        break;

            //}
            //return IntPtr.Zero;

            // TODO upper code does not work .......
            if (!this.IsActive)
            {
                if (wideParam.ToString().Equals(HotKeyManager.WM_ATOM_IDENTIFIER.ToString()))
                {
                    //if (!this.IsVisible)
                    //{
                        ShowWindow(null, null);
                    //}
                    //else
                    //{
                    //    HideWindow();
                    //}
                }
            }
            
             return IntPtr.Zero;
        }

        /// <summary>
        /// 所有控件初始化完成后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            // 注册热键
            InitHotKey();
        }

        /// <summary>
        /// 初始化注册快捷键
        /// </summary>
        /// <returns>true:保存快捷键的值；false:弹出设置窗体</returns>
        private bool InitHotKey()
        {
            if (HotKeyManager.GlobalFindAtom(HotKeyManager.WM_ATOM_STR) != 0)
            {
                HotKeyManager.GlobalDeleteAtom(HotKeyManager.GlobalFindAtom(HotKeyManager.WM_ATOM_STR));
            }
            // 获取唯一标识符
            HotKeyManager.WM_ATOM_IDENTIFIER = HotKeyManager.GlobalAddAtom(HotKeyManager.WM_ATOM_STR);
            return RegisterHotKey();
        }

        private bool RegisterHotKey()
        {
            //Last tow params, see below two page
            //  https://msdn.microsoft.com/en-us/library/windows/desktop/ms646309(v=vs.85).aspx
            //  https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx
            return HotKeyManager.RegisterHotKey(m_Hwnd, HotKeyManager.WM_ATOM_IDENTIFIER, 0x0002, 0x20);
            //return HotKeyManager.RegisterHotKey(m_Hwnd, HotKeyManager.WM_HOTKEY, 0x0002, 0x20);
        }

        private void UnregisterHotKey()
        {
            // 注销旧的热键
            HotKeyManager.UnregisterHotKey(m_Hwnd, HotKeyManager.WM_ATOM_IDENTIFIER);
        }
        #endregion

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UnregisterHotKey();
        }
    }
}
