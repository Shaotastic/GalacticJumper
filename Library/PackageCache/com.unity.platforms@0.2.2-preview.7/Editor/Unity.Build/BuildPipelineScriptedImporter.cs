

namespace Unity.Build
{
#if UNITY_2020_1_OR_NEWER
    [UnityEditor.AssetImporters.ScriptedImporter(3, new[] { BuildPipeline.AssetExtension })]
#else
    [ScriptedImporter(2, new[] { BuildPipeline.AssetExtension })]
#endif
    sealed class BuildPipelineScriptedImporter : UnityEditor.AssetImporters.ScriptedImporter
    {
        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext context)
        {
            var asset = BuildPipeline.CreateInstance();
            if (BuildPipeline.DeserializeFromPath(asset, context.assetPath))
            {
                context.AddObjectToAsset("asset", asset/*, icon*/);
                context.SetMainObject(asset);
            }
        }
    }
}
