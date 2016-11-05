using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Notifications;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Spells;
using EloBuddy.SDK.ThirdParty;
using EloBuddy.SDK.ThirdParty.Glide;
using EloBuddy.SDK.Utils;

namespace PerfectCombo_Ryze
{
	class Program
	{
		static bool QReset = true;
		static bool Initiate = true;
		static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 60, DamageType.Magical) { AllowedCollisionCount = 0, MinimumHitChance = HitChance.High };
		static Spell.Targeted W = new Spell.Targeted(SpellSlot.W, 615, DamageType.Magical);
		static Spell.Targeted E = new Spell.Targeted(SpellSlot.E, 615, DamageType.Magical);
		
		public static void Main(string[] args)
		{
			Loading.OnLoadingComplete += Loading_OnLoadingComplete;
		}

		static void Loading_OnLoadingComplete(EventArgs args)
		{
			Game.OnTick += Game_OnTick;
			Obj_AI_Base.OnBuffLose += Obj_AI_Base_OnBuffLose;
		}
		
		static void Game_OnTick(EventArgs args)
		{
			if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
			{
				CastSpells();
			}
		}
		
		static void CastSpells()
		{
			if (TargetIn != null)
			{
				AIHeroClient target = TargetIn;
				if (Initiate)
				{
					Chat.Print("initiate");
					if (Q.IsReady())
					{
						Q.Cast(target);
					}
					
					if (W.IsReady())
					{
						W.Cast(target);
					}
					else if (E.IsReady())
					{
						E.Cast(target);
					}
				}
				if (Q.IsReady())
				{
					Q.Cast(target);
					return;
				}
				if (QReset)
				{
					if (Q.IsReady())
					{
						Q.Cast(target);
						return;
					}
				}
				else
				{
					if (W.IsReady())
					{
						W.Cast(target);
						return;
					}
					if (E.IsReady())
					{
						E.Cast(target);
						return;
					}
				}
				return;
			}
			if (TargetOut != null)
			{
				AIHeroClient target = TargetOut;
				if (Q.IsReady())
				{
					Q.Cast(target);
				}
			}
		}
		
		static AIHeroClient TargetIn
		{
			get
			{
				var t = TargetSelector.GetTarget(615, DamageType.Magical);
				if (t.IsValidTarget())
				{
					return t;
				}
				return null;
			}
		}
		
		static AIHeroClient TargetOut
		{
			get
			{
				var t = TargetSelector.GetTarget(1000, DamageType.Magical);
				if (t.IsValidTarget())
				{
					return t;
				}
				return null;
			}
		}

		static void Obj_AI_Base_OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
		{
			if (sender.IsMe)
			{
				if (args.Buff.Name == "ryzeqiconnocharge")
				{
					QReset = true;
					if (Initiate)
						Initiate = false;
				}
				if (args.Buff.Name == "ryzeqiconhalfcharge")
				{
					QReset = false;
					if (Initiate)
						Initiate = false;
				}
			}
		}
	}
}
