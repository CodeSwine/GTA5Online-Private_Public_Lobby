using CodeSwine_Solo_Public_Lobby.DataAccess;
using CodeSwine_Solo_Public_Lobby.Helpers;
using CodeSwine_Solo_Public_Lobby.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CodeSwine_Solo_Public_Lobby
{
    public partial class MainWindow : Window
    {
        private IPTool iPTool = new IPTool();
        private List<IPAddress> addresses = new List<IPAddress>();
        private MWhitelist mWhitelist = new MWhitelist();

        private bool set = false;
        private bool active = false;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FirewallRule.lblAdmin = lblAdmin;
            await InitAsync();
        }

        private async Task InitAsync()
        {
            var ipAddress = await iPTool.GrabInternetAddressAsync();
            lblYourIPAddress.Content += " " + ipAddress + ".";
            addresses = DaWhitelist.ReadIPsFromJSON();
            lsbAddresses.ItemsSource = addresses;
            foreach (IPAddress ip in addresses)
            {
                mWhitelist.Ips.Add(ip.ToString());
            }
            SetIpCount();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IPTool.ValidateIP(txbIpToAdd.Text))
            {
                if (!addresses.Contains(IPAddress.Parse(txbIpToAdd.Text)))
                {
                    addresses.Add(IPAddress.Parse(txbIpToAdd.Text));
                    lsbAddresses.Items.Refresh();
                    mWhitelist.Ips.Add(txbIpToAdd.Text);
                    DaWhitelist.SaveToJson(mWhitelist);
                    set = false; active = false;
                    FirewallRule.DeleteRules();
                    SetIpCount();
                    UpdateNotActive();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lsbAddresses.SelectedIndex != -1)
            {
                mWhitelist.Ips.Remove(lsbAddresses.SelectedItem.ToString());
                addresses.Remove(IPAddress.Parse(lsbAddresses.SelectedItem.ToString()));
                lsbAddresses.Items.Refresh();
                DaWhitelist.SaveToJson(mWhitelist);
                set = false; active = false;
                FirewallRule.DeleteRules();
                SetIpCount();
                UpdateNotActive();
            }
        }

        private void SetIpCount()
        {
            lblAmountIPs.Content = addresses.Count() + " IPs whitelisted!";
        }

        private void btnEnableDisable_Click(object sender, RoutedEventArgs e)
        {
            SetRules();
        }

        private void SetRules()
        {
            string remoteAddresses = RangeCalculator.GetRemoteAddresses(addresses);

            // If the firewall rules aren't set yet.
            if (!set)
            {
                FirewallRule.CreateInbound(remoteAddresses, true, false);
                FirewallRule.CreateOutbound(remoteAddresses, true, false);
                active = true;
                set = true;
                UpdateActive();
                return;
            }

            // If they are set but not enabled.
            if (set && !active)
            {
                FirewallRule.CreateInbound(remoteAddresses, true, true);
                FirewallRule.CreateOutbound(remoteAddresses, true, true);
                active = true;
                UpdateActive();
                return;
            }

            // If they are active and set.
            if (active && set)
            {
                FirewallRule.CreateInbound(remoteAddresses, false, true);
                FirewallRule.CreateOutbound(remoteAddresses, false, true);
                UpdateNotActive();
                active = false;
            }
        }

        private void UpdateNotActive()
        {
            btnEnableDisable.Background = ColorBrush.Red;
            image4.Source = new BitmapImage(new Uri("/CodeSwine-Solo_Public_Lobby;component/ImageResources/unlocked.png", UriKind.Relative));
            lblLock.Content = "Rules not active." + Environment.NewLine + "Click to activate!";
        }

        private void UpdateActive()
        {
            btnEnableDisable.Background = ColorBrush.Green;
            image4.Source = new BitmapImage(new Uri("/CodeSwine-Solo_Public_Lobby;component/ImageResources/locked.png", UriKind.Relative));
            lblLock.Content = "Rules active." + Environment.NewLine + "Click to deactivate!";
        }

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
        [In] IntPtr hWnd,
        [In] int id,
        [In] uint fsModifiers,
        [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        private HwndSource _source;
        private const int HOTKEY_ID = 9000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
            RegisterHotKey();
        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
            FirewallRule.DeleteRules();
            base.OnClosed(e);
        }

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            const uint VK_F10 = 0x79;
            const uint MOD_CTRL = 0x0002;
            if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_F10))
            {
            }
        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            OnHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void OnHotKeyPressed()
        {
            SetRules();
            System.Media.SystemSounds.Hand.Play();
        }
    }
}