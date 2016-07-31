#region using directives

using System;
using POGOProtos.Data;
using POGOProtos.Enums;

#endregion

namespace PoGo.NecroBot.Logic.PoGoUtils
{
    public struct BaseStats
    {
        public int BaseAttack, BaseDefense, BaseStamina;

        public BaseStats(int baseStamina, int baseAttack, int baseDefense)
        {
            BaseAttack = baseAttack;
            BaseDefense = baseDefense;
            BaseStamina = baseStamina;
        }

        public override string ToString()
        {
            return $"({BaseAttack} atk,{BaseDefense} def,{BaseStamina} sta)";
        }
    }

    public static class PokemonInfo
    {
        public static int CalculateCp(PokemonData poke)
        {
            return
                Math.Max(
                    (int)
                        Math.Floor(0.1*CalculateCpMultiplier(poke)*
                                   Math.Pow(poke.CpMultiplier + poke.AdditionalCpMultiplier, 2)), 10);
        }

        public static double CalculateCpMultiplier(PokemonData poke)
        {
            var baseStats = GetBaseStats(poke.PokemonId);
            return (baseStats.BaseAttack + poke.IndividualAttack)*
                   Math.Sqrt(baseStats.BaseDefense + poke.IndividualDefense)*
                   Math.Sqrt(baseStats.BaseStamina + poke.IndividualStamina);
        }

        public static int CalculateMaxCp(PokemonData poke)
        {
            return
                Math.Max(
                    (int)
                        Math.Floor(0.1*CalculateMaxCpMultiplier(poke)*
                                   Math.Pow(poke.CpMultiplier + poke.AdditionalCpMultiplier, 2)), 10);
        }

        public static double CalculateMaxCpMultiplier(PokemonData poke)
        {
            var baseStats = GetBaseStats(poke.PokemonId);
            return (baseStats.BaseAttack + 15)*Math.Sqrt(baseStats.BaseDefense + 15)*
                   Math.Sqrt(baseStats.BaseStamina + 15);
        }

        public static int CalculateMinCp(PokemonData poke)
        {
            return
                Math.Max(
                    (int)
                        Math.Floor(0.1*CalculateMinCpMultiplier(poke)*
                                   Math.Pow(poke.CpMultiplier + poke.AdditionalCpMultiplier, 2)), 10);
        }

        public static double CalculateMinCpMultiplier(PokemonData poke)
        {
            var baseStats = GetBaseStats(poke.PokemonId);
            return baseStats.BaseAttack*Math.Sqrt(baseStats.BaseDefense)*Math.Sqrt(baseStats.BaseStamina);
        }

        public static double CalculatePokemonPerfection(PokemonData poke)
        {
            if (Math.Abs(poke.CpMultiplier + poke.AdditionalCpMultiplier) <= 0)
                return (poke.IndividualAttack + poke.IndividualDefense + poke.IndividualStamina)/45.0*100.0;

            GetBaseStats(poke.PokemonId);
            var maxCp = CalculateMaxCpMultiplier(poke);
            var minCp = CalculateMinCpMultiplier(poke);
            var curCp = CalculateCpMultiplier(poke);

            return (curCp - minCp)/(maxCp - minCp)*100.0;
        }

