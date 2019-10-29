/** == EveryUpdate.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/29 11:47:54
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System; 

namespace UniKh.utils {
    [Serializable]
    public class EveryUpdate {
        public float timeSpanS = 0;
        public float timeAccumulated = 0;
        public int round = 0;

        public EveryUpdate(float timeSpanS) {
            this.timeSpanS = timeSpanS;
        }

        public bool Test(float deltaTime) {
            timeAccumulated += deltaTime;
            if (!(timeAccumulated > timeSpanS * (round + 1))) return false;
            round++;
            return true;
        }
    }
}