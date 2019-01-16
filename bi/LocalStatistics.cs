using System;
using System.IO;
using System.Text;
using UniKh.extensions;
using UniKh.utils;
using UnityEngine;

namespace UniKh.bi {
    public class LocalStatistics : Statistics {
        private StreamWriter sw = null;

        public LocalStatistics(string path) => sw = CacheFolderUtil.GetAppendSW(this.path = path);

        public string path;

        public override void OnRegister(string account) {
            sw.WriteLine($"{Header}/api/{account}/register");
            sw.Flush();
        }

        public override void OnLogin(string account) {
            base.OnLogin(account);
            sw.WriteLine($"{Header}/api/{account}/login");
            sw.Flush();
        }

        public override void OnRoleCreate(string roleId) {
            if (!account.Exists()) return;
            sw.WriteLine($"{Header}/api/{account}/role_create?role_id={roleId}");
            sw.Flush();
        }

        public override void OnViewEnter(string pageId) {
            if (!account.Exists()) return;
            sw.WriteLine($"{Header}/api/{account}/view_enter?page_id={pageId}");
            sw.Flush();
        }

        public override void OnViewQuit(string pageId) {
            if (!account.Exists()) return;
            sw.WriteLine($"{Header}/api/{account}/view_quit?page_id={pageId}");
            sw.Flush();
        }

        public override void OnUserEvent(string eventId, string attribute = "", int amount = 0) {
            if (!account.Exists()) return;
            sw.WriteLine($"{Header}/api/{account}/event?event_id={eventId}&attribute={attribute}&amount={amount}");
            sw.Flush();
        }

        public override void OnClose() {
            sw.Close();
            Log.Info($"Close LocalStatistics. Check {Path.Combine(Application.persistentDataPath, path)}");
        }
    }
}