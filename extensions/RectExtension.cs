/** == RectExtension.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/24 17:37:04
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.core;
using UnityEngine;

namespace UniKh.extensions {
    
    public static class RectExtension {
        
        public static Vector2 GetPA(this Rect rSelf) {
            return new Vector2(rSelf.min.x, rSelf.max.y);
        }
        
        public static Vector2 GetPB(this Rect rSelf) {
            return new Vector2(rSelf.max.x, rSelf.min.y);
        }
        
        public static Vector2 ProjectionTo(this Rect rSelf, Vector2 direction) {
            var min = Vector2.Dot(rSelf.min, direction);
            var max = min;

            var vMax = Vector2.Dot(rSelf.max, direction);
            if (vMax < min) {
                min = vMax;
            }else if (vMax > max) {
                max = vMax;
            }
            
            var vPA = Vector2.Dot(rSelf.GetPA(), direction);
            if (vPA < min) {
                min = vPA;
            }else if (vPA > max) {
                max = vPA;
            }
            
            var vPB = Vector2.Dot(rSelf.GetPB(), direction);
            if (vPB < min) {
                min = vPB;
            }else if (vPB > max) {
                max = vPB;
            } 
            
            return new Vector2(min, max);
        }
        
    }
}