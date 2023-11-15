using UnityEngine;

namespace Enhancer
{
    public static class PriceRandomizer
    {
        public static void Randomize()
        {

            Plugin.Log.LogInfo("Randomizing Prices");

            if (TimeOfDay.Instance.daysUntilDeadline < 1)
            {
                StartOfRound.Instance.companyBuyingRate = 1.0f;
                return;
            }

            //Company mood factor
            float MoodFactor = GetMoodFactor();
            Plugin.Log.LogInfo("Got mood factor");
            //Small increase each day
            float DaysFactor = (float)(1.0 + 0.05f * (Plugin.Cfg.DaysPerQuota - TimeOfDay.Instance.daysUntilDeadline));

            //This maximum value should only happen after more than 10 days on a single quota
            DaysFactor = Mathf.Clamp(DaysFactor, 1.0f, 2.0f);

            //float Prices = Random.Range(MoodFactor * DaysFactor, 1.0f);

            //Use the level seed to get prices
            System.Random rng = new(StartOfRound.Instance.randomMapSeed + 77);
            float Prices = (float)rng.NextDouble() * (1.0f - MoodFactor * DaysFactor) + MoodFactor;


            Plugin.Log.LogInfo("New prices set at" + Prices.ToString());
            Plugin.Log.LogInfo("    factors " + MoodFactor.ToString() + " : " + DaysFactor.ToString() + " : " + (StartOfRound.Instance.randomMapSeed + 77).ToString());

            StartOfRound.Instance.companyBuyingRate = Prices;
        }

        private static bool IsCompanyAvailable()
        {
            if (TimeOfDay.Instance is null)
                return false;

            if (TimeOfDay.Instance.currentCompanyMood is null)
                return false;

            if (TimeOfDay.Instance.currentCompanyMood.name is null)
                return false;

            return true;
        }

        private static float GetMoodFactor()
        {

            Plugin.Log.LogInfo("Get mood factor");

            float defaultFactor = 0.40f;

            if (!IsCompanyAvailable())
                return defaultFactor;

            return TimeOfDay.Instance.currentCompanyMood.name switch
            {
                "SilentCalm" => 0.35f,
                "SnoringGiant" => 0.45f,
                "Agitated" => 0.25f,
                _ => defaultFactor,
            };
        }
    }
}