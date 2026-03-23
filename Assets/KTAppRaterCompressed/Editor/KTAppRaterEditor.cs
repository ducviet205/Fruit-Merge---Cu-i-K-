using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AppraterScript)), CanEditMultipleObjects]
public class KTAppRaterEditor : Editor {	
//	private AppRaterModel.Options selectedOption = Options.ThreeButtons;
//	private AppRaterModel.Show shouldShow = AppRaterModel.Show.No;
//	private AppRaterModel.Number numberOfDays = AppRaterModel.Number.Three;
//	private AppRaterModel.Number numberOfGamePlays = AppRaterModel.Number.Three;
	
	void Awake () {
		
	}
	
	public override void OnInspectorGUI () {
		AppraterScript appRater = target as  AppraterScript;	
		appRater.model_.Appid = EditorGUILayout.TextField("AppId:",appRater.model_.Appid);
		EditorGUILayout.Space();
		appRater.model_.ReviewTitle = EditorGUILayout.TextField("Review Title:",
			appRater.model_.ReviewTitle);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Review Messsage:");
		appRater.model_.ReviewMessage = EditorGUILayout.TextArea(appRater.model_.ReviewMessage, 
			GUILayout.MaxHeight(50));
		EditorGUILayout.Space();
		appRater.model_.ShouldAutoShow = (AppRaterModel.Show)EditorGUILayout.EnumPopup("Show Automatically:",
			appRater.model_.ShouldAutoShow);
		EditorGUILayout.Space();
		if (appRater.model_.ShouldAutoShow == AppRaterModel.Show.Yes) {
			appRater.model_.ShouldAlwaysShow = (AppRaterModel.Show)EditorGUILayout.EnumPopup("Show Popup Each Time:",
			appRater.model_.ShouldAlwaysShow);
			EditorGUILayout.Space();
			appRater.model_.NumberOfDays = (AppRaterModel.Number)EditorGUILayout.EnumPopup("Days Till Popup:",
				appRater.model_.NumberOfDays);
			EditorGUILayout.Space();
			appRater.model_.NumberOfGamePlays = (AppRaterModel.Number)EditorGUILayout.EnumPopup("GamePlays Till Popup:", 
				appRater.model_.NumberOfGamePlays);
			EditorGUILayout.Space();
		}
		
		appRater.model_.NumberOfButtons = (AppRaterModel.Options)EditorGUILayout.EnumPopup("Total Buttons:",
			appRater.model_.NumberOfButtons);
		EditorGUILayout.Space();
		appRater.model_.RateNowTitle = EditorGUILayout.TextField("Rate Now Button Title:",
			appRater.model_.RateNowTitle);
		EditorGUILayout.Space();
		appRater.model_.RateLaterTitle = EditorGUILayout.TextField("Rate Later Button Title:",
			appRater.model_.RateLaterTitle);
		EditorGUILayout.Space();
		if (appRater.model_.NumberOfButtons == AppRaterModel.Options.ThreeButtons) {
			appRater.model_.NeverRemindTitle = EditorGUILayout.TextField("Never Remind Title:",
				appRater.model_.NeverRemindTitle);
			appRater.model_.IsThirdButton = true;
		}
		else {
			appRater.model_.IsThirdButton = false;
		}
			EditorUtility.SetDirty(appRater); 
	}
}