        public static BaseStats GetBaseStats(PokemonId id)
        {
            switch ((int) id)
            {
                case 1:
                    return new BaseStats(90, 126, 126);
                case 2:
                    return new BaseStats(120, 156, 158);
                case 3:
                    return new BaseStats(160, 198, 200);
                case 4:
                    return new BaseStats(78, 128, 108);
                case 5:
                    return new BaseStats(116, 160, 140);
                case 6:
                    return new BaseStats(156, 212, 182);
                case 7:
                    return new BaseStats(88, 112, 142);
                case 8:
                    return new BaseStats(118, 144, 176);
                case 9:
                    return new BaseStats(158, 186, 222);
                case 10:
                    return new BaseStats(90, 62, 66);
                case 11:
                    return new BaseStats(100, 56, 86);
                case 12:
                    return new BaseStats(120, 144, 144);
                case 13:
                    return new BaseStats(80, 68, 64);
                case 14:
                    return new BaseStats(90, 62, 82);
                case 15:
                    return new BaseStats(130, 144, 130);
                case 16:
                    return new BaseStats(80, 94, 90);
                case 17:
                    return new BaseStats(126, 126, 122);
                case 18:
                    return new BaseStats(166, 170, 166);
                case 19:
                    return new BaseStats(60, 92, 86);
                case 20:
                    return new BaseStats(110, 146, 150);
                case 21:
                    return new BaseStats(80, 102, 78);
                case 22:
                    return new BaseStats(130, 168, 146);
                case 23:
                    return new BaseStats(70, 112, 112);
                case 24:
                    return new BaseStats(120, 166, 166);
                case 25:
                    return new BaseStats(70, 124, 108);
                case 26:
                    return new BaseStats(120, 200, 154);
                case 27:
                    return new BaseStats(100, 90, 114);
                case 28:
                    return new BaseStats(150, 150, 172);
                case 29:
                    return new BaseStats(110, 100, 104);
                case 30:
                    return new BaseStats(140, 132, 136);
                case 31:
                    return new BaseStats(180, 184, 190);
                case 32:
                    return new BaseStats(92, 110, 94);
                case 33:
                    return new BaseStats(122, 142, 128);
                case 34:
                    return new BaseStats(162, 204, 170);
                case 35:
                    return new BaseStats(140, 116, 124);
                case 36:
                    return new BaseStats(190, 178, 178);
                case 37:
                    return new BaseStats(76, 106, 118);
                case 38:
                    return new BaseStats(146, 176, 194);
                case 39:
                    return new BaseStats(230, 98, 54);
                case 40:
                    return new BaseStats(280, 168, 108);
                case 41:
                    return new BaseStats(80, 88, 90);
                case 42:
                    return new BaseStats(150, 164, 164);
                case 43:
                    return new BaseStats(90, 134, 130);
                case 44:
                    return new BaseStats(120, 162, 158);
                case 45:
                    return new BaseStats(150, 202, 190);
                case 46:
                    return new BaseStats(70, 122, 120);
                case 47:
                    return new BaseStats(120, 162, 170);
                case 48:
                    return new BaseStats(120, 108, 118);
                case 49:
                    return new BaseStats(140, 172, 154);
                case 50:
                    return new BaseStats(20, 108, 86);
                case 51:
                    return new BaseStats(70, 148, 140);
                case 52:
                    return new BaseStats(80, 104, 94);
                case 53:
                    return new BaseStats(130, 156, 146);
                case 54:
                    return new BaseStats(100, 132, 112);
                case 55:
                    return new BaseStats(160, 194, 176);
                case 56:
                    return new BaseStats(80, 122, 96);
                case 57:
                    return new BaseStats(130, 178, 150);
                case 58:
                    return new BaseStats(110, 156, 110);
                case 59:
                    return new BaseStats(180, 230, 180);
                case 60:
                    return new BaseStats(80, 108, 98);
                case 61:
                    return new BaseStats(130, 132, 132);
                case 62:
                    return new BaseStats(180, 180, 202);
                case 63:
                    return new BaseStats(50, 110, 76);
                case 64:
                    return new BaseStats(80, 150, 112);
                case 65:
                    return new BaseStats(110, 186, 152);
                case 66:
                    return new BaseStats(140, 118, 96);
                case 67:
                    return new BaseStats(160, 154, 144);
                case 68:
                    return new BaseStats(180, 198, 180);
                case 69:
                    return new BaseStats(100, 158, 78);
                case 70:
                    return new BaseStats(130, 190, 110);
                case 71:
                    return new BaseStats(160, 222, 152);
                case 72:
                    return new BaseStats(80, 106, 136);
                case 73:
                    return new BaseStats(160, 170, 196);
                case 74:
                    return new BaseStats(80, 106, 118);
                case 75:
                    return new BaseStats(110, 142, 156);
                case 76:
                    return new BaseStats(160, 176, 198);
                case 77:
                    return new BaseStats(100, 168, 138);
                case 78:
                    return new BaseStats(130, 200, 170);
                case 79:
                    return new BaseStats(180, 110, 110);
                case 80:
                    return new BaseStats(190, 184, 198);
                case 81:
                    return new BaseStats(50, 128, 138);
                case 82:
                    return new BaseStats(100, 186, 180);
                case 83:
                    return new BaseStats(104, 138, 132);
                case 84:
                    return new BaseStats(70, 126, 96);
                case 85:
                    return new BaseStats(120, 182, 150);
                case 86:
                    return new BaseStats(130, 104, 138);
                case 87:
                    return new BaseStats(180, 156, 192);
                case 88:
                    return new BaseStats(160, 124, 110);
                case 89:
                    return new BaseStats(210, 180, 188);
                case 90:
                    return new BaseStats(60, 120, 112);
                case 91:
                    return new BaseStats(100, 196, 196);
                case 92:
                    return new BaseStats(60, 136, 82);
                case 93:
                    return new BaseStats(90, 172, 118);
                case 94:
                    return new BaseStats(120, 204, 156);
                case 95:
                    return new BaseStats(70, 90, 186);
                case 96:
                    return new BaseStats(120, 104, 140);
                case 97:
                    return new BaseStats(170, 162, 196);
                case 98:
                    return new BaseStats(60, 116, 110);
                case 99:
                    return new BaseStats(110, 178, 168);
                case 100:
                    return new BaseStats(80, 102, 124);
                case 101:
                    return new BaseStats(120, 150, 174);
                case 102:
                    return new BaseStats(120, 110, 132);
                case 103:
                    return new BaseStats(190, 232, 164);
                case 104:
                    return new BaseStats(100, 102, 150);
                case 105:
                    return new BaseStats(120, 140, 202);
                case 106:
                    return new BaseStats(100, 148, 172);
                case 107:
                    return new BaseStats(100, 138, 204);
                case 108:
                    return new BaseStats(180, 126, 160);
                case 109:
                    return new BaseStats(80, 136, 142);
                case 110:
                    return new BaseStats(130, 190, 198);
                case 111:
                    return new BaseStats(160, 110, 116);
                case 112:
                    return new BaseStats(210, 166, 160);
                case 113:
                    return new BaseStats(500, 40, 60);
                case 114:
                    return new BaseStats(130, 164, 152);
                case 115:
                    return new BaseStats(210, 142, 178);
                case 116:
                    return new BaseStats(60, 122, 100);
                case 117:
                    return new BaseStats(110, 176, 150);
                case 118:
                    return new BaseStats(90, 112, 126);
                case 119:
                    return new BaseStats(160, 172, 160);
                case 120:
                    return new BaseStats(60, 130, 128);
                case 121:
                    return new BaseStats(120, 194, 192);
                case 122:
                    return new BaseStats(80, 154, 196);
                case 123:
                    return new BaseStats(140, 176, 180);
                case 124:
                    return new BaseStats(130, 172, 134);
                case 125:
                    return new BaseStats(130, 198, 160);
                case 126:
                    return new BaseStats(130, 214, 158);
                case 127:
                    return new BaseStats(130, 184, 186);
                case 128:
                    return new BaseStats(150, 148, 184);
                case 129:
                    return new BaseStats(40, 42, 84);
                case 130:
                    return new BaseStats(190, 192, 196);
                case 131:
                    return new BaseStats(260, 186, 190);
                case 132:
                    return new BaseStats(96, 110, 110);
                case 133:
                    return new BaseStats(110, 114, 128);
                case 134:
                    return new BaseStats(260, 186, 168);
                case 135:
                    return new BaseStats(130, 192, 174);
                case 136:
                    return new BaseStats(130, 238, 178);
                case 137:
                    return new BaseStats(130, 156, 158);
                case 138:
                    return new BaseStats(70, 132, 160);
                case 139:
                    return new BaseStats(140, 180, 202);
                case 140:
                    return new BaseStats(60, 148, 142);
                case 141:
                    return new BaseStats(120, 190, 190);
                case 142:
                    return new BaseStats(160, 182, 162);
                case 143:
                    return new BaseStats(320, 180, 180);
                case 144:
                    return new BaseStats(180, 198, 242);
                case 145:
                    return new BaseStats(180, 232, 194);
                case 146:
                    return new BaseStats(180, 242, 194);
                case 147:
                    return new BaseStats(82, 128, 110);
                case 148:
                    return new BaseStats(122, 170, 152);
                case 149:
                    return new BaseStats(182, 250, 212);
                case 150:
                    return new BaseStats(212, 284, 202);
                case 151:
                    return new BaseStats(200, 220, 220);
                default:
                    return new BaseStats();
            }
        }

