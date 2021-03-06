﻿using NgUtil.Generics.Enums;
using NgUtil.Maths;
using System;
using System.Collections.Generic;

namespace NgHTTP.Browser {
    public sealed class PhoneBrand : ExtendedEnum<PhoneBrand> {

        public object[,] Models { get; }


        private PhoneBrand(string name, object[,] models) : base(name) { }

		public string GetRandomModel(string version) {
			List<string> filters = new List<string>();

			for (int i = 0; i < Models.Length; i++) {
				string modelOsVersion = (string)Models[i, 0];
				string modelName = (string)Models[i, 1];

				if (!modelOsVersion.Equals(version)) {
					continue;
				}
				filters.Add(modelName);
			}
			if (filters.Count == 0) {
				Console.WriteLine("[" + NameIdentifier + "]: No model found for os version: " + version);
				return Samsung.GetRandomModel(version);
			}
			return filters[(MathUtil.Random(filters.Count))];
		}

		public static readonly PhoneBrand Samsung = new PhoneBrand("Samsung", new object[,] {
			{"9", "SAMSUNG SM-G973F Build/PQ2A"},
			{"9", "SAMSUNG SM-A505FN Build/PQ2A"},
			{"9", "SAMSUNG SM-A606F Build/PQ2A"},
			{"9", "SAMSUNG SM-A705F Build/PQ2A"},
			{"9", "SAMSUNG SM-A805F Build/PQ2A"},

			{"8.1.0", "SAMSUNG SM-J710MN Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-N960F Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-N9600 Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-G390F Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-J701M Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-J701MT Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-G610M Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-J530G Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-M205F Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-M305F Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-J260F Build/M1AJQ"},
			{"8.1.0", "SAMSUNG SM-J610FN Build/M1AJQ"},

			{"8.0.0", "SAMSUNG SM-G955U Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G930A Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-J600GT Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G9650 Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A520F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G611MT Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A520W Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A600FN Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G955F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A530F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G950U Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-J600G Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G935F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-N950F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-N950U Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G960U Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A520F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G930F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A320FL Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G965U Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-N950U Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G9650 Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A530F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-J330F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G950U Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G965F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-N950F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G611MT Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A720F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-G965U1 Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A260F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-J600F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-J810F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A750F Build/R16NW"},
			{"8.0.0", "SAMSUNG SM-A920F Build/R16NW"},

			{"7.1.2", "SAMSUNG SM-T555 Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-T550 Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J250M Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-T350 Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J510FN Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J510FN Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J330F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J530F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J730F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-N950F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-A530F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-A310F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J710F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-G925F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J730F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-T585 Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-G925I Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-A510M Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-G610M Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-A510F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J530F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J327T1 Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-G920F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-G570M Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J530G Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J710MN Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-G920F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-G950F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J730G Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J730G Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-G935F Build/N2G48H"},
			{"7.1.2", "SAMSUNG SM-J701MT Build/N2G48H"},

			{"7.1.1", "SAMSUNG SM-T555 Build/NMF26X"},
			{"7.1.1", "SAMSUNG SM-T550 Build/NMF26X"},
			{"7.1.1", "SAMSUNG SM-J250M Build/NMF26X"},
			{"7.1.1", "SAMSUNG SM-T350 Build/NMF26X"},
			{"7.1.1", "SAMSUNG SM-J510FN Build/NMF26X"},
			{"7.1.1", "SAMSUNG SM-J510FN Build/NMF26X"},
			{"7.1.1", "SAMSUNG SM-J330F Build/NMF26X"},
			{"7.1.1", "SAMSUNG SM-J530F Build/NMF26X"},
			{"7.1.1", "SAMSUNG SM-J730F Build/NMF26X"},
			{"7.1.1", "SAMSUNG SM-N950F Build/NMF26X"},
			{"7.1.1", "SAMSUNG SM-A530F Build/NMF26X"},

			{"7.0", "SAMSUNG SM-A310F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J710F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G925F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J730F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-T585 Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G925I Build/NRD90M"},
			{"7.0", "SAMSUNG SM-A510M Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G610M Build/NRD90M"},
			{"7.0", "SAMSUNG SM-A510F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J530F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J327T1 Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G920F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G570M Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J530G Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J710MN Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G920F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G950F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J730G Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J730G Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G935F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J701MT Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G920F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J701MT Build/NRD90M"},
			{"7.0", "SAMSUNG SM-A520F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G925I Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G570M Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G920F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J710MN Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J710MN Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J730G Build/NRD90M"},
			{"7.0", "SAMSUNG SM-A510F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-A510F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-A320FL Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J530G Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G955F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-J530F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-A520F Build/NRD90M"},
			{"7.0", "SAMSUNG SM-G930F Build/NRD90M"},
			{"6.0", "SAMSUNG SM-G935F Build/MMB29K"},
			{"6.0", "SAMSUNG SM-G900T Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-G935F Build/MMB29K"},
			{"6.0.1", "SAMSUNG SM-G900T Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-J500M Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-J500FN Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-G532M Build/MMB29T"},
			{"6.0.1", "SAMSUNG SM-G570M Build/MMB29K"},
			{"6.0.1", "SAMSUNG SM-G532MT Build/MMB29T"},
			{"6.0.1", "SAMSUNG SM-J120A Build/MMB29K"},
			{"6.0.1", "SAMSUNG SM-G532M Build/MMB29T"},
			{"6.0.1", "SAMSUNG SM-G532MT Build/MMB29T"},
			{"6.0.1", "SAMSUNG SM-G935F Build/MMB29K"},
			{"6.0.1", "SAMSUNG SM-J500M Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-A500FU Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-A310F Build/MMB29K"},
			{"6.0.1", "SAMSUNG SM-J510FN Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-G532M Build/MMB29T"},
			{"6.0.1", "SAMSUNG SM-G900F Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-N910F Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-J500FN Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-J500M Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-G532MT Build/MMB29T"},
			{"6.0.1", "SAMSUNG SM-G920I Build/MMB29K"},
			{"6.0.1", "SAMSUNG SM-J700M Build/MMB29K"},
			{"6.0.1", "SAMSUNG SM-J500M Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-G900F Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-G532MT Build/MMB29T"},
			{"6.0.1", "SAMSUNG SM-J710MN Build/MMB29K"},
			{"6.0.1", "SAMSUNG SM-G900F Build/MMB29M"},
			{"6.0.1", "SAMSUNG SM-G900F Build/MMB29M"},
			{"5.1.1", "SAMSUNG SM-T280 Build/LMY47V"},
			{"5.1.1", "SAMSUNG SM-G531H Build/LMY48B"},
			{"5.1.1", "SAMSUNG SM-G531H Build/LMY48B"},
			{"5.1.1", "SAMSUNG SM-G531F Build/LMY48B"},
			{"5.1.1", "SAMSUNG SM-G531BT Build/LMY48B"},
			{"5.1.1", "SAMSUNG SM-J120M Build/LMY47X"},
			{"5.1.1", "SAMSUNG SM-J200BT Build/LMY47X"},
			{"5.1.1", "SAMSUNG SM-G360T1 Build/LMY47X"},
			{"5.1.1", "SAMSUNG SM-J320M Build/LMY47V"},
			{"5.1.1", "SAMSUNG SM-G903F Build/LMY47X"},
			{"5.1.1", "SAMSUNG SM-J111M Build/LMY47V"},
			{"5.1.1", "SAMSUNG SM-J320F Build/LMY47V"},
			{"5.1.1", "SAMSUNG SM-G925F Build/LMY47X"},
			{"5.1.1", "SAMSUNG SM-G361F Build/LMY48B"},
			{"5.1.1", "SAMSUNG SM-J110M Build/LMY48B"},
			{"5.0.2", "SAMSUNG SM-G530M Build/LRX22G"},
			{"5.0.2", "SAMSUNG SM-G360BT Build/LRX22G"},
			{"5.0.2", "SAMSUNG SM-A500FU Build/LRX22G"},
			{"5.0.1", "SAMSUNG GT-I9515 Build/LRX22C"},
			{"5.0", "SAMSUNG SM-N9005 Build/LRX21V"},
			{"5.0", "SAMSUNG SM-G900F Build/LRX21T"},

			{"4.4.4", "SAMSUNG SM-G900F Build/LRX21T"}, //TODO
		});

