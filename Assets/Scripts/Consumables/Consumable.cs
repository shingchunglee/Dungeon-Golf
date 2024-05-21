public enum Consumables
{
    HEALTH_POTION,
    STRENGTH_POTION
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
            case Consumables.STRENGTH_POTION:
                return new StrengthPotion();
            default:
                return null;
        }
    }
}