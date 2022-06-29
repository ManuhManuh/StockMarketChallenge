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

    private bool automaticBuyEnabled = false;
    private int totalStock;
    private float totalCash;

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

    public void StartAutomaticPurchase()
    {
        automaticBuyEnabled = true;

        // subscribe to Game Manager notifications of stock changes
        gameManager.Subscribe(this);

    }

    public void StopAutomaticPurchase()
    {
        automaticBuyEnabled = false;

        // unsubscribe from Game Manager notifications of stock changes (no longer need)
        gameManager.Unsubscribe(this);
    }

    private void BuyStock()
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

    private void SellStock()
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

    private void UpdateUI()
    {
        // Change the total stock and total cash values
        totalStockText.text = $"TOTAL STOCK: {totalStock}";
        totalStockText.text = $"TOTAL CASH: {totalCash}";

        // 
    }

}
