// SPDX-FileCopyrightText: 2024 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Eui;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.Administration;

[Serializable, NetSerializable]
public sealed class PlayerPanelEuiState(
    NetUserId guid,
    string username,
    TimeSpan playtime,
    int? totalNotes,
    int? totalBans,
    int? totalRoleBans,
    int sharedConnections,
    bool? whitelisted,
    bool canFreeze,
    bool frozen,
    bool canAhelp)
    : EuiStateBase
{
    public readonly NetUserId Guid = guid;
    public readonly string Username = username;
    public readonly TimeSpan Playtime = playtime;
    public readonly int? TotalNotes = totalNotes;
    public readonly int? TotalBans = totalBans;
    public readonly int? TotalRoleBans = totalRoleBans;
    public readonly int SharedConnections = sharedConnections;
    public readonly bool? Whitelisted = whitelisted;
    public readonly bool CanFreeze = canFreeze;
    public readonly bool Frozen = frozen;
    public readonly bool CanAhelp = canAhelp;
}


[Serializable, NetSerializable]
public sealed class PlayerPanelFreezeMessage : EuiMessageBase
{
    public readonly bool Mute;

    public PlayerPanelFreezeMessage(bool mute = false)
    {
        Mute = mute;
    }
}

[Serializable, NetSerializable]
public sealed class PlayerPanelLogsMessage : EuiMessageBase;

[Serializable, NetSerializable]
public sealed class PlayerPanelDeleteMessage : EuiMessageBase;

[Serializable, NetSerializable]
public sealed class PlayerPanelRejuvenationMessage: EuiMessageBase;

[Serializable, NetSerializable]
public sealed class PlayerPanelFollowMessage: EuiMessageBase;
