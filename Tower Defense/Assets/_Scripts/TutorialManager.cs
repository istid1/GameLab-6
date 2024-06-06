using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class TutorialManager : MonoBehaviour
{
    public UnityEvent[] events;

    public TMP_Text rotateTowerText, zoomText, rotateCamText, deSelectText, moneyText, roundText, fpsText, hpText;

    private int currentIndex = -1;

    [SerializeField] private int eventCounter = 0;
    
    
    private GameManager _gm;
    private TowerController _towerController;
    public List<GameObject> towerList;
    private MoneySystem _moneySystem;

    private EnemySpawner enemySpawner;
    private EnemyFlySpawner _enemyFlySpawner;
    private EnemyParent _enemyParent;
    private List<GameObject> _allEnemies;

    [SerializeField]
    private Canvas mainCanvas;

    private GameObject ballistaButton, fireButton, iceButton, lightningButton, bombButton;

   [HideInInspector] public TMP_Text tutorialText, secondTutText, clickToContinueText;
   [HideInInspector] public GameObject camCtrlPanel, moneyInfoPanel, towerPanel, panelToFlash;
   [HideInInspector] public GameObject tutorialPanel, secondTutPanel, secondImageKing;
   [HideInInspector] public Button nextEventButton, sceneSwitchButton;

   //Images of the king
   //[Header("Images King")] 
   [HideInInspector] public GameObject kingHappy, kingConfused, kingSpooked, kingCry, kingAngry;
   
    private readonly float maxAlpha = 100f / 255f; //Max opacity set to 39.2%

    private bool ballistaPlaced, icePlaced, firePlaced, lightningPlaced, bombPlaced;
    private bool firstTowerPlaced, secondTowerPlaced, thirdTowerPlaced, fourthTowerPlaced, fifthTowerPlaced;
    private bool waitingForDeath;

    private void Awake()
    {
        _gm = FindFirstObjectByType<GameManager>();
        _towerController = _gm.GetComponent<TowerController>();
        towerList = _towerController.placedTower;
        _moneySystem = _gm.GetComponent<MoneySystem>();

        enemySpawner = GameObject.FindWithTag("EnemyParent").GetComponent<EnemySpawner>();
        _enemyFlySpawner = FindFirstObjectByType<EnemyFlySpawner>();
        _enemyParent = FindFirstObjectByType<EnemyParent>();
        _allEnemies = _enemyParent.allEnemies;
        
        tutorialPanel = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(0).gameObject;
        tutorialText = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        kingHappy = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(2).gameObject;
        camCtrlPanel = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(3).gameObject;
        moneyInfoPanel = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(4).gameObject;
        towerPanel = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(5).gameObject;
        secondTutPanel = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(6).gameObject;
        secondTutText = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        secondImageKing = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(8).gameObject;
        nextEventButton = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(9).GetComponent<Button>();
        sceneSwitchButton = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(10).GetComponent<Button>();
        clickToContinueText = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(11).GetComponent<TextMeshProUGUI>();
        
        //Images of king
        kingConfused = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(13).gameObject;
        kingSpooked = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(14).gameObject;
        kingCry = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(15).gameObject;
        kingAngry = GameObject.FindWithTag("TutorialCanvas").transform.GetChild(16).gameObject;
        


        ballistaButton = mainCanvas.transform.GetChild(0).gameObject;
        fireButton = mainCanvas.transform.GetChild(1).gameObject;
        iceButton = mainCanvas.transform.GetChild(2).gameObject;
        lightningButton = mainCanvas.transform.GetChild(3).gameObject;
        bombButton = mainCanvas.transform.GetChild(4).gameObject;
        
        camCtrlPanel.SetActive(false);
        moneyInfoPanel.SetActive(false);
        towerPanel.SetActive(false);
        
        secondTutPanel.SetActive(false);
        secondTutText.text = "";
        secondImageKing.SetActive(false);
        
        nextEventButton.gameObject.SetActive(false);
        sceneSwitchButton.gameObject.SetActive(false);
        
        
        HideTowerButtons();
        HideCtrlText();
        HideMoneyText();
        
        HideAllKings();
        
        DisableAllTowerButtons();

    }

    public void StartSequence()
    {
        currentIndex = 0;
        if (events.Length > 0)
        {
            events[0].Invoke();
        }
    }

    public void NextEvent()
    {
        currentIndex++;
        if (currentIndex < events.Length)
        {
            events[currentIndex].Invoke();
            eventCounter++;
        }
    }
    
    

    // Start is called before the first frame update
    void Start()
    {
        StartSequence();
        panelToFlash = camCtrlPanel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (eventCounter < 8)
            {
                NextEvent();
            }
            
            if (eventCounter >= 11 && eventCounter <= 12)
            {
                NextEvent();
            }
            if (eventCounter >= 16 && eventCounter <= 18)
            {
                NextEvent();
            }
            if (eventCounter == 22) 
            {
                NextEvent();
            }
            if (eventCounter == 26) 
            {
                NextEvent();
            }
            if (eventCounter == 30) 
            {
                NextEvent();
            }
            
        }

        if (towerList.Count == 1 && !firstTowerPlaced)
        {
            NextEvent();
            firstTowerPlaced = true;
        }
        if (towerList.Count == 2 && !secondTowerPlaced)
        {
            NextEvent();
            secondTowerPlaced = true;
        }
        if (towerList.Count == 3 && !thirdTowerPlaced)
        {
            NextEvent();
            thirdTowerPlaced = true;
        }
        if (towerList.Count == 4 && !fourthTowerPlaced)
        {
            NextEvent();
            fourthTowerPlaced = true;
        }
        if (towerList.Count == 5 && !fifthTowerPlaced)
        {
            NextEvent();
            fifthTowerPlaced = true;
        }

        if (_allEnemies.Count == 1 )
        {
            waitingForDeath = true;
        }
        if (waitingForDeath && _allEnemies.Count == 0)
        {
            NextEvent();
            waitingForDeath = false;
        }
    }

    // public void TestEvent()
    // {
    //     Debug.LogError("TestEvent");
    // }
    
    public void ActivatePanel()
    {
        panelToFlash.SetActive(true); // Activate the panel
        StartCoroutine(FlashTwiceAndDeactivate()); // Start the flashing coroutine
    }
    IEnumerator FlashTwiceAndDeactivate()
    {
        Image panelImage = panelToFlash.GetComponent<Image>();
        // Perform the flash twice
        for (int i = 0; i < 4; i++)
        {
            // Fade in
            while (panelImage.color.a < maxAlpha)
            {
                Color tempColor = panelImage.color;
                tempColor.a += Time.deltaTime;
                panelImage.color = tempColor;
                yield return null;
            }
            
            // Fade out
            while (panelImage.color.a > 0)
            {
                Color tempColor = panelImage.color;
                tempColor.a -= Time.deltaTime;
                panelImage.color = tempColor;
                yield return null;
            }
        }

        // Set the alpha to 0 again before disabling (in case it's stuck in a semi-transparent state due to deltaTime)
        Color finalColor = panelImage.color;
        finalColor.a = 0;
        panelImage.color = finalColor;

        // Disable the panel after flashing
        panelImage.gameObject.SetActive(false);
    }

    private void HideTowerButtons()
    {
        ballistaButton.SetActive(false);
        iceButton.SetActive(false);
        fireButton.SetActive(false);
        lightningButton.SetActive(false);
        bombButton.SetActive(false);
    }

    private void ShowTowerButtons()
    {
        ballistaButton.SetActive(true);
        iceButton.SetActive(true);
        fireButton.SetActive(true);
        lightningButton.SetActive(true);
        bombButton.SetActive(true);
    }

    private void DisableAllTowerButtons()
    {
        ballistaButton.GetComponent<Button>().interactable = false;
        iceButton.GetComponent<Button>().interactable = false;
        fireButton.GetComponent<Button>().interactable = false;
        lightningButton.GetComponent<Button>().interactable = false;
        bombButton.GetComponent<Button>().interactable = false;
    }

    private void HideCtrlText()
    {
        rotateTowerText.gameObject.SetActive(false);
        zoomText.gameObject.SetActive(false);
        rotateCamText.gameObject.SetActive(false);
        deSelectText.gameObject.SetActive(false);
        
    }

    private void ShowCtrlText()
    {
        rotateTowerText.gameObject.SetActive(true);
        zoomText.gameObject.SetActive(true);
        rotateCamText.gameObject.SetActive(true);
        deSelectText.gameObject.SetActive(true);
    }

    private void HideMoneyText()
    {
        moneyText.gameObject.SetActive(false);
        roundText.gameObject.SetActive(false);
        fpsText.gameObject.SetActive(false);
        hpText.gameObject.SetActive(false);
    }

    private void ShowMoneyText()
    {
        moneyText.gameObject.SetActive(true);
        roundText.gameObject.SetActive(true);
        fpsText.gameObject.SetActive(true);
        hpText.gameObject.SetActive(true);
    }

    public void TutPart1()
    {
        tutorialText.text = "Welcome to your new role as Chief Strategist of our kingdom. ";
        clickToContinueText.text = "Click anywhere to continue.";
        
        kingHappy.SetActive(true);
    }
    public void TutPart2()
    {
        tutorialText.text = "As my trusted advisor in matters of defense and warfare, your insights will be invaluable in " +
            "safeguarding our realm against threats both internal and external.";
        clickToContinueText.text = "Click anywhere to continue.";
        
    }
    public void TutPart3()
    {
        tutorialText.text = "Together, we shall fortify our defenses and ensure the prosperity and security of our people.";
        clickToContinueText.text = "Click anywhere to continue.";
        
    }
    public void TutPart4()
    {
        tutorialText.text = "First, let's acquaint you with the basics.";
        clickToContinueText.text = "Click anywhere to continue.";

    }
    public void TutPart5()
    {
        tutorialText.text = "In the bottom left corner, you'll find the camera controls and options to deselect towers.";
        clickToContinueText.text = "Click anywhere to continue.";
        ShowCtrlText();
        ActivatePanel();

    }
    public void TutPart6()
    {
        tutorialText.text = "At the top of the screen, keep an eye on your available funds, current round, and the health of our castle.";
        clickToContinueText.text = "Click anywhere to continue.";
        ShowMoneyText();
        panelToFlash = moneyInfoPanel;
        ActivatePanel();
    }
    public void TutPart7()
    {
        tutorialText.text = "Here, you'll find all the available towers for placement.";
        clickToContinueText.text = "Click anywhere to continue.";
        panelToFlash = towerPanel;
        ShowTowerButtons();
        ActivatePanel();

    }
    public void TutPart8()
    {
        tutorialText.text = "Hold on! Our scouts have spotted enemies closing in.";
        clickToContinueText.text = "Click anywhere to continue.";
        
        HideAllKings();
        kingSpooked.SetActive(true);

    }

    public void TutPart9()
    {
        tutorialText.text =
            "Select the ballista tower and position it in the middle of the battlefield. \nRemember, the enemy always aims for the shortest route to our castle.";
        ballistaButton.GetComponent<Button>().interactable = true;
        clickToContinueText.text = "";
        
        HideAllKings();
        kingAngry.SetActive(true);
    }

    public void TutPart10()
    {
        tutorialText.text = "Excellent! Now that you've chosen the Ballista tower, it's time to position it on the battlefield. Click on an empty tile to place it.";
        _gm.currentRound = 1;
        
    }

    public void TutPart11()
    {
        tutorialText.text = "Well done! Let's patiently await the enemy's approach.";
        
        HideAllKings();
        kingHappy.SetActive(true);
        
        ballistaButton.GetComponent<Button>().interactable = false;
        
        enemySpawner.SpawnStoneEnemy();
         
         
    }

    public void TutPart12()
    {
        tutorialText.text = "Our scouts have sighted a new type of enemy approaching.";
        clickToContinueText.text = "Click anywhere to continue.";
        
        HideAllKings();
        kingSpooked.SetActive(true);
    }

    public void TutPart13()
    {
        tutorialText.text = "This one's an ice type, which means we'll need our fire tower to melt its icy defenses before we can inflict damage.";
        clickToContinueText.text = "Click anywhere to continue.";
        
        HideAllKings();
        kingConfused.SetActive(true);
    }
    public void TutPart14()
    {
        tutorialText.text = "Chose the fire tower.";
        clickToContinueText.text = "";
        fireButton.GetComponent<Button>().interactable = true;
        _moneySystem.currentMoney = 100;
        
        HideAllKings();
        kingAngry.SetActive(true);

    }
    public void TutPart15()
    {
        tutorialText.text = "And position it strategically. Ensure the ballista has ample time to weaken its defenses.";
    }

    public void TutPart16()
    {
        tutorialText.text = "Now, let's anticipate its advance.";
        enemySpawner.SpawnFireEnemy();
        fireButton.GetComponent<Button>().interactable = false;
    }

    public void TutPart17()
    {
        tutorialText.text = "Splendid work!";
        clickToContinueText.text = "Click anywhere to continue.";
        
        HideAllKings();
        kingHappy.SetActive(true);
    }
    public void TutPart18()
    {
        tutorialText.text = "Another adversary approaches, this time a fire type.";
        clickToContinueText.text = "Click anywhere to continue.";
        
        HideAllKings();
        kingSpooked.SetActive(true);
    }
    public void TutPart19()
    {
        tutorialText.text = "We'll counter with ice to cool its fiery temper.";
        clickToContinueText.text = "Click anywhere to continue.";
        
        HideAllKings();
        kingAngry.SetActive(true);
    }
    public void TutPart20()
    {
        tutorialText.text = "Select the ice tower.";
        iceButton.GetComponent<Button>().interactable = true;
        _moneySystem.currentMoney = 150;
        clickToContinueText.text = "";
    }
    public void TutPart21()
    {
        tutorialText.text = "And position it carefully.";
    }
    public void TutPart22()
    {
        tutorialText.text = "Excellent! Now, we await its arrival.";
        enemySpawner.SpawnIceEnemy();
        iceButton.GetComponent<Button>().interactable = false;
        
        HideAllKings();
        kingHappy.SetActive(true);
    }
    public void TutPart23()
    {
        tutorialText.text = "Here comes the next challenge: a flying enemy. Beware, it can soar over our defenses.";
        clickToContinueText.text = "Click anywhere to continue.";
        
        HideAllKings();
        kingAngry.SetActive(true);
    }
    public void TutPart24()
    {
        clickToContinueText.text = "";
        tutorialText.text = "We'll need the lightning tower to strike it down. Select it.";
        lightningButton.GetComponent<Button>().interactable = true;
        _moneySystem.currentMoney = 200;
    }
    public void TutPart25()
    {
        tutorialText.text = "Now place it on the battlefield.";
    }
    public void TutPart26()
    {
        tutorialText.text = "Let's be patient as we await its descent.";
        _enemyFlySpawner.SpawnFlyingEnemy(1);
        lightningButton.GetComponent<Button>().interactable = false;
        
        HideAllKings();
        kingHappy.SetActive(true);
    }
    public void TutPart27()
    {
        tutorialText.text = "The final foe approaches, a formidable one indeed. We'll deploy bombs to handle this threat.";
        clickToContinueText.text = "Click anywhere to continue.";
        
        HideAllKings();
        kingAngry.SetActive(true);
    }
    public void TutPart28()
    {
        clickToContinueText.text = "";
        tutorialText.text = "Chose the bomb tower";
        bombButton.GetComponent<Button>().interactable = true;
        _moneySystem.currentMoney = 250;
    }
    public void TutPart29()
    {
        tutorialText.text = "Position it with care.";
    }
    public void TutPart30()
    {
        tutorialText.text = "And now, we wait once more.";
        enemySpawner.SpawnBombEnemy();
        bombButton.GetComponent<Button>().interactable = false;
    }
    public void TutPart31()
    {
        tutorialText.text = "Bravo! You've handled each challenge admirably.";
        clickToContinueText.text = "Click anywhere to continue.";
        
        HideAllKings();
        kingHappy.SetActive(true);
    }
    public void TutPart32()
    {
        tutorialPanel.SetActive(false);
        tutorialText.text = "";
        
        
        secondTutPanel.SetActive(true);
        secondTutText.text = "Ah, one more crucial detail! Click on any placed tower to access the upgrade menu.";
        secondImageKing.SetActive(true);
        nextEventButton.gameObject.SetActive(true);
        clickToContinueText.text = "";
        
        HideAllKings();
        secondImageKing.SetActive(true);
    }
    public void TutPart33()
    {
        secondTutText.text = "Here, you can enhance its range, damage, and fire rate, or sell it if needed.";
    }
    public void TutPart34()
    {
        secondTutText.text = "That concludes our tutorial. Best of luck defending our kingdom!";
        nextEventButton.gameObject.SetActive(false);
        sceneSwitchButton.gameObject.SetActive(true);
    }
    

        //Calling NextEvent on the first button press
    public void BallistaNextEvent()
    {
        if (!ballistaPlaced)
        {
            NextEvent();
            ballistaPlaced = true;
        }
    }
    public void FireNextEvent()
    {
        if (!firePlaced)
        {
            NextEvent();
            firePlaced = true;
        }
    }
    public void IceNextEvent()
    {
        if (!icePlaced)
        {
            NextEvent();
            icePlaced = true;
        }
    }
    public void LightningNextEvent()
    {
        if (!lightningPlaced)
        {
            NextEvent();
            lightningPlaced = true;
        }
    }
    public void BombNextEvent()
    {
        if (!bombPlaced)
        {
            NextEvent();
            bombPlaced = true;
        }
    }

    public void HideAllKings()
    {
        kingHappy.SetActive(false);
        kingConfused.SetActive(false);
        kingSpooked.SetActive(false);
        kingCry.SetActive(false);
        kingAngry.SetActive(false);
    }
}