		public static readonly PhoneBrand Apple = new PhoneBrand("Apple", new object[,] {
			{"iPhone", "iPhone"},
		});

		public static readonly PhoneBrand Huawei = new PhoneBrand("Huawei", new object[,] {
			{"9", "HMA-AL00 Build/HUAWEIHMA-AL00"},
			{"9", "VOG-L04 Build/HUAWEIVOG-L04"},
			{"9", "ELE-L09 Build/HUAWEIELE-L09"},
			{"9", "MAR-LX1A Build/HUAWEIMAR-LX1A"},

			{"8.1", "CLT-L29 Build/HUAWEICLT-L29"},
			{"8.1", "DRA-LX2 Build/HUAWEIDRA-LX2"},
			{"8.1", "DUA-LX2 Build/HUAWEIDUA-LX2"},
			{"8.0", "ALP-L09 Build/HUAWEIALP-L09"},

			{"7.0", "PRA-LX3 Build/HUAWEIPRA-LX3"},
			{"7.0", "WAS-TL10 Build/HUAWEIWAS-TL10"},
			{"7.0", "VTR-L09 Build/HUAWEIVTR-L09"},
			{"", "VTR-L29 Build/HUAWEIVTR-L29"},

			{"6.0", "MYA-L22 Build/HUAWEIMYA-L22"},
			{"6.0", "EVA-L09 Build/HUAWEIEVA-L09"},
			{"5.1.1", "HUAWEI MT7-TL10 Build/HuaweiMT7-TL10"},
			{"5.1.1", "LUA-L21 Build/HUAWEILUA-L21"},
			{"5.0.1", "ALE-L21 Build/HuaweiALE-L21"},
			{"5.0.2", "PLK-AL10 Build/HONORPLK-AL10"},
			{"4.4.4", "HUAWEI H891L Build/HuaweiH891L"},
		});

