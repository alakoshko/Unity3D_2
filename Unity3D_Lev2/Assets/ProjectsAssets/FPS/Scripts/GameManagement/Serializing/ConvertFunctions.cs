using System;
using UnityEngine;


namespace FPS
{
    public static class ConvertFunctions
    {

        public static Vector3 GetVector3(string ts)
        {
            string[] str = ts.Substring(1, ts.Length - 2).Split(' ');
            float x = 0f;
            if (!float.TryParse(str[0].Substring(0, str[0].Length - 1), out x))
            {
                Exception exception = new Exception($"Cannot parse 'x' coordinate to float");
            }
            float y = 0f;
            if (!float.TryParse(str[1].Substring(0, str[1].Length - 1), out y))
            {
                Exception exception = new Exception($"Cannot parse 'y' coordinate to float");
            }
            float z = 0f;
            if (!float.TryParse(str[2].Substring(0, str[2].Length), out z))
            {
                Exception exception = new Exception($"Cannot parse 'z' coordinate to float");
            }
            return new Vector3(x, y, z);
        }
    }
}