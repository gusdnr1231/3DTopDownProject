public static class ValueEvents
{
    public static CamDistance CamDistanceEvent = new CamDistance();
}

public class CamDistance : GameEvent
{
    public float distance;
}
