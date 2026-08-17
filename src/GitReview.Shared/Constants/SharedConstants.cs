using System.Collections.Generic;

namespace GitReview.Shared.Constants
{
    public static class SharedConstants
    {
        public static readonly Dictionary<string, string[]> ModelsByProvider = new()
        {
            ["openrouter"] =
            [
                "poolside/laguna-s-2.1:free",
                "nvidia/nemotron-3-super:free",
                "cohere/north-mini-code:free",
                "deepseek/deepseek-r1:free"
            ],
            ["gemini"] =
            [
                "gemini-2.0-flash",
                "gemini-1.5-flash",
                "gemini-1.5-pro"
            ],
            ["deepseek"] =
            [
                "deepseek-chat",
                "deepseek-reasoner"
            ]
        };
    }
}
