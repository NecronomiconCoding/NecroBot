using System.Collections.Generic;

namespace PoGo.NecroBot.Logic.Utils
{
    public static class DeviceInfoHelper
    {
        public static Dictionary<string, Dictionary<string, string>> DeviceInfoSets = new Dictionary<string, Dictionary<string, string>>() {
            { "lg-optimus-g",
                new Dictionary<string,string>()
                {
                    { "AndroidBoardName", "geehrc" },
                    { "AndroidBootloader", "MAKOZ10f" },
                    { "DeviceBrand", "LGE" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "LG-LS970" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "cm_ls970" },
                    { "FirmwareBrand", "cm_ls970" },
                    { "FirmwareFingerprint", "google/occam/mako:4.2.2/JDQ39/573038:user/release-keys" },
                    { "FirmwareTags", "test-keys" },
                    { "FirmwareType", "userdebug" },
                    { "HardwareManufacturer", "LGE" },
                    { "HardwareModel", "LG-LS970" }
                }
            },
            { "nexus7gen2",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "flo" },
                    { "AndroidBootloader", "FLO-04.07" },
                    { "DeviceBrand", "google" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "Nexus 7" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "razor" },
                    { "FirmwareBrand", "razor" },
                    { "FirmwareFingerprint", "google/razor/flo:6.0.1/MOB30P/2960889:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "asus" },
                    { "HardwareModel", "Nexus 7" }
                }
            },
            { "nexus7gen1",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "grouper" },
                    { "AndroidBootloader", "4.23" },
                    { "DeviceBrand", "google" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "Nexus 7" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "nakasi" },
                    { "FirmwareBrand", "nakasi" },
                    { "FirmwareFingerprint", "google/nakasi/grouper:5.1.1/LMY47V/1836172:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "asus" },
                    { "HardwareModel", "Nexus 7" }
                }
            },
            { "htc10",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "msm8996" },
                    { "AndroidBootloader", "1.0.0.0000" },
                    { "DeviceBrand", "HTC" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "HTC 10" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "pmewl_00531" },
                    { "FirmwareBrand", "pmewl_00531" },
                    { "FirmwareFingerprint", "htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "HTC" },
                    { "HardwareModel", "HTC 10" }
                }
            },
            { "galaxy6",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "universal7420" },
                    { "AndroidBootloader", "G920FXXU3DPEK" },
                    { "DeviceBrand", "samsung" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "zeroflte" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "SM-G920F" },
                    { "FirmwareBrand", "zerofltexx" },
                    { "FirmwareFingerprint", "samsung/zerofltexx/zeroflte:6.0.1/MMB29K/G920FXXU3DPEK:user/release-keys" },
                    { "FirmwareTags", "dev-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "samsung" },
                    { "HardwareModel", "samsungexynos7420" }
                }
            },
            { "galaxy-s5-gold",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "MSM8974" },
                    { "AndroidBootloader", "G900FXXU1CPEH" },
                    { "DeviceBrand", "samsung" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "SM-G900F" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "kltexx" },
                    { "FirmwareBrand", "kltexx" },
                    { "FirmwareFingerprint", "samsung/kltexx/klte:6.0.1/MMB29M/G900FXXU1CPEH:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "samsung" },
                    { "HardwareModel", "SM-G900F" }
                }
            },
            { "lg-optimus-f6",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "f6t" },
                    { "AndroidBootloader", "1.0.0.0000" },
                    { "DeviceBrand", "lge" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "LG-D500" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "f6_tmo_us" },
                    { "FirmwareBrand", "f6_tmo_us" },
                    { "FirmwareFingerprint", "lge/f6_tmo_us/f6:4.1.2/JZO54K/D50010h.1384764249:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "LGE" },
                    { "HardwareModel", "LG-D500" }
                }
            },
            { "nexus-5x",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "bullhead" },
                    { "AndroidBootloader", "BHZ10k" },
                    { "DeviceBrand", "google" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "Nexus 5X" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "bullhead" },
                    { "FirmwareBrand", "bullhead" },
                    { "FirmwareFingerprint", "google/bullhead/bullhead:6.0.1/MTC19T/2741993:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "LGE" },
                    { "HardwareModel", "Nexus 5X" }
                }
            },
            { "galaxy-s7-edge",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "msm8996" },
                    { "AndroidBootloader", "G935TUVU3APG1" },
                    { "DeviceBrand", "samsung" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "SM-G935T" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "hero2qltetmo" },
                    { "FirmwareBrand", "hero2qltetmo" },
                    { "FirmwareFingerprint", "samsung/hero2qltetmo/hero2qltetmo:6.0.1/MMB29M/G935TUVU3APG1:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "samsung" },
                    { "HardwareModel", "SM-G935T" }
                }
            },
            { "asus-zenfone2",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "moorefield" },
                    { "AndroidBootloader", "" },
                    { "DeviceBrand", "asus" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "ASUS_Z00AD" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "WW_Z00A" },
                    { "FirmwareBrand", "WW_Z00A" },
                    { "FirmwareFingerprint", "asus/WW_Z00A/Z00A_1:5.0/LRX21V/2.20.40.194_20160713_6971_user:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "asus" },
                    { "HardwareModel", "ASUS_Z00AD" }
                }
            },
            { "xperia-z5",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "msm8994" },
                    { "AndroidBootloader", "s1" },
                    { "DeviceBrand", "Sony" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "E6653" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "E6653" },
                    { "FirmwareBrand", "E6653" },
                    { "FirmwareFingerprint", "Sony/E6653/E6653:6.0.1/32.2.A.0.224/456768306:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "Sony" },
                    { "HardwareModel", "E6653" }
                }
            },
            { "galaxy-s4",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "MSM8960" },
                    { "AndroidBootloader", "I337MVLUGOH1" },
                    { "DeviceBrand", "samsung" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "SGH-I337M" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "jfltevl" },
                    { "FirmwareBrand", "jfltevl" },
                    { "FirmwareFingerprint", "samsung/jfltevl/jfltecan:5.0.1/LRX22C/I337MVLUGOH1:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "samsung" },
                    { "HardwareModel", "SGH-I337M" }
                }
            },
            { "nexus-6p",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "angler" },
                    { "AndroidBootloader", "angler-03.52" },
                    { "DeviceBrand", "google" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "Nexus 6P" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "angler" },
                    { "FirmwareBrand", "angler" },
                    { "FirmwareFingerprint", "google/angler/angler:6.0.1/MTC19X/2960136:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "Huawei" },
                    { "HardwareModel", "Nexus 6P" }
                }
            },
            { "sony-z3-compact",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "MSM8974" },
                    { "AndroidBootloader", "s1" },
                    { "DeviceBrand", "docomo" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "SO-02G" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "SO-02G" },
                    { "FirmwareBrand", "SO-02G" },
                    { "FirmwareFingerprint", "docomo/SO-02G/SO-02G:5.0.2/23.1.B.1.317/2161656255:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "Sony" },
                    { "HardwareModel", "SO-02G" }
                }
            },
            { "lg-v10",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "MSM8992" },
                    { "AndroidBootloader", "" },
                    { "DeviceBrand", "LG" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "V10" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "pplus" },
                    { "FirmwareBrand", "pplus" },
                    { "FirmwareFingerprint", "LG/pplus/pplus:5.1.1/LYZ28J/kasp3rd02071120:eng/test-keys" },
                    { "FirmwareTags", "test-keys" },
                    { "FirmwareType", "eng" },
                    { "HardwareManufacturer", "LG" },
                    { "HardwareModel", "V10" }
                }
            },
            { "galaxy-tab3",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "smdk4x12" },
                    { "AndroidBootloader", "T310UEUCOI1" },
                    { "DeviceBrand", "samsung" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "SM-T310" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "lt01wifiue" },
                    { "FirmwareBrand", "lt01wifiue" },
                    { "FirmwareFingerprint", "samsung/lt01wifiue/lt01wifi:4.4.2/KOT49H/T310UEUCOI1:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "samsung" },
                    { "HardwareModel", "SM-T310" }
                }
            },
            { "lg-g4",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "msm8992" },
                    { "AndroidBootloader", "" },
                    { "DeviceBrand", "lge" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "VS986" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "p1_vzw" },
                    { "FirmwareBrand", "p1_vzw" },
                    { "FirmwareFingerprint", "lge/p1_vzw/p1:5.1/LMY47D/151541507ff1b:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "LGE" },
                    { "HardwareModel", "VS986" }
                }
            },
            { "nexus5",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "hammerhead" },
                    { "AndroidBootloader", "HHZ20b" },
                    { "DeviceBrand", "google" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "Nexus 5" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "hammerhead" },
                    { "FirmwareBrand", "hammerhead" },
                    { "FirmwareFingerprint", "google/hammerhead/hammerhead:6.0.1/MOB30M/2862625:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "LGE" },
                    { "HardwareModel", "Nexus 5" }
                }
            },
            { "xoom",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "" },
                    { "AndroidBootloader", "1050" },
                    { "DeviceBrand", "motorola" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "Xoom" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "tervigon" },
                    { "FirmwareBrand", "tervigon" },
                    { "FirmwareFingerprint", "motorola/tervigon/wingray:4.1.2/JZO54K/485486:user/release-keys" },
                    { "FirmwareTags", "test-keys" },
                    { "FirmwareType", "userdebug" },
                    { "HardwareManufacturer", "Motorola" },
                    { "HardwareModel", "Xoom" }
                }
            },
            { "galaxy-note-edge",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "APQ8084" },
                    { "AndroidBootloader", "N915W8VLU1CPE2" },
                    { "DeviceBrand", "samsung" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "SM-N915W8" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "tbltecan" },
                    { "FirmwareBrand", "tbltecan" },
                    { "FirmwareFingerprint", "samsung/tbltecan/tbltecan:6.0.1/MMB29M/N915W8VLU1CPE2:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "samsung" },
                    { "HardwareModel", "SM-N915W8" }
                }
            },
            { "amazon-fire-cm12",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "ford" },
                    { "AndroidBootloader", "" },
                    { "DeviceBrand", "google" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "KFFOWI" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "cm_ford" },
                    { "FirmwareBrand", "cm_ford" },
                    { "FirmwareFingerprint", "google/cm_ford/ford:5.1.1/LMY48Y/ba503d5e70:userdebug/test-keys" },
                    { "FirmwareTags", "test-keys" },
                    { "FirmwareType", "userdebug" },
                    { "HardwareManufacturer", "amzn" },
                    { "HardwareModel", "KFFOWI" }
                }
            },
            { "nexus4-chroma",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "MAKO" },
                    { "AndroidBootloader", "MAKOZ30f" },
                    { "DeviceBrand", "google" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "Nexus 4" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "occam" },
                    { "FirmwareBrand", "occam" },
                    { "FirmwareFingerprint", "google/occam/mako:6.0.1/MOB30Y/3067468:user/release-keys" },
                    { "FirmwareTags", "test-keys" },
                    { "FirmwareType", "userdebug" },
                    { "HardwareManufacturer", "LGE" },
                    { "HardwareModel", "Nexus 4" }
                }
            },
            { "melrose-s9",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "g15" },
                    { "AndroidBootloader", "" },
                    { "DeviceBrand", "alps" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "MELROSE S9" },
                    { "DeviceModelBoot", "mtk" },
                    { "DeviceModelIdentifier", "g15" },
                    { "FirmwareBrand", "g15" },
                    { "FirmwareFingerprint", "alps/g15/g15:4.4.2/KOT49H/:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "alps" },
                    { "HardwareModel", "MELROSE S9" }
                }
            },
            { "yureka",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "MSM8916" },
                    { "AndroidBootloader", "tomato-12-gf7e8024" },
                    { "DeviceBrand", "YU" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "AO5510" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "YUREKA" },
                    { "FirmwareBrand", "YUREKA" },
                    { "FirmwareFingerprint", "YU/YUREKA/YUREKA:5.0.2/LRX22G/YNG1TAS1K0:user/release-keys" },
                    { "FirmwareTags", "test-keys" },
                    { "FirmwareType", "userdebug" },
                    { "HardwareManufacturer", "YU" },
                    { "HardwareModel", "AO5510" }
                }
            },
            { "note3",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "MSM8974" },
                    { "AndroidBootloader", "N900PVPUEOK2" },
                    { "DeviceBrand", "samsung" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "SM-N900P" },
                    { "DeviceModelBoot", "qcom" },
                    { "DeviceModelIdentifier", "cm_hltespr" },
                    { "FirmwareBrand", "cm_hltespr" },
                    { "FirmwareFingerprint", "samsung/hltespr/hltespr:5.0/LRX21V/N900PVPUEOH1:user/release-keys" },
                    { "FirmwareTags", "test-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "samsung" },
                    { "HardwareModel", "SM-N900P" }
                }
            },
            { "galaxy-tab-s84",
                new Dictionary<string, string>()
                {
                    { "AndroidBoardName", "universal5420" },
                    { "AndroidBootloader", "T705XXU1BOL2" },
                    { "DeviceBrand", "samsung" },
                    { "DeviceId", "8525f5d8201f78b5" },
                    { "DeviceModel", "Samsung Galaxy Tab S 8.4 LTE" },
                    { "DeviceModelBoot", "universal5420" },
                    { "DeviceModelIdentifier", "LRX22G.T705XXU1BOL2" },
                    { "FirmwareBrand", "Samsung Galaxy Tab S 8.4 LTE" },
                    { "FirmwareFingerprint", "samsung/klimtltexx/klimtlte:5.0.2/LRX22G/T705XXU1BOL2:user/release-keys" },
                    { "FirmwareTags", "release-keys" },
                    { "FirmwareType", "user" },
                    { "HardwareManufacturer", "samsung" },
                    { "HardwareModel", "SM-T705" }
                }
            },

        };
    }
}
