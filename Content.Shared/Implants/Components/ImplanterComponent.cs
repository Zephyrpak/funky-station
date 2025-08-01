// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 liltenhead <104418166+liltenhead@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Simon <63975668+Simyon264@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 Skye <57879983+Rainbeon@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Containers.ItemSlots;
using Content.Shared.Damage;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Implants.Components;
/// <summary>
/// Implanters are used to implant or extract implants from an entity.
/// Some can be single use (implant only) or some can draw out an implant
/// </summary>
//TODO: Rework drawing to work with implant cases when surgery is in
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class ImplanterComponent : Component
{
    public const string ImplanterSlotId = "implanter_slot";
    public const string ImplantSlotId = "implant";

    /// <summary>
    /// Whitelist to check entities against before implanting.
    /// Implants get their own whitelist which is checked afterwards.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// Blacklist to check entities against before implanting.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityWhitelist? Blacklist;

    /// <summary>
    /// Used for implanters that start with specific implants
    /// </summary>
    [DataField]
    public EntProtoId? Implant;

    /// <summary>
    /// The time it takes to implant someone else
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float ImplantTime = 5f;

    //TODO: Remove when surgery is a thing
    /// <summary>
    /// The time it takes to extract an implant from someone
    /// It's excessively long to deter from implant checking any antag
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float DrawTime = 30f;

    /// <summary>
    /// Good for single-use injectors
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool ImplantOnly;

    /// <summary>
    /// The current mode of the implanter
    /// Mode is changed automatically depending if it implants or draws
    /// </summary>
    [DataField, AutoNetworkedField]
    public ImplanterToggleMode CurrentMode;

    /// <summary>
    /// The name and description of the implant to show on the implanter
    /// </summary>
    [DataField]
    public (string, string) ImplantData;

    /// <summary>
    /// Determines if the same type of implant can be implanted into an entity multiple times.
    /// </summary>
    [DataField]
    public bool AllowMultipleImplants = false;

    /// <summary>
    /// The <see cref="ItemSlot"/> for this implanter
    /// </summary>
    [DataField(required: true)]
    public ItemSlot ImplanterSlot = new();

    /// <summary>
    /// If true, the implanter may be used to remove all kinds of (deimplantable) implants without selecting any.
    /// </summary>
    [DataField]
    public bool AllowDeimplantAll = false;

    /// <summary>
    /// The subdermal implants that may be removed via this implanter.
    /// </summary>
    [DataField]
    public List<EntProtoId> DeimplantWhitelist = new();

    /// <summary>
    /// The damage inflicted on a failed implant draw.
    /// </summary>
    [DataField]
    public DamageSpecifier DeimplantFailureDamage = new();

    /// <summary>
    /// Chosen implant to remove, if necessary.
    /// </summary>
    [AutoNetworkedField]
    public EntProtoId? DeimplantChosen = null;

    /// <summary>
    /// Whether or not drawing an implant deletes the implant.
    /// </summary>
    [DataField]
    public bool DeimplantCrushes = false;

    public bool UiUpdateNeeded;
}

[Serializable, NetSerializable]
public enum ImplanterToggleMode : byte
{
    Inject,
    Draw
}

[Serializable, NetSerializable]
public enum ImplanterVisuals : byte
{
    Full
}

[Serializable, NetSerializable]
public enum ImplanterImplantOnlyVisuals : byte
{
    ImplantOnly
}
