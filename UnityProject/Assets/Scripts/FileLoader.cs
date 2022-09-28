using UnityEngine;

namespace Assets.Scripts
{
    public class FileLoader
    {
        public static string LoadFile(string fileName)
        {
            var resultAsTextAsset = Resources.Load<TextAsset>(fileName);
            if(resultAsTextAsset == null)
            {
                return "";
            }
            return resultAsTextAsset.ToString();
        }
    }
}
