using System;

public static class FishCaughtEvent
{
    public static event Action<FishSO, int> OnFishCaught;
    public static void CaughtFish(FishSO fish, int lengthOfFish)
    {
        OnFishCaught?.Invoke(fish, lengthOfFish);
    }
}
