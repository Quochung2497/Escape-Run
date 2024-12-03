using UnityEditor;

namespace Watermelon
{
    /// <summary>
    /// Don't be shy and edit this file if you need to add new presets or change default settings.
    /// Read comment on below to understand how to set up new presets.
    /// </summary>
    public partial class TextureSmartImporter
    {
        private const int DEFAULT_PRESET_INDEX = 0;

        private readonly Preset[] presets = new Preset[]
        {
        new Preset()
        {
            name = "Default",
            optimizeSize = false,
            spriteAlignment = UnityEngine.SpriteAlignment.Center,
            packingTag = "",
            pixelsPerUnit = 100,

            presets = new Preset.Settings[]
            {
                new Preset.Settings()
                {
                    name = "Default",
                    maxTextureSize = 2048,
                    textureCompression = TextureImporterCompression.Compressed,
                    crunchedCompression = false,
                    compresionQuality = 50
                },

                //Clear special platform settings
                new Preset.Settings() { name = "Standalone", clearSettings = true },
                new Preset.Settings() { name = "Android", clearSettings = true },
                new Preset.Settings() { name = "iOS", clearSettings = true }
            }
        },
        new Preset()
        {
            name = "Optimized",
            description = "Best solution if you want to save space",
            optimizeSize = true,
            presets = new Preset.Settings[]
            {
                new Preset.Settings()
                {
                    name = "Default",
                    maxTextureSize = 4096,
                    textureCompression = TextureImporterCompression.CompressedHQ,
                    crunchedCompression = true,
                    compresionQuality = 100
                }
            }
        },

            //EXAMPLE PRESET
            //new Preset()
            //{
            //    name = "Example",                                                   //Preset name displayed in list
            //    description = "Example preset",                                     //Is shown when preset is selected
            //    optimizeSize = true,                                                //Makes texture size divisible by 4 without the rest (required if you want to use crunch compression)
            //    packingTag = "UI",                                                  //Set tag for sprite packer
            //    customPivot = new UnityEngine.Vector2(0.5f, 0.75f),                 //Set custom pivot point
            //    spriteAlignment = UnityEngine.SpriteAlignment.Custom,
            //    pixelsPerUnit = 250,                                                //100 pixels per unit would mean a sprite that's 100 pixels would equal 1 unit in the scene.

            //    presets = new Preset.Settings[]                                     //Must be at least one preset (truly - not, but why you use util if you don't add presets?)
            //    {
            //        new Preset.Settings()
            //        {
            //            name = "Default",                                           //Build target name (Android, iOS, Standalone, WebGL, tvOS, Switch and etc..)
            //            maxTextureSize = 32,                                        //Max texture size (32, 64, 128, 256, 512, 1024, 2048, 4096, 8192)
            //            format = TextureImporterFormat.ARGB16,                      //Here is all required information - https://docs.unity3d.com/ScriptReference/TextureImporterFormat.html
            //            resizeAlgorithm = TextureResizeAlgorithm.Bilinear,          //One more link - https://docs.unity3d.com/ScriptReference/TextureResizeAlgorithm.html
            //            textureCompression = TextureImporterCompression.Compressed, //Yeap, again - https://docs.unity3d.com/ScriptReference/TextureImporterCompression.html
            //            compresionQuality = 100,                                    //When using Crunch Texture compression, use the slider to adjust the quality. A higher compression quality means larger Textures and longer compression times.
            //            crunchedCompression = false,                                //Use crunch compression, if applicable. Crunch is a lossy compression format on top of DXT or ETC Texture compression. Textures are decompressed to DXT or ETC on the CPU and then uploaded on the GPU at runtime. Crunch compression helps the Texture use the lowest possible amount of space on disk and for downloads. Crunch Textures can take a long time to compress, but decompression at runtime is very fast.
            //        },

            //        //Android example settings
            //        new Preset.Settings()
            //        {
            //            name = "Android",
            //            maxTextureSize = 1024,
            //            format = TextureImporterFormat.ASTC_RGB_4x4
            //        },

            //        //Set clearSettings value to true if you want to remove special settings for iOS platform
            //        new Preset.Settings() { name = "iOS", clearSettings = true }
            //    }
            //}
        };
    }
}