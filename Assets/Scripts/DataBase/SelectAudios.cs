using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectAudios {
    public static readonly string BGM_FILE_TAG = "A_BGM";
    private static AudioClip bgm;
    public static readonly string HIT_FILE_TAG = "A_HIT";
    private static AudioClip hit;
    public static readonly string IPPON_FILE_TAG = "A_IPPON";
    private static AudioClip ippon;

    public static AudioClip Bgm {
        get { return bgm; }
        set { bgm = value; }
    }

    public static AudioClip Hit {
        get { return hit; }
        set { hit = value; }
    }

    public static AudioClip Ippon {
        get { return ippon; }
        set { ippon = value; }
    }

    public static void load() {
        
    }

    private static void loadTag(string tag) {
        SettingSaveManger.load(tag);
        
    }
}
