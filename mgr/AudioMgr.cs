/** == AudioMgr.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/29 17:29:27
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.core;
using UniKh.extensions;
using UniKh.utils;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UniKh.mgr;
using UnityEditor;
using UnityEngine;


public class AudioMgr : Singleton<AudioMgr> {
    private const string FolderPath = "audio/";  
    
    private Dictionary<string, AudioClip> _audioClips= new Dictionary<string, AudioClip>();

    AudioSource audio = null;

    AudioSource Audio {
        get { return audio ? audio : (audio = this.GetOrAdd<AudioSource>()); }
    }

    public void Play(string audioName) {
        var audioClip = _audioClips.ReadCache(audioName, LoadAudioClip);
        if (null == audioClip) return;
        Audio.PlayOneShot(audioClip, 1F); 
    }

    public AudioClip LoadAudioClip(string audioName) {
        return ResMgr.LazyInst.Load<AudioClip>(FolderPath + audioName);
    }

    [ContextMenu("Play audio - test")]
    public void PlayTest() {
        Play("test");
    }
}