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
    end,
    loading,
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
	    Test();
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
        level = new Level(levelIndex, indexStartPos, dif, (lvl) =>
        {
            WindowManager.Instance.OpenWindow(MainState.play, lvl);
        });
    }

    private IEnumerator w4death()
    {
        PostProcessingController.Instance.StartFadeIn();
        yield return new WaitForSeconds(2f);
        PostProcessingController.Instance.EndFade();
        WindowManager.Instance.OpenWindow(MainState.end);
        Map.Instance.EndLevel();
        if (level.MainHero != null)
            Destroy(level.MainHero.gameObject);
        DataBaseController.Instance.Pool.Clear();
    }

    private void EndFadeScreen()
    {

    }

    private void StartFadeScreen()
    {

    }

    public void EndLevel(EndlevelType goodEnd)
    {
        Debug.Log("EndLevel>> goodEnd:" + goodEnd);
        level.EndLevel(PlayerData, goodEnd);
        StartCoroutine(w4death());
    }

    void Update () {
        if (TimerManager != null)
	        TimerManager.Update();
	    if (level != null)
	        level.Update();
	}

}