		public static readonly PhoneBrand Xiaomi = new PhoneBrand("Xiaomi", new object[,] {
			{"5.0", "HM 1SW Build/LTD768"},
			{"5.0.2", "Mi 4i Build/LRX22G"},
			{"4.4.4", "MI 3W Build/KTU84P"},
			{"4.4.4", "MI PAD Build/KTU84P"},
			{"4.4.4", "HM 1S Build/KTU84Q"},
			{"4.4.4", "HM NOTE 1LTE Build/KTU84P"},
		});

		public static readonly PhoneBrand Pixel = new PhoneBrand("Pixel", new object[,] {
			{"9", "Pixel Build/PQ1A.181205.002.A1"},
			{"8.1.0", "Pixel Build/OPP6.171019.012"},
			{"8.0.0", "Pixel Build/OPR3.170623.013"},
			{"7.1.2", "Pixel Build/NHG47O"},
			{"7.1.2", "Pixel Build/NHG47Q"},
			{"7.1.1", "Pixel Build/NOF26V"},
		});

		public static readonly PhoneBrand Lg = new PhoneBrand("LG", new object[,] {
			{"8.0", "LG-H850 Build/OPR1.170623.032"},
			{"8.0", "LG-H870 Build/OPR1.170623.032"},
			{"8.0", "LG-H87 Build/OPR1.170623.032"},
			{"7.1.1", "LG-M700 Build/NMF26X"},
			{"7.0", "LG-H850 Build/NRD90U"},
			{"7.0", "LG-H870 Build/NRD90U"},
			{"7.0", "LG-M150 Build/NRD90U"},
			{"7.0", "LGMS210 Build/NRD90U"},
			{"7.0", "LG-M250 Build/NRD90U"},
			{"7.0", "LG-H990DS Build/NRD90U"},
			{"6.0.1", "LG-H850 Build/MMB29M"},
			{"6.0.1", "LG-M150 Build/MXB48T"},
			{"6.0", "LG-H818 Build/MRA58K"},
			{"6.0", "LG-H815 Build/MRA58K"},
			{"6.0", "LG-D855 Build/MRA58K"},
			{"6.0", "LG-K350 Build/MRA58"},
			{"6.0", "LG-K430 Build/MRA58K"},
			{"5.1", "LG-H815 Build/LMY47D"},
			{"5.1", "LG-H818 Build/LMY47D"},
			{"5.1.1", "LG-D722 Build/LMY48Y"},
			{"5.0.2", "LG-D415 Build/LRX22G"},
			{"5.0.2", "LG-D620 Build/LRX22G"},
			{"5.0.2", "LG-D802 Build/LRX22G"},
			{"5.0.2", "LG-V410/V41020c Build/LRX22G"},
			{"5.0", "LG-D855 Build/LRX21R"},
		});

		public static readonly PhoneBrand Motorola = new PhoneBrand("Motorola", new object[,] {
			{"6.0.1", "XT1254"},
			{"6.0.1", "XT1254 Build/MCG2"},
			{"6.0", "XT1068 Build/MPB24.65-34-3"},
			{"5.1", "XT1025 Build/LPCS23.13-34.8-3"},
			{"5.1", "XT1022 Build/LPCS23.13-34.8-3"},
			{"5.1", "XT1528 Build/LPIS23.29-17.5-7"},
			{"5.1", "XT1528 Build/LPIS23.29-17.5-2"},
			{"5.1", "XT1033 Build/LPBS23.13-56-2"},
			{"5.1", "XT1254 Build/SU4T"},
			{"4.4.4", "XT1022 Build/KXC21.5-40"},
			{"4.4.4", "XT1080 Build/SU6-7"},
		});

		public static readonly PhoneBrand Htc = new PhoneBrand("HTC", new object[,] {
			{"8.0.0", "HTC 10 Build/OPR1.170623.027"},
			{"8.0.0", "HTC 10)"},
			{"8.0.0", "HTC U11 plus"},
			{"7.1.1", "HTC U11 Build/NMF26X"},
			{"7.0", "HTC 10 Build/NRD90M"},
			{"7.0", "HTC6535LVW Build/NRD90M"},
			{"6.0.1", "HTC_0P6B Build/MMB29M"},
			{"6.0.1", "HTC 10 Build/MMB29M"},
			{"6.0", "HTC_0P6B Build/MRA58K"},
			{"6.0", "HTC6525LVW Build/MRA58K"},
			{"6.0", "HTC6535LVW Build/MRA58K"},
			{"6.0", "HTC_M8x"},
			{"5.0.2", "HTC6500LVW 4G Build/LRX22G"},
			{"5.0.2", "HTC_M8x Build/LRX22G; wv"},
			{"5.0.2", "HTC_PN071 Build/LRX22G"},
			{"5.1.1", "HTC6535LVW Build/LMY47O"},
			{"5.0.1", "HTC_0P6B Build/LRX22C"},
			{"5.0.1", "HTC6525LVW Build/LRX22C"},
			{"4.4.4", "HTC_0P6B Build/KTU84P"},
			{"4.4.4", "HTC6525LVW Build/KTU84P"},
		});

    }
}
