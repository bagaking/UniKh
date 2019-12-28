/** == ContextMenuUI.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/12/29 04:08:57
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */
 
using UniKh.core;
using UnityEngine;
using UnityEngine.UI;
using UniKh.extensions;

namespace UniKh.comp.ui {
    public class KhScoreBar : BetterBehavior {
        public enum ScoreShowMode {
            CurrentLevel,
            Total
        }

        [Header("Binding:Show")] 
        public Slider scoreProgress;
        public KhText aLevel;
        public KhText aScore;
        public KhText aScoreRequirement;
        
        [Header("Binding:Event")]
        public KhBtn btnBar;
        public KhBtn btnIcon;

        [Header("Setting")] public uint levelShowOffset = 1;
        public ScoreShowMode scoreShowMode;

        [Header("Runtime")] public float score = 0;
        public float scoreRequirement = 0;
        public uint level = 0;

        public void TryRepaintLevel() {
            if (aLevel == null) return;
            var showLevel = level + levelShowOffset;
            if(showLevel == (uint) aLevel.NumberValue) return;
            if (aLevel.NumberDigit != 0) aLevel.NumberDigit = 0;
            aLevel.NumberValue = showLevel;
        }
        
        public void TryRepaintScore() {
            if (aScore == null || score.EqualsTo(aScore.NumberValue)) return;
            aScore.NumberValue = score;
        }
        
        public void TryRepaintScoreRequirement() {
            if (aScoreRequirement == null || scoreRequirement.EqualsTo(aScoreRequirement.NumberValue)) return;
            aScoreRequirement.NumberValue = scoreRequirement;
        }
        
        public void TryRepaintScoreProgress() {
            if (scoreProgress == null) return;
            if (scoreRequirement.EqualsTo(0)) {
                scoreProgress.value = 0;
                return;
            }
            var progress = score / scoreRequirement;
            scoreProgress.value = Mathf.Clamp01(progress);
        }

        public void Update() {
            TryRepaintLevel();
            TryRepaintScore();
            TryRepaintScoreRequirement();
            TryRepaintScoreProgress();
        }
    }
}