using NgUtil.Generics.Enums;
using NgUtil.Maths;

namespace NgHTTP.Browser {
    public sealed class BrowserBrand : ExtendedEnum<BrowserBrand> {

		public object[,] Versions { get; }


        private BrowserBrand(string name, object[,] versions) : base(name) {
            Versions = versions;
        }

        public string GetRandomVersion() {
			double marker = 0.0;
			int roll = MathUtil.Random(100);

			for (int i = 0; i < Versions.Length; i++) {
				string version = (string)Versions[i, 0];
				double share = (double)Versions[i, 1];

				if (roll > marker && roll <= (marker + share)) {
					return version;
				}
				marker += share;
			}
			return "71.0";
		}

		public static readonly BrowserBrand Safari = new BrowserBrand("Safari", new object[,] {
			{"13.0", 70.0},
			{"12.0", 20.0},
			{"11.0", 5.0},
			{"10.0", 5.0},
		});

		public static readonly BrowserBrand Chrome = new BrowserBrand("Chrome", new object[,] {
			{"79.0.3945.88", 27.0},
			{"78.0.3904.108", 2.0},
			{"77.0.3865.90", 2.0},
			{"76.0.3809.132", 2.0},
			{"75.0.3770.142", 4.0},
			{"75.0.3770.100", 1.0},
			{"74.0.3729.169", 18.0},
			{"74.0.3729.157", 2.0},
			{"74.0.3729.136", 1.0},
			{"74.0.3729.125", 1.0},
			{"74.0.3729.131", 1.0},
			{"74.0.3729.112", 1.0},
			{"73.0.3683.90", 11.0},
			{"72.0.3626.105", 6.5},
			{"72.0.3626.96", 1.0},
			{"72.0.3626.76", 0.5},
			{"71.0.3578.99", 5.0},
			{"71.0.3578.98", 1.0},
			{"71.0.3578.83", 1.0},
			{"70.0.3538.110", 4.0},
			{"70.0.3538.80", 1.0},
			{"70.0.3538.64", 1.0},
			{"69.0.3497.100", 3.0},
			{"69.0.3497.91", 1.0},
			{"69.0.3497.86", 1.0},
			{"69.0.3497.76", 0.5},
			{"68.0.3440.91", 0.5},
		});

		public static readonly BrowserBrand Firefox = new BrowserBrand("Firefox", new object[,] {
			{"71.0", 80.0},
			{"70.0", 1.0},
			{"69.0", 1.0},
			{"68.0", 1.0},
			{"67.0", 1.0},
			{"66.0", 1.0},
			{"55.0", 15.0},
		});

	}
}
