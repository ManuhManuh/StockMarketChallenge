using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StockMarket
{
    [Serializable]
    public class GraphController
    {
        [SerializeField] private RectTransform graphContainer;
        [SerializeField] private RectTransform dotPrefab;
        [SerializeField] private RectTransform connectionPrefab;

        [Header("Visual Config")]
        [SerializeField] private float xSpacing;
        [SerializeField] private float maxDisplayedDots;

        private float maxGraphValue;
        private float graphHeight;

        private List<float> currentValues = new List<float>();

        private readonly List<RectTransform> pooledDots = new List<RectTransform>();
        private readonly List<RectTransform> pooledConnections = new List<RectTransform>();

        public void Setup(float maxStockValue)
        {
            graphHeight = graphContainer.rect.size.y;
            maxGraphValue = maxStockValue;
        }

        public void AddValue(float value)
        {
            currentValues.Add(value);
            RefreshGraphVisuals();
        }

        private void RefreshGraphVisuals()
        {
            var startIndex = (int)Math.Max(currentValues.Count - maxDisplayedDots, 0);
            for (var index = startIndex; index < currentValues.Count; index++)
            {
                var value = currentValues[index];
                var dotIndex = index - startIndex;

                var dot = dotIndex >= pooledDots.Count ? CreateDot() : pooledDots[dotIndex];
                PositionDot(dot, dotIndex, value);

                if (dotIndex > 0)
                {
                    var connectionIndex = dotIndex - 1;
                    var connection = connectionIndex >= pooledConnections.Count
                        ? CreateConnection()
                        : pooledConnections[connectionIndex];

                    ConnectDots(connection, pooledDots[dotIndex - 1], dot);
                }
            }
        }

        private void PositionDot(RectTransform dot, int dotIndex, float value)
        {
            var position = new Vector2(
                dotIndex * xSpacing,
                value / maxGraphValue * graphHeight);

            dot.anchoredPosition = position;
        }

        private void ConnectDots(RectTransform connection, RectTransform dot1, RectTransform dot2)
        {
            var direction = dot2.anchoredPosition - dot1.anchoredPosition;
            var distance = direction.magnitude;

            connection.sizeDelta = new Vector2(distance, connection.sizeDelta.y);

            connection.anchoredPosition = dot1.anchoredPosition + (direction * .5f);
            connection.localEulerAngles = new Vector3(0, 0, GetAngleFromVector(direction));
        }

        private float GetAngleFromVector(Vector3 direction) {
            direction = direction.normalized;
            float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        private RectTransform CreateDot()
        {
            var newDot = Object.Instantiate(dotPrefab, graphContainer);
            pooledDots.Add(newDot);
            return newDot;
        }

        private RectTransform CreateConnection()
        {
            var newConnection = Object.Instantiate(connectionPrefab, graphContainer);
            pooledConnections.Add(newConnection);
            newConnection.SetAsFirstSibling();
            return newConnection;
        }
    }
}
