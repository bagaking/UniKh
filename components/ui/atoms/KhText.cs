using System;
using UniKh.extensions;
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
            
            public string format;

            public string[] unitLst = {"", "K", "M", "B", "T", "P", "a", "b", "c", "e", "f", "g"};

            internal string GetValueString() {
                var valueInUse = value;
                var sign = valueInUse < 0 ? "-" : "+";
                var ret = "";
                var temp = valueInUse;
                if (format.Exists()) {
                    ret = (digit == 0 ? Mathf.FloorToInt(temp).ToString(format) : temp.ToString(format));
                }
                else { 
                    if (!shrink) {
                        ret = temp.ToString("F" + digit);
                    } else { 
                        var valueBase = temp < 1 ? 0 : Mathf.Log10(temp);
                        var scale = Mathf.FloorToInt(valueBase / 3);
                        var remainder = Mathf.FloorToInt(valueBase % 3);
                        var shrinkNumber = temp / Mathf.Pow(1000, scale);
                        var unit = unitLst[scale < unitLst.Length ? scale : unitLst.Length - 1];
                        format = shrinkNumber == Mathf.Floor(shrinkNumber)
                            ? "F0"
                            : ("F" + Mathf.Max(0, (scale <= 0 ? (int) digit : 3) - remainder));
                        ret = shrinkNumber.ToString(format) + unit;
                    }
                }
                
                return showSign ? sign + ret : ret;
            }
        }

        [SerializeField] private NumberText numberTextSetting = new NumberText();

        public override string text {
            get {
                return base.text;
            }
            set {
                this.m_type = Type.Text;
                base.text = value;
            }
        }

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
        
        public string NumberFormat {
            get => numberTextSetting.format;
            set {
                numberTextSetting.format = value;
                SetAllDirty();
            }
        }

        public float NumberRotateTo {
            get => numberTextSetting.rotateTo;
            set {
                numberTextSetting.rotateTo = value;
                _rotateTolerance = Math.Max(
                    Mathf.Abs(numberTextSetting.rotateTo - numberTextSetting.value) / 100f, Mathf.Pow(0.1f, NumberDigit) * 10);
            }
        }

        private float _rotateTolerance = float.Epsilon;

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

            if (Mathf.Abs(numberTextSetting.rotateTo - valueInUse) <= _rotateTolerance) {
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