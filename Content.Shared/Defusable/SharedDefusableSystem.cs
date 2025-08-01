// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 eclips_e <67359748+Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Defusable;

/// <summary>
/// This handles defusable explosives, such as Syndicate Bombs.
/// </summary>
/// <remarks>
/// Most of the logic is in the server
/// </remarks>
public abstract class SharedDefusableSystem : EntitySystem
{

}

[NetSerializable, Serializable]
public enum DefusableVisuals
{
    Active
}

[NetSerializable, Serializable]
public enum DefusableWireStatus
{
    LiveIndicator,
    BoltIndicator,
    BoomIndicator,
    DelayIndicator,
    ProceedIndicator,
}
