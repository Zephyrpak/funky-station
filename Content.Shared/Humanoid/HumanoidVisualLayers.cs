// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 ZeroDayDaemon <60460608+ZeroDayDaemon@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 csqrb <56765288+CaptainSqrBeard@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 2024 John Space <bigdumb421@gmail.com>
// SPDX-FileCopyrightText: 2024 Rouge2t7 <81053047+Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Humanoid.Markings;
using Robust.Shared.Serialization;

namespace Content.Shared.Humanoid
{
    [Serializable, NetSerializable]
    public enum HumanoidVisualLayers : byte
    {
        Special, // for the cat ears
        Tail,
        Wings, // For IPC wings porting from SimpleStation
        Hair,
        FacialHair,
        Chest,
        Head,
        Snout,
        HeadSide, // side parts (i.e., frills)
        HeadTop,  // top parts (i.e., ears)
        Eyes,
        RArm,
        LArm,
        RHand,

        LHand,
        RLeg,
        LLeg,
        RFoot,
        LFoot,
        Handcuffs,
        StencilMask,
        Ensnare,
        Fire,

    }
}
