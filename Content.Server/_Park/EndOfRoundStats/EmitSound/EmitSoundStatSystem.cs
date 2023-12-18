using System.Diagnostics.CodeAnalysis;
using Content.Server.GameTicking;
using Content.Shared.GameTicking;
using Content.Shared.NF14.CCVar;
using Content.Shared._Park.EndOfRoundStats.EmitSound;
using Content.Shared.Tag;
using Robust.Shared.Configuration;

namespace Content.Server._Park.EndOfRoundStats.EmitSound;

public sealed class EmitSoundStatSystem : EntitySystem
{
    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly IConfigurationManager _config = default!;


    Dictionary<SoundSources, int> soundsEmitted = new();

    // This Enum must match the exact tag you're searching for.
    // Adding a new tag to this Enum, and ensuring the localisation is set will automatically add it to the end of round stats.
    // Local string should be in the format: eorstats-emitsound-<Enum> (e.g. eorstats-emitsound-BikeHorn)
    // and should have a parameter of "times" (e.g. Horns were honked a total of {$times} times!)
    private enum SoundSources
    {
        BikeHorn,
        Plushie
    }

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EmitSoundStatEvent>(OnSoundEmitted);

        SubscribeLocalEvent<RoundEndTextAppendEvent>(OnRoundEnd);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
    }

    private void OnSoundEmitted(EmitSoundStatEvent ev)
    {
        SoundSources? source = null;

        foreach (var enumSource in Enum.GetValues<SoundSources>())
        {
            if (_tag.HasTag(ev.Emitter, enumSource.ToString()))
            {
                source = enumSource;
                break;
            }
        }

        if (source == null)
            return;

        if (soundsEmitted.ContainsKey(source.Value))
        {
            soundsEmitted[source.Value]++;
            return;
        }

        soundsEmitted.Add(source.Value, 1);
    }

    private void OnRoundEnd(RoundEndTextAppendEvent ev)
    {
        var minCount = _config.GetCVar<int>(NF14CVars.EmitSoundThreshold);

        var line = string.Empty;
        var entry = false;

        if (minCount == 0)
            return;

        foreach (var source in soundsEmitted.Keys)
        {
            if (soundsEmitted[source] > minCount && TryGenerateSoundsEmitted(source, soundsEmitted[source], out var lineTemp))
            {
                line += "\n" + lineTemp;

                entry = true;
            }
        }

        if (entry)
            ev.AddLine("[color=springGreen]" + line + "[/color]");
    }

    private bool TryGenerateSoundsEmitted(SoundSources source, int soundsEmitted, [NotNullWhen(true)] out string? line)
    {
        string preLocalString = "eorstats-emitsound-" + source.ToString();

        if (!Loc.TryGetString(preLocalString, out var localString, ("times", soundsEmitted)))
        {
            Logger.DebugS("eorstats", "Unknown messageId: {0}", preLocalString);
            Logger.Debug("Make sure the string is following the correct format, and matches the enum! (eorstats-emitsound-<enum>)");

            throw new ArgumentException("Unknown messageId: " + preLocalString);
        }

        line = localString;
        return true;
    }

    private void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        soundsEmitted.Clear();
    }
}
