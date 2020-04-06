using System;
using UniKh.extensions;
using UniKh.utils;
using UnityEngine;
using UnityEngine.UI;

namespace UniKh.comp.ui {
    public class KhText : Text {
        public enum OutlineType {
            None,
            Basic,
            Fine
        }

        public enum Type {
            Text,
            NumberText
        }

        public Type m_type;

        public OutlineType m_outline;
        public Vector2 m_outlineOffset = Vector2.one;
        public Color m_outlineColor = Color.black;


        public Vector2 m_shadowOffset = Vector2.zero;
        public Color m_shadowColor = Color.black;

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

            internal string GetValueString(string prefix, string subfix) {
                var valueInUse = value;
                var sign = valueInUse < 0 ? "-" : "+";
                var ret = "";
                var fmt = format;

                if (fmt.Exists()) {
                    var val = digit == 0 ? Mathf.FloorToInt(valueInUse).ToString(fmt) : valueInUse.ToString(fmt);
                    if (!prefix.Exists() && !subfix.Exists() && !showSign) return val;
                    var customBuilder = SGen.New[prefix];
                    if (valueInUse > 0 && showSign) customBuilder.Append(sign);
                    return customBuilder[val][subfix].End;
                }

                var builder = SGen.New[prefix];

                if (!shrink) {
                    builder.Append(valueInUse.ToString("F" + digit));
                }
                else {
                    var temp = Math.Abs(valueInUse);
                    if (showSign || valueInUse < 0) builder.Append(sign);

                    var valueBase = temp < 1 ? 0 : Mathf.Log10(temp);
                    var scale = Mathf.FloorToInt(valueBase / 3);
                    var remainder = Mathf.FloorToInt(valueBase % 3);
                    var shrinkNumber = temp / Mathf.Pow(1000, scale);
                    var unit = unitLst[scale < unitLst.Length ? scale : unitLst.Length - 1];
                    fmt = shrinkNumber == Mathf.Floor(shrinkNumber)
                        ? "F0"
                        : ("F" + Mathf.Max(0, (scale <= 0 ? (int) digit : 3) - remainder));
                    builder.Append(shrinkNumber.ToString(fmt)).Append(unit);
                }

                builder.Append(subfix);
                return builder.End;
            }
        }

        [SerializeField] private NumberText numberTextSetting = new NumberText();

        public override string text {
            get { return base.text; }
            set {
                this.m_type = Type.Text;
                base.text = value;
            }
        }

