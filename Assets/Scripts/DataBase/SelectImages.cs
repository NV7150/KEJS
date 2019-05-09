using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectImages {
    public static readonly string HIT_FILE_TAG = "I_HIT";
    private static Sprite hitSprite;
    public static readonly string IPPON_FILE_TAG = "I_IPPON";
    private static Sprite ipponSprite;

    public static Sprite HitSprite {
        get { return hitSprite; }
        set { hitSprite = value; }
    }

    public static Sprite IpponSprite {
        get { return ipponSprite; }
        set { ipponSprite = value; }
    }
    
    static SelectImages() {
        hitSprite = Resources.Load<Sprite>("DefalutHit");
        ipponSprite = Resources.Load<Sprite>("DefalutIppon");
    }
}
