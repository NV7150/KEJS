using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectAudios {
    private static AudioClip bgm;
    private static AudioClip hit;
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
}
