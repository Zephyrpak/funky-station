// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2021 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Database;

// DO NOT CHANGE THE NUMERIC VALUES OF THESE
[Serializable]
public enum LogImpact : sbyte
{
    Low = -1, // General logging
    Medium = 0, // Has impact on the round but not necessary for admins to be notified of
    High = 1, // Notable logs that come up in normal gameplay; new players causing these will pop up as admin alerts!
    Extreme = 2 // Irreversible round-impacting logs admins should always be notified of, OR big admin actions!!
}
