﻿using System;
using System.Linq;
using System.Text;
using Android.App;
using KeePassLib;
using KeePassLib.Utility;
using PluginTOTP;

namespace keepass2android
{
	class Kp2aTotp
	{

		readonly ITotpPluginAdapter[] _pluginAdapters = new ITotpPluginAdapter[] { new TrayTotpPluginAdapter(), new KeeOtpPluginAdapter(), new KeeWebOtpPluginAdapter() };

        public ITotpPluginAdapter TryGetAdapter(PwEntryOutput entry)
        {
            if (entry == null)
                return null;
            foreach (ITotpPluginAdapter adapter in _pluginAdapters)
            {
                TotpData totpData = adapter.GetTotpData(App.Kp2a.LastOpenedEntry.OutputStrings.ToDictionary(pair => StrUtil.SafeXmlString(pair.Key), pair => pair.Value.ReadString()), Application.Context, false);
                if (totpData.IsTotpEnry)
                {
                    return adapter;
                }
            }

            return null;
        }

		public void OnOpenEntry()
        {
            var adapter = TryGetAdapter(App.Kp2a.LastOpenedEntry);
            if (adapter != null)
                new UpdateTotpTimerTask(Application.Context, adapter).Run();
        }
	}
}
