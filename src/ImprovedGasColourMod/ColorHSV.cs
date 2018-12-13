/*
 * Created by C.J. Kimberlin (http://cjkimberlin.com)
 *
 * The MIT License (MIT)
 *
 * Copyright (c) 2015
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 *
 *
 * ============= Description =============
 *
 * An ColorHSV struct for interpreting a color in hue/saturation/value instead of red/green/blue.
 * NOTE! hue will be a value from 0 to 1 instead of 0 to 360.
 *
 * ColorHSV hsvRed = new ColorHSV(1, 1, 1, 1); // RED
 * ColorHSV hsvGreen = new ColorHSV(0.333f, 1, 1, 1); // GREEN
 *
 *
 * Also supports implicit conversion between Color and Color32.
 *
 * ColorHSV hsvBlue = Color.blue; // HSVA(0.667f, 1, 1, 1)
 * Color blue = hsvBlue; // RGBA(0, 0, 1, 1)
 * Color32 blue32 = hsvBlue; // RGBA(0, 0, 255, 255)
 *
 *
 * If functions are desired instead of implicit conversion then use the following.
 *
 * Color yellowBefore = Color.yellow; // RBGA(1, .922f, 0.016f, 1)
 * ColorHSV hsvYellow = Color.yellowBefore.ToHSV(); // HSVA(0.153f, 0.984f, 1, 1)
 * Color yellowAfter = hsvYellow.ToRGB(); // RBGA(1, .922f, 0.016f, 1)
 * */

// ReSharper disable All
namespace ImprovedGasColourMod
{
    using UnityEngine;

    public struct ColorHSV
    {
        public float H;

        public float S;

        public float V;

        public float A;

        public ColorHSV(float h, float s, float v, float a)
        {
            this.H = h;
            this.S = s;
            this.V = v;
            this.A = a;
        }

        public override string ToString()
        {
            return string.Format("HSVA: ({0:F3}, {1:F3}, {2:F3}, {3:F3})", this.H, this.S, this.V, this.A);
        }

        public static bool operator ==(ColorHSV lhs, ColorHSV rhs)
        {
            if (lhs.A != rhs.A)
            {
                return false;
            }

            if (lhs.V == 0 && rhs.V == 0)
            {
                return true;
            }

            if (lhs.S == 0 && rhs.S == 0)
            {
                return lhs.V == rhs.V;
            }

            return lhs.H == rhs.H && lhs.S == rhs.S && lhs.V == rhs.V;
        }

        public static implicit operator ColorHSV(Color c)
        {
            return c.ToHSV();
        }

        public static implicit operator Color(ColorHSV hsv)
        {
            return hsv.ToRgb();
        }

        public static implicit operator ColorHSV(Color32 c32)
        {
            return ((Color)c32).ToHSV();
        }

        public static implicit operator Color32(ColorHSV hsv)
        {
            return hsv.ToRgb();
        }

        public static bool operator !=(ColorHSV lhs, ColorHSV rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            if (other is ColorHSV || other is Color || other is Color32)
            {
                return this == (ColorHSV)other;
            }

            return false;
        }

        public override int GetHashCode()
        {
            // This is maybe not a good implementation :)
            return ((Color)this).GetHashCode();
        }

        public Color ToRgb()
        {
            Vector3 rgb = HuEtoRgb(this.H);
            Vector3 vc  = ((rgb - Vector3.one) * this.S + Vector3.one) * this.V;

            return new Color(vc.x, vc.y, vc.z, this.A);
        }

        private static Vector3 HuEtoRgb(float h)
        {
            float r = Mathf.Abs(h * 6 - 3) - 1;
            float g = 2                    - Mathf.Abs(h * 6 - 2);
            float b = 2                    - Mathf.Abs(h * 6 - 4);

            return new Vector3(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b));
        }
    }

    public static class ColorExtension
    {
        private const float Epsilon = 1e-10f;

        public static ColorHSV ToHSV(this Color rgb)
        {
            Vector3 hcv = RgBtoHcv(rgb);
            float   s   = hcv.y / (hcv.z + Epsilon);

            return new ColorHSV(hcv.x, s, hcv.z, rgb.a);
        }

        private static Vector3 RgBtoHcv(Color rgb)
        {
            Vector4 p = rgb.g < rgb.b
                        ? new Vector4(rgb.b, rgb.g, -1, 2f  / 3f)
                        : new Vector4(rgb.g, rgb.b, 0,  -1f / 3f);
            Vector4 q = rgb.r < p.x ? new Vector4(p.x, p.y, p.w, rgb.r) : new Vector4(rgb.r, p.y, p.z, p.x);
            float   c = q.x - Mathf.Min(q.w, q.y);
            float   h = Mathf.Abs((q.w - q.y) / (6 * c + Epsilon) + q.z);

            return new Vector3(h, c, q.x);
        }

        public static void CheckAndLogOverflow(this ColorHSV color, SimHashes elementID, float fraction)
        {
            bool error = false;
            string message = string.Empty;

            if (color.H < 0 || color.H > 1)
            {
                error = true;
                message += $"hue ({color.H}) ";
            }

            if (color.S < 0 || color.S > 1)
            {
                error = true;
                message += $"saturation ({color.S}) ";
            }

            if (color.V < 0 || color.V > 1)
            {
                error = true;
                message += $"value ({color.V}) ";
            }

            if (error)
            {
                Debug.LogError($"Gas color {message.Trim()} under/overflow for <{elementID}> at intensity [{fraction}]");
            }
        }

        public static ColorHSV Clamp(this ColorHSV color)
        {
            color.H %= 1;
            color.S = Mathf.Clamp01(color.S);
            color.V = Mathf.Clamp01(color.V);

            return color;
        }
    }
}