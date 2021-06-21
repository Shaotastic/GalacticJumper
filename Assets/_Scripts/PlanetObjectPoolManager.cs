using ATools.Unity;

public class PlanetObjectPoolManager : ObjectPoolManager
{
    public static PlanetObjectPoolManager Instance;
    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
            Instance = this;
    }

    public override void ResetPlanet()
    {
        base.ResetPlanet();
    }
}

