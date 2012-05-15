﻿/*
Home Easy HE853 Control Service
Copyright (C) 2012 Thomas Ascher

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

namespace HE853Service
{
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Ipc;
    using System.ServiceProcess;
    
    public partial class Service : ServiceBase
    {
        private HE853.Device device = new HE853.Device();
        
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.device.Open();

            ChannelServices.RegisterChannel(new IpcChannel("HE853"), true);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(HE853.Device), "Device", WellKnownObjectMode.Singleton);
        }

        protected override void OnStop()
        {
            this.device.Close();
        }
    }
}