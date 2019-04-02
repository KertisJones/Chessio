// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.BuildSystem;
using Improbable.Gdk.BuildSystem.Configuration;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable
{
    internal static class BuildWorkerMenu
    {
        private const string LocalMenu = "Build for local";
        private const string CloudMenu = "Build for cloud";

        private static readonly string[] AllWorkers =
        {
            "AndroidClient",
            "iOSClient",
            "UnityClient",
            "UnityGameLogic",
        };

        [MenuItem(EditorConfig.ParentMenu + "/" + LocalMenu + "/AndroidClient", false, EditorConfig.MenuOffset + 0)]
        public static void BuildLocalAndroidClient()
        {
            MenuBuild(BuildEnvironment.Local, "AndroidClient");
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + CloudMenu + "/AndroidClient", false, EditorConfig.MenuOffset + 0)]
        public static void BuildCloudAndroidClient()
        {
            MenuBuild(BuildEnvironment.Cloud, "AndroidClient");
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + LocalMenu + "/iOSClient", false, EditorConfig.MenuOffset + 1)]
        public static void BuildLocaliOSClient()
        {
            MenuBuild(BuildEnvironment.Local, "iOSClient");
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + CloudMenu + "/iOSClient", false, EditorConfig.MenuOffset + 1)]
        public static void BuildCloudiOSClient()
        {
            MenuBuild(BuildEnvironment.Cloud, "iOSClient");
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + LocalMenu + "/UnityClient", false, EditorConfig.MenuOffset + 2)]
        public static void BuildLocalUnityClient()
        {
            MenuBuild(BuildEnvironment.Local, "UnityClient");
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + CloudMenu + "/UnityClient", false, EditorConfig.MenuOffset + 2)]
        public static void BuildCloudUnityClient()
        {
            MenuBuild(BuildEnvironment.Cloud, "UnityClient");
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + LocalMenu + "/UnityGameLogic", false, EditorConfig.MenuOffset + 3)]
        public static void BuildLocalUnityGameLogic()
        {
            MenuBuild(BuildEnvironment.Local, "UnityGameLogic");
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + CloudMenu + "/UnityGameLogic", false, EditorConfig.MenuOffset + 3)]
        public static void BuildCloudUnityGameLogic()
        {
            MenuBuild(BuildEnvironment.Cloud, "UnityGameLogic");
        }


        [MenuItem(EditorConfig.ParentMenu + "/" + LocalMenu + "/All workers", false, EditorConfig.MenuOffset + 4)]
        public static void BuildLocalAll()
        {
            MenuBuild(BuildEnvironment.Local, AllWorkers);
        }

        [MenuItem(EditorConfig.ParentMenu + "/" + CloudMenu + "/All workers", false, EditorConfig.MenuOffset + 4)]
        public static void BuildCloudAll()
        {
            MenuBuild(BuildEnvironment.Cloud, AllWorkers);
        }

        [MenuItem(EditorConfig.ParentMenu + "/Clean all workers", false, EditorConfig.MenuOffset + 4)]
        public static void Clean()
        {
            WorkerBuilder.Clean();
            Debug.Log("Clean completed");
        }

        private static bool CheckWorkersCanBuildForEnvironment(BuildEnvironment environment, string[] workerTypes)
        {
            var canInitiateBuild = true;
            var spatialOSBuildConfiguration = SpatialOSBuildConfiguration.GetInstance();

            foreach (var workerType in workerTypes)
            {
                var buildTargetsForWorker = WorkerBuilder.GetBuildTargetsForWorkerForEnvironment(workerType, environment);
                var buildTargetsMissingBuildSupport = BuildSupportChecker.GetBuildTargetsMissingBuildSupport(buildTargetsForWorker);

                if (buildTargetsMissingBuildSupport.Length > 0)
                {
                    canInitiateBuild = false;

                    Debug.LogError(BuildSupportChecker.ConstructMissingSupportMessage(workerType, environment, buildTargetsMissingBuildSupport),
                        spatialOSBuildConfiguration);
                }
            }

            return canInitiateBuild;
        }

        private static void MenuBuild(BuildEnvironment environment, params string[] workerTypes)
        {
            // Delaying build by a frame to ensure the editor has re-rendered the UI to avoid odd glitches.
            EditorApplication.delayCall += () =>
            {
                var filteredWorkerTypes = BuildSupportChecker.FilterWorkerTypes(environment, workerTypes);

                try
                {
                    LocalLaunch.BuildConfig();

                    foreach (var workerType in filteredWorkerTypes)
                    {
                        WorkerBuilder.BuildWorkerForEnvironment(workerType, environment);
                    }

                    if (workerTypes.Length == filteredWorkerTypes.Length)
                    {
                        Debug.Log($"Completed build for {environment} target.");
                    }
                    else
                    {
                        var missingWorkerTypes = string.Join(" ", workerTypes.Except(filteredWorkerTypes));
                        var completedWorkerTypes = string.Join(" ", workerTypes.Intersect(filteredWorkerTypes));
                        Debug.LogWarning(
                            $"Completed build for {environment} target.\n"
                            + $"Completed builds for: {completedWorkerTypes}\n"
                            + $"Skipped builds for: {missingWorkerTypes}. See above for more information.");
                    }
                }
                catch (System.Exception)
                {
                    DisplayBuildFailureDialog();

                    throw;
                }
            };
        }

        private static void DisplayBuildFailureDialog()
        {
            EditorUtility.DisplayDialog("Build Failed",
                "Build failed. Please see the Unity Console Window for information.",
                "OK");
        }

        [MenuItem(EditorConfig.ParentMenu + "/Check build support", false, EditorConfig.MenuOffset + 5)]
        private static void CheckBuildSupport()
        {
            bool checksPassed = CheckWorkersCanBuildForEnvironment(BuildEnvironment.Local, AllWorkers);

            if (!CheckWorkersCanBuildForEnvironment(BuildEnvironment.Cloud, AllWorkers))
            {
                checksPassed = false;
            }

            EditorUtility.DisplayDialog("Build Support Check",
                checksPassed
                    ? "Build support check passed. Your Unity Editor installation has all necessary components."
                    : "Build support check failed. Please see the Unity Console Window for information.",
                "OK");

            Debug.Log("Build support check completed.");
        }
    }
}
