using UnityEditor;

namespace Stack
{
    public class GlobalAsset
    {
        [MenuItem("Assets/Create/Game/Config")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<GlobalInfo>();
        }
    }
}