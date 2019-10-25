/** TransformExtension.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/22 23:54:34
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Collections.Generic;
using UniKh.utils;
using UnityEngine;

namespace UniKh.extensions {
    public static class TransformExtension {
        const int MAX_PATH_LANGTH = 20;
         
                 public static List<Transform>
                     GetDirectChildren(this Transform transSelf, Predicate<Transform> transFunc = null) {
                     var ret = new List<Transform>();
                     for (var i = 0; i < transSelf.childCount; i++) {
                         var tNode = transSelf.GetChild(i);
                         if (transFunc == null || transFunc(tNode)) ret.Add(tNode);
                     }
         
                     return ret;
                 }
         
                 private static readonly List<Transform> PathTransforms = new List<Transform>(MAX_PATH_LANGTH).ChangeLength(MAX_PATH_LANGTH);
         
                 public static string GetPathName(this Transform transSelf) {
                     var cursor = MAX_PATH_LANGTH - 1;
                     PathTransforms[cursor] = transSelf;
                     Transform parent;
                     while (null != (parent = PathTransforms[cursor--].parent) && cursor >= 0) {
                         PathTransforms[cursor] = parent;
                     }
                     var builder = SGen.New;
                     while (true) {
                         builder.Append(PathTransforms[++cursor].name);
                         if (cursor >= MAX_PATH_LANGTH - 1) break;
                         builder.Append("/");
                     }
         
                     return builder.End;
                 }
    }
}