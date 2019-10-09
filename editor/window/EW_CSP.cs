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

        [MenuItem("UniKh/CSP_Monitor")]
        public static void ShowDialog() {
            GetWindow("UniKh - CSP_Monitor");
        }

        public override bool Initial() {
            return true;
        }

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
            EditorGUILayout.LabelField("|  Procs\t| Frames\t| MS\t| Ticks\t| Updates");
            EditorGUILayout.LabelField($"| {CSP.Inst.procLst.Count}\t| {CSP.Inst.MonitExecutedInFrame}\t| {CSP.Inst.MonitTickTimeCost}\t| {CSP.Inst.TotalTicks}\t| {CSP.Inst.MonitTotalUpdates}");

            foreach (var proc in CSP.Inst.procLst) {
                var OpCurr = proc.GetOpCurr();
                EditorGUILayout.LabelField(
                    SGen.New[proc.ID][". #"][proc.Tag]
                    ["  At:"][proc.ExecutedTime]
                    ["  Frames:"][proc.MonitTickFrameCount]
                    ["  MS:"][proc.MonitTickTimeCost]
                    ["  Waiting:"][OpCurr == null ? "(null)" : OpCurr.ToString()].End);
                
                var StackTrace = SGen.New["Stack: "];
                proc.ProcStack.ForEach((layer, ind) => {
                    StackTrace = StackTrace['/'][layer.Current == null ? "null" : layer.Current.ToString()];
                });
                EditorGUILayout.LabelField(StackTrace.End);

            }
        }


        //private bool using_corou_in_editor = false;
        public void Update() {
            //if (!Application.isPlaying && using_corou_in_editor) {
            //    Corou.TriggerTick();
            //}
            Repaint();
        }
    }
}
