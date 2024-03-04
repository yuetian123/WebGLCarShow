using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor.Rendering.Universal;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.Rendering.Universal.ShaderGUI;
//using UnityEditor.Rendering.Universal.ShaderGUI;

namespace AkilliMum.SRP.CarPaint
{
    internal class CarPaint_2021_2_Editor : BaseShaderGUI
    {
        static readonly string[] workflowModeNames = Enum.GetNames(typeof(LitGUI.WorkflowMode));

        private LitGUI.LitProperties litProperties;
        private LitDetailGUI.LitProperties litDetailProperties;
        //private SavedBool m_DetailInputsFoldout;

        private bool MenuRotation = true;
        private bool MenuReflection = true;
        private bool MenuFresnel = true;
        private bool MenuFlakes = true;
        //private bool MenuSnow = true;
        //private bool MenuBottom = true;
        private bool MenuDecal = true;
        private bool MenuLivery = true;
        private bool MenuDirt = true;

        public override void OnGUI(MaterialEditor materialEditorIn, MaterialProperty[] properties)
        {
            Material targetMat = materialEditorIn.target as Material;



            //todo:
            //MaterialProperty _EnableRotation = ShaderGUI.FindProperty("_EnableRotation", properties);

            //MenuRotation = EditorGUILayout.BeginFoldoutHeaderGroup(MenuRotation, new GUIContent { text = "Rotation" });

            //bool enableRotation = false;
            //if (MenuRotation)
            //{
            //    enableRotation = _EnableRotation.floatValue > 0.5f;
            //    enableRotation = EditorGUILayout.Toggle("Enable Rotation", enableRotation);
            //    _EnableRotation.floatValue = enableRotation ? 1.0f : 0.0f;

            //    if (enableRotation)
            //    {
            //        MaterialProperty rotation = ShaderGUI.FindProperty("_EnviRotation", properties);
            //        materialEditorIn.ShaderProperty(rotation, "Rotation");

            //        MaterialProperty position = ShaderGUI.FindProperty("_EnviPosition", properties);
            //        materialEditorIn.ShaderProperty(position, "Position Correction");

            //    }
            //}

            //EditorGUILayout.EndFoldoutHeaderGroup();

            //if (enableRotation)
            //    targetMat.EnableKeyword("_LCRS_PROBE_ROTATION");
            //else
            //    targetMat.DisableKeyword("_LCRS_PROBE_ROTATION");
            targetMat.DisableKeyword("_LCRS_PROBE_ROTATION");



            MaterialProperty _EnableRealTimeReflection = ShaderGUI.FindProperty("_EnableRealTimeReflection", properties);

            MenuReflection = EditorGUILayout.BeginFoldoutHeaderGroup(MenuReflection, new GUIContent { text = "Reflection" });

            bool realTimeRef = false;
            if (MenuReflection)
            {
                realTimeRef = _EnableRealTimeReflection.floatValue != 0.0f;
                realTimeRef = EditorGUILayout.Toggle("RealTime Reflection", realTimeRef);
                _EnableRealTimeReflection.floatValue = realTimeRef ? 1.0f : 0.0f;

                //if (realTimeRef)
                //{
                //    MaterialProperty _EnviCubeIntensity = ShaderGUI.FindProperty("_EnviCubeIntensity", properties);
                //    materialEditorIn.ShaderProperty(_EnviCubeIntensity, "Intensity");

                //    MaterialProperty _EnviCubeSmoothness = ShaderGUI.FindProperty("_EnviCubeSmoothness", properties);
                //    materialEditorIn.ShaderProperty(_EnviCubeSmoothness, "Smoothness");
                //}
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            //if (realTimeRef)
            //    targetMat.EnableKeyword("_REALTIMEREFLECTION");
            //else
            //    targetMat.DisableKeyword("_REALTIMEREFLECTION");



            MaterialProperty _LiveryMap = ShaderGUI.FindProperty("_LiveryMap", properties);

            MenuLivery = EditorGUILayout.BeginFoldoutHeaderGroup(MenuLivery, new GUIContent { text = "Livery" });
            if (MenuLivery)
            {
                materialEditorIn.TexturePropertySingleLine(
                    new GUIContent { text = "Map", tooltip = "Transparent top paint for liveries" }, _LiveryMap);

                if (_LiveryMap.textureValue != null)
                {
                    MaterialProperty _LiveryUsage = ShaderGUI.FindProperty("_LiveryUsage", properties);
                    _LiveryUsage.floatValue = 1;

                    MaterialProperty _LiveryUV = ShaderGUI.FindProperty("_LiveryUV", properties);
                    materialEditorIn.ShaderProperty(_LiveryUV, "UV");

                    MaterialProperty _LiveryTileOffset = ShaderGUI.FindProperty("_LiveryTileOffset", properties);
                    materialEditorIn.ShaderProperty(_LiveryTileOffset, "Tile and Offset");


                    MaterialProperty _LiveryColor = ShaderGUI.FindProperty("_LiveryColor", properties);
                    materialEditorIn.ColorProperty(_LiveryColor, "Color");

                    MaterialProperty _LiveryMetalic = ShaderGUI.FindProperty("_LiveryMetalic", properties);
                    materialEditorIn.ShaderProperty(_LiveryMetalic, "Metallic");

                    MaterialProperty _LiverySmoothness = ShaderGUI.FindProperty("_LiverySmoothness", properties);
                    materialEditorIn.ShaderProperty(_LiverySmoothness, "Smoothness");
                }
                else
                {
                    MaterialProperty _LiveryUsage = ShaderGUI.FindProperty("_LiveryUsage", properties);
                    _LiveryUsage.floatValue = 0;
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            //if (_LiveryMap.textureValue)
            //    targetMat.EnableKeyword("_LIVERYMAP");
            //else
            //    targetMat.DisableKeyword("_LIVERYMAP");



            MaterialProperty _DecalMap = ShaderGUI.FindProperty("_DecalMap", properties);

            MenuDecal = EditorGUILayout.BeginFoldoutHeaderGroup(MenuDecal, new GUIContent { text = "Decal" });
            if (MenuDecal)
            {
                materialEditorIn.TexturePropertySingleLine(
                    new GUIContent { text = "Map", tooltip = "Transparent top paint for decals" }, _DecalMap);

                if (_DecalMap.textureValue != null)
                {
                    MaterialProperty _DecalUsage = ShaderGUI.FindProperty("_DecalUsage", properties);
                    _DecalUsage.floatValue = 1;

                    MaterialProperty _DecalUV = ShaderGUI.FindProperty("_DecalUV", properties);
                    materialEditorIn.ShaderProperty(_DecalUV, "UV");

                    MaterialProperty _DecalTileOffset = ShaderGUI.FindProperty("_DecalTileOffset", properties);
                    materialEditorIn.ShaderProperty(_DecalTileOffset, "Tile and Offset");


                    MaterialProperty _DecalColor = ShaderGUI.FindProperty("_DecalColor", properties);
                    materialEditorIn.ColorProperty(_DecalColor, "Color");

                    MaterialProperty _DecalMetalic = ShaderGUI.FindProperty("_DecalMetalic", properties);
                    materialEditorIn.ShaderProperty(_DecalMetalic, "Metallic");

                    MaterialProperty _DecalSmoothness = ShaderGUI.FindProperty("_DecalSmoothness", properties);
                    materialEditorIn.ShaderProperty(_DecalSmoothness, "Smoothness");
                }
                else
                {
                    MaterialProperty _DecalUsage = ShaderGUI.FindProperty("_DecalUsage", properties);
                    _DecalUsage.floatValue = 0;
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            //if (_DecalMap.textureValue)
            //    targetMat.EnableKeyword("_DECALMAP");
            //else
            //    targetMat.DisableKeyword("_DECALMAP");



            MaterialProperty _DirtMap = ShaderGUI.FindProperty("_DirtMap", properties);

            MenuDirt = EditorGUILayout.BeginFoldoutHeaderGroup(MenuDirt, new GUIContent { text = "Dirt" });
            if (MenuDirt)
            {
                materialEditorIn.TexturePropertySingleLine(
                    new GUIContent { text = "Map", tooltip = "Transparent top paint for dirt" }, _DirtMap);

                if (_DirtMap.textureValue != null)
                {
                    MaterialProperty _DirtUsage = ShaderGUI.FindProperty("_DirtUsage", properties);
                    _DirtUsage.floatValue = 1;

                    MaterialProperty _DirtUV = ShaderGUI.FindProperty("_DirtUV", properties);
                    materialEditorIn.ShaderProperty(_DirtUV, "UV");

                    MaterialProperty _DirtTileOffset = ShaderGUI.FindProperty("_DirtTileOffset", properties);
                    materialEditorIn.ShaderProperty(_DirtTileOffset, "Tile and Offset");


                    MaterialProperty _DirtColor = ShaderGUI.FindProperty("_DirtColor", properties);
                    materialEditorIn.ColorProperty(_DirtColor, "Color");

                    MaterialProperty _DirtBumpMap = ShaderGUI.FindProperty("_DirtBumpMap", properties);
                    materialEditorIn.TexturePropertySingleLine(
                        new GUIContent { text = "Normal", tooltip = "Normal Map of Dirt" }, _DirtBumpMap);

                    MaterialProperty _DirtMapCutoff = ShaderGUI.FindProperty("_DirtMapCutoff", properties);
                    materialEditorIn.ShaderProperty(_DirtMapCutoff, "Cutoff");

                    MaterialProperty _DirtMetalic = ShaderGUI.FindProperty("_DirtMetalic", properties);
                    materialEditorIn.ShaderProperty(_DirtMetalic, "Metallic");

                    MaterialProperty _DirtSmoothness = ShaderGUI.FindProperty("_DirtSmoothness", properties);
                    materialEditorIn.ShaderProperty(_DirtSmoothness, "Smoothness");
                }
                else
                {
                    MaterialProperty _DirtUsage = ShaderGUI.FindProperty("_DirtUsage", properties);
                    _DirtUsage.floatValue = 0;
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();



            MaterialProperty _FlakesBumpMap = ShaderGUI.FindProperty("_FlakesBumpMap", properties);

            MenuFlakes = EditorGUILayout.BeginFoldoutHeaderGroup(MenuFlakes, new GUIContent { text = "Flakes" });
            if (MenuFlakes)
            {
                materialEditorIn.TexturePropertySingleLine(
                    new GUIContent { text = "Map", tooltip = "Normal Map of Flakes" }, _FlakesBumpMap);

                if (_FlakesBumpMap.textureValue != null)
                {
                    MaterialProperty _FlakesUsage = ShaderGUI.FindProperty("_FlakesUsage", properties);
                    _FlakesUsage.floatValue = 1;

                    MaterialProperty _FlakesBumpMapScale = ShaderGUI.FindProperty("_FlakesBumpMapScale", properties);
                    materialEditorIn.ShaderProperty(_FlakesBumpMapScale, "Scale");

                    MaterialProperty _FlakesBumpStrength = ShaderGUI.FindProperty("_FlakesBumpStrength", properties);
                    materialEditorIn.ShaderProperty(_FlakesBumpStrength, "Strength");
                }
                else
                {
                    MaterialProperty _FlakesUsage = ShaderGUI.FindProperty("_FlakesUsage", properties);
                    _FlakesUsage.floatValue = 0;
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();



            MenuFresnel = EditorGUILayout.BeginFoldoutHeaderGroup(MenuFresnel, new GUIContent { text = "Fresnel" });
            if (MenuFresnel)
            {
                MaterialProperty _FresnelColor = ShaderGUI.FindProperty("_FresnelColor", properties);
                materialEditorIn.ColorProperty(_FresnelColor, "Color 1");
                MaterialProperty _FresnelColor2 = ShaderGUI.FindProperty("_FresnelColor2", properties);
                materialEditorIn.ColorProperty(_FresnelColor2, "Color 2");
                //materialEditorIn.ShaderProperty(_FresnelColor, "Color");

                MaterialProperty _FresnelPower = ShaderGUI.FindProperty("_FresnelPower", properties);
                materialEditorIn.ShaderProperty(_FresnelPower, "Power");
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            MaterialProperty _AddColor = ShaderGUI.FindProperty("_AddColor", properties);
            materialEditorIn.ColorProperty(_AddColor, "Add Color");
            //call base!
            base.OnGUI(materialEditorIn, properties);

        }

        public override void FillAdditionalFoldouts(MaterialHeaderScopeList materialScopesList)
        {
            materialScopesList.RegisterHeaderScope(LitDetailGUI.Styles.detailInputs, Expandable.Details, _ => LitDetailGUI.DoDetailArea(litDetailProperties, materialEditor));
        }

        // collect properties from the material properties
        public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            litProperties = new LitGUI.LitProperties(properties);
            litDetailProperties = new LitDetailGUI.LitProperties(properties);
        }

        // material changed check
        public override void ValidateMaterial(Material material)
        {
            SetMaterialKeywords(material, LitGUI.SetMaterialKeywords, LitDetailGUI.SetMaterialKeywords);
        }

        // material main surface options
        public override void DrawSurfaceOptions(Material material)
        {
            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            if (litProperties.workflowMode != null)
                DoPopup(LitGUI.Styles.workflowModeText, litProperties.workflowMode, workflowModeNames);

            base.DrawSurfaceOptions(material);
        }

        // material main surface inputs
        public override void DrawSurfaceInputs(Material material)
        {
            base.DrawSurfaceInputs(material);
            LitGUI.Inputs(litProperties, materialEditor, material);
            DrawEmissionProperties(material, true);
            DrawTileOffset(materialEditor, baseMapProp);
        }

        // material main advanced options
        public override void DrawAdvancedOptions(Material material)
        {
            if (litProperties.reflections != null && litProperties.highlights != null)
            {
                materialEditor.ShaderProperty(litProperties.highlights, LitGUI.Styles.highlightsText);
                materialEditor.ShaderProperty(litProperties.reflections, LitGUI.Styles.reflectionsText);
            }

            base.DrawAdvancedOptions(material);
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }

            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
            {
                SetupMaterialBlendMode(material);
                return;
            }

            SurfaceType surfaceType = SurfaceType.Opaque;
            BlendMode blendMode = BlendMode.Alpha;
            if (oldShader.name.Contains("/Transparent/Cutout/"))
            {
                surfaceType = SurfaceType.Opaque;
                material.SetFloat("_AlphaClip", 1);
            }
            else if (oldShader.name.Contains("/Transparent/"))
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                surfaceType = SurfaceType.Transparent;
                blendMode = BlendMode.Alpha;
            }
            material.SetFloat("_Blend", (float)blendMode);

            material.SetFloat("_Surface", (float)surfaceType);
            if (surfaceType == SurfaceType.Opaque)
            {
                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
            else
            {
                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }

            if (oldShader.name.Equals("Standard (Specular setup)"))
            {
                material.SetFloat("_WorkflowMode", (float)LitGUI.WorkflowMode.Specular);
                Texture texture = material.GetTexture("_SpecGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }
            else
            {
                material.SetFloat("_WorkflowMode", (float)LitGUI.WorkflowMode.Metallic);
                Texture texture = material.GetTexture("_MetallicGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }
        }
    }

    internal class LitDetailGUI
    {
        public static class Styles
        {
            public static readonly GUIContent detailInputs = EditorGUIUtility.TrTextContent("Detail Inputs",
                "These settings define the surface details by tiling and overlaying additional maps on the surface.");

            public static readonly GUIContent detailMaskText = EditorGUIUtility.TrTextContent("Mask",
                "Select a mask for the Detail map. The mask uses the alpha channel of the selected texture. The Tiling and Offset settings have no effect on the mask.");

            public static readonly GUIContent detailAlbedoMapText = EditorGUIUtility.TrTextContent("Base Map",
                "Select the surface detail texture.The alpha of your texture determines surface hue and intensity.");

            public static readonly GUIContent detailNormalMapText = EditorGUIUtility.TrTextContent("Normal Map",
                "Designates a Normal Map to create the illusion of bumps and dents in the details of this Material's surface.");

            public static readonly GUIContent detailAlbedoMapScaleInfo = EditorGUIUtility.TrTextContent("Setting the scaling factor to a value other than 1 results in a less performant shader variant.");
        }

        public struct LitProperties
        {
            public MaterialProperty detailMask;
            public MaterialProperty detailAlbedoMapScale;
            public MaterialProperty detailAlbedoMap;
            public MaterialProperty detailNormalMapScale;
            public MaterialProperty detailNormalMap;

            public LitProperties(MaterialProperty[] properties)
            {
                detailMask = BaseShaderGUI.FindProperty("_DetailMask", properties, false);
                detailAlbedoMapScale = BaseShaderGUI.FindProperty("_DetailAlbedoMapScale", properties, false);
                detailAlbedoMap = BaseShaderGUI.FindProperty("_DetailAlbedoMap", properties, false);
                detailNormalMapScale = BaseShaderGUI.FindProperty("_DetailNormalMapScale", properties, false);
                detailNormalMap = BaseShaderGUI.FindProperty("_DetailNormalMap", properties, false);
            }
        }

        public static void DoDetailArea(LitProperties properties, MaterialEditor materialEditor)
        {
            materialEditor.TexturePropertySingleLine(Styles.detailMaskText, properties.detailMask);
            materialEditor.TexturePropertySingleLine(Styles.detailAlbedoMapText, properties.detailAlbedoMap,
                properties.detailAlbedoMap.textureValue != null ? properties.detailAlbedoMapScale : null);
            if (properties.detailAlbedoMapScale.floatValue != 1.0f)
            {
                EditorGUILayout.HelpBox(Styles.detailAlbedoMapScaleInfo.text, MessageType.Info, true);
            }
            materialEditor.TexturePropertySingleLine(Styles.detailNormalMapText, properties.detailNormalMap,
                properties.detailNormalMap.textureValue != null ? properties.detailNormalMapScale : null);
            materialEditor.TextureScaleOffsetProperty(properties.detailAlbedoMap);
        }

        public static void SetMaterialKeywords(Material material)
        {
            if (material.HasProperty("_DetailAlbedoMap") && material.HasProperty("_DetailNormalMap") && material.HasProperty("_DetailAlbedoMapScale"))
            {
                bool isScaled = material.GetFloat("_DetailAlbedoMapScale") != 1.0f;
                bool hasDetailMap = material.GetTexture("_DetailAlbedoMap") || material.GetTexture("_DetailNormalMap");
                CoreUtils.SetKeyword(material, "_DETAIL_MULX2", !isScaled && hasDetailMap);
                CoreUtils.SetKeyword(material, "_DETAIL_SCALED", isScaled && hasDetailMap);
            }
        }
    }

    internal class SavedBool
    {
        private bool m_Value;
        private string m_Name;

        public bool value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                if (this.m_Value == value)
                    return;
                this.m_Value = value;
                EditorPrefs.SetBool(this.m_Name, value);
            }
        }

        public SavedBool(string name, bool value)
        {
            this.m_Name = name;
            this.m_Value = EditorPrefs.GetBool(name, value);
        }

        public static implicit operator bool(SavedBool s)
        {
            return s.value;
        }
    }
}
