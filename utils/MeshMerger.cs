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
                            transform = parkCenter
                                ? parkCenter.worldToLocalMatrix * localToWorldMatrix
                                : localToWorldMatrix
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

        public static List<MeshRenderer> MergeGroupByMaterial(IEnumerable<MeshRenderer> mrs,
            Transform parkCenter = null) {
            var gb = GroupBy(mrs, parkCenter);
            var newMrs = new List<MeshRenderer>();
            gb.Combine.ForEach(
                (cl, i) => {
                    var meshI = new Mesh();
                    meshI.CombineMeshes(cl.ToArray());
                    newMrs.Add(CreateObjectByCombineInstanceLst(cl.ToArray(), new[] {gb.MatLst[i]}));
                }
            );
            return newMrs;
        }

        /// <summary>
        /// Merge small assets that the total vertex count are not larger than 65535.
        /// </summary>
        /// <param name="mrs">list of MeshRenderer to merge.</param>
        /// <param name="parkCenter">when execute merge, which transform should be considered as the center point.</param>
        /// <param name="name">name of the result object, default is `merged-mesh`.</param>
        /// <returns>MeshRenderer of the merged mesh.</returns>
        public static MeshRenderer Merge(IEnumerable<MeshRenderer> mrs,
            Transform parkCenter = null,
            string name = "merged-mesh") {
            var gb = GroupBy(mrs, parkCenter);
            var vertSum = 0;
            var mergedSubMeshes = gb.Combine.Map(
                (cl, i) => {
                    var meshI = new Mesh();
                    meshI.CombineMeshes(cl.ToArray());
                    vertSum += meshI.vertexCount;
                    return new CombineInstance {
                        mesh = meshI,
                        transform = Matrix4x4.identity
                    };
                }
            );
            
            if(vertSum >= 65535) {
                Debug.LogError("The combined mesh try to generate are exceeded the limit (" + vertSum + "/65535)");
            }
            
            return CreateObjectByCombineInstanceLst(mergedSubMeshes.ToArray(), gb.MatLst.ToArray(), name);
        }

        /// <summary>
        /// Merge assets into a list of blobs.
        /// </summary>
        /// <param name="mrs">list of MeshRenderer to merge.</param>
        /// <param name="parkCenter">when execute merge, which transform should be considered as the center point.</param>
        /// <param name="name">name of the result object, default is `batched-mesh`.</param>
        /// <returns>a list of batched MeshRenderers.</returns>
        public static List<MeshRenderer> Batch(IEnumerable<MeshRenderer> mrs,
            Transform parkCenter = null,
            string name = "batched-mesh") {
            var gb = GroupBy(mrs, parkCenter);
            
            // 1. merge meshes has same mat into a list of meshes that are not larger than 65535.
            var mergedSubMeshes = gb.Combine.Map( 
                (cl, i) => { // for every mat
                    var ret = new List<CombineInstance>();
                    var tempCiLst = new List<CombineInstance>();
                    var vertSum = 0;
                    
                    // flush scanned cis into a mesh
                    Action flush = () => {
                        var meshI = new Mesh();
                        vertSum = 0;
                        meshI.CombineMeshes(tempCiLst.ToArray());
                        tempCiLst.Clear();
                        ret.Add(
                            new CombineInstance {
                                mesh = meshI,
                                transform = Matrix4x4.identity
                            }
                        );
                    };

                    foreach (var c in cl) {
                        if (vertSum + c.mesh.vertices.Length >= 65535 && tempCiLst.Count > 0) { // if the size are about to exceed 65535, flush once.
                            flush();
                        }
                        tempCiLst.Add(c);
                        vertSum += c.mesh.vertexCount;
                    }

                    if (tempCiLst.Count > 0) { // flush results left in the queue.
                        flush();
                    }

                    return ret;
                }
            );

            //2. merge all meshes into a list of blob (of mesh renderer) that are not larger than 65535.
            var retMrs = new List<MeshRenderer>();
            for(var round = 0; mergedSubMeshes.Reduce((p, v) => p || v.Count > 0, false); round ++){
                var cis = new List<CombineInstance>();
                var mats = new List<Material>();

                bool executed;
                var vertSum = 0;
                do {
                    executed = false;
                    for (var i = 0; i < mergedSubMeshes.Count; i++) {
                        if (mergedSubMeshes[i].Count <= 0) continue;
                        if (vertSum != 0 && vertSum + mergedSubMeshes[i].Last().mesh.vertexCount >= 65535) break;
                        executed = true;
                        
                        var ci = mergedSubMeshes[i].PopLast();
                        cis.Add(ci);
                        mats.Add(gb.MatLst[i]);
                        vertSum += ci.mesh.vertexCount;
                    }
                } while (executed);
                
                retMrs.Add(CreateObjectByCombineInstanceLst(cis.ToArray(), mats.ToArray(), name + "_" + round));
            }
            return retMrs;
        }
    }
}