using System.Runtime.InteropServices;
using SolAR.Datastructure;
using UnityEngine;
using UnityEngine.Assertions;

namespace SolAR
{
    public static class TextureExtensions
    {
        static TextureFormat GetFormat(Image.ImageLayout layout)
        {
            switch (layout)
            {
                case Image.ImageLayout.LAYOUT_RGB:
                case Image.ImageLayout.LAYOUT_GRB:
                case Image.ImageLayout.LAYOUT_BGR:
                    return TextureFormat.RGB24;
                case Image.ImageLayout.LAYOUT_GREY:
                    return TextureFormat.Alpha8;
                case Image.ImageLayout.LAYOUT_RGBA:
                case Image.ImageLayout.LAYOUT_RGBX:
                case Image.ImageLayout.LAYOUT_UNDEFINED:
                default:
                    return TextureFormat.RGBA32;
            }
        }

        public static void ToUnity(this Image image, ref Texture2D texture)
        {
            var w = (int)image.getWidth();
            var h = (int)image.getHeight();
            var format = GetFormat(image.getImageLayout());
            Assert.AreEqual(3, image.getNbChannels());
            Assert.AreEqual(8, image.getNbBitsPerComponent());
            Assert.AreEqual(Image.DataType.TYPE_8U, image.getDataType());
            //Assert.AreEqual(Image.ImageLayout.LAYOUT_BGR, image.getImageLayout());
            Assert.AreEqual(Image.PixelOrder.INTERLEAVED, image.getPixelOrder());
            if (texture != null && (texture.width != w || texture.height != h || texture.format != format))
            {
                UnityEngine.Object.Destroy(texture);
                texture = null;
            }
            if (texture == null)
            {
                texture = new Texture2D(w, h, format, false);
            }
            texture.LoadRawTextureData(image.data(), (int)image.getBufferSize());
            texture.Apply();
        }

        static Image.ImageLayout GetLayout(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.Alpha8:
                case TextureFormat.R16:
                case TextureFormat.R8:
                case TextureFormat.RFloat:
                case TextureFormat.RHalf:
                    return Image.ImageLayout.LAYOUT_GREY;
                case TextureFormat.ARGB32:
                case TextureFormat.BGRA32:
                case TextureFormat.RGBA32:
                case TextureFormat.RGBAFloat:
                case TextureFormat.RGBAHalf:
                    return Image.ImageLayout.LAYOUT_RGBA;
                case TextureFormat.RG16:
                case TextureFormat.RGFloat:
                case TextureFormat.RGHalf:
                    return Image.ImageLayout.LAYOUT_UNDEFINED;
                case TextureFormat.RGB24:
                    return Image.ImageLayout.LAYOUT_RGB;
                default:
                    Debug.LogWarning(new { format });
                    return Image.ImageLayout.LAYOUT_UNDEFINED;
            }
        }

        static Image.DataType GetDataType(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.R16:
                case TextureFormat.RG16:
                    return Image.DataType.TYPE_16U;
                case TextureFormat.RFloat:
                case TextureFormat.RGBAFloat:
                case TextureFormat.RGFloat:
                    return Image.DataType.TYPE_32U;
                case TextureFormat.Alpha8:
                case TextureFormat.ARGB32:
                case TextureFormat.BGRA32:
                case TextureFormat.R8:
                case TextureFormat.RGB24:
                case TextureFormat.RGBA32:
                    return Image.DataType.TYPE_8U;
                case TextureFormat.RGBAHalf:
                case TextureFormat.RGHalf:
                case TextureFormat.RHalf:
                    return Image.DataType.TYPE_16U;
                default:
                    Debug.LogWarning(new { format });
                    return Image.DataType.TYPE_8U;
            }
        }

        public static Image ToSolAR(this Texture2D texture)
        {
            var data = texture.GetRawTextureData();
            var handler = GCHandle.Alloc(data, GCHandleType.Pinned);
            var ptr = handler.AddrOfPinnedObject();

            uint w = (uint)texture.width;
            uint h = (uint)texture.height;
            var format = texture.format;
            var pixLayout = GetLayout(format);
            var type = GetDataType(format);

            var image = new Image(ptr, w, h, pixLayout, Image.PixelOrder.INTERLEAVED, type);
            handler.Free();
            return image;
        }
    }
}
