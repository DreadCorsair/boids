using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class TypeModel : ScriptableObject
    {
        [System.Serializable]
        public class TypeModelItem
        {
            public int Type;
            public Color Color;
        }

        public TypeModelItem[] Model;
        public int Length { get { return Model.Length; } }

        public TypeModelItem GetItem(int index)
        {
            return Model[index];
        }

        [MenuItem("Assets/Create/TypeModel")]
        public static void CreateMyAsset()
        {
            TypeModel asset = CreateInstance<TypeModel>();

            AssetDatabase.CreateAsset(asset, "Assets/TypeModel.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
