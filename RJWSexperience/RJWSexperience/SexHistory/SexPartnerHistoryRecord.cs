using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using rjw;
using UnityEngine;
using RJWSexperience.ExtensionMethods;

namespace RJWSexperience
{
    public class SexPartnerHistoryRecord : IExposable
    {
        public SexPartnerHistory parent;
        public string partnerID;

        protected Pawn partner = null;
        protected string namecache;
        protected int totalsexhad = 0;
        protected int raped = 0;
        protected int rapedme = 0;
        protected int orgasms = 0;
        protected xxx.rjwSextype bestsextype = xxx.rjwSextype.None;
        protected float bestsatisfaction = 0;
        protected bool itookvirgin = false;
        protected bool incest = false;
        protected int recentsextickabs = 0;
        protected int bestsextickabs = 0;
        protected bool cannotLoadPawnData = false;
        protected ThingDef race;

        public string Label
        {
            get
            {
                if (partner != null)
                {
                    namecache = partner.Label;
                    return namecache;
                }
                else return namecache;
            }
        }
        public xxx.rjwSextype BestSextype
        {
            get
            {
                return bestsextype;
            }
        }
        public float BestSatisfaction
        {
            get
            {
                return bestsatisfaction;
            }
        }
        public int TotalSexCount
        {
            get
            {
                return totalsexhad;
            }
        }
        public Pawn Partner
        {
            get
            {
                if (!cannotLoadPawnData && partner == null)
                {
                    LoadPartnerPawn(partnerID);
                    if (partner == null) cannotLoadPawnData = true;
                }
                return partner;
            }
        }
        public string RapeInfo
        {
            get
            {
                string res = "";
                if (raped > 0) res += Keyed.RS_Raped + raped + " ";
                if (rapedme > 0) res += Keyed.RS_RapedMe + rapedme + " ";
                return res;
            }
        }
		public int OrgasmCount => orgasms;
		public bool IamFirst => itookvirgin;
		public bool Incest => incest;
		public int Raped => raped;
		public int RapedMe => rapedme;
		public int RecentSexTickAbs => recentsextickabs;
		public int BestSexTickAbs => bestsextickabs;
		public int BestSexElapsedTicks => GenTicks.TicksAbs - bestsextickabs;
		public string BestSexDays
        {
            get
            {
                if (bestsextickabs != 0) return Keyed.RS_HadBestSexDaysAgo(GenDate.ToStringTicksToDays(BestSexElapsedTicks) + " " + Keyed.RS_Ago);
                return "";
            }
        }
        public ThingDef Race
        {
            get
            {
                if (Partner != null)
                {
                    race = Partner.def;
                    return race;
                }
                else return race;
            }
        }

        public SexPartnerHistoryRecord() { }

        public SexPartnerHistoryRecord(Pawn pawn, bool incest = false)
        {
            this.partner = pawn;
            this.namecache = pawn.Label;
            this.incest = incest;
            this.race = pawn.def;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref namecache, "namecache", namecache, true);
            Scribe_Values.Look(ref totalsexhad, "totalsexhad", totalsexhad, true);
            Scribe_Values.Look(ref raped, "raped", raped, true);
            Scribe_Values.Look(ref rapedme, "rapedme", rapedme, true);
            Scribe_Values.Look(ref orgasms, "orgasms", orgasms, true);
            Scribe_Values.Look(ref bestsextype, "bestsextype", bestsextype, true);
            Scribe_Values.Look(ref bestsatisfaction, "bestsatisfaction", bestsatisfaction, true);
            Scribe_Values.Look(ref itookvirgin, "itookvirgin", itookvirgin, true);
            Scribe_Values.Look(ref incest, "incest", incest, true);
            Scribe_Values.Look(ref recentsextickabs, "recentsextickabs", recentsextickabs, true);
            Scribe_Values.Look(ref bestsextickabs, "bestsextickabs", bestsextickabs, true);
            Scribe_Defs.Look(ref race, "race");
        }

        public void RecordSex(SexProps props)
        {
            totalsexhad++;
            if (props.isRape)
            {
                if (partner == props.GetInteractionInitiator())
                {
                    rapedme++;
                }
                else if (partner == props.GetInteractionRecipient())
                {
                    raped++;
                }
            }
            recentsextickabs = GenTicks.TicksAbs;
        }

        public void RecordSatisfaction(SexProps props, float satisfaction)
        {
            if (satisfaction > bestsatisfaction)
            {
                orgasms++;
                bestsextype = props.sexType;
                bestsatisfaction = satisfaction;
                bestsextickabs = GenTicks.TicksAbs;
            }
        }

        public void TookVirgin()
        {
            itookvirgin = true;
        }

        protected void LoadPartnerPawn(string partnerID)
        {
            foreach (Map map in Find.Maps)
            {
                partner = map.mapPawns.AllPawns.FirstOrDefault(x => x.ThingID.Equals(partnerID));
                if (partner != null) return;
            }
            partner = Find.WorldPawns.AllPawnsAliveOrDead.FirstOrDefault(x => x.ThingID.Equals(partnerID));
        }

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
    }

}
