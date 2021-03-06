using System.Collections.Generic;
using System.IO;
using Unity.Serialization.Json;
using UnityEditor;

using UnityEngine;

namespace Unity.Build
{
    [UnityEditor.AssetImporters.ScriptedImporter(Version, new[] { BuildConfiguration.AssetExtension
#pragma warning disable 618
        , BuildSettings.AssetExtension
#pragma warning restore 618
    })]
    sealed class BuildConfigurationScriptedImporter : UnityEditor.AssetImporters.ScriptedImporter
    {
#if UNITY_2020_1_OR_NEWER
        const int Version = 3;
#else
        const int Version = 2;
#endif

        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext context)
        {
            var asset = BuildConfiguration.CreateInstance();
            if (BuildConfiguration.DeserializeFromPath(asset, context.assetPath))
            {
                context.AddObjectToAsset("asset", asset/*, icon*/);
                context.SetMainObject(asset);

#pragma warning disable 618
                if (Path.GetExtension(context.assetPath) == BuildSettings.AssetExtension)
                {
                    Debug.LogWarning($"{context.assetPath.ToHyperLink()}: {BuildSettings.AssetExtension.SingleQuotes()} asset extension is obsolete: it has been renamed to {BuildConfiguration.AssetExtension.SingleQuotes()}. (RemovedAfter 2020-05-01)", asset);
                }
#pragma warning restore 618
            }
        }

        static string[] GatherDependenciesFromSourceFile(string assetPath)
        {
            var dependencies = new List<string>();
            try
            {
                using (var reader = new SerializedObjectReader(assetPath))
                {
                    var root = reader.ReadObject();
                    if (!root.TryGetMember(nameof(BuildConfiguration.Dependencies), out var member))
                    {
                        return null;
                    }

                    var valueView = member.Value();
                    if (valueView.Type != TokenType.Array)
                    {
                        return null;
                    }

                    var arrayView = valueView.AsArrayView();
                    foreach (var value in arrayView)
                    {
                        if (!GlobalObjectId.TryParse(value.AsStringView().ToString(), out var id))
                        {
                            continue;
                        }

                        var dependencyPath = AssetDatabase.GUIDToAssetPath(id.assetGUID.ToString());
                        if (string.IsNullOrEmpty(dependencyPath))
                        {
                            continue;
                        }

                        dependencies.Add(dependencyPath);
                    }
                }
            }
            catch
            {
                return null;
            }
            return dependencies.ToArray();
        }
    }
}
