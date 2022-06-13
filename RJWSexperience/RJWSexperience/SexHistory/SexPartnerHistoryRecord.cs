using rjw;
using RJWSexperience.ExtensionMethods;
using System.Collections.Generic;
using Verse;

namespace RJWSexperience.SexHistory
{
	public class SexPartnerHistoryRecord : IExposable
	{
		public string PartnerID { get; set; }

		protected Pawn partner = null;
		protected string labelCache;
		protected int totalSexCount = 0;
		protected int raped = 0;
		protected int rapedMe = 0;
		protected int orgasms = 0;
		protected xxx.rjwSextype bestSextype = xxx.rjwSextype.None;
		protected float bestSatisfaction = 0;
		protected bool iTookVirgin = false;
		protected bool incest = false;
		protected int recentSexTickAbs = 0;
		protected int bestSexTickAbs = 0;
		protected bool cannotLoadPawnData = false;
		protected ThingDef raceCache;

		public xxx.rjwSextype BestSextype => bestSextype;
		public float BestSatisfaction => bestSatisfaction;
		public int TotalSexCount => totalSexCount;
		public int OrgasmCount => orgasms;
		public bool IamFirst => iTookVirgin;
		public bool Incest => incest;
		public int Raped => raped;
		public int RapedMe => rapedMe;
		public int RecentSexTickAbs => recentSexTickAbs;
		public int BestSexTickAbs => bestSexTickAbs;
		public int BestSexElapsedTicks => GenTicks.TicksAbs - bestSexTickAbs;
		public Pawn Partner
		{
			get
			{
				if (!cannotLoadPawnData && partner == null)
				{
					LoadPartnerPawn(PartnerID);
					if (partner == null) cannotLoadPawnData = true;
				}
				return partner;
			}
		}
		public string Label
		{
			get
			{
				if (Partner != null)
					labelCache = Partner.Label;
				return labelCache;
			}
		}
		public ThingDef Race
		{
			get
			{
				if (Partner != null)
					raceCache = Partner.def;
				return raceCache;
			}
		}

		public SexPartnerHistoryRecord() { }

		public SexPartnerHistoryRecord(Pawn pawn, bool incest = false)
		{
			this.partner = pawn;
			this.labelCache = pawn.Label;
			this.incest = incest;
			this.raceCache = pawn.def;
		}

		public void ExposeData()
		{
			Scribe_Values.Look(ref labelCache, "namecache");
			Scribe_Values.Look(ref totalSexCount, "totalsexhad", 0);
			Scribe_Values.Look(ref raped, "raped", 0);
			Scribe_Values.Look(ref rapedMe, "rapedme", 0);
			Scribe_Values.Look(ref orgasms, "orgasms", 0);
			Scribe_Values.Look(ref bestSextype, "bestsextype", xxx.rjwSextype.None);
			Scribe_Values.Look(ref bestSatisfaction, "bestsatisfaction", 0f);
			Scribe_Values.Look(ref iTookVirgin, "itookvirgin", false);
			Scribe_Values.Look(ref incest, "incest", false);
			Scribe_Values.Look(ref recentSexTickAbs, "recentsextickabs", 0);
			Scribe_Values.Look(ref bestSexTickAbs, "bestsextickabs", 0);
			Scribe_Defs.Look(ref raceCache, "race");
		}

		public void RecordSex(SexProps props)
		{
			totalSexCount++;
			if (props.isRape)
			{
				if (partner == props.GetInteractionInitiator())
					rapedMe++;
				else
					raped++;
			}
			recentSexTickAbs = GenTicks.TicksAbs;
		}

		public void RecordSatisfaction(SexProps props, float satisfaction)
		{
			orgasms++;

			if (satisfaction > bestSatisfaction)
			{
				bestSextype = props.sexType;
				bestSatisfaction = satisfaction;
				bestSexTickAbs = GenTicks.TicksAbs;
			}
		}

		public void TookVirgin()
		{
			iTookVirgin = true;
		}

		protected void LoadPartnerPawn(string partnerID)
		{
			foreach (Map map in Find.Maps)
			{
				partner = map.mapPawns.AllPawns.Find(x => x.ThingID.Equals(partnerID));
				if (partner != null) return;
			}
			partner = Find.WorldPawns.AllPawnsAliveOrDead.Find(x => x.ThingID.Equals(partnerID));
		}

		#region OrderComparers

		public class RecentOrderComparer : IComparer<SexPartnerHistoryRecord>
		{
			public int Compare(SexPartnerHistoryRecord x, SexPartnerHistoryRecord y)
			{
				return y.RecentSexTickAbs.CompareTo(x.RecentSexTickAbs);
			}
		}

		public class MostOrderComparer : IComparer<SexPartnerHistoryRecord>
		{
			public int Compare(SexPartnerHistoryRecord x, SexPartnerHistoryRecord y)
			{
				return y.TotalSexCount.CompareTo(x.TotalSexCount);
			}
		}

		public class NameOrderComparer : IComparer<SexPartnerHistoryRecord>
		{
			public int Compare(SexPartnerHistoryRecord x, SexPartnerHistoryRecord y)
			{
				return x.Label.CompareTo(y.Label);
			}
		}

		#endregion OrderComparers
	}
}
