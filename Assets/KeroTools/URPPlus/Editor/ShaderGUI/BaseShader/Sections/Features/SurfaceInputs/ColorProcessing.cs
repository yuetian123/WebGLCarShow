using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class ColorProcessing : IDrawable
    {
        private static readonly int SaturationID = Shader.PropertyToID("_Saturation");
        private static readonly int ContrastID = Shader.PropertyToID("_Contrast");
        private static readonly int BrightnessID = Shader.PropertyToID("_Brightness");
        private static readonly int HueShiftID = Shader.PropertyToID("_HueShift");
        private Material _material;
        private MaterialProperty _saturationProperty;
        private MaterialProperty _contrastProperty;
        private MaterialProperty _brightnessProperty;
        private MaterialProperty _hueShiftProperty;
        
        public ColorProcessing(Material material)
        {
            _material = material;
        }
        public virtual void FindProperties(MaterialProperty[] properties)
        {
            _saturationProperty = PropertyFinder.FindOptionalProperty("_Saturation", properties);
            _contrastProperty = PropertyFinder.FindOptionalProperty("_Contrast", properties);
            _brightnessProperty = PropertyFinder.FindOptionalProperty("_Brightness", properties);
            _hueShiftProperty = PropertyFinder.FindOptionalProperty("_HueShift", properties);
        }
        public virtual void Draw(PropertiesEditor editor)
        {
            editor.DrawFloat(SurfaceInputsStyles.Saturation, _saturationProperty);
            editor.DrawFloat(SurfaceInputsStyles.Contrast, _contrastProperty);
            editor.DrawFloat(SurfaceInputsStyles.Brightness, _brightnessProperty);
            editor.DrawSlider(SurfaceInputsStyles.HueShift, _hueShiftProperty, -180, 180);
        }
        
        public virtual void SetKeywords(Material material)
        {
            throw new System.NotImplementedException();
        }

        
    }
}