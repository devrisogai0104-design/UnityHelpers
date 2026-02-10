using UnityEngine;

namespace IRCore.UnityHelpers
{
    public static class ColorExtensions
    {
        /// <summary>
        /// 指定された 16 進数を色に変換する
        /// </summary>
        /// <example>
        /// <code>
        /// // RGBA(1.000, 0.502, 0.000, 1.000)
        /// ColorUtils.ToARGB( 0xFFFF8000 )
        /// </code>
        /// </example>
        public static Color ToARGB(uint val)
        {
            var inv = 1f / 255f;
            var c = Color.black;
            c.a = inv * ((val >> 24) & 0xFF);
            c.r = inv * ((val >> 16) & 0xFF);
            c.g = inv * ((val >> 8) & 0xFF);
            c.b = inv * (val & 0xFF);
            return c;
        }

        /// <summary>
        /// 指定された 16 進数を色に変換する
        /// </summary>
        /// <example>
        /// <code>
        /// // RGBA(1.000, 0.502, 0.000, 1.000)
        /// ColorUtils.ToARGB( 0xFF8000FF ) 
        /// </code>
        /// </example>
        public static Color ToRGBA(uint val)
        {
            var inv = 1f / 255f;
            var c = Color.black;
            c.r = inv * ((val >> 24) & 0xFF);
            c.g = inv * ((val >> 16) & 0xFF);
            c.b = inv * ((val >> 8) & 0xFF);
            c.a = inv * (val & 0xFF);
            return c;
        }

        /// <summary>
        /// 指定された 16 進数を色に変換する
        /// </summary>
        /// <example>
        /// <code>
        /// // RGBA(1.000, 0.502, 0.000, 1.000)
        /// ColorUtils.ToRGB( 0xFF8000 ) 
        /// </code>
        /// </example>
        public static Color ToRGB(uint val)
        {
            var inv = 1f / 255f;
            var c = Color.black;
            c.r = inv * ((val >> 16) & 0xFF);
            c.g = inv * ((val >> 8) & 0xFF);
            c.b = inv * (val & 0xFF);
            c.a = 1f;
            return c;
        }

        /// <summary>
        /// 色の透明度（Alpha）のみを変更した新しい Color を返す
        /// </summary>
        /// <param name="color">元の色</param>
        /// <param name="alpha">設定するアルファ値 (0.0 ~ 1.0)</param>
        /// <returns>アルファ値を変更した新しい Color</returns>
        public static Color ToColorAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha));
        }

        /// <summary>
        /// 元の色を維持したまま、アルファ値だけを0にしたColorを返す
        /// </summary>
        public static Color ToColorAlphaZero(this Color color)
        {
            return new Color(color.r, color.g, color.b, 0f);
        }
    }

}