#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreToolsEditor
{
    [MenuItem("Tools/Store/Generate UI For Current Scene")]
    private static void GenerateStoreUiForCurrentScene()
    {
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Store", "Hay stop Play Mode truoc khi generate store UI.", "OK");
            return;
        }

        Scene activeScene = SceneManager.GetActiveScene();
        if (!activeScene.IsValid() || !activeScene.isLoaded)
        {
            EditorUtility.DisplayDialog("Store", "Khong co scene nao dang mo.", "OK");
            return;
        }

        if (activeScene.name != "Menu")
        {
            EditorUtility.DisplayDialog("Store", "Store UI chi nen generate trong scene Menu.", "OK");
            return;
        }

        MenuManager menuManager = Object.FindObjectOfType<MenuManager>();
        if (menuManager == null)
        {
            EditorUtility.DisplayDialog("Store", "Khong tim thay MenuManager trong scene Menu.", "OK");
            return;
        }

        MenuStoreUI storeUi = menuManager.GetComponent<MenuStoreUI>();
        if (storeUi == null)
        {
            storeUi = Undo.AddComponent<MenuStoreUI>(menuManager.gameObject);
        }

        Undo.RegisterFullObjectHierarchyUndo(menuManager.gameObject, "Generate Store UI");
        storeUi.BuildGeneratedUi();

        EditorSceneManager.MarkSceneDirty(activeScene);
        EditorUtility.SetDirty(menuManager.gameObject);
        EditorUtility.DisplayDialog("Store", "Da generate Store UI cho scene Menu.", "OK");
    }

    [MenuItem("Tools/Store/Delete UI From Current Scene")]
    private static void DeleteStoreUiFromCurrentScene()
    {
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Store", "Hay stop Play Mode truoc khi xoa store UI.", "OK");
            return;
        }

        MenuStoreUI storeUi = Object.FindObjectOfType<MenuStoreUI>();
        if (storeUi == null)
        {
            EditorUtility.DisplayDialog("Store", "Khong tim thay MenuStoreUI trong scene nay.", "OK");
            return;
        }

        storeUi.DeleteGeneratedUi();
        Undo.DestroyObjectImmediate(storeUi);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    [MenuItem("Tools/Store/Reset Store Data")]
    private static void ResetStoreData()
    {
        StoreData.ResetAll();
        Debug.Log("Store data da duoc reset.");
    }

    [MenuItem("Tools/Store/Give 250 Test Coins")]
    private static void GiveTestCoins()
    {
        StoreData.AddCoins(250);
        Debug.Log("Da cong them 250 xu de test store.");
    }
}
#endif
