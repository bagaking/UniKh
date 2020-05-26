using System.Collections;
using System.Collections.Generic;
using UniKh.extensions;
using UnityEngine;

namespace UniKh.core.csp.waiting {
    
    public class WaitingKeyEvent : Singleton<WaitingKeyEvent> {

        public sealed class KeyWaiter : WaitingOperation<KeyWaiter> {

            public bool Activated { get; internal set; }

            public override string ToString() {
                return "Condition";
            }
            
            public override bool IsExpired(long timeMS) {
                return Activated;
            }
        }
        
        private Dictionary<KeyCode, List<KeyWaiter>> waitingGroup = new Dictionary<KeyCode, List<KeyWaiter>>();

        public bool KeyListened(KeyCode code) {
            return waitingGroup.ContainsKey(code) && waitingGroup[code].Count > 0;
        }

        private void OnGUI() {
            if (!Input.anyKeyDown) return;
            var e = Event.current;
            if (null == e || !e.isKey) return;
            var code = e.keyCode;
            if (!KeyListened(code)) return;
            waitingGroup[code].ForEach(w => w.Activated = true);
            waitingGroup[code].Clear();
        }
        
        public static KeyWaiter WaitKey(KeyCode key) {
            EnsureInst();
                
            if (!LazyInst.waitingGroup.ContainsKey(key)) {
                LazyInst.waitingGroup[key] = new List<KeyWaiter>();
            }

            var keyWaiter = KeyWaiter.New;
            LazyInst.waitingGroup[key].Add(keyWaiter);
            return keyWaiter;
        }
        

    }
}