using UnityEngine;

public class TransparencyScript : MonoBehaviour
{
    private Shader transparentShader;
    private Shader opaqueShader;
    private Texture2D baseMap;
    private Texture2D normalMap;
    private Texture2D metallicMap;
    private Texture2D emissionMap;
    private float metallic;
    private float smoothness;

    private void Start()
    {
        transparentShader = Shader.Find("Shader Graphs/TransparentShader");
        opaqueShader = Shader.Find("Universal Render Pipeline/Lit");
    }

    public void NewObject(GameObject obj, float transparency)
    {
        if (transparency == 1)
            TurnOpaque(obj);
        else
            TurnTransparent(obj, transparency);
    }


    public void TurnOpaque(GameObject obj)
    {
        if (obj.TryGetComponent(out BarracksTower objBarracks))
        {
            TurnOpaque(objBarracks.barrackModels[objBarracks.upgradeCount]);
            return;
        }

        obj.GetComponent<Renderer>().material.shader = opaqueShader;
    }

    public void TurnTransparent(GameObject obj, float transparency)
    {
        if (obj.TryGetComponent(out BarracksTower objBarracks))
        {
            TurnTransparent(objBarracks.barrackModels[objBarracks.upgradeCount], transparency);
            return;
        }

        Material objMaterial = obj.GetComponent<Renderer>().material;


        if (objMaterial.HasProperty("_BaseMap"))
        {
            Texture baseMapOG = objMaterial.GetTexture("_BaseMap");
            baseMap = baseMapOG as Texture2D;
        }

        if (objMaterial.HasProperty("_BumpMap"))
        {
            Texture normalMapOG = objMaterial.GetTexture("_BumpMap");
            normalMap = normalMapOG as Texture2D;
        }

        if (objMaterial.HasProperty("_MetallicGlossMap"))
        {
            Texture metallicMapOG = objMaterial.GetTexture("_MetallicGlossMap");
            metallicMap = metallicMapOG as Texture2D;
        }

        if (objMaterial.HasProperty("_EmissionMap"))
        {
            Texture emissionMapOG = objMaterial.GetTexture("_EmissionMap");
            emissionMap = emissionMapOG as Texture2D;
        }

        if (objMaterial.HasProperty("_Metallic"))
            metallic = objMaterial.GetFloat("_Metallic");

        if (objMaterial.HasProperty("_Smoothness"))
            smoothness = objMaterial.GetFloat("_Smoothness");

        objMaterial.shader = transparentShader;
        objMaterial.SetFloat("_Transparency", transparency);

        if (baseMap != null)
            objMaterial.SetTexture("_BaseMap", baseMap);

        if (normalMap != null)
            objMaterial.SetTexture("_NormalMap", normalMap);

        if (metallicMap != null)
            objMaterial.SetTexture("_MetallicMap", metallicMap);

        if (emissionMap != null)
            objMaterial.SetTexture("_EmissionMap", emissionMap);

        objMaterial.SetFloat("_Metallic", metallic);

        objMaterial.SetFloat("_Smoothness", smoothness);

        baseMap = null;
        normalMap = null;
        metallicMap = null;
        emissionMap = null;
        metallic = 0;
        smoothness = 0.5f;
    }
}
