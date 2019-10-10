/** EW_CSP.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/09 21:40:09
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniKh.core;
using UniKh.extensions;
using System;
using UnityEditor;
using UniKh.utils;

namespace UniKh.editor {
    public class EW_CSP : EWBase<EW_CSP> {

        [MenuItem("UniKh/Monitor/CSP")]
        public static void ShowDialog() {
            GetWindow("UniKh/Monitor/CSP");
        }

        public override bool Initial() {
            return true;
        }

        public long TicksLost { get; private set; } = 0;
        public class ProcStatistic {
            public string Tag = "_";
            public long TotalCpuTimeCostMS = 0;
            public long TotalFrameCount = 0;
            public long DetectedAt = 0;
            public long FinishAt = 0;
            public long DetectedTick = 0;
            public long FinishTick = 0;
        }

        public static Dictionary<int, ProcStatistic> procStatistics = new Dictionary<int, ProcStatistic>();

        public ProcStatistic GetProcStatistic(int id) {
            if (!procStatistics.ContainsKey(id)) {
                procStatistics.Add(id, new ProcStatistic());
            }
            return procStatistics[id];
        }

        public GUILayoutTogglePanel tpProcInAction = new GUILayoutTogglePanel("Proc in action", true);
        public GUILayoutTogglePanel tpProcFinished = new GUILayoutTogglePanel("Proc finished");

        public override void GUIProc(Event e) {
            if (!Application.isPlaying) {
                EditorGUILayout.LabelField("UniKh/CSP can only be accessed while the program is running ...");
                return;
            }

            if(!CSP.Inst) {
                EditorGUILayout.LabelField("UniKh/CSP cre not loaded ...");
                return;
            }

            EditorGUILayout.LabelField("Statistics:");
            EditorGUILayout.LabelField("|  Procs\t| Frames\t| MS\t| Ticks\t| Updates\t| Lost");
            EditorGUILayout.LabelField($"| {CSP.Inst.procLst.Count}\t| {CSP.Inst.MonitExecutedInFrame}\t| {CSP.Inst.MonitTickTimeCost}\t| {CSP.Inst.TotalTicks}\t| {CSP.Inst.MonitTotalUpdates}\t| {TicksLost}");

            var array = new int[procStatistics.Count];
            procStatistics.Keys.CopyTo(array, 0);
            var disabledProcs = new List<int>(array);


            tpProcInAction.Draw(CSP.Inst.procLst.Count.ToString(), () => {
                foreach (var proc in CSP.Inst.procLst) {
                    var st = GetProcStatistic(proc.ID);
                    
                    var OpCurr = proc.GetOpCurr();
                    EditorGUILayout.LabelField(
                        SGen.New[proc.ID][". #"][proc.Tag]
                        ["  Detected:at-"][st.DetectedAt][",tick-"][st.DetectedTick]
                        ["  ExecutedTime:"][proc.ExecutedTime]
                        ["  Frames:"][proc.MonitTickFrameCount]['/'][st.TotalFrameCount]
                        ["  MS:"][proc.MonitTickTimeCost]['/'][st.TotalCpuTimeCostMS]
                        ["  Waiting:"][OpCurr == null ? "(null)" : OpCurr.ToString()].End);

                    var StackTrace = SGen.New["Stack: "];
                    proc.ProcStack.ForEach((layer, ind) => {
                        StackTrace = StackTrace['/'][layer.Current == null ? "null" : layer.Current.ToString()];
                    });
                    EditorGUILayout.LabelField(StackTrace.End);
                }
            });

            for (var i = disabledProcs.Count - 1; i >= 0; i--) {
                var procID = disabledProcs[i];
                if (!CSP.Inst.procLst.Exists(proc => proc.ID == procID)) {
                    continue;
                }
                disabledProcs.RemoveAt(i);
            }

            tpProcFinished.Draw(disabledProcs.Count.ToString(), () => {
                disabledProcs.ForEach(procID => {
                    var st = GetProcStatistic(procID);
                    EditorGUILayout.LabelField(
                        SGen.New[procID][". #"][st.Tag]
                        ["  Detected:at-"][st.DetectedAt][",tick-"][st.DetectedTick]
                        ["  Frames:"][st.TotalFrameCount]
                        ["  MS:"][st.TotalCpuTimeCostMS]
                        ["  Finished:at-"][st.FinishAt][",tick-"][st.FinishTick]
                        .End);
                });
            });
        }


        long latestCollectTick = -1;
        private void CollectData() { 
            if (null == CSP.Inst) {
                return;
            }
            if(latestCollectTick >= CSP.Inst.TotalTicks) {
                return;
            }
            foreach (var proc in CSP.Inst.procLst) {
                var st = GetProcStatistic(proc.ID);
                st.Tag = proc.Tag;
                st.TotalCpuTimeCostMS += proc.MonitTickTimeCost;
                st.TotalFrameCount += proc.MonitTickFrameCount;


                if (st.DetectedAt == 0) {
                    st.DetectedAt = proc.ExecutedTime;
                }
                if (st.DetectedTick == 0) {
                    st.DetectedTick = CSP.Inst.TotalTicks;
                }
                st.FinishAt = proc.ExecutedTime;
                st.FinishTick = CSP.Inst.TotalTicks;
            }

            if(latestCollectTick != -1) {
                var skiped = (CSP.Inst.TotalTicks - latestCollectTick - 1);
                if(skiped != 0) {
                    Debug.LogWarning(SGen.New["EW_CSP CollectData: lost ticks - skiped:"][skiped][" TotalTicks:"][CSP.Inst.TotalTicks][" latestCollectTick:"][latestCollectTick]);
                    TicksLost += skiped;
                }
            }
            latestCollectTick = CSP.Inst.TotalTicks;
            
        }

        public void Update() {
            //if (!Application.isPlaying && using_corou_in_editor) {
            //    Corou.TriggerTick();
            //}
            if (Application.isPlaying && !EditorApplication.isPaused) {
                CollectData();
                Repaint();
            }
        }

    }
}