        public static double GetLevel(PokemonData poke)
        {
            switch ((int) ((poke.CpMultiplier + poke.AdditionalCpMultiplier)*1000.0))
            {
                case 93: // 0.094 * 1000 = 93.99999678134
                case 94:
                    return 1;
                case 135:
                    return 1.5;
                case 166:
                    return 2;
                case 192:
                    return 2.5;
                case 215:
                    return 3;
                case 236:
                    return 3.5;
                case 255:
                    return 4;
                case 273:
                    return 4.5;
                case 290:
                    return 5;
                case 306:
                    return 5.5;
                case 321:
                    return 6;
                case 335:
                    return 6.5;
                case 349:
                    return 7;
                case 362:
                    return 7.5;
                case 375:
                    return 8;
                case 387:
                    return 8.5;
                case 399:
                    return 9;
                case 411:
                    return 9.5;
                case 422:
                    return 10;
                case 432:
                    return 15;
                case 443:
                    return 11;
                case 453:
                    return 11.5;
                case 462:
                    return 12;
                case 472:
                    return 12.5;
                case 481:
                    return 13;
                case 490:
                    return 13.5;
                case 499:
                    return 14;
                case 508:
                    return 14.5;
                case 517:
                    return 15;
                case 525:
                    return 15.5;
                case 534:
                    return 16;
                case 542:
                    return 16.5;
                case 550:
                    return 17;
                case 558:
                    return 17.5;
                case 566:
                    return 18;
                case 574:
                    return 18.5;
                case 582:
                    return 19;
                case 589:
                    return 19.5;
                case 597:
                    return 20;
                case 604:
                    return 25;
                case 612:
                    return 21;
                case 619:
                    return 21.5;
                case 626:
                    return 22;
                case 633:
                    return 22.5;
                case 640:
                    return 23;
                case 647:
                    return 23.5;
                case 654:
                    return 24;
                case 661:
                    return 24.5;
                case 667:
                    return 25;
                case 674:
                    return 25.5;
                case 681:
                    return 26;
                case 687:
                    return 26.5;
                case 694:
                    return 27;
                case 700:
                    return 27.5;
                case 706:
                    return 28;
                case 713:
                    return 28.5;
                case 719:
                    return 29;
                case 725:
                    return 29.5;
                case 731:
                    return 30;
                case 734:
                    return 35;
                case 737:
                    return 31;
                case 740:
                    return 31.5;
                case 743:
                    return 32;
                case 746:
                    return 32.5;
                case 749:
                    return 33;
                case 752:
                    return 33.5;
                case 755:
                    return 34;
                case 758:
                    return 34.5;
                case 761:
                    return 35;
                case 764:
                    return 35.5;
                case 767:
                    return 36;
                case 770:
                    return 36.5;
                case 773:
                    return 37;
                case 776:
                    return 37.5;
                case 778:
                    return 38;
                case 781:
                    return 38.5;
                case 784:
                    return 39;
                case 787:
                    return 39.5;
                case 790:
                    return 40;
                default:
                    return 0;
            }
        }

