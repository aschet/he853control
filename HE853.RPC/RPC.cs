/*
Home Easy HE853 Control
Copyright (C) 2012 Thomas Ascher

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

namespace HE853
{
    using System;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Ipc;
    using System.Security.Principal;

    public sealed class RPC
    {
        public const string ServiceArg = "/service";
        private const string ChannelName = "HE853";
        private const string InterfaceName = "Device";
        
        public static void RegisterServer()
        {
            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            NTAccount account = (NTAccount)sid.Translate(typeof(NTAccount)); 

            System.Collections.IDictionary properties = new System.Collections.Hashtable();
            properties["portName"] = ChannelName;
            properties["authorizedGroup"] = account.ToString();

            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();

            ChannelServices.RegisterChannel(new IpcChannel(properties, clientProvider, serverProvider), true);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(HE853.Device), InterfaceName, WellKnownObjectMode.Singleton);
        }

        public static void RegisterClient()
        {
            IpcClientChannel channel = new IpcClientChannel();
            ChannelServices.RegisterChannel(channel, true);
            RemotingConfiguration.RegisterWellKnownClientType(typeof(HE853.Device), "ipc://" + ChannelName + "/" + InterfaceName);
        }

        public static bool HasServiceArg(string[] args)
        {
            bool has = false;

            foreach (string arg in args)
            {
                if (arg == ServiceArg)
                {
                    has = true;
                    break;
                }
            }

            return has;
        }
    }
}