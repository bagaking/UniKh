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
                    : ("F" + ((scale == 0 ? (int)digit : 3) - remainder));

                return showSign
                    ? (sign + shrinkNumber.ToString(format) + unit)
                    : (shrinkNumber.ToString(format) + unit);
            }

            public void TryRotate(float deltaTime) {
                if (!(Math.Abs(rotateTo - float.MinValue) > float.Epsilon)) return;

                var valueInUse = value;
                valueInUse = Mathf.Lerp(valueInUse, rotateTo, rotateTo - valueInUse > 1000 ? 0.2f : 0.1f);
                if (Mathf.Abs(rotateTo - valueInUse) <= float.Epsilon) {
                    valueInUse = rotateTo;
                    rotateTo = float.MinValue;
                }

                value = valueInUse;
            }
        }

        public NumberText numberTextSetting = new NumberText();

        protected override void OnPopulateMesh(VertexHelper toFill) {
            var textOld = m_Text;
            var builder = SGen.New[m_prefix];
            switch (m_type) {
                case Type.NumberText:
                    builder.Append(numberTextSetting.GetValueString());
                    break;
                case Type.Text:
                    builder.Append(m_Text);
                    break;
            }

            m_Text = builder.Append(m_subfix).End;
            base.OnPopulateMesh(toFill);
            m_Text = textOld;
        }

        public void Update() {
            switch (m_type) {
                case Type.NumberText:
                    numberTextSetting.TryRotate(Time.deltaTime);
                    break;
            }
        }
    }
}