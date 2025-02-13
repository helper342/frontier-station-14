﻿namespace Content.Server.Magic.Events;

public interface ISpeakSpell // The speak n spell interface
{
    /// <summary>
    /// Localized string spoken by the caster when casting this spell.
    /// </summary>
    public string? Speech { get; set;}
}

