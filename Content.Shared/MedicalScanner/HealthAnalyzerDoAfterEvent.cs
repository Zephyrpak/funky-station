// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.MedicalScanner;

[Serializable, NetSerializable]
public sealed partial class HealthAnalyzerDoAfterEvent : SimpleDoAfterEvent
{
}
