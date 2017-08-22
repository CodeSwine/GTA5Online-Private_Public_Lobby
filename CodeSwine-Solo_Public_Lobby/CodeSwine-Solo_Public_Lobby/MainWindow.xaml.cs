using CodeSwine_Solo_Public_Lobby.DataAccess;
using CodeSwine_Solo_Public_Lobby.Helpers;
using CodeSwine_Solo_Public_Lobby.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace CodeSwine_Solo_Public_Lobby
{
    public partial class MainWindow : Window
    {
        private IPTool iPTool = new IPTool();
        private DaWhitelist whiteList = new DaWhitelist();
        List<IPAddress> addresses = new List<IPAddress>();
        MWhitelist mWhitelist = new MWhitelist();

        bool set = false;
        bool active = false;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            FirewallRule.DeleteRules();
            lblYourIPAddress.Content += " " + iPTool.GrabInternetAddress() + ".";
            addresses = whiteList.IpAddressess;
            lsbAddresses.ItemsSource = addresses;
            foreach (IPAddress ip in addresses)
            {
                mWhitelist.Ips.Add(ip.ToString());
            }
            lblAmountIPs.Content = whiteList.IpAddressess.Count() + " IPs whitelisted!";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(iPTool.ValidateIPv4(txbIpToAdd.Text))
            {
                if(!addresses.Contains(IPAddress.Parse(txbIpToAdd.Text)))
                {
                    addresses.Add(IPAddress.Parse(txbIpToAdd.Text));
                    lsbAddresses.Items.Refresh();
                    mWhitelist.Ips.Add(txbIpToAdd.Text);
                    DaWhitelist.SaveToJson(mWhitelist);
                    set = false; active = false;
                    FirewallRule.DeleteRules();
                    lblAmountIPs.Content = whiteList.IpAddressess.Count() + " IPs whitelisted!";
                    UpdateNotActive();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(lsbAddresses.SelectedIndex != -1)
            {
                mWhitelist.Ips.Remove(lsbAddresses.SelectedItem.ToString());
                addresses.Remove(IPAddress.Parse(lsbAddresses.SelectedItem.ToString()));
                lsbAddresses.Items.Refresh();
                DaWhitelist.SaveToJson(mWhitelist);
                set = false; active = false;
                FirewallRule.DeleteRules();
                lblAmountIPs.Content = whiteList.IpAddressess.Count() + " IPs whitelisted!";
                UpdateNotActive();
            }
        }

        private void btnEnableDisable_Click(object sender, RoutedEventArgs e)
        {
            SetRules();
        }

        void SetRules()
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
            if(active && set)
            {
                FirewallRule.CreateInbound(remoteAddresses, false, true);
                FirewallRule.CreateOutbound(remoteAddresses, false, true);
                UpdateNotActive();
                active = false;
            }
        }

        void UpdateNotActive()
        {
            btnEnableDisable.Background = ColorBrush.Red;
            image4.Source = new BitmapImage(new Uri("/CodeSwine-Solo_Public_Lobby;component/ImageResources/unlocked.png", UriKind.Relative));
            lblLock.Content = "Rules not active." + Environment.NewLine + "Click to activate!";
        }

        void UpdateActive()
        {
            btnEnableDisable.Background = ColorBrush.Green;
            image4.Source = new BitmapImage(new Uri("/CodeSwine-Solo_Public_Lobby;component/ImageResources/locked.png", UriKind.Relative));
            lblLock.Content = "Rules active." + Environment.NewLine + "Click to deactivate!";
        }
    }
}
