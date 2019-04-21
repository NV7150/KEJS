using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectImages {
    
    private static Sprite hitSprite;
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
