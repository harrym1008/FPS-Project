using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI;

public class GraphicalSettingsMenu : MonoBehaviour
{
    [SerializeField] private Graphics currentSettings = new Graphics();
    [SerializeField] private UniversalRenderPipelineAsset pipeline;

    [SerializeField] private TMP_Dropdown[] dropdowns;
    [SerializeField] private Toggle[] toggles;


    public Graphics[] presets = new Graphics[]
    {
        new Graphics
        {
            shadows = Graphics.Shadows.Low,
            shadowDistance = Graphics.ShadowDistance.M25,
            textureResolution = Graphics.TextureResolution.Quarter,
            renderScale = Graphics.RenderScale.__75,
            levelOfDetail = Graphics.LevelOfDetail.Low,
            antiAliasing = Graphics.AntiAliasing.None,
            anistropic = false,
            hdr = false
        },

        new Graphics
        {
            shadows = Graphics.Shadows.Medium,
            shadowDistance = Graphics.ShadowDistance.M50,
            textureResolution = Graphics.TextureResolution.Half,
            renderScale = Graphics.RenderScale._1,
            levelOfDetail = Graphics.LevelOfDetail.Medium,
            antiAliasing = Graphics.AntiAliasing.MSAA2,
            anistropic = true,
            hdr = false
        },

        new Graphics
        {
            shadows = Graphics.Shadows.High,
            shadowDistance = Graphics.ShadowDistance.M75,
            textureResolution = Graphics.TextureResolution.Full,
            renderScale = Graphics.RenderScale._1,
            levelOfDetail = Graphics.LevelOfDetail.High,
            antiAliasing = Graphics.AntiAliasing.MSAA4,
            anistropic = true,
            hdr = true
        },

        new Graphics
        {
            shadows = Graphics.Shadows.Ultra,
            shadowDistance = Graphics.ShadowDistance.M100,
            textureResolution = Graphics.TextureResolution.Full,
            renderScale = Graphics.RenderScale._2,
            levelOfDetail = Graphics.LevelOfDetail.High,
            antiAliasing = Graphics.AntiAliasing.MSAA8,
            anistropic = true,
            hdr = true
        }
    };

    private void Awake()
    {
        SelectPreset(1);
        Apply();
    }

    public void SetShadows(int value) { currentSettings.shadows = (Graphics.Shadows)value; }
    public void SetShadowDistancce(int value) { currentSettings.shadowDistance = (Graphics.ShadowDistance)value; }
    public void SetTextureRes(int value) { currentSettings.textureResolution = (Graphics.TextureResolution)value; }
    public void SetRenderScale(int value) { currentSettings.renderScale = (Graphics.RenderScale)value; }
    public void SetLOD(int value) { currentSettings.levelOfDetail = (Graphics.LevelOfDetail)value; }
    public void SetAA(int value) { currentSettings.antiAliasing = (Graphics.AntiAliasing)value; }
    public void SetAnistropic(bool value) { currentSettings.anistropic = value; }
    public void SetHDR(bool value) { currentSettings.hdr = value; }


    public void UpdateEntries()
    {
        dropdowns[0].SetValueWithoutNotify((int)currentSettings.shadows);
        dropdowns[1].SetValueWithoutNotify((int)currentSettings.shadowDistance);
        dropdowns[2].SetValueWithoutNotify((int)currentSettings.textureResolution);
        dropdowns[3].SetValueWithoutNotify((int)currentSettings.renderScale);
        dropdowns[4].SetValueWithoutNotify((int)currentSettings.levelOfDetail);
        dropdowns[5].SetValueWithoutNotify((int)currentSettings.antiAliasing);

        toggles[0].SetIsOnWithoutNotify(currentSettings.anistropic);
        toggles[1].SetIsOnWithoutNotify(currentSettings.hdr);
    }


    public void SelectPreset(int preset)
    {
        currentSettings.shadows = presets[preset].shadows;
        currentSettings.shadowDistance = presets[preset].shadowDistance;
        currentSettings.textureResolution = presets[preset].textureResolution;
        currentSettings.renderScale = presets[preset].renderScale;
        currentSettings.levelOfDetail = presets[preset].levelOfDetail;
        currentSettings.antiAliasing = presets[preset].antiAliasing;
        currentSettings.anistropic = presets[preset].anistropic;
        currentSettings.hdr = presets[preset].hdr;

        UpdateEntries();
    }


