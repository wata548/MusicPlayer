using System.Drawing;

namespace Extensions {
    public static class TextExtension {

        public static string SetFontSizeBySize(this string context, float size) =>
            $"<size={size}>{context}</size>";

        public static string SetFontSizeByPercent(this string context, int percent) =>
            $"<size={percent}%>{context}</size>";

        public static string SetFontSizeByPercent(this string context, float percent) =>
            $"<size={percent*100}%>{context}</size>";

        public static string SetColor(this string context, Color color) =>
            $"<color=#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}>{context}</color>";
        
        public static string SetColor(this string context, UnityEngine.Color color) =>
            $"<color=#{color.r:X2}{color.g:X2}{color.b:X2}{color.a:X2}>{context}</color>";
    }
}