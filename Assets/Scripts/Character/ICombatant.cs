/*
 * Author: Cristion Dominguez
 * Date: 6 Oct. 2022
 */

using System;

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