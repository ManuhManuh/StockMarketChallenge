using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StockMarket
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GraphController graphController;

        [SerializeField] private float maxStockValue = 100;
        [SerializeField] private float stockUpdateTimeSeconds;
        [SerializeField] private float maxStockDelta = 15;
        [SerializeField] private float initialStockValue = 50;

        private float currentStock;

        private IEnumerator UpdateStocks()
        {
            while (true)
            {
                yield return new WaitForSeconds(stockUpdateTimeSeconds);

                var delta = Random.Range(-maxStockDelta, maxStockDelta);
                currentStock = Mathf.Clamp(currentStock + delta, 0, maxStockValue);
                graphController.AddValue(currentStock);
            }
        }

        private void Start()
        {
            graphController.Setup(maxStockValue);
            currentStock = initialStockValue;

            StartCoroutine(UpdateStocks());
        }
    }
}