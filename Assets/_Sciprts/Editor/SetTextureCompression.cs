using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class SetTextureCompression
{
    //****************************************����������**********begin
    //TODO ����EditWindow����

    private static TextureImporterFormat format = TextureImporterFormat.ASTC_12x12;  //ͼƬѹ����ʽ
    private static int compressionQuality = 60;                                      //ѹ������
    private static string platform = "WebGL";                                        //������ƽ̨ 

    //************************************************************end

    /// <summary>
    /// ������ͼ��buildʱ��ѹ��ѡ��
    /// </summary>
    [MenuItem("Assets/���÷���WebGLʱ��ͼ��ѹ����ʽ")]
    static void SetCompression()
    {
        int count = 0;

        Object[] textures = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
        if (textures.Length > 0)
        {
            foreach (Object texture in textures)
            {
                // �����ض�ƽ̨ѹ��ʵ��
                TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings();
                platformSettings.overridden = true;
                platformSettings.name = platform;

                // ����Ϊѹ��
                platformSettings.textureCompression = TextureImporterCompression.Compressed;

                // ����ѹ����ʽ
                platformSettings.resizeAlgorithm = TextureResizeAlgorithm.Bilinear;
                platformSettings.format = format;                                     //TextureImporterFormat.ASTC_12x12;
                platformSettings.compressionQuality = compressionQuality;             //40
                //platformSettings.maxTextureSize = GetMaxSize(texture as Texture2D);   //32
                platformSettings.maxTextureSize = 4096;


                //����importSettings
                TextureImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture)) as TextureImporter;
                if (importer != null)
                {
                    importer.SetPlatformTextureSettings(platformSettings);

                    //Apply ����
                    importer.SetPlatformTextureSettings(platformSettings);

                    //������Դ
                    importer.SaveAndReimport();
                }

                count++;
            }
            //Debug.Log("Texture Compression Set!");
        }
        else
        {
            Debug.LogWarning("û��ѡ��ͼƬ!");
        }
        Debug.Log($"һ��������{count}��ͼƬ��");
    }

    /// <summary>
    /// ��ȡͼƬ�ķֱ��ʣ�ȡ�ֱ����и߿�����ֵ��Ȼ�󷵻�ͼƬ�ġ�MaxSize��
    /// MaxSize�Ķ��壺assets->Image->��Texture2D ImportSettings��->��Override For WebGL��->��Max Size�� 
    /// ���䣺16,32,64,128,256,512,1024,2048,4096,8192,16384
    ///
    /// ������ͼƬ�ֱ��� = 12 * 24����ôͼƬ��MaxSize = 32
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    static int GetMaxSize(Texture2D texture)
    {
        //�ֱ��������Ԥ��
        var start = new List<int> { 0, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        var end = new List<int> { 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 100000 };
        var zones = start.Zip(end, (item1, item2) => (startIdx: item1, endIdx: item2)).ToList();

        //ȡ�ֱ��ʸ߿�����ֵ
        var size = new List<int> { texture.width, texture.height }.Max();  //ȡ�������ߡ��е����ֵ

        //�ж�����������
        var maxSize = zones
            .First(x => x.startIdx <= size && size <= x.endIdx)
            .endIdx;
        //Debug.Log($"ͼ�ķֱ��� = {texture.width} * {texture.height} size = {size}, MaxSize = {maxSize}");
        return maxSize;
    }
}

