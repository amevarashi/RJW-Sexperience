using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RJWSexperience.SexHistory.UI
{
	public class SexStatusWindow : Window
	{
		public const float WINDOW_WIDTH = 900f;
		public const float WINDOW_HEIGHT = 600f;
		public const float FONTHEIGHT = UIUtility.FONTHEIGHT;
		public const float CARDHEIGHT = UIUtility.CARDHEIGHT;
		public const float LISTPAWNSIZE = UIUtility.LISTPAWNSIZE;
		public const float BASESAT = UIUtility.BASESAT;
		public const float ICONSIZE = UIUtility.ICONSIZE;

		private static GUIStyle fontStyleCenter;
		private static GUIStyle fontStyleRight;
		private static GUIStyle fontStyleLeft;
		private static GUIStyle boxStyle;
		private static GUIStyle buttonStyle;

		private SexStatusViewModel _context;

		protected PartnerOrderMode orderMode;

		private static Vector2 LastWindowPosition { get; set; }
		private Vector2 scroll;

		private static void InitStyles()
		{
			if (fontStyleCenter != null)
			{
				return;
			}

			GUIStyleState fontStyleState = new GUIStyleState() { textColor = Color.white };
			GUIStyleState boxStyleState = GUI.skin.textArea.normal;
			GUIStyleState buttonStyleState = GUI.skin.button.normal;
			fontStyleCenter = new GUIStyle() { alignment = TextAnchor.MiddleCenter, normal = fontStyleState };
			fontStyleRight = new GUIStyle() { alignment = TextAnchor.MiddleRight, normal = fontStyleState };
			fontStyleLeft = new GUIStyle() { alignment = TextAnchor.MiddleLeft, normal = fontStyleState };
			boxStyle = new GUIStyle(GUI.skin.textArea) { hover = boxStyleState, onHover = boxStyleState, onNormal = boxStyleState };
			buttonStyle = new GUIStyle(GUI.skin.button) { hover = buttonStyleState, onHover = buttonStyleState, onNormal = buttonStyleState };
		}

		public SexStatusWindow(SexHistoryComp history)
		{
			orderMode = PartnerOrderMode.Recent;
			_context = new SexStatusViewModel(history, orderMode);

			soundClose = SoundDefOf.CommsWindow_Close;
			absorbInputAroundWindow = false;
			forcePause = false;
			preventCameraMotion = false;
			draggable = true;
			doCloseX = true;
		}

		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();

			if (LastWindowPosition == Vector2.zero)
				return;

			windowRect.x = LastWindowPosition.x;
			windowRect.y = LastWindowPosition.y;
		}

		public override Vector2 InitialSize => new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);

		public override void PreOpen()
		{
			base.PreOpen();
			InitStyles();
		}

		public override void PreClose()
		{
			base.PreClose();
			LastWindowPosition = windowRect.position;
		}

		public override void DoWindowContents(Rect inRect)
		{
			if (!SexperienceMod.Settings.SelectionLocked)
			{
				List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
				if (selectedPawns.Count == 1)
				{
					Pawn selectedPawn = selectedPawns[0];
					if (selectedPawn != _context.Pawn)
					{
						SexHistoryComp h = selectedPawn.TryGetComp<SexHistoryComp>();
						if (h != null) ChangePawn(h);
					}
				}
			}

			_context.Update();
			DrawSexStatus(inRect);
		}

		public static void ToggleWindow(SexHistoryComp history)
		{
			SexStatusWindow window = Find.WindowStack.WindowOfType<SexStatusWindow>();
			if (window != null)
			{
				if (window._context.Pawn != history.ParentPawn)
				{
					SoundDefOf.TabOpen.PlayOneShotOnCamera();
					window.ChangePawn(history);
				}
			}
			else
			{
				Find.WindowStack.Add(new SexStatusWindow(history));
			}
		}

		public void ChangePawn(SexHistoryComp history)
		{
			Find.Selector.ClearSelection();
			_context = new SexStatusViewModel(history, orderMode);
			if (!_context.Pawn.DestroyedOrNull() && Find.CurrentMap == _context.Pawn.Map)
			{
				Find.Selector.Select(_context.Pawn);
			}
		}

		/// <summary>
		/// Main contents
		/// </summary>
		protected void DrawSexStatus(Rect mainrect)
		{
			float sectionwidth = mainrect.width / 3;

			Rect leftRect = new Rect(mainrect.x, mainrect.y, sectionwidth, mainrect.height);
			Rect centerRect = new Rect(mainrect.x + sectionwidth, mainrect.y, sectionwidth, mainrect.height);
			Rect rightRect = new Rect(mainrect.x + (sectionwidth * 2), mainrect.y, sectionwidth, mainrect.height);

			//Left section
			DrawBaseSexInfoLeft(leftRect.ContractedBy(4f));

			//Center section
			DrawBaseSexInfoCenter(centerRect.ContractedBy(4f), _context.Pawn);

			//Right section
			DrawBaseSexInfoRight(rightRect.ContractedBy(4f));
		}

		protected void DrawInfoWithPortrait(Rect rect, InfoCard context)
		{
			Widgets.DrawMenuSection(rect);
			Rect portraitRect = new Rect(rect.x, rect.y, rect.height - FONTHEIGHT, rect.height - FONTHEIGHT);
			Rect nameRect = new Rect(rect.x + portraitRect.width, rect.y, rect.width - portraitRect.width, FONTHEIGHT);
			Rect sexinfoRect = new Rect(rect.x + portraitRect.width, rect.y + FONTHEIGHT, rect.width - portraitRect.width, FONTHEIGHT);
			Rect sexinfoRect2 = new Rect(rect.x + portraitRect.width, rect.y + (FONTHEIGHT * 2), rect.width - portraitRect.width, FONTHEIGHT);
			Rect bestsexRect = new Rect(rect.x + 2f, rect.y + (FONTHEIGHT * 3), rect.width - 4f, FONTHEIGHT - 2f);

			if (context.partnerRecord != null)
			{
				DrawPartnerPortrait(portraitRect, context.portraitInfo);
				Widgets.DrawHighlightIfMouseover(portraitRect);
				if (Widgets.ButtonInvisible(portraitRect))
				{
					SexHistoryComp partnerHistory = context.partnerRecord.Partner?.TryGetComp<SexHistoryComp>();
					if (partnerHistory != null)
					{
						ChangePawn(partnerHistory);
						SoundDefOf.Click.PlayOneShotOnCamera();
					}
					else
					{
						SoundDefOf.ClickReject.PlayOneShotOnCamera();
					}
				}

				GUI.Label(sexinfoRect2, context.relations + " ", fontStyleRight);
				TooltipHandler.TipRegion(rect, context.tooltip);
			}
			else
			{
				Widgets.DrawTextureFitted(portraitRect, HistoryUtility.UnknownPawn, 1.0f);
			}
			Widgets.Label(nameRect, context.name);
			Widgets.Label(sexinfoRect, context.sexCount);
			Widgets.Label(sexinfoRect2, context.orgasms);
			UIUtility.FillableBarLabeled(bestsexRect, context.bestSextype);
		}

		protected void DrawSexInfoCard(Rect rect, InfoCard context)
		{
			rect.SplitHorizontally(FONTHEIGHT, out Rect labelRect, out Rect infoRect);
			GUI.Label(labelRect, context.label, fontStyleLeft);
			GUI.Label(labelRect, context.lastSexTime, fontStyleRight);
			DrawInfoWithPortrait(infoRect, context);
		}

		/// <summary>
		/// Right section
		/// </summary>
		protected void DrawBaseSexInfoRight(Rect rect)
		{
			Listing_Standard listmain = new Listing_Standard();
			listmain.Begin(rect.ContractedBy(4f));
			foreach(InfoCard infoCard in _context.InfoCards)
			{
				DrawSexInfoCard(listmain.GetRect(CARDHEIGHT), infoCard);
			}
			GUI.Label(listmain.GetRect(FONTHEIGHT), Keyed.RS_PreferRace, fontStyleLeft);
			DrawPreferRace(listmain.GetRect(66f + 15f), _context.PreferedRaceCard);
			listmain.End();
		}

		protected void DrawPreferRace(Rect rect, PreferedRaceCard preferedRaceCard)
		{
			Widgets.DrawMenuSection(rect);
			rect.SplitVertically(rect.height - 15f, out Rect portraitRect, out Rect infoRect);
			portraitRect.height = portraitRect.width;
			Rect infoRect1 = new Rect(infoRect.x, infoRect.y, infoRect.width, FONTHEIGHT);
			Rect infoRect2 = new Rect(infoRect.x, infoRect.y + FONTHEIGHT, infoRect.width, FONTHEIGHT);
			Rect infoRect3 = new Rect(infoRect.x, infoRect.y + (FONTHEIGHT * 2), infoRect.width - 2f, FONTHEIGHT);

			Widgets.DrawTextureFitted(portraitRect, preferedRaceCard.portraitGetter(portraitRect.size), 1.0f);
			GUI.Label(infoRect1, preferedRaceCard.preferRaceLabel, fontStyleLeft);

			if (preferedRaceCard.preferRaceTypeLabel != null)
			{
				GUI.Label(infoRect1, preferedRaceCard.preferRaceTypeLabel + " ", fontStyleRight);
			}

			if (preferedRaceCard.sexCount != null)
			{
				GUI.Label(infoRect2, preferedRaceCard.sexCount, fontStyleLeft);
			}

			if (preferedRaceCard.barInfo != null)
			{
				UIUtility.FillableBarLabeled(infoRect3, (BarInfo)preferedRaceCard.barInfo);
			}
		}

		/// <summary>
		/// Center section
		/// </summary>
		protected void DrawBaseSexInfoCenter(Rect rect, Pawn pawn)
		{
			Rect portraitRect = new Rect(rect.x + (rect.width / 4), rect.y, rect.width / 2, rect.width / 1.5f);
			Rect nameRect = new Rect(portraitRect.x, portraitRect.yMax - (FONTHEIGHT * 2), portraitRect.width, FONTHEIGHT * 2);
			Rect infoRect = rect.BottomPartPixels(rect.height - portraitRect.height - 20f);

			if (Mouse.IsOver(portraitRect))
			{
				Rect lockRect = new Rect(portraitRect.xMax - ICONSIZE, portraitRect.y, ICONSIZE, ICONSIZE);
				Configurations settings = SexperienceMod.Settings;
				Texture lockIcon = settings.SelectionLocked ? HistoryUtility.Locked : HistoryUtility.Unlocked;
				Widgets.DrawTextureFitted(lockRect, lockIcon, 1.0f);
				TooltipHandler.TipRegion(lockRect, Keyed.RS_PawnLockDesc);
				if (Widgets.ButtonInvisible(lockRect))
				{
					SoundDefOf.Click.PlayOneShotOnCamera();
					settings.SelectionLocked.Value = !settings.SelectionLocked.Value;
				}
			}

			GUI.Box(portraitRect, "", boxStyle);
			Widgets.DrawTextureFitted(portraitRect, PortraitsCache.Get(pawn, portraitRect.size, Rot4.South, default, 1, true, true, false, false), 1.0f);
			if (_context.SelectedPartner != null)
			{
				Widgets.DrawHighlightIfMouseover(portraitRect);
				if (Widgets.ButtonInvisible(portraitRect))
				{
					SoundDefOf.Click.PlayOneShotOnCamera();
					_context.SetSelectedPartner(null);
				}
			}

			GUI.Box(nameRect, "", boxStyle);
			GUI.Label(nameRect.TopHalf(), _context.Name, fontStyleCenter);
			GUI.Label(nameRect.BottomHalf(), _context.AgeAndTitle, fontStyleCenter);

			Listing_Standard listmain = new Listing_Standard();
			listmain.Begin(infoRect);
			//listmain.Gap(20f);

			if (_context.VirginLabel != null)
			{
				Rect tmp = listmain.GetRect(FONTHEIGHT);
				GUI.color = Color.red;
				GUI.Box(tmp, "", boxStyle);
				GUI.color = Color.white;
				GUI.Label(tmp, _context.VirginLabel, fontStyleCenter);
				listmain.Gap(1f);
			}
			else
			{
				listmain.FillableBarLabeled(_context.TotalSex);
			}

			listmain.FillableBarLabeled(_context.Lust);
			listmain.FillableBarLabeled(_context.BestSextype);
			listmain.FillableBarLabeled(_context.RecentSextype);

			if (_context.Incest.fillPercent < _context.Necro.fillPercent)
			{
				listmain.FillableBarLabeled(_context.Necro);
			}
			else
			{
				listmain.FillableBarLabeled(_context.Incest);
			}

			listmain.FillableBarLabeled(_context.ConsumedCum);

			if (_context.CumHediff != null)
			{
				listmain.FillableBarLabeled((BarInfo)_context.CumHediff);
			}
			else
			{
				listmain.Gap(FONTHEIGHT + 1f);
			}

			if (_context.Raped.fillPercent < _context.BeenRaped.fillPercent)
			{
				listmain.FillableBarLabeled(_context.BeenRaped);
			}
			else
			{
				listmain.FillableBarLabeled(_context.Raped);
			}

			listmain.FillableBarLabeled(_context.SexSatisfaction);
			listmain.FillableBarLabeled(_context.SexSkill);

			if (_context.SelectedPartner != null)
			{
				DrawSexInfoCard(listmain.GetRect(CARDHEIGHT), _context.SelectedPartnerCard);
			}
			else
			{
				DrawExtraInfo(listmain.GetRect(CARDHEIGHT));
			}
			listmain.End();
		}

		/// <summary>
		/// Sexuality and quirks
		/// </summary>
		protected void DrawExtraInfo(Rect rect)
		{
			Widgets.DrawMenuSection(rect);
			Rect inRect = rect.ContractedBy(4f);
			Listing_Standard listmain = new Listing_Standard();
			listmain.Begin(inRect);
			listmain.Gap(4f);

			Rect sexualityRect = listmain.GetRect(FONTHEIGHT);
			if (_context.SexualityLabel != null)
			{
				Widgets.Label(sexualityRect, _context.SexualityLabel);
				Widgets.DrawHighlightIfMouseover(sexualityRect);
			}
			listmain.Gap(1f);

			Rect quirkRect = listmain.GetRect(FONTHEIGHT * 3f);
			Widgets.Label(quirkRect, _context.QuirksLabel);
			Widgets.DrawHighlightIfMouseover(quirkRect);
			TooltipHandler.TipRegion(quirkRect, _context.QuirksTooltip);
			listmain.End();
		}

		/// <summary>
		/// Left section
		/// </summary>
		protected void DrawBaseSexInfoLeft(Rect rect)
		{
			Listing_Standard listmain = new Listing_Standard();
			listmain.Begin(rect);

			//Sex statistics
			GUI.Label(listmain.GetRect(FONTHEIGHT), " " + Keyed.RS_Statistics, fontStyleLeft);
			listmain.Gap(1f);

			for (int i = 0; i < _context.SexTypes.Count; i++)
			{
				listmain.FillableBarLabeled(_context.SexTypes[i]);
			}

			//Partner list
			Rect listLabelRect = listmain.GetRect(FONTHEIGHT);
			Rect sortbtnRect = new Rect(listLabelRect.xMax - 80f, listLabelRect.y, 80f, listLabelRect.height);
			GUI.Label(listLabelRect, " " + Keyed.RS_PartnerList, fontStyleLeft);
			if (Widgets.ButtonText(sortbtnRect, orderMode.Translate()))
			{
				SoundDefOf.Click.PlayOneShotOnCamera();
				orderMode = orderMode.Next();
				_context.SetPartnerOrder(orderMode);
			}

			listmain.Gap(1f);

			Rect scrollRect = listmain.GetRect(CARDHEIGHT + 1f);
			GUI.Box(scrollRect, "", buttonStyle);
			if (!_context.Partners.EnumerableNullOrEmpty())
			{
				Rect listRect = new Rect(scrollRect.x, scrollRect.y, LISTPAWNSIZE * _context.Partners.Count(), scrollRect.height - 30f);
				Widgets.ScrollHorizontal(scrollRect, ref scroll, listRect);
				Widgets.BeginScrollView(scrollRect, ref scroll, listRect);
				DrawPartnerList(listRect, _context.Partners);
				Widgets.EndScrollView();
			}

			listmain.End();
		}

		/// <summary>
		/// Partners at the bottom of the left section
		/// </summary>
		protected void DrawPartnerList(Rect rect, IEnumerable<PartnerPortraitInfo> partners)
		{
			Rect pawnRect = new Rect(rect.x, rect.y, LISTPAWNSIZE, LISTPAWNSIZE);
			foreach (PartnerPortraitInfo partner in partners)
			{
				Rect labelRect = new Rect(pawnRect.x, pawnRect.yMax - FONTHEIGHT, pawnRect.width, FONTHEIGHT);

				DrawPartnerPortrait(pawnRect, partner);
				Widgets.DrawHighlightIfMouseover(pawnRect);
				GUI.Label(labelRect, partner.partnerRecord.Label, fontStyleCenter);
				if (Widgets.ButtonInvisible(pawnRect))
				{
					_context.SetSelectedPartner(partner.partnerRecord);
					SoundDefOf.Click.PlayOneShotOnCamera();
				}
				if (partner.partnerRecord == _context.SelectedPartner)
				{
					Widgets.DrawHighlightSelected(pawnRect);
				}

				pawnRect.x += LISTPAWNSIZE;
			}
		}

		protected void DrawPartnerPortrait(Rect rect, PartnerPortraitInfo context)
		{
			Rect iconRect = new Rect(rect.x + (rect.width * 3 / 4), rect.y, rect.width / 4, rect.height / 4);
			Texture img = context.portraitGetter(rect.size);

			if (context.partnerRecord.IamFirst)
			{
				GUI.color = HistoryUtility.HistoryColor;
				Widgets.DrawTextureFitted(rect, HistoryUtility.FirstOverlay, 1.0f);
				GUI.color = Color.white;
			}

			if (context.partnerRecord.Incest)
			{
				Widgets.DrawTextureFitted(iconRect, HistoryUtility.Incest, 1.0f);
				iconRect.x -= iconRect.width;
			}
			Widgets.DrawTextureFitted(rect, img, 1.0f);
			if (context.lover)
			{
				Widgets.DrawTextureFitted(iconRect, HistoryUtility.Heart, 1.0f);
			}
		}
	}
}
