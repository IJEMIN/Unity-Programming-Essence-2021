using UnityEngine;
using UnityEngine.UI; // 🌟 [중요] Image 컴포넌트를 쓰기 위해 UI 네임스페이스를 가져옵니다.
using TMPro;
using System.Collections;

// 인스펙터 창에서 아이템 목록을 이쁘게 편집할 수 있도록 만드는 옵션
[System.Serializable]
public class ShopItem
{
    public string itemName;      // 아이템 이름 (예: "ScoreMultiplier", "Shield", "SpeedUp")
    public int basePrice = 15;   // 시작 기본 가격
    public int currentLevel = 1; // 현재 업그레이드 레벨
    
    // UI 연결용 컴포넌트들
    public TextMeshProUGUI currentLevelText; 
    public TextMeshProUGUI priceText;
    
    // 🌟 [추가] 인스펙터 창에서 아이템 이미지를 넣을 수 있는 칸을 만듭니다!
    public Image itemIcon; 
}

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;         // 상점 UI 패널
    public TextMeshProUGUI shopCoinText;   // 상점 내 코인 표시 텍스트

    // 유니티 인스펙터에서 아이템 리스트를 원하는 만큼 늘릴 수 있습니다!
    public ShopItem[] shopItems;

    private void Start()
    {
        // 게임이 처음 켜지면 상점에 등록된 아이템들의 레벨을 하드디스크 저장소에서 읽어와 동기화합니다.
        foreach (ShopItem item in shopItems)
        {
            item.currentLevel = PlayerPrefs.GetInt(item.itemName + "_Level", 1);
        }
    }

    void Update()
    {
        if (shopPanel.activeSelf)
        {
            shopCoinText.text = "COINS: " + GameManager.instance.coinCount; 

            // 등록된 모든 아이템의 가격과 레벨 텍스트를 실시간 업데이트
            foreach (ShopItem item in shopItems)
            {
                int currentPrice = item.basePrice * item.currentLevel;
                
                if(item.currentLevelText != null)
                    item.currentLevelText.text = "LV: " + item.currentLevel;
                if(item.priceText != null)
                    item.priceText.text = "COST: " + currentPrice;
            }
        }
    }

    public void OpenShop() { shopPanel.SetActive(true); Time.timeScale = 0f; }
    
    public void CloseShop() 
    { 
        shopPanel.SetActive(false); 
        StartCoroutine(ResumeGameTime()); 
    }

    private IEnumerator ResumeGameTime()
    {
        yield return null; 
        Time.timeScale = 1f; 
    }

    public void BuyItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= shopItems.Length) return;

        ShopItem targetItem = shopItems[itemIndex];
        int currentPrice = targetItem.basePrice * targetItem.currentLevel;

        if (GameManager.instance.coinCount >= currentPrice)
        {
            GameManager.instance.coinCount -= currentPrice; 
            PlayerPrefs.SetInt("TotalCoins", GameManager.instance.coinCount);
            
            if (GameManager.instance.coinText != null) {
                GameManager.instance.coinText.text = "COIN : " + GameManager.instance.coinCount;
            }
            
            targetItem.currentLevel += 1; 

            PlayerPrefs.SetInt(targetItem.itemName + "_Level", targetItem.currentLevel);
            PlayerPrefs.Save();

            ApplyItemEffect(targetItem.itemName, targetItem.currentLevel);

            Debug.Log(targetItem.itemName + " 구매 성공! 현재 레벨: " + targetItem.currentLevel);
        }
        else
        {
            Debug.Log("코인이 부족합니다!");
        }
    }

    private void ApplyItemEffect(string itemName, int nextLevel)
    {
        switch (itemName)
        {
            case "ScoreMultiplier":
                GameManager.instance.SetScoreMultiplier(nextLevel);
                break;

            case "Shield":
                GameManager.instance.hasShield = true;
                break;

            case "SpeedUp":
                break;
        }
    }
}