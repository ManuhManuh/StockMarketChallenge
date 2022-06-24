using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StockMarket;
using System;
using TMPro;

public class Player : IObserver
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float automaticBuyPrice;

    [SerializeField] private TextMeshProUGUI totalStockText;
    [SerializeField] private TextMeshProUGUI totalCashText;
    [SerializeField] private TextMeshProUGUI buyAmountText;
    [SerializeField] private TextMeshProUGUI sellAmountText;

    private bool automaticBuyEnabled = false;
    private int totalStock;
    private float totalCash;

    public void Update(IObservable subject)
    {
        float currentPrice = gameManager.CurrentPrice;
        int maxAfforded = CanAffordToBuy(currentPrice);

        if (currentPrice <= automaticBuyPrice && totalCash > 0 && automaticBuyEnabled)
        {
            BuyStock(maxAfforded, currentPrice);
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

    private void UpdateUI()
    {
        // Change the total stock and total cash values
    }

    private void BuyStock(int amount, float currentPrice)
    {

        if (CanAffordToBuy(currentPrice) >= amount)
        {
            totalCash -= amount * currentPrice;
            totalStock += amount;
            UpdateUI();
        }

    }

    private void SellStock(int amount, float currentPrice)
    {
        int sellAmount = amount > totalStock ? totalStock : amount;

        totalStock -= sellAmount;
        totalCash += sellAmount * currentPrice;
        UpdateUI();
    }

    private int CanAffordToBuy(float currentPrice)
    {
        return (int)Math.Truncate(totalCash / currentPrice);

    }


}
