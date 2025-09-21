using UnityEngine;
using UnityEditor;

public class PackageExporter
{
    [MenuItem("Donkey Kong/Export Package")]
    public static void ExportDonkeyKongPackage()
    {
        string[] assetsToExport = {
            "Assets/Scripts",
            "Assets/Scenes",
            "Assets/Prefabs",
            "Assets/Materials"
        };
        
        AssetDatabase.ExportPackage(
            assetsToExport, 
            "DonkeyKongMobile.unitypackage", 
            ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies
        );
        
        Debug.Log("Package exported: DonkeyKongMobile.unitypackage");
    }
}