    public void Apply()
    {
        print("New gfx settings:\n" + currentSettings.ToString());

        pipeline.colorGradingMode = currentSettings.hdr ? ColorGradingMode.HighDynamicRange : ColorGradingMode.LowDynamicRange;

        if (currentSettings.anistropic)
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        else
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;


        if (currentSettings.hdr)
            pipeline.colorGradingMode = ColorGradingMode.HighDynamicRange;
        else
            pipeline.colorGradingMode = ColorGradingMode.LowDynamicRange;




        switch (currentSettings.textureResolution)
        {
            case Graphics.TextureResolution.Full:
                QualitySettings.masterTextureLimit = 0;
                break;
            case Graphics.TextureResolution.Half:
                QualitySettings.masterTextureLimit = 1;
                break;
            case Graphics.TextureResolution.Quarter:
                QualitySettings.masterTextureLimit = 2;
                break;
            case Graphics.TextureResolution.Eighth:
                QualitySettings.masterTextureLimit = 3;
                break;
            default:
                break;
        }

        switch (currentSettings.levelOfDetail)
        {
            case Graphics.LevelOfDetail.VeryLow:
                QualitySettings.lodBias = 0.1f;
                break;
            case Graphics.LevelOfDetail.Low:
                QualitySettings.lodBias = 0.5f;
                break;
            case Graphics.LevelOfDetail.Medium:
                QualitySettings.lodBias = 1f;
                break;
            case Graphics.LevelOfDetail.High:
                QualitySettings.lodBias = 1.5f;
                break;
            case Graphics.LevelOfDetail.Ultra:
                QualitySettings.lodBias = 2.5f;
                break;
            default:
                break;
        }

        switch (currentSettings.renderScale)
        {
            case Graphics.RenderScale.__125:
                pipeline.renderScale = 0.125f;
                break;
            case Graphics.RenderScale.__25:
                pipeline.renderScale = 0.25f;
                break;
            case Graphics.RenderScale.__5:
                pipeline.renderScale = 0.5f;
                break;
            case Graphics.RenderScale.__75:
                pipeline.renderScale = 0.75f;
                break;
            case Graphics.RenderScale._1:
                pipeline.renderScale = 1f;
                break;
            case Graphics.RenderScale._1_5:
                pipeline.renderScale = 1.5f;
                break;
            case Graphics.RenderScale._2:
                pipeline.renderScale = 2f;
                break;
            default:
                break;
        }

        switch (currentSettings.shadowDistance)
        {
            case Graphics.ShadowDistance.M10:
                pipeline.shadowDistance = 10f;
                break;
            case Graphics.ShadowDistance.M25:
                pipeline.shadowDistance = 25f;
                break;
            case Graphics.ShadowDistance.M50:
                pipeline.shadowDistance = 50f;
                break;
            case Graphics.ShadowDistance.M75:
                pipeline.shadowDistance = 75f;
                break;
            case Graphics.ShadowDistance.M100:
                pipeline.shadowDistance = 100f;
                break;
            default:
                break;
        }

        print((int)Mathf.Pow(2, (int)currentSettings.antiAliasing));
        pipeline.msaaSampleCount = (int) Mathf.Pow(2, (int)currentSettings.antiAliasing);

        switch (currentSettings.shadows)
        {
            case Graphics.Shadows.None:
                UnityGraphicsBullshit.AdditionalLightCastShadows = false;
                UnityGraphicsBullshit.MainLightCastShadows = false;
                break;

            case Graphics.Shadows.Low:
                UnityGraphicsBullshit.SoftShadowsEnabled = false;
                UnityGraphicsBullshit.AdditionalLightCastShadows = false;
                UnityGraphicsBullshit.MainLightCastShadows = true;
                UnityGraphicsBullshit.MainLightShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._256;
                break;

            case Graphics.Shadows.Medium:
                UnityGraphicsBullshit.SoftShadowsEnabled = true;
                UnityGraphicsBullshit.AdditionalLightCastShadows = true;
                UnityGraphicsBullshit.MainLightCastShadows = true;
                UnityGraphicsBullshit.MainLightShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._512;
                UnityGraphicsBullshit.AdditionalLightShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._256;
                break;

            case Graphics.Shadows.High:
                UnityGraphicsBullshit.SoftShadowsEnabled = true;
                UnityGraphicsBullshit.AdditionalLightCastShadows = true;
                UnityGraphicsBullshit.MainLightCastShadows = true;
                UnityGraphicsBullshit.MainLightShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._2048;
                UnityGraphicsBullshit.AdditionalLightShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._1024;
                break;

            case Graphics.Shadows.Ultra:
                UnityGraphicsBullshit.SoftShadowsEnabled = true;
                UnityGraphicsBullshit.AdditionalLightCastShadows = true;
                UnityGraphicsBullshit.MainLightCastShadows = true;
                UnityGraphicsBullshit.MainLightShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._4096;
                UnityGraphicsBullshit.AdditionalLightShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._4096;
                break;

            default:
                break;
        }
    }


    [System.Serializable]
    public class Graphics
    {
        public enum Shadows
        {
            None,
            Low,
            Medium,
            High,
            Ultra
        }

        public enum ShadowDistance
        {
            M10,
            M25,
            M50,
            M75,
            M100
        }

        public enum TextureResolution
        {
            Full,
            Half,
            Quarter,
            Eighth
        }

        public enum RenderScale
        {
            __125,
            __25,
            __5,
            __75,
            _1,
            _1_5,
            _2
        }

        public enum LevelOfDetail
        {
            VeryLow,
            Low,
            Medium,
            High,
            Ultra
        }

        public enum AntiAliasing
        {
            None,
            MSAA2,
            MSAA4,
            MSAA8
        }

        public Shadows shadows;
        public ShadowDistance shadowDistance;
        public TextureResolution textureResolution;
        public RenderScale renderScale;
        public LevelOfDetail levelOfDetail;
        public AntiAliasing antiAliasing;
        public bool anistropic;
        public bool hdr;

        public override string ToString()
        {
            return $@"Shadows: {shadows}
Shadow Distance: {shadowDistance}
Texture Res: {textureResolution}
Render Scale: {renderScale}
LOD: {levelOfDetail}
AntiAlias: {antiAliasing}
Anistropic: {anistropic}
HDR: {hdr}";
        }
    }

    
}



