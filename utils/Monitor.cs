using System.Collections.Generic;
using UniKh.core;
using UnityEngine;

namespace UniKh.utils {
    
    /// <summary>
    /// 
    /// </summary>
    public class FrameMonitor : Dictionary<int, Dictionary<string, float>> {
        private int maxId = 0;

        private Dictionary<string, float> T(int id) {
            if (ContainsKey(id)) return this[id];
            else return this[id] = new Dictionary<string, float>();
        }

        public float Inc(int id, string key, float value) {
            var table = T(id);
            if (id > maxId) maxId = id;
            if (table.ContainsKey(key)) return table[key] += value;
            else return table[key] = value;
        }

        public Dictionary<string, float> current => T(maxId);
    }

    public class Monitor : Singleton<Monitor> {
        public float fps;
        public float deltaTime;
        public float totalTime;

        public int updateCount = 0;
        public int fixedUpdateCount = 0;

        void Update()
        {
            totalTime += Time.deltaTime;
            deltaTime += (Time.deltaTime - deltaTime) * 0.5f;
            fps = 1.0f / deltaTime;
            updateCount++;
        }

        void FixedUpdate() { fixedUpdateCount++; }

        void OnDestory() {
            sws.ForEach(
                sw => {
                    if (sw.IsRunning) sw.Stop();
                });
        }

        private Dictionary<string, float> monitor = new Dictionary<string, float>();
        private readonly FrameMonitor fframemonitor = new FrameMonitor();
        private readonly FrameMonitor uframemonitor = new FrameMonitor();

        private readonly Dictionary<string, object> marks = new Dictionary<string, object>();
        public object this[string key] { get => marks[key]; set => marks[key] = value; }

        private readonly List<System.Diagnostics.Stopwatch> sws = new List<System.Diagnostics.Stopwatch>();

        public System.Action<System.Action> CreateMonitor(string key) {
            var sw = new System.Diagnostics.Stopwatch();
            sws.Add(sw);
            sw.Start();

            return action => {
                action?.Invoke();
                sw.Stop();
                sws.Remove(sw);
                monitor[key] = sw.ElapsedMilliseconds;
            };
        }

        public System.Action CreateFFrameMonitor(string key) {
            var sw = new System.Diagnostics.Stopwatch();
            sws.Add(sw);
            sw.Start();
            var fCount = fixedUpdateCount;
            return () => {
                sw.Stop();
                sws.Remove(sw);
                if (fCount != fixedUpdateCount) throw new System.Exception("FFrameMonitor: cannot escape frame.");
                IncFFrameMonitor(key, sw.ElapsedMilliseconds);
            };
        }

        public System.Action CreateUFrameMonitor(string key) {
            var sw = new System.Diagnostics.Stopwatch();
            sws.Add(sw);
            sw.Start();
            var uCount = updateCount;
            return () => {
                sw.Stop();
                sws.Remove(sw);
                if (uCount != updateCount) throw new System.Exception("UFrameMonitor: cannot escape frame.");
                IncUFrameMonitor(key, sw.ElapsedMilliseconds);
            };
        }

        public void IncFFrameMonitor(string key, float value = 1) { fframemonitor.Inc(fixedUpdateCount, key, value); }

        public void IncUFrameMonitor(string key, float value = 1) {
            uframemonitor.Inc(updateCount, key, value);
            // print($"uframemonitor.T(updateCount)[{key}] => {uframemonitor.T(updateCount)[key]}");
        }

        void OnGUI() {
            labelCount = 0;
            GUI.skin.label.fontSize = 28;
            Draw("FPS", Mathf.Ceil(fps));
            Draw("FPS(AVG)", updateCount / totalTime);
            
            Draw("Update", updateCount);
            Draw("FUpdate", fixedUpdateCount);
            Draw("Time", Time.time);
            Draw("FixedTime", Time.fixedTime);
            foreach (var pair in monitor) {
                Draw(pair.Key, pair.Value, pair.Value > 6 ? Color.red : GUI.color);
            }

            var curFFrame = fframemonitor.current;
            if (curFFrame.Count > 0) Label($"f.T({fframemonitor.Count}) {curFFrame.Count}");
            foreach (var pair in curFFrame) {
                Draw(pair.Key, pair.Value);
            }

            var curUFrame = uframemonitor.current;
            if (curUFrame.Count > 0) Label($"u.T({uframemonitor.Count}) {curUFrame.Count}");
            foreach (var pair in curUFrame) {
                Draw(pair.Key, pair.Value);
            }

            if (curUFrame.Count > 0) Label("marks " + marks.Count);
            foreach (var pair in marks) {
                Draw(pair.Key, pair.Value);
            }
        }

        private int labelCount = 0;

        void Draw(string label, object value, Color? c = null) {
            var originC = GUI.color;
            if (c != null) GUI.color = c.Value;
            GUI.Label(new Rect(20, 10 + 32 * (labelCount++), 512, 32), label + " " + value);
            GUI.color = originC;
        }

        void Label(string label) { GUI.Label(new Rect(5, 10 + 32 * (labelCount++), 512, 32), label + ":"); }
    }
}