        public float NumberValue {
            get => numberTextSetting.value;
            set {
                m_type = Type.NumberText;
                if (Math.Abs(numberTextSetting.value - value) < 0.00001f) return;
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
                    Mathf.Abs(numberTextSetting.rotateTo - numberTextSetting.value) / 100f,
                    Mathf.Pow(0.1f, NumberDigit) * 10);
            }
        }

        private float _rotateTolerance = float.Epsilon;

        protected string GetTextToRender() {
            switch (m_type) {
                case Type.NumberText:
                    return numberTextSetting.GetValueString(m_prefix, m_subfix);
                case Type.Text:
                    if (m_prefix.Exists() || m_subfix.Exists()) {
                        return SGen.New[m_prefix][m_Text][m_subfix].End;
                    }

                    return m_Text;
                default:
                    return "";
            }
        }


        /// <summary>
        /// Add a quad to the stream.
        /// </summary>
        /// <param name="verts">4 Vertices representing the quad.</param>
        public void AddUIVertexQuad(VertexHelper toFill, UIVertex[] verts, Color c) {
            var startIndex = toFill.currentVertCount;
            for (var i = 0; i < 4; i++)
                toFill.AddVert(verts[i].position, c, verts[i].uv0, verts[i].uv1, verts[i].normal,
                    verts[i].tangent);
            toFill.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            toFill.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }


        readonly UIVertex[] m_TempVerts = new UIVertex[4];

        const float outlineOffsetScale = 1.25f;

        readonly Vector2[][] m_OutlineOffsetBasic = new[] {
            new[] {new Vector2(-1, -1)},
            new[] {new Vector2(+1, -1)},
            new[] {new Vector2(-1, +1)},
            new[] {new Vector2(+1, +1)}
        };

        readonly Vector2[][] m_OutlineOffsetFine = new[] {
            new[] {new Vector2(-1, -1)},
            new[] {new Vector2(+1, -1)},
            new[] {new Vector2(-1, +1)},
            new[] {new Vector2(+1, +1)},
            new[] {new Vector2(-1.25f, 0)},
            new[] {new Vector2(+1.25f, 0)},
            new[] {new Vector2(0, -1.25f)},
            new[] {new Vector2(0, +1.25f)},
        };

        protected override void OnPopulateMesh(VertexHelper toFill) {
            if (font == null)
                return;
            var textToRender = GetTextToRender(); // get text to render

            m_DisableFontTextureRebuiltCallback = true;
            var extents = rectTransform.rect.size;
            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.PopulateWithErrors(textToRender, settings, gameObject);

            var verts = cachedTextGenerator.verts;
            var unitsPerPixel = 1 / pixelsPerUnit;
            var vertCount = verts.Count;

            // We have no verts to process just return (case 1037923)
            if (vertCount <= 0) {
                toFill.Clear();
                return;
            }

            var roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
            roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
            toFill.Clear();

            if (m_shadowOffset != Vector2.zero) {
                for (var i = 0; i < vertCount; ++i) {
                    var tempVertsIndex = i & 3;
                    m_TempVerts[tempVertsIndex] = verts[i];
                    m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                    m_TempVerts[tempVertsIndex].position.x += m_shadowOffset.x + roundingOffset.x;
                    m_TempVerts[tempVertsIndex].position.y += m_shadowOffset.y + roundingOffset.y;
                    if (tempVertsIndex == 3)
                        AddUIVertexQuad(toFill, m_TempVerts, m_shadowColor);
                }
            }

            switch (m_outline) {
                case OutlineType.Basic:
                    if (roundingOffset != Vector2.zero) {
                        for (var oInd = 0; oInd < m_OutlineOffsetBasic.Length; oInd++) {
                            var offsets = m_OutlineOffsetBasic[oInd];
//                            var fitterRate = (float) Mathf.Log10(font.fontSize);
//                            Debug.Log(fitterRate);
                            for (var i = 0; i < vertCount; ++i) {
                                var tempVertsIndex = i & 3;
                                m_TempVerts[tempVertsIndex] = verts[i];
                                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                                m_TempVerts[tempVertsIndex].position.x +=
                                    roundingOffset.x + offsets[tempVertsIndex % offsets.Length].x * m_outlineOffset.x;
                                m_TempVerts[tempVertsIndex].position.y +=
                                    roundingOffset.y + offsets[tempVertsIndex % offsets.Length].y * m_outlineOffset.y;
                                if (tempVertsIndex == 3)
                                    AddUIVertexQuad(toFill, m_TempVerts, m_outlineColor);
                            }
                        }
                    }
                    else {
                        for (var oInd = 0; oInd < m_OutlineOffsetBasic.Length; oInd++) {
                            var offsets = m_OutlineOffsetBasic[oInd];
//                            var fitterRate = settings.fontSize / 32f; 
                            for (var i = 0; i < vertCount; ++i) {
                                var tempVertsIndex = i & 3;
                                m_TempVerts[tempVertsIndex] = verts[i];
                                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                                m_TempVerts[tempVertsIndex].position.x +=
                                    offsets[tempVertsIndex % offsets.Length].x * m_outlineOffset.x;
                                m_TempVerts[tempVertsIndex].position.y +=
                                    offsets[tempVertsIndex % offsets.Length].y * m_outlineOffset.y;
                                if (tempVertsIndex == 3)
                                    AddUIVertexQuad(toFill, m_TempVerts, m_outlineColor);
                            }
                        }
                    }

                    break;
                case OutlineType.Fine:
                    if (roundingOffset != Vector2.zero) {
                        for (var oInd = 0; oInd < m_OutlineOffsetFine.Length; oInd++) {
                            var offsets = m_OutlineOffsetFine[oInd];
                            for (var i = 0; i < vertCount; ++i) {
                                var tempVertsIndex = i & 3;
                                m_TempVerts[tempVertsIndex] = verts[i];
                                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                                m_TempVerts[tempVertsIndex].position.x +=
                                    roundingOffset.x + offsets[tempVertsIndex % offsets.Length].x * m_outlineOffset.x;
                                m_TempVerts[tempVertsIndex].position.y +=
                                    roundingOffset.y + offsets[tempVertsIndex % offsets.Length].y * m_outlineOffset.y;
                                if (tempVertsIndex == 3)
                                    AddUIVertexQuad(toFill, m_TempVerts, m_outlineColor);
                            }
                        }
                    }
                    else {
                        for (var oInd = 0; oInd < m_OutlineOffsetFine.Length; oInd++) {
                            var offsets = m_OutlineOffsetFine[oInd];
                            for (var i = 0; i < vertCount; ++i) {
                                var tempVertsIndex = i & 3;
                                m_TempVerts[tempVertsIndex] = verts[i];
                                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                                m_TempVerts[tempVertsIndex].position.x +=
                                    offsets[tempVertsIndex % offsets.Length].x * m_outlineOffset.x;
                                m_TempVerts[tempVertsIndex].position.y +=
                                    offsets[tempVertsIndex % offsets.Length].y * m_outlineOffset.y;
                                if (tempVertsIndex == 3)
                                    AddUIVertexQuad(toFill, m_TempVerts, m_outlineColor);
                            }
                        }
                    }

                    break;
            }

            if (roundingOffset != Vector2.zero) {
                for (var i = 0; i < vertCount; ++i) {
                    var tempVertsIndex = i & 3;
                    m_TempVerts[tempVertsIndex] = verts[i];
                    m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                    m_TempVerts[tempVertsIndex].position.x += roundingOffset.x;
                    m_TempVerts[tempVertsIndex].position.y += roundingOffset.y;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(m_TempVerts);
                }
            }
            else {
                for (var i = 0; i < vertCount; ++i) {
                    var tempVertsIndex = i & 3;
                    m_TempVerts[tempVertsIndex] = verts[i];
                    m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(m_TempVerts);
                }
            }

            m_DisableFontTextureRebuiltCallback = false;
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