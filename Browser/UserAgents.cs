using NgUtil.Maths;

namespace NgHTTP.Browser{
    public static class UserAgents {

        private static readonly string[] DesktopUserAgents = {
			//Chrome OS
			"Mozilla/5.0 (X11; CrOS x86_64 11021.56.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.76 Safari/537.36",
			"Mozilla/5.0 (X11; CrOS x86_64 11021.81.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36",
			
			//Linux
			"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36",
			"Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:49.0) Gecko/20100101 Firefox/49.0",
			
			//Mac Os X
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_5) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.1.2 Safari/603.3.8",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_5) AppleWebKit/601.7.8 (KHTML, like Gecko) Version/9.1.3 Safari/537.86.7",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.11; rv:45.0) Gecko/20100101 Firefox/45.0",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_5) AppleWebKit/601.6.17 (KHTML, like Gecko) Version/9.1.1 Safari/601.6.17",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/11.1.2 Safari/605.1.15",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/603.2.5 (KHTML, like Gecko) Version/10.1.1 Safari/603.2.5",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/601.7.7 (KHTML, like Gecko) Version/9.1.2 Safari/601.7.7",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.1.2 Safari/603.3.8",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.13; rv:63.0) Gecko/20100101 Firefox/63.0",
			
			//Windows
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36",
			"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:54.0) Gecko/20100101 Firefox/54.0",
			"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393",
			"Mozilla/5.0 (Windows NT 5.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36",
			"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; WOW64; rv:52.0) Gecko/20100101 Firefox/52.0",
			"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0",
			"Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0",
			"Mozilla/5.0 (Windows NT 10.0; rv:63.0) Gecko/20100101 Firefox/63.0",
			"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0",
			"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:63.0) Gecko/20100101 Firefox/63.0",
			"Mozilla/5.0 (Windows NT 10.0; WOW64; rv:63.0) Gecko/20100101 Firefox/63.0",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/17.17134",
			"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393",
			"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063",
			"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299",
			"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/17.17134",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36",
		};

		private static readonly int[,] PhoneScreenResulutions = {
			{480, 854},
			{828, 1792},
			{1125, 2436},
			{1080, 1920},
			{750, 1334},
			{750, 1334},
			{1440, 2560},
			{1080, 2160},
			{1440, 2960}
		};

		private static readonly object[,] AndroidVersions = {
			//Version - Range start, Market share (6 May 2019)
			{"9", 3.0},
			{"8.1.0", 12.0},
			{"8.0.0", 21.0},
			{"7.1.2", 8.0},
			{"7.1.1", 2.0},
			{"7.0", 14.0},
			{"6.0.1", 19.5},
			{"6.0", 3.0},
			{"5.1.1", 10.0},
			{"5.0.2", 0.5},
			{"5.0.1", 0.5},
			{"5.0", 1.5},
			{"4.4.4", 5.0},
		};

		private static readonly object[,] IosVersions = {
			{"13_3", 89.5},
			{"12_4_4", 8.3},
			{"11_4_1", 1.4},
			{"10_3_3", 0.7},
			{"9_3_5", 0.1},
		};

		private static readonly object[,] PhoneBrandMarketShares = {
			{PhoneBrand.Samsung, 40.0},
			{PhoneBrand.Apple, 40.0},
			{PhoneBrand.Huawei, 7.0},
			{PhoneBrand.Xiaomi, 1.0},
			{PhoneBrand.Lg, 4.0},
			{PhoneBrand.Motorola, 2.0},
			{PhoneBrand.Htc, 3.0},
			{PhoneBrand.Pixel, 3.0},
		};

		private static readonly object[,] BROWSER_BRANDS = {
				{BrowserBrand.Chrome, 94.0},
				{BrowserBrand.Firefox, 6.0},
				//TODO opera, uc browser, samsung browser
		};
		public static string GetMobileUserAgent() {
			PhoneBrand brand = GetRandomPhoneBrand();

			if (brand == PhoneBrand.Apple) {
				return "Mozilla/5.0 (iPhone; CPU iPhone OS " + GetRandomIOSVersion()
					+ " like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/"
					+ BrowserBrand.Safari.GetRandomVersion() + " Mobile/15E148 Safari/604.1";
			}
			string osVersion = GetRandomAndroidVersion();
			BrowserBrand browser = GetRandomBrowserBrand();
			string browserVersion = browser.GetRandomVersion();

			if (browser == BrowserBrand.Firefox) {
				return "Mozilla/5.0 (Android " + osVersion + "; Mobile; rv:" + browserVersion + ") Gecko/" + browserVersion + " Firefox/" + browserVersion;
			}
			string modelName = brand.GetRandomModel(osVersion);
			return "Mozilla/5.0 (Linux; Android " + osVersion + "; " + modelName + " AppleWebKit/537.36 (KHTML, like Gecko) Chrome/" + browserVersion + " Mobile Safari/537.36";
		}
		public static string GetAppUserAgent() {
			PhoneBrand brand = GetRandomPhoneBrand(false);
			string osVersion = GetRandomAndroidVersion();
			string modelName = brand.GetRandomModel(osVersion);

			return "Linux; U; Android " + osVersion + "; " + modelName;
		}

		public static string GetDesktopUserAgent() {
			return DesktopUserAgents[MathUtil.Random(DesktopUserAgents.Length)];
		}

		public static string GetMixedUserAgent() {
			return MathUtil.Random(2) == 0 ? GetMobileUserAgent() : GetDesktopUserAgent();
		}

		public static BrowserBrand GetRandomBrowserBrand() {
			double marker = 0.0;
			int roll = MathUtil.Random(100);

			for (int i = 0; i < BROWSER_BRANDS.Length; i++) {
				BrowserBrand brand = (BrowserBrand)BROWSER_BRANDS[i, 0];
				double share = (double)BROWSER_BRANDS[i, 1];

				if (roll > marker && roll <= (marker + share)) {
					return brand;
				}
				marker += share;
			}
			return BrowserBrand.Chrome;
		}

		public static PhoneBrand GetRandomPhoneBrand() {
			return GetRandomPhoneBrand(true);
		}

		public static PhoneBrand GetRandomPhoneBrand(bool includeApple) {
			double marker = 0.0;
			int roll = MathUtil.Random(100);

			for (int i = 0; i < PhoneBrandMarketShares.Length; i++) {
				PhoneBrand brand = (PhoneBrand)PhoneBrandMarketShares[i, 0];
				double share = (double)PhoneBrandMarketShares[i, 1];

				if (roll > marker && roll <= (marker + share)) {
					return brand;
				}
				marker += share;
			}
			return PhoneBrand.Apple;
		}

		public static string GetRandomAndroidVersion() {
			double marker = 0.0;
			int roll = MathUtil.Random(100);

			for (int i = 0; i < AndroidVersions.Length; i++) {
				string version = (string)AndroidVersions[i, 0];
				double share = (double)AndroidVersions[i, 1];

				if (roll > marker && roll <= (marker + share)) {
					return version;
				}
				marker += share;
			}
			return "8.0"; //Current most common
		}

		public static string GetRandomIOSVersion() {
			double marker = 0.0;
			int roll = MathUtil.Random(100);

			for (int i = 0; i < IosVersions.Length; i++) {
				string version = (string)IosVersions[i, 0];
				double share = (double)IosVersions[i, 1];

				if (roll > marker && roll <= (marker + share)) {
					return version;
				}
				marker += share;
			}
			return "13.3"; //Most recent/common
		}

		public static int[] GetRandomPhoneScreenResolution() {
			return (int[])PhoneScreenResulutions.GetValue(MathUtil.Random(PhoneScreenResulutions.Length));
		}

	}
}
