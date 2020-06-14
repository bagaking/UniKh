using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using UniKh.extensions;
using UniKh.utils.Inspector;
using UnityEngine;

namespace UniKh.core.csp.waiting {
    
    public class WaitingKeyEvent : Singleton<WaitingKeyEvent> {

        public sealed class KeyWaiter : WaitingOperation<KeyWaiter> {

            public KeyCode Key { get; internal set; }
            
            public bool Activated { get; internal set; }

            public override string ToString() {
                return "KeyWaiter";
            }
            
            public override bool IsExpired(long timeMS) {
                return Activated;
            }

            public override void Recycle() {
                base.Recycle();
                Activated = false;
            }
        }
        
        public sealed class KeyRaceWaiter : WaitingOperation<KeyRaceWaiter> {
            public KeyCode[] Keys { get; internal set; }

            public KeyCode Result { get; private set;  }

            public bool Activated { get; private set; }

            public void Finish(KeyCode result) {
                Result = result;
                Activated = true;
            }

            public override string ToString() {
                return "KeyRaceWaiter";
            }

            public override bool IsExpired(long timeMS) {
                return Activated;
            }

            public override void Recycle() {
                base.Recycle();
                Activated = false;
            }
        }
        
        private readonly Dictionary<KeyCode, List<KeyWaiter>> keyWaiterGroup = new Dictionary<KeyCode, List<KeyWaiter>>();
        private Dictionary<KeyCode, List<KeyRaceWaiter>> keyRaceWaiterGroup = new Dictionary<KeyCode, List<KeyRaceWaiter>>();


        private void OnGUI() {
            if (!Input.anyKeyDown) return;
            var e = Event.current;
            if (null == e || !e.isKey) return;
            var code = e.keyCode;
            ConsumeKey(code); 
        }

        private void ConsumeKey(KeyCode code) {
            if (keyWaiterGroup.ContainsKey(code) && keyWaiterGroup[code].Count > 0) {
                Debug.Log("WaitingKeyEvent Activated [single]:" + code);
                keyWaiterGroup[code].ForEach(w => w.Activated = true);
                keyWaiterGroup[code].Clear();
            }
            
            if (keyRaceWaiterGroup.ContainsKey(code) && keyRaceWaiterGroup[code].Count > 0) {
                Debug.Log("WaitingKeyEvent Activated [race]:" + code);
                keyRaceWaiterGroup[code].ForEach(w => {
                    w.Finish(code);
                    w.Keys.ForEach(
                        key => {
                            // deliver to the Clear procedure behind.
                            if (key == code) return; 
                            // the remove operation take O(nm), but it avoid the condition proc per frame.
                            keyRaceWaiterGroup[key].Remove(w); 
                        });
                });
                keyRaceWaiterGroup[code].Clear();
            }
        }

        public static KeyWaiter WaitKey(KeyCode key) {
            if (!LazyInst.keyWaiterGroup.ContainsKey(key)) {
                LazyInst.keyWaiterGroup[key] = new List<KeyWaiter>();
            }

            var keyWaiter = KeyWaiter.New;
            keyWaiter.Key = key;
            LazyInst.keyWaiterGroup[key].Add(keyWaiter);
            return keyWaiter;
        }
        
        public static KeyRaceWaiter WaitKeysRace(params KeyCode[] keys) {
            var keyWaiter = KeyRaceWaiter.New;
            keyWaiter.Keys = keys;
            keys.ForEach(
                k => {
                    if (!LazyInst.keyRaceWaiterGroup.ContainsKey(k)) {
                        LazyInst.keyRaceWaiterGroup[k] = new List<KeyRaceWaiter>();
                    }
                    LazyInst.keyRaceWaiterGroup[k].Add(keyWaiter);
                }
            );

            return keyWaiter;
        }

    }
}