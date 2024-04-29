public enum Consumables
{
    HEALTH_POTION
}

public abstract class Consumable
{
    public abstract void Consume();
}

public class ConsumableFactory
{
    public static Consumable Factory(Consumables type)
    {
        switch (type)
        {
            case Consumables.HEALTH_POTION:
                return new HealthPotion();
            default:
                return null;
        }
    }
}