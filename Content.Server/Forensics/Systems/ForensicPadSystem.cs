// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 ike709 <ike709@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Menshin <Menshin@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tadeo <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2024 lzk <124214523+lzk228@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Labels;
using Content.Server.Popups;
using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.Forensics;
using Content.Shared.Forensics.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Inventory;

namespace Content.Server.Forensics
{
    /// <summary>
    /// Used to transfer fingerprints from entities to forensic pads.
    /// </summary>
    public sealed class ForensicPadSystem : EntitySystem
    {
        [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
        [Dependency] private readonly InventorySystem _inventory = default!;
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly LabelSystem _label = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<ForensicPadComponent, ExaminedEvent>(OnExamined);
            SubscribeLocalEvent<ForensicPadComponent, AfterInteractEvent>(OnAfterInteract);
            SubscribeLocalEvent<ForensicPadComponent, ForensicPadDoAfterEvent>(OnDoAfter);
        }

        private void OnExamined(EntityUid uid, ForensicPadComponent component, ExaminedEvent args)
        {
            if (!args.IsInDetailsRange)
                return;

            if (!component.Used)
            {
                args.PushMarkup(Loc.GetString("forensic-pad-unused"));
                return;
            }

            args.PushMarkup(Loc.GetString("forensic-pad-sample", ("sample", component.Sample)));
        }

        private void OnAfterInteract(EntityUid uid, ForensicPadComponent component, AfterInteractEvent args)
        {
            if (!args.CanReach || args.Target == null)
                return;

            if (HasComp<ForensicScannerComponent>(args.Target))
                return;

            args.Handled = true;

            if (component.Used)
            {
                _popupSystem.PopupEntity(Loc.GetString("forensic-pad-already-used"), args.Target.Value, args.User);
                return;
            }

            if (_inventory.TryGetSlotEntity(args.Target.Value, "gloves", out var gloves))
            {
                _popupSystem.PopupEntity(Loc.GetString("forensic-pad-gloves", ("target", Identity.Entity(args.Target.Value, EntityManager))), args.Target.Value, args.User);
                return;
            }

            if (TryComp<FingerprintComponent>(args.Target, out var fingerprint) && fingerprint.Fingerprint != null)
            {
                if (args.User != args.Target)
                {
                    _popupSystem.PopupEntity(Loc.GetString("forensic-pad-start-scan-user", ("target", Identity.Entity(args.Target.Value, EntityManager))), args.Target.Value, args.User);
                    _popupSystem.PopupEntity(Loc.GetString("forensic-pad-start-scan-target", ("user", Identity.Entity(args.User, EntityManager))), args.Target.Value, args.Target.Value);
                }
                StartScan(uid, args.User, args.Target.Value, component, fingerprint.Fingerprint);
                return;
            }

            if (TryComp<FiberComponent>(args.Target, out var fiber))
                StartScan(uid, args.User, args.Target.Value, component, string.IsNullOrEmpty(fiber.FiberColor) ? Loc.GetString("forensic-fibers", ("material", fiber.FiberMaterial)) : Loc.GetString("forensic-fibers-colored", ("color", fiber.FiberColor), ("material", fiber.FiberMaterial)));
        }

        private void StartScan(EntityUid used, EntityUid user, EntityUid target, ForensicPadComponent pad, string sample)
        {
            var ev = new ForensicPadDoAfterEvent(sample);

            var doAfterEventArgs = new DoAfterArgs(EntityManager, user, pad.ScanDelay, ev, used, target: target, used: used)
            {
                NeedHand = true,
                BreakOnMove = true,
            };

            _doAfterSystem.TryStartDoAfter(doAfterEventArgs);
        }

        private void OnDoAfter(EntityUid uid, ForensicPadComponent padComponent, ForensicPadDoAfterEvent args)
        {
            if (args.Handled || args.Cancelled)
            {
                return;
            }

            if (args.Args.Target != null)
            {
                string label = Identity.Name(args.Args.Target.Value, EntityManager);
                _label.Label(uid, label);
            }

            padComponent.Sample = args.Sample;
            padComponent.Used = true;

            args.Handled = true;
        }
    }
}
