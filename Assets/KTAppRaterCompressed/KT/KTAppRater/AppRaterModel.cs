using UnityEngine;
using System.Collections;

[System.Serializable]
public class AppRaterModel {
	
	public enum Options { 
		TwoButtons = 0, 
		ThreeButtons = 1,
	}
	
	public enum Show { 
		Yes = 0, 
		No = 1,
	}
	
	public enum Number {
		One = 0,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten
	}
	
	[SerializeField]
	private string appid = "00000000";
	[SerializeField]
	private string reviewTitle = "Rate ProductName";
	[SerializeField]
	private string reviewMessage = "Will you mind giving a minute to rate this app?";
	[SerializeField]
	private string rateNowTitle = "Rate Now";
	[SerializeField]
	private string rateLaterTitle = "Rate Later";
	[SerializeField]
	private string neverRemindTitle = "Never Remind Me";
	[SerializeField]
	private Show shouldAlwaysShow;
	[SerializeField]
	private bool isThirdButton;
	[SerializeField]
	private Show shouldAutoShow;
	[SerializeField]
	private Number numberOfDays;
	[SerializeField]
	private Number numberOfGamePlays;
	[SerializeField]
	private Options numberOfButtons;
	
	public bool IsThirdButton {
		set {
			isThirdButton = value;
		}
		get {
			return isThirdButton;
		}
	}
	public Show ShouldAlwaysShow {
		set {
			shouldAlwaysShow = value;
		}
		get {
			return shouldAlwaysShow;
		}
	}
	public Show ShouldAutoShow {
		set {
			shouldAutoShow = value;
		}
		get {
			return shouldAutoShow;
		}
	}
	public Number NumberOfDays {
		set {
			numberOfDays = value;
		}
		get {
			return numberOfDays;
		}
	}
	public Number NumberOfGamePlays {
		set {
			numberOfGamePlays = value;
		}
		get {
			return numberOfGamePlays;
		}
	}
	public Options NumberOfButtons {
		set {
			numberOfButtons = value;
		}
		get {
			return numberOfButtons;
		}
	}
	public string Appid {
		set {
			appid = value;
		}
		get {
			return appid;
		}
	}
	public string ReviewTitle {
		set {
			reviewTitle = value;
		}
		get {
			return reviewTitle;
		}
	}
	public string ReviewMessage {
		set {
			reviewMessage = value;
		}
		get {
			return reviewMessage;
		}
	}
	public string RateNowTitle {
		set {
			rateNowTitle = value;
		}
		get {
			return rateNowTitle;
		}
	}
	public string RateLaterTitle {
		set {
			rateLaterTitle = value;
		}
		get {
			return rateLaterTitle;
		}
	}
	public string NeverRemindTitle {
		set {
			neverRemindTitle = value;
		}
		get {
			return neverRemindTitle;
		}
	}
}
