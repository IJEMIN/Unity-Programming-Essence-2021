using ExitGames.Client.Photon;
using UnityEngine;

public class ColorSerialization {
    private static byte[] colorMemory = new byte[4 * 4];

    public static short SerializeColor(StreamBuffer outStream, object targetObject) {
        Color color = (Color) targetObject;

        lock (colorMemory)
        {
            byte[] bytes = colorMemory;
            int index = 0;
            
            Protocol.Serialize(color.r, bytes, ref index);
            Protocol.Serialize(color.g, bytes, ref index);
            Protocol.Serialize(color.b, bytes, ref index);
            Protocol.Serialize(color.a, bytes, ref index);
            outStream.Write(bytes, 0, 4*4);
        }

        return 4 * 4;
    }

    public static object DeserializeColor(StreamBuffer inStream, short length)  {
        Color color = new Color();
  
        lock (colorMemory)
        {
            inStream.Read(colorMemory, 0, 4 * 4);
            int index = 0;
            
            Protocol.Deserialize(out color.r,colorMemory, ref index);
            Protocol.Deserialize(out color.g,colorMemory, ref index);
            Protocol.Deserialize(out color.b,colorMemory, ref index);
            Protocol.Deserialize(out color.a,colorMemory, ref index);
        }
        
        return color;
    }
}