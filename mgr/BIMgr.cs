using UniKh.bi;
using UniKh.core;
using UnityEditor.Experimental.TerrainAPI;
using UnityEditorInternal;

namespace UniKh.mgr {
    public class BIMgr {
        
        private static IStatistics statistics;

        public static void Create(IStatistics statisticsObj) {
            statistics = statisticsObj;
        }

        public static IStatistics Record => statistics;

    }
}