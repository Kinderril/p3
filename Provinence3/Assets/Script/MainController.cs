using System;
using UnityEngine;
using System.Collections;

public enum MainState
{
    start,
    play,
    mission,
    parameters,
//    pause,
    shop,
    end,
    loading,
    statistics,
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
    public ScreenshotMaker ScreenshotMaker;

    void Start ()
    {
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
        DataBaseController.Instance.Init();
        WindowManager.Instance.Init();
        ShopController.Instance.Init();
        TimerManager = new TimerManager();
        PlayerData = new PlayerData();
        try
        {
            PlayerData.Load();
        } 
        catch (Exception ex)
        {
            Debug.LogError(":" + ex);
            DebugController.Instance.InfoField1.text = ex.ToString();
        }
        WindowManager.Instance.OpenWindow(MainState.start);
	}

    private void Test()
    {
#if UNITY_EDITOR

        for (int i = 0; i < 10; i++)
        {
            Debug.Log(ShopController.RandomSlot());
        }
#endif
    }

    public void StartLevel(int indexStartPos,int dif,int levelIndex)
    {
        WindowManager.Instance.OpenWindow(MainState.loading);
        StartCoroutine(w4load(indexStartPos, dif, levelIndex));
    }

    private IEnumerator w4load(int indexStartPos, int dif, int levelIndex)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        level = new Level(levelIndex, indexStartPos, dif);
        yield return level.Load((lvl) =>
        {
            WindowManager.Instance.OpenWindow(MainState.play, lvl);
        });
        yield return null;
    }

    private void w4death()
    {
        PostProcessingController.Instance.StartFadeIn();
//        yield return new WaitForSeconds(2f);
        PostProcessingController.Instance.EndFade();
        WindowManager.Instance.OpenWindow(MainState.end);
        Map.Instance.EndLevel();
        if (level.MainHero != null)
            Destroy(level.MainHero.gameObject);
        DataBaseController.Instance.Pool.Clear();
        Map.Instance.DestroyLevel();
    }

    private void EndFadeScreen()
    {

    }

    private void StartFadeScreen()
    {

    }

    public void EndLevel(bool endImmidiatly = false)
    {
        level.EndLevel(PlayerData, endImmidiatly);
        w4death();
    }

    void Update () {
        if (TimerManager != null)
	        TimerManager.Update();
	    if (level != null)
	        level.Update();
	}

}
