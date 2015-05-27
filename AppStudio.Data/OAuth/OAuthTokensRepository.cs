using System.Collections.Generic;

namespace AppStudio.Data
{
    public static class OAuthTokensRepository
    {
        private static Dictionary<long, OAuthTokens> Tokens { get; set; }

        static OAuthTokensRepository()
        {
            Tokens = new Dictionary<long, OAuthTokens>();


            Tokens.Add(7515, new OAuthTokens
                            {
                                { "AppId", "867487173313805" },
                                { "AppSecret", "576e941013ded22cc1850c17d73e90e8" }
                            });

            Tokens.Add(8490, new OAuthTokens
                            {
                                { "ConsumerKey", "GPoWSqwAEiaHAKL7j3qMyzPsG" },
                                { "ConsumerSecret", "gdKAGA2hXWTas3vCaWLAvc9NtFeytibYsMqLJE81YyFeSRfAVQ" },
                                { "AccessToken", "143095598-pF764xGOM88qSiJg9JmKtYwsPW5viR6MeOpKKPGJ" },
                                { "AccessTokenSecret", "s4YTRRn3T15WSDVrO6DojdU56hjUS1HYD6H7z2kOggueF" }
                            });

            Tokens.Add(5362, new OAuthTokens
                            {
                                { "ApiKey", "AIzaSyA6VnHK4Opi8FY9BNFD8BpznxfgrAe8bcM" }
                            });

        }

        public static OAuthTokens GetTokens(long key)
        {
            if (Tokens.ContainsKey(key))
            {
                return Tokens[key];
            }
            return null;
        }

    }
}
