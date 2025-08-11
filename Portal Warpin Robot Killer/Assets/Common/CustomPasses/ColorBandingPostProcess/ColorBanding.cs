using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Color Banding")]
public sealed class ColorBanding : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);
    public ClampedFloatParameter precision = new ClampedFloatParameter(8, 1, 999);
    public BoolParameter dither = new BoolParameter(false);
    public Vector2Parameter valueRemap = new Vector2Parameter(new(-0.1f, 1.1f));

    Material m_Material;

    public bool IsActive() => m_Material != null && intensity.value > 0f;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Global Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Shader Graphs/ColorBandingPostProcess";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume ColorBanding is unable to load. To fix this, please edit the 'kShaderName' constant in ColorBanding.cs or change the name of your custom post process shader.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        m_Material.SetFloat("_Intensity", intensity.value);
        m_Material.SetFloat("_BandPrecision", precision.value);
        m_Material.SetInt("_Dither", dither.value ? 1 : 0);
        m_Material.SetVector("_ValueRemap", valueRemap.value);

        //cmd.Blit(source, destination, m_Material, pass: 0);
        HDUtils.DrawFullScreen(cmd, m_Material, destination);
    }

    public override void Cleanup() => CoreUtils.Destroy(m_Material);
}
