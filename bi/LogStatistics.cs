using System;
using System.IO;
using System.Text;
using UniKh.extensions;
using UniKh.utils;
using UnityEngine;

namespace UniKh.bi {
    public class LogStatistics : Statistics {
        public override void OnRegister(string account) => Log.Verbose($"{Header}/api/{account}/register");

        public override void OnLogin(string account) {
            base.OnLogin(account);
            Log.Verbose($"{Header}/api/{account}/login");
        }

        public override void OnRoleCreate(string roleId) {
            if (!account.Exists()) return;
            Log.Verbose($"{Header}/api/{account}/role_create?role_id={roleId}");
        }

        public override void OnViewEnter(string pageId) {
            if (!account.Exists()) return;
            Log.Verbose($"{Header}/api/{account}/view_enter?page_id={pageId}");
        }

        public override void OnViewQuit(string pageId) {
            if (!account.Exists()) return;
            Log.Verbose($"{Header}/api/{account}/view_quit?page_id={pageId}");
        }

        public override void OnUserEvent(string eventId, string attribute, int amount) {
            if (!account.Exists()) return;
            Log.Verbose($"{Header}/api/{account}/event?event_id={eventId}&attribute={attribute}&amount={amount}");
        }

        public override void OnClose() => Log.Verbose($"Close LogStatistics.");
    }
}