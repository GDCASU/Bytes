/*
 * Author: Cristion Dominguez
 * Date: 6 Oct. 2022
 */

using System;

public enum CharacterAllegiance : int
{
    Protagonist,
    Antagonist
}

public static class CharacterAllegianceExtensions
{
    public static int GetLayer(this CharacterAllegiance type)
    {
        if (type == CharacterAllegiance.Protagonist)
            return Constants.Layer.Protagonist;
        else
            return Constants.Layer.Antagonist;
    }

    public static int GetLayerMask(this CharacterAllegiance type)
    {
        if (type == CharacterAllegiance.Protagonist)
            return Constants.LayerMask.Protagonist;
        else
            return Constants.LayerMask.Antagonist;
    }
}