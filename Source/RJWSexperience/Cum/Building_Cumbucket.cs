using RimWorld;
using System.Linq;
using System.Text;
using Verse;

namespace RJWSexperience // Used in Menstruation with this namespace
{
	public class Building_CumBucket : Building_Storage
	{
		protected float storedDecimalRemainder = 0f;
		protected float totalGathered = 0f;

		public int StoredStackCount
		{
			get
			{
				if (!slotGroup.HeldThings.Any())
					return 0;
				return slotGroup.HeldThings.Select(thing => thing.stackCount).Aggregate((sum, x) => sum + x);
			}
		}

		public override void ExposeData()
		{
			Scribe_Values.Look(ref storedDecimalRemainder, "storedcum", 0f);
			Scribe_Values.Look(ref totalGathered, "totalgathered", 0f);
			base.ExposeData();
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();

			string baseString = base.GetInspectString();
			if (!baseString.NullOrEmpty())
			{
				stringBuilder.AppendLine(baseString);
			}

			stringBuilder.Append(Keyed.RSTotalGatheredCum).AppendFormat("{0:0.##}ml", totalGathered);

			if (SexperienceMod.Settings.DevMode)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine($"[Debug] stored: {StoredStackCount}");
				stringBuilder.Append($"[Debug] storedDecimalRemainder: {storedDecimalRemainder}");
			}

			return stringBuilder.ToString();
		}

		public void AddCum(float amount)
		{
			AddCum(amount, VariousDefOf.GatheredCum);
		}

		public void AddCum(float amount, ThingDef cumDef)
		{
			Thing cum = ThingMaker.MakeThing(cumDef);
			AddCum(amount, cum);
		}

		public void AddCum(float amount, Thing cum)
		{
			storedDecimalRemainder += amount;
			totalGathered += amount;
			int num = (int)storedDecimalRemainder;

			cum.stackCount = num;
			if (cum.stackCount > 0 && !GenPlace.TryPlaceThing(cum, PositionHeld, Map, ThingPlaceMode.Direct, out Thing res))
			{
				FilthMaker.TryMakeFilth(PositionHeld, Map, VariousDefOf.FilthCum, num);
			}
			storedDecimalRemainder -= num;
		}
	}
}
