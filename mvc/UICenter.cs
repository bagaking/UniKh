using System;
using System.Collections.Generic;
using UniKh.core;

namespace UniKh.mvc {
    
    /**
     * usage:
     * 
     * - declaration:   
     *     ```
     *     public class NewUIClass : UICenter.UIControllerBase<NewUIClass> {
     *         public string Str => "hello world";
     *     }
     *     ```
     * - visit:  
     *     ```
     *     Debug.Log(NewUIClass.Inst.Str)
     *     ```   
     */
    public class UICenter {
        
        public static readonly Dictionary<Type, UIControllerBase> UIControllers = new Dictionary<Type, UIControllerBase>();

        public class UIControllerBase : BetterBehavior {
            protected override void OnInit() {
                base.OnInit();
                Reg(this);
            }

            protected void OnDestroy() {
                DeReg(this);
            }
        }

        public static void Reg<T>(T controller) where T : UIControllerBase {
            var type = controller.GetType(); // 注意如果用 typeof, 基类调用则会推断出基类
            if (UIControllers.ContainsKey(type)) {
                if (UIControllers[type] != controller) {
                    throw new Exception(
                        $"UICenter.Reg failed: there are an another controller of type {type} are already registered."
                    );
                }
                return;
            }
            
            UIControllers.Add(type, controller);
        }
        
        public static void DeReg<T>(T controller) where T : UIControllerBase {
            var type = controller.GetType(); 
            if (UIControllers[type] != controller) {
                throw new Exception($"UICenter.DeReg failed: controller of type {type} are not equal to given controller.");
            }

            UIControllers.Remove(type);
        }

        public class UIControllerBase<TSelf> : UIControllerBase where TSelf : UIControllerBase<TSelf> {
            public static TSelf Inst => Get<TSelf>();
        }

        public static T Get<T>() where T : UIControllerBase<T> {
            var type = typeof(T);
            return UIControllers[type] as T;
        }
    }
}