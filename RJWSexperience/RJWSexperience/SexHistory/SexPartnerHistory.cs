using RimWorld;
using rjw;
using RJWSexperience.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RJWSexperience
{
	public class SexPartnerHistory : ThingComp
	{
		public SexPartnerHistory() { }
		public const int ARRLEN = 20;

		protected Dictionary<string, SexPartnerHistoryRecord> histories = new Dictionary<string, SexPartnerHistoryRecord>();
		protected string first = "";
		protected bool dirty = true;
		protected xxx.rjwSextype recentsex = xxx.rjwSextype.None;
		protected float recentsat = 0;
		protected string recentpartner = "";
		protected int[] sextypecount = new int[ARRLEN];
		protected float[] sextypesat = new float[ARRLEN];
		protected int[] sextyperecenttickabs = new int[ARRLEN];
		protected int virginstaken = 0;
		protected int incestuous = 0;
		protected int bestiality = 0;
		protected int corpsefuck = 0;
		protected int interspecies = 0;
		protected int firstsextickabs = 0;

		protected string mostpartnercache = "";
		protected xxx.rjwSextype mostsextypecache = xxx.rjwSextype.None;
		protected xxx.rjwSextype mostsatsextypecache = xxx.rjwSextype.None;
		protected xxx.rjwSextype bestsextypecache = xxx.rjwSextype.None;
		protected float bestsextypesatcache = 0;
		protected string bestpartnercache = "";
		protected int totalsexcache = 0;
		protected int totalrapedcache = 0;
		protected int totalbeenrapedcache = 0;
		protected ThingDef preferracecache = null;
		protected int preferracesexcountcache = 0;
		protected Pawn preferracepawncache = null;
		protected float avtsatcache = 0;
		protected int recentsextickabscache = 0;
		protected int mostsextickabscache = 0;
		protected int bestsextickabscache = 0;

		public Gizmo Gizmo { get; private set; }

		public SexPartnerHistoryRecord GetFirstPartnerHistory
		{
			get
			{
				Update();
				return histories.TryGetValue(first);
			}
		}
		public SexPartnerHistoryRecord GetMostPartnerHistory
		{
			get
			{
				Update();
				return histories.TryGetValue(mostpartnercache);
			}
		}
		public xxx.rjwSextype MostSextype
		{
			get
			{
				Update();
				return mostsextypecache;
			}
		}
		public xxx.rjwSextype MostSatisfiedSex
		{
			get
			{
				Update();
				return mostsatsextypecache;
			}
		}
		public SexPartnerHistoryRecord GetRecentPartnersHistory => histories.TryGetValue(recentpartner);
		public SexPartnerHistoryRecord GetBestSexPartnerHistory
		{
			get
			{
				Update();
				return histories.TryGetValue(bestpartnercache);
			}
		}
		public float TotalSexHad
		{
			get
			{
				Update();
				return totalsexcache;
			}
		}
		public int VirginsTaken => virginstaken;
		public List<SexPartnerHistoryRecord> PartnerList
		{
			get
			{
				List<SexPartnerHistoryRecord> res = null;
				Update();
				if (!histories.NullOrEmpty())
				{
					res = histories.Values.ToList();
				}
				return res;
			}
		}
		public int PartnerCount
		{
			get
			{
				if (histories == null) histories = new Dictionary<string, SexPartnerHistoryRecord>();
				return histories.Count;
			}
		}
		public int IncestuousCount => incestuous;
		public int RapedCount
		{
			get
			{
				Update();
				return totalrapedcache;
			}
		}
		public int BeenRapedCount
		{
			get
			{
				Update();
				return totalbeenrapedcache;
			}
		}
		public ThingDef PreferRace
		{
			get
			{
				Update();
				return preferracecache;
			}
		}
		public int PreferRaceSexCount
		{
			get
			{
				Update();
				return preferracesexcountcache;
			}
		}
		public int BestialityCount => bestiality;
		public int CorpseFuckCount => corpsefuck;
		public int InterspeciesCount => interspecies;
		public float AVGSat
		{
			get
			{
				Update();
				if (totalsexcache == 0) return 0;
				return sextypesat.Sum() / totalsexcache;
			}
		}
		public int RecentSexElapsedTicks => GenTicks.TicksAbs - recentsextickabscache;
		public string RecentSexDays
		{
			get
			{
				if (recentsextickabscache != 0) return GenDate.ToStringTicksToDays(RecentSexElapsedTicks) + " " + Keyed.RS_Ago;
				return "";
			}
		}
		public int FirstSexElapsedTicks => GenTicks.TicksAbs - firstsextickabs;
		public string FirstSexDays
		{
			get
			{
				if (firstsextickabs != 0) return GenDate.ToStringTicksToDays(FirstSexElapsedTicks) + " " + Keyed.RS_Ago;
				return "";
			}
		}
		public int MostSexElapsedTicks => GenTicks.TicksAbs - mostsextickabscache;
		public string MostSexDays
		{
			get
			{
				if (mostsextickabscache != 0) return GenDate.ToStringTicksToDays(MostSexElapsedTicks) + " " + Keyed.RS_Ago;
				return "";
			}
		}
		public int BestSexElapsedTicks => GenTicks.TicksAbs - bestsextickabscache;
		public string BestSexDays
		{
			get
			{
				if (bestsextickabscache != 0) return GenDate.ToStringTicksToDays(BestSexElapsedTicks) + " " + Keyed.RS_Ago;
				return "";
			}
		}

		public Texture GetPreferRaceIcon(Vector2 size)
		{
			Update();
			if (preferracepawncache != null) return PortraitsCache.Get(preferracepawncache, size, Rot4.South, default, 1, true, true, false, false);
			else return HistoryUtility.UnknownPawn;
		}

		public float GetBestSextype(out xxx.rjwSextype sextype)
		{
			Update();
			sextype = bestsextypecache;
			return bestsextypesatcache;
		}

		public float GetRecentSextype(out xxx.rjwSextype sextype)
		{
			Update();
			sextype = recentsex;
			return recentsat;
		}

		public string SextypeRecentDays(int sextype)
		{
			int index = sextype;
			if (sextyperecenttickabs[index] != 0) return GenDate.ToStringTicksToDays(GenTicks.TicksAbs - sextyperecenttickabs[index]) + " " + Keyed.RS_Ago;
			return Keyed.Unknown;
		}

		public float GetAVGSat(int index)
		{
			float res = sextypesat[index] / sextypecount[index];
			return float.IsNaN(res) ? 0f : res;
		}

		public int GetSexCount(int index)
		{
			return sextypecount[index];
		}

		public override void PostExposeData()
		{
			List<int> sextypecountsave;
			List<float> sextypesatsave;
			List<int> sextyperecenttickabssave;

			if (Scribe.mode == LoadSaveMode.Saving)
			{
				sextypecountsave = sextypecount.ToList();
				sextypesatsave = sextypesat.ToList();
				sextyperecenttickabssave = sextyperecenttickabs.ToList();
			}
			else
			{
				sextypecountsave = new List<int>();
				sextypesatsave = new List<float>();
				sextyperecenttickabssave = new List<int>();
			}

			Scribe_Collections.Look(ref histories, "histories", LookMode.Value, LookMode.Deep);
			Scribe_Values.Look(ref first, "first", string.Empty);
			Scribe_Values.Look(ref recentsex, "recentsex", xxx.rjwSextype.None);
			Scribe_Values.Look(ref recentsat, "recentsat", 0);
			Scribe_Values.Look(ref recentpartner, "recentpartner", string.Empty);
			Scribe_Values.Look(ref virginstaken, "virginstaken", 0);
			Scribe_Values.Look(ref incestuous, "incestous", 0);
			Scribe_Values.Look(ref bestiality, "bestiality", 0);
			Scribe_Values.Look(ref corpsefuck, "corpsefuck", 0);
			Scribe_Values.Look(ref interspecies, "interspecies", 0);
			Scribe_Values.Look(ref firstsextickabs, "firstsextickabs", 0);
			Scribe_Collections.Look(ref sextypecountsave, "sextypecountsave", LookMode.Value);
			Scribe_Collections.Look(ref sextypesatsave, "sextypesatsave", LookMode.Value);
			Scribe_Collections.Look(ref sextyperecenttickabssave, "sextyperecenttickabssave", LookMode.Value);

			if (histories == null)
				histories = new Dictionary<string, SexPartnerHistoryRecord>();

			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				sextypecount = sextypecountsave?.ToArray() ?? new int[ARRLEN];
				sextypesat = sextypesatsave?.ToArray() ?? new float[ARRLEN];
				sextyperecenttickabs = sextyperecenttickabssave?.ToArray() ?? new int[ARRLEN];

				foreach (KeyValuePair<string, SexPartnerHistoryRecord> element in histories)
				{
					element.Value.PartnerID = element.Key;
				}
			}
			base.PostExposeData();
		}

		public void RecordHistory(Pawn partner, SexProps props)
		{
			Pawn pawn = parent as Pawn;
			SexPartnerHistoryRecord history = GetPartnerRecord(partner);
			RecordFirst(partner, props);
			recentpartner = partner.ThingID;
			history?.RecordSex(props);
			recentsex = props.sexType;
			sextypecount[(int)props.sexType]++;
			sextyperecenttickabs[(int)props.sexType] = GenTicks.TicksAbs;
			if (partner.IsIncest(pawn)) incestuous++;
			if (partner.Dead) corpsefuck++;
			if (props.IsBestiality()) bestiality++;
			else if (pawn.def != partner.def) interspecies++;
			dirty = true;
		}

		public void RecordSatisfactionHistory(Pawn partner, SexProps props, float satisfaction)
		{
			SexPartnerHistoryRecord history = GetPartnerRecord(partner);
			RecordFirst(partner, props);
			history?.RecordSatisfaction(props, satisfaction);
			recentsat = satisfaction;
			sextypesat[(int)props.sexType] += satisfaction;
			dirty = true;
		}

		protected SexPartnerHistoryRecord GetPartnerRecord(Pawn partner)
		{
			string partnerId = partner.ThingID;

			if (histories.TryGetValue(partnerId, out SexPartnerHistoryRecord record))
			{
				return record;
			}

			SexPartnerHistoryRecord newRecord = new SexPartnerHistoryRecord(partner, partner.IsIncest(parent as Pawn));
			histories.Add(partnerId, newRecord);
			if (parent is Pawn pawn)
			{
				pawn.records.Increment(VariousDefOf.SexPartnerCount);
			}
			return newRecord;
		}

		public void RecordFirst(Pawn partner, SexProps props)
		{
			if (VirginCheck() && props.sexType == xxx.rjwSextype.Vaginal)
			{
				GetPartnerRecord(partner);
				first = partner.ThingID;
				SexPartnerHistory history = partner.GetPartnerHistory();
				firstsextickabs = GenTicks.TicksAbs;
				history?.TakeSomeonesVirgin(parent as Pawn);
			}
		}

		public void TakeSomeonesVirgin(Pawn partner)
		{
			SexPartnerHistoryRecord partnerRecord = GetPartnerRecord(partner);
			partnerRecord?.TookVirgin();
			virginstaken++;
		}

		protected void Update()
		{
			if (dirty)
			{
				UpdateStatistics();
				UpdateBestSex();
				dirty = false;
			}
		}

		protected void UpdateStatistics()
		{
			int max = 0;
			float maxsat = 0;
			float maxf = 0;
			int maxindex = 0;
			string mostID = Keyed.Unknown;
			string bestID = Keyed.Unknown;

			totalsexcache = 0;
			totalrapedcache = 0;
			totalbeenrapedcache = 0;
			Dictionary<ThingDef, int> racetotalsat = new Dictionary<ThingDef, int>();
			List<Pawn> allpartners = new List<Pawn>();

			foreach (KeyValuePair<string, SexPartnerHistoryRecord> element in histories)
			{
				SexPartnerHistoryRecord h = element.Value;

				//find most sex partner
				if (max < h.TotalSexCount)
				{
					max = h.TotalSexCount;
					mostID = element.Key;
				}
				if (maxsat < h.BestSatisfaction)
				{
					maxsat = h.BestSatisfaction;
					bestID = element.Key;
				}

				if (h.Partner != null)
				{
					Pawn partner = h.Partner;
					allpartners.Add(partner);
					if (racetotalsat.ContainsKey(h.Race))
					{
						racetotalsat[h.Race] += h.TotalSexCount - h.RapedMe;
					}
					else
					{
						racetotalsat.Add(h.Race, h.TotalSexCount - h.RapedMe);
					}
				}

				totalsexcache += h.TotalSexCount;
				totalrapedcache += h.Raped;
				totalbeenrapedcache += h.RapedMe;
			}

			if (!racetotalsat.NullOrEmpty())
			{
				KeyValuePair<ThingDef, int> prefer = racetotalsat.MaxBy(x => x.Value);
				preferracecache = prefer.Key;
				preferracesexcountcache = prefer.Value;
				preferracepawncache = allpartners.FirstOrDefault(x => x.def == preferracecache);
			}

			for (int i = 0; i < sextypecount.Length; i++)
			{
				float avgsat = sextypesat[i] / sextypecount[i];
				if (maxf < avgsat)
				{
					maxf = avgsat;
					maxindex = i;
				}
			}

			mostsatsextypecache = (xxx.rjwSextype)maxindex;
			mostsextypecache = (xxx.rjwSextype)sextypecount.FirstIndexOf(x => x == sextypecount.Max());
			mostpartnercache = mostID;
			bestpartnercache = bestID;

			recentsextickabscache = histories.TryGetValue(recentpartner)?.RecentSexTickAbs ?? 0;
			mostsextickabscache = histories.TryGetValue(mostpartnercache)?.RecentSexTickAbs ?? 0;
			bestsextickabscache = histories.TryGetValue(bestpartnercache)?.BestSexTickAbs ?? 0;

			racetotalsat.Clear();
			allpartners.Clear();
		}

		protected void UpdateBestSex()
		{
			int bestindex = 0;
			float bestsat = 0;
			float avgsat;
			for (int i = 0; i < sextypecount.Length; i++)
			{
				avgsat = sextypesat[i] / sextypecount[i];
				if (bestsat < avgsat)
				{
					bestindex = i;
					bestsat = avgsat;
				}
			}
			bestsextypecache = (xxx.rjwSextype)bestindex;
			bestsextypesatcache = bestsat;
		}

		protected bool VirginCheck()
		{
			if (histories.TryGetValue(first) != null) return false;

			Pawn pawn = parent as Pawn;
			return pawn?.IsVirgin() == true;
		}

		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);

			Gizmo = new Command_Action
			{
				defaultLabel = Keyed.RS_Sex_History,
				icon = HistoryUtility.HistoryIcon,
				defaultIconColor = HistoryUtility.HistoryColor,
				hotKey = VariousDefOf.OpenSexStatistics,
				action = delegate
				{
					UI.SexStatusWindow.ToggleWindow(parent as Pawn, this);
				}
			};
		}
	}
}
