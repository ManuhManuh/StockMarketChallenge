using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

namespace StockMarket
{
    public class GameManager : MonoBehaviour, IObservable
    {
        [SerializeField] private GraphController graphController;
        [SerializeField] private TextMeshProUGUI currentPriceText;

        [SerializeField] private float maxStockValue = 100;
        [SerializeField] private float stockUpdateTimeSeconds;
        [SerializeField] private float maxStockDelta = 15;
        [SerializeField] private float initialStockValue = 50;

        public float CurrentPrice => currentStock;

        private float currentStock;
        private List<IObserver> subscribers = new List<IObserver>();

        private IEnumerator UpdateStocks()
        {
            while (true)
            {
                yield return new WaitForSeconds(stockUpdateTimeSeconds);

                var delta = Random.Range(-maxStockDelta, maxStockDelta);
                currentStock = Mathf.Clamp(currentStock + delta, 0, maxStockValue);
                graphController.AddValue(currentStock);

                // add notification to subscribers that stock value has changed
                Notify();
                UpdateUI();
            }
        }

        private void Start()
        {
            graphController.Setup(maxStockValue);
            currentStock = initialStockValue;

            StartCoroutine(UpdateStocks());
        }

        public void Subscribe(IObserver observer)
        {
            if (!subscribers.Contains(observer))
            {
                subscribers.Add(observer);
            }
        }

        public void Unsubscribe(IObserver observer)
        {
            if (subscribers.Contains(observer))
            {
                subscribers.Remove(observer);
            }
        }

        public void Notify()
        {
            foreach (var subscriber in subscribers)
            {
                subscriber.UpdatedInfo(this);
            }
        }

        private void UpdateUI()
        {
            currentPriceText.text = $"CURRENT PRICE: {currentStock}";
        }
    }
}