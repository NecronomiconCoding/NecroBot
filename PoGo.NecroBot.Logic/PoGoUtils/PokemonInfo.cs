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
        public static double CalculateCp(PokemonData poke)
        {
            return
                Math.Max(
                    (int)
                        Math.Floor(0.1*CalculateCpMultiplier(poke)*
                                   Math.Pow(poke.CpMultiplier + poke.AdditionalCpMultiplier, 2)), 10);
        }

        public static double CalculatePokemonBattleRating(PokemonData poke, float battleRatingIVPercentage = 50, int referenceTrainerLevel = 40)
        {
            var maxCPatTrainerLevel = PokemonInfo.GetMaxCpAtTrainerLevel(poke, referenceTrainerLevel);
            return ((poke.Cp / maxCPatTrainerLevel) * (1 - (battleRatingIVPercentage / 100))) + (CalculatePokemonPerfection(poke) * (battleRatingIVPercentage / 100));
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
                return (poke.IndividualAttack*2 + poke.IndividualDefense + poke.IndividualStamina)/(4.0*15.0)*100.0;

            GetBaseStats(poke.PokemonId);
            var maxCp = CalculateMaxCpMultiplier(poke);
            var minCp = CalculateMinCpMultiplier(poke);
            var curCp = CalculateCpMultiplier(poke);

            return (curCp - minCp)/(maxCp - minCp)*100.0;
        }


        public static int CalculateMinHP(PokemonData poke)
        {
            return Math.Max((int)Math.Floor((GetBaseStats(poke.PokemonId).BaseStamina) * CpMultiplier[(int)PokemonInfo.GetLevel(poke) * 2 - 2]), 10);
        }
        public static int CalculateMaxHP(PokemonData poke)
        {
            return Math.Max((int)Math.Floor((GetBaseStats(poke.PokemonId).BaseStamina + 15) * CpMultiplier[(int)PokemonInfo.GetLevel(poke) * 2 - 2]), 10);
        }
        public static double CalculateMinEstTotalStamina(PokemonData poke)
        {
            return (poke.Stamina) / CpMultiplier[(int)PokemonInfo.GetLevel(poke) * 2 - 2];
        }
        public static double CalculateEstTotalStamina(PokemonData poke)
        {
            return (poke.Stamina + 0.5) / CpMultiplier[(int)PokemonInfo.GetLevel(poke) * 2 - 2];
            // est_ind_stamina = CalculateEstTotalStamina(pokemon) - GetBaseStats(pokemon.PokemonId).BaseStamina;
        }
        public static double CalculateMaxEstTotalStamina(PokemonData poke)
        {
            return (poke.Stamina + 1) / CpMultiplier[(int)PokemonInfo.GetLevel(poke) * 2 - 2];
        }

        public static double CalculateDeltaCP(PokemonData poke)
        {
            return (-1 + (poke.Cp + 0.5) / ((GetBaseStats(poke.PokemonId).BaseAttack + 7.5) * Math.Sqrt(GetBaseStats(poke.PokemonId).BaseDefense + 7.5) * Math.Sqrt(GetBaseStats(poke.PokemonId).BaseStamina + 7.5) * Math.Pow(CpMultiplier[(int)PokemonInfo.GetLevel(poke) * 2 - 2], 2) / 10)) * 100;
        }
        public static double CalculateDeltaSta(PokemonData poke)
        {
            return (-1 + (poke.Stamina + 0.5) / ((GetBaseStats(poke.PokemonId).BaseStamina + 7.5) * CpMultiplier[(int)PokemonInfo.GetLevel(poke) * 2 - 2])) * 100;
        }

        public static double CalculateDeltaBR(PokemonData poke)
        {
            return (-1 + (poke.Cp + 0.5) / ((GetBaseStats(poke.PokemonId).BaseAttack + 7.5) * Math.Sqrt(GetBaseStats(poke.PokemonId).BaseDefense + 7.5) * Math.Sqrt(CalculateEstTotalStamina(poke)) * Math.Pow(CpMultiplier[(int)PokemonInfo.GetLevel(poke) * 2 - 2], 2) / 10)) * 100;
        }

        public static double CalculatePokemonRatingCP(PokemonData poke)
        {
            return (poke.Cp - CalculateMinCp(poke)) / (CalculateMaxCp(poke) - CalculateMinCp(poke)) * 100;
        }
        public static double CalculatePokemonRatingSta(PokemonData poke)
        {
            return (poke.Stamina - CalculateMinHP(poke)) / (CalculateMaxHP(poke) - CalculateMinHP(poke)) * 100;
        }
        public static double[] CpMultiplier
        {
            get
            {
                return new double[] { 0.094, 0.1351374, 0.1663979, 0.1926509, 0.2157325, 0.2365727, 0.2557201, 0.2735304, 0.2902499, 0.3060574, 0.3210876, 0.335445, 0.3492127, 0.3624578, 0.3752356, 0.3875924, 0.3995673, 0.4111936, 0.4225, 0.4335117, 0.4431076, 0.45306, 0.4627984, 0.4723361, 0.481685, 0.4908558, 0.4998584, 0.5087018, 0.517394, 0.5259425, 0.5343543, 0.5426358, 0.5507927, 0.5588306, 0.5667545, 0.5745692, 0.5822789, 0.5898879, 0.5974, 0.6048188, 0.6121573, 0.6194041, 0.6265671, 0.6336492, 0.640653, 0.647581, 0.6544356, 0.6612193, 0.667934, 0.6745819, 0.6811649, 0.6876849, 0.6941437, 0.7005429, 0.7068842, 0.7131691, 0.7193991, 0.7255756, 0.7317, 0.734741, 0.7377695, 0.7407856, 0.7437894, 0.7467812, 0.749761, 0.7527291, 0.7556855, 0.7586304, 0.7615638, 0.7644861, 0.7673972, 0.7702973, 0.7731865, 0.776065, 0.7789328, 0.7817901, 0.784637, 0.7874736, 0.7903, 0.7931164 };
            }
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
            var i = 0;
            double cpm = (poke.CpMultiplier + poke.AdditionalCpMultiplier);
            while (cpm > CpMultiplier[i])
            {
                i++;
            }
            var level = 1 + 0.5 * (double)i;
            return level;
        }
        public static double GetMaxCpAtTrainerLevel(PokemonData poke, int trainerLevel)
        {
            double pokemonMaxLevel = (double)(Math.Min(trainerLevel,40)) + 1.5;
            double cpm = CpMultiplier[Math.Min((int)(pokemonMaxLevel * 2 - 2),CpMultiplier.Length-1)];

            var maxAttackNoCpM = (GetBaseStats(poke.PokemonId).BaseAttack + poke.IndividualAttack);
            var maxDefenseNoCpM = (GetBaseStats(poke.PokemonId).BaseDefense + poke.IndividualDefense);
            var maxStaminaNoCpM = (GetBaseStats(poke.PokemonId).BaseStamina + poke.IndividualStamina);

            var maxCPatLevel = maxAttackNoCpM * Math.Sqrt(maxDefenseNoCpM) * Math.Sqrt(maxStaminaNoCpM) * Math.Pow(cpm, 2) / 10;

            return maxCPatLevel;

        }

        public static int GetPowerUpLevel(PokemonData poke)
        {
            return (int) (GetLevel(poke)*2.0);
        }
    }
}