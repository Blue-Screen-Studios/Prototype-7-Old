using Assembly.IBX.WebIO;

using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

using Unity.Services.CloudCode;
using Unity.Services.Core;

namespace Assembly.IBX.Accounts
{
    public class Account : MonoBehaviour
    {
        public static AccountInfo Info { get; private set; } = null;

        public static async Task<AccountInfo> GetPublicAccountInfo(string targetPlayerId)
        {
            if(UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }


            Dictionary<string, object> arguments = new Dictionary<string, object> { { "TargetPlayerId", targetPlayerId } };
            int rawFlags = await CloudCodeService.Instance.CallEndpointAsync<int>("GetPlayerFlags", arguments);

            bool[] flagBits = Parser.ParseFlags(rawFlags);

            AccountFlags flags = new AccountFlags
            {
                DEVELOPER = flagBits[0],
                DISCORD_MEMBER = flagBits[1],
                EARLY_ADOPTER = flagBits[2],
                EARLY_PROMOTER = flagBits[3],
                COMMUNITY_MOD = flagBits[4],
                THANK_YOU_BADGE = flagBits[5],

                ACC_API_ABUSE = flagBits[6],
                ACC_UNDERAGE = flagBits[7],
                ACC_SUSPEND_PREV = flagBits[8],
                ACC_TERMINATED = flagBits[9]
            };

            return new AccountInfo(flags);
        }
    }
}
