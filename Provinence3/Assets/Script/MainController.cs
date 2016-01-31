using UnityEngine;
using System.Collections;

public enum MainState
{
    start,
    play,
    mission,
    parameters,
    pause,
    shop,
    end
}

public class MainController : Singleton<MainController>
{
    public Camera MainCamera;
    //public UIMain uiMain;
    public TimerManager TimerManager;
    public Level level;
    //public WindowInGame WindowInGame;st 
    public MainState MainState;
    public PlayerData PlayerData;
    
	void Start () {
        WindowManager.Instance.Init();
        ShopController.Instance.Init();
        TimerManager = new TimerManager();
        PlayerData = new PlayerData();
        PlayerData.Load();
        WindowManager.Instance.OpenWindow(MainState.start);
	}

    public void StartLevel(int index)
    {
        level = new Level(index);
        WindowManager.Instance.OpenWindow(MainState.play);
    }
    private IEnumerator w4death()
    {
        yield return new WaitForSeconds(1);
        GameObject.Destroy(level.MainHero.gameObject);
        WindowManager.Instance.OpenWindow(MainState.end);
    }

    public void EndLevel(bool goodEnd)
    {
        Debug.Log("EndLevel>> goodEnd:" + goodEnd);
        level.EndLevel(PlayerData, goodEnd);
        Map.Instance.EndLevel();
        StartCoroutine(w4death());
    }
    
    // Update is called once per frame
	void Update () {
        if (TimerManager != null)
	        TimerManager.Update();
	    if (level != null)
	        level.Update();
	}

}
