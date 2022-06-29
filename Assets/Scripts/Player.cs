using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StockMarket;
using System;
using TMPro;

public class Player : MonoBehaviour, IObserver
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float automaticBuyPrice;

    [SerializeField] private TextMeshProUGUI totalStockText;
    [SerializeField] private TextMeshProUGUI totalCashText;

    [SerializeField] private TextMeshProUGUI buyThresholdText;
    [SerializeField] private TextMeshProUGUI autoButtonText;

    private bool automaticBuyEnabled = false;
    [SerializeField] private int totalStock = 0;
    [SerializeField] private float totalCash = 1000;

    private void Start()
    {
        UpdateUI();

    }
    public void UpdatedInfo(IObservable subject)
    {
        float currentPrice = gameManager.CurrentPrice;
        int maxAfforded = CanAffordToBuy(currentPrice);

        if (currentPrice <= automaticBuyPrice && totalCash > 0 && automaticBuyEnabled)
        {
            BuyStock();
            UpdateUI();
        }
    }

    public void ToggleAutoPurchase()
    {
        if (automaticBuyEnabled)
        {
            StopAutomaticPurchase();
        }
        else
        {
            StartAutomaticPurchase();
        }
    }

    private void StartAutomaticPurchase()
    {
        automaticBuyPrice = int.Parse(buyThresholdText.text);
        automaticBuyEnabled = true;
        autoButtonText.text = "STOP";

        // subscribe to Game Manager notifications of stock changes
        gameManager.Subscribe(this);

    }

    private void StopAutomaticPurchase()
    {
        automaticBuyEnabled = false;
        autoButtonText.text = "AUTO BUY";

        // unsubscribe from Game Manager notifications of stock changes (no longer need)
        gameManager.Unsubscribe(this);
    }

    public void BuyStock()
    {
        float currentPrice = gameManager.CurrentPrice;

        int amount = CanAffordToBuy(currentPrice);
        if (amount > 0)
        {
            totalCash -= amount * currentPrice;
            totalStock += amount;
            UpdateUI();
        }

    }

    public void SellStock()
    {
        float currentPrice = gameManager.CurrentPrice;
        
        totalCash += totalStock * currentPrice;
        totalStock = 0;
        UpdateUI();
    }

    private int CanAffordToBuy(float currentPrice)
    {
        return (int)Math.Truncate(totalCash / currentPrice);

    }

    public void IncreaseBuyThreshold()
    {
        if(automaticBuyPrice < 100) automaticBuyPrice++;
        buyThresholdText.text = automaticBuyPrice.ToString();
    }

    public void DecreaseBuyThreshold()
    {
        if(automaticBuyPrice > 0) automaticBuyPrice--;
        buyThresholdText.text = automaticBuyPrice.ToString();
    }

    private void UpdateUI()
    {
        totalStockText.text = $"TOTAL STOCK: {totalStock}";
        totalCashText.text = $"TOTAL CASH: {totalCash}";

    }

}
