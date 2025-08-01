// SPDX-FileCopyrightText: 2023 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Morb <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Chat.Prototypes;

/// <summary>
///     IC emotes (scream, smile, clapping, etc).
///     Entities can activate emotes by chat input, radial or code.
/// </summary>
[Prototype]
public sealed partial class EmotePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    ///     Localization string for the emote name. Displayed in the radial UI.
    /// </summary>
    [DataField(required: true)]
    public string Name = default!;

    /// <summary>
    ///     Determines if emote available to all by default
    ///     <see cref="Whitelist"/> check comes after this setting
    ///     <see cref="Content.Shared.Speech.SpeechComponent.AllowedEmotes"/> can ignore this setting
    /// </summary>
    [DataField]
    public bool Available = true;

    /// <summary>
    ///     Different emote categories may be handled by different systems.
    ///     Also may be used for filtering.
    /// </summary>
    [DataField]
    public EmoteCategory Category = EmoteCategory.General;

    /// <summary>
    ///     An icon used to visually represent the emote in radial UI.
    /// </summary>
    [DataField]
    public SpriteSpecifier Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/Actions/scream.png"));

    /// <summary>
    ///     Determines conditions to this emote be available to use
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    ///     Determines conditions to this emote be unavailable to use
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;

    /// <summary>
    ///     Collection of words that will be sent to chat if emote activates.
    ///     Will be picked randomly from list.
    /// </summary>
    [DataField]
    public List<string> ChatMessages = new();

    /// <summary>
    ///     Trigger words for emote. Case independent.
    ///     When typed into players chat they will activate emote event.
    ///     All words should be unique across all emote prototypes.
    /// </summary>
    [DataField]
    public HashSet<string> ChatTriggers = new();

    // goob edit - animations
    [DataField]
    public object? Event = null;
}

/// <summary>
///     IC emote category. Usually physical source of emote,
///     like hands, voice, face, etc.
/// </summary>
[Flags]
[Serializable, NetSerializable]
public enum EmoteCategory : byte
{
    Invalid = 0,
    Vocal = 1 << 0,
    Hands = 1 << 1,
    General = byte.MaxValue
}
