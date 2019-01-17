using UniKh.bi;
using UniKh.core;

namespace UniKh.mgr {
    public class BIMgr {
        
        private static Statistics statistics = new LogStatistics();

        public static void Create(Statistics statisticsObj) {
            statistics = statisticsObj;
        }

        public static Statistics Record => statistics;

    }
}