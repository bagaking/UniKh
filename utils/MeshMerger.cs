using System;
using System.Collections.Generic;
using UniKh.extensions;
using UnityEngine;

namespace UniKh.utils {
    public class MeshMerger {
        public sealed class GroupByResult {
            private Dictionary<Material, int> mats = new Dictionary<Material, int>();

            public List<Material> MatLst { get; } = new List<Material>();

            public List<List<CombineInstance>> Combine { get; } = new List<List<CombineInstance>>();

            internal void ClearAll() {
                mats.Clear();
                MatLst.Clear();
                Combine.Clear();
            }

            internal int CacheMat(Material mat) {
                if (mats.ContainsKey(mat)) return mats[mat];
                Combine.Add(new List<CombineInstance>());
                MatLst.Add(mat);
                return mats[mat] = mats.Count;
            }

            internal void Exec(MeshRenderer mr, Transform parkCenter = null) {
                if (mr == null) return;
                var mf = mr.GetComponent<MeshFilter>();
                if (mf == null || mf.sharedMesh == null) return;
                var transMr = mr.transform;
                var localToWorldMatrix = transMr.localToWorldMatrix;
                for (var subi = 0; subi < mf.sharedMesh.subMeshCount; subi++) {
                    var mat = mr.sharedMaterials[subi];
                    var indCombineLst = CacheMat(mat);
                    Combine[indCombineLst].Add(
                        new CombineInstance {
                            mesh = mf.sharedMesh,
                            subMeshIndex = subi,
                            transform = parkCenter ? parkCenter.worldToLocalMatrix * localToWorldMatrix : localToWorldMatrix
                        }
                    );
                }
            }
        }


        public static MeshRenderer CreateObjectByCombineInstanceLst(CombineInstance[] cis,
            Material[] mats,
            string name = "new-combine") {
            var trans = new GameObject(name).transform;
            var mesh = trans.GetOrAdd<MeshFilter>().mesh = new Mesh();
            mesh.CombineMeshes(cis, false);
            var mr = trans.GetOrAdd<MeshRenderer>();
            mr.sharedMaterials = mats;
            trans.SetObjectActive();
            Debug.Log(SGen.New["mesh generated: sub-count("][mesh.subMeshCount][")"].End);
            return mr;
        }

        public static GroupByResult GroupBy(IEnumerable<MeshRenderer> mrs, Transform parkCenter = null) {
            var gb = new GroupByResult();
            foreach (var mr in mrs) {
                gb.Exec(mr, parkCenter);
            }

            return gb;
        }

        public static List<MeshRenderer> MergeGroupByMaterial(IEnumerable<MeshRenderer> mrs, Transform parkCenter = null) {
            var gb = GroupBy(mrs, parkCenter);
            var newMrs = new List<MeshRenderer>();
            gb.Combine.ForEach(
                (cl, i) => {
                    var meshI = new Mesh();
                    meshI.CombineMeshes(cl.ToArray());
                    newMrs.Add(CreateObjectByCombineInstanceLst(cl.ToArray(), new [] {gb.MatLst[i]}));
                }
            );
            return newMrs;
        }

        public static MeshRenderer Merge(IEnumerable<MeshRenderer> mrs, Transform parkCenter = null, string name = "merged-mesh") {
            var gb = GroupBy(mrs, parkCenter);
            var mergedSubMeshes = gb.Combine.Map(
                (cl, i) => {
                    var meshI = new Mesh();
                    meshI.CombineMeshes(cl.ToArray());
                    // CreateObjectByCombineInstanceLst(cl.ToArray(), new [] {gb.MatLst[i]});
                    return new CombineInstance {
                        mesh = meshI,
                        transform = Matrix4x4.identity
                    };
                }
            );

            return CreateObjectByCombineInstanceLst(mergedSubMeshes.ToArray(), gb.MatLst.ToArray(), name);
        }
    }
}