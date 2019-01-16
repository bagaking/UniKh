namespace UniKh.bi {
    
    public interface IStatistics {

        void OnInit(string appHash, string channelId = "", string version = "", string deviceId = "");

        void OnRegister(string account);

        void OnLogin(string account);

        void OnRoleCreate(string roleId);

        void OnViewEnter(string pageId);

        void OnViewQuit(string pageId);

        void OnUserEvent(string eventId, string attribute = "", int amount);

    }
}