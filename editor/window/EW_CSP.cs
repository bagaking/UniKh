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
        public class ProcStatistic {
            public string Tag = "_";
            public long TotalCpuTimeCostMS = 0;
            public long TotalFrameCount = 0;
            public long DetectedAt = 0;
            public long FinishAt = 0;
            public long DetectedTick = 0;
            public long FinishTick = 0;
        }

        [MenuItem("UniKh/Monitor/CSP")]
        public static void ShowDialog() {
            GetWindow("UniKh/Monitor/CSP");
        }

        // initial (OnEnable) will be called when enter the playing mode
        // thus there are no needs to use playModeStateChanged methods
        public override bool Initial() {
            //Debug.Log("EW_CSP Initialed");
            //EditorApplication.playModeStateChanged += (state) => {
            //if (state == PlayModeStateChange.ExitingPlayMode) {
            latestCollectTick = -1;
            TicksLost = 0;
            procStatistics.Clear();
            //}
            //};

            return true;
        }


        // Related fields should be cleared when restart the game, cuz the editor state will not be cleared;
        private static long latestCollectTick { get; set; } = -1;
        public long TicksLost { get; private set; } = 0;

        public static Dictionary<int, ProcStatistic> procStatistics = new Dictionary<int, ProcStatistic>();

        public ProcStatistic GetProcStatistic(int id) {
            if (!procStatistics.ContainsKey(id)) {
                procStatistics.Add(id, new ProcStatistic());
            }
            return procStatistics[id];
        }

        public GUILayoutTogglePanel tpProcInAction = new GUILayoutTogglePanel("Proc in action", true, false);
        public GUILayoutTogglePanel tpProcFinished = new GUILayoutTogglePanel("Proc finished", false, false);
        public GUILayoutScrollPanel scrollProcsInfo = new GUILayoutScrollPanel();


        public override void GUIProc(Event e) {
            if (latestCollectTick < 0) {
                if (!Application.isPlaying) {
                    EditorGUILayout.LabelField("UniKh/CSP can only be accessed while the program is running ...");
                    return;
                }

                if (!CSP.Inst) {
                    EditorGUILayout.LabelField("UniKh/CSP cre not loaded ...");
                    return;
                }
            }

            DrawRuntime();

            var array = new int[procStatistics.Count];
            procStatistics.Keys.CopyTo(array, 0);
            var disabledProcs = new List<int>(array);

            scrollProcsInfo.Draw(() => {
                if (Application.isPlaying) {
                    DrawActiveProcs();
                    disabledProcs = disabledProcs.Filter(procID => !CSP.Inst.procLst.Exists(proc => proc.ID == procID));
                } else {
                    EditorGUILayout.LabelField("UniKh/CSP is not running. Here's the data of procs from the last run.");
                }
                disabledProcs.Reverse();
                DrawFinishedProcs(disabledProcs);
            });

        }

        public void DrawRuntime() {
            if (!CSP.Inst) {
                EditorGUILayout.LabelField("UniKh/CSP cre not running ...");
            } else {
                EditorGUILayout.LabelField("Runtime:");
                EditorGUILayout.LabelField("|  Procs\t| Frames\t| MS\t| Ticks\t| Updates\t| Lost");
                EditorGUILayout.LabelField($"| {CSP.Inst.procLst.Count}\t| {CSP.Inst.MonitExecutedInFrame}\t| {CSP.Inst.MonitTickTimeCost}\t| {CSP.Inst.TotalTicks}\t| {CSP.Inst.MonitTotalUpdates}\t| {TicksLost}");
            }
        }

        public void DrawActiveProcs() {
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
        }

        public void DrawFinishedProcs(List<int> finishedProcs) {
            tpProcFinished.Draw("(" + finishedProcs.Count + ")", () => {
                finishedProcs.ForEach(procID => {
                    var id = procID;
                    var st = GetProcStatistic(id);
                    EditorGUILayout.LabelField(
                        SGen.New[id][". #"][st.Tag]
                        ["  Detected:at-"][st.DetectedAt][",tick-"][st.DetectedTick]
                        ["  Frames:"][st.TotalFrameCount]
                        ["  MS:"][st.TotalCpuTimeCostMS]
                        ["  Finished:at-"][st.FinishAt][",tick-"][st.FinishTick]
                        .End);
                });
            });
        }

        private void CollectData() {
            if (null == CSP.Inst) {
                return;
            }
            var totalTicks = CSP.Inst.TotalTicks;
            if (latestCollectTick >= totalTicks) {
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
                    st.DetectedTick = totalTicks;
                }
                st.FinishAt = proc.ExecutedTime;
                st.FinishTick = totalTicks;
            }

            if (latestCollectTick != -1) {
                var skiped = (totalTicks - latestCollectTick - 1);
                if (skiped != 0) {
                    Debug.LogWarning(SGen.New["EW_CSP CollectData: lost ticks - skiped:"][skiped][" TotalTicks:"][totalTicks][" latestCollectTick:"][latestCollectTick]);
                    TicksLost += skiped;
                }
            }
            latestCollectTick = totalTicks;

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

