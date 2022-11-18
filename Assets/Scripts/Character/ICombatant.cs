/*
 * Author: Cristion Dominguez
 * Date: 6 Oct. 2022
 */

public interface ICombatant: ICharacter
{
    public CombatantAllegiance Allegiance { get; }
}

public enum CombatantAllegiance: int
{
    Protagonist,
    Antagonist
}

public static class CombatantAllegianceExtensions
{
    public static CombatantAllegiance GetOpposite(this CombatantAllegiance type)
    {
        if (type == CombatantAllegiance.Protagonist)
            return CombatantAllegiance.Antagonist;
        else
            return CombatantAllegiance.Protagonist;
    }

    public static int GetLayer(this CombatantAllegiance type)
    {
        if (type == CombatantAllegiance.Protagonist)
            return Constants.Layer.Protagonist;
        else
            return Constants.Layer.Antagonist;
    }

    public static int GetLayerMask(this CombatantAllegiance type)
    {
        if (type == CombatantAllegiance.Antagonist)
            return Constants.LayerMask.Protagonist;
        else
            return Constants.LayerMask.Antagonist;
    }
}