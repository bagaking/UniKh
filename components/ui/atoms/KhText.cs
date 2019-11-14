using System;
using UniKh.utils;
using UnityEngine;
using UnityEngine.UI;

namespace UniKh.comp.ui {
    public class KhText : Text {
        public enum Type {
            Text,
            NumberText
        }

        public Type m_type;

        public string m_prefix;
        public string m_subfix;

        public override float preferredWidth {
            get {
                var settings = GetGenerationSettings(Vector2.zero);
                return cachedTextGeneratorForLayout.GetPreferredWidth(GetTextToRender(), settings) / pixelsPerUnit;
            }
        }

        public EveryUpdate updateRotate = new EveryUpdate(0.1f);

        [Serializable]
        public class NumberText {
            public float value = 0;
            public float rotateTo = 0;
            public bool showSign = false;
            public uint digit = 3;
            public bool shrink = false;
            public string[] unitLst = {"", "K", "M", "B", "T", "P", "a", "b", "c", "e", "f", "g"};

            internal string GetValueString() {
                var valueInUse = value;
                var sign = valueInUse < 0 ? "-" : "+";
                var temp = Mathf.Abs(valueInUse);
                if (!shrink) {
                    var ret = temp.ToString("F" + digit);
                    return showSign ? sign + ret : ret;
                }

                var valueBase = temp < 1 ? 0 : Mathf.Log10(temp);
                var scale = Mathf.FloorToInt(valueBase / 3);
                var remainder = Mathf.FloorToInt(valueBase % 3);
                var shrinkNumber = temp / Mathf.Pow(1000, scale);
                var unit = unitLst[scale < unitLst.Length ? scale : unitLst.Length - 1];
                var format = shrinkNumber == Mathf.Floor(shrinkNumber)
                    ? "F0"
                    : ("F" + Mathf.Max(0,(scale <= 0 ? (int) digit : 3) - remainder));

                return showSign
                    ? (sign + shrinkNumber.ToString(format) + unit)
                    : (shrinkNumber.ToString(format) + unit);
            }
        }

        [SerializeField] private NumberText numberTextSetting = new NumberText();

        public float NumberValue {
            get => numberTextSetting.value;
            set {
                m_type = Type.NumberText;
                numberTextSetting.value = value;
                SetAllDirty();
            }
        }

        public bool NumberShowSign {
            get => numberTextSetting.showSign;
            set {
                numberTextSetting.showSign = value;
                SetAllDirty();
            }
        }

        public uint NumberDigit {
            get => numberTextSetting.digit;
            set {
                numberTextSetting.digit = value;
                SetAllDirty();
            }
        }

        public bool NumberShrink {
            get => numberTextSetting.shrink;
            set {
                numberTextSetting.shrink = value;
                SetAllDirty();
            }
        }

        public float NumberRotateTo {
            get => numberTextSetting.rotateTo;
            set => numberTextSetting.rotateTo = value;
        }

        protected string GetTextToRender() {
            var builder = SGen.New[m_prefix];
            switch (m_type) {
                case Type.NumberText:
                    builder.Append(numberTextSetting.GetValueString());
                    break;
                case Type.Text:
                    builder.Append(m_Text);
                    break;
            }
            return builder.Append(m_subfix).End;
        }

        protected override void OnPopulateMesh(VertexHelper toFill) {
            var textOld = m_Text; 
            m_Text = GetTextToRender();
            base.OnPopulateMesh(toFill);
            m_Text = textOld;
        }

        private void TryRotate() {
            var digitRate = Mathf.Pow(10f, numberTextSetting.digit);
            if (!(Math.Abs(numberTextSetting.rotateTo - float.MinValue) > 1 / digitRate)) return;

            var valueInUse = NumberValue;
            valueInUse = Mathf.Lerp(
                valueInUse,
                numberTextSetting.rotateTo,
                (numberTextSetting.rotateTo - valueInUse) > 1000 ? 0.3f : 0.2f
            );

            if (Mathf.Abs(numberTextSetting.rotateTo - valueInUse) <= float.Epsilon) {
                valueInUse = numberTextSetting.rotateTo;
                numberTextSetting.rotateTo = float.MinValue;
            }

            NumberValue = Mathf.Round(valueInUse * digitRate) / digitRate;
        }

        public void Update() {
            switch (m_type) {
                case Type.NumberText:
                    if (updateRotate.Test(Time.deltaTime)) {
                        TryRotate();
                    }

                    break;
            }
        }
    }
}