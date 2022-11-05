using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace ResearchableStatUpgrades
{
	public class StatPart_ResearchDependent : StatPart
    {
        internal void AddFactor(ResearchFactor researchFactor)
        {
            this.researchFactors.Add(researchFactor);
        }

        public override string ExplanationPart(StatRequest req)
        {
            float num = 1f;
            this.TransformValue(req, ref num);
            string text = GenText.ToStringPercent(num);
            return (text == "100%" ? new TaggedString(string.Empty) : (Translator.Translate("RSU_FactorFromResearch") + text));
        }

		public override void TransformValue(StatRequest req, ref float val)
		{
			bool flag = req.HasThing && req.Thing.def.race != null;
			if (flag)
			{
				for (int i = 0; i < this.researchFactors.Count; i++)
				{
					ResearchFactor researchFactor = this.researchFactors[i];
					bool flag2 = StatPart_ResearchDependent.ShouldApplyFactorToRequest(req, researchFactor);
					if (flag2)
					{
						val *= researchFactor.factor;
					}
				}
				for (int j = 0; j < this.repeatables.Count; j++)
				{
					ResearchFactor researchFactor2 = this.repeatables[j];
					bool flag3 = StatPart_ResearchDependent.ShouldApplyFactorToRequest(req, researchFactor2);
					if (flag3)
					{
						int factorFor = RSUUtil.RepeatableResearchManager.GetFactorFor(researchFactor2.def);
						for (j = 0; j < factorFor; j++)
						{
							val *= researchFactor2.factor;
						}
					}
				}
			}
		}

		private static bool ShouldApplyFactorToRequest(StatRequest req, ResearchFactor factor)
		{
			if (factor.def.IsFinished || factor.def.IsRepeatableResearch())
			{
				if (req.HasThing)
				{
					if (req.Thing.def.race.Humanlike || factor.applyToNonHumanlike)
					{
						if (req.Thing.Spawned)
						{
							int thingIDNumber = req.Thing.thingIDNumber;
							Array array = req.Thing.Map.mapPawns.AllPawns.ToArray();
							Pawn pawn = null;
							bool shouldApplyToPawn= true;
							if (array.Length > 0)
							{
								foreach (object obj in array)
								{
									Pawn pawn2 = (Pawn)obj;
									if (pawn2.thingIDNumber == thingIDNumber)
									{
										pawn = pawn2;
									}
								}
								bool flag6 = req.Thing.def.race.Humanlike && pawn != null;
								if (flag6)
								{
									shouldApplyToPawn = (factor.applyToSlave || !pawn.IsSlave);
								}
							}
							if (shouldApplyToPawn)
							{
								if (req.Thing.Faction == Faction.OfPlayer || factor.applyToNonColonistFaction)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public List<ResearchFactor> researchFactors = new List<ResearchFactor>();
		public List<ResearchFactor> repeatables = new List<ResearchFactor>();
	}
}