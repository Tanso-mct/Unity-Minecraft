using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McVideos : MonoBehaviour
{
    private static bool hasSaveData = false;

    [SerializeField] SelectBarParts fovSb;
    [SerializeField] SelectBarParts renderDistanceSb;
    [SerializeField] SelectBarParts brightnessSb;

    private static int fov = 70;
    public static int Fov { get { return fov; } }
    private static Vector2 fovSbPos;

    private static int renderDistance = 1;
    public static int RenderDistance { get { return renderDistance; } }
    
    private static Vector2 renderDistanceSbPos;

    private static int brightness = 50;
    public static int Brightness { get { return brightness; } }
    
    private static Vector2 brightnessSbPos;

    public void Init()
    {
        if (hasSaveData)
        {
            // Load data
        }
    }

    public void Save()
    {
        // Save data
    }

    public void SetFov()
    {
        fov = (int)fovSb.Val;
        fovSbPos = fovSb.SelectorPos;

        if (fov == 70) fovSb.EditTxt("FOV: Normal");
        else if (fov == 110) fovSb.EditTxt("FOV: Quake Pro");
        else fovSb.EditTxt("FOV: " + fov);
    }

    public void SetRenderDistance()
    {
        renderDistance = (int)renderDistanceSb.Val;
        renderDistanceSbPos = renderDistanceSb.SelectorPos;

        string txt = "Render Distance: " + renderDistance;

        if (renderDistance == 2) txt += " Tiny";
        else if (renderDistance == 3) txt += " Short";
        else if (renderDistance >= 4 && renderDistance < 8) txt += " Short+";
        else if (renderDistance == 8) txt += " Normal";
        else if (renderDistance > 8 && renderDistance < 16) txt += " Normal+";
        else if (renderDistance == 16) txt += " Far";
        else if (renderDistance > 16 && renderDistance < 32) txt += " Far+";
        else if (renderDistance == 32) txt += " Extreme";
        else if (renderDistance > 32 && renderDistance < 48) txt += " Extreme+";
        else if (renderDistance == 48) txt += " Insane";

        renderDistanceSb.EditTxt(txt);
    }

    public void SetBrightness()
    {
        brightness = (int)brightnessSb.Val;
        brightnessSbPos = brightnessSb.SelectorPos;

        if (brightness == 0) brightnessSb.EditTxt("Brightness: Moody");
        else if (brightness > 0 && brightness < 100) brightnessSb.EditTxt("Brightness: +" + brightness + "%");
        else if (brightness == 100) brightnessSb.EditTxt("Brightness: Bright");
    }
}