        public static PokemonMove GetPokemonMove1(PokemonData poke)
        {
            var move1 = poke.Move1;
            return move1;
        }

        public static PokemonMove GetPokemonMove2(PokemonData poke)
        {
            var move2 = poke.Move2;
            return move2;
        }

        public static int GetPowerUpLevel(PokemonData poke)
        {
            return (int) (GetLevel(poke)*2.0);
        }
		
		public static double CalculateDPSMove1(PokemonData poke)
        {


            switch (poke.Move1)
            {
                case PokemonMove.ThunderShock: return 8.33;
                case PokemonMove.QuickAttack: return 7.52;
                case PokemonMove.Scratch: return 12.00;
                case PokemonMove.Ember: return 9.52;
                case PokemonMove.VineWhip: return 10.77;
                case PokemonMove.Tackle: return 10.91;
                case PokemonMove.RazorLeaf: return 10.34;
                case PokemonMove.TakeDown: return 666;
                case PokemonMove.WaterGun: return 12;
                case PokemonMove.Bite: return 12;
                case PokemonMove.Pound: return 12.96;

                case PokemonMove.DoubleSlap: return 666;
                case PokemonMove.Wrap: return 6.25;
                case PokemonMove.HyperBeam: return 24;
                case PokemonMove.Lick: return 10;
                case PokemonMove.DarkPulse: return 12.86;
                case PokemonMove.Smog: return 666;
                case PokemonMove.Sludge: return 11.54;
                case PokemonMove.MetalClaw: return 12.7;
                case PokemonMove.ViceGrip: return 11.9;
                case PokemonMove.FlameWheel: return 8.7;
                case PokemonMove.Megahorn: return 25;
                case PokemonMove.WingAttack: return 12;
                case PokemonMove.Flamethrower:
                    return 18.97;
                case PokemonMove.SuckerPunch:
                    return 10;
                case PokemonMove.Dig:
                    return 12.07;
                case PokemonMove.LowKick:
                    return 8.33;
                case PokemonMove.CrossChop:
                    return 30;
                case PokemonMove.PsychoCut:
                    return 12.28;
                case PokemonMove.Psybeam:
                    return 10.53;
                case PokemonMove.Earthquake:
                    return 23.81;
                case PokemonMove.StoneEdge:
                    return 25.81;
                case PokemonMove.IcePunch:
                    return 12.86;
                case PokemonMove.HeartStamp:
                    return 9.8;
                case PokemonMove.Discharge:
                    return 14;
                case PokemonMove.FlashCannon:
                    return 15.38;
                case PokemonMove.Peck:
                    return 8.7;
                case PokemonMove.DrillPeck:
                    return 14.81;
                case PokemonMove.IceBeam:
                    return 17.81;
                case PokemonMove.Blizzard:
                    return 25.64;
                case PokemonMove.AirSlash:
                    return 666;
                case PokemonMove.HeatWave:
                    return 21.05;
                case PokemonMove.Twineedle:
                    return 666;
                case PokemonMove.PoisonJab:
                    return 11.43;
                case PokemonMove.AerialAce:
                    return 10.34;
                case PokemonMove.DrillRun:
                    return 14.71;
                case PokemonMove.PetalBlizzard:
                    return 20.31;
                case PokemonMove.MegaDrain:
                    return 7.81;
                case PokemonMove.BugBuzz:
                    return 17.65;
                case PokemonMove.PoisonFang:
                    return 10.42;
                case PokemonMove.NightSlash:
                    return 11.11;
                case PokemonMove.Slash:
                    return 666;
                case PokemonMove.BubbleBeam:
                    return 10.34;
                case PokemonMove.Submission:
                    return 14.29;
                case PokemonMove.KarateChop:
                    return 7.5;
                case PokemonMove.LowSweep:
                    return 13.33;
                case PokemonMove.AquaJet:
                    return 10.64;
                case PokemonMove.AquaTail:
                    return 19.15;
                case PokemonMove.SeedBomb:
                    return 16.67;
                case PokemonMove.Psyshock:
                    return 14.81;
                case PokemonMove.RockThrow:
                    return 8.82;
                case PokemonMove.AncientPower:
                    return 9.72;
                case PokemonMove.RockTomb:
                    return 8.82;
                case PokemonMove.RockSlide:
                    return 15.63;
                case PokemonMove.PowerGem:
                    return 13.79;
                case PokemonMove.ShadowSneak:
                    return 8.06;
                case PokemonMove.ShadowPunch:
                    return 11.9;
                case PokemonMove.ShadowClaw:
                    return 11.58;
                case PokemonMove.OminousWind:
                    return 9.68;
                case PokemonMove.ShadowBall:
                    return 14.61;
                case PokemonMove.BulletPunch:
                    return 8.33;
                case PokemonMove.MagnetBomb:
                    return 10.71;
                case PokemonMove.SteelWing:
                    return 11.28;
                case PokemonMove.IronHead:
                    return 15;
                case PokemonMove.ParabolicCharge:
                    return 11.9;
                case PokemonMove.Spark:
                    return 10;
                case PokemonMove.ThunderPunch:
                    return 16.67;
                case PokemonMove.Thunder:
                    return 23.26;
                case PokemonMove.Thunderbolt:
                    return 20.37;
                case PokemonMove.Twister:
                    return 9.26;
                case PokemonMove.DragonBreath:
                    return 12;
                case PokemonMove.DragonPulse:
                    return 18.06;
                case PokemonMove.DragonClaw:
                    return 23.33;
                case PokemonMove.DisarmingVoice:
                    return 6.41;
                case PokemonMove.DrainingKiss:
                    return 8.93;
                case PokemonMove.DazzlingGleam:
                    return 13.1;
                case PokemonMove.Moonblast:
                    return 20.73;
                case PokemonMove.PlayRough:
                    return 18.97;
                case PokemonMove.CrossPoison:
                    return 16.67;
                case PokemonMove.SludgeBomb:
                    return 21.15;
                case PokemonMove.SludgeWave:
                    return 20.59;
                case PokemonMove.GunkShot:
                    return 21.67;
                case PokemonMove.MudShot:
                    return 10.91;
                case PokemonMove.BoneClub:
                    return 15.63;
                case PokemonMove.Bulldoze:
                    return 10.29;
                case PokemonMove.MudBomb:
                    return 11.54;
                case PokemonMove.FuryCutter:
                    return 7.5;
                case PokemonMove.BugBite:
                    return 11.11;
                case PokemonMove.SignalBeam:
                    return 14.52;
                case PokemonMove.XScissor:
                    return 16.67;
                case PokemonMove.FlameCharge:
                    return 8.06;
                case PokemonMove.FlameBurst:
                    return 14.29;
                case PokemonMove.FireBlast:
                    return 24.39;
                case PokemonMove.Brine:
                    return 10.42;
                case PokemonMove.WaterPulse:
                    return 10.61;
                case PokemonMove.Scald:
                    return 13.75;
                case PokemonMove.HydroPump:
                    return 23.68;
                case PokemonMove.Psychic:
                    return 19.64;
                case PokemonMove.Psystrike:
                    return 19.61;
                case PokemonMove.IceShard:
                    return 10.71;
                case PokemonMove.IcyWind:
                    return 6.58;
                case PokemonMove.FrostBreath:
                    return 11.11;
                case PokemonMove.Absorb:
                    return 666;
                case PokemonMove.GigaDrain:
                    return 13.89;
                case PokemonMove.FirePunch:
                    return 14.29;
                case PokemonMove.SolarBeam:
                    return 24.49;
                case PokemonMove.LeafBlade:
                    return 19.64;
                case PokemonMove.PowerWhip:
                    return 25;
                case PokemonMove.Splash:
                    return 0;
                case PokemonMove.Acid:
                    return 9.52;
                case PokemonMove.AirCutter:
                    return 9.09;
                case PokemonMove.Hurricane:
                    return 25;
                case PokemonMove.BrickBreak:
                    return 18.75;
                case PokemonMove.Cut:
                    return 10.62;
                case PokemonMove.Swift:
                    return 10;
                case PokemonMove.HornAttack:
                    return 11.36;
                case PokemonMove.Stomp:
                    return 14.29;
                case PokemonMove.Headbutt:
                    return 11.43;
                case PokemonMove.HyperFang:
                    return 16.67;
                case PokemonMove.Slam:
                    return 666;
                case PokemonMove.BodySlam:
                    return 25.64;
                case PokemonMove.Rest:
                    return 16.13;
                case PokemonMove.Struggle:
                    return 8.85;
                case PokemonMove.ScaldBlastoise:
                    return 12.5;
                case PokemonMove.HydroPumpBlastoise:
                    return 23.68;
                case PokemonMove.WrapGreen:
                    return 6.76;
                case PokemonMove.WrapPink:
                    return 6.76;
                case PokemonMove.FuryCutterFast:
                    return 7.5;
                case PokemonMove.BugBiteFast:
                    return 11.11;
                case PokemonMove.BiteFast:
                    return 12;
                case PokemonMove.SuckerPunchFast:
                    return 10;
                case PokemonMove.DragonBreathFast:
                    return 12;
                case PokemonMove.ThunderShockFast:
                    return 8.33;
                case PokemonMove.SparkFast:
                    return 10;
                case PokemonMove.LowKickFast:
                    return 8.33;
                case PokemonMove.KarateChopFast:
                    return 7.5;
                case PokemonMove.EmberFast:
                    return 9.52;
                case PokemonMove.WingAttackFast:
                    return 12;
                case PokemonMove.PeckFast:
                    return 8.7;
                case PokemonMove.LickFast:
                    return 10;
                case PokemonMove.ShadowClawFast:
                    return 11.58;
                case PokemonMove.VineWhipFast:
                    return 10.77;
                case PokemonMove.RazorLeafFast:
                    return 10.34;
                case PokemonMove.MudShotFast:
                    return 10.91;
                case PokemonMove.IceShardFast:
                    return 10.71;
                case PokemonMove.FrostBreathFast:
                    return 11.11;
                case PokemonMove.QuickAttackFast:
                    return 7.52;
                case PokemonMove.ScratchFast:
                    return 12;
                case PokemonMove.TackleFast:
                    return 10.91;
                case PokemonMove.PoundFast:
                    return 12.96;
                case PokemonMove.CutFast:
                    return 10.62;
                case PokemonMove.PoisonJabFast:
                    return 11.43;
                case PokemonMove.AcidFast:
                    return 9.52;
                case PokemonMove.PsychoCutFast:
                    return 12.28;
                case PokemonMove.RockThrowFast:
                    return 8.82;
                case PokemonMove.MetalClawFast:
                    return 12.7;
                case PokemonMove.BulletPunchFast:
                    return 8.33;
                case PokemonMove.WaterGunFast:
                    return 12;
                case PokemonMove.SplashFast:
                    return 0;
                case PokemonMove.WaterGunFastBlastoise:
                    return 12;
                case PokemonMove.MudSlapFast:
                    return 11.11;
                case PokemonMove.ZenHeadbuttFast:
                    return 11.43;
                case PokemonMove.ConfusionFast:
                    return 9.93;
                case PokemonMove.PoisonStingFast:
                    return 10.43;
                case PokemonMove.BubbleFast:
                    return 10.87;
                case PokemonMove.FeintAttackFast:
                    return 11.54;
                case PokemonMove.SteelWingFast:
                    return 11.28;
                case PokemonMove.FireFangFast:
                    return 11.9;
                case PokemonMove.RockSmashFast:
                    return 10.64;
                default: return 666;
            }
        }

