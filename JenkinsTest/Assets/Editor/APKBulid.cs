using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace GameEditor{
    public class APKBuild : Editor
    {
        private static BuildOptions s_BuildOptions = BuildOptions.CompressWithLz4HC;

        //手动打包设置环境变量
        private static void ManualExportEnvironment()
        {
            string rootPath = Application.dataPath;
            rootPath = Path.GetDirectoryName(rootPath);
            rootPath = Path.GetDirectoryName(rootPath);

            string exportPath = Path.Combine(rootPath, "Export/Android");
            string exportApkPath = Path.Combine(exportPath, "test.apk");
            string exportAABPath = Path.Combine(exportPath, "googleplay.aab");

            Environment.SetEnvironmentVariable("EXPORT_PATH", exportPath);
            Environment.SetEnvironmentVariable("EXPORT_APK_PATH", exportApkPath);
            Environment.SetEnvironmentVariable("GOOGLE_PLAY_AAB_PATH", exportAABPath);
        }

        private static void ManualExportAPKSpecial()
        {
            Environment.SetEnvironmentVariable("BUILD_AAB", "false");
        }

        private static void ManualExportAABSpecial()
        {
            Environment.SetEnvironmentVariable("BUILD_AAB", "true");
        }

        [MenuItem("Tools/Export/ExportAPK")]
        public static void ExportAPKManual()
        {
            ManualExportEnvironment();
            ManualExportAPKSpecial();
            ExportApk();
        }

        
        public static void ExportApk()
        {
            Debug.Log("ExportAPK start");
            bool switchAndroid = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            if (!switchAndroid)
            {
                Debug.LogError("ExportApk Switch Android Error");
                return;
            }
            Debug.Log("ExportApk Switch Android success");
            string export = WorkExportPath();
            if(Directory.Exists(export))
            {
                Directory.Delete(export, true );
            }
            Directory.CreateDirectory(export);

            PlayerSettings.applicationIdentifier = "com.DeCompany.Project";

            PlayerSettings.Android.bundleVersionCode = 2;
            PlayerSettings.Android.useAPKExpansionFiles = false;

            bool isAab = true;
            bool.TryParse(Environment.GetEnvironmentVariable("BUILD_AAB"), out isAab);
            EditorUserBuildSettings.buildAppBundle = isAab;
            // 生成符号文件
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Public;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;

            var options = s_BuildOptions;
            bool connectProfiler = false;
            if(connectProfiler)
            {
                options |= BuildOptions.Development;
                EditorUserBuildSettings.development = true;
                EditorUserBuildSettings.connectProfiler = true;
                EditorUserBuildSettings.buildWithDeepProfilingSupport = true;
            }

            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            PlayerSettings.bundleVersion = "1.1.1";
            PlayerSettings.productName = "TestProduct";
            var targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            PlayerSettings.Android.targetArchitectures = (AndroidArchitecture)targetArchitectures;

            // PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defs);

            List<string> levels = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                // 获取有效的 Scene
                levels.Add(scene.path);
            }

            string apkPath = GetApkPath();
            Debug.Log("apkPath:" + apkPath);
            BuildPipeline.BuildPlayer(levels.ToArray(), apkPath, BuildTarget.Android, options);
        }

        public static string WorkExportPath()
        {
            string exportPath = Environment.GetEnvironmentVariable("EXPORT_PATH");
            if(string.IsNullOrEmpty(exportPath))
            {
                exportPath = Application.dataPath;
            }
            return exportPath;
        }

        private static string GetApkPath()
        {
            string apkPath = Environment.GetEnvironmentVariable("EXPORT_APK_PATH");
            if (string.IsNullOrEmpty(apkPath))
            {
                apkPath = "output.apk";
            }
            return apkPath;
        }

        private static string GetAABPath()
        {
            string aaBPath = Environment.GetEnvironmentVariable("GOOGLE_PLAY_AAB_PATH");
            if (string.IsNullOrEmpty(aaBPath))
            {
                aaBPath = "googleplay.aab";
            }
            return aaBPath;
        }
    }
}
