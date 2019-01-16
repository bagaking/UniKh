using System;

namespace UniKh.bi {
    public abstract class Statistics {
        
        public string Header => $"[{DateTime.Now}][{info}]";
        public string appHash { get; private set; }
        public string channelId { get; private set; }
        public string version { get; private set; }
        public string deviceId { get; private set; }

        public string account = "";
        
        private string info;
        
        public virtual void OnInit(string appHash, string channelId, string version, string deviceId) {
            this.appHash = appHash;
            this.channelId = channelId;
            this.version = version;
            this.deviceId = deviceId;
            info = $"[{appHash},{channelId},{version},{deviceId}]";
        }

        public abstract void OnRegister(string account);

        public virtual void OnLogin(string account) { this.account = account; }
        public abstract void OnRoleCreate(string roleId);
        public abstract void OnViewEnter(string pageId);
        public abstract void OnViewQuit(string pageId);
        public abstract void OnUserEvent(string eventId, string attribute, int amount);
        public abstract void OnClose();
    }
}