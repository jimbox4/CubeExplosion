using UnityEngine;

namespace Features.Utils
{
    public static class Utils
    {
        private const int SquarePower = 2;

        public static class Randomizer
        {
            public static bool TryChance(float positiveChance, float maxChance = 100)
            {
                if (Random.Range(0, maxChance + 1) <= positiveChance)
                {
                    return true;
                }

                return false;
            }

            public static Vector3 GetRandomPosition(Vector3 centerPosition, float minValue, float maxValue)
            {
                float x = Random.Range(centerPosition.x - minValue, centerPosition.x + maxValue);
                float y = Random.Range(centerPosition.y - minValue, centerPosition.y + maxValue);
                float z = Random.Range(centerPosition.z - minValue, centerPosition.z + maxValue);

                return new Vector3(x, y, z);
            }
        }


        public static class Math
        {
            public static float ExponentialDecay(float value, float coefficient, float x)
            {
                float result;

                result = value * Mathf.Pow(coefficient, x);

                return result;
            }

            public static float LinearDecayForDistance(float maxValue, float value, float distance)
            {
                float result = (-value * maxValue * distance - 
                    value * maxValue + maxValue * Mathf.Pow(distance, SquarePower) + 
                    maxValue*distance) / Mathf.Pow(distance, SquarePower);

                return result;
            }
        }
    }
}
