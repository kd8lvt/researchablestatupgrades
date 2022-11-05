﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ResearchableStatUpgrades
{
    public class ModExtension_CompoundBonus : ModExtension_ResearchScaleable
    {
        public override void ResolveCost(ref ResearchProjectDef def, int factor)
        {
            if (this.originalBaseCost < 0f)
            {
                this.originalBaseCost = def.baseCost;
            }
            def.baseCost = this.originalBaseCost * Mathf.Pow(this.factor, (float)factor);
        }
        public float originalBaseCost = -1f;
        public float factor = 1f;
    }
}
