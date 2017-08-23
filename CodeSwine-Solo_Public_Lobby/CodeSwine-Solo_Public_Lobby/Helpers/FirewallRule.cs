using NetFwTypeLib;
using System;
using System.Windows;

namespace CodeSwine_Solo_Public_Lobby.Helpers
{
    public class FirewallRule
    {
        /// <summary>
        /// Sets, Removes or Toggles CodeSwine Outbound firewall rules.
        /// </summary>
        /// <param name="addresses">Scope to block.</param>
        /// <param name="enabled">True to enable, false to disable the rule.</param>
        /// <param name="toggle">True to prevent adding or removing the rule again.</param>
        public static void CreateOutbound(string addresses, bool enabled, bool toggle)
        {
            try
            {
                INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                firewallRule.Protocol = 17;
                firewallRule.Enabled = enabled;
                firewallRule.InterfaceTypes = "All";
                firewallRule.RemoteAddresses = addresses;
                firewallRule.LocalPorts = "6672";
                firewallRule.Name = "GTA5 CodeSwine - Private Public Lobby Outbound";
                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

                if (!toggle)
                {
                    firewallPolicy.Rules.Add(firewallRule);
                }
                else
                {
                    firewallPolicy.Rules.Remove(firewallRule.Name);
                    firewallPolicy.Rules.Add(firewallRule);
                }

            } catch (Exception e)
            {
                ErrorLogger.LogException(e);
                MessageBox.Show("Please start this program as administrator!");
            }
        }

        /// <summary>
        /// Sets, Removes or Toggles CodeSwine Inbound firewall rules.
        /// </summary>
        /// <param name="addresses">Scope to block.</param>
        /// <param name="enabled">True to enable, false to disable the rule.</param>
        /// <param name="toggle">True to prevent adding or removing the rule again.</param>
        public static void CreateInbound(string addresses, bool enabled, bool toggle)
        {
            try
            {
                INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                firewallRule.Protocol = 17;
                firewallRule.Enabled = enabled;
                firewallRule.InterfaceTypes = "All";
                if(!string.IsNullOrEmpty(addresses))
                {
                    firewallRule.RemoteAddresses = addresses;
                }
                
                Console.WriteLine(addresses);

                firewallRule.LocalPorts = "6672";
                firewallRule.Name = "GTA5 CodeSwine - Private Public Lobby Inbound";
                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

                if(!toggle)
                {
                    firewallPolicy.Rules.Add(firewallRule);
                } else
                {
                    firewallPolicy.Rules.Remove(firewallRule.Name);
                    firewallPolicy.Rules.Add(firewallRule);
                }

            }
            catch (Exception e)
            {
                ErrorLogger.LogException(e);
            }
        }

        /// <summary>
        /// Removes CodeSwine Inbound & Outbound firewall rules at program startup.
        /// </summary>
        public static void DeleteRules()
        {
            try {
                INetFwRule firewallRuleInbound = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRuleInbound.Name = "GTA5 CodeSwine - Private Public Lobby Inbound";

                INetFwRule firewallRuleOutbound = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRuleOutbound.Name = "GTA5 CodeSwine - Private Public Lobby Outbound";

                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

                firewallPolicy.Rules.Remove(firewallRuleInbound.Name);
                firewallPolicy.Rules.Remove(firewallRuleOutbound.Name);
            } catch (Exception e)
            {
                ErrorLogger.LogException(e);
                MessageBox.Show("Run this program as administrator!");
            }
        }
    }
}   