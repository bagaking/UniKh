/** #SCRIPTNAME#.cs
 *  Author:         #AuthorName# <#AuthorEmail#>
 *  CreateTime:     #CreateTime#
 *  Copyright:      (C) 2019 - 2029 #AuthorName#, All Rights Reserved
 */
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ScriptFilePreProcessPreWarm {

    const string tpl = @"
/** == #SCRIPTNAME#.cs ==
 *  Author:         #AuthorName# <#AuthorEmail#>
 *  CreateTime:     #CreateTime#
 *  Copyright:      (C) 2019 - 2029 #AuthorName#, All Rights Reserved
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniKh.core;
using UniKh.extensions;

public class #SCRIPTNAME# : BetterBehavior {

#region Events
    /// <summary>
    /// The initial method of #SCRIPTNAME#.
    /// This method **CAN** be removed
    /// </summary>
    protected override void OnInit() {

    }

    /// <summary>
    /// when the active state changes, the method will be call.
    /// This method **CAN** be removed
    /// </summary>
    protected override void OnSetActive(bool active) {
        if(active) {
            // When this behavior are activated
        } else {
            // When this behavior are disabled
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// This method **CAN** be removed
    /// </summary>
    void Update() {
        #NOTRIM#
    }
#endregion Events

}
";

    static ScriptFilePreProcessPreWarm() {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_XBOXONE || UNITY_WINRT_8_1
        var path = Path.Combine(Path.GetDirectoryName(EditorApplication.applicationPath), "Data", "Resources", "ScriptTemplates", "81-C# Script-NewBehaviourScript.cs.txt");
        var dirPath = Path.GetDirectoryName(path);

        if (!File.Exists(path)) {
            Debug.LogWarning("Try update scripts template failed: file " + path + " cannot be found.");
        }

        try {
            using (FileStream fs = File.Create(Path.Combine(dirPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose)) { }
        } catch (UnauthorizedAccessException) {
            Debug.LogWarning("Try update scripts template failed: has no write access of dir " + dirPath + ".");
            return;
        }

        var localTemplate = File.ReadAllText(path);
        if (localTemplate == tpl) {
            return; // no need to update template
        }

        // Debug.Log("Try update scripts template: " + path);
        // back up old file
        File.Copy(path, path + "."
            + DateTime.Now.ToString().Replace(" ", "_").Replace(":", "-").Replace("/", "-").Replace("\\", "-")
            + ".bak");
        File.WriteAllText(path, tpl);
        Debug.Log("Scripts template updated: " + path);

#else
        // you can only do this at windows right now.
#endif
    }

}

public class ScriptFilePreProcess : UnityEditor.AssetModificationProcessor {

    // todo: read author info form project

    private const string AuthorName = "bagaking";
    private const string AuthorEmail = "kinghand@foxmail.com";

    private const string DateFormat = "yyyy/MM/dd HH:mm:ss";
    private static void OnWillCreateAsset(string path) {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs")) {
            string allText = File.ReadAllText(path);
            allText = allText.Replace("#AuthorName#", AuthorName);
            allText = allText.Replace("#AuthorEmail#", AuthorEmail);
            allText = allText.Replace("#CreateTime#", System.DateTime.Now.ToString(DateFormat));
            File.WriteAllText(path, allText);
            AssetDatabase.Refresh();
        }
    }
}