        public static double CalculateDPSMove2(PokemonData poke)
        {


            switch (poke.Move1)
            {
                case PokemonMove.ThunderShock: return 8.33;
                case PokemonMove.QuickAttack: return 7.52;
                case PokemonMove.Scratch: return 12.00;
                case PokemonMove.Ember: return 9.52;
                case PokemonMove.VineWhip: return 10.77;
                case PokemonMove.Tackle: return 10.91;
                case PokemonMove.RazorLeaf: return 10.34;
                case PokemonMove.TakeDown: return 666;
                case PokemonMove.WaterGun: return 12;
                case PokemonMove.Bite: return 12;
                case PokemonMove.Pound: return 12.96;

                case PokemonMove.DoubleSlap: return 666;
                case PokemonMove.Wrap: return 6.25;
                case PokemonMove.HyperBeam: return 24;
                case PokemonMove.Lick: return 10;
                case PokemonMove.DarkPulse: return 12.86;
                case PokemonMove.Smog: return 666;
                case PokemonMove.Sludge: return 11.54;
                case PokemonMove.MetalClaw: return 12.7;
                case PokemonMove.ViceGrip: return 11.9;
                case PokemonMove.FlameWheel: return 8.7;
                case PokemonMove.Megahorn: return 25;
                case PokemonMove.WingAttack: return 12;
                case PokemonMove.Flamethrower:
                    return 18.97;
                case PokemonMove.SuckerPunch:
                    return 10;
                case PokemonMove.Dig:
                    return 12.07;
                case PokemonMove.LowKick:
                    return 8.33;
                case PokemonMove.CrossChop:
                    return 30;
                case PokemonMove.PsychoCut:
                    return 12.28;
                case PokemonMove.Psybeam:
                    return 10.53;
                case PokemonMove.Earthquake:
                    return 23.81;
                case PokemonMove.StoneEdge:
                    return 25.81;
                case PokemonMove.IcePunch:
                    return 12.86;
                case PokemonMove.HeartStamp:
                    return 9.8;
                case PokemonMove.Discharge:
                    return 14;
                case PokemonMove.FlashCannon:
                    return 15.38;
                case PokemonMove.Peck:
                    return 8.7;
                case PokemonMove.DrillPeck:
                    return 14.81;
                case PokemonMove.IceBeam:
                    return 17.81;
                case PokemonMove.Blizzard:
                    return 25.64;
                case PokemonMove.AirSlash:
                    return 666;
                case PokemonMove.HeatWave:
                    return 21.05;
                case PokemonMove.Twineedle:
                    return 666;
                case PokemonMove.PoisonJab:
                    return 11.43;
                case PokemonMove.AerialAce:
                    return 10.34;
                case PokemonMove.DrillRun:
                    return 14.71;
                case PokemonMove.PetalBlizzard:
                    return 20.31;
                case PokemonMove.MegaDrain:
                    return 7.81;
                case PokemonMove.BugBuzz:
                    return 17.65;
                case PokemonMove.PoisonFang:
                    return 10.42;
                case PokemonMove.NightSlash:
                    return 11.11;
                case PokemonMove.Slash:
                    return 666;
                case PokemonMove.BubbleBeam:
                    return 10.34;
                case PokemonMove.Submission:
                    return 14.29;
                case PokemonMove.KarateChop:
                    return 7.5;
                case PokemonMove.LowSweep:
                    return 13.33;
                case PokemonMove.AquaJet:
                    return 10.64;
                case PokemonMove.AquaTail:
                    return 19.15;
                case PokemonMove.SeedBomb:
                    return 16.67;
                case PokemonMove.Psyshock:
                    return 14.81;
                case PokemonMove.RockThrow:
                    return 8.82;
                case PokemonMove.AncientPower:
                    return 9.72;
                case PokemonMove.RockTomb:
                    return 8.82;
                case PokemonMove.RockSlide:
                    return 15.63;
                case PokemonMove.PowerGem:
                    return 13.79;
                case PokemonMove.ShadowSneak:
                    return 8.06;
                case PokemonMove.ShadowPunch:
                    return 11.9;
                case PokemonMove.ShadowClaw:
                    return 11.58;
                case PokemonMove.OminousWind:
                    return 9.68;
                case PokemonMove.ShadowBall:
                    return 14.61;
                case PokemonMove.BulletPunch:
                    return 8.33;
                case PokemonMove.MagnetBomb:
                    return 10.71;
                case PokemonMove.SteelWing:
                    return 11.28;
                case PokemonMove.IronHead:
                    return 15;
                case PokemonMove.ParabolicCharge:
                    return 11.9;
                case PokemonMove.Spark:
                    return 10;
                case PokemonMove.ThunderPunch:
                    return 16.67;
                case PokemonMove.Thunder:
                    return 23.26;
                case PokemonMove.Thunderbolt:
                    return 20.37;
                case PokemonMove.Twister:
                    return 9.26;
                case PokemonMove.DragonBreath:
                    return 12;
                case PokemonMove.DragonPulse:
                    return 18.06;
                case PokemonMove.DragonClaw:
                    return 23.33;
                case PokemonMove.DisarmingVoice:
                    return 6.41;
                case PokemonMove.DrainingKiss:
                    return 8.93;
                case PokemonMove.DazzlingGleam:
                    return 13.1;
                case PokemonMove.Moonblast:
                    return 20.73;
                case PokemonMove.PlayRough:
                    return 18.97;
                case PokemonMove.CrossPoison:
                    return 16.67;
                case PokemonMove.SludgeBomb:
                    return 21.15;
                case PokemonMove.SludgeWave:
                    return 20.59;
                case PokemonMove.GunkShot:
                    return 21.67;
                case PokemonMove.MudShot:
                    return 10.91;
                case PokemonMove.BoneClub:
                    return 15.63;
                case PokemonMove.Bulldoze:
                    return 10.29;
                case PokemonMove.MudBomb:
                    return 11.54;
                case PokemonMove.FuryCutter:
                    return 7.5;
                case PokemonMove.BugBite:
                    return 11.11;
                case PokemonMove.SignalBeam:
                    return 14.52;
                case PokemonMove.XScissor:
                    return 16.67;
                case PokemonMove.FlameCharge:
                    return 8.06;
                case PokemonMove.FlameBurst:
                    return 14.29;
                case PokemonMove.FireBlast:
                    return 24.39;
                case PokemonMove.Brine:
                    return 10.42;
                case PokemonMove.WaterPulse:
                    return 10.61;
                case PokemonMove.Scald:
                    return 13.75;
                case PokemonMove.HydroPump:
                    return 23.68;
                case PokemonMove.Psychic:
                    return 19.64;
                case PokemonMove.Psystrike:
                    return 19.61;
                case PokemonMove.IceShard:
                    return 10.71;
                case PokemonMove.IcyWind:
                    return 6.58;
                case PokemonMove.FrostBreath:
                    return 11.11;
                case PokemonMove.Absorb:
                    return 666;
                case PokemonMove.GigaDrain:
                    return 13.89;
                case PokemonMove.FirePunch:
                    return 14.29;
                case PokemonMove.SolarBeam:
                    return 24.49;
                case PokemonMove.LeafBlade:
                    return 19.64;
                case PokemonMove.PowerWhip:
                    return 25;
                case PokemonMove.Splash:
                    return 0;
                case PokemonMove.Acid:
                    return 9.52;
                case PokemonMove.AirCutter:
                    return 9.09;
                case PokemonMove.Hurricane:
                    return 25;
                case PokemonMove.BrickBreak:
                    return 18.75;
                case PokemonMove.Cut:
                    return 10.62;
                case PokemonMove.Swift:
                    return 10;
                case PokemonMove.HornAttack:
                    return 11.36;
                case PokemonMove.Stomp:
                    return 14.29;
                case PokemonMove.Headbutt:
                    return 11.43;
                case PokemonMove.HyperFang:
                    return 16.67;
                case PokemonMove.Slam:
                    return 666;
                case PokemonMove.BodySlam:
                    return 25.64;
                case PokemonMove.Rest:
                    return 16.13;
                case PokemonMove.Struggle:
                    return 8.85;
                case PokemonMove.ScaldBlastoise:
                    return 12.5;
                case PokemonMove.HydroPumpBlastoise:
                    return 23.68;
                case PokemonMove.WrapGreen:
                    return 6.76;
                case PokemonMove.WrapPink:
                    return 6.76;
                case PokemonMove.FuryCutterFast:
                    return 7.5;
                case PokemonMove.BugBiteFast:
                    return 11.11;
                case PokemonMove.BiteFast:
                    return 12;
                case PokemonMove.SuckerPunchFast:
                    return 10;
                case PokemonMove.DragonBreathFast:
                    return 12;
                case PokemonMove.ThunderShockFast:
                    return 8.33;
                case PokemonMove.SparkFast:
                    return 10;
                case PokemonMove.LowKickFast:
                    return 8.33;
                case PokemonMove.KarateChopFast:
                    return 7.5;
                case PokemonMove.EmberFast:
                    return 9.52;
                case PokemonMove.WingAttackFast:
                    return 12;
                case PokemonMove.PeckFast:
                    return 8.7;
                case PokemonMove.LickFast:
                    return 10;
                case PokemonMove.ShadowClawFast:
                    return 11.58;
                case PokemonMove.VineWhipFast:
                    return 10.77;
                case PokemonMove.RazorLeafFast:
                    return 10.34;
                case PokemonMove.MudShotFast:
                    return 10.91;
                case PokemonMove.IceShardFast:
                    return 10.71;
                case PokemonMove.FrostBreathFast:
                    return 11.11;
                case PokemonMove.QuickAttackFast:
                    return 7.52;
                case PokemonMove.ScratchFast:
                    return 12;
                case PokemonMove.TackleFast:
                    return 10.91;
                case PokemonMove.PoundFast:
                    return 12.96;
                case PokemonMove.CutFast:
                    return 10.62;
                case PokemonMove.PoisonJabFast:
                    return 11.43;
                case PokemonMove.AcidFast:
                    return 9.52;
                case PokemonMove.PsychoCutFast:
                    return 12.28;
                case PokemonMove.RockThrowFast:
                    return 8.82;
                case PokemonMove.MetalClawFast:
                    return 12.7;
                case PokemonMove.BulletPunchFast:
                    return 8.33;
                case PokemonMove.WaterGunFast:
                    return 12;
                case PokemonMove.SplashFast:
                    return 0;
                case PokemonMove.WaterGunFastBlastoise:
                    return 12;
                case PokemonMove.MudSlapFast:
                    return 11.11;
                case PokemonMove.ZenHeadbuttFast:
                    return 11.43;
                case PokemonMove.ConfusionFast:
                    return 9.93;
                case PokemonMove.PoisonStingFast:
                    return 10.43;
                case PokemonMove.BubbleFast:
                    return 10.87;
                case PokemonMove.FeintAttackFast:
                    return 11.54;
                case PokemonMove.SteelWingFast:
                    return 11.28;
                case PokemonMove.FireFangFast:
                    return 11.9;
                case PokemonMove.RockSmashFast:
                    return 10.64;
                default: return 666;
            }
        }
 
    }
}