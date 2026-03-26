using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AchievementUiGeneratorEditor
{
    [MenuItem("Tools/Achievements/Generate UI For Current Scene")]
    public static void GenerateUiForCurrentScene()
    {
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Achievements", "Hay Stop Play Mode truoc khi generate UI.", "OK");
            return;
        }

        Scene activeScene = SceneManager.GetActiveScene();
        if (!activeScene.IsValid() || !activeScene.isLoaded)
        {
            EditorUtility.DisplayDialog("Achievements", "Khong co scene nao dang mo.", "OK");
            return;
        }

        bool isMenuScene = activeScene.name == "Menu";
        if (isMenuScene)
        {
            EditorUtility.DisplayDialog("Achievements", "Scene Menu khong can Achievement UI. Hay mo scene Game roi generate.", "OK");
            return;
        }

        AchievementSceneUI sceneUi = Object.FindObjectOfType<AchievementSceneUI>();

        if (sceneUi == null)
        {
            GameObject uiObject = new GameObject("AchievementSceneUI");
            sceneUi = uiObject.AddComponent<AchievementSceneUI>();
        }

        Undo.RegisterFullObjectHierarchyUndo(sceneUi.gameObject, "Generate Achievement UI");
        sceneUi.BuildGeneratedUi(isMenuScene);

        EditorSceneManager.MarkSceneDirty(activeScene);
        EditorUtility.SetDirty(sceneUi.gameObject);

        string sceneType = isMenuScene ? "Menu" : "Gameplay";
        EditorUtility.DisplayDialog("Achievements", "Da generate Achievement UI cho scene " + sceneType + ".", "OK");
    }

    [MenuItem("Tools/Achievements/Delete UI From Current Scene")]
    public static void DeleteUiFromCurrentScene()
    {
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Achievements", "Hay Stop Play Mode truoc khi xoa UI.", "OK");
            return;
        }

        AchievementSceneUI sceneUi = Object.FindObjectOfType<AchievementSceneUI>();
        if (sceneUi == null)
        {
            EditorUtility.DisplayDialog("Achievements", "Khong tim thay AchievementSceneUI trong scene nay.", "OK");
            return;
        }

        Undo.DestroyObjectImmediate(sceneUi.gameObject);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
}
